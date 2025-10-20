// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperWormBoss
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperWormBoss : BasicBoss
  {
    private float lastTimeSpawnedDeathParticles;
    private int deathSegment;
    public float deathSplatInterval = 0.05f;
    public BezierCurveUniformMover bezierMover;
    public SuperWormEntryExit[] entryExitPoints;
    private Waypoint[] currentAttackWaypoints;
    public SuperWormAttackPattern[] attackPatterns;
    private SuperWormAttackPattern currentAttackPattern;
    private Waypoint victimPosition;
    private Waypoint curverPosition;
    public SuperWormHead headPrefab;
    public GameObject tailPrefab;
    public GameObject segmentPrefab;
    public PoolableParticle createOnSegmentDeath;
    public AudioWrapper playOnSegmentDeath;
    private GameObject frontSearcher;
    private SuperWormHead head;
    private GameObject tail;
    private GameObject[] segments;
    public int segmentCount;
    public float distBetweenSegments;
    private SuperWormBoss.WormBossState wormState;
    public SuperWormBoss.WormBossState[] attackCycle;
    private int currentAttackCycleIndex = -1;

    public override void Awake() => base.Awake();

    public override void Start()
    {
      base.Start();
      this.Initialize();
      this.bezierMover.Initialize();
      this.DeActivate();
    }

    private new void Initialize()
    {
      this.victimPosition = new GameObject().AddComponent<Waypoint>();
      this.victimPosition.name = "Super Worm Boss Victim Position";
      this.curverPosition = new GameObject().AddComponent<Waypoint>();
      this.curverPosition.name = "Super Worm Boss Curver Position";
      ChainedMoverTarget[] tmpParts = new ChainedMoverTarget[this.segmentCount + 2];
      this.ObjectsToDeactivateOnDeath = new GameObject[this.segmentCount + 2];
      this.head = (SuperWormHead) UnityObject.Instantiate((UnityObject) this.headPrefab);
      ChainedMoverTarget chainedMoverTarget1 = new ChainedMoverTarget();
      chainedMoverTarget1.target = this.head.transform;
      this.head.transform.parent = this.transform;
      this.bezierMover.head = chainedMoverTarget1;
      this.head.sndLoop.Initialize();
      this.frontSearcher = new GameObject();
      ChainedMoverTarget chainedMoverTarget2 = new ChainedMoverTarget();
      chainedMoverTarget2.distToHead = -8f;
      chainedMoverTarget2.target = this.frontSearcher.transform;
      this.frontSearcher.transform.parent = this.transform;
      tmpParts[this.segmentCount + 1] = chainedMoverTarget2;
      this.ObjectsToDeactivateOnDeath[0] = this.head.gameObject;
      this.segments = new GameObject[this.segmentCount];
      for (int index = 0; index < this.segmentCount; ++index)
      {
        this.segments[index] = (GameObject) UnityObject.Instantiate((UnityObject) this.segmentPrefab);
        this.segments[index].transform.parent = this.transform;
        tmpParts[index] = new ChainedMoverTarget()
        {
          target = this.segments[index].transform,
          distToHead = (float) (index + 1) * this.distBetweenSegments
        };
        this.ObjectsToDeactivateOnDeath[index + 1] = this.segments[index];
      }
      this.tail = (GameObject) UnityObject.Instantiate((UnityObject) this.tailPrefab);
      this.tail.transform.parent = this.transform;
      tmpParts[this.segmentCount] = new ChainedMoverTarget()
      {
        target = this.tail.transform,
        distToHead = (float) (this.segmentCount + 1) * this.distBetweenSegments
      };
      this.ObjectsToDeactivateOnDeath[this.segmentCount + 1] = this.tail;
      for (int index = 0; index < this.ObjectsToDeactivateOnDeath.Length; ++index)
      {
        foreach (Collider componentsInChild in this.ObjectsToDeactivateOnDeath[index].GetComponentsInChildren<Collider>())
          componentsInChild.allowTurnOff = true;
      }
      this.bezierMover.SetParts(tmpParts);
      this.GetComponent<EnergyContainer>().ForcedInitialize();
    }

    protected override void DoOnChangeStateWaking() => base.DoOnChangeStateWaking();

    private void ChangeWormBossState(SuperWormBoss.WormBossState _state)
    {
      this.wormState = _state;
      if (this.wormState != SuperWormBoss.WormBossState.playingAttackPattern)
        return;
      this.StartAttackPattern(this.GetBestAttackPattern(LevelHandler.Instance.lemmy.transform.position));
    }

    private void GotoNextAttackInCycle()
    {
      this.currentAttackCycleIndex = (this.currentAttackCycleIndex + 1) % this.attackCycle.Length;
      this.ChangeWormBossState(this.attackCycle[this.currentAttackCycleIndex]);
    }

    private void AttackUsingWaypoints(Waypoint[] _waypoints)
    {
      this.currentAttackWaypoints = _waypoints;
      this.bezierMover.SetWaypoints(this.currentAttackWaypoints);
      this.bezierMover.StartMovement();
      this.bezierMover.doOnHeadReachPathEnd = new BezierCurveUniformMover.DoOnReachPathEnd(this.AttackFinishedCallback);
    }

    private void StartAttackPattern(SuperWormAttackPattern _attackPattern)
    {
      if (this.currentAttackPattern != null && this.currentAttackPattern.entry != null && _attackPattern.entry != this.currentAttackPattern.entry)
        this.currentAttackPattern.entry.Close();
      if (this.currentAttackPattern != null && this.currentAttackPattern.exit != null && _attackPattern.exit != this.currentAttackPattern.exit)
        this.currentAttackPattern.exit.Close();
      if (_attackPattern.entry != null)
        _attackPattern.entry.Open();
      this.currentAttackPattern = _attackPattern;
      this.AttackUsingWaypoints(_attackPattern.path.waypoints);
    }

    protected override void DoOnChangeStateActive()
    {
      base.DoOnChangeStateActive();
      this.GotoNextAttackInCycle();
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      Quaternion quaternion = Quaternion.LookRotation(this.head.transform.position - this.frontSearcher.transform.position);
      this.head.rightAntenna.transform.rotation = quaternion;
      this.head.leftAntenna.transform.rotation = quaternion;
      if (!this.head.sndLoop.isPlaying)
        this.head.sndLoop.PlaySound();
      if (this.currentAttackPattern.exit == null || (double) (this.head.transform.position - this.currentAttackPattern.exit.transform.position).sqrMagnitude >= 2.0999999046325684)
        return;
      this.currentAttackPattern.exit.Open();
    }

    private void AttackFinishedCallback()
    {
      if (this.creatureState != BaseCreature.CreatureState.active)
        return;
      this.GotoNextAttackInCycle();
    }

    protected override void UpdateBossStateAttack() => base.UpdateBossStateAttack();

    public override void OnLemmyAttack()
    {
      this.spawnPickupPositions[0].transform.position = LevelHandler.Instance.lemmy.claw.transform.position;
      base.OnLemmyAttack();
    }

    protected override void DoOnChangeBossStateAngry() => base.DoOnChangeBossStateAngry();

    protected override void DoOnChangeBossStateAttack() => base.DoOnChangeBossStateAttack();

    protected override void DoOnChangeBossStateExposed() => base.DoOnChangeBossStateExposed();

    protected override void UpdateBossStateAngry() => base.UpdateBossStateAngry();

    protected override void UpdateBossStateExposed() => base.UpdateBossStateExposed();

    private EntryExitPair GetEntryExitPair(Vector3 _victimPosition) => new EntryExitPair();

    private SuperWormAttackPattern GetBestAttackPattern(Vector3 _victimPosition)
    {
      float num = this.EvaluateAttackPattern(this.attackPatterns[0], _victimPosition);
      SuperWormAttackPattern attackPattern1 = this.attackPatterns[0];
      float[] numArray = new float[this.attackPatterns.Length];
      for (int index = 1; index < this.attackPatterns.Length; ++index)
      {
        float attackPattern2 = this.EvaluateAttackPattern(this.attackPatterns[index], _victimPosition);
        if (this.attackPatterns[index] == this.currentAttackPattern)
          attackPattern2 -= 10f;
        numArray[index] = attackPattern2;
        if ((double) attackPattern2 > (double) num)
        {
          num = attackPattern2;
          attackPattern1 = this.attackPatterns[index];
        }
      }
      for (int index = 0; index < numArray.Length; ++index)
      {
        if ((double) num - (double) numArray[index] < 1.0 && (double) Random.Range(0.0f, 1f) > 0.800000011920929)
          attackPattern1 = this.attackPatterns[index];
      }
      return attackPattern1;
    }

    private float EvaluateAttackPattern(
      SuperWormAttackPattern _attackPattern,
      Vector3 _victimPosition)
    {
      float num1 = 0.0f;
      float num2 = 0.0f;
      float num3 = -1f;
      float num4 = 0.0f;
      if (_attackPattern.entry != null)
        num4 += (_attackPattern.entry.transform.position - _victimPosition).sqrMagnitude * num1;
      if (_attackPattern.exit != null)
        num4 += (_attackPattern.exit.transform.position - _victimPosition).sqrMagnitude * num2;
      return num4 + this.GetClosestDistToWaypoint(_attackPattern.path.waypoints, _victimPosition) * num3;
    }

    private float GetClosestDistToWaypoint(Waypoint[] _waypoints, Vector3 _pos)
    {
      if (_waypoints.Length == 0)
        return -1f;
      float x = (_waypoints[0].transform.position - _pos).sqrMagnitude;
      for (int index = 1; index < _waypoints.Length; ++index)
      {
        float sqrMagnitude = (_waypoints[index].transform.position - _pos).sqrMagnitude;
        if ((double) sqrMagnitude < (double) x)
          x = sqrMagnitude;
      }
      return Mathf.Sqrt(x);
    }

    private void CreateAttackPath(
      WaypointWrapper _entryPath,
      WaypointWrapper _exitPath,
      Vector3 _victimPosition)
    {
      int length = 1 + _entryPath.waypoints.Length + _exitPath.waypoints.Length;
      if (this.currentAttackWaypoints == null || this.currentAttackWaypoints.Length != length)
        this.currentAttackWaypoints = new Waypoint[length];
      int index1 = 0;
      foreach (Waypoint waypoint in _entryPath.waypoints)
      {
        this.currentAttackWaypoints[index1] = waypoint;
        ++index1;
      }
      this.currentAttackWaypoints[index1] = this.victimPosition;
      int index2 = index1 + 1;
      foreach (Waypoint waypoint in _exitPath.waypoints)
      {
        this.currentAttackWaypoints[index2] = waypoint;
        ++index2;
      }
      this.victimPosition.transform.position = _victimPosition;
      for (int index3 = 0; index3 < this.currentAttackWaypoints.Length - 1; ++index3)
        this.currentAttackWaypoints[index3].nextWaypoint = this.currentAttackWaypoints[index3 + 1];
    }

    protected override void DoOnChangeStateDying()
    {
      if (this.currentAttackPattern != null && this.currentAttackPattern.entry != null)
        this.currentAttackPattern.entry.Close();
      if (this.currentAttackPattern != null && this.currentAttackPattern.exit != null)
        this.currentAttackPattern.exit.Close();
      ObjectPool.Instance.Draw((PoolableObject) this.createOnSegmentDeath, this.head.transform.position, this.head.transform.rotation);
      this.playOnSegmentDeath.PlaySound();
      this.deathSegment = -1;
      this.head.gameObject.SetActiveRecursively(false);
      this.lastTimeSpawnedDeathParticles = Time.time;
    }

    protected override void DoOnChangeStateDead()
    {
      if (this.currentAttackPattern != null && this.currentAttackPattern.entry != null)
        this.currentAttackPattern.entry.Close();
      if (this.currentAttackPattern != null && this.currentAttackPattern.exit != null)
        this.currentAttackPattern.exit.Close();
      this.head.sndLoop.Stop();
      base.DoOnChangeStateDead();
    }

    protected override void UpdateStateDying()
    {
      if ((double) Time.time <= (double) this.lastTimeSpawnedDeathParticles + (double) this.deathSplatInterval)
        return;
      ++this.deathSegment;
      this.lastTimeSpawnedDeathParticles = Time.time;
      if (this.deathSegment == this.segments.Length)
      {
        this.playOnSegmentDeath.PlaySound();
        ObjectPool.Instance.Draw((PoolableObject) this.createOnSegmentDeath, this.tail.transform.position, this.tail.transform.rotation);
        this.tail.SetActiveRecursively(false);
        this.ChangeState(BaseCreature.CreatureState.dead);
      }
      else
      {
        this.playOnSegmentDeath.PlaySound();
        ObjectPool.Instance.Draw((PoolableObject) this.createOnSegmentDeath, this.segments[this.deathSegment].transform.position, this.segments[this.deathSegment].transform.rotation);
        this.segments[this.deathSegment].SetActiveRecursively(false);
      }
    }

    public override void Activate()
    {
      if (this.creatureState == BaseCreature.CreatureState.dead || this.creatureState == BaseCreature.CreatureState.dying)
        return;
      base.Activate();
      this.head.gameObject.SetActiveRecursively(true);
      this.tail.SetActiveRecursively(true);
      foreach (GameObject segment in this.segments)
        segment.SetActiveRecursively(true);
      this.ChangeState(BaseCreature.CreatureState.active);
    }

    public override void DeActivate()
    {
      this.head.gameObject.SetActiveRecursively(false);
      this.tail.SetActiveRecursively(false);
      foreach (GameObject segment in this.segments)
        segment.SetActiveRecursively(false);
      if (this.creatureState == BaseCreature.CreatureState.dead || this.creatureState == BaseCreature.CreatureState.dying)
        return;
      this.ChangeState(BaseCreature.CreatureState.sleeping);
    }

    public enum WormBossState
    {
      attackPatternPending,
      playingAttackPattern,
    }
  }
}
