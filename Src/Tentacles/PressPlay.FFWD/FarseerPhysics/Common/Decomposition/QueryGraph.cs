// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.QueryGraph
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class QueryGraph
  {
    private Node _head;

    public QueryGraph(Node head) => this._head = head;

    private Trapezoid Locate(Edge edge) => this._head.Locate(edge).Trapezoid;

    public List<Trapezoid> FollowEdge(Edge edge)
    {
      List<Trapezoid> trapezoidList = new List<Trapezoid>();
      trapezoidList.Add(this.Locate(edge));
      for (int index = 0; (double) edge.Q.X > (double) trapezoidList[index].RightPoint.X; ++index)
      {
        if (edge.IsAbove(trapezoidList[index].RightPoint))
          trapezoidList.Add(trapezoidList[index].UpperRight);
        else
          trapezoidList.Add(trapezoidList[index].LowerRight);
      }
      return trapezoidList;
    }

    private void Replace(Sink sink, Node node)
    {
      if (sink.ParentList.Count == 0)
        this._head = node;
      else
        node.Replace((Node) sink);
    }

    public void Case1(Sink sink, Edge edge, Trapezoid[] tList)
    {
      YNode lChild = new YNode(edge, (Node) Sink.Isink(tList[1]), (Node) Sink.Isink(tList[2]));
      XNode rChild = new XNode(edge.Q, (Node) lChild, (Node) Sink.Isink(tList[3]));
      XNode xnode = new XNode(edge.P, (Node) Sink.Isink(tList[0]), (Node) rChild);
      this.Replace(sink, (Node) xnode);
    }

    public void Case2(Sink sink, Edge edge, Trapezoid[] tList)
    {
      YNode rChild = new YNode(edge, (Node) Sink.Isink(tList[1]), (Node) Sink.Isink(tList[2]));
      XNode xnode = new XNode(edge.P, (Node) Sink.Isink(tList[0]), (Node) rChild);
      this.Replace(sink, (Node) xnode);
    }

    public void Case3(Sink sink, Edge edge, Trapezoid[] tList)
    {
      YNode ynode = new YNode(edge, (Node) Sink.Isink(tList[0]), (Node) Sink.Isink(tList[1]));
      this.Replace(sink, (Node) ynode);
    }

    public void Case4(Sink sink, Edge edge, Trapezoid[] tList)
    {
      YNode lChild = new YNode(edge, (Node) Sink.Isink(tList[0]), (Node) Sink.Isink(tList[1]));
      XNode xnode = new XNode(edge.Q, (Node) lChild, (Node) Sink.Isink(tList[2]));
      this.Replace(sink, (Node) xnode);
    }
  }
}
