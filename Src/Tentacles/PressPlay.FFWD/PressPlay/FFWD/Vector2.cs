// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Vector2
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Text;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Vector2 : IEquatable<Vector2>
  {
    private static Vector2 Zero = new Vector2(0.0f, 0.0f);
    private static Vector2 One = new Vector2(1f, 1f);
    private static Vector2 Up = new Vector2(0.0f, 1f);
    private static Vector2 Right = new Vector2(1f, 0.0f);
    public float x;
    public float y;

    public float magnitude
    {
      get => (float) Math.Sqrt((double) Vector2.DistanceSquared(this, Vector2.zero));
    }

    public float sqrMagnitude => Vector2.DistanceSquared(this, Vector2.zero);

    public static Vector2 zero => Vector2.Zero;

    public static Vector2 one => Vector2.One;

    public static Vector2 up => Vector2.Up;

    public static Vector2 right => Vector2.Right;

    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.x;
          case 1:
            return this.y;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y) as index to the vector.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.x = value;
            break;
          case 1:
            this.y = value;
            break;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y) as index to the vector.");
        }
      }
    }

    public Vector2(float value)
    {
      this.x = value;
      this.y = value;
    }

    public Vector2(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public Vector2(Microsoft.Xna.Framework.Vector2 v)
      : this(v.X, v.Y)
    {
    }

    public void Scale(Vector2 a)
    {
      this.x *= a.x;
      this.y *= a.y;
    }

    public static float Dot(Vector2 lhs, Vector2 rhs)
    {
      return (float) ((double) lhs.x * (double) rhs.x + (double) lhs.y * (double) rhs.y);
    }

    public static float Distance(Vector2 a, Vector2 b)
    {
      return (float) Math.Sqrt((double) Vector2.DistanceSquared(a, b));
    }

    public static float DistanceSquared(Vector2 a, Vector2 b)
    {
      return (float) (((double) a.x - (double) b.x) * ((double) a.x - (double) b.x) + ((double) a.y - (double) b.y) * ((double) a.y - (double) b.y));
    }

    public static Vector2 Max(Vector2 value1, Vector2 value2)
    {
      return new Vector2(MathHelper.Max(value1.x, value2.x), MathHelper.Max(value1.y, value2.y));
    }

    public static Vector2 Min(Vector2 value1, Vector2 value2)
    {
      return new Vector2(MathHelper.Min(value1.x, value2.x), MathHelper.Min(value1.y, value2.y));
    }

    public override bool Equals(object obj) => obj is Vector2 vector2 && this == vector2;

    public bool Equals(Vector2 other) => this == other;

    public override int GetHashCode() => (int) ((double) this.x + (double) this.y);

    public void Normalize()
    {
      float num = 1f / Vector2.Distance(this, Vector2.zero);
      this.x *= num;
      this.y *= num;
    }

    private static Vector2 Normalize(Vector2 vector)
    {
      vector.Normalize();
      return vector;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder(32);
      stringBuilder.Append("{X=");
      stringBuilder.Append(this.x.ToString() + "f,");
      stringBuilder.Append(" Y=");
      stringBuilder.Append(this.y.ToString() + "f");
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static implicit operator Microsoft.Xna.Framework.Vector2(Vector2 v)
    {
      return new Microsoft.Xna.Framework.Vector2(v.x, v.y);
    }

    public static implicit operator Vector2(Microsoft.Xna.Framework.Vector2 v) => new Vector2(v);

    public static implicit operator Vector3(Vector2 v)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          return new Vector3(0.0f, v.x, v.y);
        case ApplicationSettings.To2dMode.DropY:
          return new Vector3(v.x, 0.0f, v.y);
        case ApplicationSettings.To2dMode.DropZ:
          return new Vector3(v.x, v.y, 0.0f);
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public static bool operator ==(Vector2 value1, Vector2 value2)
    {
      return (double) value1.x == (double) value2.x && (double) value1.y == (double) value2.y;
    }

    public static bool operator !=(Vector2 value1, Vector2 value2) => !(value1 == value2);

    public static Vector2 operator +(Vector2 value1, Vector2 value2)
    {
      value1.x += value2.x;
      value1.y += value2.y;
      return value1;
    }

    public static Vector2 operator -(Vector2 value)
    {
      value = new Vector2(-value.x, -value.y);
      return value;
    }

    public static Vector2 operator -(Vector2 value1, Vector2 value2)
    {
      value1.x -= value2.x;
      value1.y -= value2.y;
      return value1;
    }

    public static Vector2 operator *(Vector2 value1, Vector2 value2)
    {
      value1.x *= value2.x;
      value1.y *= value2.y;
      return value1;
    }

    public static Vector2 operator *(Vector2 value, float scaleFactor)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      return value;
    }

    public static Vector2 operator *(float scaleFactor, Vector2 value)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      return value;
    }

    public static Vector2 operator /(Vector2 value1, Vector2 value2)
    {
      value1.x /= value2.x;
      value1.y /= value2.y;
      return value1;
    }

    public static Vector2 operator /(Vector2 value, float divider)
    {
      float num = 1f / divider;
      value.x *= num;
      value.y *= num;
      return value;
    }
  }
}
