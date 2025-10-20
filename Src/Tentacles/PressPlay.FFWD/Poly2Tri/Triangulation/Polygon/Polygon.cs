// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Polygon.Polygon
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Delaunay;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Poly2Tri.Triangulation.Polygon
{
  public class Polygon : Triangulatable
  {
    protected List<Poly2Tri.Triangulation.Polygon.Polygon> _holes;
    protected PolygonPoint _last;
    protected List<TriangulationPoint> _points = new List<TriangulationPoint>();
    protected List<TriangulationPoint> _steinerPoints;
    protected List<DelaunayTriangle> _triangles;

    public Polygon(IList<PolygonPoint> points)
    {
      if (points.Count < 3)
        throw new ArgumentException("List has fewer than 3 points", nameof (points));
      if (points[0].Equals((object) points[points.Count - 1]))
        points.RemoveAt(points.Count - 1);
      this._points.AddRange(points.Cast<TriangulationPoint>());
    }

    public Polygon(IEnumerable<PolygonPoint> points)
      : this((points is IList<PolygonPoint> pts) ? pts : points.ToArray())
    {
    }

    public Polygon()
    {
    }

    public IList<Poly2Tri.Triangulation.Polygon.Polygon> Holes => (IList<Poly2Tri.Triangulation.Polygon.Polygon>) this._holes;

    public TriangulationMode TriangulationMode => TriangulationMode.Polygon;

    public IList<TriangulationPoint> Points => (IList<TriangulationPoint>) this._points;

    public IList<DelaunayTriangle> Triangles => (IList<DelaunayTriangle>) this._triangles;

    public void AddTriangle(DelaunayTriangle t) => this._triangles.Add(t);

    public void AddTriangles(IEnumerable<DelaunayTriangle> list) => this._triangles.AddRange(list);

    public void ClearTriangles()
    {
      if (this._triangles == null)
        return;
      this._triangles.Clear();
    }

    public void PrepareTriangulation(TriangulationContext tcx)
    {
      if (this._triangles == null)
        this._triangles = new List<DelaunayTriangle>(this._points.Count);
      else
        this._triangles.Clear();
      for (int index = 0; index < this._points.Count - 1; ++index)
        tcx.NewConstraint(this._points[index], this._points[index + 1]);
      tcx.NewConstraint(this._points[0], this._points[this._points.Count - 1]);
      tcx.Points.AddRange((IEnumerable<TriangulationPoint>) this._points);
      if (this._holes != null)
      {
        foreach (Poly2Tri.Triangulation.Polygon.Polygon hole in this._holes)
        {
          for (int index = 0; index < hole._points.Count - 1; ++index)
            tcx.NewConstraint(hole._points[index], hole._points[index + 1]);
          tcx.NewConstraint(hole._points[0], hole._points[hole._points.Count - 1]);
          tcx.Points.AddRange((IEnumerable<TriangulationPoint>) hole._points);
        }
      }
      if (this._steinerPoints == null)
        return;
      tcx.Points.AddRange((IEnumerable<TriangulationPoint>) this._steinerPoints);
    }

    public void AddSteinerPoint(TriangulationPoint point)
    {
      if (this._steinerPoints == null)
        this._steinerPoints = new List<TriangulationPoint>();
      this._steinerPoints.Add(point);
    }

    public void AddSteinerPoints(List<TriangulationPoint> points)
    {
      if (this._steinerPoints == null)
        this._steinerPoints = new List<TriangulationPoint>();
      this._steinerPoints.AddRange((IEnumerable<TriangulationPoint>) points);
    }

    public void ClearSteinerPoints()
    {
      if (this._steinerPoints == null)
        return;
      this._steinerPoints.Clear();
    }

    public void AddHole(Poly2Tri.Triangulation.Polygon.Polygon poly)
    {
      if (this._holes == null)
        this._holes = new List<Poly2Tri.Triangulation.Polygon.Polygon>();
      this._holes.Add(poly);
    }

    public void InsertPointAfter(PolygonPoint point, PolygonPoint newPoint)
    {
      int num = this._points.IndexOf((TriangulationPoint) point);
      if (num == -1)
        throw new ArgumentException("Tried to insert a point into a Polygon after a point not belonging to the Polygon", nameof (point));
      newPoint.Next = point.Next;
      newPoint.Previous = point;
      point.Next.Previous = newPoint;
      point.Next = newPoint;
      this._points.Insert(num + 1, (TriangulationPoint) newPoint);
    }

    public void AddPoints(IEnumerable<PolygonPoint> list)
    {
      foreach (PolygonPoint polygonPoint in list)
      {
        polygonPoint.Previous = this._last;
        if (this._last != null)
        {
          polygonPoint.Next = this._last.Next;
          this._last.Next = polygonPoint;
        }
        this._last = polygonPoint;
        this._points.Add((TriangulationPoint) polygonPoint);
      }
      PolygonPoint point = (PolygonPoint) this._points[0];
      this._last.Next = point;
      point.Previous = this._last;
    }

    public void AddPoint(PolygonPoint p)
    {
      p.Previous = this._last;
      p.Next = this._last.Next;
      this._last.Next = p;
      this._points.Add((TriangulationPoint) p);
    }

    public void RemovePoint(PolygonPoint p)
    {
      PolygonPoint next = p.Next;
      PolygonPoint previous = p.Previous;
      previous.Next = next;
      next.Previous = previous;
      this._points.Remove((TriangulationPoint) p);
    }
  }
}
