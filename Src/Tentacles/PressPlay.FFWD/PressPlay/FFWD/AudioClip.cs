// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.AudioClip
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace PressPlay.FFWD
{
  public class AudioClip : Asset
  {
    [ContentSerializer(Optional = true)]
    public string clip;
    [ContentSerializerIgnore]
    private SoundEffect sound;
    private bool loopSet;

    public float length => this.sound == null ? 0.0f : (float) this.sound.Duration.TotalSeconds;

    public AudioClip()
    {
    }

    public AudioClip(SoundEffect sound)
    {
      this.sound = sound;
      if (sound == null)
        return;
      this.Instance = sound.CreateInstance();
      this.loopSet = false;
    }

    protected override void DoLoadAsset(AssetHelper assetHelper)
    {
      if (this.sound != null)
        return;
      this.sound = assetHelper.Load<SoundEffect>("Sounds/" + this.clip);
      this.Instance = this.sound.CreateInstance();
      this.loopSet = false;
      this.name = this.clip;
    }

    internal SoundEffectInstance Instance { get; private set; }

    internal void Loop(bool loop)
    {
      if (this.loopSet || this.Instance == null)
        return;
      this.loopSet = true;
      this.Instance.IsLooped = loop;
    }
  }
}
