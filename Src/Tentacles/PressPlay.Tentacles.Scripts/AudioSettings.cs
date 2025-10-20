// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AudioSettings
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AudioSettings
  {
    [ContentSerializerIgnore]
    public int id = -1;
    [ContentSerializerIgnore]
    public float delay;
    [ContentSerializerIgnore]
    public bool isAvailableForOverwrite = true;
    [ContentSerializerIgnore]
    public AudioSettings.AudioType audioType;
    [ContentSerializerIgnore]
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
    public bool loop;
    public bool ignoreListenerVolume;
    public float minVolume;
    public float maxVolume = 1f;
    public float rolloffFactor = 1f;
    public AudioVelocityUpdateMode velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
    private bool _useSpacialAwareness;
    public Transform worldPosition;
    public Transform listenerPosition;
    public float soundRadius;
    public float fullVolumeRadius;
    private float distanceToListener = 100000f;

    public bool useSpacialAwareness => this._useSpacialAwareness;

    public float spacialVolume
    {
      get
      {
        if (this.listenerPosition == null || this.worldPosition == null)
          return 0.0f;
        float magnitude = (this.listenerPosition.position - this.worldPosition.position).magnitude;
        if ((double) magnitude > (double) this.soundRadius)
          return 0.0f;
        return (double) magnitude < (double) this.fullVolumeRadius ? this.volume : this.volume * (float) (1.0 - ((double) magnitude - (double) this.fullVolumeRadius) / ((double) this.soundRadius - (double) this.fullVolumeRadius));
      }
    }

    public AudioSettings() => this.audioType = AudioSettings.AudioType.SoundEffect;

    public AudioSettings(AudioClip clip)
    {
      this.clip = clip;
      this.audioType = AudioSettings.AudioType.SoundEffect;
    }

    public AudioSettings(AudioClip clip, float volume, bool loop)
    {
      this.clip = clip;
      this.volume = volume;
      this.loop = loop;
      this.audioType = AudioSettings.AudioType.SoundEffect;
    }

    public AudioSettings(AudioClip clip, float volume, float pitch)
    {
      this.clip = clip;
      this.volume = volume;
      this.pitch = pitch;
      this.audioType = AudioSettings.AudioType.SoundEffect;
    }

    public AudioSettings(AudioClip clip, float volume, float pitch, bool loop)
    {
      this.clip = clip;
      this.volume = volume;
      this.pitch = pitch;
      this.loop = loop;
      this.audioType = AudioSettings.AudioType.SoundEffect;
    }

    public AudioSettings(AudioClip clip, float volume, float pitch, bool loop, float delay)
    {
      this.clip = clip;
      this.volume = volume;
      this.pitch = pitch;
      this.loop = loop;
      this.delay = delay;
      this.audioType = AudioSettings.AudioType.SoundEffect;
    }

    public void EnableSpacialAwareness(
      float fullVolumeRadius,
      float radius,
      Transform worldPosition,
      Transform listenerPosition)
    {
      this.fullVolumeRadius = fullVolumeRadius;
      this.soundRadius = radius;
      this.worldPosition = worldPosition;
      this.listenerPosition = listenerPosition;
      this._useSpacialAwareness = true;
    }

    public void CopyToSource(ref AudioSource source)
    {
      source.clip = this.clip;
      source.volume = this.volume;
      source.pitch = this.pitch;
      source.loop = this.loop;
      source.ignoreListenerVolume = this.ignoreListenerVolume;
      source.velocityUpdateMode = this.velocityUpdateMode;
    }

    public AudioSettings Clone()
    {
      return new AudioSettings()
      {
        delay = this.delay,
        isAvailableForOverwrite = this.isAvailableForOverwrite,
        volume = this.volume,
        pitch = this.pitch,
        loop = this.loop,
        ignoreListenerVolume = this.ignoreListenerVolume,
        minVolume = this.minVolume,
        maxVolume = this.maxVolume,
        rolloffFactor = this.rolloffFactor,
        velocityUpdateMode = this.velocityUpdateMode
      };
    }

    public enum AudioType
    {
      SoundEffect,
      Music,
    }
  }
}
