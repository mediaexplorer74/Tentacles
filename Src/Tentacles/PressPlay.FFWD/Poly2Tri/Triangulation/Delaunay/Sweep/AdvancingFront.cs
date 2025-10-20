// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Delaunay.Sweep.AdvancingFront
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Text;

#nullable disable
namespace Poly2Tri.Triangulation.Delaunay.Sweep
{
  public class AdvancingFront
  {
    public AdvancingFrontNode Head;
    protected AdvancingFrontNode Search;
    public AdvancingFrontNode Tail;

    public AdvancingFront(AdvancingFrontNode head, AdvancingFrontNode tail)
    {
      this.Head = head;
      this.Tail = tail;
      this.Search = head;
      this.AddNode(head);
      this.AddNode(tail);
    }

    public void AddNode(AdvancingFrontNode node)
    {
    }

    public void RemoveNode(AdvancingFrontNode node)
    {
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (AdvancingFrontNode advancingFrontNode = this.Head; advancingFrontNode != this.Tail; advancingFrontNode = advancingFrontNode.Next)
        stringBuilder.Append(advancingFrontNode.Point.X).Append("->");
      stringBuilder.Append(this.Tail.Point.X);
      return stringBuilder.ToString();
    }

    private AdvancingFrontNode FindSearchNode(double x) => this.Search;

    public AdvancingFrontNode LocateNode(TriangulationPoint point) => this.LocateNode(point.X);

    private AdvancingFrontNode LocateNode(double x)
    {
      AdvancingFrontNode advancingFrontNode = this.FindSearchNode(x);
      if (x < advancingFrontNode.Value)
      {
        while ((advancingFrontNode = advancingFrontNode.Prev) != null)
        {
          if (x >= advancingFrontNode.Value)
          {
            this.Search = advancingFrontNode;
            return advancingFrontNode;
          }
        }
      }
      else
      {
        while ((advancingFrontNode = advancingFrontNode.Next) != null)
        {
          if (x < advancingFrontNode.Value)
          {
            this.Search = advancingFrontNode.Prev;
            return advancingFrontNode.Prev;
          }
        }
      }
      return (AdvancingFrontNode) null;
    }

    public AdvancingFrontNode LocatePoint(TriangulationPoint point)
    {
      double x1 = point.X;
      AdvancingFrontNode advancingFrontNode = this.FindSearchNode(x1);
      double x2 = advancingFrontNode.Point.X;
      if (x1 == x2)
      {
        if (point != advancingFrontNode.Point)
        {
          if (point == advancingFrontNode.Prev.Point)
          {
            advancingFrontNode = advancingFrontNode.Prev;
          }
          else
          {
            if (point != advancingFrontNode.Next.Point)
              throw new Exception("Failed to find Node for given afront point");
            advancingFrontNode = advancingFrontNode.Next;
          }
        }
      }
      else if (x1 < x2)
      {
        while ((advancingFrontNode = advancingFrontNode.Prev) != null && point != advancingFrontNode.Point)
          ;
      }
      else
      {
        while ((advancingFrontNode = advancingFrontNode.Next) != null && point != advancingFrontNode.Point)
          ;
      }
      this.Search = advancingFrontNode;
      return advancingFrontNode;
    }
  }
}
