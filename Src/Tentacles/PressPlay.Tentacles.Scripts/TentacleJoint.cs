// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleJoint
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleJoint : MonoBehaviour
  {
    private TentacleJoint _frontConnection;
    private TentacleJoint _backConnection;
    private Tentacle tentacle;
    private int index;
    private float backConnectionRigidity = 1f / 1000f;
    private float frontConnectionRigidity = 1f / 1000f;
    private Vector3 seekPosition;
    private Vector3 normal = Vector3.right;
    private TentacleVisualStats visualStats;

    public TentacleJoint frontConnection
    {
      private set => this._frontConnection = value;
      get => this._frontConnection;
    }

    public TentacleJoint backConnection
    {
      private set => this._backConnection = value;
      get => this._backConnection;
    }

    public Vector3 directionFromFront
    {
      get => (this.transform.position - this.frontConnection.transform.position).normalized;
    }

    public Vector3 directionFromBack
    {
      get => (this.transform.position - this.backConnection.transform.position).normalized;
    }

    public void Initialize(
      TentacleJoint _backConnection,
      TentacleJoint _frontConnection,
      Tentacle _tentacle,
      int _index,
      TentacleVisualStats _visualStats)
    {
      this.visualStats = _visualStats;
      this.frontConnection = _frontConnection;
      this.backConnection = _backConnection;
      this.tentacle = _tentacle;
      this.index = _index;
    }

    public void Initialize(
      TentacleJoint _backConnection,
      TentacleJoint _frontConnection,
      TentacleVisualStats _visualStats)
    {
      this.visualStats = _visualStats;
      this.frontConnection = _frontConnection;
      this.backConnection = _backConnection;
    }

    public void DoUpdate()
    {
      this.seekPosition = Vector3.Lerp(this.backConnection.transform.position, this.frontConnection.transform.position, 0.48f);
      this.seekPosition += (this.transform.position - this.seekPosition).normalized * this.visualStats.curvature;
      this.transform.position = Vector3.Lerp(this.transform.position, this.seekPosition, this.visualStats.jointLinearStiffnes * Time.deltaTime);
    }

    public void DoHoseMechanicUpdate(Vector3 backDirection, Vector3 frontDirection)
    {
      float num = (this.backConnection.transform.position - this.frontConnection.transform.position).magnitude / 3f;
      this.transform.position = Vector3.Lerp(this.transform.position, this.frontConnection.transform.position + frontDirection * num, Time.deltaTime * 4f);
      this.transform.position = Vector3.Lerp(this.transform.position, this.backConnection.transform.position + backDirection * num, Time.deltaTime * 4f);
    }

    public void MoveTowardsBackConnection(float _amount)
    {
      this.transform.position = Vector3.Lerp(this.transform.position, this.backConnection.transform.position, _amount);
    }

    public Vector3 GetBackConnectionDirection()
    {
      return this.backConnection == null ? this.normal : (this.transform.position - this.backConnection.transform.position).normalized;
    }

    public void SetNormal(Vector3 _normal) => this.normal = _normal;

    public Vector3 GetFrontConnectionDirection()
    {
      return this.frontConnection == null ? this.normal : (this.frontConnection.transform.position - this.transform.position).normalized;
    }
  }
}
