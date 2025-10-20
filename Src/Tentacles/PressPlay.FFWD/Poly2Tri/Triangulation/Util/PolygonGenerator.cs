// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Util.PolygonGenerator
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Polygon;
using System;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Util
{
  public class PolygonGenerator
  {
    private static readonly Random RNG = new Random();
    private static double PI_2 = 2.0 * Math.PI;

    public static Poly2Tri.Triangulation.Polygon.Polygon RandomCircleSweep(
      double scale,
      int vertexCount)
    {
      double num1 = scale / 4.0;
      PolygonPoint[] points = new PolygonPoint[vertexCount];
      for (int index = 0; index < vertexCount; ++index)
      {
        do
        {
          double num2 = index % 250 != 0 ? (index % 50 != 0 ? num1 + 25.0 * scale / (double) vertexCount * (0.5 - PolygonGenerator.RNG.NextDouble()) : num1 + scale / 5.0 * (0.5 - PolygonGenerator.RNG.NextDouble())) : num1 + scale / 2.0 * (0.5 - PolygonGenerator.RNG.NextDouble());
          double num3 = num2 > scale / 2.0 ? scale / 2.0 : num2;
          num1 = num3 < scale / 10.0 ? scale / 10.0 : num3;
        }
        while (num1 < scale / 10.0 || num1 > scale / 2.0);
        PolygonPoint polygonPoint = new PolygonPoint(num1 * Math.Cos(PolygonGenerator.PI_2 * (double) index / (double) vertexCount), num1 * Math.Sin(PolygonGenerator.PI_2 * (double) index / (double) vertexCount));
        points[index] = polygonPoint;
      }
      return new Poly2Tri.Triangulation.Polygon.Polygon((IList<PolygonPoint>) points);
    }

    public static Poly2Tri.Triangulation.Polygon.Polygon RandomCircleSweep2(
      double scale,
      int vertexCount)
    {
      double num1 = scale / 4.0;
      PolygonPoint[] points = new PolygonPoint[vertexCount];
      for (int index = 0; index < vertexCount; ++index)
      {
        do
        {
          double num2 = num1 + scale / 5.0 * (0.5 - PolygonGenerator.RNG.NextDouble());
          double num3 = num2 > scale / 2.0 ? scale / 2.0 : num2;
          num1 = num3 < scale / 10.0 ? scale / 10.0 : num3;
        }
        while (num1 < scale / 10.0 || num1 > scale / 2.0);
        PolygonPoint polygonPoint = new PolygonPoint(num1 * Math.Cos(PolygonGenerator.PI_2 * (double) index / (double) vertexCount), num1 * Math.Sin(PolygonGenerator.PI_2 * (double) index / (double) vertexCount));
        points[index] = polygonPoint;
      }
      return new Poly2Tri.Triangulation.Polygon.Polygon((IList<PolygonPoint>) points);
    }
  }
}
