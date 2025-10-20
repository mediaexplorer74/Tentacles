// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelSession
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelSession : MonoBehaviour
  {
    public AudioWrapper sndBankPoints;
    public float time;
    public Level level;
    private int _score;
    private bool isChallengeCompleted;
    private Challenge.ChallengeType completedChallenge;
    private float damageSinceLastCheckpoint;
    public float damageTotal;
    public int numberOfKills;
    public int numberOfDeaths;
    private float lastKillTime;
    public int totalNumberOfPickups;
    public int numberOfPickupsCollected;
    private float lastPickupTime;
    private int pickupCounter;
    private int pickupScore;
    public int subtractScorePerDamage = 5;
    public int subtractScoreOnDeath = 500;
    public int maxPickupCombo;
    private int tempScore;
    private PoolableText tempFeedback;
    private bool isStarted;
    private bool isPaused;
    private float pauseTime;
    private float pauseDelta;
    private float lastEyeRipTime = -1f;
    private int consecutiveRips;

    public int score
    {
      get => this._score;
      set => this._score = value;
    }

    public void Initialize(Level _level) => this.level = _level;

    public override void Start() => base.Start();

    public void Pause(bool pause)
    {
      if (!pause)
        this.pauseDelta = Time.time - this.pauseTime;
      this.isPaused = pause;
      this.pauseTime = Time.time;
    }

    public void StartTime()
    {
      this.isStarted = true;
      this.time = 0.0f;
    }

    public void StopTime() => this.isStarted = false;

    public void RegisterEyeRip(BasicEnemyHitLump lump)
    {
      if ((double) this.lastEyeRipTime != -1.0 && (double) Time.time < (double) this.lastEyeRipTime + 1.0)
        ++this.consecutiveRips;
      else
        this.consecutiveRips = 0;
      if (this.consecutiveRips >= 5)
        AchievementsHandler.Instance.UnlockAchievement(AchievementsHandler.Achievements.MAKE_5_CONSECUTIVE_KILLS);
      this.lastEyeRipTime = Time.time;
    }

    public void RegisterKill(int point, Vector3 position, BaseCreature _creature)
    {
      this.lastKillTime = Time.time;
      this.AddPointToScore(point, position, true, true);
      AchievementsHandler.Instance.EvaluateKillForAchievements(_creature);
      LevelHandler.Instance.challengeHandler.RegisterKill();
    }

    public void RegisterAttack(int point, float multiplierFactor, Vector3 position)
    {
      this.AddPointToScore(point, position, true, true);
    }

    public void RegisterPickup(int point, PointPickup.PickupType type, Vector3 position)
    {
      this.lastPickupTime = Time.time;
      point = GlobalManager.Instance.gameplaySettings.GetPickupScore(this.pickupCounter);
      this.pickupScore += this.AddPointToScore(point, position, GlobalManager.Instance.gameplaySettings.multiplyPointsInPickups, false);
      if (type == PointPickup.PickupType.collectable)
        ++this.numberOfPickupsCollected;
      ++this.pickupCounter;
      LevelHandler.Instance.challengeHandler.RegisterPickup();
      if (GlobalManager.Instance.gameplaySettings.pickupFeedbackMode != GameplaySettings.PickupFeedbackType.instantAndCollectiveFeedback && GlobalManager.Instance.gameplaySettings.pickupFeedbackMode != GameplaySettings.PickupFeedbackType.instantFeedback)
        return;
      this.tempFeedback = (PoolableText) ObjectPool.Instance.Draw(LevelHandler.Instance.library.pointFeedback, position, LevelHandler.Instance.GetCameraRelativeRotation(Vector3.up));
      this.tempFeedback.SetText(this.tempScore.ToString());
      this.tempFeedback.SetColor(GlobalManager.Instance.gameplaySettings.GetPickupColor(this.pickupCounter));
      this.tempFeedback.SetTiming(0.4f, GlobalManager.Instance.gameplaySettings.groupPickupTime - 0.6f, 0.2f);
    }

    public void RegisterPoint(
      int point,
      float multiplierFactor,
      Vector3 position,
      bool doMultiply)
    {
      this.AddPointToScore(point, position, doMultiply, true);
    }

    public void RegisterDamage(float damage)
    {
      this.AddPointToScore((int) (-(double) damage * (double) this.subtractScorePerDamage));
      this.damageSinceLastCheckpoint += damage;
      this.damageTotal += damage;
      LevelHandler.Instance.challengeHandler.RegisterDamage(damage);
    }

    public int AddPointToScore(int point, Vector3 position, bool multiply, bool showFeedback)
    {
      this.tempScore = point;
      this._score += this.tempScore;
      if (this._score < 0)
        this._score = 0;
      LevelHandler.Instance.ingameGUI.ShowScore(this._score);
      if (showFeedback && point > 0)
      {
        this.tempFeedback = (PoolableText) ObjectPool.Instance.Draw(LevelHandler.Instance.library.pointFeedback, position, LevelHandler.Instance.GetCameraRelativeRotation(Vector3.up));
        this.tempFeedback.SetText(this.tempScore.ToString());
        this.tempFeedback.renderer.material.color = Color.green;
        this.tempFeedback.SetTiming(0.4f, 1.5f, 0.2f);
      }
      return this.tempScore;
    }

    public int AddPointToScore(int point)
    {
      return this.AddPointToScore(point, Vector3.zero, false, false);
    }

    private void BankPickupScore()
    {
      this.maxPickupCombo = Mathf.Max(this.maxPickupCombo, this.pickupCounter);
      LevelHandler.Instance.challengeHandler.RegisterPickupCombo(this.pickupCounter);
      this.pickupCounter = 0;
      this.pickupScore = 0;
    }

    public void ResetScore() => this._score = 0;

    public void RegisterDeath()
    {
      this.AddPointToScore(-this.subtractScoreOnDeath);
      ++this.numberOfDeaths;
      this.damageSinceLastCheckpoint = 0.0f;
      this.lastKillTime = 0.0f;
      this.lastPickupTime = 0.0f;
      this.pickupCounter = 0;
      this.pickupScore = 0;
    }

    public void RegisterCompletedChallenge(Challenge.ChallengeType _type)
    {
      this.isChallengeCompleted = true;
      this.completedChallenge = _type;
      AchievementsHandler.Instance.EvaluateCompletedChallengeForAchievements(_type);
    }

    public override void Update()
    {
      if (this.isPaused)
        return;
      if (this.pickupCounter > 0 && (double) Time.time - (double) this.pauseDelta - (double) this.lastPickupTime > (double) GlobalManager.Instance.gameplaySettings.groupPickupTime)
        this.BankPickupScore();
      if (!this.isStarted)
        return;
      this.time += Time.deltaTime;
    }

    public bool EvaluateAllPickupsMedal()
    {
      return this.numberOfPickupsCollected == this.totalNumberOfPickups;
    }

    public bool EvaluateCompletedWithoutDying() => this.numberOfDeaths == 0;

    public bool EvaluateCompletedChallenge() => this.isChallengeCompleted;

    public void SaveScore()
    {
      if (LevelHandler.Instance.challengeHandler.bonusPointsPending != -1)
        LevelHandler.Instance.challengeHandler.AwardSuccededChallengeBonus();
      GlobalManager.Instance.currentProfile.globalData.playButtonOpensThisLevel = !GlobalManager.Instance.database.IsLastLevelInWorld(this.level) ? this.level.nextLevel : -1;
      GlobalManager.Instance.currentProfile.UnlockLevelAfterThis(LevelHandler.Instance.currentLevel);
      GlobalManager.Instance.currentProfile.AddCompletedLevelSession(this);
      AchievementsHandler.Instance.EvaluateUserProfileForAchievements(GlobalManager.Instance.currentProfile);
      GlobalManager.Instance.SaveToLeaderboard(LevelHandler.Instance.currentLevel);
    }
  }
}
