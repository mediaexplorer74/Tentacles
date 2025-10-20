// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UnlockedAchievements
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct UnlockedAchievements
  {
    public int achievementsLength;
    public List<string> achievementKeys;

    public static UnlockedAchievements CreateEmpty()
    {
      return new UnlockedAchievements()
      {
        achievementsLength = 0,
        achievementKeys = new List<string>()
      };
    }

    public void ReadBinary(BinaryReader reader)
    {
      this.achievementsLength = reader.ReadInt32();
      this.achievementKeys = new List<string>();
      for (int index = 0; index < this.achievementsLength; ++index)
        this.achievementKeys.Add(reader.ReadString());
    }

    public void WriteBinary(BinaryWriter writer)
    {
      this.achievementsLength = this.achievementKeys.Count;
      writer.Write(this.achievementsLength);
      for (int index = 0; index < this.achievementsLength; ++index)
        writer.Write(this.achievementKeys[index]);
    }

    public void AddKey(string key)
    {
      if (this.achievementKeys.Contains(key))
        return;
      this.achievementKeys.Add(key);
    }
  }
}
