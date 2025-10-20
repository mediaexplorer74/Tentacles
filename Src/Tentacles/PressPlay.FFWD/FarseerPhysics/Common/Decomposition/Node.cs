// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Node
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal abstract class Node
  {
    protected Node LeftChild;
    public List<Node> ParentList;
    protected Node RightChild;

    protected Node(Node left, Node right)
    {
      this.ParentList = new List<Node>();
      this.LeftChild = left;
      this.RightChild = right;
      left?.ParentList.Add(this);
      right?.ParentList.Add(this);
    }

    public abstract Sink Locate(Edge s);

    public void Replace(Node node)
    {
      foreach (Node parent in node.ParentList)
      {
        if (parent.LeftChild == node)
          parent.LeftChild = this;
        else
          parent.RightChild = this;
      }
      this.ParentList.AddRange((IEnumerable<Node>) node.ParentList);
    }
  }
}
