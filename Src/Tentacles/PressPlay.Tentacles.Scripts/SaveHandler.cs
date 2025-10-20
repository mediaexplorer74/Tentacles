// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SaveHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SaveHandler
  {
    private static SaveHandler _instance;
    private string saveFileName = "savegame";
    private UserProfile userProfile;
    private SaveHandler.OnLoadComplete onLoadComplete;
    private SaveHandler.OnSaveComplete onSaveComplete;

    private SaveHandler()
    {
    }

    public static SaveHandler Instance
    {
      get
      {
        if (SaveHandler._instance == null)
          SaveHandler._instance = new SaveHandler();
        return SaveHandler._instance;
      }
    }

    public void LoadGame(UserProfile _userProfile, SaveHandler.OnLoadComplete _onLoadComplete)
    {
      this.userProfile = _userProfile;
      this.onLoadComplete = _onLoadComplete;
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      try
      {
        using (BinaryReader reader = new BinaryReader((Stream) storeForApplication.OpenFile(this.saveFileName, FileMode.Open)))
        {
          _userProfile.ReadBinary(reader);
          this.onLoadComplete();
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.Message);
        throw;
      }
    }

    public void SaveGame(UserProfile _userProfile, SaveHandler.OnSaveComplete _onSaveComplete)
    {
      this.userProfile = _userProfile;
      this.onSaveComplete = _onSaveComplete;
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      try
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) storeForApplication.OpenFile(this.saveFileName, FileMode.Create)))
        {
          _userProfile.WriteBinary(writer);
          this.onSaveComplete();
        }
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.Message);
      }
    }

    public delegate void OnLoadComplete();

    public delegate void OnSaveComplete();
  }
}
