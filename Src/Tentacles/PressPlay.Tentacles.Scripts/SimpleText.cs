// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SimpleText
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SimpleText : SimpleRenderObject
  {
    public string text = "";
    public SpriteFont font;
    private Vector2 center;

    public SimpleText(string text)
      : this(text, Vector2.Zero)
    {
    }

    public SimpleText(string text, Vector2 position)
    {
      this.text = text;
      this.position = position;
      this.font = GUIAssets.berlinsSans40;
      this.center = this.font.MeasureString(text) / 2f;
    }

    public override void Draw(SpriteBatch batch)
    {
      this.color.a = this.alpha;
      batch.DrawString(this.font, this.text, this.position, (Color) this.color, this.rotation, this.center, 1f, this.effects, 1f);
    }
  }
}
