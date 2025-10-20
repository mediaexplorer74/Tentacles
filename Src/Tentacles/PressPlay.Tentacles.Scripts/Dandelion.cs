// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Dandelion
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Dandelion : MovingCreature
  {
    public float maxAttackVictimDist = -1f;
    public float moveSpeed = 5f;
    public float checkLineOfSightInterval = 1f;
    private Transform victim;
    public float attackStateDuration;
    public float attackStateChangeTime;
    public Dandelion.DandelionAttackState attackState;
    public Dandelion.DandelionAttackState nextAttackState;

    public override void Start()
    {
      this.victim = LevelHandler.Instance.lemmy.transform;
      base.Start();
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      this.UpdateDandelionState();
    }

    protected override void UpdateStateSleeping() => base.UpdateStateSleeping();

    protected override void DoOnChangeStateActive()
    {
      base.DoOnChangeStateActive();
      this.ChangeDandelionState(Dandelion.DandelionAttackState.selectMovement);
    }

    protected void ChangeDandelionState(Dandelion.DandelionAttackState _state)
    {
      if (!this.isInitialized)
        this.Initialize();
      this.attackState = _state;
      this.attackStateDuration = -1f;
      this.attackStateChangeTime = Time.time;
      switch (this.attackState)
      {
        case Dandelion.DandelionAttackState.moveToLemmy:
          this.mover.SetVelocity((this.victim.position - this.transform.position).normalized * this.moveSpeed);
          break;
        case Dandelion.DandelionAttackState.moveToNode:
          this.creatureMover.LinearMoveToNode(this.currentTarget, new CreatureMover.MoveFinished(this.ReachedNodeCallback), this.moveSpeed);
          this.nextAttackState = Dandelion.DandelionAttackState.moveToNodeCheckLemmyLineOfSight;
          this.attackStateDuration = this.checkLineOfSightInterval;
          break;
        case Dandelion.DandelionAttackState.selectMovement:
          this.nextAttackState = Dandelion.DandelionAttackState.selectMovement;
          this.attackStateDuration = this.checkLineOfSightInterval;
          this.mover.SetVelocity(Vector3.zero);
          if (this.CheckClearPathToLemmy())
          {
            this.ChangeDandelionState(Dandelion.DandelionAttackState.moveToLemmy);
            break;
          }
          this.MoveToBestNode();
          break;
        case Dandelion.DandelionAttackState.moveToNodeCheckLemmyLineOfSight:
          if (this.CheckClearPathToLemmy())
          {
            this.ChangeDandelionState(Dandelion.DandelionAttackState.moveToLemmy);
            break;
          }
          this.ChangeDandelionState(Dandelion.DandelionAttackState.moveToNode);
          break;
      }
    }

    public void ReachedNodeCallback()
    {
      if (this.CheckClearPathToLemmy())
        this.ChangeDandelionState(Dandelion.DandelionAttackState.moveToLemmy);
      else
        this.MoveToBestNode();
    }

    protected void UpdateDandelionState()
    {
      if ((double) this.attackStateDuration != -1.0 && (double) Time.time > (double) this.attackStateChangeTime + (double) this.attackStateDuration)
        this.ChangeDandelionState(this.nextAttackState);
      switch (this.attackState)
      {
        case Dandelion.DandelionAttackState.moveToLemmy:
          this.mover.SetVelocity((this.victim.position - this.transform.position).normalized * this.moveSpeed);
          this.mover.DoMovement(Time.deltaTime);
          if (this.CheckClearPathToLemmy())
            break;
          this.ChangeDandelionState(Dandelion.DandelionAttackState.selectMovement);
          break;
      }
    }

    protected bool CheckClearPathToTargetNode()
    {
      return this.currentTarget != null && this.creatureMover.StandardFreePathCheck(this.currentTarget.transform.position);
    }

    protected bool CheckClearPathToLemmy()
    {
      return (double) (this.transform.position - this.victim.position).magnitude < (double) this.maxAttackVictimDist && this.creatureMover.StandardFreePathCheck(this.victim.position);
    }

    protected void MoveToBestNode()
    {
      this.currentTarget = this.creatureMover.GetBestNodeInSight(this.victim.position, this.transform.position, this.currentTarget, this.maxAttackVictimDist);
      if (this.currentTarget != null)
      {
        this.ChangeDandelionState(Dandelion.DandelionAttackState.moveToNode);
      }
      else
      {
        this.nextAttackState = Dandelion.DandelionAttackState.selectMovement;
        this.attackStateDuration = this.checkLineOfSightInterval;
      }
    }

    public enum DandelionAttackState
    {
      idle,
      moveToLemmy,
      moveToNode,
      selectMovement,
      moveToNodeCheckLemmyLineOfSight,
    }
  }
}
