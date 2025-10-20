// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ButtonControlOld
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ButtonControlOld : Control
  {
    public TextControl textControl;
    private string link;
    private bool useCustomClickRect;
    private Rectangle _clickRect;

    public ButtonControlOld(Texture2D texture, string link)
      : this(texture, link, "", (SpriteFont) null, PressPlay.FFWD.Vector2.zero)
    {
    }

    public ButtonControlOld(
      Texture2D texture,
      string link,
      string text,
      SpriteFont font,
      PressPlay.FFWD.Vector2 textPosition)
    {
      this.gameObject.name = "ButtonControl";
      this.link = link;
      this.AddChild((Control) new ImageControl(texture));
      if (!(text != "") || font == null)
        return;
      this.textControl = new TextControl(text, font, PressPlay.FFWD.Color.white, textPosition);
      this.AddChild((Control) this.textControl);
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

    protected internal virtual void OnClickMethod()
    {
      if (this.OnClickEvent == null)
        return;
      this.OnClickEvent((object) this, (EventArgs) new ButtonControlEventArgs(this.link));
    }

    public override void HandleInput(InputState input) => base.HandleInput(input);

    protected bool IsMouseClickWithinBounds(Point p)
    {
      return this.useCustomClickRect ? this.clickRect.Contains(p) : this.bounds.Contains(p);
    }
  }
}
