// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Shapes.LoopShape
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Collision.Shapes
{
  public class LoopShape : Shape
  {
    private static EdgeShape _edgeShape = new EdgeShape();
    public Vertices Vertices;

    private LoopShape()
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Loop;
      this._radius = 0.01f;
    }

    public LoopShape(Vertices vertices)
      : base(0.0f)
    {
      this.ShapeType = ShapeType.Loop;
      this._radius = 0.01f;
      this.Vertices = new Vertices((IList<Vector2>) vertices);
    }

    public override int ChildCount => this.Vertices.Count;

    public override Shape Clone()
    {
      LoopShape loopShape = new LoopShape();
      loopShape._density = this._density;
      loopShape._radius = this._radius;
      loopShape.Vertices = this.Vertices;
      loopShape.MassData = this.MassData;
      return (Shape) loopShape;
    }

    public void GetChildEdge(ref EdgeShape edge, int index)
    {
      edge.ShapeType = ShapeType.Edge;
      edge._radius = this._radius;
      edge.HasVertex0 = true;
      edge.HasVertex3 = true;
      int index1 = index - 1 >= 0 ? index - 1 : this.Vertices.Count - 1;
      int index2 = index;
      int index3 = index + 1 < this.Vertices.Count ? index + 1 : 0;
      int index4 = index + 2;
      while (index4 >= this.Vertices.Count)
        index4 -= this.Vertices.Count;
      edge.Vertex0 = this.Vertices[index1];
      edge.Vertex1 = this.Vertices[index2];
      edge.Vertex2 = this.Vertices[index3];
      edge.Vertex3 = this.Vertices[index4];
    }

    public override bool TestPoint(ref Transform transform, ref Vector2 point) => false;

    public override bool RayCast(
      out RayCastOutput output,
      ref RayCastInput input,
      ref Transform transform,
      int childIndex)
    {
      int index1 = childIndex;
      int index2 = childIndex + 1;
      if (index2 == this.Vertices.Count)
        index2 = 0;
      LoopShape._edgeShape.Vertex1 = this.Vertices[index1];
      LoopShape._edgeShape.Vertex2 = this.Vertices[index2];
      return LoopShape._edgeShape.RayCast(out output, ref input, ref transform, 0);
    }

    public override void ComputeAABB(out AABB aabb, ref Transform transform, int childIndex)
    {
      int index1 = childIndex;
      int index2 = childIndex + 1;
      if (index2 == this.Vertices.Count)
        index2 = 0;
      Vector2 vector2_1 = MathUtils.Multiply(ref transform, this.Vertices[index1]);
      Vector2 vector2_2 = MathUtils.Multiply(ref transform, this.Vertices[index2]);
      aabb.LowerBound = Vector2.Min(vector2_1, vector2_2);
      aabb.UpperBound = Vector2.Max(vector2_1, vector2_2);
    }

    public override void ComputeProperties()
    {
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
  }
}
