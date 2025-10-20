// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Challenge
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Challenge
  {
    public Challenge.ChallengeType type;
    public string enemies;
    public int numberOfPickups;
    public float duration;
    public int bonus;

    public bool areAllEnemiesKilled => true;

    public string startText
    {
      get
      {
        switch (this.type)
        {
          case Challenge.ChallengeType.raceChallenge:
            return "chal_rac_01_start";
          case Challenge.ChallengeType.noDamageChallenge:
            return "chal_dam_01_start";
          default:
            return "wrong challenge type";
        }
      }
    }

    public string successText
    {
      get
      {
        switch (this.type)
        {
          case Challenge.ChallengeType.raceChallenge:
            return "chal_rac_01_ok";
          case Challenge.ChallengeType.noDamageChallenge:
            return "chal_dam_01_ok";
          default:
            return "wrong challenge type";
        }
      }
    }

    public string failText
    {
      get
      {
        switch (this.type)
        {
          case Challenge.ChallengeType.raceChallenge:
            return "chal_rac_01_fail";
          case Challenge.ChallengeType.noDamageChallenge:
            return "chal_dam_01_fail";
          default:
            return "wrong challenge type";
        }
      }
    }

    public enum ChallengeType
    {
      raceChallenge,
      pickupComboChallenge,
      noDamageChallenge,
      getAllPickups,
      killAllEnemies,
    }
  }
}
