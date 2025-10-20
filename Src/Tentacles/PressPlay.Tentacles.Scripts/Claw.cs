// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Claw
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Claw : ClawBehaviour
  {
    private GameObject body;
    private Vector3 bodyNormal;
    private TentacleStats stats;
    private bool isInitialized;
    private float shootTime;
    private Vector3 shootDir = Vector3.zero;

    public bool isIdle => this.clawState == ClawBehaviour.ClawStates.idle;

    public bool isAttacking => this.clawState == ClawBehaviour.ClawStates.attacking;

    public bool isConnected => this.clawState == ClawBehaviour.ClawStates.connected;

    public override void FixedUpdate()
    {
      this.IdleMovement();
      this.HandleOverextensionElasticity();
      this.DoFixedUpdate();
    }

    public override void Update()
    {
      if (this.isAttacking)
        this.CheckAttackTime();
      this.DoUpdate();
    }

    public override void LateUpdate()
    {
      base.LateUpdate();
      this.transform.LookAt(this.transform.position + (this.transform.position - this.body.transform.position));
    }

    public void Initialize(Lemmy _lemmy, Vector3 _bodyNormal, TentacleStats _stats)
    {
      this.lemmy = _lemmy;
      this.body = _lemmy.gameObject;
      this.bodyNormal = _bodyNormal;
      this.stats = _stats;
      Physics.IgnoreCollision(this.collider, this.lemmy.collider);
      this.isInitialized = true;
    }

    public void HandleLemmyDeath()
    {
      if (this.grabbedObject == null)
        return;
      UnityObject.Destroy((UnityObject) this.grabbedObject);
      this.connectedObjectRipScript = (RipableObject) null;
    }

    public void SetBodyNormal(Vector3 _bodyNormal) => this.bodyNormal = _bodyNormal;

    private void CheckAttackTime()
    {
      if (!this.isAttacking || (double) Time.time - (double) this.shootTime <= (double) this.stats.searchForConnectionTimeout)
        return;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    private void IdleMovement()
    {
      if (!this.isIdle)
        return;
      this.bodyNormal = LevelHandler.Instance.cam.GetForwardDirection() + new Vector3(Mathf.Cos(Time.time * 2.5f) * 0.8f, 0.0f, Mathf.Sin(Time.time * 1.75f) * 0.8f);
      Vector3 vector3 = this.transform.position - (this.body.transform.position + this.bodyNormal * this.stats.optimalConnectionDistance);
      float magnitude = vector3.magnitude;
      this.rigidbody.velocity *= 0.91f;
      this.rigidbody.AddForce(magnitude * -vector3 * this.stats.overMaxLengthElasticity);
      this.transform.LookAt(this.transform.position + (this.transform.position - this.body.transform.position));
    }

    private void HandleOverextensionElasticity()
    {
      if (!this.isAttacking && !this.isIdle)
        return;
      Vector3 vector3 = this.transform.position - this.body.transform.position;
      float magnitude = vector3.magnitude;
      if ((double) magnitude <= (double) this.stats.tentacleLength)
        return;
      this.rigidbody.velocity *= 0.91f;
      this.rigidbody.AddForce((this.stats.tentacleLength - magnitude) * vector3 * this.stats.overMaxLengthElasticity);
    }

    public void ShootInDirection(Vector3 _direction)
    {
      if ((bool) (UnityObject) this.connectedObjectRipScript && this.connectedObjectRipScript.lockClaw)
        return;
      this.transform.position = this.body.transform.position;
      this.lastPosition = this.body.transform.position;
      float num = Mathf.Max(Mathf.Min(_direction.magnitude, this.stats.maxShootSpeed), this.stats.minShootSpeed);
      this.rigidbody.velocity = _direction.normalized * this.stats.tentacleTipMoveSpeed * num;
      this.ChangeClawState(ClawBehaviour.ClawStates.attacking);
      this.shootTime = Time.time;
      this.shootDir = _direction.normalized;
      this.transform.LookAt(this.transform.position + _direction);
    }

    public Vector3 GetElasticityForce()
    {
      Vector3 zero = Vector3.zero;
      Vector3 vector3 = this.body.transform.position - (this.transform.position + this.connectionNormal * this.stats.optimalConnectionDistance);
      if ((double) vector3.magnitude > (double) this.stats.dragDistMin)
        zero += -vector3 * this.stats.dragBodyForce;
      return zero;
    }

    public override void DoOnReset()
    {
      if (this.isConnected)
        this.BreakConnection();
      if (this.grabbedObject != null)
      {
        UnityObject.Destroy((UnityObject) this.grabbedObject);
        this.connectedObjectRipScript = (RipableObject) null;
      }
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = this.body.transform.position;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      this.lastPosition = this.body.transform.position;
    }
  }
}
