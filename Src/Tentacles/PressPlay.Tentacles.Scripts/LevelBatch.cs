// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelBatch
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelBatch
  {
    public List<Level> levels = new List<Level>();

    public int GetTotalScore()
    {
      int totalScore = 0;
      foreach (Level level in this.levels)
      {
        LevelResult levelResult = GlobalManager.Instance.currentProfile.GetLevelResult(level);
        if (levelResult != null)
          totalScore += levelResult.score;
      }
      return totalScore;
    }
  }
}
