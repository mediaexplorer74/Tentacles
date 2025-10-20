// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UIProgressBar
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.UI;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class UIProgressBar : Control
  {
    private ImageControl background;
    private ImageControl foreground;
    private Rectangle source;
    private Rectangle mask;
    private float _progress;

    public float progress => this._progress;

    public UIProgressBar(ImageControl background, ImageControl foreground)
    {
      this.background = background;
      this.foreground = foreground;
      this.AddChild((Control) background);
      this.AddChild((Control) foreground);
      this.source = ((UISpriteRenderer) foreground.gameObject.renderer).sourceRect;
      this.mask = this.source;
    }

    public void SetProgress(float value)
    {
      this._progress = Mathf.Clamp01(value);
      this.mask.Width = (int) ((double) this.source.Width * (double) this.progress);
      ((UISpriteRenderer) this.foreground.gameObject.renderer).sourceRect = this.mask;
      this.foreground.size = new PressPlay.FFWD.Vector2((float) this.source.Width * this.progress, (float) this.mask.Height);
    }

    public override void Update() => base.Update();
  }
}
