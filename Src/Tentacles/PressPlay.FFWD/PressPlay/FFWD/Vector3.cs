// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Vector3
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Text;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Vector3 : IEquatable<Vector3>
  {
    private static Vector3 _zero = new Vector3(0.0f, 0.0f, 0.0f);
    private static Vector3 _one = new Vector3(1f, 1f, 1f);
    private static Vector3 _up = new Vector3(0.0f, 1f, 0.0f);
    private static Vector3 _right = new Vector3(1f, 0.0f, 0.0f);
    private static Vector3 _forward = new Vector3(0.0f, 0.0f, -1f);
    private static Vector3 _back = new Vector3(0.0f, 0.0f, 1f);
    private static Vector3 _down = new Vector3(0.0f, -1f, 0.0f);
    public float x;
    public float y;
    public float z;

    public Vector3 normalized => Vector3.Normalize(this);

    public float magnitude
    {
      get => (float) Math.Sqrt((double) Vector3.DistanceSquared(this, Vector3.zero));
    }

    public float sqrMagnitude => Vector3.DistanceSquared(this, Vector3.zero);

    public static Vector3 zero => Vector3._zero;

    public static Vector3 one => Vector3._one;

    public static Vector3 up => Vector3._up;

    public static Vector3 down => Vector3._down;

    public static Vector3 right => Vector3._right;

    public static Vector3 forward => Vector3._forward;

    public static Vector3 back => Vector3._back;

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
          case 2:
            return this.z;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y) or 2 (z) as index to the vector.");
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
          case 2:
            this.z = value;
            break;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y) or 2 (z) as index to the vector.");
        }
      }
    }

    public Vector3(float value)
    {
      this.x = value;
      this.y = value;
      this.z = value;
    }

    public Vector3(float x, float y)
    {
      this.x = x;
      this.y = y;
      this.z = 0.0f;
    }

    public Vector3(float x, float y, float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }

    public Vector3(Vector2 position, float depth)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          this.x = depth;
          this.y = position.x;
          this.z = position.y;
          break;
        case ApplicationSettings.To2dMode.DropY:
          this.x = position.x;
          this.y = depth;
          this.z = position.y;
          break;
        case ApplicationSettings.To2dMode.DropZ:
          this.x = position.x;
          this.y = position.y;
          this.z = depth;
          break;
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public Vector3(Microsoft.Xna.Framework.Vector3 v)
      : this(v.X, v.Y, v.Z)
    {
    }

    public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
    {
      float amount = Mathf.Clamp01(t);
      return new Vector3(MathHelper.Lerp(from.x, to.x, amount), MathHelper.Lerp(from.y, to.y, amount), MathHelper.Lerp(from.z, to.z, amount));
    }

    public static Vector3 Slerp(Vector3 from, Vector3 to, float t)
    {
      throw new NotImplementedException();
    }

    public static void OrthoNormalize(ref Vector3 normal, ref Vector3 tangent)
    {
      throw new NotImplementedException();
    }

    public static void OrthoNormalize(
      ref Vector3 normal,
      ref Vector3 tangent,
      ref Vector3 binormal)
    {
      throw new NotImplementedException();
    }

    public static void MoveTowards(ref Vector3 normal, ref Vector3 tangent, ref Vector3 binormal)
    {
      throw new NotImplementedException();
    }

    public static Vector3 RotateTowards(
      Vector3 from,
      Vector3 to,
      float maxRadiansDelta,
      float maxMagnitudeDelta)
    {
      throw new NotImplementedException();
    }

    public static Vector3 Scale(Vector3 a, Vector3 b)
    {
      a.x *= b.x;
      a.y *= b.y;
      a.z *= b.z;
      return a;
    }

    public void Scale(Vector3 a)
    {
      this.x *= a.x;
      this.y *= a.y;
      this.z *= a.z;
    }

    public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
    {
      return new Vector3((float) ((double) lhs.y * (double) rhs.z - (double) rhs.y * (double) lhs.z), (float) -((double) lhs.x * (double) rhs.z - (double) rhs.x * (double) lhs.z), (float) ((double) lhs.x * (double) rhs.y - (double) rhs.x * (double) lhs.y));
    }

    public static Vector3 Reflect(Vector3 vector, Vector3 normal)
    {
      throw new NotImplementedException();
    }

    public static float Dot(Vector3 lhs, Vector3 rhs)
    {
      return (float) ((double) lhs.x * (double) rhs.x + (double) lhs.y * (double) rhs.y + (double) lhs.z * (double) rhs.z);
    }

    public static Vector3 Project(Vector3 vector, Vector3 onNormal)
    {
      throw new NotImplementedException();
    }

    public static float Angle(Vector3 from, Vector3 to)
    {
      float d = Vector3.Dot(from.normalized, to.normalized);
      return (double) d >= 1.0 || (double) d <= -1.0 ? 0.0f : MathHelper.ToDegrees((float) Math.Acos((double) d));
    }

    public static float Distance(Vector3 a, Vector3 b)
    {
      return (float) Math.Sqrt((double) Vector3.DistanceSquared(a, b));
    }

    public static float DistanceSquared(Vector3 a, Vector3 b)
    {
      return (float) (((double) a.x - (double) b.x) * ((double) a.x - (double) b.x) + ((double) a.y - (double) b.y) * ((double) a.y - (double) b.y) + ((double) a.z - (double) b.z) * ((double) a.z - (double) b.z));
    }

    public static Vector3 Max(Vector3 value1, Vector3 value2)
    {
      return new Vector3(MathHelper.Max(value1.x, value2.x), MathHelper.Max(value1.y, value2.y), MathHelper.Max(value1.z, value2.z));
    }

    public static Vector3 Min(Vector3 value1, Vector3 value2)
    {
      return new Vector3(MathHelper.Min(value1.x, value2.x), MathHelper.Min(value1.y, value2.y), MathHelper.Min(value1.z, value2.z));
    }

    public override bool Equals(object obj) => obj is Vector3 vector3 && this == vector3;

    public bool Equals(Vector3 other) => this == other;

    public override int GetHashCode()
    {
      return (int) ((double) this.x + (double) this.y + (double) this.z);
    }

    public void Normalize()
    {
      if (this == Vector3.zero)
        return;
      float num1 = Vector3.Distance(this, Vector3.zero);
      if ((double) num1 == 0.0)
        return;
      float num2 = 1f / num1;
      this.x *= num2;
      this.y *= num2;
      this.z *= num2;
    }

    private static Vector3 Normalize(Vector3 vector)
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
      stringBuilder.Append(this.y.ToString() + "f,");
      stringBuilder.Append(" Z=");
      stringBuilder.Append(this.z.ToString() + "f");
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static implicit operator Microsoft.Xna.Framework.Vector3(Vector3 v)
    {
      return new Microsoft.Xna.Framework.Vector3(v.x, v.y, v.z);
    }

    public static implicit operator Vector3(Microsoft.Xna.Framework.Vector3 v) => new Vector3(v);

    public static implicit operator Vector2(Vector3 v)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          return new Vector2(v.y, v.z);
        case ApplicationSettings.To2dMode.DropY:
          return new Vector2(v.x, v.z);
        case ApplicationSettings.To2dMode.DropZ:
          return new Vector2(v.x, v.y);
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public static implicit operator Microsoft.Xna.Framework.Vector2(Vector3 v)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          return new Microsoft.Xna.Framework.Vector2(v.y, v.z);
        case ApplicationSettings.To2dMode.DropY:
          return new Microsoft.Xna.Framework.Vector2(v.x, v.z);
        case ApplicationSettings.To2dMode.DropZ:
          return new Microsoft.Xna.Framework.Vector2(v.x, v.y);
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public static implicit operator Vector3(Microsoft.Xna.Framework.Vector2 v)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          return (Vector3) new Microsoft.Xna.Framework.Vector3(0.0f, v.X, v.Y);
        case ApplicationSettings.To2dMode.DropY:
          return (Vector3) new Microsoft.Xna.Framework.Vector3(v.X, 0.0f, v.Y);
        case ApplicationSettings.To2dMode.DropZ:
          return (Vector3) new Microsoft.Xna.Framework.Vector3(v.X, v.Y, 0.0f);
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public static explicit operator float(Vector3 v)
    {
      switch (ApplicationSettings.to2dMode)
      {
        case ApplicationSettings.To2dMode.DropX:
          return v.x;
        case ApplicationSettings.To2dMode.DropY:
          return v.y;
        case ApplicationSettings.To2dMode.DropZ:
          return v.z;
        default:
          throw new Exception("Unknown enum " + (object) ApplicationSettings.to2dMode);
      }
    }

    public static bool operator ==(Vector3 value1, Vector3 value2)
    {
      return (double) value1.x == (double) value2.x && (double) value1.y == (double) value2.y && (double) value1.z == (double) value2.z;
    }

    public static bool operator !=(Vector3 value1, Vector3 value2) => !(value1 == value2);

    public static Vector3 operator +(Vector3 value1, Vector3 value2)
    {
      value1.x += value2.x;
      value1.y += value2.y;
      value1.z += value2.z;
      return value1;
    }

    public static Vector3 operator -(Vector3 value)
    {
      value = new Vector3(-value.x, -value.y, -value.z);
      return value;
    }

    public static Vector3 operator -(Vector3 value1, Vector3 value2)
    {
      value1.x -= value2.x;
      value1.y -= value2.y;
      value1.z -= value2.z;
      return value1;
    }

    public static Vector3 operator *(Vector3 value1, Vector3 value2)
    {
      value1.x *= value2.x;
      value1.y *= value2.y;
      value1.z *= value2.z;
      return value1;
    }

    public static Vector3 operator *(Vector3 value, float scaleFactor)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      value.z *= scaleFactor;
      return value;
    }

    public static Vector3 operator *(float scaleFactor, Vector3 value)
    {
      value.x *= scaleFactor;
      value.y *= scaleFactor;
      value.z *= scaleFactor;
      return value;
    }

    public static Vector3 operator /(Vector3 value1, Vector3 value2)
    {
      value1.x /= value2.x;
      value1.y /= value2.y;
      value1.z /= value2.z;
      return value1;
    }

    public static Vector3 operator /(Vector3 value, float divider)
    {
      float num = 1f / divider;
      value.x *= num;
      value.y *= num;
      value.z *= num;
      return value;
    }
  }
}
