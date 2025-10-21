// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Physics
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using PressPlay.FFWD.Components;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.FFWD
{
  public static class Physics
  {
    public const int kDefaultRaycastLayers = -5;
    private static World _world;
    private static bool isPaused = false;
    private static GameObjectContactProcessor contactProcessor;
    public static int velocityIterations = 2;
    public static int positionIterations = 2;
    private static readonly List<Body> movingBodies = new List<Body>(50);
    private static readonly List<Body> rigidBodies = new List<Body>(50);

    public static World world
    {
      get
      {
        if (Physics._world == null)
          Physics.Initialize();
        return Physics._world;
      }
      internal set => Physics._world = value;
    }

    public static void Initialize() => Physics.Initialize(Vector2.zero);

    public static void Initialize(Vector2 gravity)
    {
      Settings.EnableDiagnostics = false;
      Settings.VelocityIterations = Physics.velocityIterations;
      Settings.PositionIterations = Physics.positionIterations;
      Settings.ContinuousPhysics = false;
      Physics.world = new World((Microsoft.Xna.Framework.Vector2) gravity);
      Physics.contactProcessor = new GameObjectContactProcessor();
      Physics.world.ContactManager.BeginContact = new BeginContactDelegate(Physics.contactProcessor.BeginContact);
      Physics.world.ContactManager.EndContact = new EndContactDelegate(Physics.contactProcessor.EndContact);
    }

    public static void TogglePause() => Physics.isPaused = !Physics.isPaused;

    public static void Update(float elapsedTime)
    {
      for (int index = Physics.movingBodies.Count - 1; index >= 0; --index)
      {
        Body movingBody = Physics.movingBodies[index];
        Component userData = movingBody.UserData;
        BodyType bodyType = movingBody.BodyType;
        if (bodyType == BodyType.Static)
        {
          Physics.movingBodies.RemoveAt(index);
        }
        else
        {
          if (userData.gameObject.active && bodyType == BodyType.Kinematic)
          {
            float angle = -MathHelper.ToRadians(userData.transform.eulerAngles.y);
            if (movingBody.Position != (Microsoft.Xna.Framework.Vector2) userData.transform.position || (double) movingBody.Rotation != (double) angle)
            {
              Microsoft.Xna.Framework.Vector2 position = (Microsoft.Xna.Framework.Vector2) userData.transform.position;
              movingBody.SetTransformIgnoreContacts(ref position, angle);
            }
          }
          if (userData.collider.allowTurnOff)
            movingBody.Enabled = userData.gameObject.active;
        }
      }
      Physics.world.Step(elapsedTime);
      for (int index = Physics.rigidBodies.Count - 1; index >= 0; --index)
      {
        Body rigidBody = Physics.rigidBodies[index];
        Component userData = rigidBody.UserData;
        FarseerPhysics.Common.Transform transform;
        rigidBody.GetTransform(out transform);
        userData.transform.SetPositionFromPhysics((Vector3) transform.Position, transform.Angle);
      }
      Physics.contactProcessor.Update();
    }

    public static Bounds BoundsFromAABB(AABB aabb, float width)
    {
      Vector2 center = (Vector2) aabb.Center;
      Vector2 extents = (Vector2) aabb.Extents;
      return new Bounds(new Vector3(center.x, 0.0f, center.y), new Vector3(extents.x * 2f, width, extents.y * 2f));
    }

    public static Body AddBody() => new Body(Physics.world);

    public static void AddBox(
      Body body,
      bool isTrigger,
      float width,
      float height,
      Vector2 position,
      float density)
    {
      if (Physics.world == null)
        throw new InvalidOperationException("You have to Initialize the Physics system before adding bodies");
      FixtureFactory.AttachRectangle(width, height, density, (Microsoft.Xna.Framework.Vector2) position, body).IsSensor = isTrigger;
    }

    public static void AddCircle(
      Body body,
      bool isTrigger,
      float radius,
      Vector2 position,
      float density)
    {
      if (Physics.world == null)
        throw new InvalidOperationException("You have to Initialize the Physics system before adding bodies");
      body.CreateFixture((Shape) new CircleShape(radius, density)
      {
        Position = (Microsoft.Xna.Framework.Vector2) position
      }).IsSensor = isTrigger;
    }

    public static void AddTriangle(Body body, bool isTrigger, Vertices vertices, float density)
    {
      if (Physics.world == null)
        throw new InvalidOperationException("You have to Initialize the Physics system before adding bodies");
      PolygonShape polygonShape = new PolygonShape(vertices, density);
      body.CreateFixture((Shape) polygonShape).IsSensor = isTrigger;
    }

    public static void AddMesh(Body body, bool isTrigger, List<Microsoft.Xna.Framework.Vector2[]> tris, float density)
    {
      if (Physics.world == null)
        throw new InvalidOperationException("You have to Initialize the Physics system before adding bodies");
      for (int index = 0; index < tris.Count<Microsoft.Xna.Framework.Vector2[]>(); ++index)
      {
        Vertices vertices = new Vertices(tris.ElementAt<Microsoft.Xna.Framework.Vector2[]>(index));
        try
        {
          PolygonShape polygonShape = new PolygonShape(vertices, density);
          body.CreateFixture((Shape) polygonShape).IsSensor = isTrigger;
        }
        catch (Exception ex)
        {
          Debug.Log((object) (body.UserData.ToString() + ". Collider triangle is broken: " + (object) vertices[0] + "; " + (object) vertices[1] + "; " + (object) vertices[2] + ": " + ex.Message));
        }
      }
    }

    public static void AddMesh(Body body, bool isTrigger, List<Vertices> tris, float density)
    {
      if (Physics.world == null)
        throw new InvalidOperationException("You have to Initialize the Physics system before adding bodies");
      List<Fixture> fixtureList = FixtureFactory.AttachCompoundPolygon(tris, density, body);
      for (int index = 0; index < fixtureList.Count; ++index)
        fixtureList[index].IsSensor = isTrigger;
    }

    public static bool Raycast(Vector2 origin, Vector2 direction)
    {
      return Physics.Raycast(origin, direction, float.PositiveInfinity, -5);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, float distance)
    {
      return Physics.Raycast(origin, direction, distance, -5);
    }

    public static bool Raycast(Vector2 origin, Vector2 direction, float distance, int layerMask)
    {
      RaycastHelper.SetValues(distance, true, layerMask);
      Vector2 point2 = origin + direction * distance;
      if (point2 == origin)
        return false;
      try
      {
        Physics.world.RayCast((RayCastCallback) null, (Microsoft.Xna.Framework.Vector2) origin, (Microsoft.Xna.Framework.Vector2) point2);
      }
      catch (InvalidOperationException ex)
      {
        Debug.Log((object) "RAYCAST THREW InvalidOperationException");
        return false;
      }
      return RaycastHelper.HitCount > 0;
    }

    public static bool Raycast(Vector3 origin, Vector3 direction)
    {
      return Physics.Raycast(origin, direction, float.PositiveInfinity, -5);
    }

    public static bool Raycast(Vector3 origin, Vector3 direction, float distance)
    {
      return Physics.Raycast(origin, direction, distance, -5);
    }

    public static bool Raycast(Vector3 origin, Vector3 direction, float distance, int layerMask)
    {
      return Physics.Raycast((Vector2) origin, (Vector2) direction, distance, layerMask);
    }

    public static bool Raycast(
      Vector2 origin,
      Vector2 direction,
      out RaycastHit hitInfo,
      float distance,
      int layerMask)
    {
      RaycastHelper.SetValues(distance, true, layerMask);
      Vector2 point2 = origin + direction * distance;
      if (point2 == origin)
      {
        hitInfo = new RaycastHit();
        return false;
      }
      try
      {
        Physics.world.RayCast((RayCastCallback) null, (Microsoft.Xna.Framework.Vector2) origin, (Microsoft.Xna.Framework.Vector2) point2);
        hitInfo = RaycastHelper.ClosestHit();
      }
      catch (InvalidOperationException ex)
      {
        hitInfo = new RaycastHit();
        Debug.Log((object) "RAYCAST THREW InvalidOperationException");
        return false;
      }
      return RaycastHelper.HitCount > 0;
    }

    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo)
    {
      return Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, -5);
    }

    public static bool Raycast(
      Vector3 origin,
      Vector3 direction,
      out RaycastHit hitInfo,
      float distance)
    {
      return Physics.Raycast(origin, direction, out hitInfo, distance, -5);
    }

    public static bool Raycast(
      Vector3 origin,
      Vector3 direction,
      out RaycastHit hitInfo,
      float distance,
      int layerMask)
    {
      return Physics.Raycast((Vector2) origin, (Vector2) direction, out hitInfo, distance, layerMask);
    }

    public static bool Raycast(Ray ray)
    {
      return Physics.Raycast(ray.origin, ray.direction, float.PositiveInfinity, -5);
    }

    public static bool Raycast(Ray ray, float distance)
    {
      return Physics.Raycast(ray.origin, ray.direction, distance, -5);
    }

    public static bool Raycast(Ray ray, float distance, int layerMask)
    {
      return Physics.Raycast(ray.origin, ray.direction, distance, layerMask);
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo)
    {
      return Physics.Raycast(ray.origin, ray.direction, out hitInfo, float.PositiveInfinity, -5);
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance)
    {
      return Physics.Raycast(ray.origin, ray.direction, out hitInfo, distance, -5);
    }

    public static bool Raycast(Ray ray, out RaycastHit hitInfo, float distance, int layerMask)
    {
      return Physics.Raycast(ray.origin, ray.direction, out hitInfo, distance, layerMask);
    }

    public static RaycastHit[] RaycastAll(Vector2 origin, Vector2 direction)
    {
      return Physics.RaycastAll(origin, direction, float.PositiveInfinity, -5);
    }

    public static RaycastHit[] RaycastAll(Vector2 origin, Vector2 direction, float distance)
    {
      return Physics.RaycastAll(origin, direction, distance, -5);
    }

    public static RaycastHit[] RaycastAll(
      Vector2 origin,
      Vector2 direction,
      float distance,
      int layerMask)
    {
      RaycastHelper.SetValues(distance, false, layerMask);
      Vector2 point2 = origin + direction * distance;
      if (point2 == origin)
        return new RaycastHit[0];
      Physics.world.RayCast((RayCastCallback) null, (Microsoft.Xna.Framework.Vector2) origin, (Microsoft.Xna.Framework.Vector2) point2);
      return RaycastHelper.Hits;
    }

    public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction)
    {
      return Physics.RaycastAll((Vector2) origin, (Vector2) direction, float.PositiveInfinity, -5);
    }

    public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float distance)
    {
      return Physics.RaycastAll((Vector2) origin, (Vector2) direction, distance, -5);
    }

    public static RaycastHit[] RaycastAll(Ray ray, float distance, int layerMask)
    {
      return Physics.RaycastAll((Vector2) ray.origin, (Vector2) ray.direction, distance, layerMask);
    }

    internal static bool Raycast(Body body, Ray ray, out RaycastHit hitInfo, float distance)
    {
      RayCastInput input = new RayCastInput()
      {
        Point1 = (Microsoft.Xna.Framework.Vector2) ray.origin,
        Point2 = (Microsoft.Xna.Framework.Vector2) (ray.origin + ray.direction),
        MaxFraction = distance
      };
      hitInfo = new RaycastHit() { body = body };
      for (int index = 0; index < body.FixtureList.Count; ++index)
      {
        RayCastOutput output;
        if (body.FixtureList[index].RayCast(out output, ref input, 0))
        {
          hitInfo.normal = (Vector3) output.Normal;
          hitInfo.distance = output.Fraction;
          hitInfo.point = ray.GetPoint(output.Fraction);
          return true;
        }
      }
      return false;
    }

    public static bool Pointcast(Vector2 point) => Physics.Pointcast(point, -5);

    public static bool Pointcast(Vector2 point, int layerMask)
    {
      RaycastHelper.SetValues(float.MaxValue, true, layerMask);
      AABB aabb = new AABB((Microsoft.Xna.Framework.Vector2) new Vector2(point.x - float.Epsilon, point.y - float.Epsilon), (Microsoft.Xna.Framework.Vector2) new Vector2(point.x + float.Epsilon, point.y + float.Epsilon));
      Physics.world.QueryAABB(new Func<Fixture, bool>(RaycastHelper.pointCastCallback), ref aabb);
      return RaycastHelper.HitCount > 0;
    }

    public static bool Pointcast(Vector2 point, out RaycastHit hitInfo)
    {
      return Physics.Pointcast(point, out hitInfo, -5);
    }

    public static bool Pointcast(Vector2 point, out RaycastHit hitInfo, int layerMask)
    {
      RaycastHelper.SetValues(float.MaxValue, true, layerMask);
      AABB aabb = new AABB((Microsoft.Xna.Framework.Vector2) new Vector2(point.x - float.Epsilon, point.y - float.Epsilon), (Microsoft.Xna.Framework.Vector2) new Vector2(point.x + float.Epsilon, point.y + float.Epsilon));
      Physics.world.QueryAABB(new Func<Fixture, bool>(RaycastHelper.pointCastCallback), ref aabb);
      if (RaycastHelper.HitCount > 0)
      {
        hitInfo = RaycastHelper.ClosestHit();
        return true;
      }
      hitInfo = new RaycastHit();
      return false;
    }

    public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask)
    {
      Vector2 point2 = (Vector2) end;
      Vector2 point1 = (Vector2) start;
      RaycastHelper.SetValues((point1 - point2).magnitude, true, layerMask);
      if (point2 == point1)
      {
        hitInfo = new RaycastHit();
        return false;
      }
      Physics.world.RayCast((RayCastCallback) null, (Microsoft.Xna.Framework.Vector2) point1, (Microsoft.Xna.Framework.Vector2) point2);
      hitInfo = RaycastHelper.ClosestHit();
      return RaycastHelper.HitCount > 0;
    }

    public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo)
    {
      return Physics.Linecast(start, end, out hitInfo, -5);
    }

    public static bool CheckCapsule(Vector3 start, Vector3 end, float radius)
    {
      return Physics.CheckCapsule(start, end, radius, (LayerMask)(-5));
    }

    public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, LayerMask layermask)
    {
      Vector3 normalized = (end - start).normalized;
      Vector3 vector3 = new Vector3(normalized.z, 0.0f, -normalized.x);
      Ray ray1 = new Ray(start, normalized);
      Ray ray2 = new Ray(start + vector3 * radius + normalized * radius, normalized);
      Ray ray3 = new Ray(start - vector3 * radius + normalized * radius, normalized);
      float magnitude = (end - start).magnitude;
      float distance = magnitude - radius * 2f;
      return Physics.Raycast(ray1, magnitude, (int) layermask) || Physics.Raycast(ray2, distance, (int) layermask) || Physics.Raycast(ray3, distance, (int) layermask);
    }

    public static void IgnoreCollision(Collider collider1, Collider collider2)
    {
      Physics.IgnoreCollision(collider1, collider2, true);
    }

    public static void IgnoreCollision(Collider collider1, Collider collider2, bool ignore)
    {
      if (ignore)
      {
        collider1.connectedBody.IgnoreCollisionWith(collider2.connectedBody);
        Physics.RemoveStays(collider1, collider2);
      }
      else
        collider1.connectedBody.RestoreCollisionWith(collider2.connectedBody);
    }

    private static void RemoveStays(Collider collider1, Collider collider2)
    {
      Physics.contactProcessor.RemoveStay(collider1, collider2);
    }

    internal static void RemoveStays(Collider collider)
    {
      Physics.contactProcessor.ResetStays(collider);
    }

    internal static void AddMovingBody(Body body)
    {
      if (Physics.movingBodies.Contains(body) || body.UserData == null)
        return;
      Physics.movingBodies.Add(body);
    }

    internal static void AddRigidBody(Body body)
    {
      if (Physics.rigidBodies.Contains(body) || body.UserData == null)
        return;
      Physics.rigidBodies.Add(body);
    }

    internal static void RemoveBody(Body body)
    {
      if (body.UserData == null)
        return;
      Physics.world.RemoveBody(body);
      body.UserData = (Component) null;
      if (Physics.movingBodies.Contains(body))
        Physics.movingBodies.Remove(body);
      if (!Physics.rigidBodies.Contains(body))
        return;
      Physics.rigidBodies.Remove(body);
    }
  }
}
