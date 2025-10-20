// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PoolablePickup
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class PoolablePickup : PoolableObject, IPickup
  {
    public ObjectMover mover;
    public bool destroyOnPickup = true;
    public bool pickUpOnCollision = true;
    public PoolablePickup.showVisualPositionType showVisualPosition;
    public PoolableObject onPickupVisualFX;
    public AudioWrapper onPickupSound;
    public AudioWrapper onGrabSound;
    public float timeToDie = 1f;
    public float lifetime = 1f;
    public float blinkSpeed = 0.2f;
    private float lastBlinkTime;
    private bool isInvisible;
    private float birthtime;
    private bool isAboutToDie;
    private MoveToAndPullLemmy moveToLemmyScript;
    protected bool _isBeingDragged;
    protected bool _hasBeenPickedUp;

    public bool isBeingDragged => this._isBeingDragged;

    public bool hasBeenPickedUp => this._hasBeenPickedUp;

    public override void Start()
    {
    }

    public override void Update()
    {
      if (this.mover != null && !this.mover.isSleeping)
        this.mover.DoMovement(Time.deltaTime);
      if (this.isBeingDragged && this.hasBeenPickedUp)
        return;
      if (!this.isAboutToDie && (double) this.lifetime > 0.0 && (double) Time.time > (double) this.birthtime + (double) this.lifetime)
      {
        this.isAboutToDie = true;
        this.lastBlinkTime = Time.time;
      }
      else
      {
        if (!this.isAboutToDie)
          return;
        if ((double) Time.time > (double) this.lastBlinkTime + (double) this.blinkSpeed)
        {
          this.isInvisible = !this.isInvisible;
          this.lastBlinkTime = Time.time;
        }
        if ((double) Time.time <= (double) this.birthtime + (double) this.lifetime + (double) this.timeToDie)
          return;
        this.Return();
      }
    }

    public override void Activate()
    {
      base.Activate();
      if (this.moveToLemmyScript == null)
        this.moveToLemmyScript = this.GetComponent<MoveToAndPullLemmy>();
      this.moveToLemmyScript.Reset();
      this.moveToLemmyScript.DelayForSeconds(Random.Range(0.15f, 0.35f));
      this._isBeingDragged = false;
      this._hasBeenPickedUp = false;
      this.isAboutToDie = false;
      this.birthtime = Time.time + Random.Range(0.0f, 0.5f);
      this.InitRandomMovement();
    }

    private void InitRandomMovement()
    {
      if (this.mover == null)
        return;
      this.mover.SetDampening(1f);
      Vector3 insideUnitSphere = Random.insideUnitSphere with
      {
        y = 0.0f
      };
      insideUnitSphere *= 5f;
      this.mover.SetVelocity(insideUnitSphere);
    }

    public void SetMovement(Vector3 direction)
    {
      if (this.mover == null)
        return;
      this.mover.SetDampening(1f);
      direction.y = 0.0f;
      direction *= 5f;
      this.mover.SetVelocity(direction);
    }

    public void MoveTowardsLemmy()
    {
      this.SetMovement((LevelHandler.Instance.lemmy.transform.position - this.transform.position).normalized + Random.insideUnitSphere * 0.7f);
    }

    public virtual void StartLemmyDrag()
    {
      this._isBeingDragged = true;
      this.DoOnPickUpAction(LevelHandler.Instance.lemmy);
      this.onGrabSound.PlaySound();
    }

    public void DoOnCollision(Lemmy lemmy)
    {
      if (!this.pickUpOnCollision || this.hasBeenPickedUp || this.isBeingDragged)
        return;
      this.DoOnPickUp(lemmy);
    }

    public void DoGrabPickUp(Lemmy lemmy)
    {
      if (this.hasBeenPickedUp)
        return;
      this.DoOnPickUp(lemmy);
    }

    protected void DoOnPickUp(Lemmy lemmy)
    {
      if (this.onPickupVisualFX != null)
      {
        if (this.showVisualPosition == PoolablePickup.showVisualPositionType.onPickupPosition)
          ObjectPool.Instance.Draw(this.onPickupVisualFX, this.transform.position, this.transform.rotation);
        else if (this.showVisualPosition == PoolablePickup.showVisualPositionType.onLemmyPosition)
          ObjectPool.Instance.Draw(this.onPickupVisualFX, lemmy.transform.position, lemmy.transform.rotation);
      }
      this.onPickupSound.PlaySound();
      this._hasBeenPickedUp = true;
      if (this.destroyOnPickup)
        UnityObject.Destroy((UnityObject) this.gameObject);
      else
        this.Return();
    }

    protected abstract void DoOnPickUpAction(Lemmy lemmy);

    public enum showVisualPositionType
    {
      onPickupPosition,
      onLemmyPosition,
    }
  }
}
