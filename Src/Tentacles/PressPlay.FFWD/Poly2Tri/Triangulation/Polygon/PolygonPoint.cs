// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Polygon.PolygonPoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace Poly2Tri.Triangulation.Polygon
{
  public class PolygonPoint(double x, double y) : TriangulationPoint(x, y)
  {
    public PolygonPoint Next { get; set; }

    public PolygonPoint Previous { get; set; }
  }
}
