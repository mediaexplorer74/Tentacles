// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.EdgeShape
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public class EdgeShape : Shape
  {
    public bool HasVertex0;
    public bool HasVertex3;
    public Vector2 Vertex0;
    private Vector2 _vertex1;
    private Vector2 _vertex2;
    public Vector2 Vertex3;

    internal EdgeShape()
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Edge;
      this._radius = 0.01f;
    }

    public EdgeShape(Vector2 start, Vector2 end)
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Edge;
      this._radius = 0.01f;
      this.Set(start, end);
    }

    public override int ChildCount => 1;

    public Vector2 Vertex1
    {
      get => this._vertex1;
      set
      {
        this._vertex1 = value;
        this.ComputeProperties();
      }
    }

    public Vector2 Vertex2
    {
      get => this._vertex2;
      set
      {
        this._vertex2 = value;
        this.ComputeProperties();
      }
    }

    public void Set(Vector2 start, Vector2 end)
    {
      this._vertex1 = start;
      this._vertex2 = end;
      this.HasVertex0 = false;
      this.HasVertex3 = false;
      this.ComputeProperties();
    }

    public override Shape Clone()
    {
      EdgeShape edgeShape = new EdgeShape();
      edgeShape._radius = this._radius;
      edgeShape._density = this._density;
      edgeShape.HasVertex0 = this.HasVertex0;
      edgeShape.HasVertex3 = this.HasVertex3;
      edgeShape.Vertex0 = this.Vertex0;
      edgeShape._vertex1 = this._vertex1;
      edgeShape._vertex2 = this._vertex2;
      edgeShape.Vertex3 = this.Vertex3;
      edgeShape.MassData = this.MassData;
      return (Shape) edgeShape;
    }

    public override bool TestPoint(ref Transform transform, ref Vector2 point) => false;

    public override bool RayCast(
      out RayCastOutput output,
      ref RayCastInput input,
      ref Transform transform,
      int childIndex)
    {
      output = new RayCastOutput();
      Vector2 vector2_1 = MathUtils.MultiplyT(ref transform.R, input.Point1 - transform.Position);
      Vector2 vector2_2 = MathUtils.MultiplyT(ref transform.R, input.Point2 - transform.Position) - vector2_1;
      Vector2 vertex1 = this._vertex1;
      Vector2 vertex2 = this._vertex2;
      Vector2 vector2_3 = vertex2 - vertex1;
      Vector2 vector2_4 = new Vector2(vector2_3.Y, -vector2_3.X);
      vector2_4.Normalize();
      float num1 = Vector2.Dot(vector2_4, vertex1 - vector2_1);
      float num2 = Vector2.Dot(vector2_4, vector2_2);
      if ((double) num2 == 0.0)
        return false;
      float num3 = num1 / num2;
      if ((double) num3 < 0.0 || 1.0 < (double) num3)
        return false;
      Vector2 vector2_5 = vector2_1 + num3 * vector2_2;
      Vector2 vector2_6 = vertex2 - vertex1;
      float num4 = Vector2.Dot(vector2_6, vector2_6);
      if ((double) num4 == 0.0)
        return false;
      float num5 = Vector2.Dot(vector2_5 - vertex1, vector2_6) / num4;
      if ((double) num5 < 0.0 || 1.0 < (double) num5)
        return false;
      output.Fraction = num3;
      output.Normal = (double) num1 <= 0.0 ? vector2_4 : -vector2_4;
      return true;
    }

    public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
    {
      Vector2 vector2_1 = MathUtils.Multiply(ref transform, this._vertex1);
      Vector2 vector2_2 = MathUtils.Multiply(ref transform, this._vertex2);
      Vector2 vector2_3 = Vector2.Min(vector2_1, vector2_2);
      Vector2 vector2_4 = Vector2.Max(vector2_1, vector2_2);
      Vector2 vector2_5 = new Vector2(this.Radius, this.Radius);
      aabb.LowerBound = vector2_3 - vector2_5;
      aabb.UpperBound = vector2_4 + vector2_5;
    }

    public override void ComputeProperties()
    {
      this.MassData.Centroid = 0.5f * (this._vertex1 + this._vertex2);
    }

    public override float ComputeSubmergedArea(
      Vector2 normal,
      float offset,
      Transform xf,
      out Vector2 sc)
    {
      sc = Vector2.Zero;
      return 0.0f;
    }

    public bool CompareTo(EdgeShape shape)
    {
      return this.HasVertex0 == shape.HasVertex0 && this.HasVertex3 == shape.HasVertex3 && this.Vertex0 == shape.Vertex0 && this.Vertex1 == shape.Vertex1 && this.Vertex2 == shape.Vertex2 && this.Vertex3 == shape.Vertex3;
    }
  }
}
