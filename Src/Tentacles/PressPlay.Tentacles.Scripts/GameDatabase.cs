// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GameDatabase
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GameDatabase
  {
    public List<World> worlds;

    public Level GetNextLevel(LevelProgress progress)
    {
      Level level = this.GetLevel(progress.levelId);
      this.GetLevels(level.worldId, level.batch);
      if (level.nextLevel != -1)
        return this.GetLevel(level.nextLevel);
      int num = this.worlds.IndexOf(this.GetWorldFromId(level.worldId));
      return num < this.worlds.Count - 1 ? this.worlds[num + 1].levels[0] : (Level) null;
    }

    public Dictionary<int, LevelBatch> GetLevelsInBatches()
    {
      Dictionary<int, LevelBatch> levelsInBatches = new Dictionary<int, LevelBatch>();
      foreach (Level allLevel in this.GetAllLevels())
      {
        if (!levelsInBatches.ContainsKey(allLevel.batch))
          levelsInBatches.Add(allLevel.batch, new LevelBatch());
        levelsInBatches[allLevel.batch].levels.Add(allLevel);
      }
      return levelsInBatches;
    }

    public List<int> GetBatches(int worldId)
    {
      World world = this.GetWorld(worldId);
      List<int> batches = new List<int>();
      foreach (Level level in world.levels)
      {
        if (!batches.Contains(level.batch))
          batches.Add(level.batch);
      }
      return batches;
    }

    public Level GetLastUnlockedLevel(LevelProgress progress) => this.GetLevel(progress.levelId);

    public Level GetNextLevelAfterId(int _levelID)
    {
      Level level = this.GetLevel(_levelID);
      this.GetLevels(level.worldId, level.batch);
      if (level.nextLevel != -1)
        return this.GetLevel(level.nextLevel);
      int num = this.worlds.IndexOf(this.GetWorldFromId(level.worldId));
      if (num < this.worlds.Count - 1)
        return this.worlds[num + 1].levels[0];
      Debug.Log((object) "Can't find any next level.");
      return (Level) null;
    }

    public bool IsLastLevelInWorld(Level _currentLevel)
    {
      return _currentLevel.levelsIndex == this.worlds[_currentLevel.worldsIndex].levels.Count - 1;
    }

    public bool IsTutorialLevel(int levelID) => this.IsTutorialLevel(this.GetLevel(levelID));

    public bool IsTutorialLevel(Level level) => level.levelsIndex == 0;

    public Level GetFirstLevel() => this.worlds[0].levels[0];

    public Level GetLevel(int levelId)
    {
      foreach (World world in this.worlds)
      {
        foreach (Level level in world.levels)
        {
          if (level.id == levelId)
            return level;
        }
      }
      return (Level) null;
    }

    public Level GetLevel(int worldId, int levelId)
    {
      World world = this.GetWorld(worldId);
      if (world == null)
      {
        Debug.LogError("The world with id " + (object) worldId + " doesn't exist!");
        return (Level) null;
      }
      Level level = world.GetLevel(levelId);
      if (level != null)
        return level;
      Debug.LogError("The level with id " + (object) levelId + " doesn't exist in world " + (object) worldId);
      return (Level) null;
    }

    public LevelBatch GetLevelsInBatch(int worldId, int batchId)
    {
      LevelBatch levelsInBatch = new LevelBatch();
      World world = this.GetWorld(worldId);
      if (world != null)
      {
        foreach (Level level in world.levels)
        {
          if (level.batch == batchId)
            levelsInBatch.levels.Add(level);
        }
      }
      return levelsInBatch;
    }

    public Level GetLevelFromSceneName(string sceneName)
    {
      foreach (Level allLevel in this.GetAllLevels())
      {
        if (allLevel.sceneName == sceneName)
          return allLevel;
      }
      return (Level) null;
    }

    public List<Level> GetLevels(int worldId, int batch)
    {
      List<Level> levels = new List<Level>();
      World world = this.GetWorld(worldId);
      if (world != null)
      {
        foreach (Level level in world.levels)
        {
          if (level.batch == batch)
            levels.Add(level);
        }
      }
      return levels;
    }

    public List<Level> GetAllLevels()
    {
      List<Level> allLevels = new List<Level>();
      foreach (World world in this.worlds)
      {
        foreach (Level level in world.levels)
          allLevels.Add(level);
      }
      return allLevels;
    }

    public World GetWorld(int id)
    {
      foreach (World world in this.worlds)
      {
        if (world.id == id)
          return world;
      }
      return (World) null;
    }

    public Level GetCurrentLevelFromSceneName(string _sceneName)
    {
      for (int index1 = 0; index1 < this.worlds.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.worlds[index1].levels.Count; ++index2)
        {
          if (this.worlds[index1].levels[index2].sceneName == _sceneName)
            return this.worlds[index1].levels[index2];
        }
      }
      Debug.Log((object) ("GetCurrentLevelFromSceneName     FAIL- NO LEVEL IN DATABASE MATCHES SCENE NAME : " + _sceneName));
      return (Level) null;
    }

    public void ConsolidateIndexes()
    {
      for (int index1 = 0; index1 < this.worlds.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.worlds[index1].levels.Count; ++index2)
        {
          this.worlds[index1].levels[index2].levelsIndex = index2;
          this.worlds[index1].levels[index2].worldsIndex = index1;
        }
      }
    }

    private bool DoesWorldExist(int id)
    {
      foreach (World world in this.worlds)
      {
        if (world.id == id)
          return true;
      }
      return false;
    }

    private World GetWorldFromId(int id)
    {
      foreach (World world in this.worlds)
      {
        if (world.id == id)
          return world;
      }
      return (World) null;
    }

    public void AddLevel(LevelImportDataStructure data)
    {
      Level level = new Level(data);
      if (!this.DoesWorldExist(int.Parse(data.world_id)))
        this.worlds.Add(new World(int.Parse(data.world_id), data.world_name));
      World worldFromId = this.GetWorldFromId(level.worldId);
      if (worldFromId.levels.Count > 0)
      {
        level.prevLevel = worldFromId.levels[worldFromId.levels.Count - 1].id;
        worldFromId.levels[worldFromId.levels.Count - 1].nextLevel = level.id;
      }
      worldFromId.AddLevel(level);
    }

    public static int GetLeaderboardIdFromBatchId(int batchId)
    {
      switch (batchId)
      {
        case 1:
          return 1;
        case 2:
          return 2;
        case 3:
          return 3;
        case 4:
          return 4;
        case 5:
          return 5;
        case 6:
          return 6;
        case 7:
          return 7;
        case 8:
          return 8;
        case 9:
          return 9;
        case 10:
          return 10;
        default:
          return -1;
      }
    }

    public void Reset() => this.worlds = new List<World>();
  }
}
