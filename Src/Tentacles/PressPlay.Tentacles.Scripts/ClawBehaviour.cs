// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ClawBehaviour
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ClawBehaviour : MonoBehaviour
  {
    public AudioWrapper sndBounce;
    public AudioWrapper sndConnection;
    public bool useLevelThemeSounds;
    protected GameObject connectTo;
    protected GameObject grabbedObject;
    protected float grabTime;
    protected OnClawBehaviourConnect doOnClawBehaviourConnect;
    protected RaycastHit rh;
    protected Ray ray;
    protected Lemmy lemmy;
    public float eatTime = 0.5f;
    public float eatSpeed = 15f;
    private Vector3 dist;
    private Vector3 dir;
    protected GameObject grabbedObjectHolder;
    protected RipableObject connectedObjectRipScript;
    protected Vector3 connectionPosition;
    protected Vector3 connectionNormal;
    protected float connectionTime;
    protected Collider connectedCollider;
    protected GameObject connectionPointerObject;
    protected Vector3 accumulatedTraversal = Vector3.zero;
    protected Vector3 lastPosition;
    protected Vector3 traversedVector;
    protected float lastDeltaTime;
    public PoolableObject createAtConnect;
    public PoolableObject createAtSlipperyConnect;
    public PoolableObject createAtShieldConnect;
    private ClawBehaviour.ClawStates _clawState;

    public bool isClawIdle => this.clawState == ClawBehaviour.ClawStates.idle;

    public bool isClawAttacking => this.clawState == ClawBehaviour.ClawStates.attacking;

    public bool isClawGrabbing => this.clawState == ClawBehaviour.ClawStates.grabbing;

    public bool isEating => this.clawState == ClawBehaviour.ClawStates.eating;

    public bool isClawConnected => this.clawState == ClawBehaviour.ClawStates.connected;

    public bool isClawDormant => this.clawState == ClawBehaviour.ClawStates.dormant;

    protected ClawBehaviour.ClawStates clawState => this._clawState;

    public override void Start()
    {
      this.lastPosition = this.transform.position;
      this.connectionPointerObject = new GameObject();
      this.connectionPointerObject.name = "connectionPointerObject";
      this.connectionPointerObject.active = false;
      this.grabbedObjectHolder = new GameObject();
      this.grabbedObjectHolder.name = "grabbedObjectHolder";
      this.grabbedObjectHolder.transform.parent = this.transform;
      this.grabbedObjectHolder.transform.localPosition = Vector3.zero;
      this.grabbedObjectHolder.transform.localRotation = Quaternion.identity;
    }

    protected virtual void ChangeClawState(ClawBehaviour.ClawStates newClawState)
    {
      this._clawState = newClawState;
      if (newClawState != ClawBehaviour.ClawStates.dormant)
        return;
      this.transform.position = this.lemmy.transform.position;
    }

    public virtual void GoDormant() => this.ChangeClawState(ClawBehaviour.ClawStates.dormant);

    public virtual void ExitDormant()
    {
      if (this.clawState != ClawBehaviour.ClawStates.dormant)
        return;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    public override void FixedUpdate() => this.DoFixedUpdate();

    protected virtual void DoFixedUpdate()
    {
    }

    public override void Update() => this.DoUpdate();

    protected virtual void DoUpdate()
    {
      this.traversedVector = this.transform.position - this.lastPosition;
      this.accumulatedTraversal *= 0.5f;
      this.accumulatedTraversal += this.traversedVector / Time.deltaTime;
      if (this.isClawDormant)
        this.transform.position = this.lemmy.transform.position;
      if (this.isClawConnected)
        this.HandleConnection();
      if (this.isClawGrabbing)
        this.HandleGrabbedObject();
      if (this.isClawAttacking)
        this.RaycastForEnemies();
      if (this.isEating)
        this.HandleEating();
      this.lastDeltaTime = Time.deltaTime;
      this.lastPosition = this.transform.position;
    }

    public void Initialize(Lemmy _lemmy) => this.lemmy = _lemmy;

    private void HandleGrabbedObject()
    {
      if (this.grabbedObject == null || (double) Time.time - (double) this.grabTime <= (double) this.lemmy.stats.grabPickupTime)
        return;
      this.ReleaseGrabbedObject();
    }

    private void HandleEating()
    {
      this.dist = this.transform.position - this.lemmy.transform.position;
      this.dir = this.dist.normalized;
      if ((double) this.dist.magnitude < (double) this.eatSpeed * (double) Time.deltaTime)
      {
        this.transform.position = this.lemmy.transform.position;
        this.connectedObjectRipScript.gameObject.transform.position = this.lemmy.transform.position;
        this.connectedObjectRipScript.Eat(this.eatTime, this.lemmy.transform);
        this.ReleaseGrabbedObject();
        this.lemmy.mainBody.Chew();
        this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      }
      else
        this.transform.position = this.transform.position - this.dir * this.eatSpeed * Time.deltaTime;
    }

    protected void HandleConnection()
    {
      this.rigidbody.velocity = Vector3.zero;
      if (this.connectionPointerObject != null && this.connectionPointerObject.active)
      {
        this.transform.position = this.connectionPointerObject.transform.position;
        this.transform.rotation = this.connectionPointerObject.transform.rotation;
      }
      else
      {
        this.transform.position = this.connectionPosition;
        this.transform.LookAt(this.transform.position - this.connectionNormal);
      }
      if (this.connectionPointerObject == null || this.connectionPointerObject.transform.parent != null && this.connectionPointerObject.transform.parent.gameObject.active)
        return;
      this.BreakConnection();
    }

    protected void RaycastForEnemies()
    {
      this.ray.origin = this.lastPosition;
      this.ray.direction = this.traversedVector;
      bool flag1 = Physics.Raycast(this.ray, out this.rh, this.traversedVector.magnitude, (int) GlobalSettings.Instance.allWallsAndShields_ClawSpecific);
      float distance1 = this.rh.distance;
      bool flag2 = Physics.Raycast(this.ray, out this.rh, this.traversedVector.magnitude, (int) GlobalSettings.Instance.enemyLayer);
      float distance2 = this.rh.distance;
      if (flag1 && !flag2 || flag1 && flag2 && (double) distance1 < (double) distance2)
      {
        this.HitWall(this.rh.point, this.rh.normal, this.rh.collider);
      }
      else
      {
        if ((flag1 || !flag2) && (!flag1 || !flag2 || (double) distance1 <= (double) distance2))
          return;
        EnergyCell component1 = this.rh.collider.gameObject.GetComponent<EnergyCell>();
        if ((bool) (UnityObject) component1)
        {
          this.HitEnergyCell(component1, this.rh.point, this.rh.normal);
        }
        else
        {
          Debug.Log((object) "NO ENERGY CELL FOUND!!! THIS IS BAD");
          RipableObject component2 = (RipableObject) this.rh.collider.gameObject.GetComponent(typeof (RipableObject));
          if (!(bool) (UnityObject) component2)
            return;
          this.ConnectToRipableObject(this.rh, component2);
        }
      }
    }

    private void HitWall(Vector3 hitPosition, Vector3 hitNormal, Collider hitCollider)
    {
      if (hitCollider != null && hitCollider.gameObject.layer == GlobalSettings.Instance.shieldLayer)
      {
        if (this.useLevelThemeSounds)
        {
          if (LevelHandler.Instance.levelTypeSettings.OnTentacleShieldParticle != null)
            ObjectPool.Instance.Draw((PoolableObject) LevelHandler.Instance.levelTypeSettings.OnTentacleShieldParticle, this.transform.position, Quaternion.LookRotation(this.connectionNormal));
        }
        else if (this.createAtShieldConnect != null)
          ObjectPool.Instance.Draw(this.createAtShieldConnect, this.transform.position, this.transform.rotation);
      }
      if (this.useLevelThemeSounds)
        LevelHandler.Instance.levelTypeSettings.audio.onTentacleBounce.PlaySound();
      else
        this.sndBounce.PlaySound();
      this.transform.position = hitPosition;
      this.rigidbody.velocity = -this.rigidbody.velocity * 0.2f;
      this.lastPosition = this.transform.position;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    private void HitEnergyCell(EnergyCell cell, Vector3 hitPosition, Vector3 hitNormal)
    {
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = hitPosition;
      BasicEnemyHitLump closestHitlump = cell.GetClosestHitlump(hitPosition);
      this.transform.position = closestHitlump.transform.position;
      this.ConnectToRipableObject(closestHitlump.transform.position, hitNormal, (RipableObject) closestHitlump);
      if (!(bool) (UnityObject) this.collider || !(bool) (UnityObject) cell.collider)
        return;
      Physics.IgnoreCollision(this.collider, cell.collider);
    }

    protected void HandleRipableObject()
    {
      RipableObject connectedObjectRipScript = this.connectedObjectRipScript;
    }

    protected void ConnectToRipableObject(Vector3 _pos, Vector3 _normal, RipableObject ripObj)
    {
      this.connectedObjectRipScript = ripObj;
      ripObj.ConnectToClaw(this);
      this.ConnectToAtPosition(_pos, _normal, ripObj.gameObject);
    }

    protected void ConnectToRipableObject(RaycastHit rh, RipableObject ripObj)
    {
      this.connectedObjectRipScript = ripObj;
      ripObj.ConnectToClaw(this);
      this.ConnectToAtPosition(rh.point, rh.normal, ripObj.gameObject);
    }

    public void ReleaseConnectionPointerObject()
    {
      this.connectionPointerObject.transform.parent = (Transform) null;
    }

    protected void ConnectToAtPosition(
      Vector3 hitPosition,
      Vector3 hitNormal,
      GameObject connectTo)
    {
      this.connectTo = connectTo;
      this.connectionPosition = hitPosition;
      this.connectedCollider = connectTo.collider;
      this.connectionNormal = hitNormal;
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = hitPosition;
      this.connectionPosition = hitPosition;
      this.ChangeClawState(ClawBehaviour.ClawStates.connected);
      this.connectionTime = Time.time;
      this.connectionPointerObject.transform.position = hitPosition;
      this.connectionPointerObject.transform.rotation = Quaternion.LookRotation(-hitNormal);
      this.connectionPointerObject.transform.parent = connectTo.transform;
      this.connectionPointerObject.active = true;
      LevelHandler.Instance.levelTypeSettings.audio.onTentacleHit.PlaySound();
      ObjectPool.Instance.Draw((PoolableObject) LevelHandler.Instance.levelTypeSettings.OnTentacleConnectParticle, this.transform.position, Quaternion.LookRotation(this.connectionNormal));
      this.doOnClawBehaviourConnect = connectTo.GetComponent<OnClawBehaviourConnect>();
      if (this.doOnClawBehaviourConnect == null)
        return;
      this.doOnClawBehaviourConnect.DoOnClawBehaviourConnect(this, hitNormal);
    }

    protected void ConnectAtPosition(Vector3 hitPosition, Vector3 hitNormal)
    {
      if (!this.isClawAttacking)
        return;
      this.connectionPointerObject.active = false;
      this.connectionNormal = (-this.rigidbody.velocity.normalized * 0.0f + hitNormal * 1f) * 0.5f;
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = hitPosition;
      this.connectionPosition = hitPosition;
      this.ChangeClawState(ClawBehaviour.ClawStates.connected);
      this.connectionTime = Time.time;
    }

    public void Push(Vector3 _force)
    {
      if (this.isClawConnected)
        return;
      this.rigidbody.AddForce(_force);
    }

    public void BreakConnection(float pushOfForce)
    {
      if (!this.isClawConnected)
        return;
      this.BreakConnection();
      this.Push(-this.transform.forward * pushOfForce);
    }

    public void BreakConnection()
    {
      if (!this.isClawConnected)
        return;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      this.connectedObjectRipScript = (RipableObject) null;
      this.connectionPointerObject.transform.parent = (Transform) null;
      this.connectionPointerObject.active = false;
    }

    public void Reset()
    {
      this.rigidbody.velocity = Vector3.zero;
      if ((bool) (UnityObject) this.grabbedObject)
        this.ReleaseGrabbedObject();
      if (this.grabbedObjectHolder.transform.childCount > 0)
      {
        UnityObject.Destroy((UnityObject) this.grabbedObjectHolder);
        this.grabbedObjectHolder = new GameObject();
        this.grabbedObjectHolder.name = "grabbedObjectHolder";
        this.grabbedObjectHolder.transform.parent = this.transform;
        this.grabbedObjectHolder.transform.localPosition = Vector3.zero;
        this.grabbedObjectHolder.transform.localRotation = Quaternion.identity;
      }
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
      this.DoOnReset();
    }

    public virtual void DoOnReset()
    {
    }

    public void Eat(RipableObject _rippedObject)
    {
      this.lemmy.mainBody.OpenMouth();
      this.Grab(_rippedObject.gameObject);
      this.connectedObjectRipScript = _rippedObject;
      this.ChangeClawState(ClawBehaviour.ClawStates.eating);
    }

    public void Grab(GameObject _obj)
    {
      if (_obj == this.grabbedObject)
        return;
      if ((bool) (UnityObject) this.grabbedObject)
        this.ReleaseGrabbedObject();
      this.grabbedObject = _obj;
      this.grabbedObject.transform.localScale = this.grabbedObject.transform.lossyScale / this.transform.lossyScale;
      this.grabbedObject.transform.parent = this.grabbedObjectHolder.transform;
      this.grabTime = Time.time;
      this.DoOnGrab(this.grabbedObject);
      this.ChangeClawState(ClawBehaviour.ClawStates.grabbing);
    }

    public virtual void DoOnGrab(GameObject _obj)
    {
    }

    public void ReleaseGrabbedObject()
    {
      this.DoOnReleaseGrabbedObject(this.grabbedObject);
      if (this.grabbedObject.tag == GlobalSettings.Instance.pickupTag)
        this.lemmy.GetPickupGrab(this.grabbedObject);
      this.grabbedObject = (GameObject) null;
      this.connectedObjectRipScript = (RipableObject) null;
      this.connectionPointerObject.transform.parent = (Transform) null;
      this.connectionPointerObject.active = false;
      this.ChangeClawState(ClawBehaviour.ClawStates.idle);
    }

    public virtual void DoOnReleaseGrabbedObject(GameObject _grabbedObject)
    {
    }

    public enum ClawStates
    {
      idle,
      attacking,
      connected,
      grabbing,
      eating,
      dormant,
    }
  }
}
