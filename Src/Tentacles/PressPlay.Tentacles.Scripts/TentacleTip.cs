// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleTip
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleTip : ClawBehaviour
  {
    private Vector3 intendedConnectionPos;
    [ContentSerializerIgnore]
    public float idleMovementRandomizer1 = 1f;
    [ContentSerializerIgnore]
    public float idleMovementRandomizer2 = 1f;
    private RaycastHit rh_1;
    private RaycastHit rh_2;
    private GameObject body;
    private Vector3 bodyNormal;
    private TentacleStats stats;
    private bool isInitialized;
    private float shootTime;
    private Vector3 shootDir = Vector3.zero;
    private TentacleTip.States _state;

    public bool isAttacking
    {
      get
      {
        return this.state == TentacleTip.States.usingClawState && this.clawState == ClawBehaviour.ClawStates.attacking;
      }
    }

    public bool isIdle
    {
      get
      {
        return this.state == TentacleTip.States.usingClawState && this.clawState == ClawBehaviour.ClawStates.idle;
      }
    }

    public bool isConnected
    {
      get
      {
        return this.state == TentacleTip.States.usingClawState && this.clawState == ClawBehaviour.ClawStates.connected;
      }
    }

    public bool isSearchingForConnection => this.state == TentacleTip.States.searchingForConnection;

    public bool isDormant
    {
      get
      {
        return this.state == TentacleTip.States.usingClawState && this.clawState == ClawBehaviour.ClawStates.dormant;
      }
    }

    private TentacleTip.States state => this._state;

    public void Initialize(
      GameObject _body,
      Vector3 _bodyNormal,
      TentacleStats _stats,
      Lemmy lemmy)
    {
      if (this.isInitialized)
        return;
      this.Initialize(lemmy);
      this.body = _body;
      this.bodyNormal = _bodyNormal;
      this.stats = _stats;
      this.idleMovementRandomizer1 = Random.Range(0.7f, 1.3f);
      this.idleMovementRandomizer2 = Random.Range(0.7f, 1.3f);
      this.isInitialized = true;
      Physics.IgnoreCollision(this.collider, lemmy.collider);
      this.collider.connectedBody.SleepingAllowed = false;
    }

    public override void Update()
    {
      if (!this.isConnected)
        return;
      if (this.connectedCollider != null && this.connectedCollider.gameObject.layer != (int) GlobalSettings.Instance.tentacleColliderLayerInt)
        this.BreakConnection(50f);
      else
        this.HandleConnection();
    }

    public override void FixedUpdate()
    {
      if (this.isDormant)
        this.transform.position = this.lemmy.transform.position;
      if (this.isSearchingForConnection)
        this.CheckSearchForConnectionTime();
      if (this.isConnected)
      {
        this.CheckConnectionDistance();
        this.CheckConnectionTime();
        if (this.connectedCollider != null && this.connectedCollider.gameObject.layer != (int) GlobalSettings.Instance.tentacleColliderLayerInt)
          this.BreakConnection(50f);
      }
      if (this.isSearchingForConnection || this.isIdle)
        this.HandleOverextensionElasticity();
      if (this.isIdle)
        this.IdleMovement();
      if (!this.isAttacking)
        return;
      this.RaycastForEnemies();
    }

    public override void LateUpdate()
    {
      base.LateUpdate();
      if (this.isConnected)
        this.HandleConnection();
      if (this.isSearchingForConnection)
      {
        this.RaycastForConnection();
        this.WallSeekingHelp();
      }
      if (!this.isIdle)
        return;
      this.transform.LookAt(this.transform.position + (this.transform.position - this.body.transform.position));
    }

    protected override void ChangeClawState(ClawBehaviour.ClawStates _clawState)
    {
      this._state = TentacleTip.States.usingClawState;
      base.ChangeClawState(_clawState);
    }

    private void ChangeTentacleState(TentacleTip.States newState) => this._state = newState;

    public override void OnTriggerStay(Collider collider)
    {
      base.OnTriggerStay(collider);
      if (!this.isIdle || !LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.allWallsAndShields, collider.gameObject.layer))
        return;
      this.rigidbody.AddForce((collider.transform.position - this.transform.position).normalized * 200f * Time.deltaTime);
    }

    public override void OnTriggerEnter(Collider collider)
    {
      base.OnTriggerEnter(collider);
      if (!this.isIdle || !LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.allWallsAndShields, collider.gameObject.layer))
        return;
      this.rigidbody.AddForce((collider.transform.position - this.transform.position).normalized * 30f);
    }

    public override void ExitDormant()
    {
      if (this.state != TentacleTip.States.usingClawState || this.clawState != ClawBehaviour.ClawStates.dormant)
        return;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    private void IdleMovement()
    {
      Vector3 vector3 = this.transform.position - (this.body.transform.position + this.bodyNormal * 2f) + new Vector3(Mathf.Cos(Time.time * 2.5f * this.idleMovementRandomizer1) * 0.8f, 0.0f, Mathf.Sin(Time.time * 1.75f * this.idleMovementRandomizer2) * 0.8f);
      float magnitude = vector3.magnitude;
      this.rigidbody.velocity *= 0.92f;
      this.rigidbody.AddForce(magnitude * -vector3 * this.stats.overMaxLengthElasticity);
      this.transform.LookAt(this.transform.position + (this.transform.position - this.body.transform.position));
    }

    private void CheckSearchForConnectionTime()
    {
      if (!this.isSearchingForConnection || (double) Time.time - (double) this.shootTime <= (double) this.stats.searchForConnectionTimeout * 1.2999999523162842)
        return;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    private void CheckConnectionDistance()
    {
      if (!this.isConnected || (double) (this.body.transform.position - this.transform.position).sqrMagnitude <= (double) this.stats.connectionMaxLength * (double) this.stats.connectionMaxLength)
        return;
      this.BreakConnection();
    }

    private void CheckConnectionTime()
    {
      if (!this.isConnected)
        return;
      double connectionTime = (double) this.connectionTime;
      double connectionTimeout = (double) this.stats.connectionTimeout;
    }

    private void WallSeekingHelp()
    {
      Vector3 vector3_1 = this.transform.position + this.rigidbody.velocity * Time.deltaTime;
      if ((double) (this.intendedConnectionPos - vector3_1).magnitude > 2.0)
        return;
      Vector3 vector3_2 = this.transform.position - this.body.transform.position;
      Ray ray1 = new Ray();
      ray1.origin = vector3_1;
      ray1.direction = new Vector3(vector3_2.z, 0.0f, -vector3_2.x) + vector3_2 * 0.5f;
      bool flag1 = Physics.Raycast(ray1, out this.rh_1, this.stats.wallSeekHelpDistance, (int) GlobalSettings.Instance.tentacleColliderLayer);
      Ray ray2 = new Ray();
      ray2.origin = vector3_1;
      ray2.direction = new Vector3(-vector3_2.z, 0.0f, vector3_2.x) + vector3_2 * 0.5f;
      bool flag2 = Physics.Raycast(ray2, out this.rh_2, this.stats.wallSeekHelpDistance, (int) GlobalSettings.Instance.tentacleColliderLayer);
      if (flag2 && !flag1 || flag2 && flag1 && (double) this.rh_2.distance < (double) this.rh_1.distance)
        this.SuckTowardRayHit(this.rh_1, ray2);
      if ((flag2 || !flag1) && (!flag2 || !flag1 || (double) this.rh_1.distance >= (double) this.rh_2.distance))
        return;
      this.SuckTowardRayHit(this.rh_2, ray1);
    }

    private void SuckTowardRayHit(RaycastHit _rh, Ray _ray)
    {
      this.rigidbody.AddForce(-this.rigidbody.velocity * Time.deltaTime * 130f);
      this.rigidbody.AddForce((this.stats.wallSeekHelpDistance - _rh.distance) * _ray.direction * this.stats.wallSeekHelpPower * (float) (1.0 / ((double) _rh.distance + 1.0)) * 1.5f);
    }

    private void RaycastForConnection()
    {
      this.traversedVector = this.transform.position - this.lastPosition;
      Vector3 vector3 = this.rigidbody.velocity * Time.deltaTime;
      this.transform.LookAt(this.transform.position + this.rigidbody.velocity);
      this.ray.origin = this.lastPosition;
      this.ray.direction = vector3 + this.traversedVector;
      this.lastPosition = this.transform.position;
      if ((double) this.ray.direction.sqrMagnitude == 0.0)
        return;
      float magnitude = (vector3 + this.traversedVector).magnitude;
      bool flag1 = Physics.Raycast(this.ray, out this.rh_1, magnitude, (int) GlobalSettings.Instance.tentacleBounceColliderLayers);
      bool flag2 = Physics.Raycast(this.ray, out this.rh_2, magnitude, (int) GlobalSettings.Instance.tentacleColliderLayer);
      if (flag1 && (!flag2 || (double) this.rh_1.distance < (double) this.rh_2.distance))
      {
        LevelHandler.Instance.levelTypeSettings.audio.onTentacleBounce.PlaySound();
        this.transform.position = this.rh_1.point;
        this.rigidbody.velocity = -this.rigidbody.velocity * 0.2f;
        this.lastPosition = this.transform.position;
        this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      }
      else
      {
        if (!flag2 || flag1 && (double) this.rh_1.distance <= (double) this.rh_2.distance)
          return;
        this.ConnectToAtPosition(this.rh_2.point + this.rh_2.normal * 0.3f, this.rh_2.normal, this.rh_2.collider.gameObject);
      }
    }

    private void HandleOverextensionElasticity()
    {
      Vector3 vector3 = this.transform.position - this.body.transform.position;
      float magnitude = vector3.magnitude;
      if ((double) magnitude <= (double) this.stats.tentacleLength)
        return;
      this.rigidbody.velocity *= 0.91f;
      this.rigidbody.AddForce((this.stats.tentacleLength - magnitude) * vector3 * this.stats.overMaxLengthElasticity);
    }

    public void ShootInDirection(Vector3 _direction)
    {
      this.ShootInDirection(_direction, this.transform.position + _direction * this.stats.connectionMaxLength);
    }

    public void ShootInDirection(Vector3 _direction, Vector3 _intendedConnectionPos)
    {
      if (this.isConnected)
        this.BreakConnection();
      this.intendedConnectionPos = _intendedConnectionPos;
      this.transform.position = this.body.transform.position;
      this.lastPosition = this.body.transform.position;
      float num = Mathf.Max(Mathf.Min(_direction.magnitude, this.stats.maxShootSpeed), this.stats.minShootSpeed);
      this.rigidbody.velocity = _direction.normalized * this.stats.tentacleTipMoveSpeed * num;
      this.ChangeTentacleState(TentacleTip.States.searchingForConnection);
      this.shootTime = Time.time;
      this.shootDir = _direction.normalized;
      this.transform.LookAt(this.transform.position + _direction);
    }

    public Vector3 GetElasticityForce()
    {
      Vector3 zero = Vector3.zero;
      Vector3 vector3 = this.body.transform.position - (this.transform.position + (this.body.transform.position - this.transform.position).normalized * this.stats.optimalConnectionDistance);
      float magnitude = vector3.magnitude;
      if ((double) magnitude > (double) this.stats.dragDistMin)
        zero += -vector3.normalized * (float) ((double) this.stats.dragBodyForce * (double) Mathf.Pow(magnitude - this.stats.dragDistMin, this.stats.dragCurvePow) + (double) Mathf.Cos(Time.time * (1.75f * this.idleMovementRandomizer2) + this.idleMovementRandomizer1) * 2.2000000476837158);
      return zero;
    }

    public override void DoOnReset()
    {
      if (this.isConnected)
        this.BreakConnection();
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = this.body.transform.position;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      this.lastPosition = this.body.transform.position;
    }

    public override void DoOnGrab(GameObject _obj)
    {
      base.DoOnGrab(_obj);
      this.ChangeTentacleState(TentacleTip.States.objectGrabbed);
    }

    public override void DoOnReleaseGrabbedObject(GameObject _grabbedObject)
    {
      base.DoOnReleaseGrabbedObject(_grabbedObject);
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    public enum States
    {
      usingClawState,
      searchingForConnection,
      objectGrabbed,
    }
  }
}
