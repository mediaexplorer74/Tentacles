// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelProgress
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct LevelProgress(int worldId, int levelId)
  {
    public int worldId = worldId;
    public int levelId = levelId;
    [ContentSerializerIgnore]
    public Dictionary<int, int> totalScoresForValidation = (Dictionary<int, int>) null;

    public void SetProgress(int worldId, int levelId)
    {
      this.worldId = worldId;
      this.levelId = levelId;
    }

    public bool IsCurrentLevel(Level level)
    {
      return this.worldId == level.worldId && this.levelId == level.id;
    }

    public void ReadBinary(BinaryReader reader)
    {
      this.worldId = reader.ReadInt32();
      this.levelId = reader.ReadInt32();
      this.totalScoresForValidation = new Dictionary<int, int>();
      foreach (KeyValuePair<int, LevelBatch> levelsInBatch in GlobalManager.Instance.database.GetLevelsInBatches())
        this.totalScoresForValidation.Add(reader.ReadInt32(), reader.ReadInt32());
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.worldId);
      writer.Write(this.levelId);
      foreach (KeyValuePair<int, LevelBatch> levelsInBatch in GlobalManager.Instance.database.GetLevelsInBatches())
      {
        writer.Write(levelsInBatch.Key);
        writer.Write(levelsInBatch.Value.GetTotalScore());
      }
    }
  }
}
