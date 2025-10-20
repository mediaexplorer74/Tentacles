// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TTBaseBehavior
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TTBaseBehavior : MonoBehaviour
  {
    protected string __(string key)
    {
      if (LocalisationManager.isLoaded)
      {
        LanguageItem languageItem = LocalisationManager.Instance.GetItem(key);
        return languageItem == null ? LocalisationManager.Instance.noTextString + " - key: " + key : languageItem.content;
      }
      Debug.Log((object) "LocalisationManager is not loaded or doesn't exist");
      return "";
    }
  }
}
