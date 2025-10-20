// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.YNode
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class YNode : Node
  {
    private Edge _edge;

    public YNode(Edge edge, Node lChild, Node rChild)
      : base(lChild, rChild)
    {
      this._edge = edge;
    }

    public override Sink Locate(Edge edge)
    {
      if (this._edge.IsAbove(edge.P))
        return this.RightChild.Locate(edge);
      if (this._edge.IsBelow(edge.P))
        return this.LeftChild.Locate(edge);
      return (double) edge.Slope < (double) this._edge.Slope ? this.RightChild.Locate(edge) : this.LeftChild.Locate(edge);
    }
  }
}
