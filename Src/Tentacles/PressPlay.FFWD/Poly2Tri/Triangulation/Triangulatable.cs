// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Triangulatable
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Delaunay;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation
{
  public interface Triangulatable
  {
    IList<TriangulationPoint> Points { get; }

    IList<DelaunayTriangle> Triangles { get; }

    TriangulationMode TriangulationMode { get; }

    void PrepareTriangulation(TriangulationContext tcx);

    void AddTriangle(DelaunayTriangle t);

    void AddTriangles(IEnumerable<DelaunayTriangle> list);

    void ClearTriangles();
  }
}
