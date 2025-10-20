// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Factories.BodyFactory
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using PressPlay.FFWD;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Factories
{
  public static class BodyFactory
  {
    public static Body CreateBody(World world) => BodyFactory.CreateBody(world, (Component) null);

    public static Body CreateBody(World world, Component userData) => new Body(world, userData);

    public static Body CreateBody(World world, Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateBody(world, position, (Component) null);
    }

    public static Body CreateBody(World world, Microsoft.Xna.Framework.Vector2 position, Component userData)
    {
      Body body = BodyFactory.CreateBody(world, userData);
      body.Position = position;
      return body;
    }

    public static Body CreateEdge(World world, Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end)
    {
      return BodyFactory.CreateEdge(world, start, end, (Component) null);
    }

    public static Body CreateEdge(World world, Microsoft.Xna.Framework.Vector2 start, Microsoft.Xna.Framework.Vector2 end, Component userData)
    {
      Body body = BodyFactory.CreateBody(world);
      FixtureFactory.AttachEdge(start, end, body, (object) userData);
      return body;
    }

    public static Body CreateLoopShape(World world, Vertices vertices)
    {
      return BodyFactory.CreateLoopShape(world, vertices, (Component) null);
    }

    public static Body CreateLoopShape(World world, Vertices vertices, Component userData)
    {
      return BodyFactory.CreateLoopShape(world, vertices, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreateLoopShape(World world, Vertices vertices, Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateLoopShape(world, vertices, position, (Component) null);
    }

    public static Body CreateLoopShape(
      World world,
      Vertices vertices,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Body body = BodyFactory.CreateBody(world, position);
      FixtureFactory.AttachLoopShape(vertices, body, (object) userData);
      return body;
    }

    public static Body CreateRectangle(World world, float width, float height, float density)
    {
      return BodyFactory.CreateRectangle(world, width, height, density, (Component) null);
    }

    public static Body CreateRectangle(
      World world,
      float width,
      float height,
      float density,
      Component userData)
    {
      return BodyFactory.CreateRectangle(world, width, height, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreateRectangle(
      World world,
      float width,
      float height,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateRectangle(world, width, height, density, position, (Component) null);
    }

    public static Body CreateRectangle(
      World world,
      float width,
      float height,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      if ((double) width <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (width), "Width must be more than 0 meters");
      if ((double) height <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (height), "Height must be more than 0 meters");
      Body body = BodyFactory.CreateBody(world, position);
      PolygonShape polygonShape = new PolygonShape(PolygonTools.CreateRectangle(width / 2f, height / 2f), density);
      body.CreateFixture((Shape) polygonShape, (object) userData);
      return body;
    }

    public static Body CreateCircle(World world, float radius, float density)
    {
      return BodyFactory.CreateCircle(world, radius, density, (Component) null);
    }

    public static Body CreateCircle(World world, float radius, float density, Component userData)
    {
      return BodyFactory.CreateCircle(world, radius, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreateCircle(World world, float radius, float density, Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateCircle(world, radius, density, position, (Component) null);
    }

    public static Body CreateCircle(
      World world,
      float radius,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Body body = BodyFactory.CreateBody(world, position);
      FixtureFactory.AttachCircle(radius, density, body, (object) userData);
      return body;
    }

    public static Body CreateEllipse(
      World world,
      float xRadius,
      float yRadius,
      int edges,
      float density)
    {
      return BodyFactory.CreateEllipse(world, xRadius, yRadius, edges, density, (Component) null);
    }

    public static Body CreateEllipse(
      World world,
      float xRadius,
      float yRadius,
      int edges,
      float density,
      Component userData)
    {
      return BodyFactory.CreateEllipse(world, xRadius, yRadius, edges, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreateEllipse(
      World world,
      float xRadius,
      float yRadius,
      int edges,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateEllipse(world, xRadius, yRadius, edges, density, position, (Component) null);
    }

    public static Body CreateEllipse(
      World world,
      float xRadius,
      float yRadius,
      int edges,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Body body = BodyFactory.CreateBody(world, position);
      FixtureFactory.AttachEllipse(xRadius, yRadius, edges, density, body, (object) userData);
      return body;
    }

    public static Body CreatePolygon(World world, Vertices vertices, float density)
    {
      return BodyFactory.CreatePolygon(world, vertices, density, (Component) null);
    }

    public static Body CreatePolygon(
      World world,
      Vertices vertices,
      float density,
      Component userData)
    {
      return BodyFactory.CreatePolygon(world, vertices, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreatePolygon(
      World world,
      Vertices vertices,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreatePolygon(world, vertices, density, position, (Component) null);
    }

    public static Body CreatePolygon(
      World world,
      Vertices vertices,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Body body = BodyFactory.CreateBody(world, position);
      FixtureFactory.AttachPolygon(vertices, density, body, (object) userData);
      return body;
    }

    public static Body CreateCompoundPolygon(World world, List<Vertices> list, float density)
    {
      return BodyFactory.CreateCompoundPolygon(world, list, density, (Component) null);
    }

    public static Body CreateCompoundPolygon(
      World world,
      List<Vertices> list,
      float density,
      Component userData)
    {
      return BodyFactory.CreateCompoundPolygon(world, list, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static Body CreateCompoundPolygon(
      World world,
      List<Vertices> list,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateCompoundPolygon(world, list, density, position, (Component) null);
    }

    public static Body CreateCompoundPolygon(
      World world,
      List<Vertices> list,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Body body = BodyFactory.CreateBody(world, position);
      FixtureFactory.AttachCompoundPolygon(list, density, body, (object) userData);
      return body;
    }

    public static Body CreateGear(
      World world,
      float radius,
      int numberOfTeeth,
      float tipPercentage,
      float toothHeight,
      float density)
    {
      return BodyFactory.CreateGear(world, radius, numberOfTeeth, tipPercentage, toothHeight, density, (Component) null);
    }

    public static Body CreateGear(
      World world,
      float radius,
      int numberOfTeeth,
      float tipPercentage,
      float toothHeight,
      float density,
      Component userData)
    {
      Vertices gear = PolygonTools.CreateGear(radius, numberOfTeeth, tipPercentage, toothHeight);
      if (gear.IsConvex())
        return BodyFactory.CreatePolygon(world, gear, density, userData);
      List<Vertices> list = EarclipDecomposer.ConvexPartition(gear);
      return BodyFactory.CreateCompoundPolygon(world, list, density, userData);
    }

    public static Body CreateCapsule(
      World world,
      float height,
      float topRadius,
      int topEdges,
      float bottomRadius,
      int bottomEdges,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Vertices capsule = PolygonTools.CreateCapsule(height, topRadius, topEdges, bottomRadius, bottomEdges);
      if (capsule.Count >= Settings.MaxPolygonVertices)
      {
        List<Vertices> list = EarclipDecomposer.ConvexPartition(capsule);
        Body compoundPolygon = BodyFactory.CreateCompoundPolygon(world, list, density, userData);
        compoundPolygon.Position = position;
        return compoundPolygon;
      }
      Body polygon = BodyFactory.CreatePolygon(world, capsule, density, userData);
      polygon.Position = position;
      return polygon;
    }

    public static Body CreateCapsule(
      World world,
      float height,
      float topRadius,
      int topEdges,
      float bottomRadius,
      int bottomEdges,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateCapsule(world, height, topRadius, topEdges, bottomRadius, bottomEdges, density, position, (Component) null);
    }

    public static Body CreateCapsule(World world, float height, float endRadius, float density)
    {
      return BodyFactory.CreateCapsule(world, height, endRadius, density, (Component) null);
    }

    public static Body CreateCapsule(
      World world,
      float height,
      float endRadius,
      float density,
      Component userData)
    {
      Vertices rectangle = PolygonTools.CreateRectangle(endRadius, height / 2f);
      Body compoundPolygon = BodyFactory.CreateCompoundPolygon(world, new List<Vertices>()
      {
        rectangle
      }, density, userData);
      compoundPolygon.CreateFixture((Shape) new CircleShape(endRadius, density)
      {
        Position = new Microsoft.Xna.Framework.Vector2(0.0f, height / 2f)
      }, (object) userData);
      compoundPolygon.CreateFixture((Shape) new CircleShape(endRadius, density)
      {
        Position = new Microsoft.Xna.Framework.Vector2(0.0f, (float) -((double) height / 2.0))
      }, (object) userData);
      return compoundPolygon;
    }

    public static Body CreateRoundedRectangle(
      World world,
      float width,
      float height,
      float xRadius,
      float yRadius,
      int segments,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      Vertices roundedRectangle = PolygonTools.CreateRoundedRectangle(width, height, xRadius, yRadius, segments);
      if (roundedRectangle.Count < Settings.MaxPolygonVertices)
        return BodyFactory.CreatePolygon(world, roundedRectangle, density);
      List<Vertices> list = EarclipDecomposer.ConvexPartition(roundedRectangle);
      Body compoundPolygon = BodyFactory.CreateCompoundPolygon(world, list, density, userData);
      compoundPolygon.Position = position;
      return compoundPolygon;
    }

    public static Body CreateRoundedRectangle(
      World world,
      float width,
      float height,
      float xRadius,
      float yRadius,
      int segments,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, position, (Component) null);
    }

    public static Body CreateRoundedRectangle(
      World world,
      float width,
      float height,
      float xRadius,
      float yRadius,
      int segments,
      float density)
    {
      return BodyFactory.CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, (Component) null);
    }

    public static Body CreateRoundedRectangle(
      World world,
      float width,
      float height,
      float xRadius,
      float yRadius,
      int segments,
      float density,
      Component userData)
    {
      return BodyFactory.CreateRoundedRectangle(world, width, height, xRadius, yRadius, segments, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static BreakableBody CreateBreakableBody(World world, Vertices vertices, float density)
    {
      return BodyFactory.CreateBreakableBody(world, vertices, density, (Component) null);
    }

    public static BreakableBody CreateBreakableBody(
      World world,
      Vertices vertices,
      float density,
      Component userData)
    {
      return BodyFactory.CreateBreakableBody(world, vertices, density, Microsoft.Xna.Framework.Vector2.Zero, userData);
    }

    public static BreakableBody CreateBreakableBody(
      World world,
      Vertices vertices,
      float density,
      Microsoft.Xna.Framework.Vector2 position,
      Component userData)
    {
      BreakableBody breakableBody = new BreakableBody((IEnumerable<Vertices>) EarclipDecomposer.ConvexPartition(vertices), world, density, (object) userData);
      breakableBody.MainBody.Position = position;
      world.AddBreakableBody(breakableBody);
      return breakableBody;
    }

    public static BreakableBody CreateBreakableBody(
      World world,
      Vertices vertices,
      float density,
      Microsoft.Xna.Framework.Vector2 position)
    {
      return BodyFactory.CreateBreakableBody(world, vertices, density, position, (Component) null);
    }

    public static Body CreateLineArc(
      World world,
      float radians,
      int sides,
      float radius,
      Microsoft.Xna.Framework.Vector2 position,
      float angle,
      bool closed)
    {
      Body body = BodyFactory.CreateBody(world);
      FixtureFactory.AttachLineArc(radians, sides, radius, position, angle, closed, body);
      return body;
    }

    public static Body CreateSolidArc(
      World world,
      float density,
      float radians,
      int sides,
      float radius,
      Microsoft.Xna.Framework.Vector2 position,
      float angle)
    {
      Body body = BodyFactory.CreateBody(world);
      FixtureFactory.AttachSolidArc(density, radians, sides, radius, position, angle, body);
      return body;
    }
  }
}
