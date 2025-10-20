// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Waypoint
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Waypoint : MonoBehaviour
  {
    public Ease easeToNextWaypoint;
    public bool wormholeToNextWaypoint;
    [ContentSerializerIgnore]
    public Waypoint nextWaypoint;
    public float _lengthToNextWaypoint;
    private float _positionOnPath;
    private float _sequenceStartTime;
    private float _sequenceEndTime;
    private float sequenceTime;

    public float lengthToNextWaypoint => this._lengthToNextWaypoint;

    public float sequenceStartTime => this._sequenceStartTime;

    public float sequenceEndTime => this._sequenceEndTime;

    public float positionOnPath => this._positionOnPath;

    public void InitializeStart()
    {
      if (this.nextWaypoint == null)
        return;
      if (this.wormholeToNextWaypoint)
        this._lengthToNextWaypoint = 0.0f;
      else
        this._lengthToNextWaypoint = (this.transform.position - this.nextWaypoint.transform.position).magnitude;
    }

    public void InitializeComplete(float _moveSpeed, float _positionOnPath)
    {
      this._positionOnPath = _positionOnPath;
      this._sequenceStartTime = _positionOnPath / _moveSpeed;
      this._sequenceEndTime = (_positionOnPath + this.lengthToNextWaypoint) / _moveSpeed;
      this.sequenceTime = this._sequenceEndTime - this._sequenceStartTime;
    }

    public Vector3 GetPositionOnPath(float _sequenceTime)
    {
      return this.nextWaypoint == null ? this.transform.position : Vector3.Lerp(this.transform.position, this.nextWaypoint.transform.position, Equations.ChangeFloat(_sequenceTime - this.sequenceStartTime, 0.0f, 1f, this.sequenceTime, this.easeToNextWaypoint));
    }

    public static Waypoint CreateWaypoint(
      Vector3 _position,
      Transform _parent,
      Waypoint _lastWaypoint)
    {
      Waypoint waypoint = Waypoint.CreateWaypoint(_position, _parent);
      _lastWaypoint.nextWaypoint = waypoint;
      return waypoint;
    }

    public static Waypoint CreateWaypoint(Vector3 _position, Transform _parent)
    {
      return (Waypoint) new GameObject()
      {
        transform = {
          position = _position,
          parent = _parent
        }
      }.AddComponent(typeof (Waypoint));
    }
  }
}
