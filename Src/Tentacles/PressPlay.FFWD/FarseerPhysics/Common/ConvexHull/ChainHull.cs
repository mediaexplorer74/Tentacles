// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.ConvexHull.ChainHull
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.ConvexHull
{
  public static class ChainHull
  {
    public static Vertices GetConvexHull(Vertices P)
    {
      P.Sort((IComparer<Vector2>) new ChainHull.PointComparer());
      Vector2[] vector2Array = new Vector2[P.Count];
      Vertices convexHull = new Vertices();
      int count = P.Count;
      int num1 = -1;
      int index1 = 0;
      float x1 = P[0].X;
      int index2 = 1;
      while (index2 < count && (double) P[index2].X == (double) x1)
        ++index2;
      int index3 = index2 - 1;
      if (index3 == count - 1)
      {
        int num2;
        vector2Array[num2 = num1 + 1] = P[index1];
        if ((double) P[index3].Y != (double) P[index1].Y)
          vector2Array[++num2] = P[index3];
        int num3;
        vector2Array[num3 = num2 + 1] = P[index1];
        for (int index4 = 0; index4 < num3 + 1; ++index4)
          convexHull.Add(vector2Array[index4]);
        return convexHull;
      }
      int num4 = convexHull.Count - 1;
      int index5 = count - 1;
      float x2 = P[count - 1].X;
      int index6 = count - 2;
      while (index6 >= 0 && (double) P[index6].X == (double) x2)
        --index6;
      int index7 = index6 + 1;
      int index8;
      vector2Array[index8 = num4 + 1] = P[index1];
      int index9 = index3;
      while (++index9 <= index7)
      {
        if ((double) MathUtils.Area(P[index1], P[index7], P[index9]) < 0.0 || index9 >= index7)
        {
          while (index8 > 0 && (double) MathUtils.Area(vector2Array[index8 - 1], vector2Array[index8], P[index9]) <= 0.0)
            --index8;
          vector2Array[++index8] = P[index9];
        }
      }
      if (index5 != index7)
        vector2Array[++index8] = P[index5];
      int num5 = index8;
      int index10 = index7;
      while (--index10 >= index3)
      {
        if ((double) MathUtils.Area(P[index5], P[index3], P[index10]) < 0.0 || index10 <= index3)
        {
          while (index8 > num5 && (double) MathUtils.Area(vector2Array[index8 - 1], vector2Array[index8], P[index10]) <= 0.0)
            --index8;
          vector2Array[++index8] = P[index10];
        }
      }
      if (index3 != index1)
        vector2Array[++index8] = P[index1];
      for (int index11 = 0; index11 < index8 + 1; ++index11)
        convexHull.Add(vector2Array[index11]);
      return convexHull;
    }

    public class PointComparer : Comparer<Vector2>
    {
      public override int Compare(Vector2 a, Vector2 b)
      {
        int num = a.X.CompareTo(b.X);
        return num == 0 ? a.Y.CompareTo(b.Y) : num;
      }
    }
  }
}
