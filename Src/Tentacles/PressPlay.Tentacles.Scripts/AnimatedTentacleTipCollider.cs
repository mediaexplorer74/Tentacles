// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AnimatedTentacleTipCollider
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AnimatedTentacleTipCollider : BasicTentacleTipCollider
  {
    public UVSpriteSheetAnimator spriteSheetAnim;
    public PPAnimationHandler anim;
    public string doDamageAnim = "Damage";
    public string idleAnim = "Idle";

    protected override void DoOnHit(TentacleTip _tip, Vector3 _hitDir)
    {
      this.anim.Stop();
      this.anim.Play(this.doDamageAnim, new PPAnimationHandler.PPAnimationCallback(this.DoDamageAnimCallback));
      if (this.spriteSheetAnim == null)
        return;
      this.spriteSheetAnim.Play(this.doDamageAnim);
    }

    public void DoDamageAnimCallback()
    {
      this.anim.CrossFade(this.idleAnim, 1.4f, (PPAnimationHandler.PPAnimationCallback) null);
      if (this.spriteSheetAnim == null)
        return;
      this.spriteSheetAnim.Play(this.idleAnim);
    }
  }
}
