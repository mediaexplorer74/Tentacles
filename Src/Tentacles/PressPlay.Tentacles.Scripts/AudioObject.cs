// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AudioObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AudioObject
  {
    private const string STOP = "stop";
    private const string PAUSE = "pause";
    public AudioClip clip;
    public AudioSource source;
    public AudioSettings settings;
    public int index = -1;
    public string category = "default";
    public int priority;
    public bool isLocked;
    public bool isOverridden;
    public bool isPaused;
    public bool doUpdate = true;
    private float delayedStart;
    private bool isWaitingToStart;
    private float timeToDecreaseSound;
    private string onFadeComplete = "";
    private bool isFading;
    private float fadeFrom;
    private float fadeTo = 1f;
    private float fadeDuration;
    private float fadeTimeCounter;
    private float fadeInTime;
    private float fadeOutTime;
    private float lastRealTime;

    public int id
    {
      get => this.settings == null ? 0 : this.settings.id;
      set => this.settings.id = value;
    }

    public bool isAvailableForOverwrite
    {
      get => this.settings == null || this.settings.isAvailableForOverwrite;
      set => this.settings.isAvailableForOverwrite = value;
    }

    public bool isLooping => this.source != null && this.source.loop;

    public bool isPlaying
    {
      get
      {
        if (this.source == null)
          return false;
        return this.isWaitingToStart || this.source.isPlaying;
      }
    }

    public float percentage
    {
      get => this.source == null ? 0.0f : this.source.time / this.source.clip.length;
    }

    public float volume
    {
      get => this.settings == null ? 0.0f : this.settings.volume;
      set
      {
        if (this.source == null)
          return;
        this.source.volume = value;
      }
    }

    public float pitch
    {
      get => this.settings == null ? 0.0f : this.settings.pitch;
      set
      {
        this.source.pitch = value;
        this.settings.pitch = value;
      }
    }

    public void Reset(AudioSettings settings, AudioSource source, string category, int priority)
    {
      this.isLocked = false;
      this.isOverridden = false;
      this.isPaused = false;
      this.doUpdate = true;
      this.delayedStart = 0.0f;
      this.isWaitingToStart = false;
      this.timeToDecreaseSound = 0.0f;
      this.onFadeComplete = "";
      this.isFading = false;
      this.fadeFrom = 0.0f;
      this.fadeTo = 1f;
      this.fadeDuration = 0.0f;
      this.fadeTimeCounter = 0.0f;
      this.fadeInTime = 0.0f;
      this.fadeOutTime = 0.0f;
      this.lastRealTime = 0.0f;
      this.settings = settings;
      this.source = source;
      this.category = category;
      this.priority = priority;
    }

    public void Update()
    {
      if (this.settings == null || this.isPaused || !this.doUpdate)
        return;
      if (this.isFading)
      {
        this.fadeTimeCounter += Time.realtimeSinceStartup - this.lastRealTime;
        if ((double) this.fadeTimeCounter < (double) this.fadeDuration)
        {
          this.source.volume = Mathf.Lerp(this.fadeFrom, this.fadeTo, this.fadeTimeCounter / this.fadeDuration);
        }
        else
        {
          this.source.volume = this.fadeTo;
          this.isFading = false;
          this.OnFadeComplete();
        }
        this.lastRealTime = Time.realtimeSinceStartup;
      }
      if (this.isWaitingToStart)
      {
        this.delayedStart -= Time.deltaTime;
        if ((double) this.delayedStart <= 0.0)
        {
          this.source.Play();
          this.isWaitingToStart = false;
        }
      }
      if (this.isOverridden)
      {
        this.timeToDecreaseSound -= Time.deltaTime;
        if ((double) this.timeToDecreaseSound <= 0.0)
        {
          this.Fade(this.volume, this.fadeInTime);
          this.isOverridden = false;
        }
      }
      if (!this.settings.useSpacialAwareness)
        return;
      this.source.volume = this.settings.spacialVolume;
    }

    public void DoSoundOverride(
      float duration,
      float volumeFactor,
      float timeToFadeIn,
      float timeToFadeOut)
    {
      this.timeToDecreaseSound = duration;
      this.Fade(this.volume * volumeFactor, this.fadeOutTime, timeToFadeIn, timeToFadeOut);
      this.isOverridden = true;
    }

    public void Play()
    {
      if (this.settings.audioType == AudioSettings.AudioType.SoundEffect && !AudioManager.Instance.soundIsEnabled || this.settings.audioType == AudioSettings.AudioType.Music && !AudioManager.Instance.musicIsEnabled)
        return;
      this.settings.CopyToSource(ref this.source);
      if (this.settings.useSpacialAwareness)
        this.source.volume = this.settings.spacialVolume;
      this.delayedStart = this.settings.delay;
      if ((double) this.delayedStart > 0.0)
        this.isWaitingToStart = true;
      else
        this.source.Play();
      this.isPaused = false;
    }

    public void Pause()
    {
      this.source.Pause();
      this.isPaused = true;
    }

    public void UnPause()
    {
      this.source.Play();
      this.isPaused = false;
    }

    public void Stop()
    {
      this.isFading = false;
      this.isWaitingToStart = false;
      if (this.source == null)
        return;
      this.source.Stop();
    }

    public void Fade(float toValue, float duration)
    {
      this.Fade(toValue, duration, this.fadeInTime, this.fadeOutTime);
    }

    public void Fade(float toValue, float duration, float timeToFadeIn, float timeToFadeOut)
    {
      if (this.settings == null || this.source == null)
        return;
      if (this.settings.audioType == AudioSettings.AudioType.SoundEffect && !AudioManager.Instance.soundIsEnabled || this.settings.audioType == AudioSettings.AudioType.Music && !AudioManager.Instance.musicIsEnabled)
      {
        this.fadeFrom = 0.0f;
        this.fadeTo = 0.0f;
      }
      else
      {
        this.fadeFrom = this.source.volume;
        this.fadeTo = toValue;
      }
      if ((double) this.fadeDuration == 0.0)
      {
        this.source.volume = this.fadeTo;
        this.isFading = false;
      }
      else
      {
        this.fadeDuration = duration;
        this.fadeInTime = timeToFadeIn;
        this.fadeOutTime = timeToFadeOut;
        this.fadeTimeCounter = 0.0f;
        this.lastRealTime = Time.realtimeSinceStartup;
        this.isFading = true;
      }
    }

    private void OnFadeComplete()
    {
      switch (this.onFadeComplete)
      {
        case "stop":
          this.Stop();
          break;
        case "pause":
          this.Pause();
          break;
      }
      this.onFadeComplete = "";
    }

    public void EnableSpacialAwareness(
      float fullVolumeRadius,
      float radius,
      Transform worldPosition,
      Transform listenerPosition)
    {
      this.settings.EnableSpacialAwareness(fullVolumeRadius, radius, worldPosition, listenerPosition);
    }

    public void Destroy()
    {
      this.Stop();
      this.clip = (AudioClip) null;
      this.settings = (AudioSettings) null;
      if (this.source == null)
        return;
      this.source.clip = (AudioClip) null;
      this.source = (AudioSource) null;
    }
  }
}
