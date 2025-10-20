// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.ConvexHull.Melkman
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Common.ConvexHull
{
  public static class Melkman
  {
    public static Vertices GetConvexHull(Vertices vertices)
    {
      if (vertices.Count < 3)
        return vertices;
      Vector2[] vector2Array = new Vector2[vertices.Count + 1];
      int index1 = 3;
      int index2 = 0;
      int index3 = 3;
      float num = MathUtils.Area(vertices[0], vertices[1], vertices[2]);
      if ((double) num == 0.0)
      {
        vector2Array[0] = vertices[0];
        vector2Array[1] = vertices[2];
        vector2Array[2] = vertices[0];
        index1 = 2;
        for (index3 = 3; index3 < vertices.Count; ++index3)
        {
          Vector2 vertex = vertices[index3];
          if ((double) MathUtils.Area(ref vector2Array[0], ref vector2Array[1], ref vertex) == 0.0)
            vector2Array[1] = vertices[index3];
          else
            break;
        }
      }
      else
      {
        vector2Array[0] = vector2Array[3] = vertices[2];
        if ((double) num > 0.0)
        {
          vector2Array[1] = vertices[0];
          vector2Array[2] = vertices[1];
        }
        else
        {
          vector2Array[1] = vertices[1];
          vector2Array[2] = vertices[0];
        }
      }
      int index4 = index1 == 0 ? vector2Array.Length - 1 : index1 - 1;
      int index5 = index2 == vector2Array.Length - 1 ? 0 : index2 + 1;
      for (int index6 = index3; index6 < vertices.Count; ++index6)
      {
        Vector2 vertex = vertices[index6];
        if ((double) MathUtils.Area(ref vector2Array[index4], ref vector2Array[index1], ref vertex) <= 0.0 || (double) MathUtils.Area(ref vector2Array[index2], ref vector2Array[index5], ref vertex) <= 0.0)
        {
          for (; (double) MathUtils.Area(ref vector2Array[index4], ref vector2Array[index1], ref vertex) <= 0.0; index4 = index1 == 0 ? vector2Array.Length - 1 : index1 - 1)
            index1 = index4;
          index1 = index1 == vector2Array.Length - 1 ? 0 : index1 + 1;
          index4 = index1 == 0 ? vector2Array.Length - 1 : index1 - 1;
          vector2Array[index1] = vertex;
          for (; (double) MathUtils.Area(ref vector2Array[index2], ref vector2Array[index5], ref vertex) <= 0.0; index5 = index2 == vector2Array.Length - 1 ? 0 : index2 + 1)
            index2 = index5;
          index2 = index2 == 0 ? vector2Array.Length - 1 : index2 - 1;
          index5 = index2 == vector2Array.Length - 1 ? 0 : index2 + 1;
          vector2Array[index2] = vertex;
        }
      }
      Vertices convexHull = new Vertices(vertices.Count + 1);
      if (index2 < index1)
      {
        for (int index7 = index2; index7 < index1; ++index7)
          convexHull.Add(vector2Array[index7]);
      }
      else
      {
        for (int index8 = 0; index8 < index1; ++index8)
          convexHull.Add(vector2Array[index8]);
        for (int index9 = index2; index9 < vector2Array.Length; ++index9)
          convexHull.Add(vector2Array[index9]);
      }
      return convexHull;
    }
  }
}
