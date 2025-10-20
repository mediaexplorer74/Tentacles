// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.DTSweepContext
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public class DTSweepContext : TriangulationContext
  {
    private const float ALPHA = 0.3f;
    public DTSweepContext.DTSweepBasin Basin = new DTSweepContext.DTSweepBasin();
    public DTSweepContext.DTSweepEdgeEvent EdgeEvent = new DTSweepContext.DTSweepEdgeEvent();
    private DTSweepPointComparator _comparator = new DTSweepPointComparator();
    public AdvancingFront aFront;

    public DTSweepContext() => this.Clear();

    public TriangulationPoint Head { get; set; }

    public TriangulationPoint Tail { get; set; }

    public void RemoveFromList(DelaunayTriangle triangle) => this.Triangles.Remove(triangle);

    public void MeshClean(DelaunayTriangle triangle) => this.MeshCleanReq(triangle);

    private void MeshCleanReq(DelaunayTriangle triangle)
    {
      if (triangle == null || triangle.IsInterior)
        return;
      triangle.IsInterior = true;
      this.Triangulatable.AddTriangle(triangle);
      for (int index = 0; index < 3; ++index)
      {
        if (!triangle.EdgeIsConstrained[index])
          this.MeshCleanReq(triangle.Neighbors[index]);
      }
    }

    public override void Clear()
    {
      base.Clear();
      this.Triangles.Clear();
    }

    public void AddNode(AdvancingFrontNode node) => this.aFront.AddNode(node);

    public void RemoveNode(AdvancingFrontNode node) => this.aFront.RemoveNode(node);

    public AdvancingFrontNode LocateNode(TriangulationPoint point) => this.aFront.LocateNode(point);

    public void CreateAdvancingFront()
    {
      DelaunayTriangle delaunayTriangle = new DelaunayTriangle(this.Points[0], this.Tail, this.Head);
      this.Triangles.Add(delaunayTriangle);
      AdvancingFrontNode head = new AdvancingFrontNode(delaunayTriangle.Points[1]);
      head.Triangle = delaunayTriangle;
      AdvancingFrontNode node = new AdvancingFrontNode(delaunayTriangle.Points[0]);
      node.Triangle = delaunayTriangle;
      AdvancingFrontNode tail = new AdvancingFrontNode(delaunayTriangle.Points[2]);
      this.aFront = new AdvancingFront(head, tail);
      this.aFront.AddNode(node);
      this.aFront.Head.Next = node;
      node.Next = this.aFront.Tail;
      node.Prev = this.aFront.Head;
      this.aFront.Tail.Prev = node;
    }

    public void MapTriangleToNodes(DelaunayTriangle t)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (t.Neighbors[index] == null)
        {
          AdvancingFrontNode advancingFrontNode = this.aFront.LocatePoint(t.PointCW(t.Points[index]));
          if (advancingFrontNode != null)
            advancingFrontNode.Triangle = t;
        }
      }
    }

    public override void PrepareTriangulation(Triangulatable t)
    {
      base.PrepareTriangulation(t);
      double x;
      double num1 = x = this.Points[0].X;
      double y;
      double num2 = y = this.Points[0].Y;
      foreach (TriangulationPoint point in this.Points)
      {
        if (point.X > num1)
          num1 = point.X;
        if (point.X < x)
          x = point.X;
        if (point.Y > num2)
          num2 = point.Y;
        if (point.Y < y)
          y = point.Y;
      }
      double num3 = 0.30000001192092896 * (num1 - x);
      double num4 = 0.30000001192092896 * (num2 - y);
      TriangulationPoint triangulationPoint1 = new TriangulationPoint(num1 + num3, y - num4);
      TriangulationPoint triangulationPoint2 = new TriangulationPoint(x - num3, y - num4);
      this.Head = triangulationPoint1;
      this.Tail = triangulationPoint2;
      this.Points.Sort((IComparer<TriangulationPoint>) this._comparator);
    }

    public void FinalizeTriangulation()
    {
      this.Triangulatable.AddTriangles((IEnumerable<DelaunayTriangle>) this.Triangles);
      this.Triangles.Clear();
    }

    public override TriangulationConstraint NewConstraint(
      TriangulationPoint a,
      TriangulationPoint b)
    {
      return (TriangulationConstraint) new DTSweepConstraint(a, b);
    }

    public class DTSweepBasin
    {
      public AdvancingFrontNode bottomNode;
      public bool leftHighest;
      public AdvancingFrontNode leftNode;
      public AdvancingFrontNode rightNode;
      public double width;
    }

    public class DTSweepEdgeEvent
    {
      public DTSweepConstraint ConstrainedEdge;
      public bool Right;
    }
  }
}
