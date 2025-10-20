// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperGobblerBoss
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperGobblerBoss : BasicBoss
  {
    private SuperGobblerBoss.GobblerState state;
    private int currentPattern;
    public GameObject body;
    public Transform spawnPosition;
    public BasicLemmyDamager[] killLemmyColliders;
    public int hitpoints = 3;
    public int[] numberOfShields;
    public float liftDistance = 4f;
    public float liftDuration = 3f;
    public float liftDelay = 0.5f;
    public iTween.EaseType liftEase = iTween.EaseType.linear;
    public float contractDuration = 3f;
    public float contractDelay = 1f;
    public iTween.EaseType contractEase = iTween.EaseType.linear;
    public float earthquakeLiftDistance = 2f;
    public float earthquakeLiftDuration = 1f;
    public float earthquakePushForce = 500f;
    public Transform shieldParent;
    public SuperGobblerBossAttackShield shieldPrefab;
    public List<SuperGobblerBossAttackShield> shields;
    public EnergyCell energy;
    private Vector3 startPosition;
    private Vector3 liftPosition;
    private Vector3 earthquakePosition;
    private bool doHealthReset;
    private bool doEnemySpawn;
    private bool hasStarted;
    public Transform[] lookAtPath;
    public Transform[] moveToPath;
    public Transform[] shieldShootDistanceCurve;
    private bool hasDoneEarthquake;
    public PoolableObject createOnEarthquake;
    public Transform[] impactPositions;
    public Transform[] explosionPositions;
    public PoolableObject[] explosionPrefabs;
    public AudioWrapper sndMonsterLoop;
    public AudioWrapper sndEartquake;
    public AudioWrapper sndOnDamage;
    public AudioWrapper sndOnLift;
    public AudioWrapper sndOnContraction;
    public AudioWrapper sndOnDeath;
    private MeshCollider shieldMeshCollider;

    public override void Start()
    {
      base.Start();
      this.shieldMeshCollider = this.gameObject.GetComponentInChildren<MeshCollider>();
      this.shieldMeshCollider.connectedBody.SleepingAllowed = false;
    }

    public override void Activate()
    {
      if (this.hasBeenActivated)
        return;
      if (this.energy != null)
      {
        this.energy.SetListener(this.gameObject, "OnDamagedByLemmy");
        this.energy.SetColliderLayers(false);
      }
      this.startPosition = this.body.transform.position;
      this.liftPosition = this.body.transform.position + this.body.transform.forward * this.liftDistance;
      this.earthquakePosition = this.body.transform.position + this.body.transform.forward * this.earthquakeLiftDistance;
      this.UpdateActiveShields();
      this.sndMonsterLoop.PlaySound();
      this.hasBeenActivated = true;
      this.ChangeState(BaseCreature.CreatureState.active);
    }

    public void DoOnShieldDamage(SuperGobblerBossAttackShield shield)
    {
      this.shields.Remove(shield);
      UnityObject.Destroy((UnityObject) shield.gameObject);
      iTween.ShakeRotation(this.body, iTween.Hash((object) "amount", (object) new Vector3(0.0f, 3f, 0.0f), (object) "time", (object) 1));
      this.ShakeAndPushLemmy(0.0f);
      this.sndOnDamage.PlaySound();
    }

    private void ShouldILift()
    {
      if (this.state != SuperGobblerBoss.GobblerState.idle || this.shields.Count != 0)
        return;
      this.DoEarthQuake();
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      if (!this.hasBeenActivated)
        return;
      this.ShouldILift();
      Ray ray = new Ray()
      {
        origin = LevelHandler.Instance.lemmy.transform.position
      };
      ray.direction = this.shieldMeshCollider.transform.position - ray.origin;
      Debug.DrawRay(ray.origin, ray.direction * 5f, Color.blue);
      Physics.RaycastAll(ray.origin, ray.direction, 5f);
    }

    private void SetShieldState(bool active)
    {
      foreach (SuperGobblerBossAttackShield shield in this.shields)
        shield.isActive = active;
    }

    private void DoLiftBody()
    {
      iTween.MoveTo(this.body, iTween.Hash((object) "position", (object) this.liftPosition, (object) "time", (object) this.liftDuration, (object) "delay", (object) this.liftDelay, (object) "easetype", (object) this.liftEase, (object) "oncomplete", (object) "OnLiftBodyComplete", (object) "oncompletetarget", (object) this.gameObject));
      this.state = SuperGobblerBoss.GobblerState.lifting;
      this.SetShieldState(false);
      this.energy.SetColliderLayers(true);
      this.doHealthReset = true;
      this.currentPattern = 0;
      this.SetKillColliderStatus(false);
    }

    public void OnLiftBodyComplete() => this.DoContractBody();

    private void DoContractBody()
    {
      if (this.state == SuperGobblerBoss.GobblerState.earthquake)
      {
        this.hasDoneEarthquake = true;
        iTween.MoveTo(this.body, iTween.Hash((object) "position", (object) this.startPosition, (object) "time", (object) this.earthquakeLiftDuration, (object) "easetype", (object) this.contractEase, (object) "delay", (object) this.contractDelay, (object) "oncomplete", (object) "OnContractBodyComplete", (object) "oncompletetarget", (object) this.gameObject));
      }
      else
        iTween.MoveTo(this.body, iTween.Hash((object) "position", (object) this.startPosition, (object) "time", (object) this.contractDuration, (object) "easetype", (object) this.contractEase, (object) "delay", (object) this.contractDelay, (object) "oncomplete", (object) "OnContractBodyComplete", (object) "oncompletetarget", (object) this.gameObject));
      this.state = SuperGobblerBoss.GobblerState.contracting;
    }

    public void OnContractBodyComplete()
    {
      LevelHandler.Instance.cam.ShakeCamera(new Vector3(0.0f, 0.0f, 3f), 1f);
      this.ShakeAndPushLemmy(this.earthquakePushForce);
      if (this.createOnEarthquake != null)
      {
        foreach (Transform impactPosition in this.impactPositions)
          ObjectPool.Instance.Draw(this.createOnEarthquake, impactPosition.position, impactPosition.rotation);
      }
      if (this.hasDoneEarthquake)
      {
        this.DoLiftBody();
        this.hasDoneEarthquake = false;
        this.sndEartquake.PlaySound();
      }
      else
      {
        this.sndOnContraction.PlaySound();
        this.UpdateActiveShields();
        this.SetShieldState(true);
        this.energy.SetColliderLayers(false);
        this.SetKillColliderStatus(true);
        this.state = SuperGobblerBoss.GobblerState.idle;
      }
    }

    private void ShakeAndPushLemmy(float force)
    {
      LevelHandler.Instance.lemmy.BreakConnections();
      LevelHandler.Instance.lemmy.rigidbody.AddForce((LevelHandler.Instance.lemmy.transform.position - this.body.transform.position).normalized * force);
    }

    private void SetKillColliderStatus(bool status)
    {
      foreach (BasicLemmyDamager killLemmyCollider in this.killLemmyColliders)
        killLemmyCollider.doDamage = status;
    }

    private void DoEarthQuake()
    {
      iTween.MoveTo(this.body, iTween.Hash((object) "position", (object) this.earthquakePosition, (object) "time", (object) this.earthquakeLiftDuration, (object) "easetype", (object) this.liftEase, (object) "oncomplete", (object) "OnLiftBodyComplete", (object) "oncompletetarget", (object) this.gameObject));
      this.state = SuperGobblerBoss.GobblerState.earthquake;
      this.SetShieldState(false);
      this.sndOnLift.PlaySound();
    }

    public override void OnLemmyAttack()
    {
    }

    public void OnDamagedByLemmy()
    {
      iTween.Stop(this.body);
      this.DoContractBody();
      iTween.ShakeRotation(this.body, iTween.Hash((object) "amount", (object) new Vector3(3f, 3f, 3f), (object) "time", (object) 1));
      this.ShakeAndPushLemmy(0.0f);
      this.energy.SetColliderLayers(false);
      --this.hitpoints;
      this.hitpoints = Mathf.Max(0, this.hitpoints);
      if (!this.energy.isDead)
        return;
      this.Kill(3f);
    }

    private void UpdateActiveShields()
    {
      if (this.energy.isDead)
        return;
      int max = this.hitpoints - 1 > this.numberOfShields.Length ? 1 : this.numberOfShields[this.hitpoints - 1];
      int index = 0;
      while (this.shields.Count < max)
      {
        SuperGobblerBossAttackShield bossAttackShield = (SuperGobblerBossAttackShield) UnityObject.Instantiate((UnityObject) this.shieldPrefab);
        bossAttackShield.transform.parent = this.shieldParent;
        bossAttackShield.Initiate(this, this.moveToPath, this.lookAtPath, this.shieldShootDistanceCurve, this.GetShieldPosition(index, max), this.GetShieldDirection(index, max));
        this.shields.Add(bossAttackShield);
        ++index;
      }
    }

    private float GetShieldPosition(int index, int max)
    {
      switch (max)
      {
        case 1:
          return 0.5f;
        case 2:
          return index == 0 ? 0.25f : 0.75f;
        case 3:
          if (index == 0)
            return 0.25f;
          return index == 1 ? 0.5f : 0.75f;
        default:
          return 0.0f;
      }
    }

    private bool GetShieldDirection(int index, int max)
    {
      switch (max)
      {
        case 1:
          return true;
        case 2:
          return index == 0;
        case 3:
          return index != 0 && index == 1;
        default:
          return true;
      }
    }

    protected override void UpdateStateDying()
    {
    }

    protected override void UpdateStateDead()
    {
      this.DeactivateBoss();
      base.UpdateStateDead();
    }

    protected override void Kill(float _delay)
    {
      this.gameObject.AddComponent<DelayedInstantiator>().Init(_delay, _delay / (float) this.explosionPositions.Length, this.explosionPrefabs, this.explosionPositions);
      base.Kill(_delay);
    }

    protected override void Kill()
    {
      this.sndMonsterLoop.Stop();
      this.sndOnDeath.PlaySound();
      iTween.Stop(this.body);
      base.Kill();
    }

    private void OnDrawGizmos()
    {
      if (this.moveToPath.Length > 0)
        iTween.DrawPath(this.moveToPath, Color.yellow);
      if (this.lookAtPath.Length > 0)
        iTween.DrawPath(this.lookAtPath, Color.green);
      if (this.shieldShootDistanceCurve.Length <= 0)
        return;
      iTween.DrawPath(this.shieldShootDistanceCurve, Color.cyan);
    }

    public enum GobblerState
    {
      idle,
      lifting,
      contracting,
      earthquake,
    }
  }
}
