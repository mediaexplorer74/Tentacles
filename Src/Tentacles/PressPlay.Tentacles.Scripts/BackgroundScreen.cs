// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BackgroundScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BackgroundScreen : MenuSceneScreen
  {
    private string backgroundSrc;
    protected ImageControl background;

    public BackgroundScreen(string backgroundSrc)
      : base("")
    {
      this.backgroundSrc = backgroundSrc;
    }

    public override void LoadContent()
    {
      if (this.backgroundSrc == null && !(this.backgroundSrc != ""))
        return;
      this.background = new ImageControl(Application.Load<Texture2D>(this.backgroundSrc));
      this.controls.Add((Control) this.background);
    }

    public override void UnloadContent() => base.UnloadContent();

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
