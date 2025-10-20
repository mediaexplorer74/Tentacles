// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.MassData
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public struct MassData : IEquatable<MassData>
  {
    public float Area;
    public Vector2 Centroid;
    public float Inertia;
    public float Mass;

    public static bool operator ==(MassData left, MassData right)
    {
      return (double) left.Area == (double) right.Area && (double) left.Mass == (double) right.Mass && left.Centroid == right.Centroid && (double) left.Inertia == (double) right.Inertia;
    }

    public static bool operator !=(MassData left, MassData right) => !(left == right);

    public bool Equals(MassData other) => this == other;

    public override bool Equals(object obj)
    {
      return !object.ReferenceEquals((object) null, obj) && obj.GetType() == typeof (MassData) && this.Equals((MassData) obj);
    }

    public override int GetHashCode()
    {
      return ((this.Area.GetHashCode() * 397 ^ this.Centroid.GetHashCode()) * 397 ^ this.Inertia.GetHashCode()) * 397 ^ this.Mass.GetHashCode();
    }
  }
}
