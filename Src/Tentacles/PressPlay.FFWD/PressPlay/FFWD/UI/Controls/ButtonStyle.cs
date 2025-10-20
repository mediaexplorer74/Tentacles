// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ButtonStyle
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ButtonStyle
  {
    public Texture2D texture;
    private Dictionary<int, Rectangle> sources;

    public Rectangle this[int state]
    {
      get => this.sources[state];
      set => this.sources[state] = value;
    }

    public ButtonStyle(Texture2D texture)
      : this(texture, texture.Bounds, texture.Bounds, texture.Bounds, texture.Bounds)
    {
    }

    public ButtonStyle(Texture2D texture, Rectangle normal)
      : this(texture, normal, normal, normal, normal)
    {
    }

    public ButtonStyle(Texture2D texture, Rectangle normal, Rectangle pressed)
      : this(texture, normal, pressed, normal, normal)
    {
    }

    public ButtonStyle(Texture2D texture, Rectangle normal, Rectangle pressed, Rectangle hover)
      : this(texture, normal, pressed, hover, normal)
    {
    }

    public ButtonStyle(
      Texture2D texture,
      Rectangle normal,
      Rectangle pressed,
      Rectangle hover,
      Rectangle disabled)
    {
      this.texture = texture;
      this.sources = new Dictionary<int, Rectangle>();
      this.sources.Add(1, normal);
      this.sources.Add(2, pressed);
      this.sources.Add(0, hover);
      this.sources.Add(3, disabled);
    }

    public Rectangle GetSourceRect(int state)
    {
      return this.sources[state] != Rectangle.Empty ? this.sources[state] : this.sources[1];
    }
  }
}
