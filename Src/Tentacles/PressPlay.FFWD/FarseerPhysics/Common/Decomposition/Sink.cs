// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Sink
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class Sink : Node
  {
    public Trapezoid Trapezoid;

    private Sink(Trapezoid trapezoid)
      : base((Node) null, (Node) null)
    {
      this.Trapezoid = trapezoid;
      trapezoid.Sink = this;
    }

    public static Sink Isink(Trapezoid trapezoid)
    {
      return trapezoid.Sink == null ? new Sink(trapezoid) : trapezoid.Sink;
    }

    public override Sink Locate(Edge edge) => this;
  }
}
