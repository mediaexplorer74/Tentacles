// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ImageControl
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ImageControl : Control
  {
    public Texture2D texture
    {
      get => this.renderer.material.texture;
      set
      {
        if (this.renderer.material.texture == value)
          return;
        this.renderer.material.texture = value;
        this.InvalidateAutoSize();
      }
    }

    public ImageControl(Texture2D texture)
    {
      this.gameObject.AddComponent<UISpriteRenderer>(new UISpriteRenderer());
      this.gameObject.name = nameof (ImageControl);
      this.texture = texture;
    }

    public ImageControl(Texture2D texture, Rectangle sourceRect)
      : this(texture)
    {
      ((UISpriteRenderer) this.gameObject.renderer).sourceRect = sourceRect;
    }

    public override PressPlay.FFWD.Vector2 ComputeSize()
    {
      UISpriteRenderer renderer = (UISpriteRenderer) this.gameObject.renderer;
      if (renderer.texture == null)
        return PressPlay.FFWD.Vector2.zero;
      return renderer.sourceRect != Rectangle.Empty ? new PressPlay.FFWD.Vector2((float) renderer.sourceRect.Width, (float) renderer.sourceRect.Height) : new PressPlay.FFWD.Vector2((float) this.texture.Width, (float) this.texture.Height);
    }
  }
}
