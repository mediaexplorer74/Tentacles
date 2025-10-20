// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PolygonManipulation.YuPengClipper
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.PolygonManipulation
{
  public static class YuPengClipper
  {
    private const float ClipperEpsilonSquared = 1.1920929E-07f;

    public static List<Vertices> Union(
      Vertices polygon1,
      Vertices polygon2,
      out PolyClipError error)
    {
      return YuPengClipper.Execute(polygon1, polygon2, PolyClipType.Union, out error);
    }

    public static List<Vertices> Difference(
      Vertices polygon1,
      Vertices polygon2,
      out PolyClipError error)
    {
      return YuPengClipper.Execute(polygon1, polygon2, PolyClipType.Difference, out error);
    }

    public static List<Vertices> Intersect(
      Vertices polygon1,
      Vertices polygon2,
      out PolyClipError error)
    {
      return YuPengClipper.Execute(polygon1, polygon2, PolyClipType.Intersect, out error);
    }

    private static List<Vertices> Execute(
      Vertices subject,
      Vertices clip,
      PolyClipType clipType,
      out PolyClipError error)
    {
      Vertices slicedPoly1;
      Vertices slicedPoly2;
      YuPengClipper.CalculateIntersections(subject, clip, out slicedPoly1, out slicedPoly2);
      Vector2 lowerBound1 = subject.GetCollisionBox().LowerBound;
      Vector2 lowerBound2 = clip.GetCollisionBox().LowerBound;
      Vector2 result1;
      Vector2.Min(ref lowerBound1, ref lowerBound2, out result1);
      Vector2 vector = Vector2.One - result1;
      if (vector != Vector2.Zero)
      {
        slicedPoly1.Translate(ref vector);
        slicedPoly2.Translate(ref vector);
      }
      slicedPoly1.ForceCounterClockWise();
      slicedPoly2.ForceCounterClockWise();
      List<float> coeff1;
      List<YuPengClipper.Edge> simplicies1;
      YuPengClipper.CalculateSimplicalChain(slicedPoly1, out coeff1, out simplicies1);
      List<float> coeff2;
      List<YuPengClipper.Edge> simplicies2;
      YuPengClipper.CalculateSimplicalChain(slicedPoly2, out coeff2, out simplicies2);
      List<YuPengClipper.Edge> resultSimplices;
      YuPengClipper.CalculateResultChain(coeff1, simplicies1, coeff2, simplicies2, clipType, out resultSimplices);
      List<Vertices> result2;
      error = YuPengClipper.BuildPolygonsFromChain(resultSimplices, out result2);
      vector *= -1f;
      for (int index = 0; index < result2.Count; ++index)
      {
        result2[index].Translate(ref vector);
        SimplifyTools.CollinearSimplify(result2[index]);
      }
      return result2;
    }

    private static void CalculateIntersections(
      Vertices polygon1,
      Vertices polygon2,
      out Vertices slicedPoly1,
      out Vertices slicedPoly2)
    {
      slicedPoly1 = new Vertices((IList<Vector2>) polygon1);
      slicedPoly2 = new Vertices((IList<Vector2>) polygon2);
      for (int index1 = 0; index1 < polygon1.Count; ++index1)
      {
        Vector2 vector2_1 = polygon1[index1];
        Vector2 vector2_2 = polygon1[polygon1.NextIndex(index1)];
        for (int index2 = 0; index2 < polygon2.Count; ++index2)
        {
          Vector2 vector2_3 = polygon2[index2];
          Vector2 vector2_4 = polygon2[polygon2.NextIndex(index2)];
          Vector2 intersectionPoint;
          if (LineTools.LineIntersect(vector2_1, vector2_2, vector2_3, vector2_4, out intersectionPoint))
          {
            float alpha1 = YuPengClipper.GetAlpha(vector2_1, vector2_2, intersectionPoint);
            if ((double) alpha1 > 0.0 && (double) alpha1 < 1.0)
            {
              int index3 = slicedPoly1.IndexOf(vector2_1) + 1;
              while (index3 < slicedPoly1.Count && (double) YuPengClipper.GetAlpha(vector2_1, vector2_2, slicedPoly1[index3]) <= (double) alpha1)
                ++index3;
              slicedPoly1.Insert(index3, intersectionPoint);
            }
            float alpha2 = YuPengClipper.GetAlpha(vector2_3, vector2_4, intersectionPoint);
            if ((double) alpha2 > 0.0 && (double) alpha2 < 1.0)
            {
              int index4 = slicedPoly2.IndexOf(vector2_3) + 1;
              while (index4 < slicedPoly2.Count && (double) YuPengClipper.GetAlpha(vector2_3, vector2_4, slicedPoly2[index4]) <= (double) alpha2)
                ++index4;
              slicedPoly2.Insert(index4, intersectionPoint);
            }
          }
        }
      }
      for (int index5 = 0; index5 < slicedPoly1.Count; ++index5)
      {
        int index6 = slicedPoly1.NextIndex(index5);
        if ((double) (slicedPoly1[index6] - slicedPoly1[index5]).LengthSquared() <= 1.1920928955078125E-07)
        {
          slicedPoly1.RemoveAt(index5);
          --index5;
        }
      }
      for (int index7 = 0; index7 < slicedPoly2.Count; ++index7)
      {
        int index8 = slicedPoly2.NextIndex(index7);
        if ((double) (slicedPoly2[index8] - slicedPoly2[index7]).LengthSquared() <= 1.1920928955078125E-07)
        {
          slicedPoly2.RemoveAt(index7);
          --index7;
        }
      }
    }

    private static void CalculateSimplicalChain(
      Vertices poly,
      out List<float> coeff,
      out List<YuPengClipper.Edge> simplicies)
    {
      simplicies = new List<YuPengClipper.Edge>();
      coeff = new List<float>();
      for (int index = 0; index < poly.Count; ++index)
      {
        simplicies.Add(new YuPengClipper.Edge(poly[index], poly[poly.NextIndex(index)]));
        coeff.Add(YuPengClipper.CalculateSimplexCoefficient(Vector2.Zero, poly[index], poly[poly.NextIndex(index)]));
      }
    }

    private static void CalculateResultChain(
      List<float> poly1Coeff,
      List<YuPengClipper.Edge> poly1Simplicies,
      List<float> poly2Coeff,
      List<YuPengClipper.Edge> poly2Simplicies,
      PolyClipType clipType,
      out List<YuPengClipper.Edge> resultSimplices)
    {
      resultSimplices = new List<YuPengClipper.Edge>();
      for (int index1 = 0; index1 < poly1Simplicies.Count; ++index1)
      {
        float num = 0.0f;
        if (poly2Simplicies.Contains(poly1Simplicies[index1]) || poly2Simplicies.Contains(-poly1Simplicies[index1]) && clipType == PolyClipType.Union)
        {
          num = 1f;
        }
        else
        {
          for (int index2 = 0; index2 < poly2Simplicies.Count; ++index2)
          {
            if (!poly2Simplicies.Contains(-poly1Simplicies[index1]))
              num += YuPengClipper.CalculateBeta(poly1Simplicies[index1].GetCenter(), poly2Simplicies[index2], poly2Coeff[index2]);
          }
        }
        if (clipType == PolyClipType.Intersect)
        {
          if ((double) num == 1.0)
            resultSimplices.Add(poly1Simplicies[index1]);
        }
        else if ((double) num == 0.0)
          resultSimplices.Add(poly1Simplicies[index1]);
      }
      for (int index3 = 0; index3 < poly2Simplicies.Count; ++index3)
      {
        if (!resultSimplices.Contains(poly2Simplicies[index3]) && !resultSimplices.Contains(-poly2Simplicies[index3]))
        {
          float num = 0.0f;
          if (poly1Simplicies.Contains(poly2Simplicies[index3]) || poly1Simplicies.Contains(-poly2Simplicies[index3]) && clipType == PolyClipType.Union)
          {
            num = 1f;
          }
          else
          {
            for (int index4 = 0; index4 < poly1Simplicies.Count; ++index4)
            {
              if (!poly1Simplicies.Contains(-poly2Simplicies[index3]))
                num += YuPengClipper.CalculateBeta(poly2Simplicies[index3].GetCenter(), poly1Simplicies[index4], poly1Coeff[index4]);
            }
          }
          if (clipType == PolyClipType.Intersect || clipType == PolyClipType.Difference)
          {
            if ((double) num == 1.0)
              resultSimplices.Add(-poly2Simplicies[index3]);
          }
          else if ((double) num == 0.0)
            resultSimplices.Add(poly2Simplicies[index3]);
        }
      }
    }

    private static PolyClipError BuildPolygonsFromChain(
      List<YuPengClipper.Edge> simplicies,
      out List<Vertices> result)
    {
      result = new List<Vertices>();
      PolyClipError polyClipError = PolyClipError.None;
      while (simplicies.Count > 0)
      {
        Vertices vertices = new Vertices();
        vertices.Add(simplicies[0].EdgeStart);
        vertices.Add(simplicies[0].EdgeEnd);
        simplicies.RemoveAt(0);
        bool flag = false;
        int index = 0;
        int count = simplicies.Count;
        while (!flag && simplicies.Count > 0)
        {
          if (YuPengClipper.VectorEqual(vertices[vertices.Count - 1], simplicies[index].EdgeStart))
          {
            if (YuPengClipper.VectorEqual(simplicies[index].EdgeEnd, vertices[0]))
              flag = true;
            else
              vertices.Add(simplicies[index].EdgeEnd);
            simplicies.RemoveAt(index);
            --index;
          }
          else if (YuPengClipper.VectorEqual(vertices[vertices.Count - 1], simplicies[index].EdgeEnd))
          {
            if (YuPengClipper.VectorEqual(simplicies[index].EdgeStart, vertices[0]))
              flag = true;
            else
              vertices.Add(simplicies[index].EdgeStart);
            simplicies.RemoveAt(index);
            --index;
          }
          if (!flag && ++index == simplicies.Count)
          {
            if (count == simplicies.Count)
            {
              result = new List<Vertices>();
              return PolyClipError.BrokenResult;
            }
            index = 0;
            count = simplicies.Count;
          }
        }
        if (vertices.Count < 3)
          polyClipError = PolyClipError.DegeneratedOutput;
        result.Add(vertices);
      }
      return polyClipError;
    }

    private static float CalculateBeta(Vector2 point, YuPengClipper.Edge e, float coefficient)
    {
      float beta = 0.0f;
      if (YuPengClipper.PointInSimplex(point, e))
        beta = coefficient;
      if (YuPengClipper.PointOnLineSegment(Vector2.Zero, e.EdgeStart, point) || YuPengClipper.PointOnLineSegment(Vector2.Zero, e.EdgeEnd, point))
        beta = 0.5f * coefficient;
      return beta;
    }

    private static float GetAlpha(Vector2 start, Vector2 end, Vector2 point)
    {
      return (point - start).LengthSquared() / (end - start).LengthSquared();
    }

    private static float CalculateSimplexCoefficient(Vector2 a, Vector2 b, Vector2 c)
    {
      float num = MathUtils.Area(ref a, ref b, ref c);
      if ((double) num < 0.0)
        return -1f;
      return (double) num > 0.0 ? 1f : 0.0f;
    }

    private static bool PointInSimplex(Vector2 point, YuPengClipper.Edge edge)
    {
      Vertices vertices = new Vertices();
      vertices.Add(Vector2.Zero);
      vertices.Add(edge.EdgeStart);
      vertices.Add(edge.EdgeEnd);
      return vertices.PointInPolygon(ref point) == 1;
    }

    private static bool PointOnLineSegment(Vector2 start, Vector2 end, Vector2 point)
    {
      Vector2 vector2 = end - start;
      return (double) MathUtils.Area(ref start, ref end, ref point) == 0.0 && (double) Vector2.Dot(point - start, vector2) >= 0.0 && (double) Vector2.Dot(point - end, vector2) <= 0.0;
    }

    private static bool VectorEqual(Vector2 vec1, Vector2 vec2)
    {
      return (double) (vec2 - vec1).LengthSquared() <= 1.1920928955078125E-07;
    }

    private sealed class Edge
    {
      public Edge(Vector2 edgeStart, Vector2 edgeEnd)
      {
        this.EdgeStart = edgeStart;
        this.EdgeEnd = edgeEnd;
      }

      public Vector2 EdgeStart { get; private set; }

      public Vector2 EdgeEnd { get; private set; }

      public Vector2 GetCenter() => (this.EdgeStart + this.EdgeEnd) / 2f;

      public static YuPengClipper.Edge operator -(YuPengClipper.Edge e)
      {
        return new YuPengClipper.Edge(e.EdgeEnd, e.EdgeStart);
      }

      public override bool Equals(object obj)
      {
        return obj != null && this.Equals(obj as YuPengClipper.Edge);
      }

      public bool Equals(YuPengClipper.Edge e)
      {
        return e != null && YuPengClipper.VectorEqual(this.EdgeStart, e.EdgeStart) && YuPengClipper.VectorEqual(this.EdgeEnd, e.EdgeEnd);
      }

      public override int GetHashCode()
      {
        return this.EdgeStart.GetHashCode() ^ this.EdgeEnd.GetHashCode();
      }
    }
  }
}
