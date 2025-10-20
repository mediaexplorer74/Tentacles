// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Quaternion
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Quaternion
  {
    internal Microsoft.Xna.Framework.Quaternion quaternion;
    private static Quaternion _identity = new Quaternion(Microsoft.Xna.Framework.Quaternion.Identity);

    public Quaternion(float x, float y, float z, float w)
    {
      this.quaternion = new Microsoft.Xna.Framework.Quaternion(x, y, z, w);
    }

    public Quaternion(Microsoft.Xna.Framework.Quaternion q) => this.quaternion = q;

    public float x
    {
      get => this.quaternion.X;
      set => this.quaternion.X = value;
    }

    public float y
    {
      get => this.quaternion.Y;
      set => this.quaternion.Y = value;
    }

    public float z
    {
      get => this.quaternion.Z;
      set => this.quaternion.Z = value;
    }

    public float w
    {
      get => this.quaternion.W;
      set => this.quaternion.W = value;
    }

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
          case 3:
            return this.w;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y), 2 (z) or 3 (w) as index to the quaternion.");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.quaternion.X = value;
            break;
          case 1:
            this.quaternion.Y = value;
            break;
          case 2:
            this.quaternion.Z = value;
            break;
          case 3:
            this.quaternion.W = value;
            break;
          default:
            throw new IndexOutOfRangeException("You must use 0 (x), 1 (y), 2 (z) or 3 (w) as index to the quaternion.");
        }
      }
    }

    public Vector3 eulerAngles => this.QuaternionToEulerAngleVector3(this);

    public void ToAngleAxis(out float angle, out Vector3 axis)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public void SetLookRotation(Vector3 view) => this.SetLookRotation(view, Vector3.up);

    public void SetLookRotation(Vector3 view, Vector3 up)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static Quaternion identity => Quaternion._identity;

    public static implicit operator Microsoft.Xna.Framework.Quaternion(Quaternion q)
    {
      return q.quaternion;
    }

    public static implicit operator Quaternion(Microsoft.Xna.Framework.Quaternion q)
    {
      return new Quaternion(q);
    }

    public static Quaternion operator *(Quaternion lhs, Quaternion rhs)
    {
      return new Quaternion(lhs.quaternion * rhs.quaternion);
    }

    public static Vector3 operator *(Quaternion rotation, Vector3 point)
    {
      return new Vector3(Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) point, rotation.quaternion));
    }

    public static bool operator ==(Quaternion lhs, Quaternion rhs)
    {
      return lhs.quaternion == rhs.quaternion;
    }

    public static bool operator !=(Quaternion lhs, Quaternion rhs)
    {
      return lhs.quaternion != rhs.quaternion;
    }

    public override bool Equals(object obj)
    {
      switch (obj)
      {
        case Quaternion quaternion1:
          return quaternion1.Equals((object) this.quaternion);
        case Microsoft.Xna.Framework.Quaternion quaternion2:
          return quaternion2.Equals(this.quaternion);
        default:
          return false;
      }
    }

    public override int GetHashCode() => this.quaternion.GetHashCode();

    public override string ToString() => this.quaternion.ToString();

    private Vector3 AngleTo(Vector3 from, Vector3 location)
    {
      Vector3 vector3 = new Vector3();
      Vector3 normalized = (location - from).normalized;
      vector3.x = (float) Math.Asin((double) normalized.y);
      vector3.y = (float) Math.Atan2(-(double) normalized.x, -(double) normalized.z);
      return vector3;
    }

    private Vector3 QuaternionToEulerAngleVector3(Quaternion rotation)
    {
      Vector3 vector3_1 = new Vector3();
      Vector3 location = rotation * Vector3.forward;
      Vector3 position = rotation * Vector3.up;
      Vector3 vector3_2 = this.AngleTo(new Vector3(), location);
      if ((double) vector3_2.x == 1.5707963705062866)
      {
        vector3_2.y = (float) Math.Atan2((double) position.x, (double) position.z);
        vector3_2.z = 0.0f;
      }
      else if ((double) vector3_2.x == -1.5707963705062866)
      {
        vector3_2.y = (float) Math.Atan2(-(double) position.x, -(double) position.z);
        vector3_2.z = 0.0f;
      }
      else
      {
        Vector3 vector3_3 = (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) (Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) position, Matrix.CreateRotationY(-vector3_2.y)), Matrix.CreateRotationX(-vector3_2.x));
        vector3_2.z = (float) Math.Atan2(-(double) vector3_3.x, (double) vector3_3.y);
      }
      return new Vector3(MathHelper.ToDegrees(vector3_2.x), MathHelper.ToDegrees(vector3_2.y), MathHelper.ToDegrees(vector3_2.z));
    }

    public static float Dot(Quaternion a, Quaternion b)
    {
      float result;
      Microsoft.Xna.Framework.Quaternion.Dot(ref a.quaternion, ref b.quaternion, out result);
      return result;
    }

    public static Quaternion AngleAxis(float angle, Vector3 axis)
    {
      return new Quaternion(Microsoft.Xna.Framework.Quaternion.CreateFromAxisAngle((Microsoft.Xna.Framework.Vector3) axis, MathHelper.ToRadians(angle)));
    }

    public static Quaternion FromToRotation(Vector3 from, Vector3 to)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static Quaternion Slerp(Quaternion from, Quaternion to, float t)
    {
      Microsoft.Xna.Framework.Quaternion result;
      Microsoft.Xna.Framework.Quaternion.Slerp(ref from.quaternion, ref to.quaternion, t, out result);
      return new Quaternion(result);
    }

    public static Quaternion Lerp(Quaternion from, Quaternion to, float t)
    {
      Microsoft.Xna.Framework.Quaternion result;
      Microsoft.Xna.Framework.Quaternion.Lerp(ref from.quaternion, ref to.quaternion, t, out result);
      return new Quaternion(result);
    }

    public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static Quaternion Inverse(Quaternion q)
    {
      return new Quaternion(Microsoft.Xna.Framework.Quaternion.Inverse(q.quaternion));
    }

    public static Quaternion Angle(Quaternion from, Quaternion to)
    {
      throw new NotImplementedException("Method not implemented.");
    }

    public static Quaternion Euler(float x, float y, float z)
    {
      return new Quaternion(Microsoft.Xna.Framework.Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(y), MathHelper.ToRadians(x), MathHelper.ToRadians(z)));
    }

    public static Quaternion Euler(Vector3 v) => Quaternion.Euler(v.x, v.y, v.z);

    public static Quaternion LookRotation(Vector3 forward)
    {
      return Quaternion.LookRotation(forward, Vector3.up);
    }

    public static Quaternion LookRotation(Vector3 forward, Vector3 up)
    {
      return forward == Vector3.zero ? Quaternion.identity : new Quaternion(Microsoft.Xna.Framework.Quaternion.CreateFromRotationMatrix(Matrix.CreateWorld(Microsoft.Xna.Framework.Vector3.Zero, (Microsoft.Xna.Framework.Vector3) forward, (Microsoft.Xna.Framework.Vector3) up)));
    }
  }
}
