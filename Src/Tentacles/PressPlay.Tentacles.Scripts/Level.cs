// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Level
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Level
  {
    public string name;
    public string nameKey;
    public int id;
    public int batch;
    public int worldId;
    public int worldsIndex;
    public int themeId;
    public string themeName;
    public int levelsIndex;
    public string sceneName;
    public int numberOfLives = 3;
    public int bronzeScore;
    public int silverScore;
    public int goldScore;
    public string hintscreen;
    public int nextLevel = -1;
    public int prevLevel = -1;
    public Level.LevelType levelType = Level.LevelType.desatGreen;

    protected Level()
    {
    }

    public Level(LevelImportDataStructure data)
    {
      this.name = data.level_name;
      this.id = int.Parse(data.level_id);
      this.worldId = int.Parse(data.world_id);
      this.sceneName = data.scene_name;
      this.batch = int.Parse(data.batch);
      if (data.theme_id != "")
        this.themeId = int.Parse(data.theme_id);
      this.themeName = data.theme_name;
      this.SetLevelTheme(this.themeName);
      if (data.bronze_score != "")
        this.bronzeScore = int.Parse(data.bronze_score);
      if (data.silver_score != "")
        this.silverScore = int.Parse(data.silver_score);
      if (!(data.gold_score != ""))
        return;
      this.goldScore = int.Parse(data.gold_score);
    }

    public int CalculateNumberOfMedals(int score)
    {
      if (score >= this.goldScore)
        return 3;
      if (score >= this.silverScore)
        return 2;
      return score >= this.bronzeScore ? 1 : 0;
    }

    private void SetLevelTheme(string theme)
    {
      switch (theme)
      {
        case "brains":
          this.levelType = Level.LevelType.brain;
          break;
        case "veins":
          this.levelType = Level.LevelType.veins;
          break;
        case "intestines":
          this.levelType = Level.LevelType.intestines;
          break;
        case "desatGreen":
          this.levelType = Level.LevelType.desatGreen;
          break;
        case "petriDish":
          this.levelType = Level.LevelType.petriDish;
          break;
        default:
          this.levelType = Level.LevelType.desatGreen;
          break;
      }
    }

    public static Level CreateDummyLevel(string _sceneName)
    {
      return new Level(new LevelImportDataStructure()
      {
        level_id = "-1",
        batch = "-1",
        world_id = "-1",
        bronze_score = "0",
        silver_score = "0",
        gold_score = "0",
        theme_id = "0",
        theme_name = "",
        scene_name = _sceneName
      });
    }

    public enum LevelType
    {
      veins,
      intestines,
      brain,
      desatGreen,
      petriDish,
    }
  }
}
