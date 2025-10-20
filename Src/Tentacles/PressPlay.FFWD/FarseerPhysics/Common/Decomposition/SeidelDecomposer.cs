// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.SeidelDecomposer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public static class SeidelDecomposer
  {
    public static List<Vertices> ConvexPartition(Vertices vertices, float sheer)
    {
      List<Point> polyLine = new List<Point>(vertices.Count);
      foreach (Vector2 vertex in (List<Vector2>) vertices)
        polyLine.Add(new Point(vertex.X, vertex.Y));
      Triangulator triangulator = new Triangulator(polyLine, sheer);
      List<Vertices> verticesList = new List<Vertices>();
      foreach (List<Point> triangle in triangulator.Triangles)
      {
        Vertices vertices1 = new Vertices(triangle.Count);
        foreach (Point point in triangle)
          vertices1.Add(new Vector2(point.X, point.Y));
        verticesList.Add(vertices1);
      }
      return verticesList;
    }

    public static List<Vertices> ConvexPartitionTrapezoid(Vertices vertices, float sheer)
    {
      List<Point> polyLine = new List<Point>(vertices.Count);
      foreach (Vector2 vertex in (List<Vector2>) vertices)
        polyLine.Add(new Point(vertex.X, vertex.Y));
      Triangulator triangulator = new Triangulator(polyLine, sheer);
      List<Vertices> verticesList = new List<Vertices>();
      foreach (Trapezoid trapezoid in triangulator.Trapezoids)
      {
        Vertices vertices1 = new Vertices();
        foreach (Point vertex in trapezoid.Vertices())
          vertices1.Add(new Vector2(vertex.X, vertex.Y));
        verticesList.Add(vertices1);
      }
      return verticesList;
    }
  }
}
