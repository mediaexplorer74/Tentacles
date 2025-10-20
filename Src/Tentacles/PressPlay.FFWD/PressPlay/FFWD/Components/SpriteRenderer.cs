// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.SpriteRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class SpriteRenderer : Renderer, PressPlay.FFWD.Interfaces.IUpdateable
  {
    [ContentSerializer(Optional = true)]
    public string Texture;
    [ContentSerializerIgnore]
    private Texture2D _texture;
    public PressPlay.FFWD.Vector2 Position = PressPlay.FFWD.Vector2.zero;
    public Rectangle bounds = Rectangle.Empty;
    public PressPlay.FFWD.Vector2 Origin = PressPlay.FFWD.Vector2.zero;
    public float Scale = 1f;
    public SpriteEffects Effects;
    public float LayerDepth;

    public Texture2D texture
    {
      get => this._texture;
      set
      {
        this._texture = value;
        if (this._texture == null)
          return;
        this.bounds = this._texture.Bounds;
      }
    }

    public SpriteRenderer()
    {
    }

    public SpriteRenderer(string texture)
    {
      this.Texture = texture;
      if (this.material != null)
        return;
      this.material = new Material();
      this.material.renderQueue = 1000;
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
      {
        this.texture = ContentHelper.GetTexture(this.Texture);
        this.material.texture = this.texture;
      }
      if (this.texture != null)
        this.bounds = this.texture.Bounds;
      if (this.material == null)
        this.material = new Material();
      this.material.renderQueue = 1000;
    }

    public void Update()
    {
      this.Position.x = this.transform.localPosition.x;
      this.Position.y = this.transform.localPosition.y;
    }

    public void LateUpdate()
    {
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.texture == null)
        return 0;
      Camera.spriteBatch.Begin();
      Camera.spriteBatch.Draw(this.texture, (Microsoft.Xna.Framework.Vector2) this.Position, new Rectangle?(this.bounds), (Microsoft.Xna.Framework.Color) this.material.color, this.transform.eulerAngles.y, (Microsoft.Xna.Framework.Vector2) this.Origin, this.Scale, this.Effects, this.LayerDepth);
      Camera.spriteBatch.End();
      return 1;
    }
  }
}
