// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.XNode
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class XNode : Node
  {
    private Point _point;

    public XNode(Point point, Node lChild, Node rChild)
      : base(lChild, rChild)
    {
      this._point = point;
    }

    public override Sink Locate(Edge edge)
    {
      return (double) edge.P.X >= (double) this._point.X ? this.RightChild.Locate(edge) : this.LeftChild.Locate(edge);
    }
  }
}
