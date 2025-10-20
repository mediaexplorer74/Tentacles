// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.DTSweepPointComparator
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public class DTSweepPointComparator : IComparer<TriangulationPoint>
  {
    public int Compare(TriangulationPoint p1, TriangulationPoint p2)
    {
      if (p1.Y < p2.Y)
        return -1;
      if (p1.Y > p2.Y)
        return 1;
      if (p1.X < p2.X)
        return -1;
      return p1.X > p2.X ? 1 : 0;
    }
  }
}
