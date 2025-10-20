// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.TriangulationContext
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Poly2Tri.Triangulation.Delaunay;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Poly2Tri.Triangulation
{
  public abstract class TriangulationContext
  {
    public readonly List<TriangulationPoint> Points = new List<TriangulationPoint>(200);
    public readonly List<DelaunayTriangle> Triangles = new List<DelaunayTriangle>();
    private int _stepTime = -1;

    public TriangulationContext() => this.Terminated = false;

    public TriangulationMode TriangulationMode { get; protected set; }

    public Triangulatable Triangulatable { get; private set; }

    public bool WaitUntilNotified { get; private set; }

    public bool Terminated { get; set; }

    public int StepCount { get; private set; }

    public virtual bool IsDebugEnabled { get; protected set; }

    public void Done() => ++this.StepCount;

    public virtual void PrepareTriangulation(Triangulatable t)
    {
      this.Triangulatable = t;
      this.TriangulationMode = t.TriangulationMode;
      t.PrepareTriangulation(this);
    }

    public abstract TriangulationConstraint NewConstraint(
      TriangulationPoint a,
      TriangulationPoint b);

    public void Update(string message)
    {
    }

    public virtual void Clear()
    {
      this.Points.Clear();
      this.Terminated = false;
      this.StepCount = 0;
    }
  }
}
