// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.AABB
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  public struct AABB
  {
    public Vector2 LowerBound;
    public Vector2 UpperBound;
    private static DistanceInput _input = new DistanceInput();

    public AABB(Vector2 min, Vector2 max)
      : this(ref min, ref max)
    {
    }

    public AABB(ref Vector2 min, ref Vector2 max)
    {
      this.LowerBound = min;
      this.UpperBound = max;
    }

    public AABB(Vector2 center, float width, float height)
    {
      this.LowerBound = center - new Vector2(width / 2f, height / 2f);
      this.UpperBound = center + new Vector2(width / 2f, height / 2f);
    }

    public Vector2 Center => 0.5f * (this.LowerBound + this.UpperBound);

    public Vector2 Extents => 0.5f * (this.UpperBound - this.LowerBound);

    public float Perimeter
    {
      get
      {
        return (float) (2.0 * ((double) (this.UpperBound.X - this.LowerBound.X) + (double) (this.UpperBound.Y - this.LowerBound.Y)));
      }
    }

    public Vertices Vertices
    {
      get
      {
        Vertices vertices = new Vertices();
        vertices.Add(this.LowerBound);
        vertices.Add(new Vector2(this.LowerBound.X, this.UpperBound.Y));
        vertices.Add(this.UpperBound);
        vertices.Add(new Vector2(this.UpperBound.X, this.LowerBound.Y));
        return vertices;
      }
    }

    public AABB Q1 => new AABB(this.Center, this.UpperBound);

    public AABB Q2
    {
      get
      {
        return new AABB(new Vector2(this.LowerBound.X, this.Center.Y), new Vector2(this.Center.X, this.UpperBound.Y));
      }
    }

    public AABB Q3 => new AABB(this.LowerBound, this.Center);

    public AABB Q4
    {
      get
      {
        return new AABB(new Vector2(this.Center.X, this.LowerBound.Y), new Vector2(this.UpperBound.X, this.Center.Y));
      }
    }

    public Vector2[] GetVertices()
    {
      return new Vector2[4]
      {
        this.UpperBound,
        new Vector2(this.UpperBound.X, this.LowerBound.Y),
        this.LowerBound,
        new Vector2(this.LowerBound.X, this.UpperBound.Y)
      };
    }

    public bool IsValid()
    {
      Vector2 vector2 = this.UpperBound - this.LowerBound;
      return (double) vector2.X >= 0.0 && (double) vector2.Y >= 0.0 && this.LowerBound.IsValid() && this.UpperBound.IsValid();
    }

    public void Combine(ref AABB aabb)
    {
      this.LowerBound = Vector2.Min(this.LowerBound, aabb.LowerBound);
      this.UpperBound = Vector2.Max(this.UpperBound, aabb.UpperBound);
    }

    public void Combine(ref AABB aabb1, ref AABB aabb2)
    {
      this.LowerBound = Vector2.Min(aabb1.LowerBound, aabb2.LowerBound);
      this.UpperBound = Vector2.Max(aabb1.UpperBound, aabb2.UpperBound);
    }

    public bool Contains(ref AABB aabb)
    {
      return (double) this.LowerBound.X <= (double) aabb.LowerBound.X && (double) this.LowerBound.Y <= (double) aabb.LowerBound.Y && (double) aabb.UpperBound.X <= (double) this.UpperBound.X && (double) aabb.UpperBound.Y <= (double) this.UpperBound.Y;
    }

    public bool Contains(ref Vector2 point)
    {
      return (double) point.X > (double) this.LowerBound.X + 1.1920928955078125E-07 && (double) point.X < (double) this.UpperBound.X - 1.1920928955078125E-07 && (double) point.Y > (double) this.LowerBound.Y + 1.1920928955078125E-07 && (double) point.Y < (double) this.UpperBound.Y - 1.1920928955078125E-07;
    }

    public static bool TestOverlap(AABB a, AABB b) => AABB.TestOverlap(ref a, ref b);

    public static bool TestOverlap(ref AABB a, ref AABB b)
    {
      Vector2 vector2_1 = b.LowerBound - a.UpperBound;
      Vector2 vector2_2 = a.LowerBound - b.UpperBound;
      return (double) vector2_1.X <= 0.0 && (double) vector2_1.Y <= 0.0 && (double) vector2_2.X <= 0.0 && (double) vector2_2.Y <= 0.0;
    }

    public static bool TestOverlap(
      Shape shapeA,
      int indexA,
      Shape shapeB,
      int indexB,
      ref Transform xfA,
      ref Transform xfB)
    {
      AABB._input.ProxyA.Set(shapeA, indexA);
      AABB._input.ProxyB.Set(shapeB, indexB);
      AABB._input.TransformA = xfA;
      AABB._input.TransformB = xfB;
      AABB._input.UseRadii = true;
      DistanceOutput output;
      Distance.ComputeDistance(out output, out SimplexCache _, AABB._input);
      return (double) output.Distance < 1.1920928955078125E-06;
    }

    public bool RayCast(out RayCastOutput output, ref RayCastInput input)
    {
      output = new RayCastOutput();
      float num1 = float.MinValue;
      float val1 = float.MaxValue;
      Vector2 point1 = input.Point1;
      Vector2 v = input.Point2 - input.Point1;
      Vector2 vector2 = MathUtils.Abs(v);
      Vector2 zero = Vector2.Zero;
      for (int index = 0; index < 2; ++index)
      {
        float num2 = index == 0 ? vector2.X : vector2.Y;
        float num3 = index == 0 ? this.LowerBound.X : this.LowerBound.Y;
        float num4 = index == 0 ? this.UpperBound.X : this.UpperBound.Y;
        float num5 = index == 0 ? point1.X : point1.Y;
        if ((double) num2 < 1.1920928955078125E-07)
        {
          if ((double) num5 < (double) num3 || (double) num4 < (double) num5)
            return false;
        }
        else
        {
          float num6 = 1f / (index == 0 ? v.X : v.Y);
          float a = (num3 - num5) * num6;
          float b = (num4 - num5) * num6;
          float num7 = -1f;
          if ((double) a > (double) b)
          {
            MathUtils.Swap<float>(ref a, ref b);
            num7 = 1f;
          }
          if ((double) a > (double) num1)
          {
            if (index == 0)
              zero.X = num7;
            else
              zero.Y = num7;
            num1 = a;
          }
          val1 = Math.Min(val1, b);
          if ((double) num1 > (double) val1)
            return false;
        }
      }
      if ((double) num1 < 0.0 || (double) input.MaxFraction < (double) num1)
        return false;
      output.Fraction = num1;
      output.Normal = zero;
      return true;
    }
  }
}
