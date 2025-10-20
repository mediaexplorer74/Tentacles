// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LoadingScreen2
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LoadingScreen2 : LoadingScreen
  {
    public LoadingScreen2.DoOnTransitionOffComplete doOnTransitionOffComplete;
    private LoadingScreen2.LoadingState _state;
    private bool hasStartedLoadingScreen;
    private bool hasAddedTravelScreen;
    private bool shouldIAddTravelScreen;
    private int count;
    private Level levelToLoad;
    private string sceneToLoad;

    public LoadingScreen2.LoadingState state
    {
      get => this._state;
      set => this._state = value;
    }

    public LoadingScreen2(
      PressPlay.FFWD.ScreenManager.ScreenManager screenManager,
      bool loadingIsSlow,
      GameScreen[] screensToLoad)
      : base(screenManager, loadingIsSlow, screensToLoad)
    {
      Debug.Log((object) "Adding LoadingScreen2!!!");
      GlobalManager.Instance.fullscreenImageHandler.DoInstantClearScreen();
      this.state = LoadingScreen2.LoadingState.loadMainMenu;
    }

    public LoadingScreen2(string sceneToLoad)
      : base(Application.screenManager, true, (GameScreen[]) null)
    {
      GlobalManager.Instance.fullscreenImageHandler.DoInstantClearScreen();
      this.levelToLoad = GlobalManager.Instance.database.GetLevelFromSceneName(sceneToLoad);
      this.sceneToLoad = sceneToLoad;
      Application.LoadLevel("Scenes/" + sceneToLoad);
      this.state = LoadingScreen2.LoadingState.loadingXml;
      this.shouldIAddTravelScreen = true;
      foreach (GameScreen screen in Application.screenManager.GetScreens())
        screen.ExitScreen();
      this.TransitionOnTime = new TimeSpan(2500000L);
      this.TransitionOffTime = new TimeSpan(2500000L);
    }

    public static void Load(
      PressPlay.FFWD.ScreenManager.ScreenManager screenManager,
      bool loadingIsSlow,
      PlayerIndex? controllingPlayer,
      params GameScreen[] screensToLoad)
    {
      foreach (GameScreen screen in screenManager.GetScreens())
        screen.ExitScreen();
      LoadingScreen screen1 = (LoadingScreen) new LoadingScreen2(screenManager, loadingIsSlow, screensToLoad);
      screenManager.AddScreen((GameScreen) screen1, controllingPlayer);
    }

    public override void LoadContent()
    {
    }

    public override void UnloadContent() => base.UnloadContent();

    public override void OnNotifyCallback() => base.OnNotifyCallback();

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      ++this.count;
      if (this.shouldIAddTravelScreen && !this.hasAddedTravelScreen && this.count > 5)
      {
        Application.screenManager.AddScreen((GameScreen) new LemmyTravelScreen(this.levelToLoad, this));
        this.hasAddedTravelScreen = true;
      }
      switch (this.state)
      {
        case LoadingScreen2.LoadingState.loadingXml:
          if ((double) Application.loadingProgress != 1.0)
            break;
          this.state = LoadingScreen2.LoadingState.complete;
          break;
        case LoadingScreen2.LoadingState.loadMainMenu:
          if (!this.otherScreensAreGone || this.hasAddedScreens || this.screensToLoad == null)
            break;
          GlobalManager.Instance.fullscreenImageHandler.DoInstantBlackScreen();
          foreach (GameScreen screen in this.screensToLoad)
          {
            if (screen != null)
              this.ScreenManager.AddScreen(screen, this.ControllingPlayer);
          }
          this.ExitScreen();
          this.hasAddedScreens = true;
          this.ScreenManager.Game.ResetElapsedTime();
          break;
      }
    }

    public override void HandleInput(InputState input)
    {
    }

    public override void OnTransitionExitComplete()
    {
      base.OnTransitionExitComplete();
      if (this.doOnTransitionOffComplete == null)
        return;
      this.doOnTransitionOffComplete();
    }

    public void StartGame()
    {
      this.doOnTransitionOffComplete = new LoadingScreen2.DoOnTransitionOffComplete(LevelHandler.Instance.ChangeStateToIntro);
      foreach (GameScreen screen in Application.screenManager.GetScreens())
        screen.ExitScreen();
      Application.screenManager.AddScreen((GameScreen) new LoadSceneScreen(""));
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      this.ScreenManager.FadeBackBufferToBlack(1f);
    }

    public enum LoadingState
    {
      idle,
      loadingXml,
      loadMainMenu,
      loadingScene,
      loadingAssets,
      complete,
    }

    public delegate void DoOnTransitionOffComplete();
  }
}
