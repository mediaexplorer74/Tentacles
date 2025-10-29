// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleTip
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleTip : ClawBehaviour
  {
    private Microsoft.Xna.Framework.Vector3 intendedConnectionPos;
    [ContentSerializerIgnore]
    public float idleMovementRandomizer1 = 1f;
    [ContentSerializerIgnore]
    public float idleMovementRandomizer2 = 1f;
    private PressPlay.FFWD.RaycastHit rh_1;
    private PressPlay.FFWD.RaycastHit rh_2;
    private GameObject body;
    private Microsoft.Xna.Framework.Vector3 bodyNormal;
    private TentacleStats stats;
    private bool isInitialized;
    private float shootTime;
    private Microsoft.Xna.Framework.Vector3 shootDir = Microsoft.Xna.Framework.Vector3.Zero;
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
      Microsoft.Xna.Framework.Vector3 _bodyNormal,
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

    public override void OnTriggerStay(Component collider)
    {
      base.OnTriggerStay(collider);
      if (!this.isIdle || !LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.allWallsAndShields, collider.gameObject.layer))
        return;
      this.rigidbody.AddForce((collider.transform.position - this.transform.position).normalized * 200f * Time.deltaTime);
    }

    public override void OnTriggerEnter(Component collider)
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
      Microsoft.Xna.Framework.Vector3 vector3 = this.transform.position - (this.body.transform.position + this.bodyNormal * 2f) + new Microsoft.Xna.Framework.Vector3((float) Math.Cos(Time.time * 2.5f * this.idleMovementRandomizer1) * 0.8f, 0.0f, (float) Math.Sin(Time.time * 1.75f * this.idleMovementRandomizer2) * 0.8f);
      float magnitude = vector3.Length();
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
      if (!this.isConnected || (this.body.transform.position - this.transform.position).LengthSquared() <= this.stats.connectionMaxLength * this.stats.connectionMaxLength)
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
      Microsoft.Xna.Framework.Vector3 vector3_1 = this.transform.position + this.rigidbody.velocity * Time.deltaTime;
      if ((this.intendedConnectionPos - vector3_1).Length() > 2.0)
        return;
      Microsoft.Xna.Framework.Vector3 vector3_2 = this.transform.position - this.body.transform.position;
      PressPlay.FFWD.Ray ray1 = new PressPlay.FFWD.Ray();
      ray1.origin = vector3_1;
      ray1.direction = new Microsoft.Xna.Framework.Vector3(vector3_2.Z, 0.0f, -vector3_2.X) + vector3_2 * 0.5f;
      bool flag1 = PressPlay.FFWD.Physics.Raycast(ray1, out this.rh_1, this.stats.wallSeekHelpDistance, (int) GlobalSettings.Instance.tentacleColliderLayer);
      PressPlay.FFWD.Ray ray2 = new PressPlay.FFWD.Ray();
      ray2.origin = vector3_1;
      ray2.direction = new Microsoft.Xna.Framework.Vector3(-vector3_2.Z, 0.0f, vector3_2.X) + vector3_2 * 0.5f;
      bool flag2 = PressPlay.FFWD.Physics.Raycast(ray2, out this.rh_2, this.stats.wallSeekHelpDistance, (int) GlobalSettings.Instance.tentacleColliderLayer);
      if (flag2 && !flag1 || flag2 && flag1 && (double) this.rh_2.distance < (double) this.rh_1.distance)
        this.SuckTowardRayHit(this.rh_1, ray2);
      if ((flag2 || !flag1) && (!flag2 || !flag1 || (double) this.rh_1.distance >= (double) this.rh_2.distance))
        return;
      this.SuckTowardRayHit(this.rh_2, ray1);
    }

    private void SuckTowardRayHit(PressPlay.FFWD.RaycastHit _rh, PressPlay.FFWD.Ray _ray)
    {
      this.rigidbody.AddForce(-this.rigidbody.velocity * Time.deltaTime * 130f);
      this.rigidbody.AddForce((this.stats.wallSeekHelpDistance - _rh.distance) * _ray.direction * this.stats.wallSeekHelpPower * (float) (1.0 / ((double) _rh.distance + 1.0)) * 1.5f);
    }

    private void RaycastForConnection()
    {
      this.traversedVector = this.transform.position - this.lastPosition;
      Microsoft.Xna.Framework.Vector3 vector3 = this.rigidbody.velocity * Time.deltaTime;
      this.transform.LookAt(this.transform.position + this.rigidbody.velocity);
      this.ray.origin = this.lastPosition;
      this.ray.direction = vector3 + this.traversedVector;
      this.lastPosition = this.transform.position;
      if ((double) this.ray.direction.LengthSquared() == 0.0)
        return;
      float magnitude = (vector3 + this.traversedVector).Length();
      bool flag1 = PressPlay.FFWD.Physics.Raycast(this.ray, out this.rh_1, magnitude, (int) GlobalSettings.Instance.tentacleBounceColliderLayers);
      bool flag2 = PressPlay.FFWD.Physics.Raycast(this.ray, out this.rh_2, magnitude, (int) GlobalSettings.Instance.tentacleColliderLayer);
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
      Microsoft.Xna.Framework.Vector3 vector3 = this.transform.position - this.body.transform.position;
      float magnitude = vector3.Length();
      if ((double) magnitude <= (double) this.stats.tentacleLength)
        return;
      this.rigidbody.velocity *= 0.91f;
      this.rigidbody.AddForce((this.stats.tentacleLength - magnitude) * vector3 * this.stats.overMaxLengthElasticity);
    }

    public void ShootInDirection(Microsoft.Xna.Framework.Vector3 _direction) 
    {
      this.ShootInDirection(_direction, this.transform.position + _direction * this.stats.connectionMaxLength);
    }

    public void ShootInDirection(Microsoft.Xna.Framework.Vector3 _direction, Microsoft.Xna.Framework.Vector3 _intendedConnectionPos) 
    {
      if (this.isConnected)
        this.BreakConnection();
      this.intendedConnectionPos = _intendedConnectionPos;
      this.transform.position = this.body.transform.position;
      this.lastPosition = this.body.transform.position;
      float num = Math.Max(Math.Min(_direction.Length(), this.stats.maxShootSpeed), this.stats.minShootSpeed);
      this.rigidbody.velocity = Microsoft.Xna.Framework.Vector3.Normalize(_direction) * this.stats.tentacleTipMoveSpeed * num;
      this.ChangeTentacleState(TentacleTip.States.searchingForConnection);
      this.shootTime = Time.time;
      this.shootDir = Microsoft.Xna.Framework.Vector3.Normalize(_direction);
      this.transform.LookAt(this.transform.position + _direction);
    }

    public Microsoft.Xna.Framework.Vector3 GetElasticityForce() 
    {
      Microsoft.Xna.Framework.Vector3 zero = Microsoft.Xna.Framework.Vector3.Zero;
      Microsoft.Xna.Framework.Vector3 vector3 = this.body.transform.position - (this.transform.position + Microsoft.Xna.Framework.Vector3.Normalize(this.body.transform.position - this.transform.position) * this.stats.optimalConnectionDistance);
      float magnitude = vector3.Length();
      if ((double) magnitude > (double) this.stats.dragDistMin)
        zero += -Microsoft.Xna.Framework.Vector3.Normalize(vector3) * (float) ((double) this.stats.dragBodyForce * (double) Math.Pow(magnitude - this.stats.dragDistMin, this.stats.dragCurvePow) + (double) Math.Cos(Time.time * (1.75f * this.idleMovementRandomizer2) + this.idleMovementRandomizer1) * 2.2000000476837158);
      return zero;
    }

    public override void DoOnReset()
    {
      if (this.isConnected)
        this.BreakConnection();
      this.rigidbody.velocity = Microsoft.Xna.Framework.Vector3.Zero;
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
