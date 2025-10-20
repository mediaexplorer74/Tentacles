// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicPenetrator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicPenetrator : ResetOnLemmyDeath
  {
    public bool RECORDING_CINEMATIC_DONT_TOUCH;
    private RaycastHit rh;
    private Ray ray;
    public CatchUpStats catchUpStats;
    public float baseSpeed = 4f;
    public WaypointWrapper waypointWrapper;
    private BezierCurveUniformMover bezierMover;
    private Waypoint[] currentAttackWaypoints;
    public BasicPenetratorMainBody mainBodyPrefab;
    public GameObject tailPrefab;
    public int tailSegmentCount = 15;
    public float distBetweenTailSegments = 1f;
    protected BasicPenetratorMainBody mainBody;
    protected Transform[] tailSegments;
    protected GameObject[] frontSearchers;
    protected Transform[] frontSearchersUpperHits;
    protected Transform[] frontSearchersLowerHits;
    public int frontSearcherCount = 3;
    public int distBetweenFrontSearchers = 1;
    public BaseCondition startRunCondition;
    public BaseCondition finishRunCondition;
    public bool restartOnStartCondition = true;
    private BasicPenetrator.PenetratorSequenceState sequenceState;

    public override void Start() => this.Initialize();

    protected virtual void Initialize()
    {
      this.CreateTrainOfObjects();
      this.bezierMover.doOnHeadReachPathEnd = new BezierCurveUniformMover.DoOnReachPathEnd(this.DoOnReachPathEnd);
      this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.runNotStarted);
    }

    public override void Update()
    {
      if (this.RECORDING_CINEMATIC_DONT_TOUCH)
      {
        if (this.sequenceState == BasicPenetrator.PenetratorSequenceState.runNotStarted && this.startRunCondition.GetConditionStatus())
          this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.recordingCinematic);
        this.DoFrontSearcherRaycasts();
        this.mainBody.UpdateRunning();
      }
      else
      {
        if (this.restartOnStartCondition && this.finishRunCondition.GetConditionStatus())
          this.restartOnStartCondition = false;
        switch (this.sequenceState)
        {
          case BasicPenetrator.PenetratorSequenceState.runNotStarted:
            if (this.startRunCondition.GetConditionStatus())
              this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.running);
            this.SetFrontSearchersToBodyPosition();
            break;
          case BasicPenetrator.PenetratorSequenceState.running:
            this.DoCatchup();
            if (this.restartOnStartCondition && !this.startRunCondition.GetConditionStatus())
              this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.runNotStarted);
            this.DoFrontSearcherRaycasts();
            this.mainBody.UpdateRunning();
            if (this.mainBody.sndLoop.isPlaying)
              break;
            this.mainBody.sndLoop.worldPosition = this.mainBody.transform;
            this.mainBody.sndLoop.PlaySound();
            break;
          case BasicPenetrator.PenetratorSequenceState.runFinished:
            if (this.restartOnStartCondition && !this.startRunCondition.GetConditionStatus())
              this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.runNotStarted);
            this.bezierMover.SetSpeed(this.bezierMover.speed + (float) ((double) Time.deltaTime * (double) this.baseSpeed * 2.7000000476837158));
            this.SetFrontSearchersToBodyPosition();
            this.mainBody.UpdateRunFinished();
            break;
        }
      }
    }

    private void ChangeSequenceState(
      BasicPenetrator.PenetratorSequenceState _sequenceState)
    {
      this.sequenceState = _sequenceState;
      switch (this.sequenceState)
      {
        case BasicPenetrator.PenetratorSequenceState.runNotStarted:
          this.mainBody.DoReset();
          this.bezierMover.SetSpeed(this.baseSpeed);
          this.bezierMover.StartMovement(0.0f);
          this.bezierMover.StopMovement();
          this.mainBody.sndLoop.Stop();
          break;
        case BasicPenetrator.PenetratorSequenceState.running:
          this.StartMovement(this.waypointWrapper.waypoints);
          break;
        case BasicPenetrator.PenetratorSequenceState.runFinished:
          this.bezierMover.StopMovement();
          this.bezierMover.SetSpeed((float) (-(double) this.baseSpeed * (double) Random.Range(0.75f, 1f) * 3.0));
          this.bezierMover.StartMovement(this.bezierMover.pathLength - 0.01f);
          this.bezierMover.doOnHeadReachPathEnd = new BezierCurveUniformMover.DoOnReachPathEnd(this.DoOnReachPathEnd);
          break;
        case BasicPenetrator.PenetratorSequenceState.recordingCinematic:
          this.mainBody.sndLoop.PlaySound();
          this.StartMovement(this.waypointWrapper.waypoints);
          LevelHandler.Instance.cam.followObject = this.mainBody.gameObject;
          LevelHandler.Instance.cam.MoveToStablePosition();
          break;
      }
    }

    public void DoOnReachPathEnd()
    {
      this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.runFinished);
    }

    protected void CreateTrainOfObjects()
    {
      this.bezierMover = this.gameObject.AddComponent<BezierCurveUniformMover>();
      this.bezierMover.automaticMovement = false;
      this.bezierMover.loopMovement = false;
      ChainedMoverTarget[] tmpParts = new ChainedMoverTarget[this.tailSegmentCount + 1 + this.frontSearcherCount];
      int index1 = 0;
      this.tailSegments = new Transform[this.tailSegmentCount];
      for (int index2 = 0; index2 < this.tailSegmentCount; ++index2)
      {
        this.tailSegments[index2] = ((GameObject) UnityObject.Instantiate((UnityObject) this.tailPrefab)).transform;
        this.tailSegments[index2].parent = this.transform;
        GameObject gameObject = this.tailSegments[index2].gameObject;
        gameObject.name = gameObject.name + " " + index2.ToString();
        tmpParts[index1] = new ChainedMoverTarget()
        {
          target = this.tailSegments[index2],
          distToHead = (float) (index2 + 1) * this.distBetweenTailSegments
        };
        ++index1;
      }
      this.mainBody = (BasicPenetratorMainBody) UnityObject.Instantiate((UnityObject) this.mainBodyPrefab);
      this.mainBody.transform.parent = this.transform;
      ChainedMoverTarget chainedMoverTarget = new ChainedMoverTarget();
      chainedMoverTarget.target = this.mainBody.transform;
      tmpParts[index1] = chainedMoverTarget;
      int index3 = index1 + 1;
      this.bezierMover.head = chainedMoverTarget;
      this.frontSearchers = new GameObject[this.frontSearcherCount];
      this.frontSearchersUpperHits = new Transform[this.frontSearcherCount];
      this.frontSearchersLowerHits = new Transform[this.frontSearcherCount];
      for (int index4 = 0; index4 < this.frontSearcherCount; ++index4)
      {
        this.frontSearchersUpperHits[index4] = new GameObject().transform;
        this.frontSearchersUpperHits[index4].name = "front searcher upper hit " + index4.ToString();
        this.frontSearchersUpperHits[index4].transform.parent = this.transform;
        this.frontSearchersLowerHits[index4] = new GameObject().transform;
        this.frontSearchersLowerHits[index4].name = "front searcher lower hit " + index4.ToString();
        this.frontSearchersLowerHits[index4].transform.parent = this.transform;
        this.frontSearchers[index4] = new GameObject();
        this.frontSearchers[index4].name = "front searcher " + index4.ToString();
        this.frontSearchers[index4].transform.parent = this.transform;
        tmpParts[index3] = new ChainedMoverTarget()
        {
          target = this.frontSearchers[index4].transform,
          distToHead = (float) (-(index4 + 1) * this.distBetweenFrontSearchers)
        };
        ++index3;
      }
      this.bezierMover.SetParts(tmpParts);
      this.bezierMover.intializationStartWaypoint = this.waypointWrapper.startWaypoint;
      this.bezierMover.Initialize();
      this.mainBody.Initialize(this.frontSearchersUpperHits, this.frontSearchersLowerHits, this.tailSegments);
    }

    protected void SetFrontSearchersToBodyPosition()
    {
      for (int index = 0; index < this.frontSearchers.Length; ++index)
      {
        this.frontSearchersUpperHits[index].position = this.mainBody.transform.position;
        this.frontSearchersUpperHits[index].rotation = this.mainBody.transform.rotation;
        this.frontSearchersLowerHits[index].position = this.mainBody.transform.position;
        this.frontSearchersLowerHits[index].rotation = this.mainBody.transform.rotation;
      }
    }

    protected void DoFrontSearcherRaycasts()
    {
      for (int index = 0; index < this.frontSearchers.Length; ++index)
      {
        this.ray.origin = this.frontSearchers[index].transform.position;
        this.ray.direction = this.frontSearchers[index].transform.right;
        bool flag = Physics.Raycast(this.ray, out this.rh, 100f, (int) GlobalSettings.Instance.allWallsLayers);
        if (flag)
        {
          Debug.DrawLine(this.ray.origin, this.rh.point, Color.gray);
          this.frontSearchersUpperHits[index].position = this.rh.point - this.ray.direction * 0.3f;
          this.frontSearchersUpperHits[index].rotation = Quaternion.LookRotation(-this.ray.direction);
          this.frontSearchersUpperHits[index].DebugDrawLocal();
        }
        this.ray.direction = -this.frontSearchers[index].transform.right;
        Physics.Raycast(this.ray, out this.rh, 100f, (int) GlobalSettings.Instance.allWallsLayers);
        if (flag)
        {
          Debug.DrawLine(this.ray.origin, this.rh.point, Color.gray);
          this.frontSearchersLowerHits[index].position = this.rh.point - this.ray.direction * 0.3f;
          this.frontSearchersLowerHits[index].rotation = Quaternion.LookRotation(-this.ray.direction);
          this.frontSearchersLowerHits[index].DebugDrawLocal();
        }
      }
    }

    public void StartMovement(Waypoint[] _waypoints)
    {
      this.bezierMover.SetSpeed(this.baseSpeed);
      this.bezierMover.SetWaypoints(_waypoints);
      this.bezierMover.StartMovement();
    }

    protected void DoCatchup()
    {
      float magnitude = (this.mainBody.transform.position - LevelHandler.Instance.lemmy.transform.position).magnitude;
      if ((double) magnitude > (double) this.catchUpStats.catchUpDistance)
        this.bezierMover.SetSpeed(this.baseSpeed + (magnitude - this.catchUpStats.catchUpDistance) * this.catchUpStats.catchUpFactor);
      else if ((double) magnitude < (double) this.catchUpStats.catchUpDistance && (double) magnitude > (double) this.catchUpStats.slowDownDistance)
        this.bezierMover.SetSpeed(this.baseSpeed);
      else if ((double) this.catchUpStats.slowDownDistance - (double) this.catchUpStats.fullStopDistance > 0.0 && (double) magnitude < (double) this.catchUpStats.slowDownDistance && (double) magnitude > (double) this.catchUpStats.fullStopDistance)
      {
        this.bezierMover.SetSpeed(this.baseSpeed * (float) (((double) magnitude - (double) this.catchUpStats.fullStopDistance) / ((double) this.catchUpStats.slowDownDistance - (double) this.catchUpStats.fullStopDistance)));
      }
      else
      {
        if ((double) magnitude >= (double) this.catchUpStats.fullStopDistance)
          return;
        this.bezierMover.SetSpeed(0.0f);
      }
    }

    internal override void DoReset()
    {
      base.DoReset();
      if (!this.restartOnStartCondition)
        return;
      this.ChangeSequenceState(BasicPenetrator.PenetratorSequenceState.runNotStarted);
    }

    public enum PenetratorSequenceState
    {
      runNotStarted,
      running,
      runFinished,
      recordingCinematic,
    }
  }
}
