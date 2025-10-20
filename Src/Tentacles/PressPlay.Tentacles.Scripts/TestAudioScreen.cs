// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TestAudioScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TestAudioScreen : MenuSceneScreen
  {
    private bool isInitialized;
    private List<AudioClip> clips = new List<AudioClip>();
    private float counter;

    public TestAudioScreen()
      : base("")
    {
    }

    public override void LoadContent()
    {
      Application.LoadLevel("Scenes/DesatGreen_intro");
      new GameObject("AudioManager").AddComponent<AudioManager>(new AudioManager());
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/LemmyScreamPitch#05")));
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/LemmyScreamPitch#06")));
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/LemmyScreamPitch#07")));
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/TentThrowSplat1")));
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/TentThrowSplat2")));
      this.clips.Add(new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/TentThrowSplat3")));
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      if (!this.isInitialized)
      {
        LevelHandler objectOfType = (LevelHandler) UnityObject.FindObjectOfType(typeof (LevelHandler));
        AudioClip audioClip1 = new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/BrainLoopV2part1"));
        AudioClip audioClip2 = new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/BrainLoopV2part2"));
        AudioClip audioClip3 = new AudioClip(ContentHelper.Content.Load<SoundEffect>("Sounds/BrainLoopV2part3"));
        objectOfType.musicController = new MusicController();
        objectOfType.gameObject.AddComponent<MusicController>(objectOfType.musicController);
        this.isInitialized = true;
      }
      if ((double) this.counter > 0.699999988079071)
      {
        AudioManager.Instance.Play(this.clips[Random.Range(0, this.clips.Count)]);
        this.counter = 0.0f;
      }
      this.counter += Time.deltaTime;
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
