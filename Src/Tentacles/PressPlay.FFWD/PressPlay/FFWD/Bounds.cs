// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Bounds
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

namespace PressPlay.FFWD
{
  public struct Bounds
  {
    public PressPlay.FFWD.Vector3 center;
    public PressPlay.FFWD.Vector3 size;

    public Bounds(PressPlay.FFWD.Vector3 center, PressPlay.FFWD.Vector3 size)
    {
      this.center = center;
      this.size = size;
    }

    public PressPlay.FFWD.Vector3 extents => this.size;

    public PressPlay.FFWD.Vector3 min => this.center - this.extents;

    public PressPlay.FFWD.Vector3 max => this.center + this.extents;
  }
}
