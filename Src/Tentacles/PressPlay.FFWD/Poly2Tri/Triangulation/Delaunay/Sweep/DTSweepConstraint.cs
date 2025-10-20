// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.DTSweepConstraint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public class DTSweepConstraint : TriangulationConstraint
  {
    public DTSweepConstraint(TriangulationPoint p1, TriangulationPoint p2)
    {
      this.P = p1;
      this.Q = p2;
      if (p1.Y > p2.Y)
      {
        this.Q = p1;
        this.P = p2;
      }
      else if (p1.Y == p2.Y)
      {
        if (p1.X > p2.X)
        {
          this.Q = p1;
          this.P = p2;
        }
        else
        {
          double x1 = p1.X;
          double x2 = p2.X;
        }
      }
      this.Q.AddEdge(this);
    }
  }
}
