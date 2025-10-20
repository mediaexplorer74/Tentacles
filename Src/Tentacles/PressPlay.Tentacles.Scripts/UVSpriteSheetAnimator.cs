// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UVSpriteSheetAnimator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class UVSpriteSheetAnimator : UVSpriteSheet
  {
    private UVSpriteSheetAnimator.AnimationCompleteCallback animCompleteCallback;
    public SpriteSheetAnim[] anims;
    private SpriteSheetAnim currentPlayingAnim;
    protected int currentFrameIndex;
    private float animTime;
    protected bool isPlaying;
    public string automaticPlayAnim;
    public bool automaticPlay;
    public bool randomizeAutomaticPlayStart;

    public override void Initialize()
    {
      if (this.isInitialized)
        return;
      base.Initialize();
      if (!this.automaticPlay || this.automaticPlayAnim == null)
        return;
      this.Play(this.automaticPlayAnim);
    }

    public void Stop() => this.isPlaying = false;

    public void Play(string _str)
    {
      if (!this.isInitialized)
        this.Initialize();
      for (int index = 0; index < this.anims.Length; ++index)
      {
        if (this.anims[index].name == _str)
        {
          this.Play(this.anims[index]);
          break;
        }
      }
    }

    public void Play(SpriteSheetAnim _anim)
    {
      if (_anim.startFrameIndex == _anim.endFrameIndex)
      {
        this.ShowFrameWithIndex(_anim.startFrameIndex);
        this.isPlaying = false;
      }
      else
      {
        this.isPlaying = true;
        this.ShowFrameWithIndex(_anim.startFrameIndex);
        this.currentPlayingAnim = _anim;
        this.animTime = 0.0f;
      }
    }

    public override void Update()
    {
      if (!this.isPlaying)
        return;
      this.UpdateAnim();
    }

    protected void UpdateAnim()
    {
      this.animTime += Time.deltaTime;
      int _index = (int) ((double) this.animTime / (double) this.currentPlayingAnim.timePerFrame) + this.currentPlayingAnim.startFrameIndex;
      if (_index > this.currentPlayingAnim.endFrameIndex)
      {
        this.HandleAnimationFinished();
      }
      else
      {
        if (_index == this.currentFrameIndex)
          return;
        this.ShowFrameWithIndex(_index);
      }
    }

    public void SetCurrentAnimFraction(float _fraction)
    {
      this.animTime = _fraction * this.currentPlayingAnim.length;
      this.UpdateAnim();
    }

    public void SetCurrentAnimTime(float _time)
    {
      this.animTime = _time;
      this.UpdateAnim();
    }

    public void ShowFrameWithIndex(int _index)
    {
      if (this.currentFrameIndex == _index)
        return;
      this.currentFrameIndex = _index;
      this.UpdateUVs(this.XPosFromIndex(_index), this.YPosFromIndex(_index));
    }

    private void HandleAnimationFinished()
    {
      switch (this.currentPlayingAnim.wrapMode)
      {
        case SpriteSheetAnim.WrapMode.clamp:
          this.isPlaying = false;
          if (this.animCompleteCallback == null)
            break;
          this.animCompleteCallback();
          break;
        case SpriteSheetAnim.WrapMode.loop:
          this.Play(this.currentPlayingAnim);
          break;
      }
    }

    public delegate void AnimationCompleteCallback();
  }
}
