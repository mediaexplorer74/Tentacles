// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.TimeOfImpact
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  public static class TimeOfImpact
  {
    public static int TOICalls;
    public static int TOIIters;
    public static int TOIMaxIters;
    public static int TOIRootIters;
    public static int TOIMaxRootIters;
    private static DistanceInput _distanceInput = new DistanceInput();

    public static void CalculateTimeOfImpact(out TOIOutput output, TOIInput input)
    {
      ++TimeOfImpact.TOICalls;
      output = new TOIOutput();
      output.State = TOIOutputState.Unknown;
      output.T = input.TMax;
      Sweep sweepA = input.SweepA;
      Sweep sweepB = input.SweepB;
      sweepA.Normalize();
      sweepB.Normalize();
      float tmax = input.TMax;
      float num1 = Math.Max(0.005f, input.ProxyA.Radius + input.ProxyB.Radius - 0.015f);
      float num2 = 0.0f;
      int val2_1 = 0;
      TimeOfImpact._distanceInput.ProxyA = input.ProxyA;
      TimeOfImpact._distanceInput.ProxyB = input.ProxyB;
      TimeOfImpact._distanceInput.UseRadii = false;
      do
      {
        Transform xf1;
        sweepA.GetTransform(out xf1, num2);
        Transform xf2;
        sweepB.GetTransform(out xf2, num2);
        TimeOfImpact._distanceInput.TransformA = xf1;
        TimeOfImpact._distanceInput.TransformB = xf2;
        DistanceOutput output1;
        SimplexCache cache;
        Distance.ComputeDistance(out output1, out cache, TimeOfImpact._distanceInput);
        if ((double) output1.Distance <= 0.0)
        {
          output.State = TOIOutputState.Overlapped;
          output.T = 0.0f;
          goto label_25;
        }
        else if ((double) output1.Distance < (double) num1 + 1.0 / 800.0)
        {
          output.State = TOIOutputState.Touching;
          output.T = num2;
          goto label_25;
        }
        else
        {
          SeparationFunction.Set(ref cache, input.ProxyA, ref sweepA, input.ProxyB, ref sweepB, num2);
          bool flag = false;
          float t1 = tmax;
          int num3 = 0;
          do
          {
            int indexA;
            int indexB;
            float num4 = SeparationFunction.FindMinSeparation(out indexA, out indexB, t1);
            if ((double) num4 > (double) num1 + 1.0 / 800.0)
            {
              output.State = TOIOutputState.Seperated;
              output.T = tmax;
              flag = true;
              break;
            }
            if ((double) num4 > (double) num1 - 1.0 / 800.0)
            {
              num2 = t1;
              break;
            }
            float num5 = SeparationFunction.Evaluate(indexA, indexB, num2);
            if ((double) num5 < (double) num1 - 1.0 / 800.0)
            {
              output.State = TOIOutputState.Failed;
              output.T = num2;
              flag = true;
              break;
            }
            if ((double) num5 <= (double) num1 + 1.0 / 800.0)
            {
              output.State = TOIOutputState.Touching;
              output.T = num2;
              flag = true;
              break;
            }
            int val2_2 = 0;
            float num6 = num2;
            float num7 = t1;
            do
            {
              float t2 = (val2_2 & 1) == 0 ? (float) (0.5 * ((double) num6 + (double) num7)) : num6 + (float) (((double) num1 - (double) num5) * ((double) num7 - (double) num6) / ((double) num4 - (double) num5));
              float num8 = SeparationFunction.Evaluate(indexA, indexB, t2);
              if ((double) Math.Abs(num8 - num1) < 1.0 / 800.0)
              {
                t1 = t2;
                break;
              }
              if ((double) num8 > (double) num1)
              {
                num6 = t2;
                num5 = num8;
              }
              else
              {
                num7 = t2;
                num4 = num8;
              }
              ++val2_2;
              ++TimeOfImpact.TOIRootIters;
            }
            while (val2_2 != 50);
            TimeOfImpact.TOIMaxRootIters = Math.Max(TimeOfImpact.TOIMaxRootIters, val2_2);
            ++num3;
          }
          while (num3 != Settings.MaxPolygonVertices);
          ++val2_1;
          ++TimeOfImpact.TOIIters;
          if (flag)
            goto label_25;
        }
      }
      while (val2_1 != 20);
      output.State = TOIOutputState.Failed;
      output.T = num2;
label_25:
      TimeOfImpact.TOIMaxIters = Math.Max(TimeOfImpact.TOIMaxIters, val2_1);
    }
  }
}
