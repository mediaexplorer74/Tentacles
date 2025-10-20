// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Simplex
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  internal struct Simplex
  {
    internal int Count;
    internal FixedArray3<SimplexVertex> V;

    internal void ReadCache(
      ref SimplexCache cache,
      DistanceProxy proxyA,
      ref Transform transformA,
      DistanceProxy proxyB,
      ref Transform transformB)
    {
      this.Count = (int) cache.Count;
      for (int index = 0; index < this.Count; ++index)
      {
        SimplexVertex simplexVertex = this.V[index] with
        {
          IndexA = (int) cache.IndexA[index],
          IndexB = (int) cache.IndexB[index]
        };
        Vector2 vertex1 = proxyA.Vertices[simplexVertex.IndexA];
        Vector2 vertex2 = proxyB.Vertices[simplexVertex.IndexB];
        simplexVertex.WA = MathUtils.Multiply(ref transformA, vertex1);
        simplexVertex.WB = MathUtils.Multiply(ref transformB, vertex2);
        simplexVertex.W = simplexVertex.WB - simplexVertex.WA;
        simplexVertex.A = 0.0f;
        this.V[index] = simplexVertex;
      }
      if (this.Count > 1)
      {
        float metric1 = cache.Metric;
        float metric2 = this.GetMetric();
        if ((double) metric2 < 0.5 * (double) metric1 || 2.0 * (double) metric1 < (double) metric2 || (double) metric2 < 1.1920928955078125E-07)
          this.Count = 0;
      }
      if (this.Count != 0)
        return;
      SimplexVertex simplexVertex1 = this.V[0] with
      {
        IndexA = 0,
        IndexB = 0
      };
      Vector2 vertex3 = proxyA.Vertices[0];
      Vector2 vertex4 = proxyB.Vertices[0];
      simplexVertex1.WA = MathUtils.Multiply(ref transformA, vertex3);
      simplexVertex1.WB = MathUtils.Multiply(ref transformB, vertex4);
      simplexVertex1.W = simplexVertex1.WB - simplexVertex1.WA;
      this.V[0] = simplexVertex1;
      this.Count = 1;
    }

    internal void WriteCache(ref SimplexCache cache)
    {
      cache.Metric = this.GetMetric();
      cache.Count = (ushort) this.Count;
      for (int index = 0; index < this.Count; ++index)
      {
        cache.IndexA[index] = (byte) this.V[index].IndexA;
        cache.IndexB[index] = (byte) this.V[index].IndexB;
      }
    }

    internal Vector2 GetSearchDirection()
    {
      switch (this.Count)
      {
        case 1:
          return -this.V[0].W;
        case 2:
          Vector2 a = this.V[1].W - this.V[0].W;
          return (double) MathUtils.Cross(a, -this.V[0].W) > 0.0 ? new Vector2(-a.Y, a.X) : new Vector2(a.Y, -a.X);
        default:
          return Vector2.Zero;
      }
    }

    internal Vector2 GetClosestPoint()
    {
      switch (this.Count)
      {
        case 0:
          return Vector2.Zero;
        case 1:
          return this.V[0].W;
        case 2:
          return this.V[0].A * this.V[0].W + this.V[1].A * this.V[1].W;
        case 3:
          return Vector2.Zero;
        default:
          return Vector2.Zero;
      }
    }

    internal void GetWitnessPoints(out Vector2 pA, out Vector2 pB)
    {
      switch (this.Count)
      {
        case 0:
          pA = Vector2.Zero;
          pB = Vector2.Zero;
          break;
        case 1:
          pA = this.V[0].WA;
          pB = this.V[0].WB;
          break;
        case 2:
          pA = this.V[0].A * this.V[0].WA + this.V[1].A * this.V[1].WA;
          pB = this.V[0].A * this.V[0].WB + this.V[1].A * this.V[1].WB;
          break;
        case 3:
          pA = this.V[0].A * this.V[0].WA + this.V[1].A * this.V[1].WA + this.V[2].A * this.V[2].WA;
          pB = pA;
          break;
        default:
          throw new Exception();
      }
    }

    internal float GetMetric()
    {
      switch (this.Count)
      {
        case 0:
          return 0.0f;
        case 1:
          return 0.0f;
        case 2:
          return (this.V[0].W - this.V[1].W).Length();
        case 3:
          return MathUtils.Cross(this.V[1].W - this.V[0].W, this.V[2].W - this.V[0].W);
        default:
          return 0.0f;
      }
    }

    internal void Solve2()
    {
      Vector2 w1 = this.V[0].W;
      Vector2 w2 = this.V[1].W;
      Vector2 vector2 = w2 - w1;
      float num1 = -Vector2.Dot(w1, vector2);
      if ((double) num1 <= 0.0)
      {
        this.V[0] = this.V[0] with { A = 1f };
        this.Count = 1;
      }
      else
      {
        float num2 = Vector2.Dot(w2, vector2);
        if ((double) num2 <= 0.0)
        {
          this.V[1] = this.V[1] with { A = 1f };
          this.Count = 1;
          this.V[0] = this.V[1];
        }
        else
        {
          float num3 = (float) (1.0 / ((double) num2 + (double) num1));
          SimplexVertex simplexVertex1 = this.V[0];
          SimplexVertex simplexVertex2 = this.V[1];
          simplexVertex1.A = num2 * num3;
          simplexVertex2.A = num1 * num3;
          this.V[0] = simplexVertex1;
          this.V[1] = simplexVertex2;
          this.Count = 2;
        }
      }
    }

    internal void Solve3()
    {
      Vector2 w1 = this.V[0].W;
      Vector2 w2 = this.V[1].W;
      Vector2 w3 = this.V[2].W;
      Vector2 a = w2 - w1;
      float num1 = Vector2.Dot(w1, a);
      float num2 = Vector2.Dot(w2, a);
      float num3 = -num1;
      Vector2 b = w3 - w1;
      float num4 = Vector2.Dot(w1, b);
      float num5 = Vector2.Dot(w3, b);
      float num6 = -num4;
      Vector2 vector2 = w3 - w2;
      float num7 = Vector2.Dot(w2, vector2);
      float num8 = Vector2.Dot(w3, vector2);
      float num9 = -num7;
      float num10 = MathUtils.Cross(a, b);
      float num11 = num10 * MathUtils.Cross(w2, w3);
      float num12 = num10 * MathUtils.Cross(w3, w1);
      float num13 = num10 * MathUtils.Cross(w1, w2);
      if ((double) num3 <= 0.0 && (double) num6 <= 0.0)
      {
        this.V[0] = this.V[0] with { A = 1f };
        this.Count = 1;
      }
      else if ((double) num2 > 0.0 && (double) num3 > 0.0 && (double) num13 <= 0.0)
      {
        float num14 = (float) (1.0 / ((double) num2 + (double) num3));
        SimplexVertex simplexVertex1 = this.V[0];
        SimplexVertex simplexVertex2 = this.V[1];
        simplexVertex1.A = num2 * num14;
        simplexVertex2.A = num3 * num14;
        this.V[0] = simplexVertex1;
        this.V[1] = simplexVertex2;
        this.Count = 2;
      }
      else if ((double) num5 > 0.0 && (double) num6 > 0.0 && (double) num12 <= 0.0)
      {
        float num15 = (float) (1.0 / ((double) num5 + (double) num6));
        SimplexVertex simplexVertex3 = this.V[0];
        SimplexVertex simplexVertex4 = this.V[2];
        simplexVertex3.A = num5 * num15;
        simplexVertex4.A = num6 * num15;
        this.V[0] = simplexVertex3;
        this.V[2] = simplexVertex4;
        this.Count = 2;
        this.V[1] = this.V[2];
      }
      else if ((double) num2 <= 0.0 && (double) num9 <= 0.0)
      {
        this.V[1] = this.V[1] with { A = 1f };
        this.Count = 1;
        this.V[0] = this.V[1];
      }
      else if ((double) num5 <= 0.0 && (double) num8 <= 0.0)
      {
        this.V[2] = this.V[2] with { A = 1f };
        this.Count = 1;
        this.V[0] = this.V[2];
      }
      else if ((double) num8 > 0.0 && (double) num9 > 0.0 && (double) num11 <= 0.0)
      {
        float num16 = (float) (1.0 / ((double) num8 + (double) num9));
        SimplexVertex simplexVertex5 = this.V[1];
        SimplexVertex simplexVertex6 = this.V[2];
        simplexVertex5.A = num8 * num16;
        simplexVertex6.A = num9 * num16;
        this.V[1] = simplexVertex5;
        this.V[2] = simplexVertex6;
        this.Count = 2;
        this.V[0] = this.V[2];
      }
      else
      {
        float num17 = (float) (1.0 / ((double) num11 + (double) num12 + (double) num13));
        SimplexVertex simplexVertex7 = this.V[0];
        SimplexVertex simplexVertex8 = this.V[1];
        SimplexVertex simplexVertex9 = this.V[2];
        simplexVertex7.A = num11 * num17;
        simplexVertex8.A = num12 * num17;
        simplexVertex9.A = num13 * num17;
        this.V[0] = simplexVertex7;
        this.V[1] = simplexVertex8;
        this.V[2] = simplexVertex9;
        this.Count = 3;
      }
    }
  }
}
