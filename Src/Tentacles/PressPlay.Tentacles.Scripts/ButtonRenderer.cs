// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ButtonRenderer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ButtonRenderer : SpriteRenderer
  {
    private string _text = "";
    public bool upperCase;
    public SpriteFont font;
    public PressPlay.FFWD.Vector2 textOffset = PressPlay.FFWD.Vector2.zero;
    private PressPlay.FFWD.Vector2 textSize = PressPlay.FFWD.Vector2.zero;
    private bool textHasChanged;

    public string text
    {
      get => this._text;
      set
      {
        this._text = value;
        this.textHasChanged = true;
      }
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      base.Draw(device, cam);
      if (this.font == null || this.text == "")
        return 0;
      if (this.textHasChanged)
      {
        this.textSize = (PressPlay.FFWD.Vector2) this.font.MeasureString(this._text);
        this.textHasChanged = false;
      }
      cam.SpriteBatch.DrawString(this.font, this.upperCase ? this._text.ToUpper() : this._text, (Microsoft.Xna.Framework.Vector2) new PressPlay.FFWD.Vector2((float) ((double) this.transform.localPosition.x + (double) (this.bounds.Width / 2) - (double) this.textSize.x / 2.0) + this.textOffset.x, (float) ((double) this.transform.localPosition.y + (double) (this.bounds.Height / 2) - (double) this.textSize.y / 2.0) + this.textOffset.y), Microsoft.Xna.Framework.Color.White);
      return 1;
    }
  }
}
