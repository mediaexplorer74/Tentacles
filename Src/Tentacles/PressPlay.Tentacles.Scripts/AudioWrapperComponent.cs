// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AudioWrapperComponent
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AudioWrapperComponent : MonoBehaviour
  {
    public AudioWrapper sound;

    public override void Update()
    {
      if (!LevelHandler.Instance.isInGamePlay || this.sound.isPlaying)
        return;
      this.sound.PlaySound();
    }

    public void OnTurnOffAtDistance() => this.sound.Stop();
  }
}
