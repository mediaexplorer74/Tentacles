// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.DebugView
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics
{
  public abstract class DebugView
  {
    protected DebugView(World world) => this.World = world;

    protected World World { get; private set; }

    public DebugViewFlags Flags { get; set; }

    public void AppendFlags(DebugViewFlags flags) => this.Flags |= flags;

    public void RemoveFlags(DebugViewFlags flags) => this.Flags &= ~flags;

    public abstract void DrawPolygon(
      Vector2[] vertices,
      int count,
      float red,
      float blue,
      float green);

    public abstract void DrawSolidPolygon(
      Vector2[] vertices,
      int count,
      float red,
      float blue,
      float green);

    public abstract void DrawCircle(
      Vector2 center,
      float radius,
      float red,
      float blue,
      float green);

    public abstract void DrawSolidCircle(
      Vector2 center,
      float radius,
      Vector2 axis,
      float red,
      float blue,
      float green);

    public abstract void DrawSegment(
      Vector2 start,
      Vector2 end,
      float red,
      float blue,
      float green);

    public abstract void DrawTransform(ref Transform transform);
  }
}
