// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Lemmy
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using FarseerPhysics.Dynamics;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Lemmy : MonoBehaviour
  {
    private const int framesBeforeSquish = 3;
    public Renderer glowRenderer;
    public bool aimAtFingerPosition;
    public bool useControllerInput;
    private RigidbodyAffectedByCurrents currentEffectScript;
    public AudioWrapper sndTentacle;
    public AudioWrapper sndDeath;
    public AudioWrapper sndDamageLow;
    public AudioWrapper sndDamageMedium;
    public AudioWrapper sndDamageHigh;
    [ContentSerializerIgnore]
    public LemmySquishedTester squishedTester;
    private Ray ray;
    private Vector3 lastPosition;
    public PoolableObject createOnLowDamage;
    public PoolableObject createOnMediumDamage;
    public PoolableObject createOnHighDamage;
    public PoolableObject createOnPushButNoDamage;
    public PoolableObject createOnDeath;
    private PoolableObject createOnLowDamageInstance;
    private PoolableObject createOnMediumDamageInstance;
    private PoolableObject createOnHighDamageInstance;
    private PoolableObject createOnPushButNoDamageInstance;
    private PoolableObject createOnDeathInstance;
    public Tentacle tentaclePrefab;
    public TentacleTip tentacleTipPrefab;
    public MainBody mainBodyPrefab;
    public Claw clawPrefab;
    public ParticleEmitter bubbleTrailPrefab;
    public ParticleEmitter bleedBubbleTrailPrefab;
    public LemmyStats stats;
    public TentacleStats tentacleStats;
    public TentacleStats clawStats;
    private int currentTentacleIndex;
    private TentacleJoint[] tentacleRoots;
    private Tentacle[] tentacles;
    private TentacleTip[] tentacleTips;
    public MainBody mainBody;
    private Tentacle clawTentacle;
    private Claw _claw;
    private ParticleEmitter bubbleTrail;
    private ParticleAnimator bubbleTrailAnimator;
    private ParticleEmitter bleedBubbleTrail;
    private ParticleAnimator bleedBubbleTrailAnimator;
    public PressPlay.FFWD.Components.Camera lemmyFollowCamera;
    public PathFollowCam pathFollowCam;
    private Vector3 forceFromTentacles;
    private Vector3 lastInputPosition = Vector3.zero;
    private float lastInputTime;
    private float _health;
    private bool invulnerable;
    private float invulnerableTime;
    private float invulnerableDuration;
    private int squishingColliderCount;
    private float squishDistance = 0.25f;
    private int framesSquished;
    private bool _isInputLocked;
    private bool _isGrabbed;
    private bool isBroughtToLife;
    private Lemmy.State _state;
    private float stateChangeTime;
    private bool doTentacleWavyMotions;

    [ContentSerializerIgnore]
    public Claw claw => this._claw;

    public float health
    {
      get => this._health;
      private set => this._health = value;
    }

    [ContentSerializerIgnore]
    public bool isInputLocked
    {
      get => this._isInputLocked || LevelHandler.Instance.isPlayingCinematicSequence;
      set => this._isInputLocked = value;
    }

    [ContentSerializerIgnore]
    public bool isGrabbed
    {
      get => this._isGrabbed;
      set => this._isGrabbed = value;
    }

    public void Initialize(Material _tentacleMaterial, Material _glowMaterial)
    {
      this.currentEffectScript = this.GetComponent<RigidbodyAffectedByCurrents>();
      this.glowRenderer.material = _glowMaterial;
      this.mainBody = (MainBody) UnityObject.Instantiate((UnityObject) this.mainBodyPrefab);
      this.mainBody.transform.position = this.transform.position;
      this.mainBody.transform.parent = this.transform;
      TentacleJoint component = (TentacleJoint) this.GetComponent(typeof (TentacleJoint));
      this._claw = (Claw) UnityObject.Instantiate((UnityObject) this.clawPrefab);
      this.claw.transform.position = this.transform.position;
      this.claw.Initialize(this, Vector3.back, this.clawStats);
      Physics.IgnoreCollision(this.collider, this.claw.collider);
      this.clawTentacle = (Tentacle) UnityObject.Instantiate((UnityObject) this.tentaclePrefab);
      this.clawTentacle.Initialize(this.clawStats, component, (TentacleJoint) this.claw.GetComponent(typeof (TentacleJoint)), _tentacleMaterial);
      this.bubbleTrail = (ParticleEmitter) UnityObject.Instantiate((UnityObject) this.bubbleTrailPrefab, this.transform.position, this.transform.rotation);
      this.bubbleTrail.transform.parent = this.transform;
      this.bubbleTrailAnimator = this.bubbleTrail.GetComponent<ParticleAnimator>();
      this.tentacleRoots = new TentacleJoint[this.stats.tentacles];
      this.tentacles = new Tentacle[this.stats.tentacles];
      this.tentacleTips = new TentacleTip[this.stats.tentacles];
      for (int index1 = 0; index1 < this.stats.tentacles; ++index1)
      {
        GameObject gameObject = new GameObject();
        gameObject.name = "tentacle root " + (object) index1;
        gameObject.AddComponent(typeof (TentacleJoint));
        this.tentacleRoots[index1] = gameObject.GetComponent<TentacleJoint>();
        this.tentacleTips[index1] = (TentacleTip) UnityObject.Instantiate((UnityObject) this.tentacleTipPrefab);
        this.tentacleTips[index1].transform.position = this.transform.position;
        Vector3 zero = Vector3.zero with
        {
          x = Mathf.Cos((float) (((double) index1 + 0.5) * 3.1415927410125732 * 2.0) / (float) this.stats.tentacles),
          z = Mathf.Sin((float) (((double) index1 + 0.5) * 3.1415927410125732 * 2.0) / (float) this.stats.tentacles)
        };
        this.tentacleRoots[index1].transform.position = this.transform.position;
        this.tentacleRoots[index1].transform.parent = this.transform;
        this.tentacleTips[index1].Initialize(this.gameObject, zero, this.tentacleStats, this);
        Physics.IgnoreCollision(this.tentacleTips[index1].collider, this.collider);
        Physics.IgnoreCollision(this.tentacleTips[index1].collider, this.claw.collider);
        for (int index2 = 0; index2 < index1; ++index2)
          Physics.IgnoreCollision(this.tentacleTips[index1].collider, this.tentacleTips[index2].collider);
        this.tentacles[index1] = (Tentacle) UnityObject.Instantiate((UnityObject) this.tentaclePrefab);
        this.tentacles[index1].Initialize(this.tentacleStats, this.tentacleRoots[index1], component, (TentacleJoint) this.tentacleTips[index1].GetComponent(typeof (TentacleJoint)), true, _tentacleMaterial);
        this.tentacles[index1].SetBodyNormal(zero);
      }
      this.squishedTester = this.GetComponentInChildren<LemmySquishedTester>();
      if (this.squishedTester != null)
        this.squishedTester.Initialize();
      this.collider.allowTurnOff = true;
      this.collider.connectedBody.FixtureList[0].Friction = 0.0f;
      this.health = this.stats.health;
      this.rigidbody.drag = this.stats.rigidbodyDrag;
      this.lastInputTime = Time.time;
      this.lastPosition = this.transform.position;
    }

    public void ChangeState(Lemmy.State _newState)
    {
      this._state = _newState;
      this.stateChangeTime = Time.time;
      switch (this._state)
      {
        case Lemmy.State.dormantBeforeSpawn:
          this.claw.GoDormant();
          this.claw.Reset();
          this.mainBody.LookUp();
          for (int index = 0; index < this.tentacleTips.Length; ++index)
          {
            this.tentacleTips[index].GoDormant();
            this.tentacles[index].Reset();
          }
          break;
        case Lemmy.State.normalActivity:
          this.claw.ExitDormant();
          this.mainBody.LookRight();
          for (int index = 0; index < this.tentacleTips.Length; ++index)
            this.tentacleTips[index].ExitDormant();
          break;
      }
    }

    public void SetNumberOfLives(int numberOfLives) => this.isBroughtToLife = true;

    public override void Update()
    {
      if (!this.isBroughtToLife)
        return;
      float num1 = this.rigidbody.velocity.sqrMagnitude + this.currentEffectScript.force.magnitude * 0.95f;
      Vector3 vector3_1 = this.currentEffectScript.force * 0.225f;
      Vector3 vector3_2 = this.currentEffectScript.force * 0.25f;
      float num2 = (float) ((double) num1 * 0.05000000074505806 + 0.039999999105930328);
      float num3 = (float) ((double) num1 * 0.10000000149011612 + 0.05000000074505806);
      this.bubbleTrail.worldVelocity = vector3_1;
      this.bubbleTrailAnimator.force = vector3_2;
      this.bubbleTrail.maxEmission = num3;
      this.bubbleTrail.minEmission = num2;
      if (this.invulnerable && (double) this.invulnerableDuration != -1.0 && (double) Time.time > (double) this.invulnerableTime + (double) this.invulnerableDuration)
        this.invulnerable = false;
      if (this.squishingColliderCount >= 2 && (bool) (UnityObject) this.squishedTester)
      {
        if (++this.framesSquished >= 3)
          this.squishedTester.Squish();
      }
      else
        this.framesSquished = 0;
      this.squishingColliderCount = 0;
    }

    public override void FixedUpdate()
    {
      if (!this.isBroughtToLife)
        return;
      if (this.useControllerInput)
        this.HandleShootTentacleInput_Controller();
      else if (!LevelHandler.Instance.CheckHitUIElements())
        this.HandleShootTentacleInput();
      this.DoWallCollisionRaycast();
      this.HandleConnectedTentacleTips();
      this.currentEffectScript.UpdateForces();
      if (this.claw.isIdle)
        this.claw.rigidbody.AddForce(this.rigidbody.velocity * 150f * Time.deltaTime);
      this.HandleHealth();
    }

    public void AddHealth(float _health)
    {
      this.health += _health;
      this.health = Mathf.Min(this.health, this.stats.health);
    }

    private void HandleHealth()
    {
      if ((double) this.health < (double) this.stats.health)
      {
        this.health += this.stats.regenerateDamagePerSecond * Time.deltaTime;
        this.health = Mathf.Min(this.health, this.stats.health);
      }
      this.mainBody.SetHealthFraction(this.health / this.stats.health);
    }

    private void HandleConnectedTentacleTips()
    {
      this.forceFromTentacles = Vector3.zero;
      for (int index = 0; index < this.tentacleTips.Length; ++index)
      {
        if (this.tentacleTips[index].isConnected)
        {
          this.forceFromTentacles += this.tentacleTips[index].GetElasticityForce();
          this.tentacleRoots[index].transform.localPosition = Vector3.Lerp(this.tentacleRoots[index].transform.localPosition, (this.tentacleTips[index].transform.position - this.transform.position).normalized * 0.45f, Time.deltaTime * 1.45f);
        }
        else
          this.tentacleRoots[index].transform.localPosition = Vector3.Lerp(this.tentacleRoots[index].transform.localPosition, Vector3.zero, Time.deltaTime * 1.5f);
        this.tentacleRoots[index].transform.localPosition -= new Vector3(0.0f, this.tentacleRoots[index].transform.localPosition.y, 0.0f);
        if (this.doTentacleWavyMotions)
          this.tentacles[index].SetBodyNormal(new Vector3(Mathf.Cos((float) ((double) Time.time * (double) this.tentacleTips[index].idleMovementRandomizer1 * 1.5)), 0.0f, -Mathf.Sin((float) ((double) Time.time * (double) this.tentacleTips[index].idleMovementRandomizer1 * 1.5))));
      }
      if (this.claw.isConnected)
        this.forceFromTentacles += this.claw.GetElasticityForce();
      this.rigidbody.AddForce(this.forceFromTentacles * Time.deltaTime * 30f);
    }

    private void ShowNextAvailableTentacle()
    {
      for (int index = 0; index < this.tentacles.Length; ++index)
        this.tentacles[index].ShowAsUnavailable();
      this.tentacles[this.GetNextAvailableTentacleIndex()].ShowAsAvailable();
    }

    private void HandleShootTentacleInput_Controller()
    {
      if (this.isInputLocked)
        return;
      if (InputHandler.Instance.GetShootClaw())
        this.ShootClawInDirection(InputHandler.InputVecToWorldVec(LevelHandler.Instance.cam.raycastCamera, InputHandler.Instance.GetClawDirection()));
      if (!InputHandler.Instance.GetShootTentacle())
        return;
      this.ShootTentacleInDirection(InputHandler.InputVecToWorldVec(LevelHandler.Instance.cam.raycastCamera, InputHandler.Instance.GetTentacleDirection()));
    }

    private void HandleShootTentacleInput()
    {
      if (this.isInputLocked || !InputHandler.Instance.GetShootTentacle())
        return;
      this.ray = this.lemmyFollowCamera.ScreenPointToRay(InputHandler.Instance.GetInputScreenPosition());
      ScreenRayCheckHit screenRayCheckHit = InputHandler.Instance.ScreenRayCheck(this.ray, GlobalSettings.Instance.enemyInputLayer);
      if (screenRayCheckHit.hitObjectInLayer)
      {
        if (this.aimAtFingerPosition)
          this.ShootClawInDirection(new Vector3(screenRayCheckHit.position.x, 0.0f, screenRayCheckHit.position.z) - this.transform.position);
        else
          this.ShootClawAtEnemy(screenRayCheckHit.obj);
      }
      else
      {
        this.lastInputPosition = screenRayCheckHit.position;
        this.lastInputTime = Time.time;
        this.ShootTentacleInDirection(screenRayCheckHit.position - this.transform.position, screenRayCheckHit.position);
      }
    }

    private void ShootClawInDirection(Vector3 _direction)
    {
      this.claw.ShootInDirection(_direction);
      this.sndTentacle.PlaySound();
    }

    private void ShootClawAtEnemy(GameObject enemy)
    {
      this.claw.ShootInDirection(enemy.transform.position - this.transform.position);
      this.sndTentacle.PlaySound();
    }

    private void ShootTentacleInDirection(Vector3 _direction)
    {
      this.currentTentacleIndex = this.GetNextAvailableTentacleIndex();
      this.tentacleTips[this.currentTentacleIndex].ShootInDirection(_direction);
      this.sndTentacle.PlaySound();
      this.tentacles[this.currentTentacleIndex].ShowAsUnavailable();
      this.tentacles[this.GetNextAvailableTentacleIndex()].ShowAsAvailable();
    }

    private void ShootTentacleInDirection(Vector3 _direction, Vector3 _intendedHitPosition)
    {
      this.currentTentacleIndex = this.GetNextAvailableTentacleIndex();
      this.tentacleTips[this.currentTentacleIndex].ShootInDirection(_direction, _intendedHitPosition);
      this.sndTentacle.PlaySound();
      this.tentacles[this.currentTentacleIndex].ShowAsUnavailable();
      this.tentacles[this.GetNextAvailableTentacleIndex()].ShowAsAvailable();
    }

    private int GetNextAvailableTentacleIndex()
    {
      for (int currentTentacleIndex = this.currentTentacleIndex; currentTentacleIndex < this.tentacleTips.Length + this.currentTentacleIndex; ++currentTentacleIndex)
      {
        if (this.tentacleTips[currentTentacleIndex % this.stats.tentacles].isIdle)
          return currentTentacleIndex % this.stats.tentacles;
      }
      return (this.currentTentacleIndex + 1) % this.stats.tentacles;
    }

    private void DoWallCollisionRaycast()
    {
      this.ray.origin = this.lastPosition;
      this.ray.direction = this.transform.position - this.lastPosition;
      RaycastHit hitInfo;
      if (Physics.Raycast(this.ray, out hitInfo, (this.transform.position - this.lastPosition).magnitude, (int) GlobalSettings.Instance.allWallsAndShields))
      {
        this.transform.position = hitInfo.point + hitInfo.normal * 0.5f;
        this.rigidbody.velocity = -this.rigidbody.velocity * 0.1f;
      }
      this.lastPosition = this.transform.position;
    }

    public override void OnCollisionEnter(Collision collision)
    {
      if (collision.gameObject.tag == GlobalSettings.Instance.pickupTag)
      {
        this.GetPickupCollision(collision.gameObject);
      }
      else
      {
        if (!(collision.gameObject.tag == GlobalSettings.Instance.triggeredByLemmyTag))
          return;
        collision.gameObject.GetComponent<TriggeredByLemmy>().Trigger();
      }
    }

    public override void OnCollisionStay(Collision collision)
    {
      base.OnCollisionStay(collision);
      if (!LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.allWallsLayers, collision.gameObject.layer) || collision.collider.connectedBody.BodyType != BodyType.Kinematic || (collision.contacts.Length == 1 ? (double) Vector3.DistanceSquared(collision.contacts[0].point, this.transform.position) : (double) Mathf.Min(Vector3.DistanceSquared(collision.contacts[0].point, this.transform.position), Vector3.DistanceSquared(collision.contacts[1].point, this.transform.position))) > (double) this.squishDistance)
        return;
      ++this.squishingColliderCount;
    }

    public override void OnTriggerEnter(Collider collider)
    {
      if (collider.gameObject.tag == GlobalSettings.Instance.pickupTag)
      {
        this.GetPickupCollision(collider.gameObject);
      }
      else
      {
        if (!(collider.gameObject.tag == GlobalSettings.Instance.triggeredByLemmyTag))
          return;
        collider.gameObject.GetComponent<TriggeredByLemmy>().Trigger();
      }
    }

    public void GetPickupCollision(GameObject pickupObj)
    {
    }

    public void GetPickupGrab(GameObject pickupObj)
    {
    }

    public void Damage(float _damage, Vector3 _direction)
    {
      if ((double) _damage == 0.0 || this.invulnerable || LevelHandler.Instance.state != LevelHandler.LevelState.playing)
        return;
      float health = this.health;
      this.health -= _damage;
      if ((double) this.health < 0.0)
        this.health = 0.0f;
      LevelHandler.Instance.levelSession.RegisterDamage(health - this.health);
      this.mainBody.StartTakeDamageAnimation();
      Quaternion rotation = Quaternion.LookRotation(_direction);
      if ((double) this.health <= 0.0)
        this.Kill();
      else if ((double) _damage == 0.0)
      {
        if (this.createOnPushButNoDamageInstance != null && this.createOnPushButNoDamageInstance.gameObject.active)
          return;
        this.createOnPushButNoDamageInstance = ObjectPool.Instance.Draw(this.createOnPushButNoDamage, this.transform.position, rotation);
      }
      else if ((double) _damage <= 10.0)
      {
        if (this.createOnLowDamageInstance == null || !this.createOnLowDamageInstance.gameObject.active)
          this.createOnLowDamageInstance = ObjectPool.Instance.Draw(this.createOnLowDamage, this.transform.position, rotation);
        this.sndDamageLow.PlaySound();
        if (!GlobalManager.Instance.currentProfile.vibrationIsEnabled)
          return;
        VibrateController.Default.Start(TimeSpan.FromMilliseconds(100.0));
      }
      else if ((double) _damage <= 40.0)
      {
        if (this.createOnMediumDamageInstance == null || !this.createOnMediumDamageInstance.gameObject.active)
          this.createOnMediumDamageInstance = ObjectPool.Instance.Draw(this.createOnMediumDamage, this.transform.position, rotation);
        this.sndDamageMedium.PlaySound();
        if (!GlobalManager.Instance.currentProfile.vibrationIsEnabled)
          return;
        VibrateController.Default.Start(TimeSpan.FromMilliseconds(200.0));
      }
      else
      {
        if (this.createOnHighDamageInstance == null || !this.createOnHighDamageInstance.gameObject.active)
          this.createOnHighDamageInstance = ObjectPool.Instance.Draw(this.createOnHighDamage, this.transform.position, rotation);
        this.sndDamageHigh.PlaySound();
        if (!GlobalManager.Instance.currentProfile.vibrationIsEnabled)
          return;
        VibrateController.Default.Start(TimeSpan.FromMilliseconds(300.0));
      }
    }

    public void SetInvulnerable(float duration)
    {
      this.invulnerable = true;
      this.invulnerableDuration = duration;
      this.invulnerableTime = Time.time;
    }

    public void Push(Vector3 _push) => this.rigidbody.AddForce(_push);

    public void BreakConnections()
    {
      for (int index = 0; index < this.tentacleTips.Length; ++index)
      {
        if (this.tentacleTips[index].isConnected)
          this.tentacleTips[index].BreakConnection();
      }
    }

    public void SetActivationStatus(bool _status)
    {
      for (int index = 0; index < this.tentacles.Length; ++index)
        this.tentacles[index].gameObject.SetActiveRecursively(_status);
      for (int index = 0; index < this.tentacleTips.Length; ++index)
        this.tentacleTips[index].gameObject.SetActiveRecursively(_status);
      this.claw.gameObject.SetActiveRecursively(_status);
      this.clawTentacle.gameObject.SetActiveRecursively(_status);
      this.mainBody.gameObject.SetActiveRecursively(_status);
      this.gameObject.SetActiveRecursively(_status);
    }

    public void SpawnAt(CheckPoint _checkpoint)
    {
      this.SetActivationStatus(true);
      this.rigidbody.velocity = Vector3.zero;
      this.transform.position = _checkpoint.transform.position;
      this.lastPosition = _checkpoint.transform.position;
      _checkpoint.ActivateCheckPoint();
      this.health = this.stats.health;
      for (int index = 0; index < this.tentacleTips.Length; ++index)
        this.tentacleTips[index].Reset();
      for (int index = 0; index < this.tentacles.Length; ++index)
        this.tentacles[index].Reset();
      this.claw.Reset();
      this.clawTentacle.Reset();
      PathFollowCam.Instance.ActivateClosestConnection(this.transform.position);
      LevelHandler.Instance.lastSpawnedAtCheckpoint = _checkpoint;
      _checkpoint.DoOnSpawnLemmy();
    }

    public void Kill()
    {
      if (LevelHandler.Instance.state != LevelHandler.LevelState.playing)
        return;
      if (this.createOnDeath != null)
        ObjectPool.Instance.Draw(this.createOnDeath, this.transform.position, this.transform.rotation);
      this.claw.HandleLemmyDeath();
      LevelHandler.Instance.DoOnLemmyDeath();
      this.SetActivationStatus(false);
      LevelHandler.Instance.levelSession.RegisterDeath();
      this.sndDeath.PlaySound();
      LevelHandler.Instance.RespawnAtLastCheckpoint();
      if (!GlobalManager.Instance.currentProfile.vibrationIsEnabled)
        return;
      VibrateController.Default.Start(TimeSpan.FromMilliseconds(500.0));
    }

    public void StartExitAnimation()
    {
    }

    public enum State
    {
      dormantBeforeSpawn,
      normalActivity,
    }
  }
}
