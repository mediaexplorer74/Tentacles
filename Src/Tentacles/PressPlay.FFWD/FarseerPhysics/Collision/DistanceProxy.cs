// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.DistanceProxy
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision
{
  public class DistanceProxy
  {
    internal float Radius;
    internal Vertices Vertices = new Vertices();

    public void Set(Shape shape, int index)
    {
      switch (shape.ShapeType)
      {
        case ShapeType.Circle:
          CircleShape circleShape = (CircleShape) shape;
          this.Vertices.Clear();
          this.Vertices.Add(circleShape.Position);
          this.Radius = circleShape.Radius;
          break;
        case ShapeType.Edge:
          EdgeShape edgeShape = (EdgeShape) shape;
          this.Vertices.Clear();
          this.Vertices.Add(edgeShape.Vertex1);
          this.Vertices.Add(edgeShape.Vertex2);
          this.Radius = edgeShape.Radius;
          break;
        case ShapeType.Polygon:
          PolygonShape polygonShape = (PolygonShape) shape;
          this.Vertices.Clear();
          for (int index1 = 0; index1 < polygonShape.Vertices.Count; ++index1)
            this.Vertices.Add(polygonShape.Vertices[index1]);
          this.Radius = polygonShape.Radius;
          break;
        case ShapeType.Loop:
          LoopShape loopShape = (LoopShape) shape;
          this.Vertices.Clear();
          this.Vertices.Add(loopShape.Vertices[index]);
          this.Vertices.Add(index + 1 < loopShape.Vertices.Count ? loopShape.Vertices[index + 1] : loopShape.Vertices[0]);
          this.Radius = loopShape.Radius;
          break;
      }
    }

    public int GetSupport(Vector2 direction)
    {
      int support = 0;
      float num1 = Vector2.Dot(this.Vertices[0], direction);
      for (int index = 1; index < this.Vertices.Count; ++index)
      {
        float num2 = Vector2.Dot(this.Vertices[index], direction);
        if ((double) num2 > (double) num1)
        {
          support = index;
          num1 = num2;
        }
      }
      return support;
    }

    public Vector2 GetSupportVertex(Vector2 direction)
    {
      int index1 = 0;
      float num1 = Vector2.Dot(this.Vertices[0], direction);
      for (int index2 = 1; index2 < this.Vertices.Count; ++index2)
      {
        float num2 = Vector2.Dot(this.Vertices[index2], direction);
        if ((double) num2 > (double) num1)
        {
          index1 = index2;
          num1 = num2;
        }
      }
      return this.Vertices[index1];
    }
  }
}
