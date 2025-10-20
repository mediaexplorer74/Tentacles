// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Mathf
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public static class Mathf
  {
    public const float PI = 3.14159274f;
    public const float Infinity = float.PositiveInfinity;
    public const float NegativeInfinity = float.NegativeInfinity;
    public const float Rad2Deg = 57.2957764f;

    public static int Sign(float value) => Math.Sign(value);

    public static float Cos(float value) => (float) Math.Cos((double) value);

    public static float Sin(float value) => (float) Math.Sin((double) value);

    public static float Pow(float x, float y) => (float) Math.Pow((double) x, (double) y);

    public static float Sqrt(float x) => (float) Math.Sqrt((double) x);

    public static float Abs(float x) => Math.Abs(x);

    public static float Asin(float x) => (float) Math.Asin((double) x);

    public static float Min(float x, float y) => Math.Min(x, y);

    public static int Min(int x, int y) => Math.Min(x, y);

    public static float Max(float x, float y) => Math.Max(x, y);

    public static int Max(int x, int y) => Math.Max(x, y);

    public static float Atan2(float x, float y) => (float) Math.Atan2((double) x, (double) y);

    public static float Clamp(float value, float min, float max)
    {
      return MathHelper.Clamp(value, min, max);
    }

    public static int Clamp(int value, int min, int max)
    {
      return (int) MathHelper.Clamp((float) value, (float) min, (float) max);
    }

    public static float Clamp01(float value)
    {
      if ((double) value > 1.0)
        return 1f;
      return (double) value < 0.0 ? 0.0f : value;
    }

    public static int CeilToInt(float f) => (int) Math.Ceiling((double) f);

    public static int FloorToInt(float f) => (int) Math.Floor((double) f);

    public static float Floor(float f) => (float) Math.Floor((double) f);

    public static float Lerp(float from, float to, float t) => MathHelper.Lerp(from, to, t);

    public static float SmoothDamp(
      float current,
      float target,
      ref float currentVelocity,
      float smoothTime)
    {
      return Mathf.SmoothDamp(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
    }

    public static float SmoothDamp(
      float current,
      float target,
      ref float currentVelocity,
      float smoothTime,
      float maxSpeed,
      float deltaTime)
    {
      throw new NotImplementedException("SmoothDamp is not implemented!");
    }

    public static float SmoothDampAngle(
      float current,
      float target,
      ref float currentVelocity,
      float smoothTime)
    {
      return Mathf.SmoothDampAngle(current, target, ref currentVelocity, smoothTime, float.PositiveInfinity, Time.deltaTime);
    }

    public static float SmoothDampAngle(
      float current,
      float target,
      ref float currentVelocity,
      float smoothTime,
      float maxSpeed,
      float deltaTime)
    {
      throw new NotImplementedException("SmoothDamp is not implemented!");
    }
  }
}
