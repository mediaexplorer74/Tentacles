// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UserProfile
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class UserProfile
  {
    private const int SAVEGAME_VERSION = 11;
    private bool _soundIsEnabled = true;
    private bool _musicIsEnabled = true;
    private bool _vibrationIsEnabled = true;
    public string gamerTag;
    public int unlockedLevelsLength;
    public List<int> unlockedLevels = new List<int>();
    public int unlockedWorldsLength;
    public List<int> unlockedWorlds = new List<int>();
    public int worldResultsLength;
    public List<WorldResult> worldResults = new List<WorldResult>();
    public int unlockedMoviesLength;
    public List<int> unlockedMovies = new List<int>();
    public LevelProgress progress;
    public GlobalUserData globalData;
    [ContentSerializerIgnore]
    public LevelLoadInfo levelLoadInfo;
    [ContentSerializerIgnore]
    public UnlockedAchievements unlockedAchievements;
    [ContentSerializerIgnore]
    public bool scoreSavedInTrialMode;

    [ContentSerializerIgnore]
    public bool soundIsEnabled
    {
      get => this._soundIsEnabled;
      set
      {
        this._soundIsEnabled = value;
        if (!AudioManager.isLoaded)
          return;
        AudioManager.Instance.soundIsEnabled = value;
      }
    }

    [ContentSerializerIgnore]
    public bool musicIsEnabled
    {
      get => this._musicIsEnabled;
      set
      {
        this._musicIsEnabled = value;
        if (!AudioManager.isLoaded)
          return;
        AudioManager.Instance.musicIsEnabled = value;
      }
    }

    [ContentSerializerIgnore]
    public bool vibrationIsEnabled
    {
      get => this._vibrationIsEnabled;
      set => this._vibrationIsEnabled = value;
    }

    public int totalScore
    {
      get
      {
        int totalScore = 0;
        foreach (WorldResult worldResult in this.worldResults)
          totalScore += worldResult.totalScore;
        return totalScore;
      }
    }

    protected UserProfile()
    {
    }

    public UserProfile(Level _firstLevel)
    {
      this.globalData.playButtonOpensThisLevel = _firstLevel.id;
      this.unlockedLevels.Add(_firstLevel.id);
      this.progress = new LevelProgress(_firstLevel.worldId, _firstLevel.id);
      this.levelLoadInfo.loadLevelOnStartup = false;
      this.unlockedAchievements = UnlockedAchievements.CreateEmpty();
    }

    public void UnlockMovie(int _movieIndex)
    {
      if (this.unlockedMovies.Contains(_movieIndex))
        return;
      this.unlockedMovies.Add(_movieIndex);
    }

    public bool IsMovieUnlocked(int _movieIndex) => this.unlockedMovies.Contains(_movieIndex);

    public void AddCompletedLevelSession(LevelSession _session)
    {
      this.scoreSavedInTrialMode = GlobalManager.isTrialMode;
      this.AddLevelResult(_session.level, new LevelResultData(_session.score, _session.EvaluateAllPickupsMedal(), _session.EvaluateCompletedWithoutDying(), _session.EvaluateCompletedChallenge(), _session.numberOfPickupsCollected, _session.totalNumberOfPickups));
      this.globalData.totalDamageTaken += _session.damageTotal;
      this.globalData.totalDeaths += _session.numberOfDeaths;
      this.globalData.totalKills += _session.numberOfKills;
      AchievementsHandler.Instance.EvaluateCompletedLevelSessionForAchievements(_session);
    }

    public void AddLevelResult(Level _level, LevelResultData _data)
    {
      if (this.HasLevelResult(_level.worldId, _level.id))
      {
        this.GetLevelResult(_level.worldId, _level.id).UpdateResult(_data);
      }
      else
      {
        if (!this.HasWorldResult(_level.worldId))
          this.worldResults.Add(new WorldResult(_level.worldId));
        this.GetWorldResult(_level.worldId).AddResult(new LevelResult(_level.id, _level.worldId, _data));
      }
    }

    public int GetLevelScore(int _worldId, int _levelId)
    {
      int levelScore = 0;
      if (this.HasLevelResult(_worldId, _levelId))
      {
        LevelResult levelResult = this.GetLevelResult(_worldId, _levelId);
        levelScore += levelResult.score;
      }
      return levelScore;
    }

    public bool HasLevelResult(int _worldId, int _levelId)
    {
      foreach (WorldResult worldResult in this.worldResults)
      {
        if (worldResult.id == _worldId && worldResult.HasLevelResult(_levelId))
          return true;
      }
      return false;
    }

    public LevelResult GetLevelResult(Level _level)
    {
      return this.GetLevelResult(_level.worldId, _level.id);
    }

    public LevelResult GetLevelResult(int _worldId, int _levelId)
    {
      foreach (WorldResult worldResult in this.worldResults)
      {
        if (worldResult.id == _worldId)
          return worldResult.GetLevelResult(_levelId);
      }
      return (LevelResult) null;
    }

    public bool HasWorldResult(int _worldId)
    {
      foreach (WorldResult worldResult in this.worldResults)
      {
        if (worldResult.id == _worldId)
          return true;
      }
      return false;
    }

    public WorldResult GetWorldResult(World _world) => this.GetWorldResult(_world.id);

    public WorldResult GetWorldResult(int _worldId)
    {
      foreach (WorldResult worldResult in this.worldResults)
      {
        if (worldResult.id == _worldId)
          return worldResult;
      }
      return new WorldResult(1);
    }

    public bool IsLevelUnlocked(int levelId)
    {
      return (!GlobalManager.isTrialMode || this.IsLevelPartOfTrial(levelId)) && this.unlockedLevels.Contains(levelId);
    }

    public void UnlockAllTrialLevels()
    {
      List<Level> allLevels = GlobalManager.Instance.database.GetAllLevels();
      for (int index = 0; index < GlobalManager.Instance.gameplaySettings.numberOfLevelsToUnlockInTrialMode; ++index)
      {
        this.AddLevelResult(allLevels[index], new LevelResultData(0));
        this.UnlockLevel(allLevels[index].id);
      }
    }

    public void UnlockAllLevels()
    {
      foreach (Level allLevel in GlobalManager.Instance.database.GetAllLevels())
      {
        this.AddLevelResult(allLevel, new LevelResultData(0));
        this.UnlockLevel(allLevel.id);
      }
    }

    public void UnlockLevelAfterThis(Level _level) => this.UnlockLevel(_level.nextLevel);

    public void UnlockNextLevel()
    {
      Level nextLevel = GlobalManager.Instance.database.GetNextLevel(this.progress);
      if (nextLevel == null)
        return;
      this.UnlockLevel(nextLevel.id);
    }

    public void UnlockLevel(int levelId)
    {
      if (this.unlockedLevels.Contains(levelId))
        return;
      this.unlockedLevels.Add(levelId);
      Level level = GlobalManager.Instance.database.GetLevel(levelId);
      if (level == null)
        return;
      this.globalData.lastUnlockedLevel = levelId;
      this.progress.SetProgress(level.worldId, level.id);
    }

    public void UnlockWorld(int worldId)
    {
      if (this.unlockedWorlds.Contains(worldId))
        return;
      this.unlockedWorlds.Add(worldId);
    }

    public bool IsLevelPartOfTrial(int levelId)
    {
      return this.IsLevelPartOfTrial(GlobalManager.Instance.database.GetLevel(levelId));
    }

    public bool IsLevelPartOfTrial(Level level)
    {
      return GlobalManager.Instance.gameplaySettings.numberOfLevelsToUnlockInTrialMode > level.levelsIndex;
    }

    public void ReadBinary(BinaryReader reader)
    {
      int num = reader.ReadInt32();
      if (11 != num)
      {
        Debug.Log((object) ("WRONG SAVE GAME VERSION !! FILE IS VERSION : " + (object) num + " READER IS VERSION : " + (object) 11 + "  IF THE FILE IS SAVED, PREVIOUS DATA IS OVERWRITTEN!!!!"));
      }
      else
      {
        this.unlockedLevelsLength = reader.ReadInt32();
        this.unlockedLevels = new List<int>();
        for (int index = 0; index < this.unlockedLevelsLength; ++index)
          this.unlockedLevels.Add(reader.ReadInt32());
        this.unlockedWorldsLength = reader.ReadInt32();
        this.unlockedWorlds = new List<int>();
        for (int index = 0; index < this.unlockedWorldsLength; ++index)
          this.unlockedWorlds.Add(reader.ReadInt32());
        this.worldResultsLength = reader.ReadInt32();
        this.worldResults = new List<WorldResult>();
        for (int index = 0; index < this.worldResultsLength; ++index)
        {
          this.worldResults.Add(new WorldResult(-1));
          this.worldResults[index].ReadBinary(reader);
        }
        this.unlockedMoviesLength = reader.ReadInt32();
        this.unlockedMovies = new List<int>();
        for (int index = 0; index < this.unlockedMoviesLength; ++index)
          this.unlockedMovies.Add(reader.ReadInt32());
        this.progress.ReadBinary(reader);
        this.globalData.ReadBinary(reader);
        this.soundIsEnabled = reader.ReadBoolean();
        this.musicIsEnabled = reader.ReadBoolean();
        this.vibrationIsEnabled = reader.ReadBoolean();
        this.levelLoadInfo.ReadBinary(reader);
        this.unlockedAchievements.ReadBinary(reader);
        this.scoreSavedInTrialMode = reader.ReadBoolean();
      }
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(11);
      this.unlockedLevelsLength = this.unlockedLevels.Count;
      writer.Write(this.unlockedLevelsLength);
      for (int index = 0; index < this.unlockedLevelsLength; ++index)
        writer.Write(this.unlockedLevels[index]);
      this.unlockedWorldsLength = this.unlockedWorlds.Count;
      writer.Write(this.unlockedWorldsLength);
      for (int index = 0; index < this.unlockedWorldsLength; ++index)
        writer.Write(this.unlockedWorlds[index]);
      this.worldResultsLength = this.worldResults.Count;
      writer.Write(this.worldResultsLength);
      for (int index = 0; index < this.worldResultsLength; ++index)
        this.worldResults[index].WriteBinary(writer);
      this.unlockedMoviesLength = this.unlockedMovies.Count;
      writer.Write(this.unlockedMoviesLength);
      for (int index = 0; index < this.unlockedMoviesLength; ++index)
        writer.Write(this.unlockedMovies[index]);
      this.progress.WriteBinary(writer);
      this.globalData.WriteBinary(writer);
      writer.Write(this.soundIsEnabled);
      writer.Write(this.musicIsEnabled);
      writer.Write(this.vibrationIsEnabled);
      this.levelLoadInfo.WriteBinary(writer);
      this.unlockedAchievements.WriteBinary(writer);
      writer.Write(this.scoreSavedInTrialMode);
    }

    public override string ToString() => "Profile";

    internal bool IsValid()
    {
      if (this.globalData.playButtonOpensThisLevel != -1 && GlobalManager.Instance.database.GetLevel(this.globalData.playButtonOpensThisLevel) == null)
      {
        Debug.LogError("The user save game contains level " + (object) this.globalData.playButtonOpensThisLevel + " which does not exist. Save game is invalid.");
        return false;
      }
      Dictionary<int, LevelBatch> levelsInBatches = GlobalManager.Instance.database.GetLevelsInBatches();
      if (this.progress.totalScoresForValidation != null)
      {
        foreach (KeyValuePair<int, int> keyValuePair in this.progress.totalScoresForValidation)
        {
          if (levelsInBatches[keyValuePair.Key].GetTotalScore() != keyValuePair.Value)
          {
            Debug.LogError(string.Format("The user save game has a total score for batch {0} of {1} which does not correspond to the registered total of {2}. Save game is invalid.", (object) keyValuePair.Key, (object) keyValuePair.Value, (object) levelsInBatches[keyValuePair.Key].GetTotalScore()));
            return false;
          }
        }
      }
      return true;
    }
  }
}
