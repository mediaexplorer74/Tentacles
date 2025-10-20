// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LanguageItem
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LanguageItem
  {
    public string id;
    public string content;
    public int linenumber;

    protected LanguageItem()
    {
    }

    public LanguageItem(string id, string content, int linenumber)
    {
      this.id = id;
      this.content = content;
      this.linenumber = linenumber;
    }

    public override string ToString() => string.Format("[LanguageItem] id: {0}", (object) this.id);
  }
}
