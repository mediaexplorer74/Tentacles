// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.CircleShape
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public class CircleShape : Shape
  {
    internal Vector2 _position;

    public CircleShape(float radius, float density)
      : base(density)
    {
      this.ShapeType = ShapeType.Circle;
      this._radius = radius;
      this._position = Vector2.Zero;
      this.ComputeProperties();
    }

    internal CircleShape()
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Circle;
      this._radius = 0.0f;
      this._position = Vector2.Zero;
    }

    public override int ChildCount => 1;

    public Vector2 Position
    {
      get => this._position;
      set
      {
        this._position = value;
        this.ComputeProperties();
      }
    }

    public override Shape Clone()
    {
      CircleShape circleShape = new CircleShape();
      circleShape._radius = this.Radius;
      circleShape._density = this._density;
      circleShape._position = this._position;
      circleShape.ShapeType = this.ShapeType;
      circleShape.MassData = this.MassData;
      return (Shape) circleShape;
    }

    public override bool TestPoint(ref Transform transform, ref Vector2 point)
    {
      Vector2 vector2_1 = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
      Vector2 vector2_2 = point - vector2_1;
      return (double) Vector2.Dot(vector2_2, vector2_2) <= (double) this.Radius * (double) this.Radius;
    }

    public override bool RayCast(
      out RayCastOutput output,
      ref RayCastInput input,
      ref Transform transform,
      int childIndex)
    {
      output = new RayCastOutput();
      Vector2 vector2_1 = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
      Vector2 vector2_2 = input.Point1 - vector2_1;
      float num1 = Vector2.Dot(vector2_2, vector2_2) - this.Radius * this.Radius;
      Vector2 vector2_3 = input.Point2 - input.Point1;
      float num2 = Vector2.Dot(vector2_2, vector2_3);
      float num3 = Vector2.Dot(vector2_3, vector2_3);
      float d = (float) ((double) num2 * (double) num2 - (double) num3 * (double) num1);
      if ((double) d < 0.0 || (double) num3 < 1.1920928955078125E-07)
        return false;
      float num4 = (float) -((double) num2 + Math.Sqrt((double) d));
      if (0.0 > (double) num4 || (double) num4 > (double) input.MaxFraction * (double) num3)
        return false;
      float num5 = num4 / num3;
      output.Fraction = num5;
      Vector2 vector2_4 = vector2_2 + num5 * vector2_3;
      vector2_4.Normalize();
      output.Normal = vector2_4;
      return true;
    }

    public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
    {
      Vector2 vector2 = transform.Position + MathUtils.Multiply(ref transform.R, this.Position);
      aabb.LowerBound = new Vector2(vector2.X - this.Radius, vector2.Y - this.Radius);
      aabb.UpperBound = new Vector2(vector2.X + this.Radius, vector2.Y + this.Radius);
    }

    public override sealed void ComputeProperties()
    {
      float num = 3.14159274f * this.Radius * this.Radius;
      this.MassData.Area = num;
      this.MassData.Mass = this.Density * num;
      this.MassData.Centroid = this.Position;
      this.MassData.Inertia = this.MassData.Mass * (0.5f * this.Radius * this.Radius + Vector2.Dot(this.Position, this.Position));
    }

    public bool CompareTo(CircleShape shape)
    {
      return (double) this.Radius == (double) shape.Radius && this.Position == shape.Position;
    }

    public override float ComputeSubmergedArea(
      Vector2 normal,
      float offset,
      Transform xf,
      out Vector2 sc)
    {
      sc = Vector2.Zero;
      Vector2 vector2 = MathUtils.Multiply(ref xf, this.Position);
      float num1 = (float) -((double) Vector2.Dot(normal, vector2) - (double) offset);
      if ((double) num1 < -(double) this.Radius + 1.1920928955078125E-07)
        return 0.0f;
      if ((double) num1 > (double) this.Radius)
      {
        sc = vector2;
        return 3.14159274f * this.Radius * this.Radius;
      }
      float num2 = this.Radius * this.Radius;
      float num3 = num1 * num1;
      float submergedArea = num2 * (float) (Math.Asin((double) num1 / (double) this.Radius) + 1.5707963705062866 + (double) num1 * Math.Sqrt((double) num2 - (double) num3));
      float num4 = -0.6666667f * (float) Math.Pow((double) num2 - (double) num3, 1.5) / submergedArea;
      sc.X = vector2.X + normal.X * num4;
      sc.Y = vector2.Y + normal.Y * num4;
      return submergedArea;
    }
  }
}
