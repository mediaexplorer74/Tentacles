// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.SeparationFunction
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision
{
  public static class SeparationFunction
  {
    private static Vector2 _axis;
    private static Vector2 _localPoint;
    private static DistanceProxy _proxyA = new DistanceProxy();
    private static DistanceProxy _proxyB = new DistanceProxy();
    private static Sweep _sweepA;
    private static Sweep _sweepB;
    private static SeparationFunctionType _type;

    public static void Set(
      ref SimplexCache cache,
      DistanceProxy proxyA,
      ref Sweep sweepA,
      DistanceProxy proxyB,
      ref Sweep sweepB,
      float t1)
    {
      SeparationFunction._localPoint = Vector2.Zero;
      SeparationFunction._proxyA = proxyA;
      SeparationFunction._proxyB = proxyB;
      int count = (int) cache.Count;
      SeparationFunction._sweepA = sweepA;
      SeparationFunction._sweepB = sweepB;
      Transform xf1;
      SeparationFunction._sweepA.GetTransform(out xf1, t1);
      Transform xf2;
      SeparationFunction._sweepB.GetTransform(out xf2, t1);
      if (count == 1)
      {
        SeparationFunction._type = SeparationFunctionType.Points;
        Vector2 vertex1 = SeparationFunction._proxyA.Vertices[(int) cache.IndexA[0]];
        Vector2 vertex2 = SeparationFunction._proxyB.Vertices[(int) cache.IndexB[0]];
        Vector2 vector2 = MathUtils.Multiply(ref xf1, vertex1);
        SeparationFunction._axis = MathUtils.Multiply(ref xf2, vertex2) - vector2;
        SeparationFunction._axis.Normalize();
      }
      else if ((int) cache.IndexA[0] == (int) cache.IndexA[1])
      {
        SeparationFunction._type = SeparationFunctionType.FaceB;
        Vector2 vertex3 = proxyB.Vertices[(int) cache.IndexB[0]];
        Vector2 vertex4 = proxyB.Vertices[(int) cache.IndexB[1]];
        Vector2 vector2_1 = vertex4 - vertex3;
        SeparationFunction._axis = new Vector2(vector2_1.Y, -vector2_1.X);
        SeparationFunction._axis.Normalize();
        Vector2 vector2_2 = MathUtils.Multiply(ref xf2.R, SeparationFunction._axis);
        SeparationFunction._localPoint = 0.5f * (vertex3 + vertex4);
        Vector2 vector2_3 = MathUtils.Multiply(ref xf2, SeparationFunction._localPoint);
        Vector2 vertex5 = proxyA.Vertices[(int) cache.IndexA[0]];
        float num1 = Vector2.Dot(MathUtils.Multiply(ref xf1, vertex5) - vector2_3, vector2_2);
        if ((double) num1 >= 0.0)
          return;
        SeparationFunction._axis = -SeparationFunction._axis;
        float num2 = -num1;
      }
      else
      {
        SeparationFunction._type = SeparationFunctionType.FaceA;
        Vector2 vertex6 = SeparationFunction._proxyA.Vertices[(int) cache.IndexA[0]];
        Vector2 vertex7 = SeparationFunction._proxyA.Vertices[(int) cache.IndexA[1]];
        Vector2 vector2_4 = vertex7 - vertex6;
        SeparationFunction._axis = new Vector2(vector2_4.Y, -vector2_4.X);
        SeparationFunction._axis.Normalize();
        Vector2 vector2_5 = MathUtils.Multiply(ref xf1.R, SeparationFunction._axis);
        SeparationFunction._localPoint = 0.5f * (vertex6 + vertex7);
        Vector2 vector2_6 = MathUtils.Multiply(ref xf1, SeparationFunction._localPoint);
        Vector2 vertex8 = SeparationFunction._proxyB.Vertices[(int) cache.IndexB[0]];
        float num3 = Vector2.Dot(MathUtils.Multiply(ref xf2, vertex8) - vector2_6, vector2_5);
        if ((double) num3 >= 0.0)
          return;
        SeparationFunction._axis = -SeparationFunction._axis;
        float num4 = -num3;
      }
    }

    public static float FindMinSeparation(out int indexA, out int indexB, float t)
    {
      Transform xf1;
      SeparationFunction._sweepA.GetTransform(out xf1, t);
      Transform xf2;
      SeparationFunction._sweepB.GetTransform(out xf2, t);
      switch (SeparationFunction._type)
      {
        case SeparationFunctionType.Points:
          Vector2 direction1 = MathUtils.MultiplyT(ref xf1.R, SeparationFunction._axis);
          Vector2 direction2 = MathUtils.MultiplyT(ref xf2.R, -SeparationFunction._axis);
          indexA = SeparationFunction._proxyA.GetSupport(direction1);
          indexB = SeparationFunction._proxyB.GetSupport(direction2);
          Vector2 vertex1 = SeparationFunction._proxyA.Vertices[indexA];
          Vector2 vertex2 = SeparationFunction._proxyB.Vertices[indexB];
          Vector2 vector2_1 = MathUtils.Multiply(ref xf1, vertex1);
          return Vector2.Dot(MathUtils.Multiply(ref xf2, vertex2) - vector2_1, SeparationFunction._axis);
        case SeparationFunctionType.FaceA:
          Vector2 vector2_2 = MathUtils.Multiply(ref xf1.R, SeparationFunction._axis);
          Vector2 vector2_3 = MathUtils.Multiply(ref xf1, SeparationFunction._localPoint);
          Vector2 direction3 = MathUtils.MultiplyT(ref xf2.R, -vector2_2);
          indexA = -1;
          indexB = SeparationFunction._proxyB.GetSupport(direction3);
          Vector2 vertex3 = SeparationFunction._proxyB.Vertices[indexB];
          return Vector2.Dot(MathUtils.Multiply(ref xf2, vertex3) - vector2_3, vector2_2);
        case SeparationFunctionType.FaceB:
          Vector2 vector2_4 = MathUtils.Multiply(ref xf2.R, SeparationFunction._axis);
          Vector2 vector2_5 = MathUtils.Multiply(ref xf2, SeparationFunction._localPoint);
          Vector2 direction4 = MathUtils.MultiplyT(ref xf1.R, -vector2_4);
          indexB = -1;
          indexA = SeparationFunction._proxyA.GetSupport(direction4);
          Vector2 vertex4 = SeparationFunction._proxyA.Vertices[indexA];
          return Vector2.Dot(MathUtils.Multiply(ref xf1, vertex4) - vector2_5, vector2_4);
        default:
          indexA = -1;
          indexB = -1;
          return 0.0f;
      }
    }

    public static float Evaluate(int indexA, int indexB, float t)
    {
      Transform xf1;
      SeparationFunction._sweepA.GetTransform(out xf1, t);
      Transform xf2;
      SeparationFunction._sweepB.GetTransform(out xf2, t);
      switch (SeparationFunction._type)
      {
        case SeparationFunctionType.Points:
          MathUtils.MultiplyT(ref xf1.R, SeparationFunction._axis);
          MathUtils.MultiplyT(ref xf2.R, -SeparationFunction._axis);
          Vector2 vertex1 = SeparationFunction._proxyA.Vertices[indexA];
          Vector2 vertex2 = SeparationFunction._proxyB.Vertices[indexB];
          Vector2 vector2_1 = MathUtils.Multiply(ref xf1, vertex1);
          return Vector2.Dot(MathUtils.Multiply(ref xf2, vertex2) - vector2_1, SeparationFunction._axis);
        case SeparationFunctionType.FaceA:
          Vector2 vector2_2 = MathUtils.Multiply(ref xf1.R, SeparationFunction._axis);
          Vector2 vector2_3 = MathUtils.Multiply(ref xf1, SeparationFunction._localPoint);
          MathUtils.MultiplyT(ref xf2.R, -vector2_2);
          Vector2 vertex3 = SeparationFunction._proxyB.Vertices[indexB];
          return Vector2.Dot(MathUtils.Multiply(ref xf2, vertex3) - vector2_3, vector2_2);
        case SeparationFunctionType.FaceB:
          Vector2 vector2_4 = MathUtils.Multiply(ref xf2.R, SeparationFunction._axis);
          Vector2 vector2_5 = MathUtils.Multiply(ref xf2, SeparationFunction._localPoint);
          MathUtils.MultiplyT(ref xf1.R, -vector2_4);
          Vector2 vertex4 = SeparationFunction._proxyA.Vertices[indexA];
          return Vector2.Dot(MathUtils.Multiply(ref xf1, vertex4) - vector2_5, vector2_4);
        default:
          return 0.0f;
      }
    }
  }
}
