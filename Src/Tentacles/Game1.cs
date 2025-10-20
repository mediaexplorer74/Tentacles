// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Game1
// Assembly: Tentacles, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 94733B2D-6956-40B2-A474-EF03B0110429
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\Tentacles.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.Tentacles.Scripts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace PressPlay.Tentacles
{
  public class Game1 : Game
  {
    private bool showingTitleUpdateMessage;
    private bool titleUpdateRequired;
    private bool titleUpdateRequiredWasAnswered;
    private GraphicsDeviceManager graphics;
    private ContentManager content;
    private PressPlay.FFWD.ScreenManager.ScreenManager screenManager;
    private bool _isDeactivated;
    private bool isFirstRun = true;
    private string oldCrashInfo = string.Empty;
    private string loadLevel = "";

    private bool isDeactivated
    {
      get => this._isDeactivated;
      set
      {
        this._isDeactivated = value;
        PressPlay.FFWD.Application.isDeactivated = value;
      }
    }

    public Game1()
    {
      this.graphics = new GraphicsDeviceManager((Game) this);
      this.Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
      Debug.Log((object) "Disabling IdleMode");
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      if (storeForApplication.FileExists("crash"))
      {
        using (StreamReader streamReader = new StreamReader((Stream) storeForApplication.OpenFile("crash", FileMode.Open)))
          this.oldCrashInfo = streamReader.ReadToEnd();
      }
      this.graphics.PreferredBackBufferWidth = 800;
      this.graphics.PreferredBackBufferHeight = 480;
      this.graphics.IsFullScreen = true;
      this.graphics.ApplyChanges();
      this.IsFixedTimeStep = true;
      this.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 30.0);
      ((Collection<IGameComponent>) this.Components).Add((IGameComponent) new PressPlay.FFWD.Application((Game) this));
      Debug.DisplayLog = false;
      ApplicationSettings.ShowDebugLines = false;
      this.content = new ContentManager((IServiceProvider) this.Services, this.Content.RootDirectory);
      this.screenManager = new PressPlay.FFWD.ScreenManager.ScreenManager((Game) this);
      this.screenManager.blankTextureSource = "Textures/blank_texture";
      ((Collection<IGameComponent>) this.Components).Add((IGameComponent) this.screenManager);
      base.Initialize();
    }

    protected virtual void OnExiting(object sender, EventArgs args)
    {
      base.OnExiting(sender, args);
      if (GlobalManager.Instance != null && GlobalManager.Instance.globalState == GlobalManager.GlobalState.inLevel && LevelHandler.Instance != null)
      {
        if (LevelHandler.Instance.state == LevelHandler.LevelState.endscreen || LevelHandler.Instance.state == LevelHandler.LevelState.outro || LevelHandler.Instance.state == LevelHandler.LevelState.finalizeEndScreen)
        {
          if (GlobalManager.Instance.database.IsLastLevelInWorld(LevelHandler.Instance.currentLevel))
          {
            GlobalManager.Instance.currentProfile.levelLoadInfo.loadLevelOnStartup = false;
          }
          else
          {
            GlobalManager.Instance.currentProfile.levelLoadInfo.levelId = LevelHandler.Instance.currentLevel.nextLevel;
            GlobalManager.Instance.currentProfile.levelLoadInfo.loadLevelOnStartup = true;
          }
        }
        else
        {
          GlobalManager.Instance.currentProfile.levelLoadInfo.levelId = LevelHandler.Instance.currentLevel.id;
          GlobalManager.Instance.currentProfile.levelLoadInfo.loadLevelOnStartup = true;
        }
      }
      if (GlobalManager.Instance == null)
        return;
      GlobalManager.Instance.SaveToDisk();
    }

    protected virtual void OnActivated(object sender, EventArgs args)
    {
      base.OnActivated(sender, args);
      this.isDeactivated = false;
      if (this.isFirstRun)
      {
        this.isFirstRun = false;
      }
      else
      {
        TrialModeManager.Instance.ForcedUpdateTrial();
        if (AudioManager.Instance != null)
        {
          AudioManager.Instance.soundIsEnabled = GlobalManager.Instance.currentProfile.soundIsEnabled;
          AudioManager.Instance.musicIsEnabled = GlobalManager.Instance.currentProfile.musicIsEnabled;
        }
        if (this.screenManager == null)
          return;
        this.screenManager.OnActivated();
      }
    }

    protected virtual void OnDeactivated(object sender, EventArgs args)
    {
      base.OnDeactivated(sender, args);
      if (AudioManager.Instance != null)
      {
        AudioManager.Instance.musicIsEnabled = false;
        AudioManager.Instance.soundIsEnabled = false;
      }
      this.isDeactivated = true;
    }

    protected override void LoadContent()
    {
      SimpleRotate simpleRotate = new SimpleRotate();
      PressPlay.FFWD.Application.Preload<Scene>("Preloader");
      this.screenManager.AddScreen((GameScreen) new PreloaderScreen(this.content.Load<Texture2D>("Textures/MGS_WP7_Horiz_Still"), this.loadLevel));
    }

    protected override void BeginRun() => base.BeginRun();

    protected override void UnloadContent() => this.content.Unload();

    protected override void Update(GameTime gameTime)
    {
      if (this.isDeactivated)
        return;
      if (this.titleUpdateRequired)
        this.HandleGameUpdateRequired();
      try
      {
        base.Update(gameTime);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Update exception: " + ex.Message));
      }
    }

    protected override void Draw(GameTime gameTime)
    {
      this.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
      if (this.isDeactivated)
        return;
      try
      {
        base.Draw(gameTime);
      }
      catch (Exception ex)
      {
        Debug.Log((object) ("Draw exception: " + ex.Message));
      }
    }

    private void HandleGameUpdateRequiredException(Exception e)
    {
      this.titleUpdateRequired = true;
    }

    private void HandleGameUpdateRequired()
    {
      if (this.titleUpdateRequiredWasAnswered || this.showingTitleUpdateMessage || !GlobalManager.isLoaded || !LocalisationManager.isLoaded)
        return;
      this.showingTitleUpdateMessage = false;
      this.titleUpdateRequiredWasAnswered = true;
      GlobalManager.titleUpdateDeclined = true;
    }

    public void UpdateTitleConfirmBoxCallback(IAsyncResult r)
    {
      this.titleUpdateRequiredWasAnswered = true;
      this.NegativeConfirmUpdateTitle();
    }

    public void PositiveConfirmUpdateTitle()
    {
      this.showingTitleUpdateMessage = false;
    }

    public void NegativeConfirmUpdateTitle()
    {
      GlobalManager.titleUpdateDeclined = true;
      this.showingTitleUpdateMessage = false;
    }
  }
}
