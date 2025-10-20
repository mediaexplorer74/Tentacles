// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LocalisationManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LocalisationManager : MonoBehaviour
  {
    private static LocalisationManager instance;
    public static bool isLoaded = false;
    private int currentLanguage;
    public Language[] languages;
    public string noTextString = "///No text///";
    public bool hasChanged;

    public string currentLanguageCode => this.languages[this.currentLanguage].id;

    public static LocalisationManager Instance
    {
      get
      {
        if (LocalisationManager.instance == null)
          Debug.LogError("Attempt to access instance of GlobalManager singleton earlier than Start or without it being attached to a GameObject.");
        return LocalisationManager.instance;
      }
    }

    public override void Awake()
    {
      if (LocalisationManager.instance != null)
      {
        Debug.LogError("Cannot have two instances of GlobalManager. Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        LocalisationManager.instance = this;
        LocalisationManager.isLoaded = true;
        UnityObject.DontDestroyOnLoad((UnityObject) this);
        foreach (Language language in this.languages)
          language.Start();
      }
    }

    public LanguageItem GetItem(string key) => this.languages[this.currentLanguage].GetItem(key);

    public Language GetLanguage(string id)
    {
      foreach (Language language in this.languages)
      {
        if (language.id == id)
          return language;
      }
      return (Language) null;
    }

    public bool SetLanguage(string lang)
    {
      int num = 0;
      foreach (Language language in this.languages)
      {
        if (language.id == lang)
        {
          this.currentLanguage = num;
          return true;
        }
        ++num;
      }
      Debug.LogError("LocalisationManager::The language " + lang + " could not be found");
      return false;
    }

    public void AddLanguage(Language language)
    {
      if (this.GetLanguage(language.id) == null)
        this.AppendLanguage(language);
      else
        this.ReplaceLanguage(language);
    }

    private void AppendLanguage(Language language)
    {
      List<Language> languageList = new List<Language>();
      foreach (Language language1 in this.languages)
        languageList.Add(language1);
      languageList.Add(language);
      this.languages = languageList.ToArray();
    }

    private void ReplaceLanguage(Language language)
    {
      for (int index = 0; index < this.languages.Length; ++index)
      {
        if (this.languages[index].id == language.id)
        {
          this.languages[index] = language;
          break;
        }
      }
    }

    public string GetString(string key)
    {
      LanguageItem languageItem = this.GetItem(key);
      return languageItem == null ? LocalisationManager.Instance.noTextString + " - key: " + key : languageItem.content;
    }
  }
}
