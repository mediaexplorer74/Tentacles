// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UpsellScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class UpsellScreen : BackgroundScreen
  {
    private bool returningFromMarketplace;
    public UpsellScreen.DoOncancel doOnCancel;

    public UpsellScreen()
      : base("Textures/Menu/Upsell/upsell_bg_" + (object) GlobalManager.Instance.deviceLanguage)
    {
      this.doOnCancel = (UpsellScreen.DoOncancel) null;
    }

    public UpsellScreen(UpsellScreen.DoOncancel doOnCancel)
      : base("Textures/Menu/Upsell/upsell_bg_" + (object) GlobalManager.Instance.deviceLanguage)
    {
      this.doOnCancel = doOnCancel;
    }

    public UpsellScreen(string background)
      : base(background)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      UnityObject.DontDestroyOnLoad((UnityObject) this.rootControl);
      AudioClip audioClip = new AudioClip(Application.Load<SoundEffect>("Sounds/Menu/MenuKlik#03"));
      MenuButton child = new MenuButton(new ButtonStyle(Application.Load<Texture2D>("Textures/Menu/Upsell/upsell_buttonAtlas_" + (object) GlobalManager.Instance.deviceLanguage), SpritePositions.upsellPurchaseButtonNormal, SpritePositions.upsellPurchaseButtonActive), "buygame");
      child.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child.buttonSound = audioClip;
      child.transform.localPosition = new PressPlay.FFWD.Vector3((float) (400 - SpritePositions.upsellPurchaseButtonNormal.Width / 2), 100f, (float) (480 - SpritePositions.upsellPurchaseButtonNormal.Height - 5));
      this.rootControl.AddChild((Control) child);
      this.SetTransitionPositionOnControls(1f);
    }

    public override void OnActivated()
    {
      base.OnActivated();
      if (!this.returningFromMarketplace)
        return;
      this.returningFromMarketplace = false;
      this.ExitScreen();
      if (!GlobalManager.isTrialMode || this.doOnCancel == null)
        return;
      this.doOnCancel();
    }

    protected override void OnCancel(PlayerIndex playerIndex)
    {
      AudioManager.Instance.Play(new AudioClip(GUIAssets.menuBackSound));
      this.ExitScreen();
      if (this.doOnCancel == null)
        return;
      this.doOnCancel();
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
      this.returningFromMarketplace = true;
      // Guide.ShowMarketplace is not available in MonoGame, so we skip this functionality
      // Guide.ShowMarketplace(Gamer.SignedInGamers[PlayerIndex.One].PlayerIndex);
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

    public delegate void DoOncancel();
  }
}
