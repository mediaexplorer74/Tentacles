// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Sets.PointSet
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Delaunay;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Sets
{
  public class PointSet : Triangulatable
  {
    public PointSet(List<TriangulationPoint> points)
    {
      this.Points = (IList<TriangulationPoint>) new List<TriangulationPoint>((IEnumerable<TriangulationPoint>) points);
    }

    public IList<TriangulationPoint> Points { get; private set; }

    public IList<DelaunayTriangle> Triangles { get; private set; }

    public virtual TriangulationMode TriangulationMode => TriangulationMode.Unconstrained;

    public void AddTriangle(DelaunayTriangle t) => this.Triangles.Add(t);

    public void AddTriangles(IEnumerable<DelaunayTriangle> list)
    {
      foreach (DelaunayTriangle delaunayTriangle in list)
        this.Triangles.Add(delaunayTriangle);
    }

    public void ClearTriangles() => this.Triangles.Clear();

    public virtual void PrepareTriangulation(TriangulationContext tcx)
    {
      if (this.Triangles == null)
        this.Triangles = (IList<DelaunayTriangle>) new List<DelaunayTriangle>(this.Points.Count);
      else
        this.Triangles.Clear();
      tcx.Points.AddRange((IEnumerable<TriangulationPoint>) this.Points);
    }
  }
}
