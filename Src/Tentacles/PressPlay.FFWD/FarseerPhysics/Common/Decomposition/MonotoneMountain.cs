// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.MonotoneMountain
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class MonotoneMountain
  {
    private const float PiSlop = 3.1f;
    public List<List<Point>> Triangles;
    private FarseerPhysics.Common.HashSet<Point> _convexPoints;
    private Point _head;
    private List<Point> _monoPoly;
    private bool _positive;
    private int _size;
    private Point _tail;

    public MonotoneMountain()
    {
      this._size = 0;
      this._tail = (Point) null;
      this._head = (Point) null;
      this._positive = false;
      this._convexPoints = new FarseerPhysics.Common.HashSet<Point>();
      this._monoPoly = new List<Point>();
      this.Triangles = new List<List<Point>>();
    }

    public void Add(Point point)
    {
      if (this._size == 0)
      {
        this._head = point;
        this._size = 1;
      }
      else if (this._size == 1)
      {
        this._tail = point;
        this._tail.Prev = this._head;
        this._head.Next = this._tail;
        this._size = 2;
      }
      else
      {
        this._tail.Next = point;
        point.Prev = this._tail;
        this._tail = point;
        ++this._size;
      }
    }

    public void Remove(Point point)
    {
      Point next = point.Next;
      Point prev = point.Prev;
      point.Prev.Next = next;
      point.Next.Prev = prev;
      --this._size;
    }

    public void Process()
    {
      this._positive = this.AngleSign();
      this.GenMonoPoly();
      for (Point next = this._head.Next; next.Neq(this._tail); next = next.Next)
      {
        float num = this.Angle(next);
        if ((double) num >= 3.0999999046325684 || (double) num <= -3.0999999046325684 || (double) num == 0.0)
          this.Remove(next);
        else if (this.IsConvex(next))
          this._convexPoints.Add(next);
      }
      this.Triangulate();
    }

    private void Triangulate()
    {
      while (this._convexPoints.Count != 0)
      {
        IEnumerator<Point> enumerator = this._convexPoints.GetEnumerator();
        enumerator.MoveNext();
        Point current = enumerator.Current;
        this._convexPoints.Remove(current);
        Point prev = current.Prev;
        Point point = current;
        Point next = current.Next;
        this.Triangles.Add(new List<Point>(3)
        {
          prev,
          point,
          next
        });
        this.Remove(current);
        if (this.Valid(prev))
          this._convexPoints.Add(prev);
        if (this.Valid(next))
          this._convexPoints.Add(next);
      }
    }

    private bool Valid(Point p) => p.Neq(this._head) && p.Neq(this._tail) && this.IsConvex(p);

    private void GenMonoPoly()
    {
      for (Point point = this._head; point != null; point = point.Next)
        this._monoPoly.Add(point);
    }

    private float Angle(Point p)
    {
      Point point = p.Next - p;
      Point p1 = p.Prev - p;
      return (float) Math.Atan2((double) point.Cross(p1), (double) point.Dot(p1));
    }

    private bool AngleSign()
    {
      Point point = this._head.Next - this._head;
      Point p = this._tail - this._head;
      return Math.Atan2((double) point.Cross(p), (double) point.Dot(p)) >= 0.0;
    }

    private bool IsConvex(Point p) => this._positive == (double) this.Angle(p) >= 0.0;
  }
}
