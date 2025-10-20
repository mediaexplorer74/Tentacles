// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicBoss
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicBoss : BaseCreature
  {
    public GameObject[] ObjectsToDeactivateOnDeath;
    public BossSounds soundFX;
    protected BasicBoss.BossState bossState;
    protected float lastBossStateChangeTime;
    protected BasicBoss.BossState lastBossState;
    protected int level;

    protected void ChangeBossState(BasicBoss.BossState state)
    {
      switch (state)
      {
        case BasicBoss.BossState.attack:
          this.DoOnChangeBossStateAttack();
          break;
        case BasicBoss.BossState.exposed:
          this.DoOnChangeBossStateExposed();
          break;
        case BasicBoss.BossState.angry:
          this.DoOnChangeBossStateAngry();
          break;
      }
      this.lastBossStateChangeTime = Time.time;
      this.lastBossState = state;
      this.bossState = state;
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      switch (this.bossState)
      {
        case BasicBoss.BossState.attack:
          this.UpdateBossStateAttack();
          break;
        case BasicBoss.BossState.exposed:
          this.UpdateBossStateExposed();
          break;
        case BasicBoss.BossState.angry:
          this.UpdateBossStateAngry();
          break;
      }
    }

    protected virtual void DoOnChangeBossStateAttack()
    {
    }

    protected virtual void DoOnChangeBossStateAngry()
    {
    }

    protected virtual void DoOnChangeBossStateExposed()
    {
    }

    protected virtual void UpdateBossStateAttack()
    {
    }

    protected virtual void UpdateBossStateAngry()
    {
    }

    protected virtual void UpdateBossStateExposed()
    {
    }

    protected override void DoOnChangeStateDying()
    {
      this.ChangeState(BaseCreature.CreatureState.dead);
    }

    protected void ShakeAndPushLemmy(float force, Vector3 position, float minDistToPoint)
    {
      LevelHandler.Instance.lemmy.BreakConnections();
      Vector3 vector3 = LevelHandler.Instance.lemmy.transform.position - position;
      if ((double) vector3.sqrMagnitude <= (double) minDistToPoint * (double) minDistToPoint)
        return;
      LevelHandler.Instance.lemmy.rigidbody.AddForce(vector3.normalized * force * Time.deltaTime);
    }

    protected override void Kill()
    {
      if (this.sndDeath.sounds.Length > 0)
        this.sndDeath.PlaySound();
      LevelHandler.Instance.levelSession.RegisterKill(this.pointsForKill, this.transform.position, (BaseCreature) this);
      if ((bool) (UnityObject) this.createOnDeath)
        ObjectPool.Instance.Draw(this.createOnDeath, this.transform.position, this.transform.rotation);
      this.SpawnPickups();
      this.DeactivateBoss();
    }

    public override void Activate()
    {
    }

    public override void DeActivate()
    {
    }

    protected void DeactivateBoss()
    {
      if (this.ObjectsToDeactivateOnDeath.Length <= 0 || !this.ObjectsToDeactivateOnDeath[0].active)
        return;
      for (int index = 0; index < this.ObjectsToDeactivateOnDeath.Length; ++index)
        this.ObjectsToDeactivateOnDeath[index].SetActiveRecursively(false);
    }

    public override void OnLemmyAttack() => base.OnLemmyAttack();

    public enum BossState
    {
      attack,
      exposed,
      angry,
    }
  }
}
