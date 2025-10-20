// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BaseCreature
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BaseCreature : ABaseBehaviour
  {
    protected BaseCreature.CreatureState creatureState;
    public AudioWrapper sndDeath;
    public PoolableObject createOnDeath;
    public int pointsForKill;
    public int pointsForEye;
    public float multiplierFactor;
    public int numberOfPointPickups;
    public bool shootPickupsInLemmyDirection;
    public Transform[] spawnPickupPositions;
    [ContentSerializerIgnore]
    public EnergyContainer life;
    [ContentSerializerIgnore]
    public ObjectMover mover;
    [ContentSerializerIgnore]
    public PPAnimationHandler animHandler;
    protected float lastStateChangeTime;
    protected BaseCreature.CreatureState lastState;
    protected float killDelay = -1f;
    protected float killTime;
    public bool isInitialized;

    public override void Start() => this.Initialize();

    public void Initialize()
    {
      if (this.isInitialized)
        return;
      this.life = this.gameObject.GetComponent<EnergyContainer>();
      this.life.Initialize();
      this.mover = this.GetComponent<ObjectMover>();
      if (this.animHandler == null)
      {
        this.animHandler = this.GetComponent<PPAnimationHandler>();
        if (this.animHandler == null)
          this.animHandler = this.GetComponentInChildren<PPAnimationHandler>();
      }
      Collider[] componentsInChildren = this.GetComponentsInChildren<Collider>();
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        if (componentsInChildren[index].connectedBody != null && componentsInChildren[index].connectedBody.BodyType == BodyType.Static)
        {
          componentsInChildren[index].connectedBody.BodyType = BodyType.Kinematic;
          componentsInChildren[index].allowTurnOff = true;
        }
      }
      this.isInitialized = true;
    }

    public override void Update()
    {
      if (this.mover != null)
        this.mover.DoMovement(Time.deltaTime);
      switch (this.creatureState)
      {
        case BaseCreature.CreatureState.sleeping:
          this.UpdateStateSleeping();
          break;
        case BaseCreature.CreatureState.waking:
          this.UpdateStateWaking();
          break;
        case BaseCreature.CreatureState.active:
          this.UpdateStateActive();
          break;
        case BaseCreature.CreatureState.dying:
          this.UpdateStateDying();
          break;
        case BaseCreature.CreatureState.dead:
          this.UpdateStateDead();
          break;
      }
      if ((double) this.killDelay == -1.0)
        return;
      if ((double) this.killTime > (double) this.killDelay)
      {
        this.ChangeState(BaseCreature.CreatureState.dying);
        this.killDelay = -1f;
      }
      this.killTime += Time.deltaTime;
    }

    protected virtual void UpdateStateSleeping()
    {
    }

    protected virtual void UpdateStateWaking()
    {
    }

    protected virtual void UpdateStateActive()
    {
    }

    protected virtual void UpdateStateDying() => this.ChangeState(BaseCreature.CreatureState.dead);

    protected virtual void UpdateStateDead()
    {
    }

    protected void ChangeState(BaseCreature.CreatureState state)
    {
      this.lastStateChangeTime = Time.time;
      this.lastState = this.creatureState;
      this.creatureState = state;
      switch (this.creatureState)
      {
        case BaseCreature.CreatureState.sleeping:
          this.DoOnChangeStateSleeping();
          break;
        case BaseCreature.CreatureState.waking:
          this.DoOnChangeStateWaking();
          break;
        case BaseCreature.CreatureState.active:
          this.DoOnChangeStateActive();
          break;
        case BaseCreature.CreatureState.dying:
          this.DoOnChangeStateDying();
          break;
        case BaseCreature.CreatureState.dead:
          if (this.lastState != BaseCreature.CreatureState.dead)
            this.Kill();
          this.DoOnChangeStateDead();
          break;
      }
    }

    protected virtual void DoOnChangeStateSleeping()
    {
    }

    protected virtual void DoOnChangeStateWaking()
    {
    }

    protected virtual void DoOnChangeStateActive()
    {
    }

    protected virtual void DoOnChangeStateDying()
    {
      this.ChangeState(BaseCreature.CreatureState.dead);
    }

    protected virtual void DoOnChangeStateDead()
    {
    }

    protected virtual void Kill(float delay) => this.killDelay = delay;

    protected virtual void Kill()
    {
      if (this.sndDeath.sounds.Length > 0)
        this.sndDeath.PlaySound();
      LevelHandler.Instance.levelSession.RegisterKill(this.pointsForKill, this.transform.position, this);
      if ((bool) (UnityObject) this.createOnDeath)
        ObjectPool.Instance.Draw(this.createOnDeath, this.transform.position, this.transform.rotation);
      this.SpawnPickups();
      UnityObject.Destroy((UnityObject) this.gameObject);
    }

    public virtual void SpawnPickups()
    {
      for (int index = 0; index < this.numberOfPointPickups; ++index)
      {
        Transform pickupSpawnPosition = this.GetPickupSpawnPosition();
        PoolablePickup poolablePickup = (PoolablePickup) ObjectPool.Instance.Draw((PoolableObject) LevelHandler.Instance.enemyDeathPickup, pickupSpawnPosition.position, pickupSpawnPosition.rotation);
        if (this.shootPickupsInLemmyDirection)
          poolablePickup.MoveTowardsLemmy();
      }
    }

    private Transform GetPickupSpawnPosition()
    {
      return this.spawnPickupPositions.Length > 0 ? this.spawnPickupPositions[Random.Range(0, this.spawnPickupPositions.Length)] : this.transform;
    }

    public virtual void OnLemmyAttack()
    {
      if (this.life.isDead)
      {
        this.ChangeState(BaseCreature.CreatureState.dying);
      }
      else
      {
        int pointsForEye = this.pointsForEye;
        if (this.creatureState != BaseCreature.CreatureState.sleeping)
          return;
        this.ChangeState(BaseCreature.CreatureState.waking);
      }
    }

    protected bool LemmyProximityCheck(float radius)
    {
      return (double) (this.transform.position - LevelHandler.Instance.lemmy.transform.position).magnitude < (double) radius;
    }

    protected void PushAndDamageLemmy(float _damage, Vector3 _push)
    {
      LevelHandler.Instance.lemmy.Push(_push);
      LevelHandler.Instance.lemmy.Damage(_damage, _push);
    }

    protected void ExplosiveDamageLemmy(
      float _damage,
      float _push,
      Vector3 _origin,
      float _radius)
    {
      Vector3 _direction = LevelHandler.Instance.lemmy.transform.position - _origin;
      float magnitude = _direction.magnitude;
      if ((double) magnitude > (double) _radius)
        return;
      _direction = (_radius - magnitude) * _direction.normalized;
      LevelHandler.Instance.lemmy.Push(_direction * _push);
      LevelHandler.Instance.lemmy.Damage(_damage * (_radius - magnitude), _direction);
    }

    public enum CreatureState
    {
      sleeping,
      waking,
      active,
      dying,
      dead,
    }
  }
}
