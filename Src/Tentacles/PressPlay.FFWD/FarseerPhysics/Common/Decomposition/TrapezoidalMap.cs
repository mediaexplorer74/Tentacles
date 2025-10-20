// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.TrapezoidalMap
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class TrapezoidalMap
  {
    public FarseerPhysics.Common.HashSet<Trapezoid> Map;
    private Edge _bCross;
    private Edge _cross;
    private float _margin;

    public TrapezoidalMap()
    {
      this.Map = new FarseerPhysics.Common.HashSet<Trapezoid>();
      this._margin = 50f;
      this._bCross = (Edge) null;
      this._cross = (Edge) null;
    }

    public void Clear()
    {
      this._bCross = (Edge) null;
      this._cross = (Edge) null;
    }

    public Trapezoid[] Case1(Trapezoid t, Edge e)
    {
      Trapezoid[] trapezoidArray = new Trapezoid[4]
      {
        new Trapezoid(t.LeftPoint, e.P, t.Top, t.Bottom),
        new Trapezoid(e.P, e.Q, t.Top, e),
        new Trapezoid(e.P, e.Q, e, t.Bottom),
        new Trapezoid(e.Q, t.RightPoint, t.Top, t.Bottom)
      };
      trapezoidArray[0].UpdateLeft(t.UpperLeft, t.LowerLeft);
      trapezoidArray[1].UpdateLeftRight(trapezoidArray[0], (Trapezoid) null, trapezoidArray[3], (Trapezoid) null);
      trapezoidArray[2].UpdateLeftRight((Trapezoid) null, trapezoidArray[0], (Trapezoid) null, trapezoidArray[3]);
      trapezoidArray[3].UpdateRight(t.UpperRight, t.LowerRight);
      return trapezoidArray;
    }

    public Trapezoid[] Case2(Trapezoid t, Edge e)
    {
      Point rightPoint = (double) e.Q.X != (double) t.RightPoint.X ? t.RightPoint : e.Q;
      Trapezoid[] trapezoidArray = new Trapezoid[3]
      {
        new Trapezoid(t.LeftPoint, e.P, t.Top, t.Bottom),
        new Trapezoid(e.P, rightPoint, t.Top, e),
        new Trapezoid(e.P, rightPoint, e, t.Bottom)
      };
      trapezoidArray[0].UpdateLeft(t.UpperLeft, t.LowerLeft);
      trapezoidArray[1].UpdateLeftRight(trapezoidArray[0], (Trapezoid) null, t.UpperRight, (Trapezoid) null);
      trapezoidArray[2].UpdateLeftRight((Trapezoid) null, trapezoidArray[0], (Trapezoid) null, t.LowerRight);
      this._bCross = t.Bottom;
      this._cross = t.Top;
      e.Above = trapezoidArray[1];
      e.Below = trapezoidArray[2];
      return trapezoidArray;
    }

    public Trapezoid[] Case3(Trapezoid t, Edge e)
    {
      Point leftPoint = (double) e.P.X != (double) t.LeftPoint.X ? t.LeftPoint : e.P;
      Point rightPoint = (double) e.Q.X != (double) t.RightPoint.X ? t.RightPoint : e.Q;
      Trapezoid[] trapezoidArray = new Trapezoid[2];
      if (this._cross == t.Top)
      {
        trapezoidArray[0] = t.UpperLeft;
        trapezoidArray[0].UpdateRight(t.UpperRight, (Trapezoid) null);
        trapezoidArray[0].RightPoint = rightPoint;
      }
      else
      {
        trapezoidArray[0] = new Trapezoid(leftPoint, rightPoint, t.Top, e);
        trapezoidArray[0].UpdateLeftRight(t.UpperLeft, e.Above, t.UpperRight, (Trapezoid) null);
      }
      if (this._bCross == t.Bottom)
      {
        trapezoidArray[1] = t.LowerLeft;
        trapezoidArray[1].UpdateRight((Trapezoid) null, t.LowerRight);
        trapezoidArray[1].RightPoint = rightPoint;
      }
      else
      {
        trapezoidArray[1] = new Trapezoid(leftPoint, rightPoint, e, t.Bottom);
        trapezoidArray[1].UpdateLeftRight(e.Below, t.LowerLeft, (Trapezoid) null, t.LowerRight);
      }
      this._bCross = t.Bottom;
      this._cross = t.Top;
      e.Above = trapezoidArray[0];
      e.Below = trapezoidArray[1];
      return trapezoidArray;
    }

    public Trapezoid[] Case4(Trapezoid t, Edge e)
    {
      Point leftPoint = (double) e.P.X != (double) t.LeftPoint.X ? t.LeftPoint : e.P;
      Trapezoid[] trapezoidArray = new Trapezoid[3];
      if (this._cross == t.Top)
      {
        trapezoidArray[0] = t.UpperLeft;
        trapezoidArray[0].RightPoint = e.Q;
      }
      else
      {
        trapezoidArray[0] = new Trapezoid(leftPoint, e.Q, t.Top, e);
        trapezoidArray[0].UpdateLeft(t.UpperLeft, e.Above);
      }
      if (this._bCross == t.Bottom)
      {
        trapezoidArray[1] = t.LowerLeft;
        trapezoidArray[1].RightPoint = e.Q;
      }
      else
      {
        trapezoidArray[1] = new Trapezoid(leftPoint, e.Q, e, t.Bottom);
        trapezoidArray[1].UpdateLeft(e.Below, t.LowerLeft);
      }
      trapezoidArray[2] = new Trapezoid(e.Q, t.RightPoint, t.Top, t.Bottom);
      trapezoidArray[2].UpdateLeftRight(trapezoidArray[0], trapezoidArray[1], t.UpperRight, t.LowerRight);
      return trapezoidArray;
    }

    public Trapezoid BoundingBox(List<Edge> edges)
    {
      Point point1 = edges[0].P + this._margin;
      Point point2 = edges[0].Q - this._margin;
      foreach (Edge edge in edges)
      {
        if ((double) edge.P.X > (double) point1.X)
          point1 = new Point(edge.P.X + this._margin, point1.Y);
        if ((double) edge.P.Y > (double) point1.Y)
          point1 = new Point(point1.X, edge.P.Y + this._margin);
        if ((double) edge.Q.X > (double) point1.X)
          point1 = new Point(edge.Q.X + this._margin, point1.Y);
        if ((double) edge.Q.Y > (double) point1.Y)
          point1 = new Point(point1.X, edge.Q.Y + this._margin);
        if ((double) edge.P.X < (double) point2.X)
          point2 = new Point(edge.P.X - this._margin, point2.Y);
        if ((double) edge.P.Y < (double) point2.Y)
          point2 = new Point(point2.X, edge.P.Y - this._margin);
        if ((double) edge.Q.X < (double) point2.X)
          point2 = new Point(edge.Q.X - this._margin, point2.Y);
        if ((double) edge.Q.Y < (double) point2.Y)
          point2 = new Point(point2.X, edge.Q.Y - this._margin);
      }
      Edge top = new Edge(new Point(point2.X, point1.Y), new Point(point1.X, point1.Y));
      Edge bottom = new Edge(new Point(point2.X, point2.Y), new Point(point1.X, point2.Y));
      return new Trapezoid(bottom.P, top.Q, top, bottom);
    }
  }
}
