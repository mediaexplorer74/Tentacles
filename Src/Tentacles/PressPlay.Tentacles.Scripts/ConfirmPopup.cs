// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ConfirmPopup
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ConfirmPopup : MenuSceneScreen
  {
    private Action positiveCallback;
    private Action negativeCallback;
    private string message;
    private TextControl txtMessage;
    private MenuButton btnPositive;
    private MenuButton btnNegative;
    private ImageControl overlay;

    public ConfirmPopup(string message, Action positiveCallback, Action negativeCallback)
      : base("")
    {
      this.positiveCallback = positiveCallback;
      this.negativeCallback = negativeCallback;
      this.message = message;
      this.IsPopup = true;
    }

    public override void LoadContent()
    {
      Texture2D texture = Application.Load<Texture2D>("Textures/blank_texture");
      ButtonStyle buttonStyle1;
      ButtonStyle buttonStyle2 = buttonStyle1 = new ButtonStyle(Application.Load<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas"), SpritePositions.ingameMenuButtonNormal, SpritePositions.ingameMenuButtonHighlighted);
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(this.rootControl.transform.position.x, 1000f, this.rootControl.transform.position.z);
      this.overlay = new ImageControl(texture);
      this.overlay.transform.localScale = this.overlay.transform.localScale * 25f;
      this.overlay.gameObject.renderer.material.color = PressPlay.FFWD.Color.black;
      this.rootControl.AddChild((Control) this.overlay);
      this.txtMessage = new TextControl(this.message, GUIAssets.berlinsSans40);
      this.txtMessage.InvalidateAutoSize();
      this.txtMessage.ScaleTextToFit(new Rectangle(PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height / 2, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width - 50, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height - 200), 0.0f);
      this.txtMessage.AlignCenter(new Rectangle(0, 0, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width, (int) ((double) PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height * 0.7)));
      this.rootControl.AddChild((Control) this.txtMessage);
      PanelControl child = new PanelControl();
      this.rootControl.AddChild((Control) child);
      this.btnPositive = new MenuButton(buttonStyle2, "positive");
      this.btnPositive.textControl.font = GUIAssets.berlinsSans40;
      this.btnPositive.textControl.text = LocalisationManager.Instance.GetString("btn_yes").ToUpper();
      this.btnPositive.textControl.ignoreSize = true;
      this.btnPositive.textControl.AlignCenter();
      this.btnPositive.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child.AddChild((Control) this.btnPositive);
      this.btnNegative = new MenuButton(buttonStyle2, "negative");
      this.btnNegative.textControl.font = GUIAssets.berlinsSans40;
      this.btnNegative.textControl.text = LocalisationManager.Instance.GetString("btn_no-1").ToUpper();
      this.btnNegative.textControl.ignoreSize = true;
      this.btnNegative.textControl.AlignCenter();
      this.btnNegative.OnClickEvent += new EventHandler<EventArgs>(this.OnButtonPress);
      child.AddChild((Control) this.btnNegative);
      child.LayoutRow();
      child.AlignCenter(new Rectangle(0, (int) ((double) this.txtMessage.size.y / 2.0), PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height));
      this.SetTransitionPositionOnControls(1f);
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      this.overlay.DoTransition(0.3f);
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
      switch (((ButtonControlEventArgs) e).link)
      {
        case "positive":
          if (this.positiveCallback != null)
          {
            this.positiveCallback();
            break;
          }
          break;
        case "negative":
          if (this.negativeCallback != null)
          {
            this.negativeCallback();
            break;
          }
          break;
      }
      this.ExitScreen();
    }

    protected override void OnCancel(PlayerIndex playerIndex)
    {
      if (this.negativeCallback != null)
        this.negativeCallback();
      base.OnCancel(playerIndex);
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
