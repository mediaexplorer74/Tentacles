// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GlobalUserData
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct GlobalUserData
  {
    public float totalDamageTaken;
    public int totalDeaths;
    public int totalKills;
    public int lastPlayedLevel;
    public int lastUnlockedLevel;
    public int playButtonOpensThisLevel;

    public void ReadBinary(BinaryReader reader)
    {
      this.totalDamageTaken = reader.ReadSingle();
      this.totalDeaths = reader.ReadInt32();
      this.totalKills = reader.ReadInt32();
      this.lastPlayedLevel = reader.ReadInt32();
      this.lastUnlockedLevel = reader.ReadInt32();
      this.playButtonOpensThisLevel = reader.ReadInt32();
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.totalDamageTaken);
      writer.Write(this.totalDeaths);
      writer.Write(this.totalKills);
      writer.Write(this.lastPlayedLevel);
      writer.Write(this.lastUnlockedLevel);
      writer.Write(this.playButtonOpensThisLevel);
    }
  }
}
