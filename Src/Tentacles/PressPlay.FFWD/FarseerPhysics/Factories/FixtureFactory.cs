// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Factories.FixtureFactory
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Factories
{
  public static class FixtureFactory
  {
    public static Fixture AttachEdge(Vector2 start, Vector2 end, Body body)
    {
      return FixtureFactory.AttachEdge(start, end, body, (object) null);
    }

    public static Fixture AttachEdge(Vector2 start, Vector2 end, Body body, object userData)
    {
      EdgeShape edgeShape = new EdgeShape(start, end);
      return body.CreateFixture((Shape) edgeShape, userData);
    }

    public static Fixture AttachLoopShape(Vertices vertices, Body body)
    {
      return FixtureFactory.AttachLoopShape(vertices, body, (object) null);
    }

    public static Fixture AttachLoopShape(Vertices vertices, Body body, object userData)
    {
      LoopShape loopShape = new LoopShape(vertices);
      return body.CreateFixture((Shape) loopShape, userData);
    }

    public static Fixture AttachRectangle(
      float width,
      float height,
      float density,
      Vector2 offset,
      Body body,
      object userData)
    {
      Vertices rectangle = PolygonTools.CreateRectangle(width / 2f, height / 2f);
      rectangle.Translate(ref offset);
      PolygonShape polygonShape = new PolygonShape(rectangle, density);
      return body.CreateFixture((Shape) polygonShape, userData);
    }

    public static Fixture AttachRectangle(
      float width,
      float height,
      float density,
      Vector2 offset,
      Body body)
    {
      return FixtureFactory.AttachRectangle(width, height, density, offset, body, (object) null);
    }

    public static Fixture AttachCircle(float radius, float density, Body body)
    {
      return FixtureFactory.AttachCircle(radius, density, body, (object) null);
    }

    public static Fixture AttachCircle(float radius, float density, Body body, object userData)
    {
      CircleShape circleShape = (double) radius > 0.0 ? new CircleShape(radius, density) : throw new ArgumentOutOfRangeException(nameof (radius), "Radius must be more than 0 meters");
      return body.CreateFixture((Shape) circleShape, userData);
    }

    public static Fixture AttachCircle(float radius, float density, Body body, Vector2 offset)
    {
      return FixtureFactory.AttachCircle(radius, density, body, offset, (object) null);
    }

    public static Fixture AttachCircle(
      float radius,
      float density,
      Body body,
      Vector2 offset,
      object userData)
    {
      if ((double) radius <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (radius), "Radius must be more than 0 meters");
      return body.CreateFixture((Shape) new CircleShape(radius, density)
      {
        Position = offset
      }, userData);
    }

    public static Fixture AttachPolygon(Vertices vertices, float density, Body body)
    {
      return FixtureFactory.AttachPolygon(vertices, density, body, (object) null);
    }

    public static Fixture AttachPolygon(
      Vertices vertices,
      float density,
      Body body,
      object userData)
    {
      PolygonShape polygonShape = vertices.Count > 1 ? new PolygonShape(vertices, density) : throw new ArgumentOutOfRangeException(nameof (vertices), "Too few points to be a polygon");
      return body.CreateFixture((Shape) polygonShape, userData);
    }

    public static Fixture AttachEllipse(
      float xRadius,
      float yRadius,
      int edges,
      float density,
      Body body)
    {
      return FixtureFactory.AttachEllipse(xRadius, yRadius, edges, density, body, (object) null);
    }

    public static Fixture AttachEllipse(
      float xRadius,
      float yRadius,
      int edges,
      float density,
      Body body,
      object userData)
    {
      if ((double) xRadius <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (xRadius), "X-radius must be more than 0");
      if ((double) yRadius <= 0.0)
        throw new ArgumentOutOfRangeException(nameof (yRadius), "Y-radius must be more than 0");
      PolygonShape polygonShape = new PolygonShape(PolygonTools.CreateEllipse(xRadius, yRadius, edges), density);
      return body.CreateFixture((Shape) polygonShape, userData);
    }

    public static List<Fixture> AttachCompoundPolygon(
      List<Vertices> list,
      float density,
      Body body)
    {
      return FixtureFactory.AttachCompoundPolygon(list, density, body, (object) null);
    }

    public static List<Fixture> AttachCompoundPolygon(
      List<Vertices> list,
      float density,
      Body body,
      object userData)
    {
      List<Fixture> fixtureList = new List<Fixture>(list.Count);
      foreach (Vertices vertices in list)
      {
        if (vertices.Count == 2)
        {
          EdgeShape edgeShape = new EdgeShape(vertices[0], vertices[1]);
          fixtureList.Add(body.CreateFixture((Shape) edgeShape, userData));
        }
        else
        {
          PolygonShape polygonShape = new PolygonShape(vertices, density);
          fixtureList.Add(body.CreateFixture((Shape) polygonShape, userData));
        }
      }
      return fixtureList;
    }

    public static List<Fixture> AttachLineArc(
      float radians,
      int sides,
      float radius,
      Vector2 position,
      float angle,
      bool closed,
      Body body)
    {
      Vertices arc = PolygonTools.CreateArc(radians, sides, radius);
      arc.Rotate((float) ((3.1415927410125732 - (double) radians) / 2.0) + angle);
      arc.Translate(ref position);
      List<Fixture> fixtureList = new List<Fixture>(arc.Count);
      if (closed)
        fixtureList.Add(FixtureFactory.AttachLoopShape(arc, body));
      for (int index = 1; index < arc.Count; ++index)
        fixtureList.Add(FixtureFactory.AttachEdge(arc[index], arc[index - 1], body));
      return fixtureList;
    }

    public static List<Fixture> AttachSolidArc(
      float density,
      float radians,
      int sides,
      float radius,
      Vector2 position,
      float angle,
      Body body)
    {
      Vertices arc = PolygonTools.CreateArc(radians, sides, radius);
      arc.Rotate((float) ((3.1415927410125732 - (double) radians) / 2.0) + angle);
      arc.Translate(ref position);
      arc.Add(arc[0]);
      return FixtureFactory.AttachCompoundPolygon(EarclipDecomposer.ConvexPartition(arc), density, body);
    }
  }
}
