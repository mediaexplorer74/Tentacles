// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.World
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class World
  {
    public string nameKey;
    public int id;
    public string menuTexture;
    public int worldsIndex;
    public List<Level> levels;

    protected World()
    {
    }

    public World(int id, string name)
    {
      this.id = id;
      this.nameKey = name;
      this.levels = new List<Level>();
    }

    public Level GetLevel(int id)
    {
      foreach (Level level in this.levels)
      {
        if (level.id == id)
          return level;
      }
      return (Level) null;
    }

    public void AddLevel(Level level) => this.levels.Add(level);
  }
}
