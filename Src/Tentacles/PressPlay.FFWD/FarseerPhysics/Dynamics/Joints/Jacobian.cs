// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.Jacobian
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  internal struct Jacobian
  {
    public float AngularA;
    public float AngularB;
    public Vector2 LinearA;
    public Vector2 LinearB;

    public void SetZero()
    {
      this.LinearA = Vector2.Zero;
      this.AngularA = 0.0f;
      this.LinearB = Vector2.Zero;
      this.AngularB = 0.0f;
    }

    public void Set(Vector2 x1, float a1, Vector2 x2, float a2)
    {
      this.LinearA = x1;
      this.AngularA = a1;
      this.LinearB = x2;
      this.AngularB = a2;
    }

    public float Compute(Vector2 x1, float a1, Vector2 x2, float a2)
    {
      return (float) ((double) Vector2.Dot(this.LinearA, x1) + (double) this.AngularA * (double) a1 + (double) Vector2.Dot(this.LinearB, x2) + (double) this.AngularB * (double) a2);
    }
  }
}
