// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Factories.PathManager
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Factories
{
  public static class PathManager
  {
    public static void ConvertPathToEdges(Path path, Body body, int subdivisions)
    {
      Vertices vertices = path.GetVertices(subdivisions);
      if (path.Closed)
      {
        LoopShape loopShape = new LoopShape(vertices);
        body.CreateFixture((Shape) loopShape);
      }
      else
      {
        for (int index = 1; index < vertices.Count; ++index)
          body.CreateFixture((Shape) new EdgeShape(vertices[index], vertices[index - 1]));
      }
    }

    public static void ConvertPathToPolygon(Path path, Body body, float density, int subdivisions)
    {
      if (!path.Closed)
        throw new Exception("The path must be closed to convert to a polygon.");
      foreach (Vertices vertices in EarclipDecomposer.ConvexPartition(new Vertices((IList<Vector2>) path.GetVertices(subdivisions))))
        body.CreateFixture((Shape) new PolygonShape(vertices, density));
    }

    public static List<Body> EvenlyDistributeShapesAlongPath(
      World world,
      Path path,
      IEnumerable<Shape> shapes,
      BodyType type,
      int copies,
      object userData)
    {
      List<Vector3> vector3List = path.SubdivideEvenly(copies);
      List<Body> bodyList = new List<Body>();
      for (int index = 0; index < vector3List.Count; ++index)
      {
        Body body = new Body(world);
        body.BodyType = type;
        body.Position = new Vector2(vector3List[index].X, vector3List[index].Y);
        body.Rotation = vector3List[index].Z;
        foreach (Shape shape in shapes)
          body.CreateFixture(shape, userData);
        bodyList.Add(body);
      }
      return bodyList;
    }

    public static List<Body> EvenlyDistributeShapesAlongPath(
      World world,
      Path path,
      IEnumerable<Shape> shapes,
      BodyType type,
      int copies)
    {
      return PathManager.EvenlyDistributeShapesAlongPath(world, path, shapes, type, copies, (object) null);
    }

    public static List<Body> EvenlyDistributeShapesAlongPath(
      World world,
      Path path,
      Shape shape,
      BodyType type,
      int copies,
      object userData)
    {
      return PathManager.EvenlyDistributeShapesAlongPath(world, path, (IEnumerable<Shape>) new List<Shape>(1)
      {
        shape
      }, type, copies, userData);
    }

    public static List<Body> EvenlyDistributeShapesAlongPath(
      World world,
      Path path,
      Shape shape,
      BodyType type,
      int copies)
    {
      return PathManager.EvenlyDistributeShapesAlongPath(world, path, shape, type, copies, (object) null);
    }

    public static void MoveBodyOnPath(
      Path path,
      Body body,
      float time,
      float strength,
      float timeStep)
    {
      Vector2 position = path.GetPosition(time);
      Vector2 vector2 = (body.Position - position) / timeStep * strength;
      body.LinearVelocity = -vector2;
    }

    public static List<RevoluteJoint> AttachBodiesWithRevoluteJoint(
      World world,
      List<Body> bodies,
      Vector2 localAnchorA,
      Vector2 localAnchorB,
      bool connectFirstAndLast,
      bool collideConnected)
    {
      List<RevoluteJoint> revoluteJointList = new List<RevoluteJoint>(bodies.Count + 1);
      for (int index = 1; index < bodies.Count; ++index)
      {
        RevoluteJoint revoluteJoint = new RevoluteJoint(bodies[index], bodies[index - 1], localAnchorA, localAnchorB);
        revoluteJoint.CollideConnected = collideConnected;
        world.AddJoint((Joint) revoluteJoint);
        revoluteJointList.Add(revoluteJoint);
      }
      if (connectFirstAndLast)
      {
        RevoluteJoint revoluteJoint = new RevoluteJoint(bodies[0], bodies[bodies.Count - 1], localAnchorA, localAnchorB);
        revoluteJoint.CollideConnected = collideConnected;
        world.AddJoint((Joint) revoluteJoint);
        revoluteJointList.Add(revoluteJoint);
      }
      return revoluteJointList;
    }

    public static List<SliderJoint> AttachBodiesWithSliderJoint(
      World world,
      List<Body> bodies,
      Vector2 localAnchorA,
      Vector2 localAnchorB,
      bool connectFirstAndLast,
      bool collideConnected,
      float minLength,
      float maxLength)
    {
      List<SliderJoint> sliderJointList = new List<SliderJoint>(bodies.Count + 1);
      for (int index = 1; index < bodies.Count; ++index)
      {
        SliderJoint sliderJoint = new SliderJoint(bodies[index], bodies[index - 1], localAnchorA, localAnchorB, minLength, maxLength);
        sliderJoint.CollideConnected = collideConnected;
        world.AddJoint((Joint) sliderJoint);
        sliderJointList.Add(sliderJoint);
      }
      if (connectFirstAndLast)
      {
        SliderJoint sliderJoint = new SliderJoint(bodies[0], bodies[bodies.Count - 1], localAnchorA, localAnchorB, minLength, maxLength);
        sliderJoint.CollideConnected = collideConnected;
        world.AddJoint((Joint) sliderJoint);
        sliderJointList.Add(sliderJoint);
      }
      return sliderJointList;
    }

    public enum LinkType
    {
      Revolute,
      Slider,
    }
  }
}
