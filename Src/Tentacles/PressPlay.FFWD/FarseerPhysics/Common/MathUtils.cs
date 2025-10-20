// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.MathUtils
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace FarseerPhysics.Common
{
  public static class MathUtils
  {
    public static float Cross(Vector2 a, Vector2 b)
    {
      return (float) ((double) a.X * (double) b.Y - (double) a.Y * (double) b.X);
    }

    public static Vector2 Cross(Vector2 a, float s) => new Vector2(s * a.Y, -s * a.X);

    public static Vector2 Cross(float s, Vector2 a) => new Vector2(-s * a.Y, s * a.X);

    public static Vector2 Abs(Vector2 v) => new Vector2(Math.Abs(v.X), Math.Abs(v.Y));

    public static Vector2 Multiply(ref Mat22 A, Vector2 v) => MathUtils.Multiply(ref A, ref v);

    public static Vector2 Multiply(ref Mat22 A, ref Vector2 v)
    {
      return new Vector2((float) ((double) A.Col1.X * (double) v.X + (double) A.Col2.X * (double) v.Y), (float) ((double) A.Col1.Y * (double) v.X + (double) A.Col2.Y * (double) v.Y));
    }

    public static Vector2 MultiplyT(ref Mat22 A, Vector2 v) => MathUtils.MultiplyT(ref A, ref v);

    public static Vector2 MultiplyT(ref Mat22 A, ref Vector2 v)
    {
      return new Vector2((float) ((double) v.X * (double) A.Col1.X + (double) v.Y * (double) A.Col1.Y), (float) ((double) v.X * (double) A.Col2.X + (double) v.Y * (double) A.Col2.Y));
    }

    public static Vector2 Multiply(ref Transform T, Vector2 v) => MathUtils.Multiply(ref T, ref v);

    public static Vector2 Multiply(ref Transform T, ref Vector2 v)
    {
      return new Vector2((float) ((double) T.Position.X + (double) T.R.Col1.X * (double) v.X + (double) T.R.Col2.X * (double) v.Y), (float) ((double) T.Position.Y + (double) T.R.Col1.Y * (double) v.X + (double) T.R.Col2.Y * (double) v.Y));
    }

    public static Vector2 MultiplyT(ref Transform T, Vector2 v)
    {
      return MathUtils.MultiplyT(ref T, ref v);
    }

    public static Vector2 MultiplyT(ref Transform T, ref Vector2 v)
    {
      Vector2 zero = Vector2.Zero with
      {
        X = v.X - T.Position.X,
        Y = v.Y - T.Position.Y
      };
      return MathUtils.MultiplyT(ref T.R, ref zero);
    }

    public static void MultiplyT(ref Mat22 A, ref Mat22 B, out Mat22 C)
    {
      C = new Mat22();
      C.Col1.X = (float) ((double) A.Col1.X * (double) B.Col1.X + (double) A.Col1.Y * (double) B.Col1.Y);
      C.Col1.Y = (float) ((double) A.Col2.X * (double) B.Col1.X + (double) A.Col2.Y * (double) B.Col1.Y);
      C.Col2.X = (float) ((double) A.Col1.X * (double) B.Col2.X + (double) A.Col1.Y * (double) B.Col2.Y);
      C.Col2.Y = (float) ((double) A.Col2.X * (double) B.Col2.X + (double) A.Col2.Y * (double) B.Col2.Y);
    }

    public static void MultiplyT(ref Transform A, ref Transform B, out Transform C)
    {
      C = new Transform();
      MathUtils.MultiplyT(ref A.R, ref B.R, out C.R);
      C.Position.X = B.Position.X - A.Position.X;
      C.Position.Y = B.Position.Y - A.Position.Y;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
      T obj = a;
      a = b;
      b = obj;
    }

    public static bool IsValid(float x) => !float.IsNaN(x) && !float.IsInfinity(x);

    public static bool IsValid(this Vector2 x) => MathUtils.IsValid(x.X) && MathUtils.IsValid(x.Y);

    public static float InvSqrt(float x)
    {
      MathUtils.FloatConverter floatConverter = new MathUtils.FloatConverter();
      floatConverter.x = x;
      float num = 0.5f * x;
      floatConverter.i = 1597463007 - (floatConverter.i >> 1);
      x = floatConverter.x;
      x *= (float) (1.5 - (double) num * (double) x * (double) x);
      return x;
    }

    public static int Clamp(int a, int low, int high) => Math.Max(low, Math.Min(a, high));

    public static float Clamp(float a, float low, float high) => Math.Max(low, Math.Min(a, high));

    public static Vector2 Clamp(Vector2 a, Vector2 low, Vector2 high)
    {
      return Vector2.Max(low, Vector2.Min(a, high));
    }

    public static void Cross(ref Vector2 a, ref Vector2 b, out float c)
    {
      c = (float) ((double) a.X * (double) b.Y - (double) a.Y * (double) b.X);
    }

    public static double VectorAngle(ref Vector2 p1, ref Vector2 p2)
    {
      double num1 = Math.Atan2((double) p1.Y, (double) p1.X);
      double num2 = Math.Atan2((double) p2.Y, (double) p2.X) - num1;
      while (num2 > Math.PI)
        num2 -= 2.0 * Math.PI;
      while (num2 < -1.0 * Math.PI)
        num2 += 2.0 * Math.PI;
      return num2;
    }

    public static double VectorAngle(Vector2 p1, Vector2 p2)
    {
      return MathUtils.VectorAngle(ref p1, ref p2);
    }

    public static float Area(Vector2 a, Vector2 b, Vector2 c)
    {
      return MathUtils.Area(ref a, ref b, ref c);
    }

    public static float Area(ref Vector2 a, ref Vector2 b, ref Vector2 c)
    {
      return (float) ((double) a.X * ((double) b.Y - (double) c.Y) + (double) b.X * ((double) c.Y - (double) a.Y) + (double) c.X * ((double) a.Y - (double) b.Y));
    }

    public static bool Collinear(ref Vector2 a, ref Vector2 b, ref Vector2 c)
    {
      return MathUtils.Collinear(ref a, ref b, ref c, 0.0f);
    }

    public static bool Collinear(ref Vector2 a, ref Vector2 b, ref Vector2 c, float tolerance)
    {
      return MathUtils.FloatInRange(MathUtils.Area(ref a, ref b, ref c), -tolerance, tolerance);
    }

    public static void Cross(float s, ref Vector2 a, out Vector2 b)
    {
      b = new Vector2(-s * a.Y, s * a.X);
    }

    public static bool FloatEquals(float value1, float value2)
    {
      return (double) Math.Abs(value1 - value2) <= 1.1920928955078125E-07;
    }

    public static bool FloatEquals(float value1, float value2, float delta)
    {
      return MathUtils.FloatInRange(value1, value2 - delta, value2 + delta);
    }

    public static bool FloatInRange(float value, float min, float max)
    {
      return (double) value >= (double) min && (double) value <= (double) max;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct FloatConverter
    {
      [FieldOffset(0)]
      public float x;
      [FieldOffset(0)]
      public int i;
    }
  }
}
