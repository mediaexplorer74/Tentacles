// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelTypeSettings
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelTypeSettings : MonoBehaviour
  {
    public Material lemmyGlowMaterial;
    public Material lemmyTentacleMaterial;
    public Color backgroundColor;
    public PoolableParticle OnTentacleConnectParticle;
    public PoolableParticle OnTentacleBounceParticle;
    public PoolableParticle OnTentacleShieldParticle;
    public LevelMusicAndSound audio;
  }
}
