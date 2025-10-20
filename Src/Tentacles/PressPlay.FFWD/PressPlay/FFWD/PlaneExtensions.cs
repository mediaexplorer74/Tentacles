// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.PlaneExtensions
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace PressPlay.FFWD
{
  public static class PlaneExtensions
  {
    public static bool Raycast(this Plane plane, Ray ray, out float enter)
    {
      float? nullable = ray.Intersects(plane);
      if (nullable.HasValue)
      {
        enter = nullable.Value;
        return true;
      }
      enter = 0.0f;
      return false;
    }
  }
}
