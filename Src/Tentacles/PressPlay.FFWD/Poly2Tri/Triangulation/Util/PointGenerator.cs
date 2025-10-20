// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Util.PointGenerator
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Util
{
  public class PointGenerator
  {
    private static readonly Random RNG = new Random();

    public static List<TriangulationPoint> UniformDistribution(int n, double scale)
    {
      List<TriangulationPoint> triangulationPointList = new List<TriangulationPoint>();
      for (int index = 0; index < n; ++index)
        triangulationPointList.Add(new TriangulationPoint(scale * (0.5 - PointGenerator.RNG.NextDouble()), scale * (0.5 - PointGenerator.RNG.NextDouble())));
      return triangulationPointList;
    }

    public static List<TriangulationPoint> UniformGrid(int n, double scale)
    {
      double num1 = scale / (double) n;
      double num2 = 0.5 * scale;
      List<TriangulationPoint> triangulationPointList = new List<TriangulationPoint>();
      for (int index1 = 0; index1 < n + 1; ++index1)
      {
        double x = num2 - (double) index1 * num1;
        for (int index2 = 0; index2 < n + 1; ++index2)
          triangulationPointList.Add(new TriangulationPoint(x, num2 - (double) index2 * num1));
      }
      return triangulationPointList;
    }
  }
}
