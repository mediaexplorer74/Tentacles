// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.EarclipDecomposer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public static class EarclipDecomposer
  {
    private const float Tol = 0.001f;

    public static List<Vertices> ConvexPartition(Vertices vertices)
    {
      return EarclipDecomposer.ConvexPartition(vertices, int.MaxValue, 0.0f);
    }

    public static List<Vertices> ConvexPartition(Vertices vertices, int maxPolys, float tolerance)
    {
      if (vertices.Count < 3)
        return new List<Vertices>() { vertices };
      List<Triangle> triangulated;
      if (vertices.IsCounterClockWise())
      {
        Vertices vertices1 = new Vertices((IList<Vector2>) vertices);
        vertices1.Reverse();
        triangulated = EarclipDecomposer.TriangulatePolygon(vertices1);
      }
      else
        triangulated = EarclipDecomposer.TriangulatePolygon(vertices);
      if (triangulated.Count < 1)
        throw new Exception("Can't triangulate your polygon.");
      List<Vertices> verticesList = EarclipDecomposer.PolygonizeTriangles(triangulated, maxPolys, tolerance);
      for (int index = 0; index < verticesList.Count; ++index)
        verticesList[index] = SimplifyTools.CollinearSimplify(verticesList[index], 0.0f);
      for (int index = verticesList.Count - 1; index >= 0; --index)
      {
        if (verticesList[index].Count == 0)
          verticesList.RemoveAt(index);
      }
      return verticesList;
    }

    public static List<Vertices> PolygonizeTriangles(
      List<Triangle> triangulated,
      int maxPolys,
      float tolerance)
    {
      List<Vertices> verticesList = new List<Vertices>(50);
      int num1 = 0;
      if (triangulated.Count <= 0)
        return verticesList;
      bool[] flagArray = new bool[triangulated.Count];
      for (int index = 0; index < triangulated.Count; ++index)
      {
        flagArray[index] = false;
        if ((double) triangulated[index].X[0] == (double) triangulated[index].X[1] && (double) triangulated[index].Y[0] == (double) triangulated[index].Y[1] || (double) triangulated[index].X[1] == (double) triangulated[index].X[2] && (double) triangulated[index].Y[1] == (double) triangulated[index].Y[2] || (double) triangulated[index].X[0] == (double) triangulated[index].X[2] && (double) triangulated[index].Y[0] == (double) triangulated[index].Y[2])
          flagArray[index] = true;
      }
      bool flag = true;
      while (flag)
      {
        int index1 = -1;
        for (int index2 = 0; index2 < triangulated.Count; ++index2)
        {
          if (!flagArray[index2])
          {
            index1 = index2;
            break;
          }
        }
        if (index1 == -1)
        {
          flag = false;
        }
        else
        {
          Vertices vertices1 = new Vertices(3);
          for (int index3 = 0; index3 < 3; ++index3)
            vertices1.Add(new Vector2(triangulated[index1].X[index3], triangulated[index1].Y[index3]));
          flagArray[index1] = true;
          int index4 = 0;
          int num2 = 0;
          while (num2 < 2 * triangulated.Count)
          {
            while (index4 >= triangulated.Count)
              index4 -= triangulated.Count;
            if (!flagArray[index4])
            {
              Vertices vertices2 = EarclipDecomposer.AddTriangle(triangulated[index4], vertices1);
              if (vertices2 != null && vertices2.Count <= Settings.MaxPolygonVertices && vertices2.IsConvex())
              {
                vertices1 = new Vertices((IList<Vector2>) vertices2);
                flagArray[index4] = true;
              }
            }
            ++num2;
            ++index4;
          }
          if (num1 < maxPolys && vertices1.Count >= 3)
            verticesList.Add(new Vertices((IList<Vector2>) vertices1));
          if (vertices1.Count >= 3)
            ++num1;
        }
      }
      return verticesList;
    }

    public static List<Triangle> TriangulatePolygon(Vertices vertices)
    {
      List<Triangle> triangleList1 = new List<Triangle>();
      if (vertices.Count < 3)
        return new List<Triangle>();
      Vertices poutA;
      Vertices poutB;
      if (EarclipDecomposer.ResolvePinchPoint(new Vertices((IList<Vector2>) vertices), out poutA, out poutB))
      {
        List<Triangle> triangleList2 = EarclipDecomposer.TriangulatePolygon(poutA);
        List<Triangle> triangleList3 = EarclipDecomposer.TriangulatePolygon(poutB);
        if (triangleList2.Count == -1 || triangleList3.Count == -1)
          throw new Exception("Can't triangulate your polygon.");
        for (int index = 0; index < triangleList2.Count; ++index)
          triangleList1.Add(new Triangle(triangleList2[index]));
        for (int index = 0; index < triangleList3.Count; ++index)
          triangleList1.Add(new Triangle(triangleList3[index]));
        return triangleList1;
      }
      Triangle[] triangleArray = new Triangle[vertices.Count - 2];
      int index1 = 0;
      float[] xv = new float[vertices.Count];
      float[] yv = new float[vertices.Count];
      for (int index2 = 0; index2 < vertices.Count; ++index2)
      {
        xv[index2] = vertices[index2].X;
        yv[index2] = vertices[index2].Y;
      }
      int count = vertices.Count;
      while (count > 3)
      {
        int index3 = -1;
        float num1 = -10f;
        for (int i = 0; i < count; ++i)
        {
          if (EarclipDecomposer.IsEar(i, xv, yv, count))
          {
            int index4 = EarclipDecomposer.Remainder(i - 1, count);
            int index5 = EarclipDecomposer.Remainder(i + 1, count);
            Vector2 vector2_1 = new Vector2(xv[index5] - xv[i], yv[index5] - yv[i]);
            Vector2 vector2_2 = new Vector2(xv[i] - xv[index4], yv[i] - yv[index4]);
            Vector2 vector2_3 = new Vector2(xv[index4] - xv[index5], yv[index4] - yv[index5]);
            vector2_1.Normalize();
            vector2_2.Normalize();
            vector2_3.Normalize();
            float c1;
            MathUtils.Cross(ref vector2_1, ref vector2_2, out c1);
            c1 = Math.Abs(c1);
            float c2;
            MathUtils.Cross(ref vector2_2, ref vector2_3, out c2);
            c2 = Math.Abs(c2);
            float c3;
            MathUtils.Cross(ref vector2_3, ref vector2_1, out c3);
            c3 = Math.Abs(c3);
            float num2 = Math.Min(c1, Math.Min(c2, c3));
            if ((double) num2 > (double) num1)
            {
              index3 = i;
              num1 = num2;
            }
          }
        }
        if (index3 == -1)
        {
          for (int index6 = 0; index6 < index1; ++index6)
            triangleList1.Add(new Triangle(triangleArray[index6]));
          return triangleList1;
        }
        --count;
        float[] numArray1 = new float[count];
        float[] numArray2 = new float[count];
        int index7 = 0;
        for (int index8 = 0; index8 < count; ++index8)
        {
          if (index7 == index3)
            ++index7;
          numArray1[index8] = xv[index7];
          numArray2[index8] = yv[index7];
          ++index7;
        }
        int index9 = index3 == 0 ? count : index3 - 1;
        int index10 = index3 == count ? 0 : index3 + 1;
        Triangle triangle = new Triangle(xv[index3], yv[index3], xv[index10], yv[index10], xv[index9], yv[index9]);
        triangleArray[index1] = triangle;
        ++index1;
        xv = numArray1;
        yv = numArray2;
      }
      Triangle triangle1 = new Triangle(xv[1], yv[1], xv[2], yv[2], xv[0], yv[0]);
      triangleArray[index1] = triangle1;
      int num = index1 + 1;
      for (int index11 = 0; index11 < num; ++index11)
        triangleList1.Add(new Triangle(triangleArray[index11]));
      return triangleList1;
    }

    private static bool ResolvePinchPoint(Vertices pin, out Vertices poutA, out Vertices poutB)
    {
      poutA = new Vertices();
      poutB = new Vertices();
      if (pin.Count < 3)
        return false;
      bool flag = false;
      int num1 = -1;
      int num2 = -1;
      for (int index1 = 0; index1 < pin.Count; ++index1)
      {
        for (int index2 = index1 + 1; index2 < pin.Count; ++index2)
        {
          if ((double) Math.Abs(pin[index1].X - pin[index2].X) < 1.0 / 1000.0 && (double) Math.Abs(pin[index1].Y - pin[index2].Y) < 1.0 / 1000.0 && index2 != index1 + 1)
          {
            num1 = index1;
            num2 = index2;
            flag = true;
            break;
          }
        }
        if (flag)
          break;
      }
      if (flag)
      {
        int num3 = num2 - num1;
        if (num3 == pin.Count)
          return false;
        for (int index3 = 0; index3 < num3; ++index3)
        {
          int index4 = EarclipDecomposer.Remainder(num1 + index3, pin.Count);
          poutA.Add(pin[index4]);
        }
        int num4 = pin.Count - num3;
        for (int index5 = 0; index5 < num4; ++index5)
        {
          int index6 = EarclipDecomposer.Remainder(num2 + index5, pin.Count);
          poutB.Add(pin[index6]);
        }
      }
      return flag;
    }

    private static int Remainder(int x, int modulus)
    {
      int num = x % modulus;
      while (num < 0)
        num += modulus;
      return num;
    }

    private static Vertices AddTriangle(Triangle t, Vertices vertices)
    {
      int num1 = -1;
      int num2 = -1;
      int num3 = -1;
      int num4 = -1;
      for (int index = 0; index < vertices.Count; ++index)
      {
        if ((double) t.X[0] == (double) vertices[index].X && (double) t.Y[0] == (double) vertices[index].Y)
        {
          if (num1 == -1)
          {
            num1 = index;
            num2 = 0;
          }
          else
          {
            num3 = index;
            num4 = 0;
          }
        }
        else if ((double) t.X[1] == (double) vertices[index].X && (double) t.Y[1] == (double) vertices[index].Y)
        {
          if (num1 == -1)
          {
            num1 = index;
            num2 = 1;
          }
          else
          {
            num3 = index;
            num4 = 1;
          }
        }
        else if ((double) t.X[2] == (double) vertices[index].X && (double) t.Y[2] == (double) vertices[index].Y)
        {
          if (num1 == -1)
          {
            num1 = index;
            num2 = 2;
          }
          else
          {
            num3 = index;
            num4 = 2;
          }
        }
      }
      if (num1 == 0 && num3 == vertices.Count - 1)
      {
        num1 = vertices.Count - 1;
        num3 = 0;
      }
      if (num3 == -1)
        return (Vertices) null;
      int index1 = 0;
      if (index1 == num2 || index1 == num4)
        index1 = 1;
      if (index1 == num2 || index1 == num4)
        index1 = 2;
      Vertices vertices1 = new Vertices(vertices.Count + 1);
      for (int index2 = 0; index2 < vertices.Count; ++index2)
      {
        vertices1.Add(vertices[index2]);
        if (index2 == num1)
          vertices1.Add(new Vector2(t.X[index1], t.Y[index1]));
      }
      return vertices1;
    }

    private static bool IsEar(int i, float[] xv, float[] yv, int xvLength)
    {
      if (i >= xvLength || i < 0 || xvLength < 3)
        return false;
      int index1 = i + 1;
      int index2 = i - 1;
      float num1;
      float num2;
      float num3;
      float num4;
      if (i == 0)
      {
        num1 = xv[0] - xv[xvLength - 1];
        num2 = yv[0] - yv[xvLength - 1];
        num3 = xv[1] - xv[0];
        num4 = yv[1] - yv[0];
        index2 = xvLength - 1;
      }
      else if (i == xvLength - 1)
      {
        num1 = xv[i] - xv[i - 1];
        num2 = yv[i] - yv[i - 1];
        num3 = xv[0] - xv[i];
        num4 = yv[0] - yv[i];
        index1 = 0;
      }
      else
      {
        num1 = xv[i] - xv[i - 1];
        num2 = yv[i] - yv[i - 1];
        num3 = xv[i + 1] - xv[i];
        num4 = yv[i + 1] - yv[i];
      }
      if ((double) num1 * (double) num4 - (double) num3 * (double) num2 > 0.0)
        return false;
      Triangle triangle = new Triangle(xv[i], yv[i], xv[index1], yv[index1], xv[index2], yv[index2]);
      for (int index3 = 0; index3 < xvLength; ++index3)
      {
        if (index3 != i && index3 != index2 && index3 != index1 && triangle.IsInside(xv[index3], yv[index3]))
          return false;
      }
      return true;
    }
  }
}
