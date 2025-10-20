// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.TextControl
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class TextControl : Control
  {
    private UITextRenderer textRenderer;
    private SpriteFont _font;
    private string _text;
    public bool useWordWrap;

    public TextControl.TextOrigin textOrigin
    {
      get => this.textRenderer.textOrigin;
      set => this.textRenderer.textOrigin = value;
    }

    public string text
    {
      get => this._text;
      set
      {
        if (!(this._text != value))
          return;
        this._text = value;
        this._text = this._text.Replace("”", "");
        this.textRenderer.text = this._text;
        this.InvalidateAutoSize();
      }
    }

    public SpriteFont font
    {
      get => this._font;
      set
      {
        if (this._font == value)
          return;
        this._font = value;
        this.textRenderer.font = value;
        this.InvalidateAutoSize();
      }
    }

    public TextControl()
      : this(string.Empty, (SpriteFont) null, PressPlay.FFWD.Color.white, PressPlay.FFWD.Vector2.zero)
    {
    }

    public TextControl(string text, SpriteFont font)
      : this(text, font, PressPlay.FFWD.Color.white, PressPlay.FFWD.Vector2.zero)
    {
    }

    public TextControl(string text, SpriteFont font, PressPlay.FFWD.Color color)
      : this(text, font, color, PressPlay.FFWD.Vector2.zero)
    {
    }

    public TextControl(string text, SpriteFont font, PressPlay.FFWD.Color color, PressPlay.FFWD.Vector2 position)
    {
      this.textRenderer = this.gameObject.AddComponent<UITextRenderer>(new UITextRenderer(text, font));
      this.gameObject.name = nameof (TextControl);
      this.text = text;
      this.font = font;
      this.position = position;
      this.SetColor(color);
    }

    public void ScaleTextToFit(Rectangle rect, float margin)
    {
      if ((double) this.bounds.Width < (double) rect.Width - (double) margin)
        return;
      this.transform.localScale = new PressPlay.FFWD.Vector3(((float) rect.Width - margin) / (float) this.bounds.Width);
      this.InvalidateAutoSize();
    }

    public void CenterTextWithinBounds(Rectangle rect)
    {
      PressPlay.FFWD.Vector2 zero = PressPlay.FFWD.Vector2.zero with
      {
        x = (float) (rect.X + rect.Width / 2) - this.size.x / 2f,
        y = (float) (rect.Y + rect.Height / 2) - this.size.y / 2f
      };
      this.transform.position = new PressPlay.FFWD.Vector3(zero.x, this.transform.position.y, zero.y);
    }

    public void SetColor(PressPlay.FFWD.Color color) => this.renderer.material.color = color;

    public override PressPlay.FFWD.Vector2 ComputeSize()
    {
      return this.font == null ? PressPlay.FFWD.Vector2.zero : (PressPlay.FFWD.Vector2) this.font.MeasureString(this.text);
    }

    public enum TextOrigin
    {
      normal,
      center,
    }
  }
}
