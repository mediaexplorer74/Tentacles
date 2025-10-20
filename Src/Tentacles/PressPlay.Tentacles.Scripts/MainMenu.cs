// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MainMenu
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MainMenu : BackgroundScreen
  {
    [ContentSerializerIgnore]
    public static bool openUpsellOnMainMenuLoad;
    public MusicController musicController;
    private PanelControl buttonColumn;
    private ImageControl logo;
    private Texture2D buttonsSpriteSheet;
    private TextControl trialText;
    private ButtonControl playButton;
    private ButtonControl buyButton;
    private ButtonControl levelSelectButton;
    private ButtonControl helpAndOptionsButton;
    private ButtonControl achievementsButton;
    private ButtonControl leaderboardsButton;
    private bool layoutIsTrial;
    private List<ButtonControl> buttons = new List<ButtonControl>();

    public MainMenu()
      : base("Textures/Menu/Main/MainMenuBg")
    {
      UnityObject.DontDestroyOnLoad((UnityObject) this.rootControl);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      GlobalManager.Instance.fullscreenImageHandler.DoInstantBlackScreen();
      GlobalManager.Instance.fullscreenImageHandler.FadeFromBlack(0.5f);
      this.logo = new ImageControl(Application.Load<Texture2D>("Textures/Menu/Main/mainMenu_logo_" + (object) GlobalManager.Instance.deviceLanguage), SpritePositions.logo);
      this.rootControl.AddChild((Control) this.logo);
      this.musicController = new GameObject().AddComponent<MusicController>();
      this.musicController.playMusicOnStart = true;
      this.musicController.musicToPlayOnStart = MusicController.BackgroundLoopId.loop1;
      this.musicController.name = "MusicController - MainMenu";
      this.musicController.Init("PetriLoop.wav", "PetriLoop.wav", "PetriLoop.wav", "PetriLoop.wav", "PetriLoop.wav", "PetriLoop.wav", "PetriLoop.wav");
      this.musicController.FadeFromTo(0.0f, 1f, 1.5f);
      AudioClip audioClip = new AudioClip(GUIAssets.menuClick);
      this.buttonsSpriteSheet = Application.Load<Texture2D>("Textures/Menu/Main/mainMenu_buttonAtlas_" + (object) GlobalManager.Instance.deviceLanguage);
      this.playButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuPlayButtonNormal, SpritePositions.mainMenuPlayButtonActive), "play");
      this.playButton.transform.position = new PressPlay.FFWD.Vector3(140f, 100f, 235f);
      this.playButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.playButton.buttonSound = audioClip;
      this.rootControl.AddChild((Control) this.playButton);
      if (GlobalManager.isTrialMode)
      {
        this.layoutIsTrial = true;
        this.playButton.transform.position = new PressPlay.FFWD.Vector3(140f, 100f, 170f);
        this.buyButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuPurchaseButtonNormal, SpritePositions.mainMenuPurchaseButtonActive), "buygame");
        this.buyButton.transform.position = new PressPlay.FFWD.Vector3(0.0f, 0.0f, 355f);
        this.buyButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
        this.buyButton.buttonSound = audioClip;
        this.rootControl.AddChild((Control) this.buyButton);
        this.trialText = new TextControl(LocalisationManager.Instance.GetString("label_trial"), GUIAssets.berlinsSans40);
        this.logo.AddChild((Control) this.trialText);
        this.trialText.AlignCenter(new PressPlay.FFWD.Vector2(10f, 20f));
      }
      this.buttonColumn = new PanelControl();
      this.levelSelectButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuButtonOneNormal, SpritePositions.mainMenuButtonOneActive), "level");
      this.levelSelectButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.levelSelectButton.buttonSound = audioClip;
      this.buttonColumn.AddChild((Control) this.levelSelectButton);
      this.helpAndOptionsButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuButtonTwoNormal, SpritePositions.mainMenuButtonTwoActive), "help");
      this.helpAndOptionsButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.helpAndOptionsButton.buttonSound = audioClip;
      this.buttonColumn.AddChild((Control) this.helpAndOptionsButton);
      this.achievementsButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuButtonThreeNormal, SpritePositions.mainMenuButtonThreeActive), "achievements");
      this.achievementsButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.achievementsButton.buttonSound = audioClip;
      this.buttonColumn.AddChild((Control) this.achievementsButton);
      this.leaderboardsButton = (ButtonControl) new MenuButton(new ButtonStyle(this.buttonsSpriteSheet, SpritePositions.mainMenuButtonFourNormal, SpritePositions.mainMenuButtonFourActive), "leaderboard");
      this.leaderboardsButton.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.leaderboardsButton.buttonSound = audioClip;
      this.buttonColumn.AddChild((Control) this.leaderboardsButton);
      this.buttonColumn.LayoutColumn(0.0f, 0.0f, 20f);
      this.buttonColumn.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) (PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width - 280), 35f);
      this.rootControl.AddChild((Control) this.buttonColumn);
      this.ScreenManager.NotifyOtherScreens((GameScreen) this);
    }

    public override void OnActivated()
    {
      base.OnActivated();
      Debug.Log((object) "Main Menu Activated!");
      if (!this.layoutIsTrial || GlobalManager.isTrialMode)
        return;
      this.RemoveTrialLayout();
    }

    private void RemoveTrialLayout()
    {
      this.playButton.transform.position = new PressPlay.FFWD.Vector3(140f, 100f, 235f);
      if (this.buyButton != null)
        this.buyButton.transform.position = new PressPlay.FFWD.Vector3(-10000f, 0.0f, 0.0f);
      if (this.trialText != null)
        this.trialText.transform.position = new PressPlay.FFWD.Vector3(-10000f, 0.0f, 0.0f);
      this.layoutIsTrial = false;
      GlobalManager.Instance.CheckForSendingTrialScoresToLeaderboard();
    }

    public override void OnTransitionOffComplete()
    {
      base.OnTransitionOffComplete();
      if (this.rootControl.gameObject != null)
        this.rootControl.gameObject.SetActiveRecursively(false);
      if (this.background.gameObject == null)
        return;
      this.background.gameObject.SetActiveRecursively(false);
    }

    public override void OnTransitionOnBegin()
    {
      base.OnTransitionOnBegin();
      if (this.rootControl != null)
        this.rootControl.gameObject.SetActiveRecursively(true);
      this.background.gameObject.SetActiveRecursively(true);
    }

    public override void OnTransitionOnComplete() => base.OnTransitionOnComplete();

    private void CacheMenuTextures()
    {
    }

    private void OnPlayButtonFadeComplete(object sender, EventArgs e)
    {
      GlobalManager.Instance.OpenLevel(GlobalManager.Instance.database.GetLevel(GlobalManager.Instance.currentProfile.globalData.playButtonOpensThisLevel));
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
      switch (((ButtonControlEventArgs) e).link)
      {
        case "play":
          if (GlobalManager.Instance.currentProfile.globalData.playButtonOpensThisLevel == -1)
          {
            this.ScreenManager.AddScreen((GameScreen) new LevelSelection(), new PlayerIndex?(PlayerIndex.One));
            break;
          }
          if (GlobalManager.isTrialMode && !GlobalManager.Instance.currentProfile.IsLevelPartOfTrial(GlobalManager.Instance.currentProfile.globalData.playButtonOpensThisLevel))
          {
            this.ScreenManager.AddScreen((GameScreen) new LevelSelection(), new PlayerIndex?(PlayerIndex.One));
            break;
          }
          MusicController.Instance.FadeTo(0.0f, 0.25f);
          GlobalManager.Instance.fullscreenImageHandler.FadeToBlack(0.25f, new EventHandler<EventArgs>(this.OnPlayButtonFadeComplete));
          break;
        case "level":
          this.ScreenManager.AddScreen((GameScreen) new LevelSelection(), new PlayerIndex?(PlayerIndex.One));
          break;
        case "story":
          this.ScreenManager.AddScreen((GameScreen) new NotImplementedScreen("Textures/Menu/Main/background_mainmenu"), new PlayerIndex?(PlayerIndex.One));
          break;
        case "achievements":
          this.ScreenManager.AddScreen((GameScreen) new AchievementScreen("Textures/Menu/Achievements/AchievementsBg"), new PlayerIndex?(PlayerIndex.One));
          break;
        case "leaderboard":
          this.ScreenManager.AddScreen((GameScreen) new LeaderBoardScreen("Textures/Menu/Leaderboards/LeaderboardsBg"), new PlayerIndex?(PlayerIndex.One));
          break;
        case "help":
          this.ScreenManager.AddScreen((GameScreen) new OptionsScreen("Textures/Menu/HelpNOptions/HelpNOptionsBg"), new PlayerIndex?(PlayerIndex.One));
          break;
        case "buygame":
          Guide.ShowMarketplace(Gamer.SignedInGamers[PlayerIndex.One].PlayerIndex);
          break;
      }
    }

    public override void HandleInput(InputState input)
    {
      if (MainMenu.openUpsellOnMainMenuLoad)
      {
        MainMenu.openUpsellOnMainMenuLoad = false;
        this.ScreenManager.AddScreen((GameScreen) new UpsellScreen());
      }
      if (this.layoutIsTrial && !GlobalManager.isTrialMode)
        this.RemoveTrialLayout();
      base.HandleInput(input);
    }

    protected override void OnCancel(PlayerIndex playerIndex)
    {
      this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_quit"), new Action(this.OnDoQuit), (Action) null));
    }

    private void OnDoQuit()
    {
      if (GlobalManager.isTrialMode)
        this.ScreenManager.AddScreen((GameScreen) new UpsellScreen(new UpsellScreen.DoOncancel(Application.Quit)));
      else
        Application.Quit();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
