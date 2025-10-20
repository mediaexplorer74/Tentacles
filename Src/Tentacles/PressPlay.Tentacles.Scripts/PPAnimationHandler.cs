// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PPAnimationHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PPAnimationHandler : MonoBehaviour
  {
    private PPAnimationHandler.PPAnimationCallback OnCompleteCallback;
    [ContentSerializerIgnore]
    public Animation animationComponent;
    public int startFrame;
    public bool playAutomatically;
    public string clipToPlay;
    public PPAnimationClip[] clips;
    private AnimationClip currentClip;
    private bool isPlayingAnimation;
    private float startTime;
    private float duration;
    public bool destroyOnComplete;
    private bool _isInitialized;

    public float currentClipDuration => this.currentClip == null ? 0.0f : this.currentClip.length;

    public string currentClipName => this.currentClip == null ? "" : this.currentClip.name;

    public bool isInitialized => this._isInitialized;

    public override void Awake() => this.Initialize();

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this._isInitialized = true;
      if (this.animationComponent == null)
        this.animationComponent = this.GetComponentInChildren<Animation>();
      if (this.animationComponent.clip != null)
      {
        this.animationComponent.AddClip(this.animationComponent.clip, "default", this.startFrame - 1, this.startFrame);
        this.animationComponent["default"].wrapMode = PressPlay.FFWD.WrapMode.Default;
        this.animationComponent["default"].speed = 1f;
      }
      foreach (PPAnimationClip clip in this.clips)
      {
        if (!string.IsNullOrEmpty(clip.id))
        {
          this.animationComponent.AddClip(this.animationComponent.clip, clip.id, clip.firstFrame, clip.lastFrame, clip.addLoopFrame);
          this.animationComponent[clip.id].wrapMode = clip.wrapMode;
          this.animationComponent[clip.id].speed = clip.speed;
          if (clip.randomizeStartTime)
            this.animationComponent[clip.id].time = Random.Range(0.0f, this.animationComponent[clip.id].length);
        }
      }
      this.animationComponent.playAutomatically = this.playAutomatically;
      if (this.playAutomatically)
        this.animationComponent.Play(this.clipToPlay);
      else
        this.animationComponent.Play("default");
    }

    public override void Update()
    {
      if (!this.isPlayingAnimation || (double) Time.time <= (double) this.startTime + (double) this.duration)
        return;
      if (this.OnCompleteCallback != null)
        this.OnCompleteCallback();
      if (!this.destroyOnComplete)
        return;
      UnityObject.Destroy((UnityObject) this.gameObject);
    }

    public void CrossFade(string id, PPAnimationHandler.PPAnimationCallback callback)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.CrossFade(id);
      this.PrepareAnimationStart(callback);
    }

    public void CrossFade(
      string id,
      float fadeLength,
      PPAnimationHandler.PPAnimationCallback callback)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.CrossFade(id, fadeLength);
      this.PrepareAnimationStart(callback);
    }

    public void Blend(string id, PPAnimationHandler.PPAnimationCallback callback)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.Blend(id);
      this.PrepareAnimationStart(callback);
    }

    public void Blend(
      string id,
      float weight,
      float length,
      PPAnimationHandler.PPAnimationCallback callback)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.Blend(id, weight, length);
      this.PrepareAnimationStart(callback);
    }

    public void Stop()
    {
      if (this.animationComponent == null)
        return;
      this.animationComponent.Stop();
      this.isPlayingAnimation = false;
    }

    public void Stop(string name)
    {
      if (this.animationComponent == null)
        return;
      this.animationComponent.Stop(name);
      this.isPlayingAnimation = false;
    }

    public void Play(string id, PPAnimationHandler.PPAnimationCallback callback)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.Play(id);
      this.PrepareAnimationStart(callback);
    }

    public void Play(string id) => this.Play(id, (PPAnimationHandler.PPAnimationCallback) null);

    public void PlayQueued(string id, QueueMode mode)
    {
      if (this.animationComponent == null || this.animationComponent[id] == null)
        return;
      this.currentClip = this.GetAnimation(id);
      this.animationComponent.PlayQueued(id, mode);
    }

    public AnimationClip GetAnimation(string id) => this.animationComponent.GetClip(id);

    private void PrepareAnimationStart(PPAnimationHandler.PPAnimationCallback callback)
    {
      this.startTime = Time.time;
      this.duration = this.currentClip.length;
      this.OnCompleteCallback = callback;
      this.isPlayingAnimation = true;
    }

    public delegate void PPAnimationCallback();
  }
}
