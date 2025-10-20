// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Mat33
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct Mat33(Vector3 c1, Vector3 c2, Vector3 c3)
  {
    public Vector3 Col1 = c1;
    public Vector3 Col2 = c2;
    public Vector3 Col3 = c3;

    public void SetZero()
    {
      this.Col1 = Vector3.Zero;
      this.Col2 = Vector3.Zero;
      this.Col3 = Vector3.Zero;
    }

    public Vector3 Solve33(Vector3 b)
    {
      float num = Vector3.Dot(this.Col1, Vector3.Cross(this.Col2, this.Col3));
      if ((double) num != 0.0)
        num = 1f / num;
      return new Vector3(num * Vector3.Dot(b, Vector3.Cross(this.Col2, this.Col3)), num * Vector3.Dot(this.Col1, Vector3.Cross(b, this.Col3)), num * Vector3.Dot(this.Col1, Vector3.Cross(this.Col2, b)));
    }

    public Vector2 Solve22(Vector2 b)
    {
      float x1 = this.Col1.X;
      float x2 = this.Col2.X;
      float y1 = this.Col1.Y;
      float y2 = this.Col2.Y;
      float num = (float) ((double) x1 * (double) y2 - (double) x2 * (double) y1);
      if ((double) num != 0.0)
        num = 1f / num;
      return new Vector2(num * (float) ((double) y2 * (double) b.X - (double) x2 * (double) b.Y), num * (float) ((double) x1 * (double) b.Y - (double) y1 * (double) b.X));
    }
  }
}
