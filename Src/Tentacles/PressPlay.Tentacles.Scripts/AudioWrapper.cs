// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AudioWrapper
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Attributes;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  [FixReferences]
  public class AudioWrapper
  {
    public AudioClip[] sounds;
    public bool random;
    public float volume = 1f;
    public float pitch = 1f;
    public Vector2 pitchVariation = new Vector2(0.0f, 0.0f);
    public bool loop;
    public float lockDelay;
    public bool useSpacialAwareness;
    public Transform worldPosition;
    public float soundRadius;
    public float fullVolumeRadius;
    private int index;
    private AudioObject currentAudioObject;
    private float lastPlayTime;
    private bool isInitialized;

    public bool isPlaying => this.currentAudioObject != null && this.currentAudioObject.isPlaying;

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
    }

    public void Stop()
    {
      if (!this.isPlaying)
        return;
      this.currentAudioObject.Stop();
    }

    public void PlaySound()
    {
      if (!AudioManager.isLoaded)
        return;
      if (!this.isInitialized)
        this.Initialize();
      if (this.sounds == null || this.sounds.Length == 0 || (double) Time.time < (double) this.lastPlayTime + (double) this.lockDelay)
        return;
      if (this.isPlaying)
        this.currentAudioObject.Stop();
      int index;
      if (this.random)
      {
        index = Mathf.Clamp(Random.Range(0, this.sounds.Length), 0, this.sounds.Length - 1);
      }
      else
      {
        index = this.index;
        ++this.index;
        if (this.index == this.sounds.Length)
          this.index = 0;
      }
      float num = Mathf.Clamp(this.pitch + Random.Range(-this.pitchVariation.x, this.pitchVariation.y), 0.0f, 1f);
      AudioSettings settings = new AudioSettings(this.sounds[index], this.volume, 1f, this.loop);
      if (this.useSpacialAwareness)
        settings.EnableSpacialAwareness(this.fullVolumeRadius, this.soundRadius, this.worldPosition, LevelHandler.Instance.cam.transform);
      settings.pitch = num;
      this.currentAudioObject = AudioManager.Instance.Get(AudioManager.Instance.Play(settings));
      this.lastPlayTime = Time.time;
    }
  }
}
