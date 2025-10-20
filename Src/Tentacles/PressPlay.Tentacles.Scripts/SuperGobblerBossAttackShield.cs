// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperGobblerBossAttackShield
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperGobblerBossAttackShield : MonoBehaviour
  {
    private SuperGobblerBossAttackShield.ShieldState _state;
    public GameObject objectToMove;
    public int direction = 1;
    public float distanceToShoot = 1f;
    public float warningDuration = 1f;
    public float warningDelay;
    public Vector3 warningShake = new Vector3(3f, 3f, 3f);
    public float attackDuration = 1f;
    public float attackDelay;
    public iTween.EaseType attackEase = iTween.EaseType.bounce;
    public float contractionDuration = 1f;
    public float contractionDelay;
    public iTween.EaseType contractionEase = iTween.EaseType.easeInCubic;
    public float onDamageContractionDuration = 1f;
    public iTween.EaseType onDamageContractionEase = iTween.EaseType.easeOutQuad;
    public float checkIfLemmyIsInSightInterval = 1f;
    public float distanceToSee = 7f;
    public Transform[] sensors;
    public bool isActive = true;
    public PoolableObject createOnSpawn;
    public PoolableObject createOnDeath;
    private float lastCheckTime;
    private float lemmyInSightTime;
    private Vector3 startPosition;
    private Vector3 attackPosition;
    private Ray ray;
    private bool isLemmyInSight;
    private EnergyCell energy;
    private SuperGobblerBoss boss;
    public float pathStartPosition;
    private Transform[] shootDistanceCurve;
    private float pathPosition;
    private bool ping = true;
    public GameObject[] objectsToDisable;
    private BezierCurve movePath;
    private BezierCurve lookAtPath;

    public SuperGobblerBossAttackShield.ShieldState state => this._state;

    public bool isDead => this.energy.isDead;

    public override void Start()
    {
    }

    public void Initiate(
      SuperGobblerBoss boss,
      Transform[] movePath,
      Transform[] lookAtPath,
      Transform[] shootDistanceCurve,
      float pathStartPosition,
      bool ping)
    {
      this.lookAtPath = new BezierCurve(new Vector3[0]);
      this.lookAtPath.ResetPath(lookAtPath);
      this.movePath = new BezierCurve(new Vector3[0]);
      this.movePath.ResetPath(movePath);
      this.shootDistanceCurve = shootDistanceCurve;
      this.pathStartPosition = pathStartPosition;
      this.boss = boss;
      this.ping = ping;
      if (movePath.Length > 0 && lookAtPath.Length > 0)
      {
        iTween.PutOnPath(this.gameObject, movePath, pathStartPosition);
        this.transform.LookAt(iTween.PointOnPath(lookAtPath, pathStartPosition));
      }
      this.pathPosition = pathStartPosition;
      if (this.createOnSpawn != null)
        ObjectPool.Instance.Draw(this.createOnDeath, this.objectToMove.transform.position, this.objectToMove.transform.rotation);
      this.startPosition = this.objectToMove.transform.localPosition;
      this.attackPosition = this.objectToMove.transform.localPosition + this.transform.InverseTransformDirection(this.transform.forward) * this.distanceToShoot * (float) this.direction;
      this.ray = new Ray(this.startPosition, (this.objectToMove.transform.position + this.objectToMove.transform.forward * this.distanceToShoot * (float) this.direction - this.objectToMove.transform.position).normalized);
      this.energy = this.GetComponentInChildren<EnergyCell>();
      this.energy.SetListener(this.gameObject, "OnDamage");
      this.energy.Init(boss.life.lumpPrefab);
      this.energy.SetColliderLayers(false);
    }

    private void SetState(SuperGobblerBossAttackShield.ShieldState s) => this._state = s;

    private void MoveOnPath()
    {
      if (this.state != SuperGobblerBossAttackShield.ShieldState.idle)
        return;
      Vector3 vector3_1 = this.movePath.PointOnPath(this.pathPosition);
      Vector3 vector3_2 = this.lookAtPath.PointOnPath(this.pathPosition);
      this.transform.position = vector3_1;
      this.transform.rotation = Quaternion.LookRotation(vector3_1 - vector3_2, Vector3.up);
      if (this.ping)
      {
        this.pathPosition = NumberUtil.Increment(this.pathPosition, 0.2f * Time.deltaTime, 0.0f, 1f, NumberUtil.IncrementMode.clamp);
        if ((double) this.pathPosition != 1.0)
          return;
        this.ping = !this.ping;
      }
      else
      {
        this.pathPosition = NumberUtil.Increment(this.pathPosition, -0.2f * Time.deltaTime, 0.0f, 1f, NumberUtil.IncrementMode.clamp);
        if ((double) this.pathPosition != 0.0)
          return;
        this.ping = !this.ping;
      }
    }

    public override void Update()
    {
      if (!this.isActive)
        return;
      this.MoveOnPath();
      if (this.state == SuperGobblerBossAttackShield.ShieldState.idle)
        this.CanISeeLemmy();
      if (this.isLemmyInSight && this.state == SuperGobblerBossAttackShield.ShieldState.idle)
      {
        this.lemmyInSightTime += Time.deltaTime;
        if ((double) this.lemmyInSightTime <= (double) this.warningDelay)
          return;
        this.DoWarning();
        this.lemmyInSightTime = 0.0f;
      }
      else
        this.lemmyInSightTime = 0.0f;
    }

    public void ResetHealth() => this.energy.ResetHealth(false);

    private void DoWarning()
    {
      iTween.ShakeRotation(this.objectToMove, iTween.Hash((object) "amount", (object) this.warningShake, (object) "time", (object) this.warningDuration, (object) "delay", (object) this.warningDelay, (object) "oncomplete", (object) "OnWarningComplete", (object) "oncompletetarget", (object) this.gameObject, (object) "islocal", (object) true));
      this.SetState(SuperGobblerBossAttackShield.ShieldState.warning);
    }

    public void OnWarningComplete() => this.DoAttack();

    private void DoAttack()
    {
      this.attackPosition = this.transform.position + this.transform.forward * this.distanceToShoot;
      iTween.MoveTo(this.objectToMove, iTween.Hash((object) "position", (object) this.attackPosition, (object) "time", (object) this.attackDuration, (object) "delay", (object) this.attackDelay, (object) "easetype", (object) this.attackEase, (object) "oncomplete", (object) "OnAttackComplete", (object) "oncompletetarget", (object) this.gameObject));
      this.SetState(SuperGobblerBossAttackShield.ShieldState.attacking);
      this.energy.SetColliderLayers(true);
    }

    public void OnAttackComplete() => this.DoContraction();

    private void DoContraction()
    {
      iTween.MoveTo(this.objectToMove, iTween.Hash((object) "position", (object) this.startPosition, (object) "time", (object) this.contractionDuration, (object) "delay", (object) this.contractionDelay, (object) "oncomplete", (object) "OnContractionComplete", (object) "oncompletetarget", (object) this.gameObject, (object) "easetype", (object) this.contractionEase, (object) "islocal", (object) true));
      this.SetState(SuperGobblerBossAttackShield.ShieldState.contracting);
    }

    public void OnContractionComplete()
    {
      this.SetState(SuperGobblerBossAttackShield.ShieldState.idle);
      this.energy.SetColliderLayers(false);
    }

    public void OnDamage()
    {
      iTween.Stop(this.objectToMove);
      iTween.MoveTo(this.objectToMove, iTween.Hash((object) "position", (object) this.startPosition, (object) "time", (object) this.onDamageContractionDuration, (object) "oncomplete", (object) "OnContractionComplete", (object) "oncompletetarget", (object) this.gameObject, (object) "easetype", (object) this.onDamageContractionEase, (object) "islocal", (object) true));
      this.SetState(SuperGobblerBossAttackShield.ShieldState.contracting);
      this.energy.SetColliderLayers(false);
      this.boss.DoOnShieldDamage(this);
      if (!(bool) (UnityObject) this.createOnDeath)
        return;
      ObjectPool.Instance.Draw(this.createOnDeath, this.objectToMove.transform.position, this.objectToMove.transform.rotation);
    }

    private bool CanISeeLemmy()
    {
      this.isLemmyInSight = false;
      this.lastCheckTime = Time.time;
      this.ray.direction = this.transform.forward;
      this.ray.origin = this.transform.position;
      Debug.DrawRay(this.ray.origin, this.ray.direction * this.distanceToSee, Color.red);
      if (!Physics.Raycast(this.ray, this.distanceToSee, (int) GlobalSettings.Instance.lemmyLayer))
        return this.isLemmyInSight;
      this.isLemmyInSight = true;
      return this.isLemmyInSight;
    }

    public enum ShieldState
    {
      idle,
      warning,
      attacking,
      contracting,
    }
  }
}
