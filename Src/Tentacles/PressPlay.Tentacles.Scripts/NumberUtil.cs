// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.NumberUtil
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class NumberUtil
  {
    public static int IncrementNumberWithinRange(
      int number,
      int increment,
      int min,
      int max,
      bool loop)
    {
      number += increment;
      if (number > max)
        number = !loop ? max : min;
      else if (number < min)
        number = !loop ? min : max;
      return number;
    }

    public static float Increment(
      float number,
      float increment,
      float min,
      float max,
      NumberUtil.IncrementMode mode)
    {
      switch (mode)
      {
        case NumberUtil.IncrementMode.clamp:
          number += increment;
          number = Mathf.Clamp(number, min, max);
          break;
        case NumberUtil.IncrementMode.loop:
          number += increment;
          if ((double) number > (double) max)
          {
            number = min;
            break;
          }
          if ((double) number < (double) min)
          {
            number = max;
            break;
          }
          break;
      }
      return number;
    }

    public enum IncrementMode
    {
      clamp,
      loop,
      pingpong,
    }
  }
}
