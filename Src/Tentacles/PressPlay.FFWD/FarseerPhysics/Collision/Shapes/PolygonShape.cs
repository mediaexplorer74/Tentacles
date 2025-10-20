// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.PolygonShape
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public class PolygonShape : Shape
  {
    public Vertices Normals;
    public Vertices Vertices;

    public PolygonShape(Vertices vertices, float density)
      : base(density)
    {
      this.ShapeType = ShapeType.Polygon;
      this._radius = 0.01f;
      this.Set(vertices);
    }

    public PolygonShape(float density)
      : base(density)
    {
      this.ShapeType = ShapeType.Polygon;
      this._radius = 0.01f;
      this.Normals = new Vertices();
      this.Vertices = new Vertices();
    }

    internal PolygonShape()
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Polygon;
      this._radius = 0.01f;
      this.Normals = new Vertices();
      this.Vertices = new Vertices();
    }

    public override int ChildCount => 1;

    public override Shape Clone()
    {
      PolygonShape polygonShape = new PolygonShape();
      polygonShape.ShapeType = this.ShapeType;
      polygonShape._radius = this._radius;
      polygonShape._density = this._density;
      polygonShape.Vertices = new Vertices((IList<Vector2>) this.Vertices);
      polygonShape.Normals = new Vertices((IList<Vector2>) this.Normals);
      polygonShape.MassData = this.MassData;
      return (Shape) polygonShape;
    }

    public void Set(Vertices vertices)
    {
      this.Vertices = new Vertices((IList<Vector2>) vertices);
      this.Normals = new Vertices(vertices.Count);
      for (int index1 = 0; index1 < vertices.Count; ++index1)
      {
        int index2 = index1;
        Vector2 vector2_1 = this.Vertices[index1 + 1 < vertices.Count ? index1 + 1 : 0] - this.Vertices[index2];
        Vector2 vector2_2 = new Vector2(vector2_1.Y, -vector2_1.X);
        vector2_2.Normalize();
        this.Normals.Add(vector2_2);
      }
      this.ComputeProperties();
    }

    public override void ComputeProperties()
    {
      if ((double) this._density <= 0.0)
        return;
      Vector2 zero1 = Vector2.Zero;
      float num1 = 0.0f;
      float num2 = 0.0f;
      Vector2 zero2 = Vector2.Zero;
      for (int index = 0; index < this.Vertices.Count; ++index)
      {
        Vector2 vector2_1 = zero2;
        Vector2 vertex = this.Vertices[index];
        Vector2 vector2_2 = index + 1 < this.Vertices.Count ? this.Vertices[index + 1] : this.Vertices[0];
        Vector2 a = vertex - vector2_1;
        Vector2 b = vector2_2 - vector2_1;
        float c;
        MathUtils.Cross(ref a, ref b, out c);
        float num3 = 0.5f * c;
        num1 += num3;
        zero1 += num3 * 0.333333343f * (vector2_1 + vertex + vector2_2);
        float x1 = vector2_1.X;
        float y1 = vector2_1.Y;
        float x2 = a.X;
        float y2 = a.Y;
        float x3 = b.X;
        float y3 = b.Y;
        float num4 = (float) (0.3333333432674408 * (0.25 * ((double) x2 * (double) x2 + (double) x3 * (double) x2 + (double) x3 * (double) x3) + ((double) x1 * (double) x2 + (double) x1 * (double) x3)) + 0.5 * (double) x1 * (double) x1);
        float num5 = (float) (0.3333333432674408 * (0.25 * ((double) y2 * (double) y2 + (double) y3 * (double) y2 + (double) y3 * (double) y3) + ((double) y1 * (double) y2 + (double) y1 * (double) y3)) + 0.5 * (double) y1 * (double) y1);
        num2 += c * (num4 + num5);
      }
      this.MassData.Area = num1;
      this.MassData.Mass = this._density * num1;
      this.MassData.Centroid = zero1 * (1f / num1);
      this.MassData.Inertia = this._density * num2;
    }

    public void SetAsBox(float halfWidth, float halfHeight)
    {
      this.Set(PolygonTools.CreateRectangle(halfWidth, halfHeight));
    }

    public void SetAsBox(float halfWidth, float halfHeight, Vector2 center, float angle)
    {
      this.Set(PolygonTools.CreateRectangle(halfWidth, halfHeight, center, angle));
    }

    public override bool TestPoint(ref Transform transform, ref Vector2 point)
    {
      Vector2 vector2 = MathUtils.MultiplyT(ref transform.R, point - transform.Position);
      for (int index = 0; index < this.Vertices.Count; ++index)
      {
        if ((double) Vector2.Dot(this.Normals[index], vector2 - this.Vertices[index]) > 0.0)
          return false;
      }
      return true;
    }

    public override bool RayCast(
      out RayCastOutput output,
      ref RayCastInput input,
      ref Transform transform,
      int childIndex)
    {
      output = new RayCastOutput();
      Vector2 vector2_1 = MathUtils.MultiplyT(ref transform.R, input.Point1 - transform.Position);
      Vector2 vector2_2 = MathUtils.MultiplyT(ref transform.R, input.Point2 - transform.Position) - vector2_1;
      float num1 = 0.0f;
      float num2 = input.MaxFraction;
      int index1 = -1;
      for (int index2 = 0; index2 < this.Vertices.Count; ++index2)
      {
        float num3 = Vector2.Dot(this.Normals[index2], this.Vertices[index2] - vector2_1);
        float num4 = Vector2.Dot(this.Normals[index2], vector2_2);
        if ((double) num4 == 0.0)
        {
          if ((double) num3 < 0.0)
            return false;
        }
        else if ((double) num4 < 0.0 && (double) num3 < (double) num1 * (double) num4)
        {
          num1 = num3 / num4;
          index1 = index2;
        }
        else if ((double) num4 > 0.0 && (double) num3 < (double) num2 * (double) num4)
          num2 = num3 / num4;
        if ((double) num2 < (double) num1)
          return false;
      }
      if (index1 < 0)
        return false;
      output.Fraction = num1;
      output.Normal = MathUtils.Multiply(ref transform.R, this.Normals[index1]);
      return true;
    }

    public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
    {
      Vector2 vector2_1 = MathUtils.Multiply(ref transform, this.Vertices[0]);
      Vector2 vector2_2 = vector2_1;
      for (int index = 1; index < this.Vertices.Count; ++index)
      {
        Vector2 vector2_3 = MathUtils.Multiply(ref transform, this.Vertices[index]);
        vector2_1 = Vector2.Min(vector2_1, vector2_3);
        vector2_2 = Vector2.Max(vector2_2, vector2_3);
      }
      Vector2 vector2_4 = new Vector2(this.Radius, this.Radius);
      aabb.LowerBound = vector2_1 - vector2_4;
      aabb.UpperBound = vector2_2 + vector2_4;
    }

    public bool CompareTo(PolygonShape shape)
    {
      if (this.Vertices.Count != shape.Vertices.Count)
        return false;
      for (int index = 0; index < this.Vertices.Count; ++index)
      {
        if (this.Vertices[index] != shape.Vertices[index])
          return false;
      }
      return (double) this.Radius == (double) shape.Radius && this.MassData == shape.MassData;
    }

    public override float ComputeSubmergedArea(
      Vector2 normal,
      float offset,
      Transform xf,
      out Vector2 sc)
    {
      sc = Vector2.Zero;
      Vector2 vector2_1 = MathUtils.MultiplyT(ref xf.R, normal);
      float num1 = offset - Vector2.Dot(normal, xf.Position);
      float[] numArray = new float[Settings.MaxPolygonVertices];
      int num2 = 0;
      int index1 = -1;
      int index2 = -1;
      bool flag1 = false;
      for (int index3 = 0; index3 < this.Vertices.Count; ++index3)
      {
        numArray[index3] = Vector2.Dot(vector2_1, this.Vertices[index3]) - num1;
        bool flag2 = (double) numArray[index3] < -1.1920928955078125E-07;
        if (index3 > 0)
        {
          if (flag2)
          {
            if (!flag1)
            {
              index1 = index3 - 1;
              ++num2;
            }
          }
          else if (flag1)
          {
            index2 = index3 - 1;
            ++num2;
          }
        }
        flag1 = flag2;
      }
      switch (num2)
      {
        case 0:
          if (!flag1)
            return 0.0f;
          sc = MathUtils.Multiply(ref xf, this.MassData.Centroid);
          return this.MassData.Mass / this.Density;
        case 1:
          if (index1 == -1)
          {
            index1 = this.Vertices.Count - 1;
            break;
          }
          index2 = this.Vertices.Count - 1;
          break;
      }
      int index4 = (index1 + 1) % this.Vertices.Count;
      int index5 = (index2 + 1) % this.Vertices.Count;
      float num3 = (float) ((0.0 - (double) numArray[index1]) / ((double) numArray[index4] - (double) numArray[index1]));
      float num4 = (float) ((0.0 - (double) numArray[index2]) / ((double) numArray[index5] - (double) numArray[index2]));
      Vector2 vector2_2 = new Vector2((float) ((double) this.Vertices[index1].X * (1.0 - (double) num3) + (double) this.Vertices[index4].X * (double) num3), (float) ((double) this.Vertices[index1].Y * (1.0 - (double) num3) + (double) this.Vertices[index4].Y * (double) num3));
      Vector2 vector2_3 = new Vector2((float) ((double) this.Vertices[index2].X * (1.0 - (double) num4) + (double) this.Vertices[index5].X * (double) num4), (float) ((double) this.Vertices[index2].Y * (1.0 - (double) num4) + (double) this.Vertices[index5].Y * (double) num4));
      float submergedArea = 0.0f;
      Vector2 vector2_4 = new Vector2(0.0f, 0.0f);
      Vector2 vector2_5 = this.Vertices[index4];
      float num5 = 0.333333343f;
      int index6 = index4;
      while (index6 != index5)
      {
        index6 = (index6 + 1) % this.Vertices.Count;
        Vector2 vector2_6 = index6 != index5 ? this.Vertices[index6] : vector2_3;
        float num6 = 0.5f * MathUtils.Cross(vector2_5 - vector2_2, vector2_6 - vector2_2);
        submergedArea += num6;
        vector2_4 += num6 * num5 * (vector2_2 + vector2_5 + vector2_6);
        vector2_5 = vector2_6;
      }
      Vector2 v = vector2_4 * (1f / submergedArea);
      sc = MathUtils.Multiply(ref xf, v);
      return submergedArea;
    }
  }
}
