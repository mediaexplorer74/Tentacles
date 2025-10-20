// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.EndLevelStarControl
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class EndLevelStarControl : Control
  {
    public ImageControl background;
    public ImageControl star;
    public TextControl title;
    public TextControl footer;

    public EndLevelStarControl(
      ImageControl background,
      ImageControl star,
      TextControl title,
      TextControl footer)
    {
      this.background = background;
      this.AddChild((Control) background);
      this.star = star;
      this.star.gameObject.active = false;
      this.AddChild((Control) star);
      this.title = title;
      title.ignoreSize = true;
      title.transform.localScale *= 0.5f;
      this.AddChild((Control) title);
      this.footer = footer;
      footer.transform.localScale *= 0.6f;
      footer.ignoreSize = true;
      this.AddChild((Control) footer);
    }
  }
}
