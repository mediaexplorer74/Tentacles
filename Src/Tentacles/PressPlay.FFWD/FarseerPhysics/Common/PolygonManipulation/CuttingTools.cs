// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PolygonManipulation.CuttingTools
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.PolygonManipulation
{
  public static class CuttingTools
  {
    public static void SplitShape(
      Fixture fixture,
      Vector2 entryPoint,
      Vector2 exitPoint,
      float splitSize,
      out Vertices first,
      out Vertices second)
    {
      Vector2 localPoint1 = fixture.Body.GetLocalPoint(ref entryPoint);
      Vector2 localPoint2 = fixture.Body.GetLocalPoint(ref exitPoint);
      if (!(fixture.Shape is PolygonShape shape))
      {
        first = new Vertices();
        second = new Vertices();
      }
      else
      {
        Vertices vertices = new Vertices((IList<Vector2>) shape.Vertices);
        Vertices[] verticesArray = new Vertices[2];
        for (int index = 0; index < verticesArray.Length; ++index)
          verticesArray[index] = new Vertices(vertices.Count);
        int[] numArray = new int[2]{ -1, -1 };
        int index1 = -1;
        for (int index2 = 0; index2 < vertices.Count; ++index2)
        {
          int index3 = (double) Vector2.Dot(MathUtils.Cross(localPoint2 - localPoint1, 1f), vertices[index2] - localPoint1) <= 1.1920928955078125E-07 ? 1 : 0;
          if (index1 != index3)
          {
            if (index1 == 0)
            {
              numArray[0] = verticesArray[index1].Count;
              verticesArray[index1].Add(localPoint2);
              verticesArray[index1].Add(localPoint1);
            }
            if (index1 == 1)
            {
              numArray[index1] = verticesArray[index1].Count;
              verticesArray[index1].Add(localPoint1);
              verticesArray[index1].Add(localPoint2);
            }
          }
          verticesArray[index3].Add(vertices[index2]);
          index1 = index3;
        }
        if (numArray[0] == -1)
        {
          numArray[0] = verticesArray[0].Count;
          verticesArray[0].Add(localPoint2);
          verticesArray[0].Add(localPoint1);
        }
        if (numArray[1] == -1)
        {
          numArray[1] = verticesArray[1].Count;
          verticesArray[1].Add(localPoint1);
          verticesArray[1].Add(localPoint2);
        }
        for (int index4 = 0; index4 < 2; ++index4)
        {
          Vector2 vector2_1 = numArray[index4] <= 0 ? verticesArray[index4][verticesArray[index4].Count - 1] - verticesArray[index4][0] : verticesArray[index4][numArray[index4] - 1] - verticesArray[index4][numArray[index4]];
          vector2_1.Normalize();
          verticesArray[index4][numArray[index4]] += splitSize * vector2_1;
          Vector2 vector2_2 = numArray[index4] >= verticesArray[index4].Count - 2 ? verticesArray[index4][0] - verticesArray[index4][verticesArray[index4].Count - 1] : verticesArray[index4][numArray[index4] + 2] - verticesArray[index4][numArray[index4] + 1];
          vector2_2.Normalize();
          verticesArray[index4][numArray[index4] + 1] += splitSize * vector2_2;
        }
        first = verticesArray[0];
        second = verticesArray[1];
      }
    }

    public static void Cut(World world, Vector2 start, Vector2 end, float thickness)
    {
      List<Fixture> fixtures = new List<Fixture>();
      List<Vector2> entryPoints = new List<Vector2>();
      List<Vector2> exitPoints = new List<Vector2>();
      if (world.TestPoint(start) != null || world.TestPoint(end) != null)
        return;
      world.RayCast((RayCastCallback) ((f, p, n, fr) =>
      {
        fixtures.Add(f);
        entryPoints.Add(p);
        return 1f;
      }), start, end);
      world.RayCast((RayCastCallback) ((f, p, n, fr) =>
      {
        exitPoints.Add(p);
        return 1f;
      }), end, start);
      if (entryPoints.Count + exitPoints.Count < 2)
        return;
      for (int index = 0; index < fixtures.Count; ++index)
      {
        if (fixtures[index].Shape.ShapeType == ShapeType.Polygon && fixtures[index].Body.BodyType != BodyType.Static)
        {
          Vertices first;
          Vertices second;
          CuttingTools.SplitShape(fixtures[index], entryPoints[index], exitPoints[index], thickness, out first, out second);
          if (CuttingTools.SanityCheck(first))
          {
            Body polygon = BodyFactory.CreatePolygon(world, first, fixtures[index].Shape.Density, fixtures[index].Body.Position);
            polygon.Rotation = fixtures[index].Body.Rotation;
            polygon.LinearVelocity = fixtures[index].Body.LinearVelocity;
            polygon.AngularVelocity = fixtures[index].Body.AngularVelocity;
            polygon.BodyType = BodyType.Dynamic;
          }
          if (CuttingTools.SanityCheck(second))
          {
            Body polygon = BodyFactory.CreatePolygon(world, second, fixtures[index].Shape.Density, fixtures[index].Body.Position);
            polygon.Rotation = fixtures[index].Body.Rotation;
            polygon.LinearVelocity = fixtures[index].Body.LinearVelocity;
            polygon.AngularVelocity = fixtures[index].Body.AngularVelocity;
            polygon.BodyType = BodyType.Dynamic;
          }
          world.RemoveBody(fixtures[index].Body);
        }
      }
    }

    private static bool SanityCheck(Vertices vertices)
    {
      if (vertices.Count < 3 || (double) vertices.GetArea() < 9.9999997473787516E-06)
        return false;
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        int index2 = index1;
        int index3 = index1 + 1 < vertices.Count ? index1 + 1 : 0;
        if ((double) (vertices[index3] - vertices[index2]).LengthSquared() < 1.4210854715202004E-14)
          return false;
      }
      for (int index4 = 0; index4 < vertices.Count; ++index4)
      {
        int index5 = index4;
        int index6 = index4 + 1 < vertices.Count ? index4 + 1 : 0;
        Vector2 vector2_1 = vertices[index6] - vertices[index5];
        for (int index7 = 0; index7 < vertices.Count; ++index7)
        {
          if (index7 != index5 && index7 != index6)
          {
            Vector2 vector2_2 = vertices[index7] - vertices[index5];
            if ((double) vector2_1.X * (double) vector2_2.Y - (double) vector2_1.Y * (double) vector2_2.X < 0.0)
              return false;
          }
        }
      }
      return true;
    }
  }
}
