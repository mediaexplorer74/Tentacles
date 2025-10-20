// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelLoadInfo
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct LevelLoadInfo
  {
    public bool loadLevelOnStartup;
    public int levelId;

    public void ReadBinary(BinaryReader reader)
    {
      this.loadLevelOnStartup = reader.ReadBoolean();
      this.levelId = reader.ReadInt32();
    }

    public void WriteBinary(BinaryWriter writer)
    {
      writer.Write(this.loadLevelOnStartup);
      writer.Write(this.levelId);
    }
  }
}
