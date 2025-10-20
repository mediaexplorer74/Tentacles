// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Gobbler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Gobbler : MovingCreature
  {
    private Transform victim;
    public float rotationSpeed;
    public float pullBackLength;
    public float pullBackDuration;
    public float forwardBurstSpeed;
    public float forwardBurstMaxDist;
    public float afterBurstIdleDuration;
    public float attackStateTime;
    public float attackStateChangeTime;
    public Gobbler.GobblerAttackState attackState;
    public Gobbler.GobblerAttackState nextAttackState;
    private CreatureMoverNode attackLemmyNode;

    public override void Activate() => base.Activate();

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      this.transform.DebugDrawLocal();
      this.UpdateAttackState();
    }

    protected override void UpdateStateSleeping() => base.UpdateStateSleeping();

    protected override void DoOnChangeStateActive()
    {
      base.DoOnChangeStateActive();
      this.victim = LevelHandler.Instance.lemmy.transform;
      this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.selectAttack);
    }

    private void ChangeGobblerAttackState(Gobbler.GobblerAttackState _state)
    {
      if (!this.isInitialized)
        this.Initialize();
      this.attackState = _state;
      this.attackStateTime = -1f;
      this.attackStateChangeTime = Time.time;
      switch (_state)
      {
        case Gobbler.GobblerAttackState.selectAttack:
          this.animHandler.CrossFade("idle", 0.1f, (PPAnimationHandler.PPAnimationCallback) null);
          this.SelectNodeToAttack();
          break;
        case Gobbler.GobblerAttackState.rotateToLemmy:
          this.animHandler.CrossFade("idle", 0.1f, (PPAnimationHandler.PPAnimationCallback) null);
          this.creatureMover.RotateToNode(this.currentTarget, new CreatureMover.MoveFinished(this.MoverCallback), this.rotationSpeed);
          break;
        case Gobbler.GobblerAttackState.rotateToNode:
          this.animHandler.CrossFade("idle", 0.1f, (PPAnimationHandler.PPAnimationCallback) null);
          this.creatureMover.RotateToNode(this.currentTarget, new CreatureMover.MoveFinished(this.MoverCallback), this.rotationSpeed);
          break;
        case Gobbler.GobblerAttackState.backUp:
          this.animHandler.CrossFade("swimPrepare", 0.1f, (PPAnimationHandler.PPAnimationCallback) null);
          this.creatureMover.BurstMove(-this.transform.forward * this.pullBackLength / this.pullBackDuration, 0.1f, new CreatureMover.MoveFinished(this.MoverCallback));
          this.nextAttackState = Gobbler.GobblerAttackState.pushForward;
          this.attackStateTime = this.pullBackDuration;
          break;
        case Gobbler.GobblerAttackState.pushForward:
          this.animHandler.CrossFade("swimStroke", 0.1f, (PPAnimationHandler.PPAnimationCallback) null);
          this.animHandler.Blend("swim", (PPAnimationHandler.PPAnimationCallback) null);
          this.creatureMover.BurstMoveToNode(this.currentTarget, new CreatureMover.MoveFinished(this.MoverCallback), this.forwardBurstSpeed);
          break;
        case Gobbler.GobblerAttackState.idleAfterPushForward:
          this.animHandler.CrossFade("idle", 2f, (PPAnimationHandler.PPAnimationCallback) null);
          this.nextAttackState = Gobbler.GobblerAttackState.selectAttack;
          this.attackStateTime = this.afterBurstIdleDuration;
          break;
      }
    }

    private void UpdateAttackState()
    {
      if ((double) this.attackStateTime != -1.0 && (double) Time.time > (double) this.attackStateChangeTime + (double) this.attackStateTime)
        this.ChangeGobblerAttackState(this.nextAttackState);
      switch (this.attackState)
      {
      }
    }

    public void MoverCallback()
    {
      switch (this.attackState)
      {
        case Gobbler.GobblerAttackState.rotateToLemmy:
          this.ray.origin = this.transform.position;
          this.ray.direction = this.transform.forward;
          if (this.victim.collider.Raycast(this.ray, out this.rh, this.forwardBurstMaxDist))
          {
            this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.backUp);
            break;
          }
          this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.selectAttack);
          break;
        case Gobbler.GobblerAttackState.rotateToNode:
          this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.backUp);
          break;
        case Gobbler.GobblerAttackState.pushForward:
          this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.idleAfterPushForward);
          break;
      }
    }

    private void SelectNodeToAttack()
    {
      if ((double) (this.transform.position - this.victim.position).magnitude < (double) this.forwardBurstMaxDist && this.creatureMover.StandardFreePathCheck(this.victim.position))
      {
        if (this.attackLemmyNode == null)
          this.attackLemmyNode = CreatureToolbox.CreateCreatureMoverNode();
        this.attackLemmyNode.transform.position = this.victim.transform.position;
        this.currentTarget = this.attackLemmyNode;
        this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.rotateToLemmy);
      }
      else
      {
        CreatureMoverNode bestNodeInSight = this.creatureMover.GetBestNodeInSight(LevelHandler.Instance.lemmy.transform.position, this.transform.position, this.currentTarget, this.forwardBurstMaxDist);
        if (bestNodeInSight == null)
        {
          this.attackStateTime = 1f;
          this.nextAttackState = Gobbler.GobblerAttackState.selectAttack;
        }
        else
        {
          this.currentTarget = bestNodeInSight;
          this.ChangeGobblerAttackState(Gobbler.GobblerAttackState.rotateToNode);
        }
      }
    }

    public override void OnLemmyAttack()
    {
      this.animHandler.Play("takeDamage", (PPAnimationHandler.PPAnimationCallback) null);
      base.OnLemmyAttack();
    }

    public enum GobblerAttackState
    {
      selectAttack,
      rotateToLemmy,
      rotateToNode,
      backUp,
      pushForward,
      idleAfterPushForward,
    }
  }
}
