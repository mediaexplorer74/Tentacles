// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelMusicAndSound
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelMusicAndSound
  {
    [ContentSerializer(Optional = true)]
    public AudioClip loop1;
    [ContentSerializer(Optional = true)]
    public AudioClip loop2;
    [ContentSerializer(Optional = true)]
    public AudioClip loop3;
    public string loopStream1;
    public string loopStream2;
    public string loopStream3;
    public string loopStream12;
    public string loopStream13;
    public string loopStream23;
    public string loopStream123;
    public AudioWrapper onTentacleHit;
    public AudioWrapper onTentacleBounce;
  }
}
