// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.NotImplementedScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class NotImplementedScreen(string background) : BackgroundScreen(background)
  {
    public override void LoadContent() => base.LoadContent();

    private void OnButtonPress(object sender, ButtonControlEventArgs e) => this.ExitScreen();

    public override void HandleInput(InputState input) => base.HandleInput(input);

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
