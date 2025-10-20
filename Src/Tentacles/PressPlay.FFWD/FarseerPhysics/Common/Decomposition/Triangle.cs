// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Triangle
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  public class Triangle
  {
    public float[] X;
    public float[] Y;

    public Triangle(float x1, float y1, float x2, float y2, float x3, float y3)
    {
      this.X = new float[3];
      this.Y = new float[3];
      float num1 = x2 - x1;
      float num2 = x3 - x1;
      float num3 = y2 - y1;
      float num4 = y3 - y1;
      if ((double) num1 * (double) num4 - (double) num2 * (double) num3 > 0.0)
      {
        this.X[0] = x1;
        this.X[1] = x2;
        this.X[2] = x3;
        this.Y[0] = y1;
        this.Y[1] = y2;
        this.Y[2] = y3;
      }
      else
      {
        this.X[0] = x1;
        this.X[1] = x3;
        this.X[2] = x2;
        this.Y[0] = y1;
        this.Y[1] = y3;
        this.Y[2] = y2;
      }
    }

    public Triangle(Triangle t)
    {
      this.X = new float[3];
      this.Y = new float[3];
      this.X[0] = t.X[0];
      this.X[1] = t.X[1];
      this.X[2] = t.X[2];
      this.Y[0] = t.Y[0];
      this.Y[1] = t.Y[1];
      this.Y[2] = t.Y[2];
    }

    public bool IsInside(float x, float y)
    {
      if ((double) x < (double) this.X[0] && (double) x < (double) this.X[1] && (double) x < (double) this.X[2] || (double) x > (double) this.X[0] && (double) x > (double) this.X[1] && (double) x > (double) this.X[2] || (double) y < (double) this.Y[0] && (double) y < (double) this.Y[1] && (double) y < (double) this.Y[2] || (double) y > (double) this.Y[0] && (double) y > (double) this.Y[1] && (double) y > (double) this.Y[2])
        return false;
      float num1 = x - this.X[0];
      float num2 = y - this.Y[0];
      float num3 = this.X[1] - this.X[0];
      float num4 = this.Y[1] - this.Y[0];
      float num5 = this.X[2] - this.X[0];
      float num6 = this.Y[2] - this.Y[0];
      float num7 = (float) ((double) num5 * (double) num5 + (double) num6 * (double) num6);
      float num8 = (float) ((double) num5 * (double) num3 + (double) num6 * (double) num4);
      float num9 = (float) ((double) num5 * (double) num1 + (double) num6 * (double) num2);
      float num10 = (float) ((double) num3 * (double) num3 + (double) num4 * (double) num4);
      float num11 = (float) ((double) num3 * (double) num1 + (double) num4 * (double) num2);
      float num12 = (float) (1.0 / ((double) num7 * (double) num10 - (double) num8 * (double) num8));
      float num13 = (float) ((double) num10 * (double) num9 - (double) num8 * (double) num11) * num12;
      float num14 = (float) ((double) num7 * (double) num11 - (double) num8 * (double) num9) * num12;
      return (double) num13 > 0.0 && (double) num14 > 0.0 && (double) num13 + (double) num14 < 1.0;
    }
  }
}
