// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AudioManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AudioManager : MonoBehaviour
  {
    private static int id = 0;
    private static AudioManager instance;
    private GameObject listenerObject;
    private Transform followObject;
    public int numberOfSoundChannels = 16;
    public float defaultVolume = 0.5f;
    public bool soundIsOn = true;
    [ContentSerializerIgnore]
    private bool _soundIsEnabled = true;
    [ContentSerializerIgnore]
    private bool _musicIsEnabled = true;
    public bool autoExpandCapacity = true;
    public int channelsToIncrementOnExpand = 16;
    public int maxSoundChannelCapacity = 512;
    public float allowedOverwritePercentage = 90f;
    private AudioSource[] sources;
    private AudioObject[] sounds;
    private List<AudioObject> soundsReadyToDie = new List<AudioObject>();
    public int soundsPlaying;
    public static bool isLoaded = false;

    public static int nextId
    {
      get
      {
        ++AudioManager.id;
        return AudioManager.id - 1;
      }
    }

    [ContentSerializerIgnore]
    public bool soundIsEnabled
    {
      get => this._soundIsEnabled;
      set
      {
        this._soundIsEnabled = value;
        if (value)
          this.TurnOnAllSounds(AudioSettings.AudioType.SoundEffect);
        else
          this.TurnOffAllSounds(AudioSettings.AudioType.SoundEffect);
      }
    }

    [ContentSerializerIgnore]
    public bool musicIsEnabled
    {
      get => this._musicIsEnabled;
      set
      {
        this._musicIsEnabled = value;
        if (value)
          this.TurnOnAllSounds(AudioSettings.AudioType.Music);
        else
          this.TurnOffAllSounds(AudioSettings.AudioType.Music);
      }
    }

    public static AudioManager Instance
    {
      get
      {
        if (AudioManager.instance == null)
          Debug.LogError("Attempt to access instance of AudioManager earlier than Start or without it being attached to a GameObject.");
        return AudioManager.instance;
      }
    }

    public override void Awake()
    {
      if (AudioManager.instance != null)
      {
        Debug.LogError("Cannot have two instances of AudioManager. Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        this.Init();
        AudioManager.instance = this;
        AudioManager.isLoaded = true;
      }
    }

    private void Init()
    {
      this.listenerObject = new GameObject("AudioManager Listener");
      this.listenerObject.transform.parent = this.transform;
      this.sources = new AudioSource[this.numberOfSoundChannels];
      this.sounds = new AudioObject[this.numberOfSoundChannels];
      for (int index = 0; index < this.numberOfSoundChannels; ++index)
      {
        this.sources[index] = this.listenerObject.AddComponent<AudioSource>(new AudioSource());
        this.sources[index].volume = this.defaultVolume;
        this.sounds[index] = new AudioObject();
      }
    }

    public override void Update()
    {
      if (this.soundIsEnabled != GlobalManager.Instance.currentProfile.soundIsEnabled)
        this.soundIsEnabled = GlobalManager.Instance.currentProfile.soundIsEnabled;
      if (this.musicIsEnabled != GlobalManager.Instance.currentProfile.musicIsEnabled)
        this.musicIsEnabled = GlobalManager.Instance.currentProfile.musicIsEnabled;
      if (this.followObject != null)
        this.listenerObject.transform.position = this.followObject.position;
      this.MaintainSounds();
    }

    public void SetFollowObject(Transform followObject) => this.followObject = followObject;

    public void ClearFollowObject() => this.followObject = (Transform) null;

    private void MaintainSounds()
    {
      this.soundsReadyToDie.Clear();
      this.soundsPlaying = 0;
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        AudioObject sound = this.sounds[index];
        if (sound.isPlaying)
          ++this.soundsPlaying;
        sound.Update();
        if (!sound.isLocked)
        {
          if (!sound.isPlaying)
            sound.Destroy();
          else if (sound.isAvailableForOverwrite && (double) sound.percentage * 100.0 > (double) this.allowedOverwritePercentage)
            this.soundsReadyToDie.Add(sound);
        }
      }
    }

    public void TurnOffAllSounds(AudioSettings.AudioType audioType)
    {
      if (this.sounds == null)
        return;
      foreach (AudioObject sound in this.sounds)
      {
        if (sound.settings != null && sound.settings.audioType == audioType)
        {
          sound.doUpdate = false;
          sound.volume = 0.0f;
        }
      }
    }

    public void TurnOnAllSounds(AudioSettings.AudioType audioType)
    {
      if (this.sounds == null)
        return;
      foreach (AudioObject sound in this.sounds)
      {
        if (sound.settings != null && sound.settings.audioType == audioType)
        {
          sound.doUpdate = true;
          sound.volume = 1f;
        }
      }
    }

    public void FadeAllSounds(float volume, float duration)
    {
      if (this.sounds == null)
        return;
      foreach (AudioObject sound in this.sounds)
      {
        if (sound.settings != null)
          sound.Fade(volume, duration);
      }
    }

    public void OnApplicationQuit() => AudioManager.instance = (AudioManager) null;

    public void DoOverride(int priority, float duration, float factor)
    {
      this.DoOverride(priority, duration, factor, 0.3f, 0.3f);
    }

    public void DoOverride(
      int priority,
      float duration,
      float factor,
      float fadeInTime,
      float fadeOutTime)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].priority < priority)
          this.sounds[index].DoSoundOverride(duration, factor, fadeInTime, fadeOutTime);
      }
    }

    public void DoOverride(string ignoreCategory, float duration, float factor)
    {
      this.DoOverride(ignoreCategory, duration, factor, 0.3f, 0.3f);
    }

    public void DoOverride(
      string ignoreCategory,
      float duration,
      float factor,
      float fadeInTime,
      float fadeOutTime)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].category != ignoreCategory)
          this.sounds[index].DoSoundOverride(duration, factor, fadeInTime, fadeOutTime);
      }
    }

    public AudioObject Get(string category)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].category == category)
          return this.sounds[index];
      }
      return (AudioObject) null;
    }

    public bool IsCategoryPlaying(string category)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index].settings != null && this.sounds[index].category == category && this.sounds[index].isPlaying)
          return true;
      }
      return false;
    }

    public AudioObject Get(int id)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index].id == id)
          return this.sounds[index];
      }
      Debug.LogWarning("AudioManager::The Audio with ID: " + (object) id + " doesn't exist anymore");
      return (AudioObject) null;
    }

    public AudioObject Add(AudioSettings settings, string _category, int _priority)
    {
      if (settings.clip == null)
        return (AudioObject) null;
      int readyAudioSource = this.GetReadyAudioSource();
      if (readyAudioSource != -1)
      {
        settings.id = AudioManager.nextId;
        this.sounds[readyAudioSource].Reset(settings, this.sources[readyAudioSource], _category, _priority);
        this.sounds[readyAudioSource].isLocked = true;
        this.sounds[readyAudioSource].index = readyAudioSource;
        return this.sounds[readyAudioSource];
      }
      Debug.Log((object) "AudioManager couldn't find available AudioSource. Aborting play");
      return (AudioObject) null;
    }

    public int Play(AudioClip _clip)
    {
      return !this.soundIsEnabled ? -1 : this.Play(new AudioSettings(_clip, this.defaultVolume, false), "default", 0);
    }

    public int Play(AudioSettings settings)
    {
      return settings.audioType == AudioSettings.AudioType.SoundEffect && !this.soundIsEnabled || settings.audioType == AudioSettings.AudioType.Music && !this.musicIsEnabled ? -1 : this.Play(settings, "default", 0);
    }

    public int Play(AudioSettings settings, string _category, int _priority)
    {
      if (settings.clip == null)
      {
        Debug.LogWarning("The AudioClip you are trying to play is null (" + (object) settings + ")");
        return -1;
      }
      if (settings.audioType == AudioSettings.AudioType.SoundEffect && !this.soundIsEnabled || settings.audioType == AudioSettings.AudioType.Music && !this.musicIsEnabled)
        return -1;
      int readyAudioSource = this.GetReadyAudioSource();
      if (readyAudioSource != -1)
      {
        settings.id = AudioManager.nextId;
        this.sounds[readyAudioSource].Reset(settings, this.sources[readyAudioSource], _category, _priority);
        this.sounds[readyAudioSource].index = readyAudioSource;
        this.sounds[readyAudioSource].Play();
        return settings.id;
      }
      Debug.LogWarning("AudioManager couldn't find available AudioSource. Aborting play");
      return -1;
    }

    public void Stop(int id) => this.Get(id)?.Stop();

    public void Stop(string category)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].category == category)
          this.sounds[index].Stop();
      }
    }

    public void PauseAllSounds(AudioSettings.AudioType audioType)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].settings.audioType == audioType && this.sounds[index].isPlaying)
          this.sounds[index].Pause();
      }
    }

    public void PauseAllSounds()
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].isPlaying)
          this.sounds[index].Pause();
      }
    }

    public void UnPauseAllSounds()
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].isPlaying)
          this.sounds[index].UnPause();
      }
    }

    public void UnPauseAllSounds(AudioSettings.AudioType audioType)
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].settings.audioType == audioType && this.sounds[index].isPlaying)
          this.sounds[index].UnPause();
      }
    }

    public void StopAllSounds()
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null && this.sounds[index].isPlaying)
          this.sounds[index].Stop();
      }
    }

    public void DestroyAllSounds()
    {
      for (int index = 0; index < this.sounds.Length; ++index)
      {
        if (this.sounds[index] != null)
          this.sounds[index].Destroy();
      }
      this.Reset();
    }

    private void Reset()
    {
      AudioManager.id = 0;
      foreach (UnityObject source in this.sources)
        UnityObject.Destroy(source);
      this.Init();
    }

    private int GetReadyAudioSource()
    {
      for (int readyAudioSource = 0; readyAudioSource < this.sounds.Length; ++readyAudioSource)
      {
        if (this.sounds[readyAudioSource] == null || !this.sounds[readyAudioSource].isLocked && !this.sounds[readyAudioSource].isPlaying)
          return readyAudioSource;
      }
      if (!this.autoExpandCapacity || this.sources.Length >= this.maxSoundChannelCapacity)
        return this.GetAudioSourceFromOverwriteList();
      this.AddSources();
      return this.GetReadyAudioSource();
    }

    private int GetAudioSourceFromOverwriteList()
    {
      float num = this.allowedOverwritePercentage;
      int fromOverwriteList = -1;
      for (int index = 0; index < this.soundsReadyToDie.Count; ++index)
      {
        AudioObject audioObject = this.soundsReadyToDie[index];
        if ((double) audioObject.percentage > (double) num)
        {
          num = audioObject.percentage;
          fromOverwriteList = audioObject.index;
        }
      }
      return fromOverwriteList;
    }

    private void AddSources()
    {
      int length = Mathf.Min(this.numberOfSoundChannels + this.channelsToIncrementOnExpand, this.maxSoundChannelCapacity);
      AudioSource[] audioSourceArray = new AudioSource[length];
      AudioObject[] audioObjectArray = new AudioObject[length];
      for (int index = 0; index < length; ++index)
      {
        if (index < this.numberOfSoundChannels)
        {
          audioSourceArray[index] = this.sources[index];
          audioObjectArray[index] = this.sounds[index];
        }
        else
        {
          audioSourceArray[index] = this.gameObject.AddComponent<AudioSource>(new AudioSource());
          audioSourceArray[index].volume = this.defaultVolume;
          audioObjectArray[index] = new AudioObject();
        }
      }
      this.sources = audioSourceArray;
      this.sounds = audioObjectArray;
      this.numberOfSoundChannels = length;
    }
  }
}
