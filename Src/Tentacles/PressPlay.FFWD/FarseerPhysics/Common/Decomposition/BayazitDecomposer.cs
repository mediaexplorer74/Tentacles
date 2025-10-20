// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.BayazitDecomposer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public static class BayazitDecomposer
  {
    private static Vector2 At(int i, Vertices vertices)
    {
      int count = vertices.Count;
      return vertices[i < 0 ? count - -i % count : i % count];
    }

    private static Vertices Copy(int i, int j, Vertices vertices)
    {
      Vertices vertices1 = new Vertices();
      while (j < i)
        j += vertices.Count;
      for (; i <= j; ++i)
        vertices1.Add(BayazitDecomposer.At(i, vertices));
      return vertices1;
    }

    public static List<Vertices> ConvexPartition(Vertices vertices)
    {
      vertices.ForceCounterClockWise();
      List<Vertices> verticesList = new List<Vertices>();
      Vector2 vector2_1 = new Vector2();
      Vector2 vector2_2 = new Vector2();
      int i1 = 0;
      int j = 0;
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        if (BayazitDecomposer.Reflex(index1, vertices))
        {
          float num1;
          float num2 = num1 = float.MaxValue;
          for (int i2 = 0; i2 < vertices.Count; ++i2)
          {
            if (BayazitDecomposer.Left(BayazitDecomposer.At(index1 - 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2, vertices)) && BayazitDecomposer.RightOn(BayazitDecomposer.At(index1 - 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2 - 1, vertices)))
            {
              Vector2 vector2_3 = LineTools.LineIntersect(BayazitDecomposer.At(index1 - 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2, vertices), BayazitDecomposer.At(i2 - 1, vertices));
              if (BayazitDecomposer.Right(BayazitDecomposer.At(index1 + 1, vertices), BayazitDecomposer.At(index1, vertices), vector2_3))
              {
                float num3 = BayazitDecomposer.SquareDist(BayazitDecomposer.At(index1, vertices), vector2_3);
                if ((double) num3 < (double) num2)
                {
                  num2 = num3;
                  vector2_1 = vector2_3;
                  i1 = i2;
                }
              }
            }
            if (BayazitDecomposer.Left(BayazitDecomposer.At(index1 + 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2 + 1, vertices)) && BayazitDecomposer.RightOn(BayazitDecomposer.At(index1 + 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2, vertices)))
            {
              Vector2 vector2_4 = LineTools.LineIntersect(BayazitDecomposer.At(index1 + 1, vertices), BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(i2, vertices), BayazitDecomposer.At(i2 + 1, vertices));
              if (BayazitDecomposer.Left(BayazitDecomposer.At(index1 - 1, vertices), BayazitDecomposer.At(index1, vertices), vector2_4))
              {
                float num4 = BayazitDecomposer.SquareDist(BayazitDecomposer.At(index1, vertices), vector2_4);
                if ((double) num4 < (double) num1)
                {
                  num1 = num4;
                  j = i2;
                  vector2_2 = vector2_4;
                }
              }
            }
          }
          Vertices vertices1;
          Vertices vertices2;
          if (i1 == (j + 1) % vertices.Count)
          {
            Vector2 vector2_5 = (vector2_1 + vector2_2) / 2f;
            vertices1 = BayazitDecomposer.Copy(index1, j, vertices);
            vertices1.Add(vector2_5);
            vertices2 = BayazitDecomposer.Copy(i1, index1, vertices);
            vertices2.Add(vector2_5);
          }
          else
          {
            double num5 = 0.0;
            double num6 = (double) i1;
            while (j < i1)
              j += vertices.Count;
            for (int index2 = i1; index2 <= j; ++index2)
            {
              if (BayazitDecomposer.CanSee(index1, index2, vertices))
              {
                double num7 = 1.0 / ((double) BayazitDecomposer.SquareDist(BayazitDecomposer.At(index1, vertices), BayazitDecomposer.At(index2, vertices)) + 1.0);
                double num8 = !BayazitDecomposer.Reflex(index2, vertices) ? num7 + 1.0 : (!BayazitDecomposer.RightOn(BayazitDecomposer.At(index2 - 1, vertices), BayazitDecomposer.At(index2, vertices), BayazitDecomposer.At(index1, vertices)) || !BayazitDecomposer.LeftOn(BayazitDecomposer.At(index2 + 1, vertices), BayazitDecomposer.At(index2, vertices), BayazitDecomposer.At(index1, vertices)) ? num7 + 2.0 : num7 + 3.0);
                if (num8 > num5)
                {
                  num6 = (double) index2;
                  num5 = num8;
                }
              }
            }
            vertices1 = BayazitDecomposer.Copy(index1, (int) num6, vertices);
            vertices2 = BayazitDecomposer.Copy((int) num6, index1, vertices);
          }
          verticesList.AddRange((IEnumerable<Vertices>) BayazitDecomposer.ConvexPartition(vertices1));
          verticesList.AddRange((IEnumerable<Vertices>) BayazitDecomposer.ConvexPartition(vertices2));
          return verticesList;
        }
      }
      if (vertices.Count > Settings.MaxPolygonVertices)
      {
        Vertices vertices3 = BayazitDecomposer.Copy(0, vertices.Count / 2, vertices);
        Vertices vertices4 = BayazitDecomposer.Copy(vertices.Count / 2, 0, vertices);
        verticesList.AddRange((IEnumerable<Vertices>) BayazitDecomposer.ConvexPartition(vertices3));
        verticesList.AddRange((IEnumerable<Vertices>) BayazitDecomposer.ConvexPartition(vertices4));
      }
      else
        verticesList.Add(vertices);
      for (int index = 0; index < verticesList.Count; ++index)
        verticesList[index] = SimplifyTools.CollinearSimplify(verticesList[index], 0.0f);
      for (int index = verticesList.Count - 1; index >= 0; --index)
      {
        if (verticesList[index].Count == 0)
          verticesList.RemoveAt(index);
      }
      return verticesList;
    }

    private static bool CanSee(int i, int j, Vertices vertices)
    {
      if (BayazitDecomposer.Reflex(i, vertices))
      {
        if (BayazitDecomposer.LeftOn(BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(i - 1, vertices), BayazitDecomposer.At(j, vertices)) && BayazitDecomposer.RightOn(BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(i + 1, vertices), BayazitDecomposer.At(j, vertices)))
          return false;
      }
      else if (BayazitDecomposer.RightOn(BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(i + 1, vertices), BayazitDecomposer.At(j, vertices)) || BayazitDecomposer.LeftOn(BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(i - 1, vertices), BayazitDecomposer.At(j, vertices)))
        return false;
      if (BayazitDecomposer.Reflex(j, vertices))
      {
        if (BayazitDecomposer.LeftOn(BayazitDecomposer.At(j, vertices), BayazitDecomposer.At(j - 1, vertices), BayazitDecomposer.At(i, vertices)) && BayazitDecomposer.RightOn(BayazitDecomposer.At(j, vertices), BayazitDecomposer.At(j + 1, vertices), BayazitDecomposer.At(i, vertices)))
          return false;
      }
      else if (BayazitDecomposer.RightOn(BayazitDecomposer.At(j, vertices), BayazitDecomposer.At(j + 1, vertices), BayazitDecomposer.At(i, vertices)) || BayazitDecomposer.LeftOn(BayazitDecomposer.At(j, vertices), BayazitDecomposer.At(j - 1, vertices), BayazitDecomposer.At(i, vertices)))
        return false;
      for (int i1 = 0; i1 < vertices.Count; ++i1)
      {
        if ((i1 + 1) % vertices.Count != i && i1 != i && (i1 + 1) % vertices.Count != j && i1 != j && LineTools.LineIntersect(BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(j, vertices), BayazitDecomposer.At(i1, vertices), BayazitDecomposer.At(i1 + 1, vertices), out Vector2 _))
          return false;
      }
      return true;
    }

    private static bool Reflex(int i, Vertices vertices) => BayazitDecomposer.Right(i, vertices);

    private static bool Right(int i, Vertices vertices)
    {
      return BayazitDecomposer.Right(BayazitDecomposer.At(i - 1, vertices), BayazitDecomposer.At(i, vertices), BayazitDecomposer.At(i + 1, vertices));
    }

    private static bool Left(Vector2 a, Vector2 b, Vector2 c)
    {
      return (double) MathUtils.Area(ref a, ref b, ref c) > 0.0;
    }

    private static bool LeftOn(Vector2 a, Vector2 b, Vector2 c)
    {
      return (double) MathUtils.Area(ref a, ref b, ref c) >= 0.0;
    }

    private static bool Right(Vector2 a, Vector2 b, Vector2 c)
    {
      return (double) MathUtils.Area(ref a, ref b, ref c) < 0.0;
    }

    private static bool RightOn(Vector2 a, Vector2 b, Vector2 c)
    {
      return (double) MathUtils.Area(ref a, ref b, ref c) <= 0.0;
    }

    private static float SquareDist(Vector2 a, Vector2 b)
    {
      float num1 = b.X - a.X;
      float num2 = b.Y - a.Y;
      return (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }
  }
}
