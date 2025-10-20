// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.TextRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class TextRenderer : Renderer
  {
    private PressPlay.FFWD.Vector2 textSize = PressPlay.FFWD.Vector2.zero;
    public PressPlay.FFWD.Color color = PressPlay.FFWD.Color.white;
    public PressPlay.FFWD.Vector2 Position;
    private string _text = "";
    private SpriteFont _font;

    public string text
    {
      get => this._text;
      set
      {
        if (!(value != this._text))
          return;
        this._text = value;
        this.textSize = (PressPlay.FFWD.Vector2) this.font.MeasureString(this._text);
      }
    }

    public SpriteFont font
    {
      get => this._font;
      set => this._font = value;
    }

    public TextRenderer(SpriteFont font)
      : this("", font)
    {
    }

    public TextRenderer(string text, SpriteFont font)
    {
      this.font = font;
      this.text = text;
    }

    public void Update()
    {
      this.Position.x = this.transform.localPosition.x;
      this.Position.y = this.transform.localPosition.y;
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      Camera.spriteBatch.DrawString(this.font, this.text, (Microsoft.Xna.Framework.Vector2) this.Position, (Microsoft.Xna.Framework.Color) this.material.color);
      return 0;
    }
  }
}
