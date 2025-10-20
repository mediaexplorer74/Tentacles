// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.CDTDecomposer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Poly2Tri.Triangulation;
using Poly2Tri.Triangulation.Delaunay;
using Poly2Tri.Triangulation.Delaunay.Sweep;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public static class CDTDecomposer
  {
    public static List<Vertices> ConvexPartition(Vertices vertices)
    {
      Poly2Tri.Triangulation.Polygon.Polygon t = new Poly2Tri.Triangulation.Polygon.Polygon();
      foreach (Vector2 vertex in (List<Vector2>) vertices)
        t.Points.Add(new TriangulationPoint((double) vertex.X, (double) vertex.Y));
      DTSweepContext tcx = new DTSweepContext();
      tcx.PrepareTriangulation((Triangulatable) t);
      DTSweep.Triangulate(tcx);
      List<Vertices> verticesList = new List<Vertices>();
      foreach (DelaunayTriangle triangle in (IEnumerable<DelaunayTriangle>) t.Triangles)
      {
        Vertices vertices1 = new Vertices();
        foreach (TriangulationPoint point in triangle.Points)
          vertices1.Add(new Vector2((float) point.X, (float) point.Y));
        verticesList.Add(vertices1);
      }
      return verticesList;
    }

    public static List<Vertices> ConvexPartition(DetectedVertices vertices)
    {
      Poly2Tri.Triangulation.Polygon.Polygon t = new Poly2Tri.Triangulation.Polygon.Polygon();
      foreach (Vector2 vertex in (List<Vector2>) vertices)
        t.Points.Add(new TriangulationPoint((double) vertex.X, (double) vertex.Y));
      if (vertices.Holes != null)
      {
        foreach (Vertices hole in vertices.Holes)
        {
          Poly2Tri.Triangulation.Polygon.Polygon poly = new Poly2Tri.Triangulation.Polygon.Polygon();
          foreach (Vector2 vector2 in (List<Vector2>) hole)
            poly.Points.Add(new TriangulationPoint((double) vector2.X, (double) vector2.Y));
          t.AddHole(poly);
        }
      }
      DTSweepContext tcx = new DTSweepContext();
      tcx.PrepareTriangulation((Triangulatable) t);
      DTSweep.Triangulate(tcx);
      List<Vertices> verticesList = new List<Vertices>();
      foreach (DelaunayTriangle triangle in (IEnumerable<DelaunayTriangle>) t.Triangles)
      {
        Vertices vertices1 = new Vertices();
        foreach (TriangulationPoint point in triangle.Points)
          vertices1.Add(new Vector2((float) point.X, (float) point.Y));
        verticesList.Add(vertices1);
      }
      return verticesList;
    }
  }
}
