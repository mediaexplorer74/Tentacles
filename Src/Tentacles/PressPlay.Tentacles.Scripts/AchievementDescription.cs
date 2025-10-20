// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AchievementDescription
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using PressPlay.FFWD;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AchievementDescription
  {
    public AchievementsHandler.Achievements achievement;
    private bool _isUnlocked;
    private string key = "";
    private string nameKey;
    private string descriptionKey;

    public string name => LocalisationManager.Instance.GetString(this.nameKey);

    public string description => LocalisationManager.Instance.GetString(this.descriptionKey);

    public bool isUnlocked => this._isUnlocked;

    public AchievementDescription(AchievementsHandler.Achievements achievement, string key)
    {
      this.achievement = achievement;
      this.key = key;
      this.nameKey = key;
    }

    protected void AwardAchievementCallback(IAsyncResult result)
    {
      if (!(result.AsyncState is SignedInGamer asyncState))
        return;
      asyncState.EndAwardAchievement(result);
    }

    public void SetUnlocked()
    {
      this._isUnlocked = true;
      GlobalManager.Instance.currentProfile.unlockedAchievements.AddKey(this.key);
    }

    public void Unlock()
    {
      if (this.isUnlocked || GlobalManager.isTrialMode)
        return;
      this._isUnlocked = true;
      GlobalManager.Instance.currentProfile.unlockedAchievements.AddKey(this.key);
      SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
      if (signedInGamer == null)
        return;
      try
      {
        signedInGamer.BeginAwardAchievement(this.key, new AsyncCallback(this.AwardAchievementCallback), (object) signedInGamer);
      }
      catch (Exception ex)
      {
        Debug.LogError("Could not award achievement " + this.key + ". " + ex.Message);
        this._isUnlocked = false;
      }
    }
  }
}
