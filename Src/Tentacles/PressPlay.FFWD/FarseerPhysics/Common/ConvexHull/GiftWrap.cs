// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.ConvexHull.GiftWrap
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Common.ConvexHull
{
  public static class GiftWrap
  {
    public static Vertices GetConvexHull(Vertices vertices)
    {
      if (vertices.Count < 3)
        return vertices;
      int[] numArray = new int[vertices.Count];
      int capacity = 0;
      float num1 = float.MaxValue;
      int num2 = vertices.Count;
      for (int index = 0; index < vertices.Count; ++index)
      {
        if ((double) vertices[index].Y < (double) num1)
        {
          num1 = vertices[index].Y;
          num2 = index;
        }
      }
      int index1 = num2;
      int index2 = -1;
      float num3 = -1f;
      float num4 = 0.0f;
      while (index2 != num2)
      {
        float num5 = -2f;
        for (int index3 = 0; index3 < vertices.Count; ++index3)
        {
          if (index3 != index1)
          {
            float num6 = vertices[index3].X - vertices[index1].X;
            float num7 = vertices[index3].Y - vertices[index1].Y;
            float num8 = (float) Math.Sqrt((double) num6 * (double) num6 + (double) num7 * (double) num7);
            float num9 = (double) num8 == 0.0 ? 1f : num8;
            float num10 = num6 / num9;
            float num11 = num7 / num9;
            float num12 = (float) ((double) num10 * (double) num3 + (double) num11 * (double) num4);
            if ((double) num12 > (double) num5)
            {
              num5 = num12;
              index2 = index3;
            }
          }
        }
        numArray[capacity++] = index2;
        float num13 = vertices[index2].X - vertices[index1].X;
        float num14 = vertices[index2].Y - vertices[index1].Y;
        float num15 = (float) Math.Sqrt((double) num13 * (double) num13 + (double) num14 * (double) num14);
        float num16 = (double) num15 == 0.0 ? 1f : num15;
        num3 = num13 / num16;
        num4 = num14 / num16;
        index1 = index2;
      }
      Vertices convexHull = new Vertices(capacity);
      for (int index4 = 0; index4 < capacity; ++index4)
        convexHull.Add(vertices[numArray[index4]]);
      return convexHull;
    }
  }
}
