// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelResultData
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct LevelResultData
  {
    public int score;
    public bool allPickupsStar;
    public bool noDeathsStar;
    public bool challengeStar;
    public int totalPickups;
    public int pickups;

    public LevelResultData(
      int score,
      bool allPickupsStar,
      bool noDeathsStar,
      bool challengeStar,
      int pickups,
      int totalPickups)
    {
      this.score = score;
      this.allPickupsStar = allPickupsStar;
      this.noDeathsStar = noDeathsStar;
      this.challengeStar = challengeStar;
      this.pickups = pickups;
      this.totalPickups = totalPickups;
    }

    public LevelResultData(int score)
    {
      this.score = score;
      this.allPickupsStar = false;
      this.noDeathsStar = false;
      this.challengeStar = false;
      this.pickups = 0;
      this.totalPickups = -1;
    }

    public void ReadBinary(BinaryReader reader)
    {
      this.score = reader.ReadInt32();
      this.allPickupsStar = reader.ReadBoolean();
      this.noDeathsStar = reader.ReadBoolean();
      this.challengeStar = reader.ReadBoolean();
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.score);
      writer.Write(this.allPickupsStar);
      writer.Write(this.noDeathsStar);
      writer.Write(this.challengeStar);
    }
  }
}
