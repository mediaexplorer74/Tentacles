// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.AdvancingFrontNode
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public class AdvancingFrontNode
  {
    public AdvancingFrontNode Next;
    public TriangulationPoint Point;
    public AdvancingFrontNode Prev;
    public DelaunayTriangle Triangle;
    public double Value;

    public AdvancingFrontNode(TriangulationPoint point)
    {
      this.Point = point;
      this.Value = point.X;
    }

    public bool HasNext => this.Next != null;

    public bool HasPrev => this.Prev != null;
  }
}
