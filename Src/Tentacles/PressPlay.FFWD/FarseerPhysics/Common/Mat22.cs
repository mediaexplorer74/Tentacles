// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Mat22
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct Mat22
  {
    public Vector2 Col1;
    public Vector2 Col2;

    public Mat22(Vector2 c1, Vector2 c2)
    {
      this.Col1 = c1;
      this.Col2 = c2;
    }

    public Mat22(float a11, float a12, float a21, float a22)
    {
      this.Col1 = new Vector2(a11, a21);
      this.Col2 = new Vector2(a12, a22);
    }

    public Mat22(float angle)
    {
      float num = (float) Math.Cos((double) angle);
      float y = (float) Math.Sin((double) angle);
      this.Col1 = new Vector2(num, y);
      this.Col2 = new Vector2(-y, num);
    }

    public float Angle => (float) Math.Atan2((double) this.Col1.Y, (double) this.Col1.X);

    public Mat22 Inverse
    {
      get
      {
        float x1 = this.Col1.X;
        float x2 = this.Col2.X;
        float y1 = this.Col1.Y;
        float y2 = this.Col2.Y;
        float num = (float) ((double) x1 * (double) y2 - (double) x2 * (double) y1);
        if ((double) num != 0.0)
          num = 1f / num;
        return new Mat22()
        {
          Col1 = {
            X = num * y2,
            Y = -num * y1
          },
          Col2 = {
            X = -num * x2,
            Y = num * x1
          }
        };
      }
    }

    public void Set(Vector2 c1, Vector2 c2)
    {
      this.Col1 = c1;
      this.Col2 = c2;
    }

    public void Set(float angle)
    {
      float num1 = (float) Math.Cos((double) angle);
      float num2 = (float) Math.Sin((double) angle);
      this.Col1.X = num1;
      this.Col2.X = -num2;
      this.Col1.Y = num2;
      this.Col2.Y = num1;
    }

    public void SetIdentity()
    {
      this.Col1.X = 1f;
      this.Col2.X = 0.0f;
      this.Col1.Y = 0.0f;
      this.Col2.Y = 1f;
    }

    public void SetZero()
    {
      this.Col1.X = 0.0f;
      this.Col2.X = 0.0f;
      this.Col1.Y = 0.0f;
      this.Col2.Y = 0.0f;
    }

    public Vector2 Solve(Vector2 b)
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

    public static void Add(ref Mat22 A, ref Mat22 B, out Mat22 R)
    {
      R.Col1 = A.Col1 + B.Col1;
      R.Col2 = A.Col2 + B.Col2;
    }
  }
}
