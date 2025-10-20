// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.FlipcodeDecomposer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public static class FlipcodeDecomposer
  {
    private static Vector2 _tmpA;
    private static Vector2 _tmpB;
    private static Vector2 _tmpC;

    private static bool InsideTriangle(ref Vector2 a, ref Vector2 b, ref Vector2 c, ref Vector2 p)
    {
      float num1 = (float) (((double) c.X - (double) b.X) * ((double) p.Y - (double) b.Y) - ((double) c.Y - (double) b.Y) * ((double) p.X - (double) b.X));
      float num2 = (float) (((double) b.X - (double) a.X) * ((double) p.Y - (double) a.Y) - ((double) b.Y - (double) a.Y) * ((double) p.X - (double) a.X));
      float num3 = (float) (((double) a.X - (double) c.X) * ((double) p.Y - (double) c.Y) - ((double) a.Y - (double) c.Y) * ((double) p.X - (double) c.X));
      return (double) num1 >= 0.0 && (double) num3 >= 0.0 && (double) num2 >= 0.0;
    }

    private static bool Snip(Vertices contour, int u, int v, int w, int n, int[] V)
    {
      if (1.1920928955078125E-07 > (double) MathUtils.Area(ref FlipcodeDecomposer._tmpA, ref FlipcodeDecomposer._tmpB, ref FlipcodeDecomposer._tmpC))
        return false;
      for (int index = 0; index < n; ++index)
      {
        if (index != u && index != v && index != w)
        {
          Vector2 p = contour[V[index]];
          if (FlipcodeDecomposer.InsideTriangle(ref FlipcodeDecomposer._tmpA, ref FlipcodeDecomposer._tmpB, ref FlipcodeDecomposer._tmpC, ref p))
            return false;
        }
      }
      return true;
    }

    public static List<Vertices> ConvexPartition(Vertices contour)
    {
      int count = contour.Count;
      if (count < 3)
        return new List<Vertices>();
      int[] V = new int[count];
      if (contour.IsCounterClockWise())
      {
        for (int index = 0; index < count; ++index)
          V[index] = index;
      }
      else
      {
        for (int index = 0; index < count; ++index)
          V[index] = count - 1 - index;
      }
      int n = count;
      int num = 2 * n;
      List<Vertices> verticesList = new List<Vertices>();
      int v = n - 1;
      while (n > 2)
      {
        if (0 >= num--)
          return new List<Vertices>();
        int u = v;
        if (n <= u)
          u = 0;
        v = u + 1;
        if (n <= v)
          v = 0;
        int w = v + 1;
        if (n <= w)
          w = 0;
        FlipcodeDecomposer._tmpA = contour[V[u]];
        FlipcodeDecomposer._tmpB = contour[V[v]];
        FlipcodeDecomposer._tmpC = contour[V[w]];
        if (FlipcodeDecomposer.Snip(contour, u, v, w, n, V))
        {
          Vertices vertices = new Vertices(3);
          vertices.Add(FlipcodeDecomposer._tmpA);
          vertices.Add(FlipcodeDecomposer._tmpB);
          vertices.Add(FlipcodeDecomposer._tmpC);
          verticesList.Add(vertices);
          int index1 = v;
          for (int index2 = v + 1; index2 < n; ++index2)
          {
            V[index1] = V[index2];
            ++index1;
          }
          --n;
          num = 2 * n;
        }
      }
      return verticesList;
    }
  }
}
