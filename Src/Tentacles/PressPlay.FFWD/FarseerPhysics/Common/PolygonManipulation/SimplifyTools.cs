// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PolygonManipulation.SimplifyTools
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.PolygonManipulation
{
  public static class SimplifyTools
  {
    private static bool[] _usePt;
    private static double _distanceTolerance;

    public static Vertices CollinearSimplify(Vertices vertices, float collinearityTolerance)
    {
      if (vertices.Count < 3)
        return vertices;
      Vertices vertices1 = new Vertices();
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        int index2 = vertices.PreviousIndex(index1);
        int index3 = vertices.NextIndex(index1);
        Vector2 vertex1 = vertices[index2];
        Vector2 vertex2 = vertices[index1];
        Vector2 vertex3 = vertices[index3];
        if (!MathUtils.Collinear(ref vertex1, ref vertex2, ref vertex3, collinearityTolerance))
          vertices1.Add(vertex2);
      }
      return vertices1;
    }

    public static Vertices CollinearSimplify(Vertices vertices)
    {
      return SimplifyTools.CollinearSimplify(vertices, 0.0f);
    }

    public static Vertices DouglasPeuckerSimplify(Vertices vertices, float distanceTolerance)
    {
      SimplifyTools._distanceTolerance = (double) distanceTolerance;
      SimplifyTools._usePt = new bool[vertices.Count];
      for (int index = 0; index < vertices.Count; ++index)
        SimplifyTools._usePt[index] = true;
      SimplifyTools.SimplifySection(vertices, 0, vertices.Count - 1);
      Vertices vertices1 = new Vertices();
      for (int index = 0; index < vertices.Count; ++index)
      {
        if (SimplifyTools._usePt[index])
          vertices1.Add(vertices[index]);
      }
      return vertices1;
    }

    private static void SimplifySection(Vertices vertices, int i, int j)
    {
      if (i + 1 == j)
        return;
      Vector2 vertex1 = vertices[i];
      Vector2 vertex2 = vertices[j];
      double num1 = -1.0;
      int num2 = i;
      for (int index = i + 1; index < j; ++index)
      {
        double num3 = SimplifyTools.DistancePointLine(vertices[index], vertex1, vertex2);
        if (num3 > num1)
        {
          num1 = num3;
          num2 = index;
        }
      }
      if (num1 <= SimplifyTools._distanceTolerance)
      {
        for (int index = i + 1; index < j; ++index)
          SimplifyTools._usePt[index] = false;
      }
      else
      {
        SimplifyTools.SimplifySection(vertices, i, num2);
        SimplifyTools.SimplifySection(vertices, num2, j);
      }
    }

    private static double DistancePointPoint(Vector2 p, Vector2 p2)
    {
      double num1 = (double) p.X - (double) p2.X;
      double num2 = (double) p.Y - (double) p2.X;
      return Math.Sqrt(num1 * num1 + num2 * num2);
    }

    private static double DistancePointLine(Vector2 p, Vector2 A, Vector2 B)
    {
      if ((double) A.X == (double) B.X && (double) A.Y == (double) B.Y)
        return SimplifyTools.DistancePointPoint(p, A);
      double num = (((double) p.X - (double) A.X) * ((double) B.X - (double) A.X) + ((double) p.Y - (double) A.Y) * ((double) B.Y - (double) A.Y)) / (((double) B.X - (double) A.X) * ((double) B.X - (double) A.X) + ((double) B.Y - (double) A.Y) * ((double) B.Y - (double) A.Y));
      if (num <= 0.0)
        return SimplifyTools.DistancePointPoint(p, A);
      return num >= 1.0 ? SimplifyTools.DistancePointPoint(p, B) : Math.Abs((((double) A.Y - (double) p.Y) * ((double) B.X - (double) A.X) - ((double) A.X - (double) p.X) * ((double) B.Y - (double) A.Y)) / (((double) B.X - (double) A.X) * ((double) B.X - (double) A.X) + ((double) B.Y - (double) A.Y) * ((double) B.Y - (double) A.Y))) * Math.Sqrt(((double) B.X - (double) A.X) * ((double) B.X - (double) A.X) + ((double) B.Y - (double) A.Y) * ((double) B.Y - (double) A.Y));
    }

    public static Vertices ReduceByArea(Vertices vertices, float areaTolerance)
    {
      if (vertices.Count <= 3)
        return vertices;
      if ((double) areaTolerance < 0.0)
        throw new ArgumentOutOfRangeException(nameof (areaTolerance), "must be equal to or greater then zero.");
      Vertices vertices1 = new Vertices();
      Vector2 a = vertices[vertices.Count - 2];
      Vector2 vector2 = vertices[vertices.Count - 1];
      areaTolerance *= 2f;
      int index = 0;
      while (index < vertices.Count)
      {
        Vector2 b;
        if (index == vertices.Count - 1)
          b = vertices1.Count != 0 ? vertices1[0] : throw new ArgumentOutOfRangeException(nameof (areaTolerance), "The tolerance is too high!");
        else
          b = vertices[index];
        float c1;
        MathUtils.Cross(ref a, ref vector2, out c1);
        float c2;
        MathUtils.Cross(ref vector2, ref b, out c2);
        float c3;
        MathUtils.Cross(ref a, ref b, out c3);
        if ((double) Math.Abs(c3 - (c1 + c2)) > (double) areaTolerance)
        {
          vertices1.Add(vector2);
          a = vector2;
        }
        ++index;
        vector2 = b;
      }
      return vertices1;
    }

    public static void MergeParallelEdges(Vertices vertices, float tolerance)
    {
      if (vertices.Count <= 3)
        return;
      bool[] flagArray = new bool[vertices.Count];
      int count = vertices.Count;
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        int index2 = index1 == 0 ? vertices.Count - 1 : index1 - 1;
        int index3 = index1;
        int index4 = index1 == vertices.Count - 1 ? 0 : index1 + 1;
        float num1 = vertices[index3].X - vertices[index2].X;
        float num2 = vertices[index3].Y - vertices[index2].Y;
        float num3 = vertices[index4].Y - vertices[index3].X;
        float num4 = vertices[index4].Y - vertices[index3].Y;
        float num5 = (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
        float num6 = (float) Math.Sqrt((double) num3 * (double) num3 + (double) num4 * (double) num4);
        if (((double) num5 <= 0.0 || (double) num6 <= 0.0) && count > 3)
        {
          flagArray[index1] = true;
          --count;
        }
        float num7 = num1 / num5;
        float num8 = num2 / num5;
        float num9 = num3 / num6;
        float num10 = num4 / num6;
        float num11 = (float) ((double) num7 * (double) num10 - (double) num9 * (double) num8);
        float num12 = (float) ((double) num7 * (double) num9 + (double) num8 * (double) num10);
        if ((double) Math.Abs(num11) < (double) tolerance && (double) num12 > 0.0 && count > 3)
        {
          flagArray[index1] = true;
          --count;
        }
        else
          flagArray[index1] = false;
      }
      if (count == vertices.Count || count == 0)
        return;
      int num = 0;
      Vertices vertices1 = new Vertices((IList<Vector2>) vertices);
      vertices.Clear();
      for (int index = 0; index < vertices1.Count; ++index)
      {
        if (!flagArray[index] && count != 0 && num != count)
        {
          vertices.Add(vertices1[index]);
          ++num;
        }
      }
    }

    public static Vertices MergeIdenticalPoints(Vertices vertices)
    {
      FarseerPhysics.Common.HashSet<Vector2> hashSet = new FarseerPhysics.Common.HashSet<Vector2>();
      for (int index = 0; index < vertices.Count; ++index)
        hashSet.Add(vertices[index]);
      Vertices vertices1 = new Vertices();
      foreach (Vector2 vector2 in hashSet)
        vertices1.Add(vector2);
      return vertices1;
    }

    public static Vertices ReduceByDistance(Vertices vertices, float distance)
    {
      if (vertices.Count < 3)
        return vertices;
      Vertices vertices1 = new Vertices();
      for (int index = 0; index < vertices.Count; ++index)
      {
        Vector2 vertex = vertices[index];
        if ((double) (vertices.NextVertex(index) - vertex).LengthSquared() > (double) distance)
          vertices1.Add(vertex);
      }
      return vertices1;
    }

    public static Vertices ReduceByNth(Vertices vertices, int nth)
    {
      if (vertices.Count < 3 || nth == 0)
        return vertices;
      Vertices vertices1 = new Vertices(vertices.Count);
      for (int index = 0; index < vertices.Count; ++index)
      {
        if (index % nth != 0)
          vertices1.Add(vertices[index]);
      }
      return vertices1;
    }
  }
}
