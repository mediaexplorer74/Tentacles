// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GameplaySettings
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GameplaySettings
  {
    public GameplaySettings.PickupFeedbackType pickupFeedbackMode = GameplaySettings.PickupFeedbackType.instantAndCollectiveFeedback;
    public Color[] multiplierColors;
    public bool multiplyPointsInPickups;
    public float multiplyDamagePenalty = 0.2f;
    public float multiplierCoolOffTime = 1f;
    public float multiplierCoolOffValue = 0.1f;
    public float multiplierHeatUpValue = 1f;
    public float multiplierSlowdownThreshold = 10f;
    public float multiplierStopThreshold = 0.1f;
    public float multiplierUpdateSpeed = 1f;
    public Ease multiplierUpdateEase;
    public float multiplierNextLevelValue = 100f;
    public int multiplierMax = 5;
    public float groupPickupTime = 0.3f;
    public PickupScoreAndColor[] pickupScoreTable;
    public float timeToRespawn = 3f;
    public float levelIntroDuration = 5f;
    public float levelOutroDuration = 5f;
    public float finalizeEndScreenDuration = 0.5f;
    public int pointsForExtraLife;
    public int pointsForMultiplier = 1;
    public int defaultNumberOfLives = 3;
    public int numberOfLevelsToUnlockInTrialMode = 5;

    public Color GetMultiplierColor(int multiplier)
    {
      return multiplier - 1 > this.multiplierColors.Length - 1 ? Color.white : this.multiplierColors[multiplier - 1];
    }

    public int GetPickupScore(int combo)
    {
      return combo < this.pickupScoreTable.Length ? this.pickupScoreTable[combo].score : this.pickupScoreTable[this.pickupScoreTable.Length - 1].score;
    }

    public Color GetPickupColor(int combo)
    {
      return combo < this.pickupScoreTable.Length ? this.pickupScoreTable[combo].color : this.pickupScoreTable[this.pickupScoreTable.Length - 1].color;
    }

    public enum PickupFeedbackType
    {
      instantFeedback,
      collectiveFeedback,
      instantAndCollectiveFeedback,
    }
  }
}
