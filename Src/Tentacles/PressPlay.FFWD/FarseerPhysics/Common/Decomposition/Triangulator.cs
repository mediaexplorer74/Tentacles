﻿// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Triangulator
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class Triangulator
  {
    public List<Trapezoid> Trapezoids;
    public List<List<Point>> Triangles;
    private Trapezoid _boundingBox;
    private List<Edge> _edgeList;
    private QueryGraph _queryGraph;
    private float _sheer = 1f / 1000f;
    private TrapezoidalMap _trapezoidalMap;
    private List<MonotoneMountain> _xMonoPoly;

    public Triangulator(List<Point> polyLine, float sheer)
    {
      this._sheer = sheer;
      this.Triangles = new List<List<Point>>();
      this.Trapezoids = new List<Trapezoid>();
      this._xMonoPoly = new List<MonotoneMountain>();
      this._edgeList = this.InitEdges(polyLine);
      this._trapezoidalMap = new TrapezoidalMap();
      this._boundingBox = this._trapezoidalMap.BoundingBox(this._edgeList);
      this._queryGraph = new QueryGraph((Node) Sink.Isink(this._boundingBox));
      this.Process();
    }

    private void Process()
    {
      foreach (Edge edge in this._edgeList)
      {
        foreach (Trapezoid t in this._queryGraph.FollowEdge(edge))
        {
          this._trapezoidalMap.Map.Remove(t);
          bool flag1 = t.Contains(edge.P);
          bool flag2 = t.Contains(edge.Q);
          Trapezoid[] tList;
          if (flag1 && flag2)
          {
            tList = this._trapezoidalMap.Case1(t, edge);
            this._queryGraph.Case1(t.Sink, edge, tList);
          }
          else if (flag1 && !flag2)
          {
            tList = this._trapezoidalMap.Case2(t, edge);
            this._queryGraph.Case2(t.Sink, edge, tList);
          }
          else if (!flag1 && !flag2)
          {
            tList = this._trapezoidalMap.Case3(t, edge);
            this._queryGraph.Case3(t.Sink, edge, tList);
          }
          else
          {
            tList = this._trapezoidalMap.Case4(t, edge);
            this._queryGraph.Case4(t.Sink, edge, tList);
          }
          foreach (Trapezoid trapezoid in tList)
            this._trapezoidalMap.Map.Add(trapezoid);
        }
        this._trapezoidalMap.Clear();
      }
      foreach (Trapezoid t in this._trapezoidalMap.Map)
        this.MarkOutside(t);
      foreach (Trapezoid trapezoid in this._trapezoidalMap.Map)
      {
        if (trapezoid.Inside)
        {
          this.Trapezoids.Add(trapezoid);
          trapezoid.AddPoints();
        }
      }
      this.CreateMountains();
    }

    private void CreateMountains()
    {
      foreach (Edge edge in this._edgeList)
      {
        if (edge.MPoints.Count > 2)
        {
          MonotoneMountain monotoneMountain = new MonotoneMountain();
          List<Point> pointList = new List<Point>((IEnumerable<Point>) edge.MPoints);
          pointList.Sort((Comparison<Point>) ((p1, p2) => p1.X.CompareTo(p2.X)));
          foreach (Point point in pointList)
            monotoneMountain.Add(point);
          monotoneMountain.Process();
          foreach (List<Point> triangle in monotoneMountain.Triangles)
            this.Triangles.Add(triangle);
          this._xMonoPoly.Add(monotoneMountain);
        }
      }
    }

    private void MarkOutside(Trapezoid t)
    {
      if (t.Top != this._boundingBox.Top && t.Bottom != this._boundingBox.Bottom)
        return;
      t.TrimNeighbors();
    }

    private List<Edge> InitEdges(List<Point> points)
    {
      List<Edge> edgeInput = new List<Edge>();
      for (int index = 0; index < points.Count - 1; ++index)
        edgeInput.Add(new Edge(points[index], points[index + 1]));
      edgeInput.Add(new Edge(points[0], points[points.Count - 1]));
      return this.OrderSegments(edgeInput);
    }

    private List<Edge> OrderSegments(List<Edge> edgeInput)
    {
      List<Edge> list = new List<Edge>();
      foreach (Edge edge in edgeInput)
      {
        Point point1 = this.ShearTransform(edge.P);
        Point point2 = this.ShearTransform(edge.Q);
        if ((double) point1.X > (double) point2.X)
          list.Add(new Edge(point2, point1));
        else if ((double) point1.X < (double) point2.X)
          list.Add(new Edge(point1, point2));
      }
      Triangulator.Shuffle<Edge>((IList<Edge>) list);
      return list;
    }

    private static void Shuffle<T>(IList<T> list)
    {
      Random random = new Random();
      int count = list.Count;
      while (count > 1)
      {
        --count;
        int index = random.Next(count + 1);
        T obj = list[index];
        list[index] = list[count];
        list[count] = obj;
      }
    }

    private Point ShearTransform(Point point)
    {
      return new Point(point.X + this._sheer * point.Y, point.Y);
    }
  }
}
