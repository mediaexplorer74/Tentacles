// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelResult
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelResult
  {
    public int id;
    public int worldId;
    public LevelResultData data;

    public int score => this.data.score;

    public LevelResult(int levelId)
    {
      this.id = levelId;
      this.data = new LevelResultData();
    }

    public LevelResult(int levelId, int worldId, LevelResultData _data)
    {
      this.id = levelId;
      this.worldId = worldId;
      this.data = _data;
    }

    public void UpdateResult(LevelResultData _data)
    {
      if (_data.score > this.data.score)
        this.data.score = _data.score;
      if (_data.allPickupsStar)
        this.data.allPickupsStar = _data.allPickupsStar;
      if (_data.noDeathsStar)
        this.data.noDeathsStar = _data.noDeathsStar;
      if (!_data.challengeStar)
        return;
      this.data.challengeStar = _data.challengeStar;
    }

    public bool HasAllStars()
    {
      return this.data.allPickupsStar && this.data.challengeStar && this.data.noDeathsStar;
    }

    public void CopyData(LevelResult result) => this.data = result.data;

    public void ReadBinary(BinaryReader reader)
    {
      this.id = reader.ReadInt32();
      this.data.ReadBinary(reader);
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.id);
      this.data.WriteBinary(writer);
    }

    public new string ToString()
    {
      return "LevelResult id : " + (object) this.id + " score : " + (object) this.score;
    }
  }
}
