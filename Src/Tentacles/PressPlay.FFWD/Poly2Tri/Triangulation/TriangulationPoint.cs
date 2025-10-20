// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.TriangulationPoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Delaunay.Sweep;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation
{
  public class TriangulationPoint
  {
    public double X;
    public double Y;

    public TriangulationPoint(double x, double y)
    {
      this.X = x;
      this.Y = y;
    }

    public List<DTSweepConstraint> Edges { get; private set; }

    public float Xf
    {
      get => (float) this.X;
      set => this.X = (double) value;
    }

    public float Yf
    {
      get => (float) this.Y;
      set => this.Y = (double) value;
    }

    public bool HasEdges => this.Edges != null;

    public override string ToString() => "[" + (object) this.X + "," + (object) this.Y + "]";

    public void AddEdge(DTSweepConstraint e)
    {
      if (this.Edges == null)
        this.Edges = new List<DTSweepConstraint>();
      this.Edges.Add(e);
    }
  }
}
