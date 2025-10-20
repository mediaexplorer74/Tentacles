// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.AudioSource
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Audio;
using PressPlay.FFWD.Interfaces;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public class AudioSource : Behaviour, IUpdateable
  {
    private AudioClip _clip;
    private float _minVolume;
    private float _maxVolume = 1f;
    private float _volume = 1f;
    public float pitch;
    private bool _loop;
    public bool ignoreListenerVolume;
    public bool playOnAwake;
    public float time;
    public AudioVelocityUpdateMode velocityUpdateMode;

    public AudioClip clip
    {
      get => this._clip;
      set
      {
        this._clip = value;
        this.SetSoundEffect(this._clip);
      }
    }

    public float minVolume
    {
      get => this._minVolume;
      set => this._minVolume = Mathf.Clamp01(value);
    }

    public float maxVolume
    {
      get => this._maxVolume;
      set => this._maxVolume = Mathf.Clamp01(value);
    }

    public float volume
    {
      get => this._volume;
      set
      {
        if (this.clip == null || this.clip.Instance.IsDisposed)
          return;
        this._volume = Mathf.Clamp01(value);
        this._volume = Mathf.Max(this._volume, this.minVolume);
        this._volume = Mathf.Min(this._volume, this.maxVolume);
        this.clip.Instance.Volume = this._volume;
      }
    }

    public bool isPlaying
    {
      get
      {
        return this.clip != null && !this.clip.Instance.IsDisposed && this.clip.Instance.State == SoundState.Playing;
      }
    }

    public bool loop
    {
      get => this._loop;
      set
      {
        this._loop = value;
        if (this.clip == null)
          return;
        this.clip.Loop(this._loop);
      }
    }

    private void SetSoundEffect(AudioClip sfx)
    {
      if (sfx == null)
        return;
      sfx.Instance.Volume = this.volume;
      this.time = 0.0f;
    }

    public void Play()
    {
      if (this.clip == null)
        return;
      if (this.clip.Instance.State != SoundState.Stopped)
        this.clip.Instance.Stop();
      this.clip.Instance.Play();
    }

    public void PlayOneShot(AudioClip clip, float volumeScale)
    {
      throw new NotImplementedException();
    }

    public void PlayOneShot(AudioClip clip) => throw new NotImplementedException();

    public void Stop()
    {
      if (this.clip == null || this.clip.Instance.IsDisposed)
        return;
      this.clip.Instance.Stop();
      this.time = 0.0f;
    }

    public void Pause()
    {
      if (this.clip == null || this.clip.Instance.IsDisposed)
        return;
      this.clip.Instance.Pause();
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
      AudioSource.PlayClipAtPoint(clip, position, 1f);
    }

    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
    {
      throw new NotImplementedException();
    }

    public void LateUpdate()
    {
    }

    public void Update()
    {
      if (this.clip == null || this.clip.Instance.IsDisposed || this.clip.Instance.State != SoundState.Playing)
        return;
      this.time += Time.deltaTime;
      if ((double) this.time < (double) this.clip.length)
        return;
      this.time -= this.clip.length;
      if (this.loop || !this.clip.Instance.IsLooped)
        return;
      this.clip.Instance.Stop();
    }
  }
}
