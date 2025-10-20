// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PreloaderScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PreloaderScreen : GameScreen
  {
    private Texture2D splash;
    private Stopwatch stopwatch;
    private List<Texture2D> splashScreenTexture;
    private List<float> splashScreenTiming;
    private int splashCount;
    private int fadeTime = 750;
    private string levelToLoad = "";
    private PreloaderScreen.PreloadState _state;

    private PreloaderScreen.PreloadState state
    {
      set => this.ChangeState(value);
      get => this._state;
    }

    private void ChangeState(PreloaderScreen.PreloadState value)
    {
      this._state = value;
      switch (value)
      {
        case PreloaderScreen.PreloadState.loadingPreloadScene:
          Application.LoadLevel("Scenes/Preloader");
          break;
        case PreloaderScreen.PreloadState.loadingStaticAssets:
          this.LoadStaticAssets();
          break;
        case PreloaderScreen.PreloadState.loadingProfile:
          this.LoadProfile();
          break;
        case PreloaderScreen.PreloadState.loadingMainMenu:
          this.DoStartUpLoad();
          break;
      }
    }

    public PreloaderScreen(Texture2D splash, string levelToLoad)
    {
      this.levelToLoad = levelToLoad;
      this.splashScreenTexture = new List<Texture2D>();
      this.splashScreenTiming = new List<float>();
      this.AddSplashScreen(splash, ApplicationSettings.MGSLogoSplashTime);
      this.stopwatch = new Stopwatch();
      this.stopwatch.Start();
    }

    private void AddSplashScreen(Texture2D texture, float timing)
    {
      this.splashScreenTexture.Add(texture);
      this.splashScreenTiming.Add(timing);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.state = PreloaderScreen.PreloadState.loadingPreloadScene;
    }

    public override void UnloadContent()
    {
      base.UnloadContent();
      this.stopwatch.Stop();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      switch (this.state)
      {
        case PreloaderScreen.PreloadState.loadingPreloadScene:
          if ((double) Application.loadingProgress != 1.0 || Application.isLoadingAssetBeforeSceneInitialize)
            break;
          PressPlay.FFWD.Debug.Log((object) "Preload scene is finished loading!");
          this.state = PreloaderScreen.PreloadState.loadingStaticAssets;
          break;
        case PreloaderScreen.PreloadState.loadingStaticAssets:
          this.state = PreloaderScreen.PreloadState.loadingProfile;
          break;
        case PreloaderScreen.PreloadState.showSplashScreens:
          if ((double) this.stopwatch.Elapsed.Seconds <= (double) this.splashScreenTiming[this.splashCount])
            break;
          if (this.splashCount < this.splashScreenTexture.Count - 1)
          {
            ++this.splashCount;
            this.stopwatch.Reset();
            this.stopwatch.Start();
            break;
          }
          this.state = PreloaderScreen.PreloadState.loadingMainMenu;
          break;
        case PreloaderScreen.PreloadState.loadingMainMenu:
          if ((double) Application.loadingProgress != 1.0)
            break;
          this.ExitScreen();
          break;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      this.ScreenManager.SpriteBatch.Begin();
      if (this.splashCount >= this.splashScreenTexture.Count)
        return;
      this.ScreenManager.SpriteBatch.Draw(this.splashScreenTexture[this.splashCount], Microsoft.Xna.Framework.Vector2.Zero, (Microsoft.Xna.Framework.Color) PressPlay.FFWD.Color.white);
      this.ScreenManager.SpriteBatch.End();
    }

    private void LoadStaticAssets()
    {
      Application.PreloadInstant<SpriteFont>("Textures/Fonts/TentaclesFont20");
      GUIAssets.defaultFont = Application.Load<SpriteFont>("Textures/Fonts/TentaclesFont20");
      GUIAssets.defaultFont.Spacing = -1.5f;
      Application.PreloadInstant<SpriteFont>("Textures/Fonts/TentaclesFont40");
      GUIAssets.berlinsSans40 = Application.Load<SpriteFont>("Textures/Fonts/TentaclesFont40");
      GUIAssets.berlinsSans40.Spacing = -4.5f;
      Application.PreloadInstant<SoundEffect>("Sounds/Menu/MenuKlik#03");
      GUIAssets.menuClick = Application.Load<SoundEffect>("Sounds/Menu/MenuKlik#03");
      Application.PreloadInstant<SoundEffect>("Sounds/Menu/MenuWhosh#05");
      GUIAssets.menuBackSound = Application.Load<SoundEffect>("Sounds/Menu/MenuWhosh#05");
      Application.PreloadInstant<Texture2D>("Textures/blank_texture");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Main/MainMenuBg");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelSelection/LevelSelectButtonAtlas");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelSelection/LevelSelectionStarAtlas");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelSelection/LevelSelectionBG");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Leaderboards/LeaderboardsBg");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LeaderBoards/Leaderboards_bg_overlay");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LeaderBoards/Leaderboards_buttonAtlas");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_atlas");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bar_tutorial");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_veins");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_brain");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_intestines");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_stomach");
      Application.PreloadInstant<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_tutorial");
      Application.PreloadInstant<Texture2D>("Textures/Menu/misc_assets");
      Application.PreloadInstant<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas");
      Application.PreloadInstant<SoundEffect>("Sounds/menu/EndLevelScreen/Kaboom");
      Application.PreloadInstant<SoundEffect>("Sounds/menu/EndLevelScreen/starhit");
      Application.PreloadInstant<SoundEffect>("Sounds/menu/EndLevelScreen/noStar");
      Application.PreloadInstant<SoundEffect>("Sounds/menu/EndLevelScreen/countup7");
      Application.PreloadInstant<SoundEffect>("Sounds/menu/EndLevelScreen/Fast zing");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Achievements/AchievementsBg");
      Application.PreloadInstant<Texture2D>("Textures/Menu/HelpNOptions/HelpNOptionsBg");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Credits/credits_bg");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Achievements/achievementBoxAtlas");
      Application.PreloadInstant<Texture2D>("Textures/Menu/Credits/credits_gradient");
      Application.AddStaticAsset("Models/suction_cup");
      Application.AddStaticAsset("Textures/circleWhite");
      Application.AddStaticAsset("Textures/RingWhite");
      Application.AddStaticAsset("Textures/sprite_pickup_enemy");
      Application.AddStaticAsset("Sounds/CuteEyeEat#07");
      Application.AddStaticAsset("Sounds/CuteEyeEat#06");
      Application.AddStaticAsset("Sounds/CuteEyeEat#05");
      Application.AddStaticAsset("Sounds/CuteEyeEat#04");
      Application.AddStaticAsset("Sounds/CuteEyeEat#03");
      Application.AddStaticAsset("Sounds/CuteEyeEat#02");
      Application.AddStaticAsset("Sounds/CuteEyeEat#01");
      Application.AddStaticAsset("Sounds/CuteEyeRip#07");
      Application.AddStaticAsset("Sounds/CuteEyeRip#06");
      Application.AddStaticAsset("Sounds/CuteEyeRip#05");
      Application.AddStaticAsset("Sounds/CuteEyeRip#04");
      Application.AddStaticAsset("Sounds/CuteEyeRip#03");
      Application.AddStaticAsset("Sounds/CuteEyeRip#02");
      Application.AddStaticAsset("Sounds/CuteEyeRip#01");
      Application.AddStaticAsset("Sounds/PointPickupBlop#01");
      Application.AddStaticAsset("Textures/enemy_sprite_eye");
      Application.AddStaticAsset("Models/sprite_square");
      Application.AddStaticAsset("Textures/white_circle");
      Application.AddStaticAsset("Textures/font_tentacles");
      Application.AddStaticAsset("Models/lemmy_body");
      Application.AddStaticAsset("Textures/lemmy_eye_anim");
      Application.AddStaticAsset("Textures/lemmy_eye_anim");
      Application.AddStaticAsset("Sounds/LemmyApuHi33");
      Application.AddStaticAsset("Sounds/LemmyApuHi32");
      Application.AddStaticAsset("Sounds/LemmyApuHi31");
      Application.AddStaticAsset("Sounds/LemmyApuMed42");
      Application.AddStaticAsset("Sounds/LemmyApuMed38");
      Application.AddStaticAsset("Sounds/LemmyApuMed41");
      Application.AddStaticAsset("Sounds/LemmyApuMed29");
      Application.AddStaticAsset("Sounds/LemmyApuMed28");
      Application.AddStaticAsset("Sounds/LemmyApuMed39");
      Application.AddStaticAsset("Sounds/LemmyApuMed34");
      Application.AddStaticAsset("Sounds/LemmyApuMed04");
      Application.AddStaticAsset("Sounds/LemmyApuMed02");
      Application.AddStaticAsset("Sounds/LemmyAbe#11");
      Application.AddStaticAsset("Sounds/LemmyAbe#10");
      Application.AddStaticAsset("Sounds/LemmyAbe#09");
      Application.AddStaticAsset("Sounds/LemmyApuLo07");
      Application.AddStaticAsset("Sounds/LemmyApuLo24");
      Application.AddStaticAsset("Sounds/LemmyApuLo03");
      Application.AddStaticAsset("Sounds/LemmyApuLo37");
      Application.AddStaticAsset("Sounds/LemmyApuLo18");
      Application.AddStaticAsset("Sounds/LemmyApuLo05");
      Application.AddStaticAsset("Sounds/LemmyApuLo14");
      Application.AddStaticAsset("Sounds/LemmyApuLo13");
      Application.AddStaticAsset("Sounds/LemmyApuLo11");
      Application.AddStaticAsset("Sounds/LemmyApuLo09");
      Application.AddStaticAsset("Sounds/LemmyApuLo01");
      Application.AddStaticAsset("Sounds/LemmyAbeSkr#04");
      Application.AddStaticAsset("Sounds/LemmyAbeSkr#03");
      Application.AddStaticAsset("Sounds/LemmyAbeSkr#02");
      Application.AddStaticAsset("Sounds/LemmyAbeSkr#01");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#21");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#20");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#19");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#18");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#17");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#16");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#15");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#14");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#13");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#12");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#11");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#10");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#09");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#08");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#07");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#06");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#05");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#04");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#03");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#02");
      Application.AddStaticAsset("Sounds/LemmyThrowNEW-4dB#01");
      Application.AddStaticAsset("Textures/lemmy_splat");
      Application.AddStaticAsset("Textures/lemmy_bubble_light");
      Application.AddStaticAsset("Models/lemmy_claw");
      Application.AddStaticAsset("Models/lemmy_body");
      Application.AddStaticAsset("Textures/lemmy_eye_anim");
      Application.AddStaticAsset("Textures/lemmy_tentacle_mini");
      Application.AddStaticAsset("Models/simple_round_flat_enemy");
      Application.AddStaticAsset("Models/w01");
      Application.AddStaticAsset("Models/w02");
      Application.AddStaticAsset("Textures/sprite_pickup_point");
      Application.AddStaticAsset("Models/hazard_spike_rotating01");
      Application.AddStaticAsset("Sounds/ChalSuccesGAIN");
      Application.AddStaticAsset("Sounds/ChalFailGAIN");
      Application.AddStaticAsset("Sounds/ChalStartGAIN");
      Application.AddStaticAsset("Sounds/PointPickupBlop#05");
      Application.AddStaticAsset("Textures/tentacle_glow");
      Application.AddStaticAsset("Textures/lemmy_mouth 1");
      Application.AddStaticAsset("Textures/damage_gradient1");
      Application.AddStaticAsset("Textures/damage_gradient2");
      Application.AddStaticAsset("Textures/damage_gradient3");
      Application.AddStaticAsset("Textures/damage_gradient4");
      Application.AddStaticAsset("Textures/damage_gradient5");
      Application.AddStaticAsset("Textures/damage_gradient6");
      Application.AddStaticAsset("Textures/damage_gradient7");
      Application.AddStaticAsset("Textures/damage_gradient8");
      Application.AddStaticAsset("Textures/damage_gradient9");
      Application.AddStaticAsset("Textures/damage_gradient10");
      Application.AddStaticAsset("Textures/damage_gradient11");
      Application.AddStaticAsset("Textures/damage_gradient12");
      Application.AddStaticAsset("Textures/damage_gradient13");
      Application.AddStaticAsset("Textures/damage_gradient14");
      Application.AddStaticAsset("Textures/damage_gradient15");
      Application.AddStaticAsset("Textures/damage_gradient16");
      Application.AddStaticAsset("Sounds/menu/EndLevelScreen/Kaboom");
      Application.AddStaticAsset("Sounds/menu/EndLevelScreen/starhit");
      Application.AddStaticAsset("Sounds/menu/EndLevelScreen/noStar");
      Application.AddStaticAsset("Sounds/menu/EndLevelScreen/countup7");
      Application.AddStaticAsset("Sounds/menu/EndLevelScreen/Fast zing");
      Application.AddStaticAsset("Sounds/drop");
      Application.AddStaticAsset("Sounds/Zing 02_cut");
      Application.AddStaticAsset("Sounds/Zing 02_cut_rev");
      Application.AddStaticAsset("Models/harmless_door");
      Application.AddStaticAsset("Sounds/SausageDoorOpen#01 ");
      Application.AddStaticAsset("Sounds/SausageDoorOpen#02");
      Application.AddStaticAsset("Sounds/SausageDoorOpen#03");
      Application.AddStaticAsset("Sounds/SausageDoorClose#01");
      Application.AddStaticAsset("Sounds/SausageDoorClose#02");
      Application.AddStaticAsset("Sounds/SausageDoorClose#03");
      Application.AddStaticAsset("Sounds/SausageDoorClose#04");
      Application.AddStaticAsset("Models/hazard_spike_wall_straight_noAnim");
      Application.AddStaticAsset("Models/hazard_spike_claw");
      Application.AddStaticAsset("Textures/lightning01");
      Application.AddStaticAsset("Textures/dandelion_green");
      Application.AddStaticAsset("Textures/enemy_wallJumper");
      Application.AddStaticAsset("Models/enemy_wallJumper_chunks");
      this.AddSplashScreen(Application.PreloadInstant<Texture2D>("Textures/Menu/loading_pressplaylogo"), ApplicationSettings.pressPlayLogoSplashTime);
    }

    private void LoadProfile()
    {
      PressPlay.FFWD.Debug.Log((object) "Loading profile!");
      this.state = PreloaderScreen.PreloadState.showSplashScreens;
      GlobalManager.Instance.LoadProfile();
    }

    private void DoStartUpLoad()
    {
      if (!GlobalManager.Instance.currentProfile.IsValid())
      {
        GlobalManager.Instance.ResetProfile();
        Application.screenManager.AddScreen((GameScreen) new AlertPopup(LocalisationManager.Instance.GetString("conf_corrupt_save_game"), new Action(this.DisconfirmOpenLastPlayedLevel)));
      }
      else
      {
        if (GlobalManager.Instance.currentProfile.levelLoadInfo.loadLevelOnStartup)
        {
          GlobalManager.Instance.currentProfile.levelLoadInfo.loadLevelOnStartup = false;
          if (GlobalManager.Instance.database.GetLevel(GlobalManager.Instance.currentProfile.levelLoadInfo.levelId) != null)
          {
            Application.screenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_open_last_played_level"), new Action(this.ConfirmOpenLastPlayedLevel), new Action(this.DisconfirmOpenLastPlayedLevel)));
            return;
          }
        }
        else if (!string.IsNullOrEmpty(this.levelToLoad))
          SceneLoaderManager.LoadPreloader(this.levelToLoad);
        SceneLoaderManager.Instance.DoStartUpLoad();
      }
    }

    public void ConfirmOpenLastPlayedLevel()
    {
      GlobalManager.Instance.OpenLevel(GlobalManager.Instance.database.GetLevel(GlobalManager.Instance.currentProfile.levelLoadInfo.levelId));
    }

    public void DisconfirmOpenLastPlayedLevel() => SceneLoaderManager.Instance.DoStartUpLoad();

    public enum PreloadState
    {
      idle,
      loadingPreloadScene,
      loadingStaticAssets,
      loadingProfile,
      showSplashScreens,
      loadingMainMenu,
    }
  }
}
