// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LoadSceneScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LoadSceneScreen : GameScreen
  {
    private string sceneName;
    private bool hasStartedLoad;
    private bool loadingIsDone;
    private float loadDelayTimer;

    public LoadSceneScreen(string sceneName)
    {
      this.TransitionOnTime = new TimeSpan(2500000L);
      this.TransitionOffTime = new TimeSpan(2500000L);
      this.sceneName = sceneName;
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void HandleInput(InputState input)
    {
      base.HandleInput(input);
      PlayerIndex playerIndex;
      if ((input.IsNewButtonPress(Buttons.Back, this.ControllingPlayer, out playerIndex) || input.IsNewKeyPress(Keys.Escape, this.ControllingPlayer, out playerIndex)) && LevelHandler.isLoaded)
        LevelHandler.Instance.HandleBackButtonDown();
      PressPlay.FFWD.Input.Update(input);
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      if (!LevelHandler.isLoaded || LevelHandler.Instance.state != LevelHandler.LevelState.preloading && LevelHandler.Instance.state != LevelHandler.LevelState.idle)
        return;
      this.ScreenManager.FadeBackBufferToBlack(1f);
    }
  }
}
