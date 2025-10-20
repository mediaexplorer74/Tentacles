// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.LineTools
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public static class LineTools
  {
    public static float DistanceBetweenPointAndPoint(ref Vector2 point1, ref Vector2 point2)
    {
      Vector2 result;
      Vector2.Subtract(ref point1, ref point2, out result);
      return result.Length();
    }

    public static float DistanceBetweenPointAndLineSegment(
      ref Vector2 point,
      ref Vector2 lineEndPoint1,
      ref Vector2 lineEndPoint2)
    {
      Vector2 vector2 = Vector2.Subtract(lineEndPoint2, lineEndPoint1);
      float num1 = Vector2.Dot(Vector2.Subtract(point, lineEndPoint1), vector2);
      if ((double) num1 <= 0.0)
        return LineTools.DistanceBetweenPointAndPoint(ref point, ref lineEndPoint1);
      float num2 = Vector2.Dot(vector2, vector2);
      if ((double) num2 <= (double) num1)
        return LineTools.DistanceBetweenPointAndPoint(ref point, ref lineEndPoint2);
      float scaleFactor = num1 / num2;
      Vector2 point2 = Vector2.Add(lineEndPoint1, Vector2.Multiply(vector2, scaleFactor));
      return LineTools.DistanceBetweenPointAndPoint(ref point, ref point2);
    }

    public static bool LineIntersect2(
      Vector2 a0,
      Vector2 a1,
      Vector2 b0,
      Vector2 b1,
      out Vector2 intersectionPoint)
    {
      intersectionPoint = Vector2.Zero;
      if (a0 == b0 || a0 == b1 || a1 == b0 || a1 == b1)
        return false;
      float x1 = a0.X;
      float y1 = a0.Y;
      float x2 = a1.X;
      float y2 = a1.Y;
      float x3 = b0.X;
      float y3 = b0.Y;
      float x4 = b1.X;
      float y4 = b1.Y;
      if ((double) Math.Max(x1, x2) < (double) Math.Min(x3, x4) || (double) Math.Max(x3, x4) < (double) Math.Min(x1, x2) || (double) Math.Max(y1, y2) < (double) Math.Min(y3, y4) || (double) Math.Max(y3, y4) < (double) Math.Min(y1, y2))
        return false;
      float num1 = (float) (((double) x4 - (double) x3) * ((double) y1 - (double) y3) - ((double) y4 - (double) y3) * ((double) x1 - (double) x3));
      float num2 = (float) (((double) x2 - (double) x1) * ((double) y1 - (double) y3) - ((double) y2 - (double) y1) * ((double) x1 - (double) x3));
      float num3 = (float) (((double) y4 - (double) y3) * ((double) x2 - (double) x1) - ((double) x4 - (double) x3) * ((double) y2 - (double) y1));
      if ((double) Math.Abs(num3) < 1.1920928955078125E-07)
        return false;
      float num4 = num1 / num3;
      float num5 = num2 / num3;
      if (0.0 >= (double) num4 || (double) num4 >= 1.0 || 0.0 >= (double) num5 || (double) num5 >= 1.0)
        return false;
      intersectionPoint.X = x1 + num4 * (x2 - x1);
      intersectionPoint.Y = y1 + num4 * (y2 - y1);
      return true;
    }

    public static Vector2 LineIntersect(Vector2 p1, Vector2 p2, Vector2 q1, Vector2 q2)
    {
      Vector2 zero = Vector2.Zero;
      float num1 = p2.Y - p1.Y;
      float num2 = p1.X - p2.X;
      float num3 = (float) ((double) num1 * (double) p1.X + (double) num2 * (double) p1.Y);
      float num4 = q2.Y - q1.Y;
      float num5 = q1.X - q2.X;
      float num6 = (float) ((double) num4 * (double) q1.X + (double) num5 * (double) q1.Y);
      float num7 = (float) ((double) num1 * (double) num5 - (double) num4 * (double) num2);
      if (!MathUtils.FloatEquals(num7, 0.0f))
      {
        zero.X = (float) ((double) num5 * (double) num3 - (double) num2 * (double) num6) / num7;
        zero.Y = (float) ((double) num1 * (double) num6 - (double) num4 * (double) num3) / num7;
      }
      return zero;
    }

    public static bool LineIntersect(
      ref Vector2 point1,
      ref Vector2 point2,
      ref Vector2 point3,
      ref Vector2 point4,
      bool firstIsSegment,
      bool secondIsSegment,
      out Vector2 point)
    {
      point = new Vector2();
      float num1 = point4.Y - point3.Y;
      float num2 = point2.X - point1.X;
      float num3 = point4.X - point3.X;
      float num4 = point2.Y - point1.Y;
      float num5 = (float) ((double) num1 * (double) num2 - (double) num3 * (double) num4);
      if ((double) num5 < -1.1920928955078125E-07 || (double) num5 > 1.1920928955078125E-07)
      {
        float num6 = point1.Y - point3.Y;
        float num7 = point1.X - point3.X;
        float num8 = 1f / num5;
        float num9 = (float) ((double) num3 * (double) num6 - (double) num1 * (double) num7) * num8;
        if (!firstIsSegment || (double) num9 >= 0.0 && (double) num9 <= 1.0)
        {
          float num10 = (float) ((double) num2 * (double) num6 - (double) num4 * (double) num7) * num8;
          if ((!secondIsSegment || (double) num10 >= 0.0 && (double) num10 <= 1.0) && ((double) num9 != 0.0 || (double) num10 != 0.0))
          {
            point.X = point1.X + num9 * num2;
            point.Y = point1.Y + num9 * num4;
            return true;
          }
        }
      }
      return false;
    }

    public static bool LineIntersect(
      Vector2 point1,
      Vector2 point2,
      Vector2 point3,
      Vector2 point4,
      bool firstIsSegment,
      bool secondIsSegment,
      out Vector2 intersectionPoint)
    {
      return LineTools.LineIntersect(ref point1, ref point2, ref point3, ref point4, firstIsSegment, secondIsSegment, out intersectionPoint);
    }

    public static bool LineIntersect(
      ref Vector2 point1,
      ref Vector2 point2,
      ref Vector2 point3,
      ref Vector2 point4,
      out Vector2 intersectionPoint)
    {
      return LineTools.LineIntersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
    }

    public static bool LineIntersect(
      Vector2 point1,
      Vector2 point2,
      Vector2 point3,
      Vector2 point4,
      out Vector2 intersectionPoint)
    {
      return LineTools.LineIntersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
    }

    public static void LineSegmentVerticesIntersect(
      ref Vector2 point1,
      ref Vector2 point2,
      Vertices vertices,
      ref List<Vector2> intersectionPoints)
    {
      for (int index = 0; index < vertices.Count; ++index)
      {
        Vector2 intersectionPoint;
        if (LineTools.LineIntersect(vertices[index], vertices[vertices.NextIndex(index)], point1, point2, true, true, out intersectionPoint))
          intersectionPoints.Add(intersectionPoint);
      }
    }

    public static void LineSegmentAABBIntersect(
      ref Vector2 point1,
      ref Vector2 point2,
      AABB aabb,
      ref List<Vector2> intersectionPoints)
    {
      LineTools.LineSegmentVerticesIntersect(ref point1, ref point2, aabb.Vertices, ref intersectionPoints);
    }
  }
}
