// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Polygon.PolygonSet
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Polygon
{
  public class PolygonSet
  {
    protected List<Poly2Tri.Triangulation.Polygon.Polygon> _polygons = new List<Poly2Tri.Triangulation.Polygon.Polygon>();

    public PolygonSet()
    {
    }

    public PolygonSet(Poly2Tri.Triangulation.Polygon.Polygon poly) => this._polygons.Add(poly);

    public IEnumerable<Poly2Tri.Triangulation.Polygon.Polygon> Polygons
    {
      get => (IEnumerable<Poly2Tri.Triangulation.Polygon.Polygon>) this._polygons;
    }

    public void Add(Poly2Tri.Triangulation.Polygon.Polygon p) => this._polygons.Add(p);
  }
}
