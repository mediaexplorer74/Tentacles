// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Equations
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Equations
  {
    public static float EaseNone(float t, float b, float c, float d) => c * t / d + b;

    public static float EaseQuadIn(float t, float b, float c, float d) => c * (t /= d) * t + b;

    public static float EaseQuadOut(float t, float b, float c, float d)
    {
      return (float) (-(double) c * (double) (t /= d) * ((double) t - 2.0)) + b;
    }

    public static float EaseQuadInOut(float t, float b, float c, float d)
    {
      return (double) (t /= d / 2f) < 1.0 ? c / 2f * t * t + b : (float) (-(double) c / 2.0 * ((double) --t * ((double) t - 2.0) - 1.0)) + b;
    }

    public static float EaseQuadOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseQuadOut(t * 2f, b, c / 2f, d) : Equations.EaseQuadIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseCubicIn(float t, float b, float c, float d) => c * (t /= d) * t * t + b;

    public static float EaseCubicOut(float t, float b, float c, float d)
    {
      return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t + 1.0) + b;
    }

    public static float EaseCubicInOut(float t, float b, float c, float d)
    {
      return (double) (t /= d / 2f) < 1.0 ? c / 2f * t * t * t + b : (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t + 2.0)) + b;
    }

    public static float EaseCubicOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseCubicOut(t * 2f, b, c / 2f, d) : Equations.EaseCubicIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseQuartIn(float t, float b, float c, float d)
    {
      return c * (t /= d) * t * t * t + b;
    }

    public static float EaseQuartOut(float t, float b, float c, float d)
    {
      return (float) (-(double) c * ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t * (double) t - 1.0)) + b;
    }

    public static float EaseQuartInOut(float t, float b, float c, float d)
    {
      return (double) (t /= d / 2f) < 1.0 ? c / 2f * t * t * t * t + b : (float) (-(double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t * (double) t - 2.0)) + b;
    }

    public static float EaseQuartOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseQuartOut(t * 2f, b, c / 2f, d) : Equations.EaseQuartIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseQuintIn(float t, float b, float c, float d)
    {
      return c * (t /= d) * t * t * t * t + b;
    }

    public static float EaseQuintOut(float t, float b, float c, float d)
    {
      return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (double) t * (double) t * (double) t + 1.0) + b;
    }

    public static float EaseQuintInOut(float t, float b, float c, float d)
    {
      return (double) (t /= d / 2f) < 1.0 ? c / 2f * t * t * t * t * t + b : (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (double) t * (double) t * (double) t + 2.0)) + b;
    }

    public static float EaseQuintOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseQuintOut(t * 2f, b, c / 2f, d) : Equations.EaseQuintIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseSineIn(float t, float b, float c, float d)
    {
      return -c * Mathf.Cos((float) ((double) t / (double) d * 1.5707963705062866)) + c + b;
    }

    public static float EaseSineOut(float t, float b, float c, float d)
    {
      return c * Mathf.Sin((float) ((double) t / (double) d * 1.5707963705062866)) + b;
    }

    public static float EaseSineInOut(float t, float b, float c, float d)
    {
      return (float) (-(double) c / 2.0 * ((double) Mathf.Cos(3.14159274f * t / d) - 1.0)) + b;
    }

    public static float EaseSineOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseSineOut(t * 2f, b, c / 2f, d) : Equations.EaseSineIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseExpoIn(float t, float b, float c, float d)
    {
      return (double) t != 0.0 ? (float) ((double) c * (double) Mathf.Pow(2f, (float) (10.0 * ((double) t / (double) d - 1.0))) + (double) b - (double) c * (1.0 / 1000.0)) : b;
    }

    public static float EaseExpoOut(float t, float b, float c, float d)
    {
      return (double) t != (double) d ? (float) ((double) c * 1.0010000467300415 * (-(double) Mathf.Pow(2f, -10f * t / d) + 1.0)) + b : b + c;
    }

    public static float EaseExpoInOut(float t, float b, float c, float d)
    {
      if ((double) t == 0.0)
        return b;
      if ((double) t == (double) d)
        return b + c;
      return (double) (t /= d / 2f) < 1.0 ? (float) ((double) c / 2.0 * (double) Mathf.Pow(2f, (float) (10.0 * ((double) t - 1.0))) + (double) b - (double) c * 0.00050000002374872565) : (float) ((double) c / 2.0 * 1.000499963760376 * (-(double) Mathf.Pow(2f, -10f * --t) + 2.0)) + b;
    }

    public static float EaseExpoOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseExpoOut(t * 2f, b, c / 2f, d) : Equations.EaseExpoIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseCircIn(float t, float b, float c, float d)
    {
      return (float) (-(double) c * ((double) Mathf.Sqrt((float) (1.0 - (double) (t /= d) * (double) t)) - 1.0)) + b;
    }

    public static float EaseCircOut(float t, float b, float c, float d)
    {
      return c * Mathf.Sqrt((float) (1.0 - (double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t)) + b;
    }

    public static float EaseCircInOut(float t, float b, float c, float d)
    {
      return (double) (t /= d / 2f) < 1.0 ? (float) (-(double) c / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) t * (double) t)) - 1.0)) + b : (float) ((double) c / 2.0 * ((double) Mathf.Sqrt((float) (1.0 - (double) (t -= 2f) * (double) t)) + 1.0)) + b;
    }

    public static float EaseCircOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseCircOut(t * 2f, b, c / 2f, d) : Equations.EaseCircIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseElasticIn(float t, float b, float c, float d)
    {
      if ((double) t == 0.0)
        return b;
      if ((double) (t /= d) == 1.0)
        return b + c;
      float num1 = d * 0.3f;
      float num2 = 0.0f;
      float num3;
      if ((double) num2 == 0.0 || (double) num2 < (double) Mathf.Abs(c))
      {
        num2 = c;
        num3 = num1 / 4f;
      }
      else
        num3 = num1 / 6.28318548f * Mathf.Asin(c / num2);
      return (float) -((double) num2 * (double) Mathf.Pow(2f, 10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num3) * 6.2831854820251465) / num1)) + b;
    }

    public static float EaseElasticOut(float t, float b, float c, float d)
    {
      if ((double) t == 0.0)
        return b;
      if ((double) (t /= d) == 1.0)
        return b + c;
      float num1 = d * 0.3f;
      float num2 = 0.0f;
      float num3;
      if ((double) num2 == 0.0 || (double) num2 < (double) Mathf.Abs(c))
      {
        num2 = c;
        num3 = num1 / 4f;
      }
      else
        num3 = num1 / 6.28318548f * Mathf.Asin(c / num2);
      return num2 * Mathf.Pow(2f, -10f * t) * Mathf.Sin((float) (((double) t * (double) d - (double) num3) * 6.2831854820251465) / num1) + c + b;
    }

    public static float EaseElasticInOut(float t, float b, float c, float d)
    {
      if ((double) t == 0.0)
        return b;
      if ((double) (t /= d / 2f) == 2.0)
        return b + c;
      float num1 = d * 0.450000018f;
      float num2 = 0.0f;
      float num3;
      if ((double) num2 == 0.0 || (double) num2 < (double) Mathf.Abs(c))
      {
        num2 = c;
        num3 = num1 / 4f;
      }
      else
        num3 = num1 / 6.28318548f * Mathf.Asin(c / num2);
      return (double) t < 1.0 ? (float) (-0.5 * ((double) num2 * (double) Mathf.Pow(2f, 10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num3) * 6.2831854820251465) / num1))) + b : (float) ((double) num2 * (double) Mathf.Pow(2f, -10f * --t) * (double) Mathf.Sin((float) (((double) t * (double) d - (double) num3) * 6.2831854820251465) / num1) * 0.5) + c + b;
    }

    public static float EaseElasticOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseElasticOut(t * 2f, b, c / 2f, d) : Equations.EaseElasticIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseBackIn(float t, float b, float c, float d)
    {
      float num = 1.70158f;
      return (float) ((double) c * (double) (t /= d) * (double) t * (((double) num + 1.0) * (double) t - (double) num)) + b;
    }

    public static float EaseBackOut(float t, float b, float c, float d)
    {
      float num = 1.70158f;
      return c * (float) ((double) (t = (float) ((double) t / (double) d - 1.0)) * (double) t * (((double) num + 1.0) * (double) t + (double) num) + 1.0) + b;
    }

    public static float EaseBackInOut(float t, float b, float c, float d)
    {
      float num1 = 1.70158f;
      float num2;
      float num3;
      return (double) (t /= d / 2f) < 1.0 ? (float) ((double) c / 2.0 * ((double) t * (double) t * (((double) (num2 = num1 * 1.525f) + 1.0) * (double) t - (double) num2))) + b : (float) ((double) c / 2.0 * ((double) (t -= 2f) * (double) t * (((double) (num3 = num1 * 1.525f) + 1.0) * (double) t + (double) num3) + 2.0)) + b;
    }

    public static float EaseBackOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseBackOut(t * 2f, b, c / 2f, d) : Equations.EaseBackIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static float EaseBounceIn(float t, float b, float c, float d)
    {
      return c - Equations.EaseBounceOut(d - t, 0.0f, c, d) + b;
    }

    public static float EaseBounceOut(float t, float b, float c, float d)
    {
      if ((double) (t /= d) < 0.36363637447357178)
        return c * (121f / 16f * t * t) + b;
      if ((double) t < 0.72727274894714355)
        return c * (float) (121.0 / 16.0 * (double) (t -= 0.545454562f) * (double) t + 0.75) + b;
      return (double) t < 0.90909093618392944 ? c * (float) (121.0 / 16.0 * (double) (t -= 0.8181818f) * (double) t + 15.0 / 16.0) + b : c * (float) (121.0 / 16.0 * (double) (t -= 0.954545438f) * (double) t + 63.0 / 64.0) + b;
    }

    public static float EaseBounceInOut(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseBounceIn(t * 2f, 0.0f, c, d) * 0.5f + b : (float) ((double) Equations.EaseBounceOut(t * 2f - d, 0.0f, c, d) * 0.5 + (double) c * 0.5) + b;
    }

    public static float EaseBounceOutIn(float t, float b, float c, float d)
    {
      return (double) t < (double) d / 2.0 ? Equations.EaseBounceOut(t * 2f, b, c / 2f, d) : Equations.EaseBounceIn(t * 2f - d, b + c / 2f, c / 2f, d);
    }

    public static Vector3 ChangeVector(float t, Vector3 b, Vector3 c, float d, Ease Ease)
    {
      float x = 0.0f;
      float y = 0.0f;
      float z = 0.0f;
      switch (Ease)
      {
        case Ease.Linear:
          x = Equations.EaseNone(t, b.x, c.x, d);
          y = Equations.EaseNone(t, b.y, c.y, d);
          z = Equations.EaseNone(t, b.z, c.z, d);
          break;
        case Ease.EaseQuadIn:
          x = Equations.EaseQuadIn(t, b.x, c.x, d);
          y = Equations.EaseQuadIn(t, b.y, c.y, d);
          z = Equations.EaseQuadIn(t, b.z, c.z, d);
          break;
        case Ease.EaseQuadOut:
          x = Equations.EaseQuadOut(t, b.x, c.x, d);
          y = Equations.EaseQuadOut(t, b.y, c.y, d);
          z = Equations.EaseQuadOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuadInOut:
          x = Equations.EaseQuadInOut(t, b.x, c.x, d);
          y = Equations.EaseQuadInOut(t, b.y, c.y, d);
          z = Equations.EaseQuadInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuadOutIn:
          x = Equations.EaseQuadOutIn(t, b.x, c.x, d);
          y = Equations.EaseQuadOutIn(t, b.y, c.y, d);
          z = Equations.EaseQuadOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseCubicIn:
          x = Equations.EaseCubicIn(t, b.x, c.x, d);
          y = Equations.EaseCubicIn(t, b.y, c.y, d);
          z = Equations.EaseCubicIn(t, b.z, c.z, d);
          break;
        case Ease.EaseCubicOut:
          x = Equations.EaseCubicOut(t, b.x, c.x, d);
          y = Equations.EaseCubicOut(t, b.y, c.y, d);
          z = Equations.EaseCubicOut(t, b.z, c.z, d);
          break;
        case Ease.EaseCubicInOut:
          x = Equations.EaseCubicInOut(t, b.x, c.x, d);
          y = Equations.EaseCubicInOut(t, b.y, c.y, d);
          z = Equations.EaseCubicInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseCubicOutIn:
          x = Equations.EaseCubicOutIn(t, b.x, c.x, d);
          y = Equations.EaseCubicOutIn(t, b.y, c.y, d);
          z = Equations.EaseCubicOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseQuartIn:
          x = Equations.EaseQuartIn(t, b.x, c.x, d);
          y = Equations.EaseQuartIn(t, b.y, c.y, d);
          z = Equations.EaseQuartIn(t, b.z, c.z, d);
          break;
        case Ease.EaseQuartOut:
          x = Equations.EaseQuartOut(t, b.x, c.x, d);
          y = Equations.EaseQuartOut(t, b.y, c.y, d);
          z = Equations.EaseQuartOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuartInOut:
          x = Equations.EaseQuartInOut(t, b.x, c.x, d);
          y = Equations.EaseQuartInOut(t, b.y, c.y, d);
          z = Equations.EaseQuartInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuartOutIn:
          x = Equations.EaseQuartOutIn(t, b.x, c.x, d);
          y = Equations.EaseQuartOutIn(t, b.y, c.y, d);
          z = Equations.EaseQuartOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseQuintIn:
          x = Equations.EaseQuintIn(t, b.x, c.x, d);
          y = Equations.EaseQuintIn(t, b.y, c.y, d);
          z = Equations.EaseQuintIn(t, b.z, c.z, d);
          break;
        case Ease.EaseQuintOut:
          x = Equations.EaseQuintOut(t, b.x, c.x, d);
          y = Equations.EaseQuintOut(t, b.y, c.y, d);
          z = Equations.EaseQuintOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuintInOut:
          x = Equations.EaseQuintInOut(t, b.x, c.x, d);
          y = Equations.EaseQuintInOut(t, b.y, c.y, d);
          z = Equations.EaseQuintInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseQuintOutIn:
          x = Equations.EaseQuintOutIn(t, b.x, c.x, d);
          y = Equations.EaseQuintOutIn(t, b.y, c.y, d);
          z = Equations.EaseQuintOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseSineIn:
          x = Equations.EaseSineIn(t, b.x, c.x, d);
          y = Equations.EaseSineIn(t, b.y, c.y, d);
          z = Equations.EaseSineIn(t, b.z, c.z, d);
          break;
        case Ease.EaseSineOut:
          x = Equations.EaseSineOut(t, b.x, c.x, d);
          y = Equations.EaseSineOut(t, b.y, c.y, d);
          z = Equations.EaseSineOut(t, b.z, c.z, d);
          break;
        case Ease.EaseSineInOut:
          x = Equations.EaseSineInOut(t, b.x, c.x, d);
          y = Equations.EaseSineInOut(t, b.y, c.y, d);
          z = Equations.EaseSineInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseSineOutIn:
          x = Equations.EaseSineOutIn(t, b.x, c.x, d);
          y = Equations.EaseSineOutIn(t, b.y, c.y, d);
          z = Equations.EaseSineOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseExpoIn:
          x = Equations.EaseExpoIn(t, b.x, c.x, d);
          y = Equations.EaseExpoIn(t, b.y, c.y, d);
          z = Equations.EaseExpoIn(t, b.z, c.z, d);
          break;
        case Ease.EaseExpoOut:
          x = Equations.EaseExpoOut(t, b.x, c.x, d);
          y = Equations.EaseExpoOut(t, b.y, c.y, d);
          z = Equations.EaseExpoOut(t, b.z, c.z, d);
          break;
        case Ease.EaseExpoInOut:
          x = Equations.EaseExpoInOut(t, b.x, c.x, d);
          y = Equations.EaseExpoInOut(t, b.y, c.y, d);
          z = Equations.EaseExpoInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseExpoOutIn:
          x = Equations.EaseExpoOutIn(t, b.x, c.x, d);
          y = Equations.EaseExpoOutIn(t, b.y, c.y, d);
          z = Equations.EaseExpoOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseCircIn:
          x = Equations.EaseCircIn(t, b.x, c.x, d);
          y = Equations.EaseCircIn(t, b.y, c.y, d);
          z = Equations.EaseCircIn(t, b.z, c.z, d);
          break;
        case Ease.EaseCircOut:
          x = Equations.EaseCircOut(t, b.x, c.x, d);
          y = Equations.EaseCircOut(t, b.y, c.y, d);
          z = Equations.EaseCircOut(t, b.z, c.z, d);
          break;
        case Ease.EaseCircInOut:
          x = Equations.EaseCircInOut(t, b.x, c.x, d);
          y = Equations.EaseCircInOut(t, b.y, c.y, d);
          z = Equations.EaseCircInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseCircOutIn:
          x = Equations.EaseCircOutIn(t, b.x, c.x, d);
          y = Equations.EaseCircOutIn(t, b.y, c.y, d);
          z = Equations.EaseCircOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseElasticIn:
          x = Equations.EaseElasticIn(t, b.x, c.x, d);
          y = Equations.EaseElasticIn(t, b.y, c.y, d);
          z = Equations.EaseElasticIn(t, b.z, c.z, d);
          break;
        case Ease.EaseElasticOut:
          x = Equations.EaseElasticOut(t, b.x, c.x, d);
          y = Equations.EaseElasticOut(t, b.y, c.y, d);
          z = Equations.EaseElasticOut(t, b.z, c.z, d);
          break;
        case Ease.EaseElasticInOut:
          x = Equations.EaseElasticInOut(t, b.x, c.x, d);
          y = Equations.EaseElasticInOut(t, b.y, c.y, d);
          z = Equations.EaseElasticInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseElasticOutIn:
          x = Equations.EaseElasticOutIn(t, b.x, c.x, d);
          y = Equations.EaseElasticOutIn(t, b.y, c.y, d);
          z = Equations.EaseElasticOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseBackIn:
          x = Equations.EaseBackIn(t, b.x, c.x, d);
          y = Equations.EaseBackIn(t, b.y, c.y, d);
          z = Equations.EaseBackIn(t, b.z, c.z, d);
          break;
        case Ease.EaseBackOut:
          x = Equations.EaseBackOut(t, b.x, c.x, d);
          y = Equations.EaseBackOut(t, b.y, c.y, d);
          z = Equations.EaseBackOut(t, b.z, c.z, d);
          break;
        case Ease.EaseBackInOut:
          x = Equations.EaseBackInOut(t, b.x, c.x, d);
          y = Equations.EaseBackInOut(t, b.y, c.y, d);
          z = Equations.EaseBackInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseBackOutIn:
          x = Equations.EaseBackOutIn(t, b.x, c.x, d);
          y = Equations.EaseBackOutIn(t, b.y, c.y, d);
          z = Equations.EaseBackOutIn(t, b.z, c.z, d);
          break;
        case Ease.EaseBounceIn:
          x = Equations.EaseBounceIn(t, b.x, c.x, d);
          y = Equations.EaseBounceIn(t, b.y, c.y, d);
          z = Equations.EaseBounceIn(t, b.z, c.z, d);
          break;
        case Ease.EaseBounceOut:
          x = Equations.EaseBounceOut(t, b.x, c.x, d);
          y = Equations.EaseBounceOut(t, b.y, c.y, d);
          z = Equations.EaseBounceOut(t, b.z, c.z, d);
          break;
        case Ease.EaseBounceInOut:
          x = Equations.EaseBounceInOut(t, b.x, c.x, d);
          y = Equations.EaseBounceInOut(t, b.y, c.y, d);
          z = Equations.EaseBounceInOut(t, b.z, c.z, d);
          break;
        case Ease.EaseBounceOutIn:
          x = Equations.EaseBounceOutIn(t, b.x, c.x, d);
          y = Equations.EaseBounceOutIn(t, b.y, c.y, d);
          z = Equations.EaseBounceOutIn(t, b.z, c.z, d);
          break;
      }
      return new Vector3(x, y, z);
    }

    public static float ChangeFloat(float t, float b, float c, float d, Ease Ease)
    {
      float num = 0.0f;
      switch (Ease)
      {
        case Ease.Linear:
          num = Equations.EaseNone(t, b, c, d);
          break;
        case Ease.EaseQuadIn:
          num = Equations.EaseQuadIn(t, b, c, d);
          break;
        case Ease.EaseQuadOut:
          num = Equations.EaseQuadOut(t, b, c, d);
          break;
        case Ease.EaseQuadInOut:
          num = Equations.EaseQuadInOut(t, b, c, d);
          break;
        case Ease.EaseQuadOutIn:
          num = Equations.EaseQuadOutIn(t, b, c, d);
          break;
        case Ease.EaseCubicIn:
          num = Equations.EaseCubicIn(t, b, c, d);
          break;
        case Ease.EaseCubicOut:
          num = Equations.EaseCubicOut(t, b, c, d);
          break;
        case Ease.EaseCubicInOut:
          num = Equations.EaseCubicInOut(t, b, c, d);
          break;
        case Ease.EaseCubicOutIn:
          num = Equations.EaseCubicOutIn(t, b, c, d);
          break;
        case Ease.EaseQuartIn:
          num = Equations.EaseQuartIn(t, b, c, d);
          break;
        case Ease.EaseQuartOut:
          num = Equations.EaseQuartOut(t, b, c, d);
          break;
        case Ease.EaseQuartInOut:
          num = Equations.EaseQuartInOut(t, b, c, d);
          break;
        case Ease.EaseQuartOutIn:
          num = Equations.EaseQuartOutIn(t, b, c, d);
          break;
        case Ease.EaseQuintIn:
          num = Equations.EaseQuintIn(t, b, c, d);
          break;
        case Ease.EaseQuintOut:
          num = Equations.EaseQuintOut(t, b, c, d);
          break;
        case Ease.EaseQuintInOut:
          num = Equations.EaseQuintInOut(t, b, c, d);
          break;
        case Ease.EaseQuintOutIn:
          num = Equations.EaseQuintOutIn(t, b, c, d);
          break;
        case Ease.EaseSineIn:
          num = Equations.EaseSineIn(t, b, c, d);
          break;
        case Ease.EaseSineOut:
          num = Equations.EaseSineOut(t, b, c, d);
          break;
        case Ease.EaseSineInOut:
          num = Equations.EaseSineInOut(t, b, c, d);
          break;
        case Ease.EaseSineOutIn:
          num = Equations.EaseSineOutIn(t, b, c, d);
          break;
        case Ease.EaseExpoIn:
          num = Equations.EaseExpoIn(t, b, c, d);
          break;
        case Ease.EaseExpoOut:
          num = Equations.EaseExpoOut(t, b, c, d);
          break;
        case Ease.EaseExpoInOut:
          num = Equations.EaseExpoInOut(t, b, c, d);
          break;
        case Ease.EaseExpoOutIn:
          num = Equations.EaseExpoOutIn(t, b, c, d);
          break;
        case Ease.EaseCircIn:
          num = Equations.EaseCircIn(t, b, c, d);
          break;
        case Ease.EaseCircOut:
          num = Equations.EaseCircOut(t, b, c, d);
          break;
        case Ease.EaseCircInOut:
          num = Equations.EaseCircInOut(t, b, c, d);
          break;
        case Ease.EaseCircOutIn:
          num = Equations.EaseCircOutIn(t, b, c, d);
          break;
        case Ease.EaseElasticIn:
          num = Equations.EaseElasticIn(t, b, c, d);
          break;
        case Ease.EaseElasticOut:
          num = Equations.EaseElasticOut(t, b, c, d);
          break;
        case Ease.EaseElasticInOut:
          num = Equations.EaseElasticInOut(t, b, c, d);
          break;
        case Ease.EaseElasticOutIn:
          num = Equations.EaseElasticOutIn(t, b, c, d);
          break;
        case Ease.EaseBackIn:
          num = Equations.EaseBackIn(t, b, c, d);
          break;
        case Ease.EaseBackOut:
          num = Equations.EaseBackOut(t, b, c, d);
          break;
        case Ease.EaseBackInOut:
          num = Equations.EaseBackInOut(t, b, c, d);
          break;
        case Ease.EaseBackOutIn:
          num = Equations.EaseBackOutIn(t, b, c, d);
          break;
        case Ease.EaseBounceIn:
          num = Equations.EaseBounceIn(t, b, c, d);
          break;
        case Ease.EaseBounceOut:
          num = Equations.EaseBounceOut(t, b, c, d);
          break;
        case Ease.EaseBounceInOut:
          num = Equations.EaseBounceInOut(t, b, c, d);
          break;
        case Ease.EaseBounceOutIn:
          num = Equations.EaseBounceOutIn(t, b, c, d);
          break;
      }
      return num;
    }
  }
}
