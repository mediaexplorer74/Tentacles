// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Collision
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  public static class Collision
  {
    private static FatEdge _edgeA;
    private static EPProxy _proxyA = new EPProxy();
    private static EPProxy _proxyB = new EPProxy();
    private static Transform _xf;
    private static Vector2 _limit11;
    private static Vector2 _limit12;
    private static Vector2 _limit21;
    private static Vector2 _limit22;
    private static float _radius;
    private static Vector2[] _tmpNormals = new Vector2[2];

    public static void GetWorldManifold(
      ref Manifold manifold,
      ref Transform transformA,
      float radiusA,
      ref Transform transformB,
      float radiusB,
      out Vector2 normal,
      out FixedArray2<Vector2> points)
    {
      points = new FixedArray2<Vector2>();
      normal = Vector2.Zero;
      if (manifold.PointCount == 0)
      {
        normal = Vector2.UnitY;
      }
      else
      {
        switch (manifold.Type)
        {
          case ManifoldType.Circles:
            Vector2 localPoint1 = manifold.Points[0].LocalPoint;
            float num1 = (float) ((double) transformA.Position.X + (double) transformA.R.Col1.X * (double) manifold.LocalPoint.X + (double) transformA.R.Col2.X * (double) manifold.LocalPoint.Y);
            float num2 = (float) ((double) transformA.Position.Y + (double) transformA.R.Col1.Y * (double) manifold.LocalPoint.X + (double) transformA.R.Col2.Y * (double) manifold.LocalPoint.Y);
            float num3 = (float) ((double) transformB.Position.X + (double) transformB.R.Col1.X * (double) localPoint1.X + (double) transformB.R.Col2.X * (double) localPoint1.Y);
            float num4 = (float) ((double) transformB.Position.Y + (double) transformB.R.Col1.Y * (double) localPoint1.X + (double) transformB.R.Col2.Y * (double) localPoint1.Y);
            normal.X = 1f;
            normal.Y = 0.0f;
            if (((double) num1 - (double) num3) * ((double) num1 - (double) num3) + ((double) num2 - (double) num4) * ((double) num2 - (double) num4) > 1.4210854715202004E-14)
            {
              float num5 = num3 - num1;
              float num6 = num4 - num2;
              float num7 = 1f / (float) Math.Sqrt((double) num5 * (double) num5 + (double) num6 * (double) num6);
              normal.X = num5 * num7;
              normal.Y = num6 * num7;
            }
            Vector2 zero1 = Vector2.Zero with
            {
              X = (float) ((double) num1 + (double) radiusA * (double) normal.X + ((double) num3 - (double) radiusB * (double) normal.X)),
              Y = (float) ((double) num2 + (double) radiusA * (double) normal.Y + ((double) num4 - (double) radiusB * (double) normal.Y))
            };
            points[0] = 0.5f * zero1;
            break;
          case ManifoldType.FaceA:
            normal.X = (float) ((double) transformA.R.Col1.X * (double) manifold.LocalNormal.X + (double) transformA.R.Col2.X * (double) manifold.LocalNormal.Y);
            normal.Y = (float) ((double) transformA.R.Col1.Y * (double) manifold.LocalNormal.X + (double) transformA.R.Col2.Y * (double) manifold.LocalNormal.Y);
            float num8 = (float) ((double) transformA.Position.X + (double) transformA.R.Col1.X * (double) manifold.LocalPoint.X + (double) transformA.R.Col2.X * (double) manifold.LocalPoint.Y);
            float num9 = (float) ((double) transformA.Position.Y + (double) transformA.R.Col1.Y * (double) manifold.LocalPoint.X + (double) transformA.R.Col2.Y * (double) manifold.LocalPoint.Y);
            for (int index = 0; index < manifold.PointCount; ++index)
            {
              Vector2 localPoint2 = manifold.Points[index].LocalPoint;
              float num10 = (float) ((double) transformB.Position.X + (double) transformB.R.Col1.X * (double) localPoint2.X + (double) transformB.R.Col2.X * (double) localPoint2.Y);
              float num11 = (float) ((double) transformB.Position.Y + (double) transformB.R.Col1.Y * (double) localPoint2.X + (double) transformB.R.Col2.Y * (double) localPoint2.Y);
              float num12 = (float) (((double) num10 - (double) num8) * (double) normal.X + ((double) num11 - (double) num9) * (double) normal.Y);
              Vector2 zero2 = Vector2.Zero with
              {
                X = (float) ((double) num10 + ((double) radiusA - (double) num12) * (double) normal.X + ((double) num10 - (double) radiusB * (double) normal.X)),
                Y = (float) ((double) num11 + ((double) radiusA - (double) num12) * (double) normal.Y + ((double) num11 - (double) radiusB * (double) normal.Y))
              };
              points[index] = 0.5f * zero2;
            }
            break;
          case ManifoldType.FaceB:
            normal.X = (float) ((double) transformB.R.Col1.X * (double) manifold.LocalNormal.X + (double) transformB.R.Col2.X * (double) manifold.LocalNormal.Y);
            normal.Y = (float) ((double) transformB.R.Col1.Y * (double) manifold.LocalNormal.X + (double) transformB.R.Col2.Y * (double) manifold.LocalNormal.Y);
            float num13 = (float) ((double) transformB.Position.X + (double) transformB.R.Col1.X * (double) manifold.LocalPoint.X + (double) transformB.R.Col2.X * (double) manifold.LocalPoint.Y);
            float num14 = (float) ((double) transformB.Position.Y + (double) transformB.R.Col1.Y * (double) manifold.LocalPoint.X + (double) transformB.R.Col2.Y * (double) manifold.LocalPoint.Y);
            for (int index = 0; index < manifold.PointCount; ++index)
            {
              Vector2 localPoint3 = manifold.Points[index].LocalPoint;
              float num15 = (float) ((double) transformA.Position.X + (double) transformA.R.Col1.X * (double) localPoint3.X + (double) transformA.R.Col2.X * (double) localPoint3.Y);
              float num16 = (float) ((double) transformA.Position.Y + (double) transformA.R.Col1.Y * (double) localPoint3.X + (double) transformA.R.Col2.Y * (double) localPoint3.Y);
              float num17 = (float) (((double) num15 - (double) num13) * (double) normal.X + ((double) num16 - (double) num14) * (double) normal.Y);
              Vector2 zero3 = Vector2.Zero with
              {
                X = (float) ((double) num15 - (double) radiusA * (double) normal.X + ((double) num15 + ((double) radiusB - (double) num17) * (double) normal.X)),
                Y = (float) ((double) num16 - (double) radiusA * (double) normal.Y + ((double) num16 + ((double) radiusB - (double) num17) * (double) normal.Y))
              };
              points[index] = 0.5f * zero3;
            }
            normal *= -1f;
            break;
          default:
            normal = Vector2.UnitY;
            break;
        }
      }
    }

    public static void GetPointStates(
      out FixedArray2<PointState> state1,
      out FixedArray2<PointState> state2,
      ref Manifold manifold1,
      ref Manifold manifold2)
    {
      state1 = new FixedArray2<PointState>();
      state2 = new FixedArray2<PointState>();
      for (int index1 = 0; index1 < manifold1.PointCount; ++index1)
      {
        ContactID id = manifold1.Points[index1].Id;
        state1[index1] = PointState.Remove;
        for (int index2 = 0; index2 < manifold2.PointCount; ++index2)
        {
          if ((int) manifold2.Points[index2].Id.Key == (int) id.Key)
          {
            state1[index1] = PointState.Persist;
            break;
          }
        }
      }
      for (int index3 = 0; index3 < manifold2.PointCount; ++index3)
      {
        ContactID id = manifold2.Points[index3].Id;
        state2[index3] = PointState.Add;
        for (int index4 = 0; index4 < manifold1.PointCount; ++index4)
        {
          if ((int) manifold1.Points[index4].Id.Key == (int) id.Key)
          {
            state2[index3] = PointState.Persist;
            break;
          }
        }
      }
    }

    public static void CollideCircles(
      ref Manifold manifold,
      CircleShape circleA,
      ref Transform xfA,
      CircleShape circleB,
      ref Transform xfB)
    {
      manifold.PointCount = 0;
      float num1 = (float) ((double) xfA.Position.X + (double) xfA.R.Col1.X * (double) circleA.Position.X + (double) xfA.R.Col2.X * (double) circleA.Position.Y);
      float num2 = (float) ((double) xfA.Position.Y + (double) xfA.R.Col1.Y * (double) circleA.Position.X + (double) xfA.R.Col2.Y * (double) circleA.Position.Y);
      float num3 = (float) ((double) xfB.Position.X + (double) xfB.R.Col1.X * (double) circleB.Position.X + (double) xfB.R.Col2.X * (double) circleB.Position.Y);
      float num4 = (float) ((double) xfB.Position.Y + (double) xfB.R.Col1.Y * (double) circleB.Position.X + (double) xfB.R.Col2.Y * (double) circleB.Position.Y);
      float num5 = (float) (((double) num3 - (double) num1) * ((double) num3 - (double) num1) + ((double) num4 - (double) num2) * ((double) num4 - (double) num2));
      float num6 = circleA.Radius + circleB.Radius;
      if ((double) num5 > (double) num6 * (double) num6)
        return;
      manifold.Type = ManifoldType.Circles;
      manifold.LocalPoint = circleA.Position;
      manifold.LocalNormal = Vector2.Zero;
      manifold.PointCount = 1;
      ManifoldPoint point = manifold.Points[0] with
      {
        LocalPoint = circleB.Position
      };
      point.Id.Key = 0U;
      manifold.Points[0] = point;
    }

    public static void CollidePolygonAndCircle(
      ref Manifold manifold,
      PolygonShape polygonA,
      ref Transform transformA,
      CircleShape circleB,
      ref Transform transformB)
    {
      manifold.PointCount = 0;
      Vector2 vector2_1 = new Vector2((float) ((double) transformB.Position.X + (double) transformB.R.Col1.X * (double) circleB.Position.X + (double) transformB.R.Col2.X * (double) circleB.Position.Y), (float) ((double) transformB.Position.Y + (double) transformB.R.Col1.Y * (double) circleB.Position.X + (double) transformB.R.Col2.Y * (double) circleB.Position.Y));
      Vector2 vector2_2 = new Vector2((float) (((double) vector2_1.X - (double) transformA.Position.X) * (double) transformA.R.Col1.X + ((double) vector2_1.Y - (double) transformA.Position.Y) * (double) transformA.R.Col1.Y), (float) (((double) vector2_1.X - (double) transformA.Position.X) * (double) transformA.R.Col2.X + ((double) vector2_1.Y - (double) transformA.Position.Y) * (double) transformA.R.Col2.Y));
      int index1 = 0;
      float num1 = float.MinValue;
      float num2 = polygonA.Radius + circleB.Radius;
      int count = polygonA.Vertices.Count;
      for (int index2 = 0; index2 < count; ++index2)
      {
        Vector2 normal = polygonA.Normals[index2];
        Vector2 vector2_3 = vector2_2 - polygonA.Vertices[index2];
        float num3 = (float) ((double) normal.X * (double) vector2_3.X + (double) normal.Y * (double) vector2_3.Y);
        if ((double) num3 > (double) num2)
          return;
        if ((double) num3 > (double) num1)
        {
          num1 = num3;
          index1 = index2;
        }
      }
      int index3 = index1;
      int index4 = index3 + 1 < count ? index3 + 1 : 0;
      Vector2 vertex1 = polygonA.Vertices[index3];
      Vector2 vertex2 = polygonA.Vertices[index4];
      if ((double) num1 < 1.1920928955078125E-07)
      {
        manifold.PointCount = 1;
        manifold.Type = ManifoldType.FaceA;
        manifold.LocalNormal = polygonA.Normals[index1];
        manifold.LocalPoint = 0.5f * (vertex1 + vertex2);
        ManifoldPoint point = manifold.Points[0] with
        {
          LocalPoint = circleB.Position
        };
        point.Id.Key = 0U;
        manifold.Points[0] = point;
      }
      else
      {
        float num4 = (float) (((double) vector2_2.X - (double) vertex1.X) * ((double) vertex2.X - (double) vertex1.X) + ((double) vector2_2.Y - (double) vertex1.Y) * ((double) vertex2.Y - (double) vertex1.Y));
        float num5 = (float) (((double) vector2_2.X - (double) vertex2.X) * ((double) vertex1.X - (double) vertex2.X) + ((double) vector2_2.Y - (double) vertex2.Y) * ((double) vertex1.Y - (double) vertex2.Y));
        if ((double) num4 <= 0.0)
        {
          if (((double) vector2_2.X - (double) vertex1.X) * ((double) vector2_2.X - (double) vertex1.X) + ((double) vector2_2.Y - (double) vertex1.Y) * ((double) vector2_2.Y - (double) vertex1.Y) > (double) num2 * (double) num2)
            return;
          manifold.PointCount = 1;
          manifold.Type = ManifoldType.FaceA;
          manifold.LocalNormal = vector2_2 - vertex1;
          float num6 = 1f / (float) Math.Sqrt((double) manifold.LocalNormal.X * (double) manifold.LocalNormal.X + (double) manifold.LocalNormal.Y * (double) manifold.LocalNormal.Y);
          manifold.LocalNormal.X *= num6;
          manifold.LocalNormal.Y *= num6;
          manifold.LocalPoint = vertex1;
          ManifoldPoint point = manifold.Points[0] with
          {
            LocalPoint = circleB.Position
          };
          point.Id.Key = 0U;
          manifold.Points[0] = point;
        }
        else if ((double) num5 <= 0.0)
        {
          if (((double) vector2_2.X - (double) vertex2.X) * ((double) vector2_2.X - (double) vertex2.X) + ((double) vector2_2.Y - (double) vertex2.Y) * ((double) vector2_2.Y - (double) vertex2.Y) > (double) num2 * (double) num2)
            return;
          manifold.PointCount = 1;
          manifold.Type = ManifoldType.FaceA;
          manifold.LocalNormal = vector2_2 - vertex2;
          float num7 = 1f / (float) Math.Sqrt((double) manifold.LocalNormal.X * (double) manifold.LocalNormal.X + (double) manifold.LocalNormal.Y * (double) manifold.LocalNormal.Y);
          manifold.LocalNormal.X *= num7;
          manifold.LocalNormal.Y *= num7;
          manifold.LocalPoint = vertex2;
          ManifoldPoint point = manifold.Points[0] with
          {
            LocalPoint = circleB.Position
          };
          point.Id.Key = 0U;
          manifold.Points[0] = point;
        }
        else
        {
          Vector2 vector2_4 = 0.5f * (vertex1 + vertex2);
          Vector2 vector2_5 = vector2_2 - vector2_4;
          Vector2 normal = polygonA.Normals[index3];
          if ((double) vector2_5.X * (double) normal.X + (double) vector2_5.Y * (double) normal.Y > (double) num2)
            return;
          manifold.PointCount = 1;
          manifold.Type = ManifoldType.FaceA;
          manifold.LocalNormal = polygonA.Normals[index3];
          manifold.LocalPoint = vector2_4;
          ManifoldPoint point = manifold.Points[0] with
          {
            LocalPoint = circleB.Position
          };
          point.Id.Key = 0U;
          manifold.Points[0] = point;
        }
      }
    }

    public static void CollidePolygons(
      ref Manifold manifold,
      PolygonShape polyA,
      ref Transform transformA,
      PolygonShape polyB,
      ref Transform transformB)
    {
      manifold.PointCount = 0;
      float num1 = polyA.Radius + polyB.Radius;
      int edgeIndex1 = 0;
      float maxSeparation1 = FarseerPhysics.Collision.Collision.FindMaxSeparation(out edgeIndex1, polyA, ref transformA, polyB, ref transformB);
      if ((double) maxSeparation1 > (double) num1)
        return;
      int edgeIndex2 = 0;
      float maxSeparation2 = FarseerPhysics.Collision.Collision.FindMaxSeparation(out edgeIndex2, polyB, ref transformB, polyA, ref transformA);
      if ((double) maxSeparation2 > (double) num1)
        return;
      PolygonShape poly1;
      PolygonShape poly2;
      Transform xf1;
      Transform xf2;
      int edge1;
      bool flag;
      if ((double) maxSeparation2 > 0.98000001907348633 * (double) maxSeparation1 + 1.0 / 1000.0)
      {
        poly1 = polyB;
        poly2 = polyA;
        xf1 = transformB;
        xf2 = transformA;
        edge1 = edgeIndex2;
        manifold.Type = ManifoldType.FaceB;
        flag = true;
      }
      else
      {
        poly1 = polyA;
        poly2 = polyB;
        xf1 = transformA;
        xf2 = transformB;
        edge1 = edgeIndex1;
        manifold.Type = ManifoldType.FaceA;
        flag = false;
      }
      FixedArray2<ClipVertex> c;
      FarseerPhysics.Collision.Collision.FindIncidentEdge(out c, poly1, ref xf1, edge1, poly2, ref xf2);
      int count = poly1.Vertices.Count;
      int num2 = edge1;
      int num3 = edge1 + 1 < count ? edge1 + 1 : 0;
      Vector2 vector2_1 = poly1.Vertices[num2];
      Vector2 vector2_2 = poly1.Vertices[num3];
      float num4 = vector2_2.X - vector2_1.X;
      float num5 = vector2_2.Y - vector2_1.Y;
      float num6 = 1f / (float) Math.Sqrt((double) num4 * (double) num4 + (double) num5 * (double) num5);
      float num7 = num4 * num6;
      float x = num5 * num6;
      Vector2 vector2_3 = new Vector2(x, -num7);
      Vector2 vector2_4 = 0.5f * (vector2_1 + vector2_2);
      Vector2 normal = new Vector2((float) ((double) xf1.R.Col1.X * (double) num7 + (double) xf1.R.Col2.X * (double) x), (float) ((double) xf1.R.Col1.Y * (double) num7 + (double) xf1.R.Col2.Y * (double) x));
      float y = normal.Y;
      float num8 = -normal.X;
      vector2_1 = new Vector2((float) ((double) xf1.Position.X + (double) xf1.R.Col1.X * (double) vector2_1.X + (double) xf1.R.Col2.X * (double) vector2_1.Y), (float) ((double) xf1.Position.Y + (double) xf1.R.Col1.Y * (double) vector2_1.X + (double) xf1.R.Col2.Y * (double) vector2_1.Y));
      vector2_2 = new Vector2((float) ((double) xf1.Position.X + (double) xf1.R.Col1.X * (double) vector2_2.X + (double) xf1.R.Col2.X * (double) vector2_2.Y), (float) ((double) xf1.Position.Y + (double) xf1.R.Col1.Y * (double) vector2_2.X + (double) xf1.R.Col2.Y * (double) vector2_2.Y));
      float num9 = (float) ((double) y * (double) vector2_1.X + (double) num8 * (double) vector2_1.Y);
      float offset1 = (float) -((double) normal.X * (double) vector2_1.X + (double) normal.Y * (double) vector2_1.Y) + num1;
      float offset2 = (float) ((double) normal.X * (double) vector2_2.X + (double) normal.Y * (double) vector2_2.Y) + num1;
      FixedArray2<ClipVertex> vOut1;
      FixedArray2<ClipVertex> vOut2;
      if (FarseerPhysics.Collision.Collision.ClipSegmentToLine(out vOut1, ref c, -normal, offset1, num2) < 2 || FarseerPhysics.Collision.Collision.ClipSegmentToLine(out vOut2, ref vOut1, normal, offset2, num3) < 2)
        return;
      manifold.LocalNormal = vector2_3;
      manifold.LocalPoint = vector2_4;
      int index1 = 0;
      for (int index2 = 0; index2 < 2; ++index2)
      {
        Vector2 v1 = vOut2[index2].V;
        if ((double) ((float) ((double) y * (double) v1.X + (double) num8 * (double) v1.Y) - num9) <= (double) num1)
        {
          ManifoldPoint point = manifold.Points[index1];
          Vector2 v2 = vOut2[index2].V;
          float num10 = v2.X - xf2.Position.X;
          float num11 = v2.Y - xf2.Position.Y;
          point.LocalPoint.X = (float) ((double) num10 * (double) xf2.R.Col1.X + (double) num11 * (double) xf2.R.Col1.Y);
          point.LocalPoint.Y = (float) ((double) num10 * (double) xf2.R.Col2.X + (double) num11 * (double) xf2.R.Col2.Y);
          point.Id = vOut2[index2].ID;
          if (flag)
          {
            ContactFeature features = point.Id.Features;
            point.Id.Features.IndexA = features.IndexB;
            point.Id.Features.IndexB = features.IndexA;
            point.Id.Features.TypeA = features.TypeB;
            point.Id.Features.TypeB = features.TypeA;
          }
          manifold.Points[index1] = point;
          ++index1;
        }
      }
      manifold.PointCount = index1;
    }

    public static void CollideEdgeAndCircle(
      ref Manifold manifold,
      EdgeShape edgeA,
      ref Transform transformA,
      CircleShape circleB,
      ref Transform transformB)
    {
      manifold.PointCount = 0;
      Vector2 vector2_1 = MathUtils.MultiplyT(ref transformA, MathUtils.Multiply(ref transformB, ref circleB._position));
      Vector2 vertex1 = edgeA.Vertex1;
      Vector2 vertex2 = edgeA.Vertex2;
      Vector2 vector2_2 = vertex2 - vertex1;
      float num1 = Vector2.Dot(vector2_2, vertex2 - vector2_1);
      float num2 = Vector2.Dot(vector2_2, vector2_1 - vertex1);
      float num3 = edgeA.Radius + circleB.Radius;
      ContactFeature contactFeature;
      contactFeature.IndexB = (byte) 0;
      contactFeature.TypeB = (byte) 0;
      if ((double) num2 <= 0.0)
      {
        Vector2 vector2_3 = vertex1;
        Vector2 vector2_4 = vector2_1 - vector2_3;
        float result;
        Vector2.Dot(ref vector2_4, ref vector2_4, out result);
        if ((double) result > (double) num3 * (double) num3)
          return;
        if (edgeA.HasVertex0)
        {
          Vector2 vertex0 = edgeA.Vertex0;
          Vector2 vector2_5 = vertex1;
          if ((double) Vector2.Dot(vector2_5 - vertex0, vector2_5 - vector2_1) > 0.0)
            return;
        }
        contactFeature.IndexA = (byte) 0;
        contactFeature.TypeA = (byte) 0;
        manifold.PointCount = 1;
        manifold.Type = ManifoldType.Circles;
        manifold.LocalNormal = Vector2.Zero;
        manifold.LocalPoint = vector2_3;
        manifold.Points[0] = new ManifoldPoint()
        {
          Id = {
            Key = 0U,
            Features = contactFeature
          },
          LocalPoint = circleB.Position
        };
      }
      else if ((double) num1 <= 0.0)
      {
        Vector2 vector2_6 = vertex2;
        Vector2 vector2_7 = vector2_1 - vector2_6;
        float result;
        Vector2.Dot(ref vector2_7, ref vector2_7, out result);
        if ((double) result > (double) num3 * (double) num3)
          return;
        if (edgeA.HasVertex3)
        {
          Vector2 vertex3 = edgeA.Vertex3;
          Vector2 vector2_8 = vertex2;
          if ((double) Vector2.Dot(vertex3 - vector2_8, vector2_1 - vector2_8) > 0.0)
            return;
        }
        contactFeature.IndexA = (byte) 1;
        contactFeature.TypeA = (byte) 0;
        manifold.PointCount = 1;
        manifold.Type = ManifoldType.Circles;
        manifold.LocalNormal = Vector2.Zero;
        manifold.LocalPoint = vector2_6;
        manifold.Points[0] = new ManifoldPoint()
        {
          Id = {
            Key = 0U,
            Features = contactFeature
          },
          LocalPoint = circleB.Position
        };
      }
      else
      {
        float result1;
        Vector2.Dot(ref vector2_2, ref vector2_2, out result1);
        Vector2 vector2_9 = 1f / result1 * (num1 * vertex1 + num2 * vertex2);
        Vector2 vector2_10 = vector2_1 - vector2_9;
        float result2;
        Vector2.Dot(ref vector2_10, ref vector2_10, out result2);
        if ((double) result2 > (double) num3 * (double) num3)
          return;
        Vector2 vector2_11 = new Vector2(-vector2_2.Y, vector2_2.X);
        if ((double) Vector2.Dot(vector2_11, vector2_1 - vertex1) < 0.0)
          vector2_11 = new Vector2(-vector2_11.X, -vector2_11.Y);
        vector2_11.Normalize();
        contactFeature.IndexA = (byte) 0;
        contactFeature.TypeA = (byte) 1;
        manifold.PointCount = 1;
        manifold.Type = ManifoldType.FaceA;
        manifold.LocalNormal = vector2_11;
        manifold.LocalPoint = vertex1;
        manifold.Points[0] = new ManifoldPoint()
        {
          Id = {
            Key = 0U,
            Features = contactFeature
          },
          LocalPoint = circleB.Position
        };
      }
    }

    public static void CollideEdgeAndPolygon(
      ref Manifold manifold,
      EdgeShape edgeA,
      ref Transform xfA,
      PolygonShape polygonB,
      ref Transform xfB)
    {
      MathUtils.MultiplyT(ref xfA, ref xfB, out FarseerPhysics.Collision.Collision._xf);
      FarseerPhysics.Collision.Collision._edgeA.V0 = edgeA.Vertex0;
      FarseerPhysics.Collision.Collision._edgeA.V1 = edgeA.Vertex1;
      FarseerPhysics.Collision.Collision._edgeA.V2 = edgeA.Vertex2;
      FarseerPhysics.Collision.Collision._edgeA.V3 = edgeA.Vertex3;
      Vector2 vector2_1 = FarseerPhysics.Collision.Collision._edgeA.V2 - FarseerPhysics.Collision.Collision._edgeA.V1;
      FarseerPhysics.Collision.Collision._edgeA.Normal = new Vector2(vector2_1.Y, -vector2_1.X);
      FarseerPhysics.Collision.Collision._edgeA.Normal.Normalize();
      FarseerPhysics.Collision.Collision._edgeA.HasVertex0 = edgeA.HasVertex0;
      FarseerPhysics.Collision.Collision._edgeA.HasVertex3 = edgeA.HasVertex3;
      FarseerPhysics.Collision.Collision._proxyA.Vertices[0] = FarseerPhysics.Collision.Collision._edgeA.V1;
      FarseerPhysics.Collision.Collision._proxyA.Vertices[1] = FarseerPhysics.Collision.Collision._edgeA.V2;
      FarseerPhysics.Collision.Collision._proxyA.Normals[0] = FarseerPhysics.Collision.Collision._edgeA.Normal;
      FarseerPhysics.Collision.Collision._proxyA.Normals[1] = -FarseerPhysics.Collision.Collision._edgeA.Normal;
      FarseerPhysics.Collision.Collision._proxyA.Centroid = 0.5f * (FarseerPhysics.Collision.Collision._edgeA.V1 + FarseerPhysics.Collision.Collision._edgeA.V2);
      FarseerPhysics.Collision.Collision._proxyA.Count = 2;
      FarseerPhysics.Collision.Collision._proxyB.Count = polygonB.Vertices.Count;
      FarseerPhysics.Collision.Collision._proxyB.Centroid = MathUtils.Multiply(ref FarseerPhysics.Collision.Collision._xf, ref polygonB.MassData.Centroid);
      for (int index = 0; index < polygonB.Vertices.Count; ++index)
      {
        FarseerPhysics.Collision.Collision._proxyB.Vertices[index] = MathUtils.Multiply(ref FarseerPhysics.Collision.Collision._xf, polygonB.Vertices[index]);
        FarseerPhysics.Collision.Collision._proxyB.Normals[index] = MathUtils.Multiply(ref FarseerPhysics.Collision.Collision._xf.R, polygonB.Normals[index]);
      }
      FarseerPhysics.Collision.Collision._radius = 0.02f;
      FarseerPhysics.Collision.Collision._limit11 = Vector2.Zero;
      FarseerPhysics.Collision.Collision._limit12 = Vector2.Zero;
      FarseerPhysics.Collision.Collision._limit21 = Vector2.Zero;
      FarseerPhysics.Collision.Collision._limit22 = Vector2.Zero;
      manifold.PointCount = 0;
      Vector2 v0 = FarseerPhysics.Collision.Collision._edgeA.V0;
      Vector2 v1 = FarseerPhysics.Collision.Collision._edgeA.V1;
      Vector2 v2 = FarseerPhysics.Collision.Collision._edgeA.V2;
      Vector2 v3 = FarseerPhysics.Collision.Collision._edgeA.V3;
      Vector2 centroid = FarseerPhysics.Collision.Collision._proxyB.Centroid;
      if (FarseerPhysics.Collision.Collision._edgeA.HasVertex0)
      {
        Vector2 vector2_2 = v1 - v0;
        Vector2 vector2_3 = v2 - v1;
        Vector2 a = new Vector2(vector2_2.Y, -vector2_2.X);
        Vector2 b = new Vector2(vector2_3.Y, -vector2_3.X);
        a.Normalize();
        b.Normalize();
        bool flag1 = (double) MathUtils.Cross(a, b) >= 0.0;
        bool flag2 = (double) Vector2.Dot(a, centroid - v0) >= 0.0;
        bool flag3 = (double) Vector2.Dot(b, centroid - v1) >= 0.0;
        if (flag1)
        {
          if (flag2 || flag3)
          {
            FarseerPhysics.Collision.Collision._limit11 = b;
            FarseerPhysics.Collision.Collision._limit12 = a;
          }
          else
          {
            FarseerPhysics.Collision.Collision._limit11 = -b;
            FarseerPhysics.Collision.Collision._limit12 = -a;
          }
        }
        else if (flag2 && flag3)
        {
          FarseerPhysics.Collision.Collision._limit11 = a;
          FarseerPhysics.Collision.Collision._limit12 = b;
        }
        else
        {
          FarseerPhysics.Collision.Collision._limit11 = -a;
          FarseerPhysics.Collision.Collision._limit12 = -b;
        }
      }
      else
      {
        FarseerPhysics.Collision.Collision._limit11 = Vector2.Zero;
        FarseerPhysics.Collision.Collision._limit12 = Vector2.Zero;
      }
      if (FarseerPhysics.Collision.Collision._edgeA.HasVertex3)
      {
        Vector2 vector2_4 = v2 - v1;
        Vector2 vector2_5 = v3 - v2;
        Vector2 a = new Vector2(vector2_4.Y, -vector2_4.X);
        Vector2 b = new Vector2(vector2_5.Y, -vector2_5.X);
        a.Normalize();
        b.Normalize();
        bool flag4 = (double) MathUtils.Cross(a, b) >= 0.0;
        bool flag5 = (double) Vector2.Dot(a, centroid - v1) >= 0.0;
        bool flag6 = (double) Vector2.Dot(b, centroid - v2) >= 0.0;
        if (flag4)
        {
          if (flag5 || flag6)
          {
            FarseerPhysics.Collision.Collision._limit21 = b;
            FarseerPhysics.Collision.Collision._limit22 = a;
          }
          else
          {
            FarseerPhysics.Collision.Collision._limit21 = -b;
            FarseerPhysics.Collision.Collision._limit22 = -a;
          }
        }
        else if (flag5 && flag6)
        {
          FarseerPhysics.Collision.Collision._limit21 = a;
          FarseerPhysics.Collision.Collision._limit22 = b;
        }
        else
        {
          FarseerPhysics.Collision.Collision._limit21 = -a;
          FarseerPhysics.Collision.Collision._limit22 = -b;
        }
      }
      else
      {
        FarseerPhysics.Collision.Collision._limit21 = Vector2.Zero;
        FarseerPhysics.Collision.Collision._limit22 = Vector2.Zero;
      }
      EPAxis edgeSeparation = FarseerPhysics.Collision.Collision.ComputeEdgeSeparation();
      if (edgeSeparation.Type == EPAxisType.Unknown || (double) edgeSeparation.Separation > (double) FarseerPhysics.Collision.Collision._radius)
        return;
      EPAxis polygonSeparation = FarseerPhysics.Collision.Collision.ComputePolygonSeparation();
      if (polygonSeparation.Type != EPAxisType.Unknown && (double) polygonSeparation.Separation > (double) FarseerPhysics.Collision.Collision._radius)
        return;
      EPAxis epAxis = polygonSeparation.Type != EPAxisType.Unknown ? ((double) polygonSeparation.Separation <= 0.98000001907348633 * (double) edgeSeparation.Separation + 1.0 / 1000.0 ? edgeSeparation : polygonSeparation) : edgeSeparation;
      FixedArray2<ClipVertex> fixedArray2 = new FixedArray2<ClipVertex>();
      EPProxy proxy1;
      EPProxy proxy2;
      if (epAxis.Type == EPAxisType.EdgeA)
      {
        proxy1 = FarseerPhysics.Collision.Collision._proxyA;
        proxy2 = FarseerPhysics.Collision.Collision._proxyB;
        manifold.Type = ManifoldType.FaceA;
      }
      else
      {
        proxy1 = FarseerPhysics.Collision.Collision._proxyB;
        proxy2 = FarseerPhysics.Collision.Collision._proxyA;
        manifold.Type = ManifoldType.FaceB;
      }
      int index1 = epAxis.Index;
      FarseerPhysics.Collision.Collision.FindIncidentEdge(ref fixedArray2, proxy1, epAxis.Index, proxy2);
      int count = proxy1.Count;
      int vertexIndexA1 = index1;
      int vertexIndexA2 = index1 + 1 < count ? index1 + 1 : 0;
      Vector2 vertex1 = proxy1.Vertices[vertexIndexA1];
      Vector2 vertex2 = proxy1.Vertices[vertexIndexA2];
      Vector2 vector2_6 = vertex2 - vertex1;
      vector2_6.Normalize();
      Vector2 v4 = MathUtils.Cross(vector2_6, 1f);
      Vector2 v5 = 0.5f * (vertex1 + vertex2);
      float num = Vector2.Dot(v4, vertex1);
      float offset1 = -Vector2.Dot(vector2_6, vertex1) + FarseerPhysics.Collision.Collision._radius;
      float offset2 = Vector2.Dot(vector2_6, vertex2) + FarseerPhysics.Collision.Collision._radius;
      FixedArray2<ClipVertex> vOut1;
      FixedArray2<ClipVertex> vOut2;
      if (FarseerPhysics.Collision.Collision.ClipSegmentToLine(out vOut1, ref fixedArray2, -vector2_6, offset1, vertexIndexA1) < 2 || FarseerPhysics.Collision.Collision.ClipSegmentToLine(out vOut2, ref vOut1, vector2_6, offset2, vertexIndexA2) < 2)
        return;
      if (epAxis.Type == EPAxisType.EdgeA)
      {
        manifold.LocalNormal = v4;
        manifold.LocalPoint = v5;
      }
      else
      {
        manifold.LocalNormal = MathUtils.MultiplyT(ref FarseerPhysics.Collision.Collision._xf.R, ref v4);
        manifold.LocalPoint = MathUtils.MultiplyT(ref FarseerPhysics.Collision.Collision._xf, ref v5);
      }
      int index2 = 0;
      for (int index3 = 0; index3 < 2; ++index3)
      {
        if ((double) (Vector2.Dot(v4, vOut2[index3].V) - num) <= (double) FarseerPhysics.Collision.Collision._radius)
        {
          ManifoldPoint point = manifold.Points[index2];
          if (epAxis.Type == EPAxisType.EdgeA)
          {
            point.LocalPoint = MathUtils.MultiplyT(ref FarseerPhysics.Collision.Collision._xf, vOut2[index3].V);
            point.Id = vOut2[index3].ID;
          }
          else
          {
            point.LocalPoint = vOut2[index3].V;
            point.Id.Features.TypeA = vOut2[index3].ID.Features.TypeB;
            point.Id.Features.TypeB = vOut2[index3].ID.Features.TypeA;
            point.Id.Features.IndexA = vOut2[index3].ID.Features.IndexB;
            point.Id.Features.IndexB = vOut2[index3].ID.Features.IndexA;
          }
          manifold.Points[index2] = point;
          ++index2;
        }
      }
      manifold.PointCount = index2;
    }

    private static EPAxis ComputeEdgeSeparation()
    {
      EPAxis edgeSeparation1;
      edgeSeparation1.Type = EPAxisType.Unknown;
      edgeSeparation1.Index = -1;
      edgeSeparation1.Separation = float.MinValue;
      FarseerPhysics.Collision.Collision._tmpNormals[0] = FarseerPhysics.Collision.Collision._edgeA.Normal;
      FarseerPhysics.Collision.Collision._tmpNormals[1] = -FarseerPhysics.Collision.Collision._edgeA.Normal;
      for (int index1 = 0; index1 < 2; ++index1)
      {
        Vector2 tmpNormal = FarseerPhysics.Collision.Collision._tmpNormals[index1];
        bool flag1 = (double) MathUtils.Cross(tmpNormal, FarseerPhysics.Collision.Collision._limit11) >= -1.0 * Math.PI / 90.0 && (double) MathUtils.Cross(FarseerPhysics.Collision.Collision._limit12, tmpNormal) >= -1.0 * Math.PI / 90.0;
        bool flag2 = (double) MathUtils.Cross(tmpNormal, FarseerPhysics.Collision.Collision._limit21) >= -1.0 * Math.PI / 90.0 && (double) MathUtils.Cross(FarseerPhysics.Collision.Collision._limit22, tmpNormal) >= -1.0 * Math.PI / 90.0;
        if (flag1 && flag2)
        {
          EPAxis edgeSeparation2;
          edgeSeparation2.Type = EPAxisType.EdgeA;
          edgeSeparation2.Index = index1;
          edgeSeparation2.Separation = float.MaxValue;
          for (int index2 = 0; index2 < FarseerPhysics.Collision.Collision._proxyB.Count; ++index2)
          {
            float num = Vector2.Dot(tmpNormal, FarseerPhysics.Collision.Collision._proxyB.Vertices[index2] - FarseerPhysics.Collision.Collision._edgeA.V1);
            if ((double) num < (double) edgeSeparation2.Separation)
              edgeSeparation2.Separation = num;
          }
          if ((double) edgeSeparation2.Separation > (double) FarseerPhysics.Collision.Collision._radius)
            return edgeSeparation2;
          if ((double) edgeSeparation2.Separation > (double) edgeSeparation1.Separation)
            edgeSeparation1 = edgeSeparation2;
        }
      }
      return edgeSeparation1;
    }

    private static EPAxis ComputePolygonSeparation()
    {
      EPAxis polygonSeparation;
      polygonSeparation.Type = EPAxisType.Unknown;
      polygonSeparation.Index = -1;
      polygonSeparation.Separation = float.MinValue;
      for (int index = 0; index < FarseerPhysics.Collision.Collision._proxyB.Count; ++index)
      {
        Vector2 vector2 = -FarseerPhysics.Collision.Collision._proxyB.Normals[index];
        bool flag1 = (double) MathUtils.Cross(vector2, FarseerPhysics.Collision.Collision._limit11) >= -1.0 * Math.PI / 90.0 && (double) MathUtils.Cross(FarseerPhysics.Collision.Collision._limit12, vector2) >= -1.0 * Math.PI / 90.0;
        bool flag2 = (double) MathUtils.Cross(vector2, FarseerPhysics.Collision.Collision._limit21) >= -1.0 * Math.PI / 90.0 && (double) MathUtils.Cross(FarseerPhysics.Collision.Collision._limit22, vector2) >= -1.0 * Math.PI / 90.0;
        if (flag1 || flag2)
        {
          float num = Math.Min(Vector2.Dot(vector2, FarseerPhysics.Collision.Collision._proxyB.Vertices[index] - FarseerPhysics.Collision.Collision._edgeA.V1), Vector2.Dot(vector2, FarseerPhysics.Collision.Collision._proxyB.Vertices[index] - FarseerPhysics.Collision.Collision._edgeA.V2));
          if ((double) num > (double) FarseerPhysics.Collision.Collision._radius)
          {
            polygonSeparation.Type = EPAxisType.EdgeB;
            polygonSeparation.Index = index;
            polygonSeparation.Separation = num;
          }
          if ((double) num > (double) polygonSeparation.Separation)
          {
            polygonSeparation.Type = EPAxisType.EdgeB;
            polygonSeparation.Index = index;
            polygonSeparation.Separation = num;
          }
        }
      }
      return polygonSeparation;
    }

    private static void FindIncidentEdge(
      ref FixedArray2<ClipVertex> c,
      EPProxy proxy1,
      int edge1,
      EPProxy proxy2)
    {
      int count = proxy2.Count;
      Vector2 normal = proxy1.Normals[edge1];
      int num1 = 0;
      float num2 = float.MaxValue;
      for (int index = 0; index < count; ++index)
      {
        float num3 = Vector2.Dot(normal, proxy2.Normals[index]);
        if ((double) num3 < (double) num2)
        {
          num2 = num3;
          num1 = index;
        }
      }
      int index1 = num1;
      int index2 = index1 + 1 < count ? index1 + 1 : 0;
      ClipVertex clipVertex = new ClipVertex();
      clipVertex.V = proxy2.Vertices[index1];
      clipVertex.ID.Features.IndexA = (byte) edge1;
      clipVertex.ID.Features.IndexB = (byte) index1;
      clipVertex.ID.Features.TypeA = (byte) 1;
      clipVertex.ID.Features.TypeB = (byte) 0;
      c[0] = clipVertex;
      clipVertex.V = proxy2.Vertices[index2];
      clipVertex.ID.Features.IndexA = (byte) edge1;
      clipVertex.ID.Features.IndexB = (byte) index2;
      clipVertex.ID.Features.TypeA = (byte) 1;
      clipVertex.ID.Features.TypeB = (byte) 0;
      c[1] = clipVertex;
    }

    private static int ClipSegmentToLine(
      out FixedArray2<ClipVertex> vOut,
      ref FixedArray2<ClipVertex> vIn,
      Vector2 normal,
      float offset,
      int vertexIndexA)
    {
      vOut = new FixedArray2<ClipVertex>();
      ClipVertex clipVertex1 = vIn[0];
      ClipVertex clipVertex2 = vIn[1];
      int index = 0;
      float num1 = (float) ((double) normal.X * (double) clipVertex1.V.X + (double) normal.Y * (double) clipVertex1.V.Y) - offset;
      float num2 = (float) ((double) normal.X * (double) clipVertex2.V.X + (double) normal.Y * (double) clipVertex2.V.Y) - offset;
      if ((double) num1 <= 0.0)
        vOut[index++] = clipVertex1;
      if ((double) num2 <= 0.0)
        vOut[index++] = clipVertex2;
      if ((double) num1 * (double) num2 < 0.0)
      {
        float num3 = num1 / (num1 - num2);
        ClipVertex clipVertex3 = vOut[index];
        clipVertex3.V.X = clipVertex1.V.X + num3 * (clipVertex2.V.X - clipVertex1.V.X);
        clipVertex3.V.Y = clipVertex1.V.Y + num3 * (clipVertex2.V.Y - clipVertex1.V.Y);
        clipVertex3.ID.Features.IndexA = (byte) vertexIndexA;
        clipVertex3.ID.Features.IndexB = clipVertex1.ID.Features.IndexB;
        clipVertex3.ID.Features.TypeA = (byte) 0;
        clipVertex3.ID.Features.TypeB = (byte) 1;
        vOut[index] = clipVertex3;
        ++index;
      }
      return index;
    }

    private static float EdgeSeparation(
      PolygonShape poly1,
      ref Transform xf1,
      int edge1,
      PolygonShape poly2,
      ref Transform xf2)
    {
      int count = poly2.Vertices.Count;
      Vector2 normal = poly1.Normals[edge1];
      float num1 = (float) ((double) xf1.R.Col1.X * (double) normal.X + (double) xf1.R.Col2.X * (double) normal.Y);
      float num2 = (float) ((double) xf1.R.Col1.Y * (double) normal.X + (double) xf1.R.Col2.Y * (double) normal.Y);
      Vector2 vector2 = new Vector2((float) ((double) num1 * (double) xf2.R.Col1.X + (double) num2 * (double) xf2.R.Col1.Y), (float) ((double) num1 * (double) xf2.R.Col2.X + (double) num2 * (double) xf2.R.Col2.Y));
      int index1 = 0;
      float num3 = float.MaxValue;
      for (int index2 = 0; index2 < count; ++index2)
      {
        float num4 = Vector2.Dot(poly2.Vertices[index2], vector2);
        if ((double) num4 < (double) num3)
        {
          num3 = num4;
          index1 = index2;
        }
      }
      Vector2 vertex1 = poly1.Vertices[edge1];
      Vector2 vertex2 = poly2.Vertices[index1];
      return (float) (((double) xf2.Position.X + (double) xf2.R.Col1.X * (double) vertex2.X + (double) xf2.R.Col2.X * (double) vertex2.Y - ((double) xf1.Position.X + (double) xf1.R.Col1.X * (double) vertex1.X + (double) xf1.R.Col2.X * (double) vertex1.Y)) * (double) num1 + ((double) xf2.Position.Y + (double) xf2.R.Col1.Y * (double) vertex2.X + (double) xf2.R.Col2.Y * (double) vertex2.Y - ((double) xf1.Position.Y + (double) xf1.R.Col1.Y * (double) vertex1.X + (double) xf1.R.Col2.Y * (double) vertex1.Y)) * (double) num2);
    }

    private static float FindMaxSeparation(
      out int edgeIndex,
      PolygonShape poly1,
      ref Transform xf1,
      PolygonShape poly2,
      ref Transform xf2)
    {
      int count = poly1.Vertices.Count;
      float num1 = (float) ((double) xf2.Position.X + (double) xf2.R.Col1.X * (double) poly2.MassData.Centroid.X + (double) xf2.R.Col2.X * (double) poly2.MassData.Centroid.Y - ((double) xf1.Position.X + (double) xf1.R.Col1.X * (double) poly1.MassData.Centroid.X + (double) xf1.R.Col2.X * (double) poly1.MassData.Centroid.Y));
      float num2 = (float) ((double) xf2.Position.Y + (double) xf2.R.Col1.Y * (double) poly2.MassData.Centroid.X + (double) xf2.R.Col2.Y * (double) poly2.MassData.Centroid.Y - ((double) xf1.Position.Y + (double) xf1.R.Col1.Y * (double) poly1.MassData.Centroid.X + (double) xf1.R.Col2.Y * (double) poly1.MassData.Centroid.Y));
      Vector2 vector2 = new Vector2((float) ((double) num1 * (double) xf1.R.Col1.X + (double) num2 * (double) xf1.R.Col1.Y), (float) ((double) num1 * (double) xf1.R.Col2.X + (double) num2 * (double) xf1.R.Col2.Y));
      int edge1_1 = 0;
      float num3 = float.MinValue;
      for (int index = 0; index < count; ++index)
      {
        float num4 = Vector2.Dot(poly1.Normals[index], vector2);
        if ((double) num4 > (double) num3)
        {
          num3 = num4;
          edge1_1 = index;
        }
      }
      float maxSeparation1 = FarseerPhysics.Collision.Collision.EdgeSeparation(poly1, ref xf1, edge1_1, poly2, ref xf2);
      int edge1_2 = edge1_1 - 1 >= 0 ? edge1_1 - 1 : count - 1;
      float num5 = FarseerPhysics.Collision.Collision.EdgeSeparation(poly1, ref xf1, edge1_2, poly2, ref xf2);
      int edge1_3 = edge1_1 + 1 < count ? edge1_1 + 1 : 0;
      float num6 = FarseerPhysics.Collision.Collision.EdgeSeparation(poly1, ref xf1, edge1_3, poly2, ref xf2);
      int num7;
      int num8;
      float maxSeparation2;
      if ((double) num5 > (double) maxSeparation1 && (double) num5 > (double) num6)
      {
        num7 = -1;
        num8 = edge1_2;
        maxSeparation2 = num5;
      }
      else if ((double) num6 > (double) maxSeparation1)
      {
        num7 = 1;
        num8 = edge1_3;
        maxSeparation2 = num6;
      }
      else
      {
        edgeIndex = edge1_1;
        return maxSeparation1;
      }
      while (true)
      {
        int edge1_4 = num7 != -1 ? (num8 + 1 < count ? num8 + 1 : 0) : (num8 - 1 >= 0 ? num8 - 1 : count - 1);
        float num9 = FarseerPhysics.Collision.Collision.EdgeSeparation(poly1, ref xf1, edge1_4, poly2, ref xf2);
        if ((double) num9 > (double) maxSeparation2)
        {
          num8 = edge1_4;
          maxSeparation2 = num9;
        }
        else
          break;
      }
      edgeIndex = num8;
      return maxSeparation2;
    }

    private static void FindIncidentEdge(
      out FixedArray2<ClipVertex> c,
      PolygonShape poly1,
      ref Transform xf1,
      int edge1,
      PolygonShape poly2,
      ref Transform xf2)
    {
      c = new FixedArray2<ClipVertex>();
      int count = poly2.Vertices.Count;
      Vector2 normal = poly1.Normals[edge1];
      float num1 = (float) ((double) xf1.R.Col1.X * (double) normal.X + (double) xf1.R.Col2.X * (double) normal.Y);
      float num2 = (float) ((double) xf1.R.Col1.Y * (double) normal.X + (double) xf1.R.Col2.Y * (double) normal.Y);
      Vector2 vector2 = new Vector2((float) ((double) num1 * (double) xf2.R.Col1.X + (double) num2 * (double) xf2.R.Col1.Y), (float) ((double) num1 * (double) xf2.R.Col2.X + (double) num2 * (double) xf2.R.Col2.Y));
      int num3 = 0;
      float num4 = float.MaxValue;
      for (int index = 0; index < count; ++index)
      {
        float num5 = Vector2.Dot(vector2, poly2.Normals[index]);
        if ((double) num5 < (double) num4)
        {
          num4 = num5;
          num3 = index;
        }
      }
      int index1 = num3;
      int index2 = index1 + 1 < count ? index1 + 1 : 0;
      ClipVertex clipVertex1 = c[0];
      Vector2 vertex1 = poly2.Vertices[index1];
      clipVertex1.V.X = (float) ((double) xf2.Position.X + (double) xf2.R.Col1.X * (double) vertex1.X + (double) xf2.R.Col2.X * (double) vertex1.Y);
      clipVertex1.V.Y = (float) ((double) xf2.Position.Y + (double) xf2.R.Col1.Y * (double) vertex1.X + (double) xf2.R.Col2.Y * (double) vertex1.Y);
      clipVertex1.ID.Features.IndexA = (byte) edge1;
      clipVertex1.ID.Features.IndexB = (byte) index1;
      clipVertex1.ID.Features.TypeA = (byte) 1;
      clipVertex1.ID.Features.TypeB = (byte) 0;
      c[0] = clipVertex1;
      ClipVertex clipVertex2 = c[1];
      Vector2 vertex2 = poly2.Vertices[index2];
      clipVertex2.V.X = (float) ((double) xf2.Position.X + (double) xf2.R.Col1.X * (double) vertex2.X + (double) xf2.R.Col2.X * (double) vertex2.Y);
      clipVertex2.V.Y = (float) ((double) xf2.Position.Y + (double) xf2.R.Col1.Y * (double) vertex2.X + (double) xf2.R.Col2.Y * (double) vertex2.Y);
      clipVertex2.ID.Features.IndexA = (byte) edge1;
      clipVertex2.ID.Features.IndexB = (byte) index2;
      clipVertex2.ID.Features.TypeA = (byte) 1;
      clipVertex2.ID.Features.TypeB = (byte) 0;
      c[1] = clipVertex2;
    }
  }
}
