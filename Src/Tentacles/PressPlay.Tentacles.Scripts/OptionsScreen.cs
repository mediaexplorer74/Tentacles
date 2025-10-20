// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.OptionsScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class OptionsScreen : BackgroundScreen
  {
    private Version version = new Version(1, 0, 0, 0);
    private OnOffSlider btnSound;
    private OnOffSlider btnMusic;
    private OnOffSlider btnVibration;
    private MenuButton btnLockedScreen;
    private MenuButton btnCredits;
    private bool runOnLockedScreen;

    public OptionsScreen(string background)
      : base(background)
    {
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 10f, 0.0f);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      TextControl child1 = new TextControl(LocalisationManager.Instance.GetString("menu_btn_helpAndOptions"), GUIAssets.berlinsSans40);
      child1.transform.localScale = child1.transform.localScale * 0.8f;
      child1.AlignCenter(new Rectangle(0, 0, 375, 50));
      this.rootControl.AddChild((Control) child1);
      ButtonStyle buttonStyle = new ButtonStyle(Application.Load<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas"), SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted);
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/HelpNOptions/menu_switch_atlas");
      ButtonStyle style = new ButtonStyle(texture, SpritePositions.onOffSliderOff, SpritePositions.onOffSliderOffHover, SpritePositions.onOffSliderOn, SpritePositions.onOffSliderOnHover);
      int x = 240;
      int num = 14;
      PanelControl child2 = new PanelControl();
      TextControl child3 = new TextControl(LocalisationManager.Instance.GetString("label_sound").ToUpper(), GUIAssets.berlinsSans40);
      child3.transform.localScale = child1.transform.localScale * 0.8f;
      child3.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(10f, 100f);
      child2.AddChild((Control) child3);
      this.btnSound = new OnOffSlider(texture, style, "sound");
      this.btnSound.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.btnSound.buttonSound = new AudioClip(GUIAssets.menuClick);
      this.btnSound.transform.localScale = new PressPlay.FFWD.Vector3(0.5f);
      this.btnSound.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) x, (float) (100 - num));
      this.btnSound.state = GlobalManager.Instance.currentProfile.soundIsEnabled ? OnOffSlider.ControlStates.on : OnOffSlider.ControlStates.off;
      child2.AddChild((Control) this.btnSound);
      TextControl child4 = new TextControl(LocalisationManager.Instance.GetString("label_music").ToUpper(), GUIAssets.berlinsSans40);
      child4.transform.localScale = child1.transform.localScale * 0.8f;
      child4.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(10f, 180f);
      child2.AddChild((Control) child4);
      this.btnMusic = new OnOffSlider(texture, style, "music");
      this.btnMusic.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.btnMusic.buttonSound = new AudioClip(GUIAssets.menuClick);
      this.btnMusic.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) x, (float) (180 - num));
      this.btnMusic.transform.localScale = new PressPlay.FFWD.Vector3(0.5f);
      this.btnMusic.state = GlobalManager.Instance.currentProfile.musicIsEnabled ? OnOffSlider.ControlStates.on : OnOffSlider.ControlStates.off;
      child2.AddChild((Control) this.btnMusic);
      TextControl child5 = new TextControl(LocalisationManager.Instance.GetString("label_vibration").ToUpper(), GUIAssets.berlinsSans40);
      child5.transform.localScale = child1.transform.localScale * 0.8f;
      child5.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(10f, 260f);
      child2.AddChild((Control) child5);
      this.btnVibration = new OnOffSlider(texture, style, "vibration");
      this.btnVibration.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.btnVibration.buttonSound = new AudioClip(GUIAssets.menuClick);
      this.btnVibration.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) x, (float) (260 - num));
      this.btnVibration.transform.localScale = new PressPlay.FFWD.Vector3(0.5f);
      this.btnVibration.state = GlobalManager.Instance.currentProfile.vibrationIsEnabled ? OnOffSlider.ControlStates.on : OnOffSlider.ControlStates.off;
      child2.AddChild((Control) this.btnVibration);
      this.btnCredits = new MenuButton(buttonStyle, "credits");
      this.btnCredits.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      this.btnCredits.buttonSound = new AudioClip(GUIAssets.menuClick);
      this.btnCredits.textControl.font = GUIAssets.berlinsSans40;
      this.btnCredits.textControl.text = LocalisationManager.Instance.GetString("menu_credits").ToUpper();
      this.btnCredits.ScaleTextToFit(30f);
      this.btnCredits.textControl.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, 0.0f));
      this.btnCredits.AlignCenter(new Rectangle(0, 330, 375, 100));
      child2.AddChild((Control) this.btnCredits);
      this.rootControl.AddChild((Control) child2);
      TextControl child6 = new TextControl(this.version.ToString() + ", support@pressplay.dk", GUIAssets.berlinsSans40);
      child6.transform.localScale = child1.transform.localScale * 0.4f;
      child6.AlignCenter(new Rectangle(0, 430, 375, 50));
      this.rootControl.AddChild((Control) child6);
      this.SetTransitionPositionOnControls(1f);
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
      switch (((ButtonControlEventArgs) e).link)
      {
        case "sound":
          GlobalManager.Instance.currentProfile.soundIsEnabled = !GlobalManager.Instance.currentProfile.soundIsEnabled;
          AudioManager.Instance.soundIsEnabled = GlobalManager.Instance.currentProfile.soundIsEnabled;
          GlobalManager.Instance.SaveToDisk();
          break;
        case "music":
          GlobalManager.Instance.currentProfile.musicIsEnabled = !GlobalManager.Instance.currentProfile.musicIsEnabled;
          GlobalManager.Instance.SaveToDisk();
          break;
        case "vibration":
          GlobalManager.Instance.currentProfile.vibrationIsEnabled = !GlobalManager.Instance.currentProfile.vibrationIsEnabled;
          GlobalManager.Instance.SaveToDisk();
          break;
        case "locked":
          this.runOnLockedScreen = !this.runOnLockedScreen;
          if (this.runOnLockedScreen)
          {
            this.btnLockedScreen.textControl.text = LocalisationManager.Instance.GetString("label_on").ToUpper();
            break;
          }
          this.btnLockedScreen.textControl.text = LocalisationManager.Instance.GetString("label_off").ToUpper();
          break;
        case "credits":
          this.ScreenManager.AddScreen((GameScreen) new CreditsScreen("Textures/Menu/Credits/credits_bg"), new PlayerIndex?(PlayerIndex.One));
          break;
      }
    }

    public override void OnTransitionOffComplete()
    {
      base.OnTransitionOffComplete();
      this.rootControl.gameObject.SetActiveRecursively(false);
      this.background.gameObject.SetActiveRecursively(false);
    }

    public override void OnTransitionOnBegin()
    {
      base.OnTransitionOnBegin();
      if (this.rootControl != null)
        this.rootControl.gameObject.SetActiveRecursively(true);
      this.background.gameObject.SetActiveRecursively(true);
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
  }
}
