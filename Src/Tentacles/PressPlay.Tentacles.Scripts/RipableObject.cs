// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.RipableObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class RipableObject : MonoBehaviour
  {
    public PoolableObject createdOnEat;
    public PoolableObject trailPrefab;
    public AudioWrapper soundOnEat;
    public AudioWrapper soundOnRip;
    protected PoolableObject trail;
    public bool lockClaw = true;
    private Vector3 originalPosition;
    private ClawBehaviour connectedClaw;
    public float connectionTime;
    public float minForce;
    public float forceCap;
    public float forcedRipDelay;
    public float healthOnEat = 25f;
    private float eatStartTime = -1f;
    private float eatDuration = -1f;
    private float ripTime = -1f;
    private Vector3 ripLossyScale;

    public override void Start()
    {
      ObjectPool.Instance.Grow(this.createdOnEat, 1, 1);
      this.InitializeStartPosition(this.transform.position);
    }

    public void InitializeStartPosition(Vector3 _position)
    {
      this.originalPosition = _position;
      this.transform.position = _position;
    }

    public void Eat(float _eatTime, Transform _parent)
    {
      this.soundOnEat.PlaySound();
      if ((bool) (UnityObject) this.createdOnEat)
        ObjectPool.Instance.Draw(this.createdOnEat, _parent.position, _parent.rotation);
      this.transform.parent = _parent;
      LevelHandler.Instance.lemmy.AddHealth(this.healthOnEat);
      this.eatStartTime = Time.time;
      this.eatDuration = _eatTime;
      if (this.trail == null)
        return;
      this.trail.Return();
    }

    public void RemoveWhenEaten() => UnityObject.Destroy((UnityObject) this.gameObject);

    public override void Update()
    {
      if ((double) this.eatStartTime != -1.0 && 1.0 - ((double) Time.time - (double) this.eatStartTime) / (double) this.eatDuration < 0.0)
        this.RemoveWhenEaten();
      if ((double) this.ripTime != -1.0)
        this.transform.localScale = Vector3.Lerp(this.ripLossyScale, Vector3.one, Mathf.Clamp01((float) (((double) Time.time - (double) this.ripTime) / 0.40000000596046448))) / this.transform.parent.lossyScale;
      if ((bool) (UnityObject) this.connectedClaw && this.connectedClaw.isClawConnected)
        this.HandleConnectionTime();
      else
        this.connectedClaw = (ClawBehaviour) null;
    }

    private void HandleConnectionTime()
    {
      if ((double) Time.time - (double) this.connectionTime <= (double) this.forcedRipDelay)
        return;
      this.Rip(this.connectedClaw);
    }

    public void Rip(ClawBehaviour claw)
    {
      claw.ReleaseConnectionPointerObject();
      claw.Grab(this.gameObject);
      claw.BreakConnection();
      this.ripTime = Time.time;
      this.ripLossyScale = this.transform.lossyScale;
      this.DoOnRip(claw);
    }

    internal virtual void DoOnRip(ClawBehaviour claw)
    {
    }

    public void ConnectToClaw(ClawBehaviour _claw)
    {
      this.connectedClaw = _claw;
      this.connectionTime = Time.time;
      this.DoOnConnectToClaw(_claw);
    }

    internal virtual void DoOnConnectToClaw(ClawBehaviour claw)
    {
    }
  }
}
