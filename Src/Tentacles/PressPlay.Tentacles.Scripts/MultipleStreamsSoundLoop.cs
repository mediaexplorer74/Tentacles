// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MultipleStreamsSoundLoop
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  internal class MultipleStreamsSoundLoop : StreamedSoundLoop
  {
    private Stream[] waveFileStreams;
    private Dictionary<string, BinaryReader> readers;

    public MultipleStreamsSoundLoop(
      string[] fileLocations,
      string[] ids,
      int startBuffer,
      float bufferSizeMilliseconds)
      : base(fileLocations[startBuffer], bufferSizeMilliseconds)
    {
      this.waveFileStreams = new Stream[ids.Length];
      for (int index = 0; index < fileLocations.Length; ++index)
        this.waveFileStreams[index] = TitleContainer.OpenStream(fileLocations[index]);
      this.readers = new Dictionary<string, BinaryReader>();
      for (int index = 0; index < ids.Length; ++index)
        this.readers.Add(ids[index], new BinaryReader(this.waveFileStreams[index]));
    }

    public void SwitchToStream(string id)
    {
      Debug.Display(nameof (SwitchToStream), (object) id);
      this.reader = this.readers[id];
    }
  }
}
