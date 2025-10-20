// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Language
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Language
  {
    public string id;
    public LanguageItem[] items;
    private Dictionary<string, LanguageItem> entries;

    protected Language()
    {
    }

    public Language(string id)
    {
      this.id = id;
      this.entries = new Dictionary<string, LanguageItem>();
    }

    public void Start()
    {
      this.entries = new Dictionary<string, LanguageItem>();
      foreach (LanguageItem languageItem in this.items)
        this.AddItem(languageItem);
    }

    public void AddItem(LanguageItem item)
    {
      if (this.entries.ContainsKey(item.id))
        Debug.LogWarning(this.id + ": Language already contains " + item.id + " (" + (object) this.entries[item.id] + ")");
      else
        this.entries.Add(item.id, item);
    }

    public LanguageItem GetItem(string itemId)
    {
      return this.entries.ContainsKey(itemId) ? this.entries[itemId] : (LanguageItem) null;
    }

    public void FinishImporting()
    {
      List<LanguageItem> languageItemList = new List<LanguageItem>();
      foreach (KeyValuePair<string, LanguageItem> entry in this.entries)
        languageItemList.Add(entry.Value);
      this.items = new LanguageItem[languageItemList.Count];
      for (int index = 0; index < languageItemList.Count; ++index)
        this.items[index] = languageItemList[index];
    }
  }
}
