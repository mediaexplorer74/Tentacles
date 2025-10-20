// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.UISpriteRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD.UI
{
  public class UISpriteRenderer : UIRenderer
  {
    public string Texture;
    private PressPlay.FFWD.Vector2 sourceOffset = PressPlay.FFWD.Vector2.zero;
    private Rectangle _sourceRect = Rectangle.Empty;
    public PressPlay.FFWD.Vector2 origin = PressPlay.FFWD.Vector2.zero;
    public float scale = 1f;
    public SpriteEffects effects;
    public float layerDepth;
    private PressPlay.FFWD.Vector2 drawPosition = PressPlay.FFWD.Vector2.zero;

    public Texture2D texture
    {
      get => this.material.texture;
      set => this.material.texture = value;
    }

    public Rectangle sourceRect
    {
      get
      {
        return this._sourceRect == Rectangle.Empty ? new Rectangle(0, 0, this.texture.Bounds.Width, this.texture.Bounds.Height) : this._sourceRect;
      }
      set => this._sourceRect = value;
    }

    public UISpriteRenderer()
    {
    }

    public UISpriteRenderer(string texture)
      : this()
    {
      this.Texture = texture;
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

    private void CalculateDrawOffset()
    {
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.texture == null || this.texture.IsDisposed)
        return 0;
      Rectangle rectangle;
      if (this.control != null)
      {
        rectangle = this.control.bounds;
      }
      else
      {
        PressPlay.FFWD.Vector2 position = (PressPlay.FFWD.Vector2) this.transform.position;
        PressPlay.FFWD.Vector2 lossyScale = (PressPlay.FFWD.Vector2) this.transform.lossyScale;
        rectangle = new Rectangle((int) position.x, (int) position.y, (int) lossyScale.x, (int) lossyScale.y);
      }
      float num = (float) (1.0 - (double) (float) this.transform.position / 10000.0);
      Camera.spriteBatch.Draw(this.texture, rectangle, new Rectangle?(this.sourceRect), (Microsoft.Xna.Framework.Color) this.material.color, this.transform.eulerAngles.y, (Microsoft.Xna.Framework.Vector2) this.origin, this.effects, num);
      return 0;
    }
  }
}
