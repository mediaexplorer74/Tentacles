// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperShooter
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperShooter : BasicBoss
  {
    public SuperShooter.SuperShooterType superShooterType;
    public SuperShooterTentacle tentaclePrefab;
    public GameObject[] tentaclePositions;
    public GameObject[] tentacleFinalPositions;
    public GameObject tentacleParent;
    private List<SuperShooterTentacle> tentacles = new List<SuperShooterTentacle>();
    public Collider[] damageColliders;
    public RifleWeapon weapon;
    public CannonScript cannon;
    public AudioWrapper sndLoop;
    public AudioWrapper sndHurt;
    public AudioWrapper sndHurtBigEye;
    public AudioWrapper sndAttack;
    public AudioWrapper sndBurst;
    private bool playingBurstSound;
    public AudioWrapper sndShoot;
    public string shootAnimation;
    private ShootPatternSuperShooter_v2 weaponPatternSuper = new ShootPatternSuperShooter_v2();
    private ShootPatternMiniSuperShooter weaponPatternMini = new ShootPatternMiniSuperShooter();
    public ShootPattern shootPattern;
    public Transform pushLemmyTowardsPoint;
    public float bossAttackDelay;
    public float bossPushLemmyDelay;
    public float bossPushLemmyDuration;
    public float pushLemmySmallForce;
    public float pushLemmyPersistentForce;
    public float pushLemmyDelay;
    public PPAnimationHandler anim;
    public Transform[] pushParticleEffectPositions;
    public PoolableParticle pushParticleEffect;
    public ParticleEmitter[] pushParticleEmitters;
    public float sequenceOffset;
    protected float progress;
    protected int currentShootPattern;

    private float sequenceLength => this.shootPattern.duration;

    private bool areAllTentaclesDead
    {
      get
      {
        foreach (SuperShooterTentacle tentacle in this.tentacles)
        {
          if (!tentacle.isDead)
            return false;
        }
        return true;
      }
    }

    public override void Start()
    {
      base.Start();
      this.weapon.Initialize((ABaseBehaviour) this, this.cannon);
      this.life.Initialize();
      this.SpawnTentacles();
      this.ChangeState(BaseCreature.CreatureState.active);
      this.ChangeBossState(BasicBoss.BossState.attack);
      this.shootPattern = this.superShooterType != SuperShooter.SuperShooterType.super ? this.weaponPatternMini.GetShootPattern(0, 0) : this.weaponPatternSuper.GetShootPattern(0, 0);
      this.anim.Play("idle");
      this.sndLoop.PlaySound();
    }

    protected override void UpdateStateDying()
    {
    }

    protected override void DoOnChangeStateDying()
    {
      this.anim.Play("death", new PPAnimationHandler.PPAnimationCallback(this.OnDeathAnimationComplete));
      this.sndDeath.PlaySound();
    }

    private void OnDeathAnimationComplete()
    {
      this.anim.Stop();
      this.ChangeState(BaseCreature.CreatureState.dead);
    }

    protected override void DoOnChangeStateDead() => base.DoOnChangeStateDead();

    public override void Update() => base.Update();

    public override void OnLemmyAttack()
    {
      if (this.life.isDead)
      {
        this.ChangeState(BaseCreature.CreatureState.dying);
      }
      else
      {
        ++this.level;
        this.shootPattern = this.superShooterType != SuperShooter.SuperShooterType.super ? this.weaponPatternMini.GotoNextLevel() : this.weaponPatternSuper.GotoNextLevel();
        this.sndHurtBigEye.PlaySound();
        this.anim.Play("damageReaction");
        this.anim.PlayQueued("idle", QueueMode.CompleteOthers);
        this.ContractTentacles();
        this.ChangeBossState(BasicBoss.BossState.angry);
      }
    }

    public void PlayDamageAnimation()
    {
      this.sndHurt.PlaySound();
      this.anim.PlayQueued("damageReaction", QueueMode.PlayNow);
      this.anim.PlayQueued("idle", QueueMode.CompleteOthers);
    }

    private void SpawnTentacles()
    {
      GameObject[] newNozzles = new GameObject[this.tentaclePositions.Length];
      int index = 0;
      foreach (GameObject tentaclePosition in this.tentaclePositions)
      {
        Vector3 vector3 = tentaclePosition.transform.localPosition * new Vector3(1f, -1f, -1f);
        tentaclePosition.transform.localPosition = vector3;
        SuperShooterTentacle superShooterTentacle = (SuperShooterTentacle) UnityObject.Instantiate((UnityObject) this.tentaclePrefab, tentaclePosition.transform.position, tentaclePosition.transform.rotation);
        superShooterTentacle.Initialize(this, this.tentaclePositions[index].transform, this.tentacleFinalPositions[index].transform);
        superShooterTentacle.transform.parent = this.tentacleParent.transform;
        this.tentacles.Add(superShooterTentacle);
        newNozzles[index] = superShooterTentacle.nozzle;
        ++index;
      }
      this.cannon.SetNozzles(newNozzles);
      this.tentaclePositions = (GameObject[]) null;
    }

    private void TogglePushParticles(bool on)
    {
      foreach (ParticleEmitter pushParticleEmitter in this.pushParticleEmitters)
        pushParticleEmitter.emit = on;
    }

    protected override void UpdateBossStateAttack()
    {
      base.UpdateBossStateAttack();
      this.progress = (LevelHandler.Instance.globalLevelTime - this.sequenceOffset) % this.sequenceLength;
      if ((double) LevelHandler.Instance.globalLevelTime > (double) this.sequenceOffset && (double) this.progress - (double) LevelHandler.Instance.globalLevelDeltaTime < 0.0)
        this.Shoot();
      if (!this.areAllTentaclesDead)
        return;
      this.ChangeBossState(BasicBoss.BossState.angry);
    }

    protected override void UpdateBossStateAngry()
    {
      if ((double) Time.time <= (double) this.lastBossStateChangeTime + (double) this.bossAttackDelay)
        return;
      this.ShakeAndPushLemmy((float) (-(double) this.pushLemmySmallForce * 30.0), this.pushLemmyTowardsPoint.position, 0.0f);
      this.anim.Play("earthquake", new PPAnimationHandler.PPAnimationCallback(this.onDamageReactionComplete));
      this.sndAttack.PlaySound();
      if (this.pushParticleEffect != null)
      {
        foreach (Transform particleEffectPosition in this.pushParticleEffectPositions)
          ObjectPool.Instance.Draw((PoolableObject) this.pushParticleEffect, particleEffectPosition.position, particleEffectPosition.rotation);
      }
      this.ChangeBossState(BasicBoss.BossState.exposed);
    }

    protected override void UpdateBossStateExposed()
    {
      if ((double) Time.time > (double) this.lastBossStateChangeTime + (double) this.bossPushLemmyDelay)
      {
        this.ShakeAndPushLemmy(-this.pushLemmyPersistentForce, this.pushLemmyTowardsPoint.position, 10f);
        this.TogglePushParticles(true);
        if (!this.playingBurstSound)
        {
          this.sndBurst.PlaySound();
          this.playingBurstSound = true;
        }
      }
      this.progress = (LevelHandler.Instance.globalLevelTime - this.sequenceOffset) % this.sequenceLength;
      if ((double) LevelHandler.Instance.globalLevelTime > (double) this.sequenceOffset && (double) this.progress - (double) LevelHandler.Instance.globalLevelDeltaTime < 0.0)
        this.Shoot();
      if (this.areAllTentaclesDead)
        this.ChangeBossState(BasicBoss.BossState.angry);
      if ((double) Time.time <= (double) this.lastBossStateChangeTime + (double) this.bossPushLemmyDuration + (double) this.bossPushLemmyDelay)
        return;
      this.TogglePushParticles(false);
      this.sndBurst.Stop();
      this.playingBurstSound = false;
      this.ChangeBossState(BasicBoss.BossState.attack);
    }

    protected override void DoOnChangeBossStateAngry() => this.life.SetVulnerability(false);

    protected override void DoOnChangeBossStateAttack() => this.life.SetVulnerability(true);

    protected override void DoOnChangeBossStateExposed()
    {
      base.DoOnChangeBossStateExposed();
      this.WakeUpTentacles();
    }

    private void ContractTentacles()
    {
      foreach (SuperShooterTentacle tentacle in this.tentacles)
        tentacle.Contract();
    }

    private void WakeUpTentacles()
    {
      foreach (SuperShooterTentacle tentacle in this.tentacles)
        tentacle.BringToLife();
    }

    private void onDamageReactionComplete()
    {
      this.anim.Play("idle");
      LevelHandler.Instance.lemmy.SetInvulnerable(1.5f);
    }

    protected override void Kill()
    {
      if ((bool) (UnityObject) this.createOnDeath)
        ObjectPool.Instance.Draw(this.createOnDeath, this.transform.position, this.transform.rotation);
      this.sndLoop.Stop();
      LevelHandler.Instance.levelSession.RegisterKill(this.pointsForKill, this.transform.position, (BaseCreature) this);
      this.SpawnPickups();
      this.DeactivateBoss();
      LevelHandler.Instance.cam.ShakeCamera(new Vector3(0.0f, 0.0f, 3f), 1f);
    }

    private void Shoot()
    {
      this.sndShoot.PlaySound();
      if (this.superShooterType == SuperShooter.SuperShooterType.super)
      {
        if (this.weaponPatternSuper.isEmpty)
          this.weaponPatternSuper.ResetPattern();
        this.shootPattern = this.weaponPatternSuper.GetNextShootPattern();
      }
      else
      {
        if (this.weaponPatternMini.isEmpty)
          this.weaponPatternMini.ResetPattern();
        this.shootPattern = this.weaponPatternMini.GetNextShootPattern();
      }
      for (int index = 0; index < this.shootPattern.shootPattern.Length; ++index)
      {
        if (this.shootPattern.shootPattern[index] && !this.tentacles[index].isDead && this.tentacles[index].isFullyExtended)
        {
          this.weapon.DoInstantFire(true, index, this.shootPattern.ammoType[index]);
          this.cannon.PlayNozzleAnimation(this.shootAnimation, index);
        }
      }
    }

    private void OnTurnOffAtDistance() => this.sndLoop.Stop();

    private void OnTurnOnAtDistance()
    {
      if (this.creatureState == BaseCreature.CreatureState.dead)
        return;
      this.sndLoop.PlaySound();
    }

    public enum SuperShooterType
    {
      super,
      mini,
    }
  }
}
