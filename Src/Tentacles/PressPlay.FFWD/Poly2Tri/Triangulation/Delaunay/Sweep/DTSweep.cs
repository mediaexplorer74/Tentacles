// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.DTSweep
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common.Decomposition.CDT;
using System;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public static class DTSweep
  {
    private const double PI_div2 = 1.5707963267948966;
    private const double PI_3div4 = 2.3561944901923448;

    public static void Triangulate(DTSweepContext tcx)
    {
      tcx.CreateAdvancingFront();
      DTSweep.Sweep(tcx);
      if (tcx.TriangulationMode == TriangulationMode.Polygon)
        DTSweep.FinalizationPolygon(tcx);
      else
        DTSweep.FinalizationConvexHull(tcx);
      tcx.Done();
    }

    private static void Sweep(DTSweepContext tcx)
    {
      List<TriangulationPoint> points = tcx.Points;
      for (int index = 1; index < points.Count; ++index)
      {
        TriangulationPoint point = points[index];
        AdvancingFrontNode node = DTSweep.PointEvent(tcx, point);
        if (point.HasEdges)
        {
          foreach (DTSweepConstraint edge in point.Edges)
            DTSweep.EdgeEvent(tcx, edge, node);
        }
        tcx.Update((string) null);
      }
    }

    private static void FinalizationConvexHull(DTSweepContext tcx)
    {
      AdvancingFrontNode next1 = tcx.aFront.Head.Next;
      AdvancingFrontNode next2 = next1.Next;
      TriangulationPoint point1 = next1.Point;
      DTSweep.TurnAdvancingFrontConvex(tcx, next1, next2);
      AdvancingFrontNode prev1 = tcx.aFront.Tail.Prev;
      if (prev1.Triangle.Contains(prev1.Next.Point) && prev1.Triangle.Contains(prev1.Prev.Point))
      {
        DelaunayTriangle delaunayTriangle = prev1.Triangle.NeighborAcross(prev1.Point);
        DTSweep.RotateTrianglePair(prev1.Triangle, prev1.Point, delaunayTriangle, delaunayTriangle.OppositePoint(prev1.Triangle, prev1.Point));
        tcx.MapTriangleToNodes(prev1.Triangle);
        tcx.MapTriangleToNodes(delaunayTriangle);
      }
      AdvancingFrontNode next3 = tcx.aFront.Head.Next;
      if (next3.Triangle.Contains(next3.Prev.Point) && next3.Triangle.Contains(next3.Next.Point))
      {
        DelaunayTriangle delaunayTriangle = next3.Triangle.NeighborAcross(next3.Point);
        DTSweep.RotateTrianglePair(next3.Triangle, next3.Point, delaunayTriangle, delaunayTriangle.OppositePoint(next3.Triangle, next3.Point));
        tcx.MapTriangleToNodes(next3.Triangle);
        tcx.MapTriangleToNodes(delaunayTriangle);
      }
      TriangulationPoint point2 = tcx.aFront.Head.Point;
      AdvancingFrontNode prev2 = tcx.aFront.Tail.Prev;
      DelaunayTriangle triangle1 = prev2.Triangle;
      TriangulationPoint point3 = prev2.Point;
      prev2.Triangle = (DelaunayTriangle) null;
      while (true)
      {
        tcx.RemoveFromList(triangle1);
        point3 = triangle1.PointCCW(point3);
        if (point3 != point2)
        {
          DelaunayTriangle delaunayTriangle = triangle1.NeighborCCW(point3);
          triangle1.Clear();
          triangle1 = delaunayTriangle;
        }
        else
          break;
      }
      TriangulationPoint point4 = tcx.aFront.Head.Next.Point;
      TriangulationPoint point5 = triangle1.PointCW(tcx.aFront.Head.Point);
      DelaunayTriangle delaunayTriangle1 = triangle1.NeighborCW(tcx.aFront.Head.Point);
      triangle1.Clear();
      DelaunayTriangle triangle2 = delaunayTriangle1;
      while (point5 != point4)
      {
        tcx.RemoveFromList(triangle2);
        point5 = triangle2.PointCCW(point5);
        DelaunayTriangle delaunayTriangle2 = triangle2.NeighborCCW(point5);
        triangle2.Clear();
        triangle2 = delaunayTriangle2;
      }
      tcx.aFront.Head = tcx.aFront.Head.Next;
      tcx.aFront.Head.Prev = (AdvancingFrontNode) null;
      tcx.aFront.Tail = tcx.aFront.Tail.Prev;
      tcx.aFront.Tail.Next = (AdvancingFrontNode) null;
      tcx.FinalizeTriangulation();
    }

    private static void TurnAdvancingFrontConvex(
      DTSweepContext tcx,
      AdvancingFrontNode b,
      AdvancingFrontNode c)
    {
      AdvancingFrontNode advancingFrontNode = b;
      while (c != tcx.aFront.Tail)
      {
        if (TriangulationUtil.Orient2d(b.Point, c.Point, c.Next.Point) == Orientation.CCW)
        {
          DTSweep.Fill(tcx, c);
          c = c.Next;
        }
        else if (b != advancingFrontNode && TriangulationUtil.Orient2d(b.Prev.Point, b.Point, c.Point) == Orientation.CCW)
        {
          DTSweep.Fill(tcx, b);
          b = b.Prev;
        }
        else
        {
          b = c;
          c = c.Next;
        }
      }
    }

    private static void FinalizationPolygon(DTSweepContext tcx)
    {
      DelaunayTriangle triangle = tcx.aFront.Head.Next.Triangle;
      TriangulationPoint point = tcx.aFront.Head.Next.Point;
      while (!triangle.GetConstrainedEdgeCW(point))
        triangle = triangle.NeighborCCW(point);
      tcx.MeshClean(triangle);
    }

    private static AdvancingFrontNode PointEvent(DTSweepContext tcx, TriangulationPoint point)
    {
      AdvancingFrontNode node = tcx.LocateNode(point);
      AdvancingFrontNode advancingFrontNode = DTSweep.NewFrontTriangle(tcx, point, node);
      if (point.X <= node.Point.X + TriangulationUtil.EPSILON)
        DTSweep.Fill(tcx, node);
      tcx.AddNode(advancingFrontNode);
      DTSweep.FillAdvancingFront(tcx, advancingFrontNode);
      return advancingFrontNode;
    }

    private static AdvancingFrontNode NewFrontTriangle(
      DTSweepContext tcx,
      TriangulationPoint point,
      AdvancingFrontNode node)
    {
      DelaunayTriangle t = new DelaunayTriangle(point, node.Point, node.Next.Point);
      t.MarkNeighbor(node.Triangle);
      tcx.Triangles.Add(t);
      AdvancingFrontNode node1 = new AdvancingFrontNode(point);
      node1.Next = node.Next;
      node1.Prev = node;
      node.Next.Prev = node1;
      node.Next = node1;
      tcx.AddNode(node1);
      if (!DTSweep.Legalize(tcx, t))
        tcx.MapTriangleToNodes(t);
      return node1;
    }

    private static void EdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      try
      {
        tcx.EdgeEvent.ConstrainedEdge = edge;
        tcx.EdgeEvent.Right = edge.P.X > edge.Q.X;
        if (DTSweep.IsEdgeSideOfTriangle(node.Triangle, edge.P, edge.Q))
          return;
        DTSweep.FillEdgeEvent(tcx, edge, node);
        DTSweep.EdgeEvent(tcx, edge.P, edge.Q, node.Triangle, edge.Q);
      }
      catch (PointOnEdgeException ex)
      {
      }
    }

    private static void FillEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      if (tcx.EdgeEvent.Right)
        DTSweep.FillRightAboveEdgeEvent(tcx, edge, node);
      else
        DTSweep.FillLeftAboveEdgeEvent(tcx, edge, node);
    }

    private static void FillRightConcaveEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      DTSweep.Fill(tcx, node.Next);
      if (node.Next.Point == edge.P || TriangulationUtil.Orient2d(edge.Q, node.Next.Point, edge.P) != Orientation.CCW || TriangulationUtil.Orient2d(node.Point, node.Next.Point, node.Next.Next.Point) != Orientation.CCW)
        return;
      DTSweep.FillRightConcaveEdgeEvent(tcx, edge, node);
    }

    private static void FillRightConvexEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      if (TriangulationUtil.Orient2d(node.Next.Point, node.Next.Next.Point, node.Next.Next.Next.Point) == Orientation.CCW)
      {
        DTSweep.FillRightConcaveEdgeEvent(tcx, edge, node.Next);
      }
      else
      {
        if (TriangulationUtil.Orient2d(edge.Q, node.Next.Next.Point, edge.P) != Orientation.CCW)
          return;
        DTSweep.FillRightConvexEdgeEvent(tcx, edge, node.Next);
      }
    }

    private static void FillRightBelowEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      if (node.Point.X >= edge.P.X)
        return;
      if (TriangulationUtil.Orient2d(node.Point, node.Next.Point, node.Next.Next.Point) == Orientation.CCW)
      {
        DTSweep.FillRightConcaveEdgeEvent(tcx, edge, node);
      }
      else
      {
        DTSweep.FillRightConvexEdgeEvent(tcx, edge, node);
        DTSweep.FillRightBelowEdgeEvent(tcx, edge, node);
      }
    }

    private static void FillRightAboveEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      while (node.Next.Point.X < edge.P.X)
      {
        if (TriangulationUtil.Orient2d(edge.Q, node.Next.Point, edge.P) == Orientation.CCW)
          DTSweep.FillRightBelowEdgeEvent(tcx, edge, node);
        else
          node = node.Next;
      }
    }

    private static void FillLeftConvexEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      if (TriangulationUtil.Orient2d(node.Prev.Point, node.Prev.Prev.Point, node.Prev.Prev.Prev.Point) == Orientation.CW)
      {
        DTSweep.FillLeftConcaveEdgeEvent(tcx, edge, node.Prev);
      }
      else
      {
        if (TriangulationUtil.Orient2d(edge.Q, node.Prev.Prev.Point, edge.P) != Orientation.CW)
          return;
        DTSweep.FillLeftConvexEdgeEvent(tcx, edge, node.Prev);
      }
    }

    private static void FillLeftConcaveEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      DTSweep.Fill(tcx, node.Prev);
      if (node.Prev.Point == edge.P || TriangulationUtil.Orient2d(edge.Q, node.Prev.Point, edge.P) != Orientation.CW || TriangulationUtil.Orient2d(node.Point, node.Prev.Point, node.Prev.Prev.Point) != Orientation.CW)
        return;
      DTSweep.FillLeftConcaveEdgeEvent(tcx, edge, node);
    }

    private static void FillLeftBelowEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      if (node.Point.X <= edge.P.X)
        return;
      if (TriangulationUtil.Orient2d(node.Point, node.Prev.Point, node.Prev.Prev.Point) == Orientation.CW)
      {
        DTSweep.FillLeftConcaveEdgeEvent(tcx, edge, node);
      }
      else
      {
        DTSweep.FillLeftConvexEdgeEvent(tcx, edge, node);
        DTSweep.FillLeftBelowEdgeEvent(tcx, edge, node);
      }
    }

    private static void FillLeftAboveEdgeEvent(
      DTSweepContext tcx,
      DTSweepConstraint edge,
      AdvancingFrontNode node)
    {
      while (node.Prev.Point.X > edge.P.X)
      {
        if (TriangulationUtil.Orient2d(edge.Q, node.Prev.Point, edge.P) == Orientation.CW)
          DTSweep.FillLeftBelowEdgeEvent(tcx, edge, node);
        else
          node = node.Prev;
      }
    }

    private static bool IsEdgeSideOfTriangle(
      DelaunayTriangle triangle,
      TriangulationPoint ep,
      TriangulationPoint eq)
    {
      int index = triangle.EdgeIndex(ep, eq);
      if (index == -1)
        return false;
      triangle.MarkConstrainedEdge(index);
      triangle = triangle.Neighbors[index];
      triangle?.MarkConstrainedEdge(ep, eq);
      return true;
    }

    private static void EdgeEvent(
      DTSweepContext tcx,
      TriangulationPoint ep,
      TriangulationPoint eq,
      DelaunayTriangle triangle,
      TriangulationPoint point)
    {
      if (DTSweep.IsEdgeSideOfTriangle(triangle, ep, eq))
        return;
      TriangulationPoint triangulationPoint1 = triangle.PointCCW(point);
      Orientation orientation1 = TriangulationUtil.Orient2d(eq, triangulationPoint1, ep);
      if (orientation1 == Orientation.Collinear)
      {
        if (!triangle.Contains(eq, triangulationPoint1))
          throw new PointOnEdgeException("EdgeEvent - Point on constrained edge not supported yet");
        triangle.MarkConstrainedEdge(eq, triangulationPoint1);
        tcx.EdgeEvent.ConstrainedEdge.Q = triangulationPoint1;
        triangle = triangle.NeighborAcross(point);
        DTSweep.EdgeEvent(tcx, ep, triangulationPoint1, triangle, triangulationPoint1);
        int num = tcx.IsDebugEnabled ? 1 : 0;
      }
      else
      {
        TriangulationPoint triangulationPoint2 = triangle.PointCW(point);
        Orientation orientation2 = TriangulationUtil.Orient2d(eq, triangulationPoint2, ep);
        if (orientation2 == Orientation.Collinear)
        {
          if (!triangle.Contains(eq, triangulationPoint2))
            throw new PointOnEdgeException("EdgeEvent - Point on constrained edge not supported yet");
          triangle.MarkConstrainedEdge(eq, triangulationPoint2);
          tcx.EdgeEvent.ConstrainedEdge.Q = triangulationPoint2;
          triangle = triangle.NeighborAcross(point);
          DTSweep.EdgeEvent(tcx, ep, triangulationPoint2, triangle, triangulationPoint2);
          int num = tcx.IsDebugEnabled ? 1 : 0;
        }
        else if (orientation1 == orientation2)
        {
          triangle = orientation1 != Orientation.CW ? triangle.NeighborCW(point) : triangle.NeighborCCW(point);
          DTSweep.EdgeEvent(tcx, ep, eq, triangle, point);
        }
        else
          DTSweep.FlipEdgeEvent(tcx, ep, eq, triangle, point);
      }
    }

    private static void FlipEdgeEvent(
      DTSweepContext tcx,
      TriangulationPoint ep,
      TriangulationPoint eq,
      DelaunayTriangle t,
      TriangulationPoint p)
    {
      DelaunayTriangle delaunayTriangle = t.NeighborAcross(p);
      TriangulationPoint triangulationPoint = delaunayTriangle.OppositePoint(t, p);
      if (delaunayTriangle == null)
        throw new InvalidOperationException("[BUG:FIXME] FLIP failed due to missing triangle");
      if (TriangulationUtil.InScanArea(p, t.PointCCW(p), t.PointCW(p), triangulationPoint))
      {
        DTSweep.RotateTrianglePair(t, p, delaunayTriangle, triangulationPoint);
        tcx.MapTriangleToNodes(t);
        tcx.MapTriangleToNodes(delaunayTriangle);
        if (p == eq && triangulationPoint == ep)
        {
          if (eq == tcx.EdgeEvent.ConstrainedEdge.Q && ep == tcx.EdgeEvent.ConstrainedEdge.P)
          {
            if (tcx.IsDebugEnabled)
              Console.WriteLine("[FLIP] - constrained edge done");
            t.MarkConstrainedEdge(ep, eq);
            delaunayTriangle.MarkConstrainedEdge(ep, eq);
            DTSweep.Legalize(tcx, t);
            DTSweep.Legalize(tcx, delaunayTriangle);
          }
          else
          {
            if (!tcx.IsDebugEnabled)
              return;
            Console.WriteLine("[FLIP] - subedge done");
          }
        }
        else
        {
          if (tcx.IsDebugEnabled)
            Console.WriteLine("[FLIP] - flipping and continuing with triangle still crossing edge");
          Orientation o = TriangulationUtil.Orient2d(eq, triangulationPoint, ep);
          t = DTSweep.NextFlipTriangle(tcx, o, t, delaunayTriangle, p, triangulationPoint);
          DTSweep.FlipEdgeEvent(tcx, ep, eq, t, p);
        }
      }
      else
      {
        TriangulationPoint p1 = DTSweep.NextFlipPoint(ep, eq, delaunayTriangle, triangulationPoint);
        DTSweep.FlipScanEdgeEvent(tcx, ep, eq, t, delaunayTriangle, p1);
        DTSweep.EdgeEvent(tcx, ep, eq, t, p);
      }
    }

    private static TriangulationPoint NextFlipPoint(
      TriangulationPoint ep,
      TriangulationPoint eq,
      DelaunayTriangle ot,
      TriangulationPoint op)
    {
      switch (TriangulationUtil.Orient2d(eq, op, ep))
      {
        case Orientation.CW:
          return ot.PointCCW(op);
        case Orientation.CCW:
          return ot.PointCW(op);
        default:
          throw new PointOnEdgeException("Point on constrained edge not supported yet");
      }
    }

    private static DelaunayTriangle NextFlipTriangle(
      DTSweepContext tcx,
      Orientation o,
      DelaunayTriangle t,
      DelaunayTriangle ot,
      TriangulationPoint p,
      TriangulationPoint op)
    {
      if (o == Orientation.CCW)
      {
        int index = ot.EdgeIndex(p, op);
        ot.EdgeIsDelaunay[index] = true;
        DTSweep.Legalize(tcx, ot);
        ot.EdgeIsDelaunay.Clear();
        return t;
      }
      int index1 = t.EdgeIndex(p, op);
      t.EdgeIsDelaunay[index1] = true;
      DTSweep.Legalize(tcx, t);
      t.EdgeIsDelaunay.Clear();
      return ot;
    }

    private static void FlipScanEdgeEvent(
      DTSweepContext tcx,
      TriangulationPoint ep,
      TriangulationPoint eq,
      DelaunayTriangle flipTriangle,
      DelaunayTriangle t,
      TriangulationPoint p)
    {
      DelaunayTriangle delaunayTriangle = t.NeighborAcross(p);
      TriangulationPoint triangulationPoint = delaunayTriangle.OppositePoint(t, p);
      if (delaunayTriangle == null)
        throw new Exception("[BUG:FIXME] FLIP failed due to missing triangle");
      if (TriangulationUtil.InScanArea(eq, flipTriangle.PointCCW(eq), flipTriangle.PointCW(eq), triangulationPoint))
      {
        DTSweep.FlipEdgeEvent(tcx, eq, triangulationPoint, delaunayTriangle, triangulationPoint);
      }
      else
      {
        TriangulationPoint p1 = DTSweep.NextFlipPoint(ep, eq, delaunayTriangle, triangulationPoint);
        DTSweep.FlipScanEdgeEvent(tcx, ep, eq, flipTriangle, delaunayTriangle, p1);
      }
    }

    private static void FillAdvancingFront(DTSweepContext tcx, AdvancingFrontNode n)
    {
      for (AdvancingFrontNode next = n.Next; next.HasNext; next = next.Next)
      {
        double num = DTSweep.HoleAngle(next);
        if (num <= Math.PI / 2.0 && num >= -1.0 * Math.PI / 2.0)
          DTSweep.Fill(tcx, next);
        else
          break;
      }
      for (AdvancingFrontNode prev = n.Prev; prev.HasPrev; prev = prev.Prev)
      {
        double num = DTSweep.HoleAngle(prev);
        if (num <= Math.PI / 2.0 && num >= -1.0 * Math.PI / 2.0)
          DTSweep.Fill(tcx, prev);
        else
          break;
      }
      if (!n.HasNext || !n.Next.HasNext || DTSweep.BasinAngle(n) >= 3.0 * Math.PI / 4.0)
        return;
      DTSweep.FillBasin(tcx, n);
    }

    private static void FillBasin(DTSweepContext tcx, AdvancingFrontNode node)
    {
      tcx.Basin.leftNode = TriangulationUtil.Orient2d(node.Point, node.Next.Point, node.Next.Next.Point) != Orientation.CCW ? node.Next : node;
      tcx.Basin.bottomNode = tcx.Basin.leftNode;
      while (tcx.Basin.bottomNode.HasNext && tcx.Basin.bottomNode.Point.Y >= tcx.Basin.bottomNode.Next.Point.Y)
        tcx.Basin.bottomNode = tcx.Basin.bottomNode.Next;
      if (tcx.Basin.bottomNode == tcx.Basin.leftNode)
        return;
      tcx.Basin.rightNode = tcx.Basin.bottomNode;
      while (tcx.Basin.rightNode.HasNext && tcx.Basin.rightNode.Point.Y < tcx.Basin.rightNode.Next.Point.Y)
        tcx.Basin.rightNode = tcx.Basin.rightNode.Next;
      if (tcx.Basin.rightNode == tcx.Basin.bottomNode)
        return;
      tcx.Basin.width = tcx.Basin.rightNode.Point.X - tcx.Basin.leftNode.Point.X;
      tcx.Basin.leftHighest = tcx.Basin.leftNode.Point.Y > tcx.Basin.rightNode.Point.Y;
      DTSweep.FillBasinReq(tcx, tcx.Basin.bottomNode);
    }

    private static void FillBasinReq(DTSweepContext tcx, AdvancingFrontNode node)
    {
      if (DTSweep.IsShallow(tcx, node))
        return;
      DTSweep.Fill(tcx, node);
      if (node.Prev == tcx.Basin.leftNode && node.Next == tcx.Basin.rightNode)
        return;
      if (node.Prev == tcx.Basin.leftNode)
      {
        if (TriangulationUtil.Orient2d(node.Point, node.Next.Point, node.Next.Next.Point) == Orientation.CW)
          return;
        node = node.Next;
      }
      else if (node.Next == tcx.Basin.rightNode)
      {
        if (TriangulationUtil.Orient2d(node.Point, node.Prev.Point, node.Prev.Prev.Point) == Orientation.CCW)
          return;
        node = node.Prev;
      }
      else
        node = node.Prev.Point.Y >= node.Next.Point.Y ? node.Next : node.Prev;
      DTSweep.FillBasinReq(tcx, node);
    }

    private static bool IsShallow(DTSweepContext tcx, AdvancingFrontNode node)
    {
      double num = !tcx.Basin.leftHighest ? tcx.Basin.rightNode.Point.Y - node.Point.Y : tcx.Basin.leftNode.Point.Y - node.Point.Y;
      return tcx.Basin.width > num;
    }

    private static double HoleAngle(AdvancingFrontNode node)
    {
      double x = node.Point.X;
      double y = node.Point.Y;
      double num1 = node.Next.Point.X - x;
      double num2 = node.Next.Point.Y - y;
      double num3 = node.Prev.Point.X - x;
      double num4 = node.Prev.Point.Y - y;
      return Math.Atan2(num1 * num4 - num2 * num3, num1 * num3 + num2 * num4);
    }

    private static double BasinAngle(AdvancingFrontNode node)
    {
      double x = node.Point.X - node.Next.Next.Point.X;
      return Math.Atan2(node.Point.Y - node.Next.Next.Point.Y, x);
    }

    private static void Fill(DTSweepContext tcx, AdvancingFrontNode node)
    {
      DelaunayTriangle t = new DelaunayTriangle(node.Prev.Point, node.Point, node.Next.Point);
      t.MarkNeighbor(node.Prev.Triangle);
      t.MarkNeighbor(node.Triangle);
      tcx.Triangles.Add(t);
      node.Prev.Next = node.Next;
      node.Next.Prev = node.Prev;
      tcx.RemoveNode(node);
      if (DTSweep.Legalize(tcx, t))
        return;
      tcx.MapTriangleToNodes(t);
    }

    private static bool Legalize(DTSweepContext tcx, DelaunayTriangle t)
    {
      for (int index1 = 0; index1 < 3; ++index1)
      {
        if (!t.EdgeIsDelaunay[index1])
        {
          DelaunayTriangle neighbor = t.Neighbors[index1];
          if (neighbor != null)
          {
            TriangulationPoint point = t.Points[index1];
            TriangulationPoint triangulationPoint = neighbor.OppositePoint(t, point);
            int index2 = neighbor.IndexOf(triangulationPoint);
            if (neighbor.EdgeIsConstrained[index2] || neighbor.EdgeIsDelaunay[index2])
              t.EdgeIsConstrained[index1] = neighbor.EdgeIsConstrained[index2];
            else if (TriangulationUtil.SmartIncircle(point, t.PointCCW(point), t.PointCW(point), triangulationPoint))
            {
              t.EdgeIsDelaunay[index1] = true;
              neighbor.EdgeIsDelaunay[index2] = true;
              DTSweep.RotateTrianglePair(t, point, neighbor, triangulationPoint);
              if (!DTSweep.Legalize(tcx, t))
                tcx.MapTriangleToNodes(t);
              if (!DTSweep.Legalize(tcx, neighbor))
                tcx.MapTriangleToNodes(neighbor);
              t.EdgeIsDelaunay[index1] = false;
              neighbor.EdgeIsDelaunay[index2] = false;
              return true;
            }
          }
        }
      }
      return false;
    }

    private static void RotateTrianglePair(
      DelaunayTriangle t,
      TriangulationPoint p,
      DelaunayTriangle ot,
      TriangulationPoint op)
    {
      DelaunayTriangle t1 = t.NeighborCCW(p);
      DelaunayTriangle t2 = t.NeighborCW(p);
      DelaunayTriangle t3 = ot.NeighborCCW(op);
      DelaunayTriangle t4 = ot.NeighborCW(op);
      bool constrainedEdgeCcw1 = t.GetConstrainedEdgeCCW(p);
      bool constrainedEdgeCw1 = t.GetConstrainedEdgeCW(p);
      bool constrainedEdgeCcw2 = ot.GetConstrainedEdgeCCW(op);
      bool constrainedEdgeCw2 = ot.GetConstrainedEdgeCW(op);
      bool delaunayEdgeCcw1 = t.GetDelaunayEdgeCCW(p);
      bool delaunayEdgeCw1 = t.GetDelaunayEdgeCW(p);
      bool delaunayEdgeCcw2 = ot.GetDelaunayEdgeCCW(op);
      bool delaunayEdgeCw2 = ot.GetDelaunayEdgeCW(op);
      t.Legalize(p, op);
      ot.Legalize(op, p);
      ot.SetDelaunayEdgeCCW(p, delaunayEdgeCcw1);
      t.SetDelaunayEdgeCW(p, delaunayEdgeCw1);
      t.SetDelaunayEdgeCCW(op, delaunayEdgeCcw2);
      ot.SetDelaunayEdgeCW(op, delaunayEdgeCw2);
      ot.SetConstrainedEdgeCCW(p, constrainedEdgeCcw1);
      t.SetConstrainedEdgeCW(p, constrainedEdgeCw1);
      t.SetConstrainedEdgeCCW(op, constrainedEdgeCcw2);
      ot.SetConstrainedEdgeCW(op, constrainedEdgeCw2);
      t.Neighbors.Clear();
      ot.Neighbors.Clear();
      if (t1 != null)
        ot.MarkNeighbor(t1);
      if (t2 != null)
        t.MarkNeighbor(t2);
      if (t3 != null)
        t.MarkNeighbor(t3);
      if (t4 != null)
        ot.MarkNeighbor(t4);
      t.MarkNeighbor(ot);
    }
  }
}
