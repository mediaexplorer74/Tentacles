// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.InGamePauseMenu
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class InGamePauseMenu : MenuSceneScreen
  {
    private ImageControl bgTop;
    private ImageControl bgBottom;
    private List<ButtonControl> buttons = new List<ButtonControl>();

    public InGamePauseMenu()
      : base("Textures/Menu/EndLevel/EndLevelScreen")
    {
      this.IsPopup = true;
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 100f, 0.0f);
    }

    public void Init()
    {
      PanelControl child1 = new PanelControl();
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas");
      this.bgTop = new ImageControl(texture, SpritePositions.ingameMenuBackgroundTop);
      this.bgTop.transform.localPosition = new PressPlay.FFWD.Vector3(0.0f, 0.0f, -164f);
      this.rootControl.AddChild((Control) this.bgTop);
      TextControl child2 = LevelHandler.Instance.currentLevel.levelsIndex != 0 ? new TextControl(LocalisationManager.Instance.GetString("label_level").ToUpperInvariant() + " " + LevelHandler.Instance.currentLevel.levelsIndex.ToString(), GUIAssets.berlinsSans40) : new TextControl(LocalisationManager.Instance.GetString("menu_petridish").ToUpperInvariant(), GUIAssets.berlinsSans40);
      this.bgTop.AddChild((Control) child2);
      child2.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, 15f));
      this.bgBottom = new ImageControl(texture, SpritePositions.ingameMenuBackgroundBottom);
      this.rootControl.AddChild((Control) this.bgBottom);
      TextControl child3 = new TextControl(LocalisationManager.Instance.GetString("label_Pause").ToUpperInvariant(), GUIAssets.berlinsSans40);
      this.bgBottom.AddChild((Control) child3);
      child3.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, -80f));
      MenuButton child4 = new MenuButton(new ButtonStyle(texture, SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted), "exitToMenu");
      child4.textControl.font = GUIAssets.berlinsSans40;
      child4.textControl.text = LocalisationManager.Instance.GetString("label_menu").ToUpper();
      child4.ScaleTextToFit(20f);
      child4.textControl.AlignCenter();
      child4.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) child4);
      this.buttons.Add((ButtonControl) child4);
      MenuButton child5 = new MenuButton(new ButtonStyle(texture, SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted), "restart");
      child5.textControl.font = GUIAssets.berlinsSans40;
      child5.textControl.text = LocalisationManager.Instance.GetString("btn_restart").ToUpper();
      child5.ScaleTextToFit(20f);
      child5.textControl.AlignCenter();
      child5.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) child5);
      this.buttons.Add((ButtonControl) child5);
      MenuButton child6 = new MenuButton(new ButtonStyle(texture, SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted), "resume");
      child6.textControl.font = GUIAssets.berlinsSans40;
      child6.textControl.text = LocalisationManager.Instance.GetString("btn_resume").ToUpper();
      child6.ScaleTextToFit(20f);
      child6.textControl.AlignCenter();
      child6.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child1.AddChild((Control) child6);
      this.buttons.Add((ButtonControl) child6);
      child1.LayoutRow(0.0f, 0.0f, 0.0f);
      child1.transform.localPosition = new PressPlay.FFWD.Vector3(90f, 0.0f, 30f);
      this.bgBottom.AddChild((Control) child1);
      this.bgBottom.transform.localPosition = new PressPlay.FFWD.Vector3(0.0f, 0.0f, 480f);
      this.AdjustButtonScale();
      this.rootControl.gameObject.SetActiveRecursively(false);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.Open();
    }

    public override void UnloadContent()
    {
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

    private void OnButtonPress(object sender, EventArgs e)
    {
      switch (((ButtonControlEventArgs) e).link)
      {
        case "resume":
          this.ResumeGame();
          break;
        case "restart":
          this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_restart_level"), new Action(this.RestartLevel), (Action) null));
          break;
        case "exitToMenu":
          this.ScreenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_goto_menu_long"), new Action(this.OpenMainMenu), (Action) null));
          break;
      }
    }

    public void Open()
    {
      this.rootControl.gameObject.SetActiveRecursively(true);
      AudioManager.Instance.TurnOffAllSounds(AudioSettings.AudioType.SoundEffect);
      iTween.MoveTo(this.bgBottom.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "z", (object) 380, (object) "ignoretimescale", (object) true));
      iTween.MoveTo(this.bgTop.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "z", (object) -60, (object) "ignoretimescale", (object) true));
      LevelHandler.Instance.ingameGUI.ToggleScoreText(false);
      LevelHandler.Instance.musicController.FadeTo(0.3f, 0.5f);
      GlobalManager.Instance.Pause();
    }

    private void ResumeGame()
    {
      iTween.MoveTo(this.bgTop.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "z", (object) -164, (object) "ignoretimescale", (object) true));
      iTween.MoveTo(this.bgBottom.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "z", (object) 580, (object) "ignoretimescale", (object) true));
      GlobalManager.Instance.UnPause();
      LevelHandler.Instance.musicController.FadeTo(1f, 0.5f);
      AudioManager.Instance.TurnOnAllSounds(AudioSettings.AudioType.SoundEffect);
      LevelHandler.Instance.ingameGUI.ToggleScoreText(true);
      this.ExitScreen();
    }

    private void RestartLevel()
    {
      GlobalManager.Instance.UnPause();
      GlobalManager.Instance.RestartCurrentLevel();
      this.ExitScreen();
    }

    private void OpenMainMenu()
    {
      GlobalManager.Instance.UnPause();
      GlobalManager.Instance.OpenMainMenu();
      this.ExitScreen();
    }

    protected override void OnCancel(PlayerIndex playerIndex) => this.ResumeGame();

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void Draw(GameTime gameTime)
    {
    }
  }
}
