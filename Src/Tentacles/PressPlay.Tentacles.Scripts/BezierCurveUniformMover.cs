// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BezierCurveUniformMover
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BezierCurveUniformMover : MonoBehaviour
  {
    private BezierCurve bezierCurve;
    [ContentSerializerIgnore]
    public BezierCurveUniformMover.DoOnReachPathEnd doOnHeadReachPathEnd;
    public ChainedMoverTarget head;
    protected Waypoint[] waypoints;
    protected Vector3[] bezierPoints;
    protected BezierConnection[] bezierConnections;
    public float speed;
    private float _pathLength;
    public Waypoint intializationStartWaypoint;
    [ContentSerializerIgnore]
    public float headPositionOnPath;
    [ContentSerializerIgnore]
    public bool isMoving;
    public ChainedMoverTarget[] parts;
    public bool automaticMovement = true;
    public bool loopMovement = true;
    public bool rotateToPathDirection = true;
    private ChainedMoverTarget frontPart;
    private ChainedMoverTarget backPart;
    private bool isInitialized;

    public float pathLength => this._pathLength;

    public override void Start()
    {
      if (this.isInitialized)
        return;
      this.Initialize();
    }

    public void Initialize()
    {
      List<Waypoint> waypointList = new List<Waypoint>();
      Waypoint waypoint = this.intializationStartWaypoint;
      if ((double) this.head.distToHead != 0.0)
      {
        Debug.LogError("Chained Mover head.distToHead MUST BE ZERO!!");
        this.head.distToHead = 0.0f;
      }
      if (waypoint != null)
      {
        waypointList.Add(waypoint);
        for (; waypoint.nextWaypoint != null && !waypointList.Contains(waypoint.nextWaypoint); waypoint = waypoint.nextWaypoint)
          waypointList.Add(waypoint.nextWaypoint);
        this.waypoints = new Waypoint[waypointList.Count];
        for (int index = 0; index < this.waypoints.Length; ++index)
          this.waypoints[index] = waypointList[index];
      }
      else
        this.waypoints = new Waypoint[0];
      this.bezierPoints = new Vector3[this.waypoints.Length];
      for (int index = 0; index < this.bezierPoints.Length; ++index)
        this.bezierPoints[index] = this.waypoints[index].transform.position;
      this.bezierCurve = new BezierCurve(this.bezierPoints);
      this.UpdateBezierConnections();
      if (this.automaticMovement)
        this.StartMovement();
      this.isInitialized = true;
    }

    public void SetParts(ChainedMoverTarget[] tmpParts) => this.parts = tmpParts;

    public void StartMovement() => this.StartMovement(0.0f);

    public void StartMovement(float _posOnPath)
    {
      this.UpdateBezierConnections();
      this.isMoving = true;
      this.headPositionOnPath = _posOnPath;
      this.UpdateAllPositions();
    }

    public void SetSpeed(float tmpSpeed) => this.speed = tmpSpeed;

    public void SetWaypoints(Waypoint[] tmpWaypoints)
    {
      this.waypoints = tmpWaypoints;
      this.bezierPoints = new Vector3[this.waypoints.Length];
      for (int index = 0; index < this.bezierPoints.Length; ++index)
        this.bezierPoints[index] = this.waypoints[index].transform.position;
      this.bezierCurve = new BezierCurve(this.bezierPoints);
      this.UpdateBezierConnections();
    }

    public void StopMovement() => this.isMoving = false;

    public override void Update()
    {
      if (this.isMoving)
      {
        this.headPositionOnPath += Time.deltaTime * this.speed;
        this.frontPart = this.head;
        this.backPart = this.head;
        for (int index = 0; index < this.parts.Length; ++index)
        {
          if ((double) this.parts[index].distToHead > (double) this.frontPart.distToHead)
            this.frontPart = this.parts[index];
          if ((double) this.parts[index].distToHead < (double) this.frontPart.distToHead)
            this.backPart = this.parts[index];
        }
        if ((double) this.headPositionOnPath > (double) this.pathLength)
        {
          if (this.loopMovement)
          {
            this.StartMovement(this.headPositionOnPath - this.pathLength);
          }
          else
          {
            this.headPositionOnPath = this.pathLength;
            this.StopMovement();
          }
          if (this.doOnHeadReachPathEnd != null)
            this.doOnHeadReachPathEnd();
        }
      }
      this.UpdateAllPositions();
    }

    private void UpdateAllPositions()
    {
      this.MoveTargetToPositionOnPath(this.head.target, this.headPositionOnPath);
      for (int index = 0; index < this.parts.Length; ++index)
        this.MoveTargetToPositionOnPath(this.parts[index].target, this.headPositionOnPath - this.parts[index].distToHead);
    }

    private void MoveTargetToPositionOnPath(Transform _target, float _position)
    {
      if (this.bezierPoints.Length == 0)
        return;
      float bezierFraction = this.PathPositionToBezierFraction(_position);
      Vector3 vector3 = this.bezierCurve.PointOnPath(bezierFraction);
      _target.position = !float.IsNaN(vector3.sqrMagnitude) ? vector3 : throw new InvalidOperationException();
      if (!this.rotateToPathDirection)
        return;
      Quaternion quaternion = Quaternion.LookRotation(_target.position - this.bezierCurve.PointOnPath(bezierFraction + 1f / 1000f));
      if (float.IsNaN(quaternion.x) || float.IsNaN(quaternion.y) || float.IsNaN(quaternion.z) || float.IsNaN(quaternion.w))
        quaternion = _target.rotation;
      _target.rotation = quaternion;
    }

    private float PathPositionToBezierFraction(float _position)
    {
      for (int index = 0; index < this.bezierConnections.Length; ++index)
      {
        if ((double) _position > (double) this.bezierConnections[index].previousPathLength && (double) _position < (double) this.bezierConnections[index].endConnectionPathLength)
          return this.bezierConnections[index].bezierFractionStart + (_position - this.bezierConnections[index].previousPathLength) / this.bezierConnections[index].length * this.bezierConnections[index].bezierFractionLength;
      }
      return 0.0f;
    }

    private void UpdateBezierConnections()
    {
      if (this.bezierConnections == null)
        this.bezierConnections = new BezierConnection[0];
      if (this.bezierPoints.Length != 0 && (this.bezierConnections == null || this.bezierConnections.Length != this.bezierPoints.Length - 1))
      {
        this.bezierConnections = new BezierConnection[this.bezierPoints.Length - 1];
        for (int index = 0; index < this.bezierConnections.Length; ++index)
          this.bezierConnections[index] = new BezierConnection();
      }
      float num = 0.0f;
      for (int index = 0; index < this.bezierConnections.Length; ++index)
      {
        this.bezierConnections[index].previousPathLength = num;
        this.bezierConnections[index].bezierFractionStart = (float) index / (float) this.bezierConnections.Length;
        this.bezierConnections[index].previousPoint = this.bezierPoints[index];
        this.bezierConnections[index].nextPoint = this.bezierPoints[index + 1];
        this.bezierConnections[index].length = (this.bezierPoints[index] - this.bezierPoints[index + 1]).magnitude;
        this.bezierConnections[index].endConnectionPathLength = this.bezierConnections[index].length + this.bezierConnections[index].previousPathLength;
        this.bezierConnections[index].bezierFractionLength = 1f / (float) this.bezierConnections.Length;
        num += this.bezierConnections[index].length;
      }
      this._pathLength = num;
    }

    public delegate void DoOnReachPathEnd();
  }
}
