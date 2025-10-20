// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Edge
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class Edge
  {
    public Trapezoid Above;
    public float B;
    public Trapezoid Below;
    public HashSet<Point> MPoints;
    public Point P;
    public Point Q;
    public float Slope;

    public Edge(Point p, Point q)
    {
      this.P = p;
      this.Q = q;
      this.Slope = (double) q.X - (double) p.X == 0.0 ? 0.0f : (float) (((double) q.Y - (double) p.Y) / ((double) q.X - (double) p.X));
      this.B = p.Y - p.X * this.Slope;
      this.Above = (Trapezoid) null;
      this.Below = (Trapezoid) null;
      this.MPoints = new HashSet<Point>();
      this.MPoints.Add(p);
      this.MPoints.Add(q);
    }

    public bool IsAbove(Point point) => (double) this.P.Orient2D(this.Q, point) < 0.0;

    public bool IsBelow(Point point) => (double) this.P.Orient2D(this.Q, point) > 0.0;

    public void AddMpoint(Point point)
    {
      foreach (Point mpoint in this.MPoints)
      {
        if (!mpoint.Neq(point))
          return;
      }
      this.MPoints.Add(point);
    }
  }
}
