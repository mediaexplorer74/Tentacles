// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MusicController
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MusicController : ResetOnLemmyDeath
  {
    private List<MusicController.BackgroundLoopId> loopsPlaying = new List<MusicController.BackgroundLoopId>();
    private MultipleStreamsSoundLoop soundStreamer;
    private Dictionary<MusicController.BackgroundLoopId, AudioObject> loops = new Dictionary<MusicController.BackgroundLoopId, AudioObject>();
    private List<AudioObject> musicToBeRemoved = new List<AudioObject>();
    public AudioSettings defaultTrackSetting;
    public bool playMusicOnStart;
    public MusicController.BackgroundLoopId musicToPlayOnStart;
    public float defautFadeIn = 1f;
    public float defaultFadeOut = 1f;
    private bool _isInitialized;
    public AudioWrapper swoosh;
    private float swooshTime = -1f;
    private float fadeStartTime;
    private float fadeDuration;
    private float fadeStartVolume = 1f;
    private float fadeEndVolume = 1f;
    private static MusicController instance;

    private float soundOnOffVolume
    {
      get
      {
        return !MediaPlayer.GameHasControl || !GlobalManager.Instance.currentProfile.musicIsEnabled ? 0.0f : 1f;
      }
    }

    private float fadeVolume
    {
      get
      {
        return (double) this.fadeDuration == 0.0 || (double) Time.realtimeSinceStartup > (double) this.fadeStartTime + (double) this.fadeDuration ? this.fadeEndVolume : (float) (((double) Time.realtimeSinceStartup - (double) this.fadeStartTime) / (double) this.fadeDuration * ((double) this.fadeEndVolume - (double) this.fadeStartVolume)) + this.fadeStartVolume;
      }
    }

    public bool isInitialized => this._isInitialized;

    public static MusicController Instance
    {
      get
      {
        if (MusicController.instance == null)
          Debug.LogError("Attempt to access instance of LevelHandler singleton earlier than Start or without it being attached to a GameObject.");
        return MusicController.instance;
      }
    }

    public override void Awake()
    {
      base.Awake();
      if (MusicController.instance != null)
        MusicController.instance.Destroy();
      MusicController.instance = this;
    }

    public void Init(
      string _stream1,
      string _stream2,
      string _stream3,
      string _stream12,
      string _stream13,
      string _stream23,
      string _stream123)
    {
      int musicToPlayOnStart = (int) this.musicToPlayOnStart;
      this.soundStreamer = new MultipleStreamsSoundLoop(new string[7]
      {
        "Content/Sounds/Streams/" + _stream1,
        "Content/Sounds/Streams/" + _stream2,
        "Content/Sounds/Streams/" + _stream3,
        "Content/Sounds/Streams/" + _stream12,
        "Content/Sounds/Streams/" + _stream13,
        "Content/Sounds/Streams/" + _stream23,
        "Content/Sounds/Streams/" + _stream123
      }, new string[7]
      {
        "stream1",
        "stream2",
        "stream3",
        "stream12",
        "stream13",
        "stream23",
        "stream123"
      }, musicToPlayOnStart, 500f);
      this.loops.Add(MusicController.BackgroundLoopId.loop1, (AudioObject) null);
      this.loops.Add(MusicController.BackgroundLoopId.loop2, (AudioObject) null);
      this.loops.Add(MusicController.BackgroundLoopId.loop3, (AudioObject) null);
      if (this.playMusicOnStart)
      {
        this.FadeFromTo(0.0f, 1f, 2.5f);
        this.Play(this.musicToPlayOnStart);
      }
      this._isInitialized = true;
    }

    public void MuteAll(float fadeDuration)
    {
      foreach (KeyValuePair<MusicController.BackgroundLoopId, AudioObject> loop in this.loops)
        loop.Value.Fade(0.0f, this.defaultFadeOut);
    }

    public void MuteAll() => this.MuteAll(this.defaultFadeOut);

    public void Mute(MusicController.BackgroundLoopId id, float fadeDuration)
    {
      if (!this.loops.ContainsKey(id))
        return;
      if (this.loops[id] != null)
        this.loops[id].Fade(0.0f, fadeDuration);
      if (this.loopsPlaying.Contains(id))
        this.loopsPlaying.Remove(id);
      this.SwitchToCurrentTrack();
    }

    public void Mute(MusicController.BackgroundLoopId id) => this.Mute(id, this.defaultFadeOut);

    public void Play(MusicController.BackgroundLoopId id) => this.Play(id, this.defautFadeIn);

    public void Play(MusicController.BackgroundLoopId id, float fadeDuration)
    {
      if (!this.loops.ContainsKey(id))
        return;
      if (this.loops[id] != null)
        this.loops[id].Fade(1f, fadeDuration);
      if (!this.loopsPlaying.Contains(id))
        this.loopsPlaying.Add(id);
      this.SwitchToCurrentTrack();
    }

    private void SwitchToCurrentTrack()
    {
      if (this.loopsPlaying.Count == 3)
        this.soundStreamer.SwitchToStream("stream123");
      if (this.loopsPlaying.Count == 1)
      {
        if (this.loopsPlaying[0] == MusicController.BackgroundLoopId.loop1)
          this.soundStreamer.SwitchToStream("stream1");
        if (this.loopsPlaying[0] == MusicController.BackgroundLoopId.loop2)
          this.soundStreamer.SwitchToStream("stream2");
        if (this.loopsPlaying[0] == MusicController.BackgroundLoopId.loop3)
          this.soundStreamer.SwitchToStream("stream3");
      }
      if (this.loopsPlaying.Count == 2)
      {
        if (this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop1) && this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop2))
          this.soundStreamer.SwitchToStream("stream12");
        if (this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop1) && this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop3))
          this.soundStreamer.SwitchToStream("stream13");
        if (this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop2) && this.loopsPlaying.Contains(MusicController.BackgroundLoopId.loop3))
          this.soundStreamer.SwitchToStream("stream23");
      }
      if (this.soundStreamer.State == SoundState.Playing)
        this.swooshTime = Time.time + 1.6f;
      else
        this.soundStreamer.Play();
    }

    public override void Update()
    {
      if (!this.isInitialized)
        return;
      base.Update();
      if ((double) this.swooshTime != -1.0 && (double) Time.time >= (double) this.swooshTime)
      {
        this.swoosh.PlaySound();
        this.swooshTime = -1f;
      }
      float num = this.soundOnOffVolume * this.fadeVolume;
      this.soundStreamer.Volume = num;
      if (this.swoosh == null)
        return;
      this.swoosh.volume = num;
    }

    public void FadeTo(float newVolume, float duration)
    {
      this.fadeStartVolume = this.soundStreamer.Volume;
      this.fadeEndVolume = newVolume;
      this.fadeStartTime = Time.realtimeSinceStartup;
      this.fadeDuration = duration;
    }

    public void FadeFromTo(float startVolume, float endVolume, float duration)
    {
      this.soundStreamer.Volume = startVolume;
      this.fadeStartVolume = startVolume;
      this.fadeEndVolume = endVolume;
      this.fadeStartTime = Time.realtimeSinceStartup;
      this.fadeDuration = duration;
    }

    public void Pause() => this.soundStreamer.Pause();

    public void Resume() => this.soundStreamer.Resume();

    protected override void Destroy()
    {
      base.Destroy();
      this.soundStreamer.Destroy();
    }

    public enum BackgroundLoopId
    {
      loop1,
      loop2,
      loop3,
    }
  }
}
