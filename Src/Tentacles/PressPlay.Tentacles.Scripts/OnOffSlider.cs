// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.OnOffSlider
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI;
using PressPlay.FFWD.UI.Controls;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class OnOffSlider : Control
  {
    private OnOffSlider.ControlStates _state;
    public AudioClip buttonSound;
    private ImageControl background;
    private ButtonStyle buttonStyle;
    private string link;

    public OnOffSlider(Texture2D texture, ButtonStyle style, string link)
    {
      this.link = link;
      this.buttonStyle = style;
      this.background = new ImageControl(texture, style.GetSourceRect((int) this.state));
      this.AddChild((Control) this.background);
    }

    public OnOffSlider.ControlStates state
    {
      get => this._state;
      set
      {
        this._state = value;
        this.ChangeState();
      }
    }

    public event EventHandler<EventArgs> OnClickEvent;

    protected virtual void OnClickMethod()
    {
      if (this.buttonSound != null)
        AudioManager.Instance.Play(this.buttonSound);
      if (this.OnClickEvent == null)
        return;
      this.OnClickEvent((object) this, (EventArgs) new ButtonControlEventArgs(this.link));
    }

    public override void HandleInput(InputState input)
    {
      base.HandleInput(input);
      if (this.isMouseWithinBounds(input))
      {
        if (input.isMouseDown)
        {
          if (this.state == OnOffSlider.ControlStates.on)
            this.state = OnOffSlider.ControlStates.onHover;
          if (this.state != OnOffSlider.ControlStates.off)
            return;
          this.state = OnOffSlider.ControlStates.offHover;
        }
        else
        {
          if (!input.isMouseUp)
            return;
          if (this.state == OnOffSlider.ControlStates.onHover)
          {
            this.state = OnOffSlider.ControlStates.off;
            this.OnClickMethod();
          }
          if (this.state != OnOffSlider.ControlStates.offHover)
            return;
          this.state = OnOffSlider.ControlStates.on;
          this.OnClickMethod();
        }
      }
      else
      {
        if (this.state == OnOffSlider.ControlStates.offHover)
          this.state = OnOffSlider.ControlStates.off;
        if (this.state != OnOffSlider.ControlStates.onHover)
          return;
        this.state = OnOffSlider.ControlStates.on;
      }
    }

    private void ChangeState()
    {
      ((UISpriteRenderer) this.background.renderer).sourceRect = this.buttonStyle.GetSourceRect((int) this.state);
    }

    public enum ControlStates
    {
      on,
      off,
      onHover,
      offHover,
    }
  }
}
