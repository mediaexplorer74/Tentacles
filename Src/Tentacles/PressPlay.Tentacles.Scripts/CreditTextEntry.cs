// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CreditTextEntry
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CreditTextEntry : Control
  {
    public TextControl title;
    public PanelControl content;

    public CreditTextEntry(string titleText, SpriteFont font)
    {
      this.title = new TextControl(titleText, font);
      this.title.gameObject.transform.localScale *= 0.7f;
      this.AddChild((Control) this.title);
      this.content = new PanelControl();
      if (titleText.Contains("\n"))
        this.content.transform.localPosition = new Vector3(0.0f, 0.0f, 37f);
      else
        this.content.transform.localPosition = new Vector3(0.0f, 0.0f, 19f);
      this.AddChild((Control) this.content);
    }

    public void AddTextEntry(string nameText, SpriteFont font)
    {
      TextControl child = new TextControl(nameText, font);
      child.transform.localScale *= 0.5f;
      this.content.AddChild((Control) child);
      this.content.LayoutColumn();
    }
  }
}
