// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Menu.MainMenu.EndLevelScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts.Menu.MainMenu
{
  public class EndLevelScreen : MenuSceneScreen
  {
    private EndLevelScreen.EndLevelSequenceState _state;
    public Action callback;
    private TimeSpan timeout;
    private TextControl scoreText;
    private TextControl bestScore;
    private ImageControl panelTop;
    private ImageControl panelBottom;
    private PanelControl starPanel;
    private ImageControl scorebar;
    private TextControl highscoreLabel;
    private MenuButton btnExit;
    private MenuButton btnContinue;
    private MenuButton btnRetry;
    private float timeScale = 1f;
    private List<EndLevelStar> stars = new List<EndLevelStar>();
    private int starsCounted;
    private GameObject callbackObject;
    private CallbackObject callbackHandler;
    private EndLevelStarControl pickupStarControl;
    private EndLevelStarControl deathStarControl;
    private EndLevelStarControl challengeStarControl;
    private AudioClip sndKaboom;
    private AudioClip[] sndStar;
    private AudioClip[] sndNoStar;
    private AudioClip sndCountUp;
    private AudioObject sndCountUpObject;
    private AudioClip sndSwoosh;
    private AudioClip sndSwoosh1;
    private List<ButtonControl> buttons = new List<ButtonControl>();

    private EndLevelScreen.EndLevelSequenceState state
    {
      get => this._state;
      set => this.ChangeState(value);
    }

    public EndLevelScreen(Action callback)
      : base("Level finished!")
    {
      this.callback = callback;
      this.IsPopup = true;
      this.timeout = TimeSpan.FromSeconds(5.0);
      Debug.Log((object) "Creating EndLevelScreen");
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 100f, 0.0f);
    }

    public void Init()
    {
      this.sndKaboom = new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/Kaboom"));
      this.sndStar = new AudioClip[3]
      {
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/starhit")),
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/starhit")),
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/starhit"))
      };
      this.sndNoStar = new AudioClip[3]
      {
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/noStar")),
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/noStar")),
        new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/noStar"))
      };
      this.sndCountUp = new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/countup7"));
      this.sndSwoosh = new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/Fast zing"));
      this.sndSwoosh1 = new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/Fast zing"));
      this.sndCountUpObject = AudioManager.Instance.Add(new AudioSettings(this.sndCountUp, 1f, false), "menu", 1);
      this.CreateMenuElements();
      this.rootControl.gameObject.SetActiveRecursively(false);
    }

    public void Open()
    {
      this.rootControl.gameObject.SetActiveRecursively(true);
      AudioManager.Instance.FadeAllSounds(0.0f, 0.5f);
      LevelHandler.Instance.musicController.FadeTo(0.3f, 0.5f);
      this.callbackObject = new GameObject("callbackObject");
      this.callbackHandler = this.callbackObject.AddComponent<CallbackObject>(new CallbackObject());
      this.stars.Add(new EndLevelStar(LevelHandler.Instance.levelSession.EvaluateAllPickupsMedal(), LocalisationManager.Instance.GetString("label_challenge"), ""));
      this.stars.Add(new EndLevelStar(LevelHandler.Instance.levelSession.EvaluateCompletedWithoutDying(), LocalisationManager.Instance.GetString("label_pickups"), ""));
      this.stars.Add(new EndLevelStar(LevelHandler.Instance.levelSession.EvaluateCompletedChallenge(), LocalisationManager.Instance.GetString("label_deaths"), ""));
      this.scoreText.text = LevelHandler.Instance.levelSession.score.ToString();
      this.scoreText.AlignCenter(new PressPlay.FFWD.Vector2((float) (-this.scoreText.bounds.Width / 2 - 10), 0.0f));
      this.bestScore.text = GlobalManager.Instance.currentProfile.GetLevelScore(GlobalManager.Instance.currentLevel.worldId, GlobalManager.Instance.currentLevel.id).ToString();
      this.bestScore.transform.position = new PressPlay.FFWD.Vector3((float) ((double) this.scorebar.transform.position.x + (double) this.scorebar.bounds.Width - (double) this.bestScore.bounds.Width - 20.0), this.highscoreLabel.transform.position.y, this.highscoreLabel.transform.position.z);
      this.state = EndLevelScreen.EndLevelSequenceState.intro;
      this.pickupStarControl.star.gameObject.active = false;
      this.deathStarControl.star.gameObject.active = false;
      this.challengeStarControl.star.gameObject.active = false;
    }

    public override void UnloadContent()
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.Open();
    }

    private void CreateMenuElements()
    {
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas");
      ButtonStyle buttonStyle1;
      ButtonStyle buttonStyle2 = buttonStyle1 = new ButtonStyle(texture, SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted);
      this.panelTop = new ImageControl(texture, SpritePositions.ingameMenuBackgroundTop);
      this.rootControl.AddChild((Control) this.panelTop);
      this.panelBottom = new ImageControl(texture, SpritePositions.ingameMenuBackgroundBottom);
      PanelControl child1 = new PanelControl();
      this.panelBottom.AddChild((Control) child1);
      this.scorebar = new ImageControl(texture, SpritePositions.endlevelScoreBackground);
      this.panelBottom.AddChild((Control) this.scorebar);
      this.scorebar.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, -20f));
      this.scoreText = new TextControl(LevelHandler.Instance.levelSession.score.ToString(), GUIAssets.berlinsSans40);
      this.scorebar.AddChild((Control) this.scoreText);
      this.scoreText.transform.localScale *= 0.75f;
      this.scoreText.InvalidateAutoSize();
      this.scoreText.AlignCenter(new PressPlay.FFWD.Vector2((float) (-this.scoreText.bounds.Width / 2 - 10), 0.0f));
      TextControl child2 = new TextControl(LocalisationManager.Instance.GetString("label_score") + ":", GUIAssets.berlinsSans40);
      this.scorebar.AddChild((Control) child2);
      child2.transform.localScale *= 0.75f;
      child2.InvalidateAutoSize();
      child2.transform.position = new PressPlay.FFWD.Vector3(this.scorebar.transform.position.x + 15f, child2.transform.position.y, this.scorebar.transform.position.z + (float) (this.scorebar.bounds.Height / 2) - (float) (child2.bounds.Height / 2));
      this.highscoreLabel = new TextControl(LocalisationManager.Instance.GetString("label_highscore") + ":", GUIAssets.berlinsSans40);
      this.scorebar.AddChild((Control) this.highscoreLabel);
      this.highscoreLabel.transform.localScale *= 0.75f;
      this.highscoreLabel.InvalidateAutoSize();
      this.highscoreLabel.AlignCenter(new PressPlay.FFWD.Vector2((float) (this.highscoreLabel.bounds.Width / 2 + 10), 0.0f));
      this.bestScore = new TextControl(GlobalManager.Instance.currentProfile.GetLevelScore(GlobalManager.Instance.currentLevel.worldId, GlobalManager.Instance.currentLevel.id).ToString(), GUIAssets.berlinsSans40);
      this.scorebar.AddChild((Control) this.bestScore);
      this.bestScore.transform.localScale *= 0.75f;
      this.bestScore.InvalidateAutoSize();
      this.bestScore.transform.localPosition = new PressPlay.FFWD.Vector3((float) ((double) this.highscoreLabel.transform.position.x + (double) this.highscoreLabel.bounds.Width + 10.0), this.highscoreLabel.transform.position.y, this.highscoreLabel.transform.position.z);
      this.btnExit = new MenuButton(buttonStyle2, "exitToMenu");
      this.btnExit.textControl.font = GUIAssets.berlinsSans40;
      this.btnExit.textControl.text = LocalisationManager.Instance.GetString("label_menu").ToUpper();
      this.btnExit.textControl.ignoreSize = true;
      this.btnExit.ScaleTextToFit(20f);
      this.btnExit.textControl.AlignCenter();
      this.btnExit.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) this.btnExit);
      this.buttons.Add((ButtonControl) this.btnExit);
      this.btnRetry = new MenuButton(buttonStyle2, "restart");
      this.btnRetry.textControl.font = GUIAssets.berlinsSans40;
      this.btnRetry.textControl.text = LocalisationManager.Instance.GetString("btn_restart").ToUpper();
      this.btnRetry.textControl.ignoreSize = true;
      this.btnRetry.ScaleTextToFit(20f);
      this.btnRetry.textControl.AlignCenter();
      this.btnRetry.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) this.btnRetry);
      this.buttons.Add((ButtonControl) this.btnRetry);
      this.btnContinue = new MenuButton(buttonStyle2, "resume");
      this.btnContinue.textControl.font = GUIAssets.berlinsSans40;
      this.btnContinue.textControl.text = LocalisationManager.Instance.GetString("label_continue").ToUpper();
      this.btnContinue.textControl.ignoreSize = true;
      this.btnContinue.ScaleTextToFit(20f);
      this.btnContinue.textControl.AlignCenter();
      this.btnContinue.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) this.btnContinue);
      this.buttons.Add((ButtonControl) this.btnContinue);
      child1.LayoutRow(0.0f, 0.0f, 20f);
      child1.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, 48f));
      this.rootControl.AddChild((Control) this.panelBottom);
      this.starPanel = new PanelControl();
      this.panelTop.AddChild((Control) this.starPanel);
      this.pickupStarControl = new EndLevelStarControl(new ImageControl(texture, SpritePositions.starPanelNeutralOne), new ImageControl(texture, SpritePositions.star), new TextControl(LocalisationManager.Instance.GetString("label_pickups").ToUpper(), GUIAssets.berlinsSans40), new TextControl("", GUIAssets.berlinsSans40));
      this.pickupStarControl.star.AlignCenter(new PressPlay.FFWD.Vector2(5f, 11f));
      this.pickupStarControl.title.AlignCenter(new PressPlay.FFWD.Vector2(5f, -52f));
      this.pickupStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(5f, 60f));
      this.starPanel.AddChild((Control) this.pickupStarControl);
      this.deathStarControl = new EndLevelStarControl(new ImageControl(texture, SpritePositions.starPanelNeutralTwo), new ImageControl(texture, SpritePositions.star), new TextControl(LocalisationManager.Instance.GetString("label_deaths").ToUpper(), GUIAssets.berlinsSans40), new TextControl("", GUIAssets.berlinsSans40));
      this.deathStarControl.star.AlignCenter(new PressPlay.FFWD.Vector2(-1f, 11f));
      this.deathStarControl.title.AlignCenter(new PressPlay.FFWD.Vector2(-1f, -52f));
      this.deathStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(-1f, 60f));
      this.starPanel.AddChild((Control) this.deathStarControl);
      this.challengeStarControl = new EndLevelStarControl(new ImageControl(texture, SpritePositions.starPanelNeutralThree), new ImageControl(texture, SpritePositions.star), new TextControl(LocalisationManager.Instance.GetString("label_challenge").ToUpper(), GUIAssets.berlinsSans40), new TextControl("", GUIAssets.berlinsSans40));
      this.challengeStarControl.star.AlignCenter(new PressPlay.FFWD.Vector2(-4f, 11f));
      this.challengeStarControl.title.AlignCenter(new PressPlay.FFWD.Vector2(-4f, -52f));
      this.challengeStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(-4f, 60f));
      this.starPanel.AddChild((Control) this.challengeStarControl);
      this.starPanel.LayoutRow();
      this.starPanel.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, -12f));
      this.panelTop.transform.localPosition = new PressPlay.FFWD.Vector3(0.0f, 0.0f, (float) -this.panelTop.bounds.Height);
      this.panelBottom.transform.localPosition = new PressPlay.FFWD.Vector3(0.0f, 0.0f, 480f);
      this.AdjustButtonScale();
      this.SetTransitionPositionOnControls(1f);
    }

    private void AdjustButtonScale()
    {
      float scale = 10000f;
      for (int index = 0; index < this.buttons.Count; ++index)
      {
        if ((double) this.buttons[index].GetTextScale().x < (double) scale)
          scale = this.buttons[index].GetTextScale().x;
      }
      for (int index = 0; index < this.buttons.Count; ++index)
      {
        this.buttons[index].ScaleText(scale);
        this.buttons[index].textControl.AlignCenter();
      }
    }

    private void DoIntroAnimation()
    {
      AudioManager.Instance.Play(this.sndKaboom);
      iTween.MoveBy(this.panelTop.gameObject, iTween.Hash((object) "time", (object) (float) (0.5 * (double) this.timeScale), (object) "delay", (object) 0.25f, (object) "z", (object) this.panelTop.bounds.Height, (object) "oncomplete", (object) "DoOnComplete", (object) "oncompletetarget", (object) this.callbackObject));
      this.callbackHandler.Reset();
      this.callbackHandler.onComplete = new CallbackObject.CallbackCompleteMethod(this.OnIntroAnimationComplete);
    }

    public void OnIntroAnimationComplete()
    {
      this.state = EndLevelScreen.EndLevelSequenceState.stars;
    }

    public void DoStarSequence()
    {
      this.callbackHandler.Reset();
      this.callbackHandler.onUpdate = new CallbackObject.CallbackUpdateMethod(this.OnStarSequenceUpdate);
      this.callbackHandler.onComplete = this.starsCounted >= this.stars.Count - 1 ? new CallbackObject.CallbackCompleteMethod(this.OnStarSequenceComplete) : new CallbackObject.CallbackCompleteMethod(this.OnStarSequenceMedioComplete);
      if (this.starsCounted == 0)
        this.sndCountUpObject.Play();
      iTween.ValueTo(this.callbackObject, iTween.Hash((object) "time", (object) (float) (0.60000002384185791 * (double) this.timeScale), (object) "from", (object) 0, (object) "to", (object) 1, (object) "onupdate", (object) "DoOnUpdate", (object) "oncomplete", (object) "DoOnComplete"));
    }

    private void DoStarCompletion(int starNumber)
    {
      this.DoStarUpdate(starNumber, 1f);
      this.sndCountUpObject.Stop();
      switch (starNumber)
      {
        case 0:
          if (LevelHandler.Instance.levelSession.EvaluateAllPickupsMedal())
          {
            this.pickupStarControl.star.gameObject.active = true;
            ((UISpriteRenderer) this.pickupStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelGreenOne;
            AudioManager.Instance.Play(this.sndStar[starNumber]);
            break;
          }
          ((UISpriteRenderer) this.pickupStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelRedOne;
          AudioManager.Instance.Play(this.sndNoStar[starNumber]);
          break;
        case 1:
          if (LevelHandler.Instance.levelSession.EvaluateCompletedWithoutDying())
          {
            this.deathStarControl.star.gameObject.active = true;
            ((UISpriteRenderer) this.deathStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelGreenTwo;
            AudioManager.Instance.Play(this.sndStar[starNumber]);
            break;
          }
          ((UISpriteRenderer) this.deathStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelRedTwo;
          AudioManager.Instance.Play(this.sndNoStar[starNumber]);
          break;
        case 2:
          if (LevelHandler.Instance.levelSession.EvaluateCompletedChallenge())
          {
            this.challengeStarControl.star.gameObject.active = true;
            ((UISpriteRenderer) this.challengeStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelGreenThree;
            AudioManager.Instance.Play(this.sndStar[starNumber]);
            break;
          }
          ((UISpriteRenderer) this.challengeStarControl.background.gameObject.renderer).sourceRect = SpritePositions.starPanelRedThree;
          AudioManager.Instance.Play(this.sndNoStar[starNumber]);
          break;
      }
    }

    private void DoStarUpdate(int starNumber, float progress)
    {
      progress = Mathf.Clamp01(progress);
      switch (starNumber)
      {
        case 0:
          if ((double) progress == 1.0)
          {
            if (LevelHandler.Instance.levelSession.EvaluateAllPickupsMedal())
              this.pickupStarControl.footer.text = LocalisationManager.Instance.GetString("label_challengeOK");
          }
          else
            this.pickupStarControl.footer.text = ((int) ((double) LevelHandler.Instance.levelSession.numberOfPickupsCollected * (double) progress)).ToString() + "/" + (object) LevelHandler.Instance.levelSession.totalNumberOfPickups;
          this.pickupStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(5f, 60f));
          break;
        case 1:
          if ((double) progress != 1.0)
            break;
          this.deathStarControl.footer.text = !LevelHandler.Instance.levelSession.EvaluateCompletedWithoutDying() ? LocalisationManager.Instance.GetString("label_challengeFail") : LocalisationManager.Instance.GetString("label_challengeOK");
          this.deathStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(-1f, 60f));
          break;
        case 2:
          if ((double) progress != 1.0)
            break;
          this.challengeStarControl.footer.text = !LevelHandler.Instance.levelSession.EvaluateCompletedChallenge() ? LocalisationManager.Instance.GetString("label_challengeFail") : LocalisationManager.Instance.GetString("label_challengeOK");
          this.challengeStarControl.footer.AlignCenter(new PressPlay.FFWD.Vector2(-4f, 60f));
          break;
      }
    }

    public void OnStarSequenceMedioComplete()
    {
      this.DoStarCompletion(this.starsCounted);
      ++this.starsCounted;
      this.DoStarSequence();
    }

    public void OnStarSequenceUpdate(float value) => this.DoStarUpdate(this.starsCounted, value);

    public void OnStarSequenceComplete()
    {
      this.DoStarCompletion(this.starsCounted);
      this.state = EndLevelScreen.EndLevelSequenceState.showScore;
    }

    private void DoShowScoreAnimation()
    {
      this.callbackHandler.onComplete = new CallbackObject.CallbackCompleteMethod(this.OnShowScoreComplete);
      iTween.MoveBy(this.panelBottom.gameObject, iTween.Hash((object) "time", (object) (float) (0.5 * (double) this.timeScale), (object) "delay", (object) (float) (0.20000000298023224 * (double) this.timeScale), (object) "z", (object) (-this.panelBottom.bounds.Height / 2 - 18), (object) "oncomplete", (object) "DoOnComplete", (object) "oncompletetarget", (object) this.callbackObject));
      AudioManager.Instance.Play(new AudioSettings(this.sndSwoosh, 1f, 1f, false, 0.2f * this.timeScale), "menu", 1);
    }

    public void OnShowScoreComplete()
    {
      this.state = EndLevelScreen.EndLevelSequenceState.showButtons;
    }

    private void DoShowButtonAnimation()
    {
      iTween.MoveBy(this.panelBottom.gameObject, iTween.Hash((object) "time", (object) (float) (1.0 * (double) this.timeScale), (object) "delay", (object) (float) (0.5 * (double) this.timeScale), (object) "z", (object) (-this.panelBottom.bounds.Height / 2 + 18)));
      AudioManager.Instance.Play(new AudioSettings(this.sndSwoosh1, 1f, 1f, false, 0.5f * this.timeScale), "menu", 1);
    }

    private void DoOutroAnimation()
    {
    }

    private void ChangeState(EndLevelScreen.EndLevelSequenceState value)
    {
      switch (value)
      {
        case EndLevelScreen.EndLevelSequenceState.intro:
          this.DoIntroAnimation();
          break;
        case EndLevelScreen.EndLevelSequenceState.stars:
          this.DoStarSequence();
          break;
        case EndLevelScreen.EndLevelSequenceState.showScore:
          this.DoShowScoreAnimation();
          break;
        case EndLevelScreen.EndLevelSequenceState.showButtons:
          this.DoShowButtonAnimation();
          break;
      }
      this._state = value;
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
      switch (((ButtonControlEventArgs) e).link)
      {
        case "resume":
          this.Continue();
          break;
        case "restart":
          this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_restart_level"), new Action(this.RestartLevel), (Action) null));
          break;
        case "exitToMenu":
          this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_goto_menu"), new Action(this.OpenMainMenu), (Action) null));
          break;
      }
    }

    private void Continue()
    {
      if (this.callback != null)
        this.callback();
      GlobalManager.Instance.UnPause();
      this.ExitScreen();
    }

    private void RestartLevel()
    {
      if (this.callback != null)
        this.callback();
      GlobalManager.Instance.SaveToDisk();
      GlobalManager.Instance.UnPause();
      GlobalManager.Instance.RestartCurrentLevel();
      this.ExitScreen();
    }

    private void OpenMainMenu()
    {
      if (this.callback != null)
        this.callback();
      GlobalManager.Instance.SaveToDisk();
      GlobalManager.Instance.UnPause();
      GlobalManager.Instance.OpenMainMenu();
      this.ExitScreen();
    }

    protected override void OnCancel(PlayerIndex playerIndex)
    {
      this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_goto_menu"), new Action(this.OpenMainMenu), (Action) null));
    }

    public override void HandleInput(InputState input) => base.HandleInput(input);

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);

    public enum EndLevelSequenceState
    {
      loading,
      intro,
      stars,
      showScore,
      showButtons,
      outro,
    }
  }
}
