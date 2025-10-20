// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DamageFeedbackMeshAndSpriteSheet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DamageFeedbackMeshAndSpriteSheet : DamageFeedback
  {
    public UVSpriteSheetAnimator spriteSheetAnim;
    public PPAnimationHandler meshAnim;
    public string doDamageAnim = "Damage";
    public string idleAnim = "Idle";

    public override void DoOnHitLemmy(Vector3 _hitDir, Vector3 _position)
    {
      base.DoOnHitLemmy(_hitDir, _position);
      this.meshAnim.CrossFade(this.doDamageAnim, new PPAnimationHandler.PPAnimationCallback(this.DoDamageAnimCallback));
      if (!(bool) (UnityObject) this.spriteSheetAnim)
        return;
      this.spriteSheetAnim.Play(this.doDamageAnim);
    }

    public void DoDamageAnimCallback()
    {
      this.meshAnim.CrossFade(this.idleAnim, (PPAnimationHandler.PPAnimationCallback) null);
      if (!(bool) (UnityObject) this.spriteSheetAnim)
        return;
      this.spriteSheetAnim.Play(this.idleAnim);
    }
  }
}
