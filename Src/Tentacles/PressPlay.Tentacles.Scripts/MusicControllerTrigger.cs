// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MusicControllerTrigger
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MusicControllerTrigger : MonoBehaviour
  {
    public MusicController.BackgroundLoopId[] loopsToTrigger;
    public MusicController.BackgroundLoopId[] loopsToFadeOut;
    public float triggerDelay = 1f;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public bool fadeOutOnExitTrigger;
    private bool isLemmyInTrigger;
    private float lastEnterTime;
    private float lastExitTime;
    private MusicControllerTrigger.MusicControllerMode mode;
    private bool hasStartedMusic;

    public override void OnTriggerEnter(Collider collider)
    {
      if (this.isLemmyInTrigger)
        return;
      this.isLemmyInTrigger = true;
      this.mode = MusicControllerTrigger.MusicControllerMode.isWaitingToTriggerEnter;
      this.lastEnterTime = Time.time;
    }

    public override void OnTriggerExit(Collider collider)
    {
      if (!this.isLemmyInTrigger)
        return;
      this.isLemmyInTrigger = false;
      this.mode = MusicControllerTrigger.MusicControllerMode.isWaitingToTriggerExit;
      this.lastExitTime = Time.time;
    }

    public override void Update()
    {
      if (this.mode == MusicControllerTrigger.MusicControllerMode.inactive)
        return;
      switch (this.mode)
      {
        case MusicControllerTrigger.MusicControllerMode.isWaitingToTriggerEnter:
          if ((double) Time.time <= (double) this.lastEnterTime + (double) this.triggerDelay)
            break;
          if (!this.hasStartedMusic)
            this.PlayMusic();
          this.mode = MusicControllerTrigger.MusicControllerMode.inactive;
          break;
        case MusicControllerTrigger.MusicControllerMode.isWaitingToTriggerExit:
          if ((double) Time.time <= (double) this.lastExitTime + (double) this.triggerDelay)
            break;
          if (this.hasStartedMusic && this.fadeOutOnExitTrigger)
            this.MuteMusic();
          this.mode = MusicControllerTrigger.MusicControllerMode.inactive;
          break;
      }
    }

    private void PlayMusic()
    {
      if (!LevelHandler.isLoaded)
        return;
      foreach (MusicController.BackgroundLoopId id in this.loopsToFadeOut)
        LevelHandler.Instance.musicController.Mute(id, this.fadeOutTime);
      foreach (MusicController.BackgroundLoopId id in this.loopsToTrigger)
        LevelHandler.Instance.musicController.Play(id, this.fadeInTime);
      this.hasStartedMusic = true;
    }

    private void MuteMusic()
    {
      if (!LevelHandler.isLoaded)
        return;
      foreach (MusicController.BackgroundLoopId id in this.loopsToTrigger)
        LevelHandler.Instance.musicController.Mute(id, this.fadeOutTime);
      this.hasStartedMusic = false;
    }

    public enum MusicControllerMode
    {
      inactive,
      isWaitingToTriggerEnter,
      isWaitingToTriggerExit,
    }
  }
}
