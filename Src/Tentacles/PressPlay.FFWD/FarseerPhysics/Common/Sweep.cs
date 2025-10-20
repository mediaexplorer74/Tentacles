// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Sweep
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct Sweep
  {
    public float A;
    public float A0;
    public float Alpha0;
    public Vector2 C;
    public Vector2 C0;
    public Vector2 LocalCenter;

    public void GetTransform(out Transform xf, float beta)
    {
      xf = new Transform();
      xf.Position.X = (float) ((1.0 - (double) beta) * (double) this.C0.X + (double) beta * (double) this.C.X);
      xf.Position.Y = (float) ((1.0 - (double) beta) * (double) this.C0.Y + (double) beta * (double) this.C.Y);
      float angle = (float) ((1.0 - (double) beta) * (double) this.A0 + (double) beta * (double) this.A);
      xf.R.Set(angle);
      xf.Position -= MathUtils.Multiply(ref xf.R, ref this.LocalCenter);
    }

    public void Advance(float alpha)
    {
      float num = (float) (((double) alpha - (double) this.Alpha0) / (1.0 - (double) this.Alpha0));
      this.C0.X = (float) ((1.0 - (double) num) * (double) this.C0.X + (double) num * (double) this.C.X);
      this.C0.Y = (float) ((1.0 - (double) num) * (double) this.C0.Y + (double) num * (double) this.C.Y);
      this.A0 = (float) ((1.0 - (double) num) * (double) this.A0 + (double) num * (double) this.A);
      this.Alpha0 = alpha;
    }

    public void Normalize()
    {
      float num = 6.28318548f * (float) Math.Floor((double) this.A0 / 6.2831854820251465);
      this.A0 -= num;
      this.A -= num;
    }
  }
}
