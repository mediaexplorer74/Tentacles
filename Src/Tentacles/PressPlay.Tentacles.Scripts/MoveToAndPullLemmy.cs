// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MoveToAndPullLemmy
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MoveToAndPullLemmy : MonoBehaviour
  {
    private static float lastDragTime;
    public MiniTentacle miniTentaclePrefab;
    private bool waitingForStartMovement;
    private MiniTentacle miniTentacle;
    public ObjectMover mover;
    public PoolablePickup pickupScript;
    private Vector3 dirToLemmy = Vector3.zero;
    private Vector3 dirToLemmyNormalized = Vector3.zero;
    public float moveDistSqrt = 4f;
    public float pickupDistSqrt = 1f;
    private float distSqrt;
    private bool isMoving;
    public float pullLemmyForce = 1f;
    public float moveSpeed = 4f;
    private bool isDelaying;
    private float startDelayTime;
    private float delayTime;

    public override void Update()
    {
      if (this.isDelaying && (double) Time.time > (double) this.startDelayTime + (double) this.delayTime)
        this.isDelaying = false;
      if (this.isDelaying)
        return;
      this.dirToLemmy = LevelHandler.Instance.lemmy.transform.position - this.transform.position;
      this.dirToLemmyNormalized = this.dirToLemmy.normalized;
      this.distSqrt = this.dirToLemmy.sqrMagnitude;
      if (!this.waitingForStartMovement && !this.isMoving && (double) this.distSqrt < (double) this.moveDistSqrt)
        this.waitingForStartMovement = true;
      if (!this.isMoving && this.waitingForStartMovement && (double) Time.time > (double) MoveToAndPullLemmy.lastDragTime + 0.075000002980232239)
      {
        this.waitingForStartMovement = false;
        MoveToAndPullLemmy.lastDragTime = Time.time;
        this.isMoving = true;
        this.DoOnStartMovement();
      }
      if (this.isMoving)
      {
        this.mover.SetDampening(0.0f);
        this.mover.SetVelocity(this.dirToLemmyNormalized * Mathf.Max(1f, this.dirToLemmy.magnitude) * this.moveSpeed + LevelHandler.Instance.lemmy.rigidbody.velocity * 0.5f);
        this.mover.DoMovement(Time.deltaTime);
        LevelHandler.Instance.lemmy.Push(-this.dirToLemmyNormalized * this.pullLemmyForce * Time.deltaTime);
        this.DoWhileMovement();
      }
      if (!this.isMoving || (double) this.distSqrt >= (double) this.pickupDistSqrt)
        return;
      this.DoOnPickUp();
      this.pickupScript.DoGrabPickUp(LevelHandler.Instance.lemmy);
    }

    public void DelayForSeconds(float _delay)
    {
      this.isDelaying = true;
      this.startDelayTime = Time.time;
      this.delayTime = _delay;
    }

    public void Reset() => this.isMoving = false;

    private void DoOnStartMovement()
    {
      if (this.miniTentaclePrefab != null)
      {
        this.miniTentacle = (MiniTentacle) ObjectPool.Instance.Draw((PoolableObject) this.miniTentaclePrefab);
        this.miniTentacle.Initialize(LevelHandler.Instance.lemmy.transform, this.transform);
      }
      this.pickupScript.StartLemmyDrag();
    }

    private void DoOnPickUp() => this.miniTentacle.Return();

    private void DoWhileMovement()
    {
    }
  }
}
