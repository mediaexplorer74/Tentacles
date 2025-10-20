// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BlockCrumble
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using FarseerPhysics.Dynamics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BlockCrumble : ResetOnLemmyDeath
  {
    public AudioWrapper sndOnConnect;
    public AudioWrapper sndOnDeflate;
    public AudioWrapper sndOnInflate;
    public SphereCollider pushLemmyOutCollider;
    private bool isInflatingPushOutCollider;
    private float inflationStartTime;
    private float inflationDuration = 0.55f;
    public OnClawBehaviourConnect[] onConnects;
    public UVSpriteSheetAnimator uvAnim;
    public PPAnimationHandler meshAnim;
    public PoolableObject createOnBreak;
    public string[] crumbleAnims;
    private int currentCrumbleState;
    public Collider[] turnOffColliders;
    public Renderer rendererToTurnOff;
    public float totalCrumbleTime = 3f;
    public bool resetOverTime = true;
    public float timeBeforeReset = 4f;
    private float resetTime;
    private bool isCrumblingOverTime;
    private float crumbleTime;
    private float inflationStateDuration = -1f;
    private float inflationStateChangeTime;
    private BlockCrumble.InflationState inflationState;
    private BlockCrumble.InflationState nextInflationState;
    public BlockCrumble.Mode mode = BlockCrumble.Mode.crumbleMoreOnConnect;

    protected bool isBroken
    {
      get
      {
        return this.inflationState == BlockCrumble.InflationState.deflated || this.inflationState == BlockCrumble.InflationState.deflating;
      }
    }

    public override void Start()
    {
      foreach (OnClawBehaviourConnect onConnect in this.onConnects)
      {
        if (onConnect != null)
          onConnect.doOnConnectDelegate = new OnClawBehaviourConnect.DoOnConnectDelegate(this.DoOnHit);
      }
      this.turnOffColliders = (Collider[]) this.GetComponentsInChildren<MeshCollider>();
      Collider[] componentsInChildren = this.GetComponentsInChildren<Collider>();
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        componentsInChildren[index].connectedBody.BodyType = BodyType.Kinematic;
        componentsInChildren[index].allowTurnOff = true;
      }
      this.pushLemmyOutCollider.isTrigger = true;
      BasicLemmyDamager basicLemmyDamager = this.pushLemmyOutCollider.gameObject.AddComponent<BasicLemmyDamager>();
      basicLemmyDamager.damageStats = new DamageLemmyStats();
      basicLemmyDamager.damageStats.damage = 0.0f;
      basicLemmyDamager.damageStats.breakLemmysConnections = false;
      basicLemmyDamager.damageStats.onStayDamage = 0.0f;
      basicLemmyDamager.damageStats.onStayPush = 50f;
      basicLemmyDamager.damageStats.push = 0.0f;
      this.pushLemmyOutCollider.gameObject.active = false;
      this.meshAnim.Initialize();
      this.ChangeInflationState(BlockCrumble.InflationState.inflated);
    }

    public override void Update()
    {
      this.UpdateCrumble();
      this.HandleInflationState();
    }

    protected void ChangeInflationState(BlockCrumble.InflationState _inflationState)
    {
      this.inflationStateDuration = -1f;
      this.inflationStateChangeTime = Time.time;
      this.inflationState = _inflationState;
      switch (this.inflationState)
      {
        case BlockCrumble.InflationState.inflated:
          this.isCrumblingOverTime = false;
          this.SetCrumbleState(0);
          this.uvAnim.Stop();
          this.pushLemmyOutCollider.gameObject.active = false;
          this.SetColliderActive(true);
          this.meshAnim.Play("inflate");
          this.meshAnim.animationComponent["inflate"].normalizedTime = 1f;
          break;
        case BlockCrumble.InflationState.deflating:
          this.meshAnim.Play("deflate");
          this.inflationStateDuration = this.meshAnim.animationComponent["deflate"].length;
          this.SetColliderActive(false);
          this.pushLemmyOutCollider.gameObject.active = false;
          this.currentCrumbleState = this.crumbleAnims.Length;
          this.inflationStateDuration = this.meshAnim.animationComponent["deflate"].length;
          this.nextInflationState = BlockCrumble.InflationState.deflated;
          this.sndOnDeflate.PlaySound();
          break;
        case BlockCrumble.InflationState.deflated:
          this.SetColliderActive(false);
          this.pushLemmyOutCollider.gameObject.active = false;
          this.currentCrumbleState = this.crumbleAnims.Length;
          this.meshAnim.Play("deflate");
          this.meshAnim.animationComponent["deflate"].normalizedTime = 1f;
          if (!this.resetOverTime)
            break;
          this.inflationStateDuration = this.timeBeforeReset;
          this.nextInflationState = BlockCrumble.InflationState.inflating;
          break;
        case BlockCrumble.InflationState.inflating:
          this.isCrumblingOverTime = false;
          this.SetCrumbleState(0);
          this.uvAnim.Stop();
          this.pushLemmyOutCollider.gameObject.active = true;
          this.inflationStartTime = Time.time;
          this.inflationStateDuration = this.meshAnim.animationComponent["inflate"].length;
          this.nextInflationState = BlockCrumble.InflationState.inflated;
          this.sndOnInflate.PlaySound();
          break;
      }
    }

    protected void HandleInflationState()
    {
      if ((double) this.inflationStateDuration != -1.0 && (double) Time.time > (double) this.inflationStateChangeTime + (double) this.inflationStateDuration)
        this.ChangeInflationState(this.nextInflationState);
      switch (this.inflationState)
      {
        case BlockCrumble.InflationState.inflated:
          this.meshAnim.Play("inflate");
          this.meshAnim.animationComponent["inflate"].normalizedTime = 1f;
          break;
        case BlockCrumble.InflationState.inflating:
          float num = (Time.time - this.inflationStateChangeTime) / this.inflationStateDuration;
          this.meshAnim.Play("inflate");
          this.meshAnim.animationComponent["inflate"].normalizedTime = num;
          break;
      }
    }

    protected void UpdateCrumble()
    {
      if (!this.isCrumblingOverTime || this.isBroken)
        return;
      this.crumbleTime += Time.deltaTime;
      if ((double) this.crumbleTime >= (double) this.totalCrumbleTime)
      {
        this.Break();
      }
      else
      {
        this.uvAnim.ShowFrameWithIndex((int) ((double) (this.crumbleTime / this.totalCrumbleTime) * (double) this.uvAnim.frameCount));
        this.uvAnim.Stop();
      }
    }

    public void DoOnHit(ClawBehaviour _tip, Vector3 _hitDir)
    {
      if (!this.isCrumblingOverTime)
        this.sndOnConnect.PlaySound();
      switch (this.mode)
      {
        case BlockCrumble.Mode.crumbleOverTimeAfterFirstConnect:
          if (this.isCrumblingOverTime)
            break;
          this.SetCrumbleState(0);
          this.isCrumblingOverTime = true;
          this.crumbleTime = 0.0f;
          break;
        case BlockCrumble.Mode.crumbleMoreOnConnect:
          this.SetCrumbleState(this.currentCrumbleState + 1);
          break;
      }
    }

    private void SetCrumbleState(int _crumbleState)
    {
      if (_crumbleState != 0 && _crumbleState > this.crumbleAnims.Length - 1)
      {
        this.Break();
      }
      else
      {
        if (!this.rendererToTurnOff.enabled)
          this.rendererToTurnOff.enabled = true;
        this.currentCrumbleState = _crumbleState;
        this.uvAnim.Play(this.crumbleAnims[_crumbleState]);
      }
    }

    private void Break()
    {
      this.ChangeInflationState(BlockCrumble.InflationState.deflating);
      if (this.createOnBreak == null)
        return;
      ObjectPool.Instance.Draw(this.createOnBreak, this.transform.position, this.transform.rotation);
    }

    internal override void DoReset()
    {
      base.DoReset();
      this.ChangeInflationState(BlockCrumble.InflationState.inflated);
    }

    private void HandleMeshAnim()
    {
      float num = (Time.time - this.inflationStartTime) / this.inflationDuration;
      this.meshAnim.Play("inflate");
      this.meshAnim.animationComponent["inflate"].normalizedTime = num;
    }

    private void HandlePushOutLemmy()
    {
      if (!this.isInflatingPushOutCollider || (double) ((Time.time - this.inflationStartTime) / this.inflationDuration) < 1.0)
        return;
      this.isInflatingPushOutCollider = false;
      this.SetColliderActive(true);
    }

    private void SetColliderActive(bool _active)
    {
      foreach (Collider turnOffCollider in this.turnOffColliders)
      {
        if (turnOffCollider != null && turnOffCollider.gameObject != null)
          turnOffCollider.gameObject.active = _active;
      }
    }

    public enum InflationState
    {
      inflated,
      deflating,
      deflated,
      inflating,
    }

    public enum Mode
    {
      crumbleOverTimeAfterFirstConnect,
      crumbleMoreOnConnect,
    }
  }
}
