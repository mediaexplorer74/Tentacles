// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.MenuEntry
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class MenuEntry
  {
    private string text;
    private float selectionFade;
    private PressPlay.FFWD.Vector2 position;

    public string Text
    {
      get => this.text;
      set => this.text = value;
    }

    public PressPlay.FFWD.Vector2 Position
    {
      get => this.position;
      set => this.position = value;
    }

    public event EventHandler<PlayerIndexEventArgs> Selected;

    protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
    {
      if (this.Selected == null)
        return;
      this.Selected((object) this, new PlayerIndexEventArgs(playerIndex));
    }

    public MenuEntry(string text) => this.text = text;

    public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
      isSelected = false;
      float num = (float) gameTime.ElapsedGameTime.TotalSeconds * 4f;
      if (isSelected)
        this.selectionFade = Math.Min(this.selectionFade + num, 1f);
      else
        this.selectionFade = Math.Max(this.selectionFade - num, 0.0f);
    }

    public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
    {
      isSelected = false;
      PressPlay.FFWD.Color color1 = isSelected ? PressPlay.FFWD.Color.yellow : PressPlay.FFWD.Color.white;
      float scale = (float) (1.0 + (double) ((float) Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6.0) + 1f) * 0.05000000074505806 * (double) this.selectionFade);
      PressPlay.FFWD.Color color2 = color1 * screen.TransitionAlpha;
      PressPlay.FFWD.ScreenManager.ScreenManager screenManager = screen.ScreenManager;
      SpriteBatch spriteBatch = screenManager.SpriteBatch;
      SpriteFont font = screenManager.Font;
      PressPlay.FFWD.Vector2 origin = new PressPlay.FFWD.Vector2(0.0f, (float) (font.LineSpacing / 2));
      spriteBatch.DrawString(font, this.text, (Microsoft.Xna.Framework.Vector2) this.position, (Microsoft.Xna.Framework.Color) color2, 0.0f, (Microsoft.Xna.Framework.Vector2) origin, scale, SpriteEffects.None, 0.0f);
    }

    public virtual int GetHeight(MenuScreen screen) => screen.ScreenManager.Font.LineSpacing;

    public virtual int GetWidth(MenuScreen screen)
    {
      return (int) screen.ScreenManager.Font.MeasureString(this.Text).X;
    }
  }
}
