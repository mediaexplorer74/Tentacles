// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PoolableAnimation
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PoolableAnimation : PoolableObject
  {
    public PPAnimationHandler animHandler;
    public string anim;

    public override void Activate()
    {
      base.Activate();
      if (this.animHandler == null)
        this.animHandler = this.GetComponent<PPAnimationHandler>();
      if (this.animHandler == null)
        this.animHandler = this.GetComponentInChildren<PPAnimationHandler>();
      if (!this.animHandler.isInitialized)
        this.animHandler.Initialize();
      this.animHandler.Play(this.anim, new PPAnimationHandler.PPAnimationCallback(this.AnimationDoneCallback));
    }

    public void AnimationDoneCallback() => this.Return();
  }
}
