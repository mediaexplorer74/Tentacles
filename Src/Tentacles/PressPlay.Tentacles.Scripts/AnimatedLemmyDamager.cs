// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AnimatedLemmyDamager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AnimatedLemmyDamager : BasicLemmyDamager
  {
    public PPAnimationHandler anim;
    public string doDamageAnim = "Damage";
    public string idleAnim = "Idle";

    internal override void DoOnHitLemmy(Vector3 _hitDir, Vector3 _position)
    {
      base.DoOnHitLemmy(_hitDir, _position);
      this.anim.CrossFade(this.doDamageAnim, new PPAnimationHandler.PPAnimationCallback(this.DoDamageAnimCallback));
    }

    public void DoDamageAnimCallback()
    {
      if (this.isLemmyTouching)
        this.anim.CrossFade(this.doDamageAnim, (PPAnimationHandler.PPAnimationCallback) null);
      else
        this.anim.CrossFade(this.idleAnim, (PPAnimationHandler.PPAnimationCallback) null);
    }
  }
}
