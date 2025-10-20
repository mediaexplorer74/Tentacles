// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.DebugViewXNA
// Assembly: Tentacles, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 94733B2D-6956-40B2-A474-EF03B0110429
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\Tentacles.dll

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.Tentacles
{
  public class DebugViewXNA : DebugView, IDisposable
  {
    private const int MaxContactPoints = 2048;
    public const int CircleSegments = 16;
    private PrimitiveBatch _primitiveBatch;
    private SpriteBatch _batch;
    private SpriteFont _font;
    private GraphicsDevice _device;
    private Vector2[] _tempVertices = new Vector2[FarseerPhysics.Settings.MaxPolygonVertices];
    private List<DebugViewXNA.StringData> _stringData;
    private Matrix _localProjection;
    private Matrix _localView;
    public Color DefaultShapeColor = new Color(0.9f, 0.7f, 0.7f);
    public Color InactiveShapeColor = new Color(0.1f, 0.1f, 0.9f);
    public Color KinematicShapeColor = new Color(0.5f, 0.5f, 0.9f);
    public Color SleepingShapeColor = new Color(0.6f, 0.6f, 0.6f);
    public Color StaticShapeColor = new Color(0.5f, 0.9f, 0.5f);
    public Color TextColor = Color.White;
    private int _pointCount;
    private DebugViewXNA.ContactPoint[] _points = new DebugViewXNA.ContactPoint[2048];
    public Vector2 DebugPanelPosition = new Vector2(40f, 100f);
    private int _max;
    private int _avg;
    private int _min;
    public bool AdaptiveLimits = true;
    public int ValuesToGraph = 500;
    public int MinimumValue;
    public int MaximumValue = 1000;
    private List<float> _graphValues = new List<float>();
    public Rectangle PerformancePanelBounds = new Rectangle(250, 100, 200, 100);
    private Vector2[] _background = new Vector2[4];
    public bool Enabled = true;

    public DebugViewXNA(World world)
      : base(world)
    {
      world.ContactManager.PreSolve += new PreSolveDelegate(this.PreSolve);
      this.AppendFlags(DebugViewFlags.Shape);
      this.AppendFlags(DebugViewFlags.Controllers);
      this.AppendFlags(DebugViewFlags.ContactPoints);
    }

    public void BeginCustomDraw(ref Matrix projection, ref Matrix view)
    {
      this._primitiveBatch.Begin(ref projection, ref view);
    }

    public void EndCustomDraw() => this._primitiveBatch.End();

    public void Dispose()
    {
      this.World.ContactManager.PreSolve -= new PreSolveDelegate(this.PreSolve);
    }

    private void PreSolve(Contact contact, ref Manifold oldManifold)
    {
      if ((this.Flags & DebugViewFlags.ContactPoints) != DebugViewFlags.ContactPoints)
        return;
      Manifold manifold = contact.Manifold;
      if (manifold.PointCount == 0)
        return;
      Fixture fixtureA = contact.FixtureA;
      FixedArray2<PointState> state2;
      FarseerPhysics.Collision.Collision.GetPointStates(out FixedArray2<PointState> _, out state2, ref oldManifold, ref manifold);
      Vector2 normal;
      FixedArray2<Vector2> points;
      contact.GetWorldManifold(out normal, out points);
      for (int index = 0; index < manifold.PointCount && this._pointCount < 2048; ++index)
      {
        if (fixtureA == null)
          this._points[index] = new DebugViewXNA.ContactPoint();
        this._points[this._pointCount] = this._points[this._pointCount] with
        {
          Position = points[index],
          Normal = normal,
          State = state2[index]
        };
        ++this._pointCount;
      }
    }

    private void DrawDebugData()
    {
      if ((this.Flags & DebugViewFlags.Shape) == DebugViewFlags.Shape)
      {
        foreach (Body body in this.World.BodyList)
        {
          Transform transform;
          body.GetTransform(out transform);
          foreach (Fixture fixture in body.FixtureList)
          {
            if (!body.Enabled)
              this.DrawShape(fixture, transform, this.InactiveShapeColor);
            else if (body.BodyType == BodyType.Static)
              this.DrawShape(fixture, transform, this.StaticShapeColor);
            else if (body.BodyType == BodyType.Kinematic)
              this.DrawShape(fixture, transform, this.KinematicShapeColor);
            else if (!body.Awake)
              this.DrawShape(fixture, transform, this.SleepingShapeColor);
            else
              this.DrawShape(fixture, transform, this.DefaultShapeColor);
          }
        }
      }
      if ((this.Flags & DebugViewFlags.ContactPoints) == DebugViewFlags.ContactPoints)
      {
        for (int index = 0; index < this._pointCount; ++index)
        {
          DebugViewXNA.ContactPoint point = this._points[index];
          if (point.State == PointState.Add)
            this.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.95f, 0.3f));
          else if (point.State == PointState.Persist)
            this.DrawPoint(point.Position, 0.1f, new Color(0.3f, 0.3f, 0.95f));
          if ((this.Flags & DebugViewFlags.ContactNormals) == DebugViewFlags.ContactNormals)
          {
            Vector2 position = point.Position;
            Vector2 end = position + 0.3f * point.Normal;
            this.DrawSegment(position, end, new Color(0.4f, 0.9f, 0.4f));
          }
        }
        this._pointCount = 0;
      }
      if ((this.Flags & DebugViewFlags.PolygonPoints) == DebugViewFlags.PolygonPoints)
      {
        foreach (Body body in this.World.BodyList)
        {
          foreach (Fixture fixture in body.FixtureList)
          {
            if (fixture.Shape is PolygonShape shape)
            {
              Transform transform;
              body.GetTransform(out transform);
              for (int index = 0; index < shape.Vertices.Count; ++index)
                this.DrawPoint(MathUtils.Multiply(ref transform, shape.Vertices[index]), 0.1f, Color.Red);
            }
          }
        }
      }
      if ((this.Flags & DebugViewFlags.Joint) == DebugViewFlags.Joint)
      {
        foreach (Joint joint in this.World.JointList)
          this.DrawJoint(joint);
      }
      if ((this.Flags & DebugViewFlags.Pair) == DebugViewFlags.Pair)
      {
        Color color = new Color(0.3f, 0.9f, 0.9f);
        for (int index = 0; index < this.World.ContactManager.ContactList.Count; ++index)
        {
          Contact contact = this.World.ContactManager.ContactList[index];
          Fixture fixtureA = contact.FixtureA;
          Fixture fixtureB = contact.FixtureB;
          AABB aabb1;
          fixtureA.GetAABB(out aabb1, 0);
          AABB aabb2;
          fixtureB.GetAABB(out aabb2, 0);
          this.DrawSegment(aabb1.Center, aabb2.Center, color);
        }
      }
      if ((this.Flags & DebugViewFlags.AABB) == DebugViewFlags.AABB)
      {
        Color color = new Color(0.9f, 0.3f, 0.9f);
        IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
        foreach (Body body in this.World.BodyList)
        {
          if (body.Enabled)
          {
            foreach (Fixture fixture in body.FixtureList)
            {
              for (int index = 0; index < fixture.ProxyCount; ++index)
              {
                FixtureProxy proxy = fixture.Proxies[index];
                AABB aabb;
                broadPhase.GetFatAABB(proxy.ProxyId, out aabb);
                this.DrawAABB(ref aabb, color);
              }
            }
          }
        }
      }
      if ((this.Flags & DebugViewFlags.CenterOfMass) == DebugViewFlags.CenterOfMass)
      {
        foreach (Body body in this.World.BodyList)
        {
          Transform transform;
          body.GetTransform(out transform);
          transform.Position = body.WorldCenter;
          this.DrawTransform(ref transform);
        }
      }
      if ((this.Flags & DebugViewFlags.Controllers) == DebugViewFlags.Controllers)
      {
        for (int index = 0; index < this.World.ControllerList.Count; ++index)
        {
          if (this.World.ControllerList[index] is BuoyancyController controller)
          {
            AABB container = controller.Container;
            this.DrawAABB(ref container, Color.LightBlue);
          }
        }
      }
      if ((this.Flags & DebugViewFlags.DebugPanel) != DebugViewFlags.DebugPanel)
        return;
      this.DrawDebugPanel();
    }

    private void DrawPerformanceGraph()
    {
      this._graphValues.Add(this.World.UpdateTime);
      if (this._graphValues.Count > this.ValuesToGraph + 1)
        this._graphValues.RemoveAt(0);
      float x = (float) this.PerformancePanelBounds.X;
      float num1 = (float) this.PerformancePanelBounds.Width / (float) this.ValuesToGraph;
      float num2 = (float) this.PerformancePanelBounds.Bottom - (float) this.PerformancePanelBounds.Top;
      if (this._graphValues.Count > 2)
      {
        this._max = (int) this._graphValues.Max();
        this._avg = (int) this._graphValues.Average();
        this._min = (int) this._graphValues.Min();
        if (this.AdaptiveLimits)
        {
          this.MaximumValue = this._max;
          this.MinimumValue = 0;
        }
        for (int index = this._graphValues.Count - 1; index > 0; --index)
        {
          float num3 = (float) this.PerformancePanelBounds.Bottom - this._graphValues[index] / (float) (this.MaximumValue - this.MinimumValue) * num2;
          float num4 = (float) this.PerformancePanelBounds.Bottom - this._graphValues[index - 1] / (float) (this.MaximumValue - this.MinimumValue) * num2;
          this.DrawSegment(new Vector2(MathHelper.Clamp(x, (float) this.PerformancePanelBounds.Left, (float) this.PerformancePanelBounds.Right), MathHelper.Clamp(num3, (float) this.PerformancePanelBounds.Top, (float) this.PerformancePanelBounds.Bottom)), new Vector2(MathHelper.Clamp(x + num1, (float) this.PerformancePanelBounds.Left, (float) this.PerformancePanelBounds.Right), MathHelper.Clamp(num4, (float) this.PerformancePanelBounds.Top, (float) this.PerformancePanelBounds.Bottom)), Color.LightGreen);
          x += num1;
        }
      }
      this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Top, "Max: " + (object) this._max);
      this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Center.Y - 7, "Avg: " + (object) this._avg);
      this.DrawString(this.PerformancePanelBounds.Right + 10, this.PerformancePanelBounds.Bottom - 15, "Min: " + (object) this._min);
      this._background[0] = new Vector2((float) this.PerformancePanelBounds.X, (float) this.PerformancePanelBounds.Y);
      this._background[1] = new Vector2((float) this.PerformancePanelBounds.X, (float) (this.PerformancePanelBounds.Y + this.PerformancePanelBounds.Height));
      this._background[2] = new Vector2((float) (this.PerformancePanelBounds.X + this.PerformancePanelBounds.Width), (float) (this.PerformancePanelBounds.Y + this.PerformancePanelBounds.Height));
      this._background[3] = new Vector2((float) (this.PerformancePanelBounds.X + this.PerformancePanelBounds.Width), (float) this.PerformancePanelBounds.Y);
      this.DrawSolidPolygon(this._background, 4, Color.DarkGray, true);
    }

    private void DrawDebugPanel()
    {
      int num = 0;
      for (int index = 0; index < this.World.BodyList.Count; ++index)
        num += this.World.BodyList[index].FixtureList.Count;
      int x = (int) this.DebugPanelPosition.X;
      int y = (int) this.DebugPanelPosition.Y;
      this.DrawString(x, y, "Objects:\n- Bodies: " + (object) this.World.BodyList.Count + "\n- Fixtures: " + (object) num + "\n- Contacts: " + (object) this.World.ContactList.Count + "\n- Joints: " + (object) this.World.JointList.Count + "\n- Controllers: " + (object) this.World.ControllerList.Count + "\n- Proxies: " + (object) this.World.ProxyCount);
      this.DrawString(x + 110, y, "Update time:\n- Body: " + (object) this.World.SolveUpdateTime + "\n- Contact: " + (object) this.World.ContactsUpdateTime + "\n- CCD: " + (object) this.World.ContinuousPhysicsTime + "\n- Joint: " + (object) this.World.Island.JointUpdateTime + "\n- Controller: " + (object) this.World.ControllersUpdateTime + "\n- Total: " + (object) this.World.UpdateTime);
    }

    public void DrawAABB(ref AABB aabb, Color color)
    {
      this.DrawPolygon(new Vector2[4]
      {
        new Vector2(aabb.LowerBound.X, aabb.LowerBound.Y),
        new Vector2(aabb.UpperBound.X, aabb.LowerBound.Y),
        new Vector2(aabb.UpperBound.X, aabb.UpperBound.Y),
        new Vector2(aabb.LowerBound.X, aabb.UpperBound.Y)
      }, 4, color);
    }

    private void DrawJoint(Joint joint)
    {
      if (!joint.Enabled)
        return;
      Body bodyA = joint.BodyA;
      Body bodyB = joint.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Vector2 vector2 = Vector2.Zero;
      if (!joint.IsFixedType())
      {
        Transform transform2;
        bodyB.GetTransform(out transform2);
        vector2 = transform2.Position;
      }
      Vector2 worldAnchorA = joint.WorldAnchorA;
      Vector2 worldAnchorB = joint.WorldAnchorB;
      Vector2 position = transform1.Position;
      Color color = new Color(0.5f, 0.8f, 0.8f);
      switch (joint.JointType)
      {
        case JointType.Revolute:
          this.DrawSegment(worldAnchorB, worldAnchorA, color);
          this.DrawSolidCircle(worldAnchorB, 0.1f, Vector2.Zero, Color.Red);
          this.DrawSolidCircle(worldAnchorA, 0.1f, Vector2.Zero, Color.Blue);
          break;
        case JointType.Distance:
          this.DrawSegment(worldAnchorA, worldAnchorB, color);
          break;
        case JointType.Pulley:
          PulleyJoint pulleyJoint = (PulleyJoint) joint;
          Vector2 groundAnchorA = pulleyJoint.GroundAnchorA;
          Vector2 groundAnchorB = pulleyJoint.GroundAnchorB;
          this.DrawSegment(groundAnchorA, worldAnchorA, color);
          this.DrawSegment(groundAnchorB, worldAnchorB, color);
          this.DrawSegment(groundAnchorA, groundAnchorB, color);
          break;
        case JointType.Gear:
          this.DrawSegment(position, vector2, color);
          break;
        case JointType.FixedMouse:
          this.DrawPoint(worldAnchorA, 0.5f, new Color(0.0f, 1f, 0.0f));
          this.DrawSegment(worldAnchorA, worldAnchorB, new Color(0.8f, 0.8f, 0.8f));
          break;
        case JointType.FixedRevolute:
          this.DrawSegment(position, worldAnchorA, color);
          this.DrawSolidCircle(worldAnchorA, 0.1f, Vector2.Zero, Color.Pink);
          break;
        case JointType.FixedDistance:
          this.DrawSegment(position, worldAnchorA, color);
          this.DrawSegment(worldAnchorA, worldAnchorB, color);
          break;
        case JointType.FixedLine:
          this.DrawSegment(position, worldAnchorA, color);
          this.DrawSegment(worldAnchorA, worldAnchorB, color);
          break;
        case JointType.FixedPrismatic:
          this.DrawSegment(position, worldAnchorA, color);
          this.DrawSegment(worldAnchorA, worldAnchorB, color);
          break;
        case JointType.FixedAngle:
          break;
        default:
          this.DrawSegment(position, worldAnchorA, color);
          this.DrawSegment(worldAnchorA, worldAnchorB, color);
          this.DrawSegment(vector2, worldAnchorB, color);
          break;
      }
    }

    public void DrawShape(Fixture fixture, Transform xf, Color color)
    {
      switch (fixture.ShapeType)
      {
        case ShapeType.Circle:
          CircleShape shape1 = (CircleShape) fixture.Shape;
          this.DrawSolidCircle(MathUtils.Multiply(ref xf, shape1.Position), shape1.Radius, xf.R.Col1, color);
          break;
        case ShapeType.Edge:
          EdgeShape shape2 = (EdgeShape) fixture.Shape;
          this.DrawSegment(MathUtils.Multiply(ref xf, shape2.Vertex1), MathUtils.Multiply(ref xf, shape2.Vertex2), color);
          break;
        case ShapeType.Polygon:
          PolygonShape shape3 = (PolygonShape) fixture.Shape;
          int count1 = shape3.Vertices.Count;
          for (int index = 0; index < count1; ++index)
            this._tempVertices[index] = MathUtils.Multiply(ref xf, shape3.Vertices[index]);
          this.DrawSolidPolygon(this._tempVertices, count1, color);
          break;
        case ShapeType.Loop:
          LoopShape shape4 = (LoopShape) fixture.Shape;
          int count2 = shape4.Vertices.Count;
          Vector2 vector2 = MathUtils.Multiply(ref xf, shape4.Vertices[count2 - 1]);
          this.DrawCircle(vector2, 0.05f, color);
          for (int index = 0; index < count2; ++index)
          {
            Vector2 end = MathUtils.Multiply(ref xf, shape4.Vertices[index]);
            this.DrawSegment(vector2, end, color);
            vector2 = end;
          }
          break;
      }
    }

    public override void DrawPolygon(
      Vector2[] vertices,
      int count,
      float red,
      float green,
      float blue)
    {
      this.DrawPolygon(vertices, count, new Color(red, green, blue));
    }

    public void DrawPolygon(Vector2[] vertices, int count, Color color)
    {
      if (!this._primitiveBatch.IsReady())
        throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
      for (int index = 0; index < count - 1; ++index)
      {
        this._primitiveBatch.AddVertex(vertices[index], color, PrimitiveType.LineList);
        this._primitiveBatch.AddVertex(vertices[index + 1], color, PrimitiveType.LineList);
      }
      this._primitiveBatch.AddVertex(vertices[count - 1], color, PrimitiveType.LineList);
      this._primitiveBatch.AddVertex(vertices[0], color, PrimitiveType.LineList);
    }

    public override void DrawSolidPolygon(
      Vector2[] vertices,
      int count,
      float red,
      float green,
      float blue)
    {
      this.DrawSolidPolygon(vertices, count, new Color(red, green, blue), true);
    }

    public void DrawSolidPolygon(Vector2[] vertices, int count, Color color)
    {
      this.DrawSolidPolygon(vertices, count, color, true);
    }

    public void DrawSolidPolygon(Vector2[] vertices, int count, Color color, bool outline)
    {
      if (!this._primitiveBatch.IsReady())
        throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
      if (count == 2)
      {
        this.DrawPolygon(vertices, count, color);
      }
      else
      {
        Color color1 = color * (outline ? 0.5f : 1f);
        for (int index = 1; index < count - 1; ++index)
        {
          this._primitiveBatch.AddVertex(vertices[0], color1, PrimitiveType.TriangleList);
          this._primitiveBatch.AddVertex(vertices[index], color1, PrimitiveType.TriangleList);
          this._primitiveBatch.AddVertex(vertices[index + 1], color1, PrimitiveType.TriangleList);
        }
        if (!outline)
          return;
        this.DrawPolygon(vertices, count, color);
      }
    }

    public override void DrawCircle(
      Vector2 center,
      float radius,
      float red,
      float green,
      float blue)
    {
      this.DrawCircle(center, radius, new Color(red, green, blue));
    }

    public void DrawCircle(Vector2 center, float radius, Color color)
    {
      if (!this._primitiveBatch.IsReady())
        throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
      double num = 0.0;
      for (int index = 0; index < 16; ++index)
      {
        Vector2 vertex1 = center + radius * new Vector2((float) Math.Cos(num), (float) Math.Sin(num));
        Vector2 vertex2 = center + radius * new Vector2((float) Math.Cos(num + Math.PI / 8.0), (float) Math.Sin(num + Math.PI / 8.0));
        this._primitiveBatch.AddVertex(vertex1, color, PrimitiveType.LineList);
        this._primitiveBatch.AddVertex(vertex2, color, PrimitiveType.LineList);
        num += Math.PI / 8.0;
      }
    }

    public override void DrawSolidCircle(
      Vector2 center,
      float radius,
      Vector2 axis,
      float red,
      float green,
      float blue)
    {
      this.DrawSolidCircle(center, radius, axis, new Color(red, green, blue));
    }

    public void DrawSolidCircle(Vector2 center, float radius, Vector2 axis, Color color)
    {
      if (!this._primitiveBatch.IsReady())
        throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
      double num1 = 0.0;
      Color color1 = color * 0.5f;
      Vector2 vertex1 = center + radius * new Vector2((float) Math.Cos(num1), (float) Math.Sin(num1));
      double num2 = num1 + Math.PI / 8.0;
      for (int index = 1; index < 15; ++index)
      {
        Vector2 vertex2 = center + radius * new Vector2((float) Math.Cos(num2), (float) Math.Sin(num2));
        Vector2 vertex3 = center + radius * new Vector2((float) Math.Cos(num2 + Math.PI / 8.0), (float) Math.Sin(num2 + Math.PI / 8.0));
        this._primitiveBatch.AddVertex(vertex1, color1, PrimitiveType.TriangleList);
        this._primitiveBatch.AddVertex(vertex2, color1, PrimitiveType.TriangleList);
        this._primitiveBatch.AddVertex(vertex3, color1, PrimitiveType.TriangleList);
        num2 += Math.PI / 8.0;
      }
      this.DrawCircle(center, radius, color);
      this.DrawSegment(center, center + axis * radius, color);
    }

    public override void DrawSegment(
      Vector2 start,
      Vector2 end,
      float red,
      float green,
      float blue)
    {
      this.DrawSegment(start, end, new Color(red, green, blue));
    }

    public void DrawSegment(Vector2 start, Vector2 end, Color color)
    {
      if (!this._primitiveBatch.IsReady())
        throw new InvalidOperationException("BeginCustomDraw must be called before drawing anything.");
      this._primitiveBatch.AddVertex(start, color, PrimitiveType.LineList);
      this._primitiveBatch.AddVertex(end, color, PrimitiveType.LineList);
    }

    public override void DrawTransform(ref Transform transform)
    {
      Vector2 position = transform.Position;
      Vector2 end1 = position + 0.4f * transform.R.Col1;
      this.DrawSegment(position, end1, Color.Red);
      Vector2 end2 = position + 0.4f * transform.R.Col2;
      this.DrawSegment(position, end2, Color.Green);
    }

    public void DrawPoint(Vector2 p, float size, Color color)
    {
      Vector2[] vertices = new Vector2[4];
      float num = size / 2f;
      vertices[0] = p + new Vector2(-num, -num);
      vertices[1] = p + new Vector2(num, -num);
      vertices[2] = p + new Vector2(num, num);
      vertices[3] = p + new Vector2(-num, num);
      this.DrawSolidPolygon(vertices, 4, color, true);
    }

    public void DrawString(int x, int y, string s, params object[] args)
    {
      this._stringData.Add(new DebugViewXNA.StringData(x, y, s, args, this.TextColor));
    }

    public void DrawArrow(
      Vector2 start,
      Vector2 end,
      float length,
      float width,
      bool drawStartIndicator,
      Color color)
    {
      this.DrawSegment(start, end, color);
      float x = width / 2f;
      Vector2 vector2 = start - end;
      vector2.Normalize();
      Matrix rotationZ = Matrix.CreateRotationZ((float) Math.Atan2((double) vector2.X, -(double) vector2.Y));
      Matrix translation1 = Matrix.CreateTranslation(end.X, end.Y, 0.0f);
      Vector2[] vector2Array1 = new Vector2[3]
      {
        new Vector2(0.0f, 0.0f),
        new Vector2(-x, -length),
        new Vector2(x, -length)
      };
      Vector2.Transform(vector2Array1, ref rotationZ, vector2Array1);
      Vector2.Transform(vector2Array1, ref translation1, vector2Array1);
      this.DrawSolidPolygon(vector2Array1, 3, color, false);
      if (!drawStartIndicator)
        return;
      Matrix translation2 = Matrix.CreateTranslation(start.X, start.Y, 0.0f);
      Vector2[] vector2Array2 = new Vector2[4]
      {
        new Vector2(-x, length / 4f),
        new Vector2(x, length / 4f),
        new Vector2(x, 0.0f),
        new Vector2(-x, 0.0f)
      };
      Vector2.Transform(vector2Array2, ref rotationZ, vector2Array2);
      Vector2.Transform(vector2Array2, ref translation2, vector2Array2);
      this.DrawSolidPolygon(vector2Array2, 4, color, false);
    }

    public void RenderDebugData(ref Matrix projection, ref Matrix view)
    {
      if (!this.Enabled || this.Flags == (DebugViewFlags) 0)
        return;
      this._device.RasterizerState = RasterizerState.CullNone;
      this._device.DepthStencilState = DepthStencilState.Default;
      this._primitiveBatch.Begin(ref projection, ref view);
      this.DrawDebugData();
      this._primitiveBatch.End();
      if ((this.Flags & DebugViewFlags.PerformanceGraph) == DebugViewFlags.PerformanceGraph)
      {
        this._primitiveBatch.Begin(ref this._localProjection, ref this._localView);
        this.DrawPerformanceGraph();
        this._primitiveBatch.End();
      }
      this._batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      for (int index = 0; index < this._stringData.Count; ++index)
      {
        this._batch.DrawString(this._font, string.Format(this._stringData[index].S, this._stringData[index].Args), new Vector2((float) this._stringData[index].X + 1f, (float) this._stringData[index].Y + 1f), Color.Black);
        this._batch.DrawString(this._font, string.Format(this._stringData[index].S, this._stringData[index].Args), new Vector2((float) this._stringData[index].X, (float) this._stringData[index].Y), this._stringData[index].Color);
      }
      this._batch.End();
      this._stringData.Clear();
    }

    public void RenderDebugData(ref Matrix projection)
    {
      if (!this.Enabled)
        return;
      Matrix identity = Matrix.Identity;
      this.RenderDebugData(ref projection, ref identity);
    }

    public void LoadContent(GraphicsDevice device, ContentManager content)
    {
      this._device = device;
      this._batch = new SpriteBatch(this._device);
      this._primitiveBatch = new PrimitiveBatch(this._device, 1000);
      this._font = content.Load<SpriteFont>("TestFont");
      this._stringData = new List<DebugViewXNA.StringData>();
      this._localProjection = Matrix.CreateOrthographicOffCenter(0.0f, (float) this._device.Viewport.Width, (float) this._device.Viewport.Height, 0.0f, 0.0f, 1f);
      this._localView = Matrix.Identity;
    }

    private struct ContactPoint
    {
      public Vector2 Normal;
      public Vector2 Position;
      public PointState State;
    }

    private struct StringData(int x, int y, string s, object[] args, Color color)
    {
      public object[] Args = args;
      public Color Color = color;
      public string S = s;
      public int X = x;
      public int Y = y;
    }
  }
}
