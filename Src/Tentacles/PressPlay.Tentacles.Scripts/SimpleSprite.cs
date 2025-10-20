// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SimpleSprite
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SimpleSprite : SimpleRenderObject
  {
    public Texture2D gfx;
    public bool drawFromCenter;
    public Rectangle sourceRect = Rectangle.Empty;
    public Rectangle mask = Rectangle.Empty;

    public SimpleSprite(Texture2D gfx)
      : this(gfx, Vector2.Zero, new Rectangle(0, 0, gfx.Width, gfx.Height))
    {
    }

    public SimpleSprite(Texture2D gfx, Vector2 position)
      : this(gfx, position, new Rectangle(0, 0, gfx.Width, gfx.Height))
    {
    }

    public SimpleSprite(Texture2D gfx, Vector2 position, Rectangle sourceRect)
    {
      this.gfx = gfx;
      this.position = position;
      this.sourceRect = sourceRect;
      this.mask = sourceRect;
    }

    public SimpleSprite(Texture2D gfx, Rectangle sourceRect)
      : this(gfx, Vector2.Zero, sourceRect)
    {
    }

    public override void Draw(SpriteBatch batch)
    {
      this.color.a = this.alpha;
      if (this.gfx == null)
        return;
      Vector2 position = this.position;
      if (this.drawFromCenter)
      {
        position.X = this.position.X - (float) (this.sourceRect.Width / 2);
        position.Y = this.position.Y - (float) (this.sourceRect.Height / 2);
      }
      batch.Draw(this.gfx, position, new Rectangle?(this.mask), (Color) this.color, this.rotation, Vector2.Zero, 1f, this.effects, 0.0f);
    }
  }
}
