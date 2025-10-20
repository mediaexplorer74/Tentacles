// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.UIButtonRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD.UI
{
  public class UIButtonRenderer : UISpriteRenderer
  {
    private string _text = "";
    public bool upperCase;
    public SpriteFont font;
    public Vector2 textOffset = Vector2.zero;
    public Color textColor = Color.white;
    private Vector2 textSize = Vector2.zero;
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

    public UIButtonRenderer()
    {
    }

    public UIButtonRenderer(string texture)
      : base(texture)
    {
    }

    public override void Awake()
    {
      base.Awake();
      ContentHelper.LoadTexture(this.Texture);
    }

    public override void Start()
    {
      base.Start();
      if (this.texture == null)
        this.texture = ContentHelper.GetTexture(this.Texture);
      if (this.material != null)
        return;
      this.material = new Material();
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      base.Draw(device, cam);
      if (this.font == null || this.text == "" || !this.textHasChanged)
        return 0;
      this.textSize = (Vector2) this.font.MeasureString(this._text);
      this.textHasChanged = false;
      return 0;
    }
  }
}
