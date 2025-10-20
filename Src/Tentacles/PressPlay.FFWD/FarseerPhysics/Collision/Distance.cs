// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Distance
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  public static class Distance
  {
    public static int GJKCalls;
    public static int GJKIters;
    public static int GJKMaxIters;

    public static void ComputeDistance(
      out DistanceOutput output,
      out SimplexCache cache,
      DistanceInput input)
    {
      cache = new SimplexCache();
      ++Distance.GJKCalls;
      Simplex simplex = new Simplex();
      simplex.ReadCache(ref cache, input.ProxyA, ref input.TransformA, input.ProxyB, ref input.TransformB);
      FixedArray3<int> fixedArray3_1 = new FixedArray3<int>();
      FixedArray3<int> fixedArray3_2 = new FixedArray3<int>();
      float num = simplex.GetClosestPoint().LengthSquared();
      int val2 = 0;
      while (val2 < 20)
      {
        int count = simplex.Count;
        for (int index = 0; index < count; ++index)
        {
          fixedArray3_1[index] = simplex.V[index].IndexA;
          fixedArray3_2[index] = simplex.V[index].IndexB;
        }
        switch (simplex.Count)
        {
          case 2:
            simplex.Solve2();
            break;
          case 3:
            simplex.Solve3();
            break;
        }
        if (simplex.Count != 3)
        {
          num = simplex.GetClosestPoint().LengthSquared();
          Vector2 searchDirection = simplex.GetSearchDirection();
          if ((double) searchDirection.LengthSquared() >= 1.4210854715202004E-14)
          {
            SimplexVertex simplexVertex = simplex.V[simplex.Count] with
            {
              IndexA = input.ProxyA.GetSupport(MathUtils.MultiplyT(ref input.TransformA.R, -searchDirection))
            };
            simplexVertex.WA = MathUtils.Multiply(ref input.TransformA, input.ProxyA.Vertices[simplexVertex.IndexA]);
            simplexVertex.IndexB = input.ProxyB.GetSupport(MathUtils.MultiplyT(ref input.TransformB.R, searchDirection));
            simplexVertex.WB = MathUtils.Multiply(ref input.TransformB, input.ProxyB.Vertices[simplexVertex.IndexB]);
            simplexVertex.W = simplexVertex.WB - simplexVertex.WA;
            simplex.V[simplex.Count] = simplexVertex;
            ++val2;
            ++Distance.GJKIters;
            bool flag = false;
            for (int index = 0; index < count; ++index)
            {
              if (simplexVertex.IndexA == fixedArray3_1[index] && simplexVertex.IndexB == fixedArray3_2[index])
              {
                flag = true;
                break;
              }
            }
            if (!flag)
              ++simplex.Count;
            else
              break;
          }
          else
            break;
        }
        else
          break;
      }
      Distance.GJKMaxIters = Math.Max(Distance.GJKMaxIters, val2);
      simplex.GetWitnessPoints(out output.PointA, out output.PointB);
      output.Distance = (output.PointA - output.PointB).Length();
      output.Iterations = val2;
      simplex.WriteCache(ref cache);
      if (!input.UseRadii)
        return;
      float radius1 = input.ProxyA.Radius;
      float radius2 = input.ProxyB.Radius;
      if ((double) output.Distance > (double) radius1 + (double) radius2 && (double) output.Distance > 1.1920928955078125E-07)
      {
        output.Distance -= radius1 + radius2;
        Vector2 vector2 = output.PointB - output.PointA;
        vector2.Normalize();
        output.PointA += radius1 * vector2;
        output.PointB -= radius2 * vector2;
      }
      else
      {
        Vector2 vector2 = 0.5f * (output.PointA + output.PointB);
        output.PointA = vector2;
        output.PointB = vector2;
        output.Distance = 0.0f;
      }
    }
  }
}
