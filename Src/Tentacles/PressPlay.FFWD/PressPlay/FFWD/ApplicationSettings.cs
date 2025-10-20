// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ApplicationSettings
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD
{
  public static class ApplicationSettings
  {
    public static ApplicationSettings.To2dMode to2dMode = ApplicationSettings.To2dMode.DropY;
    public static bool ShowComponentProfile = false;
    public static bool ShowiTweenUpdateTime = false;
    public static bool ShowTurnOffTime = false;
    public static bool ShowTimeBetweenUpdates = false;
    public static bool ShowRaycastTime = false;
    public static bool ShowParticleAnimTime = false;
    public static bool ShowPerformanceBreakdown = true;
    public static bool ShowFPSCounter = true;
    public static bool ShowBodyCounter = true;
    public static bool ShowDebugDisplays = true;
    public static bool ShowDebugLines = true;
    public static bool LogActivatedComponents = false;
    public static Camera DebugCamera;
    public static SpriteFont DebugFont;
    public static float pressPlayLogoSplashTime = 1f;
    public static float MGSLogoSplashTime = 5f;
    public static int AssetLoadInterval = 50;

    public enum To2dMode
    {
      DropX,
      DropY,
      DropZ,
    }
  }
}
