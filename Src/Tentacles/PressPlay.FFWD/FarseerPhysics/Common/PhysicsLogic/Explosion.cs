// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PhysicsLogic.Explosion
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FarseerPhysics.Common.PhysicsLogic
{
  public sealed class Explosion : FarseerPhysics.Common.PhysicsLogic.PhysicsLogic
  {
    private const float MaxEdgeOffset = 0.0349065848f;
    public float EdgeRatio = 0.025f;
    public bool IgnoreWhenInsideShape;
    public float MaxAngle = 0.209439516f;
    public int MaxShapes = 100;
    public int MinRays = 5;
    private List<ShapeData> _data = new List<ShapeData>();
    private Dictionary<Fixture, List<Vector2>> _exploded;
    private RayDataComparer _rdc;

    public Explosion(World world)
      : base(world, PhysicsLogicType.Explosion)
    {
      this._exploded = new Dictionary<Fixture, List<Vector2>>();
      this._rdc = new RayDataComparer();
      this._data = new List<ShapeData>();
    }

    public Dictionary<Fixture, List<Vector2>> Activate(Vector2 pos, float radius, float maxForce)
    {
      this._exploded.Clear();
      AABB aabb;
      aabb.LowerBound = pos + new Vector2(-radius, -radius);
      aabb.UpperBound = pos + new Vector2(radius, radius);
      Fixture[] shapes = new Fixture[this.MaxShapes];
      Fixture[] containedShapes = new Fixture[5];
      bool exit = false;
      int shapeCount = 0;
      int containedShapeCount = 0;
      this.World.QueryAABB((Func<Fixture, bool>) (fixture =>
      {
        if (fixture.TestPoint(ref pos))
        {
          if (this.IgnoreWhenInsideShape)
            exit = true;
          else
            containedShapes[containedShapeCount++] = fixture;
        }
        else
          shapes[shapeCount++] = fixture;
        return true;
      }), ref aabb);
      if (exit)
        return this._exploded;
      float[] array = new float[shapeCount * 2];
      int length = 0;
      for (int index1 = 0; index1 < shapeCount; ++index1)
      {
        PolygonShape polygonShape;
        if (shapes[index1].Shape is CircleShape shape)
        {
          Vertices vertices = new Vertices();
          Vector2 vector2_1 = Vector2.Zero + new Vector2(shape.Radius, 0.0f);
          vertices.Add(vector2_1);
          Vector2 vector2_2 = Vector2.Zero + new Vector2(0.0f, shape.Radius);
          vertices.Add(vector2_2);
          Vector2 vector2_3 = Vector2.Zero + new Vector2(-shape.Radius, shape.Radius);
          vertices.Add(vector2_3);
          Vector2 vector2_4 = Vector2.Zero + new Vector2(0.0f, -shape.Radius);
          vertices.Add(vector2_4);
          polygonShape = new PolygonShape(vertices, 0.0f);
        }
        else
          polygonShape = shapes[index1].Shape as PolygonShape;
        if (shapes[index1].Body.BodyType == BodyType.Dynamic && polygonShape != null)
        {
          Vector2 vector2_5 = shapes[index1].Body.GetWorldPoint(polygonShape.MassData.Centroid) - pos;
          float num1 = (float) Math.Atan2((double) vector2_5.Y, (double) vector2_5.X);
          float num2 = float.MaxValue;
          float num3 = float.MinValue;
          float num4 = 0.0f;
          float num5 = 0.0f;
          for (int index2 = 0; index2 < polygonShape.Vertices.Count<Vector2>(); ++index2)
          {
            Vector2 vector2_6 = shapes[index1].Body.GetWorldPoint(polygonShape.Vertices[index2]) - pos;
            float num6 = (float) Math.Atan2((double) vector2_6.Y, (double) vector2_6.X);
            float num7 = (float) (((double) (num6 - num1) - 3.1415927410125732) % 6.2831854820251465);
            if ((double) num7 < 0.0)
              num7 += 6.28318548f;
            float num8 = num7 - 3.14159274f;
            if ((double) Math.Abs(num8) > 3.1415927410125732)
              throw new ArgumentException("OMG!");
            if ((double) num8 > (double) num3)
            {
              num3 = num8;
              num5 = num6;
            }
            if ((double) num8 < (double) num2)
            {
              num2 = num8;
              num4 = num6;
            }
          }
          array[length] = num4;
          int index3 = length + 1;
          array[index3] = num5;
          length = index3 + 1;
        }
      }
      Array.Sort<float>(array, 0, length, (IComparer<float>) this._rdc);
      this._data.Clear();
      bool flag = true;
      for (int index4 = 0; index4 < length; ++index4)
      {
        Fixture shape = (Fixture) null;
        int index5 = index4 == length - 1 ? 0 : index4 + 1;
        if ((double) array[index4] != (double) array[index5])
        {
          float num = (index4 != length - 1 ? array[index4 + 1] + array[index4] : array[0] + 6.28318548f + array[index4]) / 2f;
          Vector2 point1 = pos;
          Vector2 point2 = radius * new Vector2((float) Math.Cos((double) num), (float) Math.Sin((double) num)) + pos;
          bool hitClosest = false;
          this.World.RayCast((RayCastCallback) ((f, p, n, fr) =>
          {
            if (!this.IsActiveOn(f.Body))
              return 0.0f;
            hitClosest = true;
            shape = f;
            return fr;
          }), point1, point2);
          if (hitClosest && shape.Body.BodyType == BodyType.Dynamic)
          {
            if (this._data.Count<ShapeData>() > 0 && this._data.Last<ShapeData>().Body == shape.Body && !flag)
            {
              int index6 = this._data.Count - 1;
              ShapeData shapeData = this._data[index6] with
              {
                Max = array[index5]
              };
              this._data[index6] = shapeData;
            }
            else
            {
              ShapeData shapeData;
              shapeData.Body = shape.Body;
              shapeData.Min = array[index4];
              shapeData.Max = array[index5];
              this._data.Add(shapeData);
            }
            if (this._data.Count<ShapeData>() > 1 && index4 == length - 1 && this._data.Last<ShapeData>().Body == this._data.First<ShapeData>().Body && (double) this._data.Last<ShapeData>().Max == (double) this._data.First<ShapeData>().Min)
            {
              ShapeData shapeData = this._data[0] with
              {
                Min = this._data.Last<ShapeData>().Min
              };
              this._data.RemoveAt(this._data.Count<ShapeData>() - 1);
              this._data[0] = shapeData;
              while ((double) this._data.First<ShapeData>().Min >= (double) this._data.First<ShapeData>().Max)
              {
                shapeData.Min -= 6.28318548f;
                this._data[0] = shapeData;
              }
            }
            int index7 = this._data.Count - 1;
            ShapeData shapeData1 = this._data[index7];
            while (this._data.Count<ShapeData>() > 0 && (double) this._data.Last<ShapeData>().Min >= (double) this._data.Last<ShapeData>().Max)
            {
              shapeData1.Min = this._data.Last<ShapeData>().Min - 6.28318548f;
              this._data[index7] = shapeData1;
            }
            flag = false;
          }
          else
            flag = true;
        }
      }
      for (int index8 = 0; index8 < this._data.Count<ShapeData>(); ++index8)
      {
        if (this.IsActiveOn(this._data[index8].Body))
        {
          float num9 = this._data[index8].Max - this._data[index8].Min;
          float num10 = MathHelper.Min((float) Math.PI / 90f, this.EdgeRatio * num9);
          int num11 = (int) Math.Ceiling(((double) num9 - 2.0 * (double) num10 - (double) (this.MinRays - 1) * (double) this.MaxAngle) / (double) this.MaxAngle);
          if (num11 < 0)
            num11 = 0;
          float num12 = (float) (((double) num9 - (double) num10 * 2.0) / ((double) this.MinRays + (double) num11 - 1.0));
          for (float num13 = this._data[index8].Min + num10; (double) num13 < (double) this._data[index8].Max || MathUtils.FloatEquals(num13, this._data[index8].Max, 0.0001f); num13 += num12)
          {
            Vector2 vector2_7 = pos;
            Vector2 vector2_8 = pos + radius * new Vector2((float) Math.Cos((double) num13), (float) Math.Sin((double) num13));
            Vector2 point = Vector2.Zero;
            float val2 = float.MaxValue;
            List<Fixture> fixtureList = this._data[index8].Body.FixtureList;
            for (int index9 = 0; index9 < fixtureList.Count; ++index9)
            {
              Fixture key = fixtureList[index9];
              RayCastInput input;
              input.Point1 = vector2_7;
              input.Point2 = vector2_8;
              input.MaxFraction = 50f;
              RayCastOutput output;
              if (key.RayCast(out output, ref input, 0) && (double) val2 > (double) output.Fraction)
              {
                val2 = output.Fraction;
                point = output.Fraction * vector2_8 + (1f - output.Fraction) * vector2_7;
              }
              Vector2 impulse = Vector2.Dot((float) ((double) num9 / (double) (this.MinRays + num11) * (double) maxForce * 180.0 / 3.1415927410125732 * (1.0 - (double) Math.Min(1f, val2))) * new Vector2((float) Math.Cos((double) num13), (float) Math.Sin((double) num13)), -output.Normal) * new Vector2((float) Math.Cos((double) num13), (float) Math.Sin((double) num13));
              this._data[index8].Body.ApplyLinearImpulse(ref impulse, ref point);
              Vector2 zero = Vector2.Zero;
              List<Vector2> vector2List;
              if (this._exploded.TryGetValue(key, out vector2List))
              {
                zero.X += Math.Abs(impulse.X);
                zero.Y += Math.Abs(impulse.Y);
                vector2List.Add(zero);
              }
              else
              {
                vector2List = new List<Vector2>();
                zero.X = Math.Abs(impulse.X);
                zero.Y = Math.Abs(impulse.Y);
                vector2List.Add(zero);
                this._exploded.Add(key, vector2List);
              }
              if ((double) val2 > 1.0)
                point = vector2_8;
            }
          }
        }
      }
      for (int index = 0; index < containedShapeCount; ++index)
      {
        Fixture key = containedShapes[index];
        if (this.IsActiveOn(key.Body))
        {
          float num = (float) ((double) this.MinRays * (double) maxForce * 180.0 / 3.1415927410125732);
          Vector2 worldPoint;
          if (key.Shape is CircleShape shape1)
          {
            worldPoint = key.Body.GetWorldPoint(shape1.Position);
          }
          else
          {
            PolygonShape shape = key.Shape as PolygonShape;
            worldPoint = key.Body.GetWorldPoint(shape.MassData.Centroid);
          }
          Vector2 impulse = num * (worldPoint - pos);
          List<Vector2> vector2List = new List<Vector2>();
          vector2List.Add(impulse);
          key.Body.ApplyLinearImpulse(ref impulse, ref worldPoint);
          if (!this._exploded.ContainsKey(key))
            this._exploded.Add(key, vector2List);
        }
      }
      return this._exploded;
    }
  }
}
