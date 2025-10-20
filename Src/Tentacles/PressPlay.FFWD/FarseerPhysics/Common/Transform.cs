// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Transform
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct Transform(ref Vector2 position, ref Mat22 r)
  {
    public Vector2 Position = position;
    public Mat22 R = r;

    public float Angle => (float) Math.Atan2((double) this.R.Col1.Y, (double) this.R.Col1.X);

    public void SetIdentity()
    {
      this.Position = Vector2.Zero;
      this.R.SetIdentity();
    }

    public void Set(Vector2 position, float angle)
    {
      this.Position = position;
      this.R.Set(angle);
    }
  }
}
