// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ButtonControl
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ButtonControl : Control
  {
    public TextControl textControl;
    public AudioClip buttonSound;
    private ImageControl background;
    private ButtonStyle buttonStyle;
    private ButtonControlStates previousState = ButtonControlStates.normal;
    private ButtonControlStates _state = ButtonControlStates.normal;
    public string link;
    private bool useCustomClickRect;
    private Rectangle _clickRect;
    private PressPlay.FFWD.Vector3 lastPressPosition;

    public ButtonControlStates state
    {
      get => this._state;
      set => this.ChangeState(value);
    }

    public Rectangle clickRect
    {
      get => this._clickRect;
      set
      {
        Rectangle clickRect = this.clickRect;
        this.useCustomClickRect = true;
        this._clickRect = value;
      }
    }

    public event EventHandler<EventArgs> OnClickEvent;

    protected virtual void OnClickMethod()
    {
      if (this.OnClickEvent == null)
        return;
      this.OnClickEvent((object) this, (EventArgs) new ButtonControlEventArgs(this.link));
    }

    public ButtonControl(ButtonStyle buttonStyle, string link)
    {
      this.gameObject.name = nameof (ButtonControl);
      this.buttonStyle = buttonStyle;
      this.link = link;
      this.background = new ImageControl(buttonStyle.texture, buttonStyle[1]);
      this.AddChild((Control) this.background);
      this.textControl = new TextControl();
      this.textControl.ignoreSize = true;
      this.AddChild((Control) this.textControl);
    }

    private void ChangeState(ButtonControlStates newState)
    {
      ((UISpriteRenderer) this.background.renderer).texture = this.buttonStyle.texture;
      ((UISpriteRenderer) this.background.renderer).sourceRect = this.buttonStyle.GetSourceRect((int) newState);
      this.previousState = this._state;
      this._state = newState;
    }

    public void ScaleTextToFit() => this.ScaleTextToFit(0.0f);

    public void ScaleText(float scale)
    {
      this.textControl.transform.localScale = new PressPlay.FFWD.Vector3(scale);
      this.InvalidateAutoSize();
    }

    public PressPlay.FFWD.Vector3 GetTextScale() => this.textControl.transform.localScale;

    public void ScaleTextToFit(float margin)
    {
      if ((double) this.textControl.bounds.Width < (double) this.background.bounds.Width - (double) margin)
        return;
      this.textControl.transform.localScale = new PressPlay.FFWD.Vector3((float) (((double) this.background.bounds.Width - (double) margin) / ((double) this.textControl.size.x * 1.1499999761581421)));
      this.InvalidateAutoSize();
    }

    public override void HandleInput(InputState input)
    {
      if (this.state == ButtonControlStates.disabled)
        return;
      base.HandleInput(input);
      if (this.isMouseWithinBounds(input))
      {
        if (input.isMouseDown)
        {
          if (this.state != ButtonControlStates.pressed)
          {
            this.ChangeState(ButtonControlStates.pressed);
            this.lastPressPosition = this.transform.position;
          }
        }
        else if (input.isMouseUp)
        {
          if (this.state == ButtonControlStates.pressed)
          {
            this.ChangeState(ButtonControlStates.normal);
            this.OnClickMethod();
          }
        }
        else if (this.state != ButtonControlStates.pressed && this.state != ButtonControlStates.hover)
          this.ChangeState(ButtonControlStates.hover);
      }
      else if (this.state == ButtonControlStates.hover || this.state == ButtonControlStates.pressed)
        this.ChangeState(ButtonControlStates.normal);
      if (this.state != ButtonControlStates.pressed || !(this.lastPressPosition != this.transform.position))
        return;
      this.ChangeState(ButtonControlStates.normal);
    }

    protected override bool isMouseWithinBounds(InputState input)
    {
      return this.useCustomClickRect ? this.isMouseWithinBounds(input, this.clickRect) : base.isMouseWithinBounds(input);
    }
  }
}
