// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MainBody
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MainBody : MonoBehaviour
  {
    public Transform gfxTransform;
    private Quaternion oldRotation;
    public PPAnimationHandler anim;
    public UVSpriteSheetAnimator bodySpriteAnim;
    private float stateLength;
    private float stateChangeTime;
    private MainBody.State lastState;
    private MainBody.State state;
    private MainBody.State nextState;

    public bool isAnimatingTakingDamage => this.state == MainBody.State.animatingTakingDamage;

    public override void Start()
    {
      this.LookRight();
      this.ChangeState(MainBody.State.neutral);
    }

    private void ChangeState(MainBody.State _state)
    {
      this.HandleExitState(_state, this.state);
      this.lastState = this.state;
      this.state = _state;
      this.stateChangeTime = Time.time;
      this.stateLength = -1f;
      switch (this.state)
      {
        case MainBody.State.neutral:
          this.anim.Play("idleCalm");
          break;
        case MainBody.State.animatingTakingDamage:
          this.anim.Play("takeDamage");
          this.nextState = MainBody.State.neutral;
          this.stateLength = 0.8f;
          break;
        case MainBody.State.openMouth:
          this.anim.Play("eatStart", (PPAnimationHandler.PPAnimationCallback) null);
          break;
        case MainBody.State.chew:
          this.anim.Play("eatChew", new PPAnimationHandler.PPAnimationCallback(this.AnimationDoneCallback));
          this.nextState = MainBody.State.closeMouth;
          break;
        case MainBody.State.closeMouth:
          this.anim.Play("eatReturn", new PPAnimationHandler.PPAnimationCallback(this.AnimationDoneCallback));
          this.nextState = MainBody.State.neutral;
          break;
      }
    }

    private void AnimationDoneCallback() => this.ChangeState(this.nextState);

    private void HandleExitState(MainBody.State _oldState, MainBody.State _newState)
    {
      switch (_oldState)
      {
      }
    }

    private void UpdateState()
    {
      if ((double) this.stateLength != -1.0 && (double) Time.time > (double) this.stateChangeTime + (double) this.stateLength)
        this.ChangeState(this.nextState);
      switch (this.state)
      {
        case MainBody.State.animatingTakingDamage:
          if ((double) Time.time <= (double) this.stateChangeTime + (double) this.stateLength)
            break;
          this.ChangeState(this.nextState);
          break;
      }
    }

    public override void FixedUpdate()
    {
      this.oldRotation = this.transform.rotation;
      this.transform.LookAt(this.transform.position + LevelHandler.Instance.cam.GetForwardDirection());
      this.transform.rotation = Quaternion.Lerp(this.oldRotation, this.transform.rotation, Time.deltaTime * 2f);
      this.UpdateState();
    }

    public void LookUp() => this.gfxTransform.localEulerAngles = Vector3.zero;

    public void LookRight() => this.gfxTransform.localEulerAngles = Vector3.right * 30f;

    public void SetHealthFraction(float _fraction)
    {
      this.bodySpriteAnim.Play("Damage");
      this.bodySpriteAnim.SetCurrentAnimFraction(Mathf.Pow(1f - _fraction, 0.7f));
      this.bodySpriteAnim.Stop();
      int num = this.isAnimatingTakingDamage ? 1 : 0;
    }

    public void StartTakeDamageAnimation()
    {
      this.ChangeState(MainBody.State.animatingTakingDamage);
    }

    public void OpenMouth()
    {
      if (this.state == MainBody.State.openMouth || this.state == MainBody.State.chew)
        return;
      this.ChangeState(MainBody.State.openMouth);
    }

    public void Chew() => this.ChangeState(MainBody.State.chew);

    public enum State
    {
      neutral,
      excited,
      animatingTakingDamage,
      openMouth,
      chew,
      closeMouth,
    }
  }
}
