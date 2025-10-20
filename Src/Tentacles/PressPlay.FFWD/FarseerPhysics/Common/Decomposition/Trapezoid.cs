// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Trapezoid
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class Trapezoid
  {
    public Edge Bottom;
    public bool Inside;
    public Point LeftPoint;
    public Trapezoid LowerLeft;
    public Trapezoid LowerRight;
    public Point RightPoint;
    public Sink Sink;
    public Edge Top;
    public Trapezoid UpperLeft;
    public Trapezoid UpperRight;

    public Trapezoid(Point leftPoint, Point rightPoint, Edge top, Edge bottom)
    {
      this.LeftPoint = leftPoint;
      this.RightPoint = rightPoint;
      this.Top = top;
      this.Bottom = bottom;
      this.UpperLeft = (Trapezoid) null;
      this.UpperRight = (Trapezoid) null;
      this.LowerLeft = (Trapezoid) null;
      this.LowerRight = (Trapezoid) null;
      this.Inside = true;
      this.Sink = (Sink) null;
    }

    public void UpdateLeft(Trapezoid ul, Trapezoid ll)
    {
      this.UpperLeft = ul;
      if (ul != null)
        ul.UpperRight = this;
      this.LowerLeft = ll;
      if (ll == null)
        return;
      ll.LowerRight = this;
    }

    public void UpdateRight(Trapezoid ur, Trapezoid lr)
    {
      this.UpperRight = ur;
      if (ur != null)
        ur.UpperLeft = this;
      this.LowerRight = lr;
      if (lr == null)
        return;
      lr.LowerLeft = this;
    }

    public void UpdateLeftRight(Trapezoid ul, Trapezoid ll, Trapezoid ur, Trapezoid lr)
    {
      this.UpperLeft = ul;
      if (ul != null)
        ul.UpperRight = this;
      this.LowerLeft = ll;
      if (ll != null)
        ll.LowerRight = this;
      this.UpperRight = ur;
      if (ur != null)
        ur.UpperLeft = this;
      this.LowerRight = lr;
      if (lr == null)
        return;
      lr.LowerLeft = this;
    }

    public void TrimNeighbors()
    {
      if (!this.Inside)
        return;
      this.Inside = false;
      if (this.UpperLeft != null)
        this.UpperLeft.TrimNeighbors();
      if (this.LowerLeft != null)
        this.LowerLeft.TrimNeighbors();
      if (this.UpperRight != null)
        this.UpperRight.TrimNeighbors();
      if (this.LowerRight == null)
        return;
      this.LowerRight.TrimNeighbors();
    }

    public bool Contains(Point point)
    {
      return (double) point.X > (double) this.LeftPoint.X && (double) point.X < (double) this.RightPoint.X && this.Top.IsAbove(point) && this.Bottom.IsBelow(point);
    }

    public List<Point> Vertices()
    {
      return new List<Point>(4)
      {
        this.LineIntersect(this.Top, this.LeftPoint.X),
        this.LineIntersect(this.Bottom, this.LeftPoint.X),
        this.LineIntersect(this.Bottom, this.RightPoint.X),
        this.LineIntersect(this.Top, this.RightPoint.X)
      };
    }

    private Point LineIntersect(Edge edge, float x)
    {
      float y = edge.Slope * x + edge.B;
      return new Point(x, y);
    }

    public void AddPoints()
    {
      if (this.LeftPoint != this.Bottom.P)
        this.Bottom.AddMpoint(this.LeftPoint);
      if (this.RightPoint != this.Bottom.Q)
        this.Bottom.AddMpoint(this.RightPoint);
      if (this.LeftPoint != this.Top.P)
        this.Top.AddMpoint(this.LeftPoint);
      if (this.RightPoint == this.Top.Q)
        return;
      this.Top.AddMpoint(this.RightPoint);
    }
  }
}
