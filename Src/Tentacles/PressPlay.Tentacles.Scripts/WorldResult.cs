// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WorldResult
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WorldResult
  {
    public int id;
    private bool _isCompleted;
    public int levelResultsLength;
    public List<LevelResult> levelResults = new List<LevelResult>();

    public bool isCompleted => this._isCompleted;

    public int currentLevel => this.levelResults.Count;

    public int totalScore
    {
      get
      {
        int totalScore = 0;
        foreach (LevelResult levelResult in this.levelResults)
          totalScore += levelResult.score;
        return totalScore;
      }
    }

    public WorldResult(int worldId) => this.id = worldId;

    public void AddResult(LevelResult result)
    {
      foreach (LevelResult levelResult in this.levelResults)
      {
        if (levelResult.id == result.id)
        {
          levelResult.CopyData(result);
          return;
        }
      }
      this.levelResults.Add(result);
      bool flag = true;
      World world = GlobalManager.Instance.database.GetWorld(this.id);
      for (int index = 0; index < world.levels.Count; ++index)
      {
        if (!this.HasLevelResult(world.levels[index].id))
          flag = false;
      }
      if (!flag)
        return;
      this._isCompleted = true;
    }

    public LevelResult GetLevelResult(int levelId)
    {
      foreach (LevelResult levelResult in this.levelResults)
      {
        if (levelResult.id == levelId)
          return levelResult;
      }
      return (LevelResult) null;
    }

    public bool HasAllStars()
    {
      if (!this.isCompleted)
        return false;
      for (int index = 0; index < this.levelResults.Count; ++index)
      {
        if (!GlobalManager.Instance.database.IsTutorialLevel(this.levelResults[index].id) && !this.levelResults[index].HasAllStars())
          return false;
      }
      return true;
    }

    public bool HasAllNoDeathStars()
    {
      if (!this.isCompleted)
        return false;
      for (int index = 0; index < this.levelResults.Count; ++index)
      {
        if (!GlobalManager.Instance.database.IsTutorialLevel(this.levelResults[index].id) && !this.levelResults[index].data.noDeathsStar)
          return false;
      }
      return true;
    }

    public bool HasAllPickups()
    {
      if (!this.isCompleted)
        return false;
      for (int index = 0; index < this.levelResults.Count; ++index)
      {
        if (!GlobalManager.Instance.database.IsTutorialLevel(this.levelResults[index].id) && (this.levelResults[index].data.totalPickups == -1 || this.levelResults[index].data.totalPickups != this.levelResults[index].data.pickups))
          return false;
      }
      return true;
    }

    public bool CompletedAllChallenges()
    {
      if (!this.isCompleted)
        return false;
      for (int index = 0; index < this.levelResults.Count; ++index)
      {
        if (!GlobalManager.Instance.database.IsTutorialLevel(this.levelResults[index].id) && !this.levelResults[index].data.challengeStar)
          return false;
      }
      return true;
    }

    public bool HasLevelResult(int _levelId)
    {
      foreach (LevelResult levelResult in this.levelResults)
      {
        if (levelResult.id == _levelId)
          return true;
      }
      return false;
    }

    public void ReadBinary(BinaryReader reader)
    {
      this.id = reader.ReadInt32();
      this._isCompleted = reader.ReadBoolean();
      this.levelResultsLength = reader.ReadInt32();
      this.levelResults = new List<LevelResult>();
      for (int index = 0; index < this.levelResultsLength; ++index)
      {
        this.levelResults.Add(new LevelResult(-1));
        this.levelResults[index].ReadBinary(reader);
      }
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.id);
      writer.Write(this.isCompleted);
      this.levelResultsLength = this.levelResults.Count;
      writer.Write(this.levelResultsLength);
      for (int index = 0; index < this.levelResultsLength; ++index)
        this.levelResults[index].WriteBinary(writer);
    }
  }
}
