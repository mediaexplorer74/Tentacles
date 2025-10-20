// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AchievementsHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AchievementsHandler
  {
    protected Dictionary<int, AchievementDescription> enumToAchievement = new Dictionary<int, AchievementDescription>();
    protected Dictionary<string, AchievementDescription> keyToAchievement = new Dictionary<string, AchievementDescription>();
    private bool hasReceivedAchievements;
    private static AchievementsHandler _instance;

    public static AchievementsHandler Instance
    {
      get
      {
        if (AchievementsHandler._instance == null)
          AchievementsHandler._instance = new AchievementsHandler();
        return AchievementsHandler._instance;
      }
    }

    public AchievementsHandler()
    {
      this.CreateAchievements();
      SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
      signedInGamer?.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), (object) signedInGamer);
    }

    protected void GetAchievementsCallback(IAsyncResult result)
    {
      if (!(result.AsyncState is SignedInGamer asyncState))
        return;
      try
      {
        AchievementCollection achievements = asyncState.EndGetAchievements(result);
        for (int index = 0; index < achievements.Count; ++index)
        {
          if (achievements[index].IsEarned)
            this.keyToAchievement[achievements[index].Key].SetUnlocked();
        }
      }
      catch (Exception ex)
      {
      }
      this.hasReceivedAchievements = true;
      this.ConsolidateAchievements();
    }

    public void ConsolidateAchievements()
    {
      for (int index = 0; index < GlobalManager.Instance.currentProfile.unlockedAchievements.achievementKeys.Count; ++index)
        this.keyToAchievement[GlobalManager.Instance.currentProfile.unlockedAchievements.achievementKeys[index]].Unlock();
    }

    private void CreateAchievements()
    {
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COLLECT_ALL_PICKUPS, "COLLECT_ALL_PICKUPS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_LEVEL_NO_DAMAGE, "COMPLETE_LEVEL_NO_DAMAGE");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_TIME_CHALLENGE, "COMPLETE_TIME_CHALLENGE");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_ALL_CHALLENGES, "COMPLETE_ALL_CHALLENGES");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_PENETRATOR_LEVEL, "COMPLETE_PENETRATOR_LEVEL");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_GAME, "COMPLETE_GAME");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLERE_PICKUP_CHALLENGE, "COMPLETE_PICKUP_CHALLENGE");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_DAMAGE_CHALLENGE, "COMPLETE_DAMAGE_CHALLENGE");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_FIRST_GOBBLER_BOSS, "DEFEAT_FIRST_GOBBLER_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_SECOND_GOBBLER_BOSS, "DEFEAT_SECOND_GOBBLER_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_FIRST_SHOOTER_BOSS, "DEFEAT_FIRST_SHOOTER_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_SECOND_SHOOTER_BOSS, "DEFEAT_SECOND_SHOOTER_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_FIRST_WORM_BOSS, "DEFEAT_FIRST_WORM_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.DEFEAT_SECOND_WORM_BOSS, "DEFEAT_SECOND_WORM_BOSS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.FIRST_KILL, "FIRST_KILL");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.GET_ALL_STARS, "GET_ALL_STARS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.KILL_GOBBLER, "KILL_GOBBLER");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.COMPLETE_ALL_LEVELS_WITHOUT_DYING, "COMPLETE_ALL_LEVELS_WITHOUT_DYING");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.MAKE_5_CONSECUTIVE_KILLS, "MAKE_5_CONSECUTIVE_KILLS");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.MAKE_A_PERFECT_RUN, "MAKE_A_PERFECT_RUN");
      this.AddAchievementToDictionary(AchievementsHandler.Achievements.GET_NO_PICKUPS, "GET_NO_PICKUPS");
    }

    private void AddAchievementToDictionary(
      AchievementsHandler.Achievements _achievementEnum,
      string _key)
    {
      AchievementDescription achievementDescription = new AchievementDescription(_achievementEnum, _key);
      this.enumToAchievement.Add((int) _achievementEnum, achievementDescription);
      this.keyToAchievement.Add(_key, achievementDescription);
    }

    public void LoadAchievements()
    {
    }

    public void UnlockAchievement(AchievementsHandler.Achievements _achievement)
    {
      this.GetAchievementDescription(_achievement).Unlock();
    }

    public AchievementDescription GetAchievementDescription(
      AchievementsHandler.Achievements _achievement)
    {
      return this.enumToAchievement[(int) _achievement];
    }

    public void EvaluateUserProfileForAchievements(UserProfile _userProfile)
    {
      if (_userProfile.unlockedLevels.Count == 1)
        return;
      if (_userProfile.GetWorldResult(1).isCompleted)
        this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_GAME);
      if (_userProfile.GetWorldResult(1).HasAllPickups())
        this.UnlockAchievement(AchievementsHandler.Achievements.COLLECT_ALL_PICKUPS);
      if (_userProfile.GetWorldResult(1).CompletedAllChallenges())
        this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_ALL_CHALLENGES);
      if (_userProfile.GetWorldResult(1).HasAllStars())
        this.UnlockAchievement(AchievementsHandler.Achievements.GET_ALL_STARS);
      if (!_userProfile.GetWorldResult(1).HasAllNoDeathStars())
        return;
      this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_ALL_LEVELS_WITHOUT_DYING);
    }

    public void EvaluateCompletedChallengeForAchievements(Challenge.ChallengeType _challengeType)
    {
      switch (_challengeType)
      {
        case Challenge.ChallengeType.raceChallenge:
          this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_TIME_CHALLENGE);
          break;
        case Challenge.ChallengeType.noDamageChallenge:
          this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_DAMAGE_CHALLENGE);
          break;
        case Challenge.ChallengeType.getAllPickups:
          this.UnlockAchievement(AchievementsHandler.Achievements.COMPLERE_PICKUP_CHALLENGE);
          break;
      }
    }

    public void EvaluateCompletedLevelSessionForAchievements(LevelSession _session)
    {
      if (_session.numberOfPickupsCollected == 0)
        this.UnlockAchievement(AchievementsHandler.Achievements.GET_NO_PICKUPS);
      if ((double) _session.damageTotal == 0.0)
        this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_LEVEL_NO_DAMAGE);
      if ((double) _session.damageTotal == 0.0 && _session.EvaluateAllPickupsMedal() && _session.EvaluateCompletedChallenge() && _session.EvaluateCompletedWithoutDying())
        this.UnlockAchievement(AchievementsHandler.Achievements.MAKE_A_PERFECT_RUN);
      if (_session.level.id != 27)
        return;
      this.UnlockAchievement(AchievementsHandler.Achievements.COMPLETE_PENETRATOR_LEVEL);
    }

    public void EvaluateKillForAchievements(BaseCreature _creature)
    {
      if (_creature.GetType() == typeof (Gobbler))
        AchievementsHandler.Instance.UnlockAchievement(AchievementsHandler.Achievements.KILL_GOBBLER);
      AchievementsHandler.Instance.UnlockAchievement(AchievementsHandler.Achievements.FIRST_KILL);
    }

    public enum Achievements
    {
      COLLECT_ALL_PICKUPS,
      COMPLETE_LEVEL_NO_DAMAGE,
      COMPLETE_TIME_CHALLENGE,
      COMPLETE_ALL_CHALLENGES,
      COMPLETE_PENETRATOR_LEVEL,
      COMPLETE_GAME,
      COMPLERE_PICKUP_CHALLENGE,
      COMPLETE_DAMAGE_CHALLENGE,
      DEFEAT_FIRST_GOBBLER_BOSS,
      DEFEAT_SECOND_GOBBLER_BOSS,
      DEFEAT_FIRST_SHOOTER_BOSS,
      DEFEAT_SECOND_SHOOTER_BOSS,
      DEFEAT_FIRST_WORM_BOSS,
      DEFEAT_SECOND_WORM_BOSS,
      FIRST_KILL,
      GET_ALL_STARS,
      KILL_GOBBLER,
      COMPLETE_ALL_LEVELS_WITHOUT_DYING,
      MAKE_5_CONSECUTIVE_KILLS,
      MAKE_A_PERFECT_RUN,
      GET_NO_PICKUPS,
    }
  }
}
