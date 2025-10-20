// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.LoadingScreen
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class LoadingScreen : GameScreen
  {
    protected bool loadingIsSlow;
    protected bool otherScreensAreGone;
    protected bool hasAddedScreens;
    protected List<Control> controls = new List<Control>();
    protected Control rootControl;
    protected GameScreen[] screensToLoad;

    protected LoadingScreen(
      PressPlay.FFWD.ScreenManager.ScreenManager screenManager,
      bool loadingIsSlow,
      GameScreen[] screensToLoad)
    {
      this.loadingIsSlow = loadingIsSlow;
      this.screensToLoad = screensToLoad;
      this.IsSerializable = false;
      this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      this.EnabledGestures = GestureType.Tap;
    }

    public override void OnNotifyCallback()
    {
      base.OnNotifyCallback();
      Debug.Log((object) "Notifying LoadingScreen");
      this.ExitScreen();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.ScreenState != ScreenState.Active || this.ScreenManager.GetScreens().Length != 1)
        return;
      this.otherScreensAreGone = true;
    }
  }
}
