// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.Shape
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public abstract class Shape
  {
    public MassData MassData;
    public int ShapeId;
    internal float _radius;
    internal float _density;
    private static int _shapeIdCounter;

    protected Shape(float density)
    {
      this._density = density;
      this.ShapeType = ShapeType.Unknown;
      this.ShapeId = Shape._shapeIdCounter++;
    }

    public ShapeType ShapeType { get; internal set; }

    public abstract int ChildCount { get; }

    public float Density
    {
      get => this._density;
      set
      {
        this._density = value;
        this.ComputeProperties();
      }
    }

    public float Radius
    {
      get => this._radius;
      set
      {
        this._radius = value;
        this.ComputeProperties();
      }
    }

    public abstract Shape Clone();

    public abstract bool TestPoint(ref Transform transform, ref Vector2 point);

    public abstract bool RayCast(
      out RayCastOutput output,
      ref RayCastInput input,
      ref Transform transform,
      int childIndex);

    public abstract void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex);

    public abstract void ComputeProperties();

    public bool CompareTo(Shape shape)
    {
      switch (shape)
      {
        case PolygonShape _ when this is PolygonShape:
          return ((PolygonShape) this).CompareTo((PolygonShape) shape);
        case CircleShape _ when this is CircleShape:
          return ((CircleShape) this).CompareTo((CircleShape) shape);
        case EdgeShape _ when this is EdgeShape:
          return ((EdgeShape) this).CompareTo((EdgeShape) shape);
        default:
          return false;
      }
    }

    public abstract float ComputeSubmergedArea(
      Vector2 normal,
      float offset,
      Transform xf,
      out Vector2 sc);
  }
}
