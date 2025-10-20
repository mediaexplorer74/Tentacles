// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WaypointMover
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WaypointMover : MonoBehaviour
  {
    public Transform movementTarget;
    public float sequenceLengthOverride = -1f;
    public float moveSpeed = 1f;
    public float sequenceOffset;
    protected bool isCyclic;
    protected Waypoint[] waypoints;
    protected BezierCurve bezierCurve;
    protected Vector3[] bezierPoints;
    protected float totalPathLength;
    protected float totalSequenceTime;
    private float _sequenceTime;
    protected bool isInitialized;
    public GameObject waypointParent;
    public Waypoint startWaypoint;
    public bool rotateToMovementDirection;
    protected Vector3 lastPosition;
    public bool automaticMovement = true;
    protected bool isMoving;
    private Vector3 zeroVector = Vector3.zero;
    public WaypointMover.Mode mode;
    public bool autoInitialize = true;

    public float sequenceTime => this._sequenceTime;

    public override void Start()
    {
      if (!this.autoInitialize)
        return;
      this.Initialize();
    }

    public virtual void Initialize()
    {
      if (this.automaticMovement)
        this.isMoving = true;
      this.GetAndSetWaypoints();
      this.isInitialized = true;
      if (this.movementTarget != null)
        this.lastPosition = this.movementTarget.position;
      else
        this.lastPosition = this.zeroVector;
    }

    public override void FixedUpdate()
    {
      if (!this.isInitialized || this.movementTarget == null || !this.isMoving)
        return;
      this.movementTarget.position = this.GetPositionFromGlobalTime(LevelHandler.Instance.globalLevelTime);
      if (!this.rotateToMovementDirection)
        return;
      if (this.lastPosition != this.movementTarget.position)
        this.movementTarget.LookAt(this.movementTarget.position + (this.movementTarget.position - this.lastPosition));
      this.lastPosition = this.movementTarget.position;
    }

    protected Vector3 GetPositionFromGlobalTime(float _globalTime)
    {
      this._sequenceTime = (_globalTime - this.sequenceOffset) % this.totalSequenceTime;
      if ((double) this._sequenceTime < 0.0)
        this._sequenceTime += this.totalSequenceTime;
      return this.GetPositionFromSequenceTime(this.sequenceTime);
    }

    protected Vector3 GetPositionFromSequenceTime(float _sequenceTime)
    {
      switch (this.mode)
      {
        case WaypointMover.Mode.straightPath:
          return this.GetLinearPositionFromSequenceTime(_sequenceTime);
        case WaypointMover.Mode.bezier:
          return this.GetBezierPositionFromSequenceTime(_sequenceTime);
        default:
          return this.zeroVector;
      }
    }

    protected Vector3 GetBezierPositionFromSequenceTime(float _sequenceTime)
    {
      if ((double) _sequenceTime < 0.0)
        return this.waypoints[0].transform.position;
      return (double) _sequenceTime > (double) this.totalSequenceTime ? this.waypoints[this.waypoints.Length - 1].transform.position : this.bezierCurve.PointOnPath(_sequenceTime / this.totalSequenceTime);
    }

    protected Vector3 GetLinearPositionFromSequenceTime(float _sequenceTime)
    {
      if (this.waypoints == null)
        return Vector3.zero;
      for (int index = 0; index < this.waypoints.Length; ++index)
      {
        if ((double) _sequenceTime >= (double) this.waypoints[index].sequenceStartTime && (double) _sequenceTime <= (double) this.waypoints[index].sequenceEndTime)
          return this.waypoints[index].GetPositionOnPath(_sequenceTime);
      }
      return this.zeroVector;
    }

    protected void GetAndSetWaypoints()
    {
      if (this.startWaypoint == null)
      {
        Debug.LogError("Startwaypoint on object: " + this.name + " is NULL. IT MUST BE SOMETHING!! Or something bad will happen...");
      }
      else
      {
        Waypoint[] waypointArray = this.waypointParent != null ? this.waypointParent.GetComponentsInChildren<Waypoint>() : this.gameObject.GetComponentsInChildren<Waypoint>();
        this.waypoints = new Waypoint[waypointArray.Length];
        this.waypoints[0] = this.startWaypoint;
        this.bezierPoints = new Vector3[waypointArray.Length];
        if (this.waypoints.Length == 0)
          return;
        int index = 1;
        Waypoint waypoint = this.waypoints[0];
        while (waypoint.nextWaypoint != null)
        {
          if (waypoint.nextWaypoint == this.waypoints[0])
          {
            this.isCyclic = true;
            break;
          }
          this.waypoints[index] = waypoint.nextWaypoint;
          waypoint = waypoint.nextWaypoint;
          ++index;
        }
        this.SetWaypoints(this.waypoints);
      }
    }

    public void SetWaypoints(Waypoint[] tmpWaypoints)
    {
      if (tmpWaypoints != this.waypoints)
      {
        for (int index = 0; index < this.waypoints.Length; ++index)
          UnityObject.Destroy((UnityObject) this.waypoints[index]);
      }
      this.waypoints = tmpWaypoints;
      this.bezierPoints = !this.isCyclic ? new Vector3[this.waypoints.Length] : new Vector3[this.waypoints.Length + 1];
      for (int index = 0; index < this.waypoints.Length; ++index)
      {
        if (this.waypoints[index] == null)
          return;
        this.waypoints[index].InitializeStart();
        this.totalPathLength += this.waypoints[index].lengthToNextWaypoint;
        this.bezierPoints[index] = this.waypoints[index].transform.position;
      }
      if (this.isCyclic)
        this.bezierPoints[this.waypoints.Length] = this.waypoints[0].transform.position;
      if ((double) this.sequenceLengthOverride == -1.0)
      {
        this.totalSequenceTime = this.totalPathLength / this.moveSpeed;
      }
      else
      {
        this.totalSequenceTime = this.sequenceLengthOverride;
        this.moveSpeed = this.totalPathLength / this.totalSequenceTime;
      }
      float _positionOnPath = 0.0f;
      for (int index = 0; index < this.waypoints.Length; ++index)
      {
        this.waypoints[index].InitializeComplete(this.moveSpeed, _positionOnPath);
        _positionOnPath += this.waypoints[index].lengthToNextWaypoint;
      }
      this.bezierCurve = new BezierCurve(this.bezierPoints);
    }

    public enum Mode
    {
      straightPath,
      bezier,
    }
  }
}
