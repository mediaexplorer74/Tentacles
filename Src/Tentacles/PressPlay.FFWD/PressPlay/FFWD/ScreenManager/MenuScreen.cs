// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.MenuScreen
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public abstract class MenuScreen : GameScreen
  {
    private const int menuEntryPadding = 10;
    private List<MenuEntry> menuEntries = new List<MenuEntry>();
    private int selectedEntry;
    private string menuTitle;

    protected IList<MenuEntry> MenuEntries => (IList<MenuEntry>) this.menuEntries;

    public MenuScreen(string menuTitle)
    {
      this.EnabledGestures = GestureType.Tap;
      this.menuTitle = menuTitle;
      this.TransitionOnTime = TimeSpan.FromSeconds(0.5);
      this.TransitionOffTime = TimeSpan.FromSeconds(0.5);
    }

    protected virtual Rectangle GetMenuEntryHitBounds(MenuEntry entry)
    {
      return new Rectangle(0, (int) entry.Position.y - 10, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width, entry.GetHeight(this) + 20);
    }

    public override void HandleInput(InputState input)
    {
      PlayerIndex playerIndex;
      if (input.IsMenuCancel(this.ControllingPlayer, out playerIndex))
        this.OnCancel(playerIndex);
      foreach (GestureSample gesture in input.Gestures)
      {
        if (gesture.GestureType == GestureType.Tap)
        {
          Point point = new Point((int) gesture.Position.X, (int) gesture.Position.Y);
          for (int index = 0; index < this.menuEntries.Count; ++index)
          {
            if (this.GetMenuEntryHitBounds(this.menuEntries[index]).Contains(point))
              this.OnSelectEntry(index, PlayerIndex.One);
          }
        }
      }
    }

    protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
    {
      this.menuEntries[entryIndex].OnSelectEntry(playerIndex);
    }

    protected virtual void OnCancel(PlayerIndex playerIndex) => this.ExitScreen();

    protected void OnCancel(object sender, PlayerIndexEventArgs e) => this.OnCancel(e.PlayerIndex);

    protected virtual void UpdateMenuEntryLocations()
    {
      float num = (float) Math.Pow((double) this.TransitionPosition, 2.0);
      PressPlay.FFWD.Vector2 vector2 = new PressPlay.FFWD.Vector2(0.0f, 175f);
      for (int index = 0; index < this.menuEntries.Count; ++index)
      {
        MenuEntry menuEntry = this.menuEntries[index];
        vector2.x = (float) (PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2);
        if (this.ScreenState == ScreenState.TransitionOn)
          vector2.x -= num * 256f;
        else
          vector2.x += num * 512f;
        menuEntry.Position = vector2;
        vector2.y += (float) (menuEntry.GetHeight(this) + 20);
      }
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      for (int index = 0; index < this.menuEntries.Count; ++index)
      {
        bool isSelected = this.IsActive && index == this.selectedEntry;
        this.menuEntries[index].Update(this, isSelected, gameTime);
      }
    }

    public override void Draw(GameTime gameTime)
    {
      this.UpdateMenuEntryLocations();
      GraphicsDevice graphicsDevice = this.ScreenManager.GraphicsDevice;
      SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
      SpriteFont font = this.ScreenManager.Font;
      spriteBatch.Begin();
      for (int index = 0; index < this.menuEntries.Count; ++index)
        this.menuEntries[index].Draw(this, this.IsActive && index == this.selectedEntry, gameTime);
      float num = (float) Math.Pow((double) this.TransitionPosition, 2.0);
      PressPlay.FFWD.Vector2 position = new PressPlay.FFWD.Vector2((float) (PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2), 80f);
      PressPlay.FFWD.Vector2 origin = (PressPlay.FFWD.Vector2) (font.MeasureString(this.menuTitle) / 2f);
      PressPlay.FFWD.Color color = new PressPlay.FFWD.Color(192f, 192f, 192f) * this.TransitionAlpha;
      float scale = 1.25f;
      position.y -= num * 100f;
      spriteBatch.DrawString(font, this.menuTitle, (Microsoft.Xna.Framework.Vector2) position, (Microsoft.Xna.Framework.Color) color, 0.0f, (Microsoft.Xna.Framework.Vector2) origin, scale, SpriteEffects.None, 0.0f);
      spriteBatch.End();
    }
  }
}
