// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Mine
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Mine : ResetOnLemmyDeath
  {
    public AudioWrapper sndOnActivate;
    public AudioWrapper sndOnExplode;
    public GameObject[] objectsToTurnOffWhenDead;
    public UVSpriteSheetAnimator uvSpriteSheetAnim;
    public bool resetAfterExplosion;
    public float chargeBeforeExplosion = 2f;
    protected float currentCharge;
    protected float mineStateDuration;
    protected float mineStateChangeTime;
    protected Mine.MineState nextMineState;
    protected Mine.MineState mineState;

    protected virtual bool MineActivationCheck() => this.LemmyProximityCheck(4f);

    protected void ChangeMineState(Mine.MineState _newMinState)
    {
      this.mineStateDuration = -1f;
      this.mineState = _newMinState;
      this.mineStateChangeTime = Time.time;
      switch (this.mineState)
      {
        case Mine.MineState.idle:
          foreach (GameObject gameObject in this.objectsToTurnOffWhenDead)
            gameObject.SetActiveRecursively(true);
          this.DoOnChangeMineToIdle();
          break;
        case Mine.MineState.charging:
          this.sndOnActivate.PlaySound();
          this.DoOnChangeMineToCharging();
          break;
        case Mine.MineState.exploding:
          this.sndOnExplode.PlaySound();
          this.DoOnChangeMineToExploding();
          break;
        case Mine.MineState.afterExplosion:
          foreach (GameObject gameObject in this.objectsToTurnOffWhenDead)
            gameObject.SetActiveRecursively(false);
          this.DoOnChangeMineToAfterExplosion();
          break;
      }
    }

    public void OnTurnOnAtDistance()
    {
      if (this.mineState == Mine.MineState.idle)
        return;
      this.ChangeMineState(Mine.MineState.afterExplosion);
    }

    public void OnTurnOffAtDistance()
    {
      if (this.mineState == Mine.MineState.idle)
        return;
      this.ChangeMineState(Mine.MineState.afterExplosion);
    }

    protected void UpdateMineState()
    {
      if ((double) this.mineStateDuration != -1.0 && (double) Time.time > (double) this.mineStateChangeTime + (double) this.mineStateDuration)
        this.ChangeMineState(this.nextMineState);
      switch (this.mineState)
      {
        case Mine.MineState.idle:
          this.DoOnUpdateMineIdle();
          break;
        case Mine.MineState.charging:
          this.DoOnUpdateMineCharging();
          break;
        case Mine.MineState.exploding:
          this.DoOnUpdateMineExploding();
          break;
        case Mine.MineState.afterExplosion:
          if (this.resetAfterExplosion)
            this.ChangeMineState(Mine.MineState.idle);
          this.DoOnUpdateMineAfterExplosion();
          break;
      }
    }

    protected virtual void DoOnChangeMineToIdle()
    {
      this.gameObject.SetActiveRecursively(true);
      if (this.uvSpriteSheetAnim != null)
        this.uvSpriteSheetAnim.Play("Idle");
      this.currentCharge = 0.0f;
    }

    protected virtual void DoOnChangeMineToCharging()
    {
      if (this.uvSpriteSheetAnim == null)
        return;
      this.uvSpriteSheetAnim.Play("Charging");
    }

    protected virtual void DoOnChangeMineToExploding()
    {
    }

    protected virtual void DoOnChangeMineToAfterExplosion()
    {
    }

    protected virtual void DoOnUpdateMineIdle()
    {
      if (!this.MineActivationCheck())
        return;
      this.ChangeMineState(Mine.MineState.charging);
    }

    protected virtual void DoOnUpdateMineCharging()
    {
      this.currentCharge += Time.deltaTime;
      if ((double) this.currentCharge >= (double) this.chargeBeforeExplosion)
        this.ChangeMineState(Mine.MineState.exploding);
      else
        this.transform.localScale = Vector3.one * (float) (1.0 + (double) Mathf.Cos((float) ((double) (Time.time - this.mineStateChangeTime) * 20.0 / (0.5 + (double) this.chargeBeforeExplosion - (double) this.currentCharge))) * 0.15000000596046448);
    }

    protected virtual void DoOnUpdateMineExploding()
    {
      this.ChangeMineState(Mine.MineState.afterExplosion);
    }

    protected virtual void DoOnUpdateMineAfterExplosion()
    {
    }

    public override void Update() => this.UpdateMineState();

    protected bool LemmyProximityCheck(float radius)
    {
      return (double) (this.transform.position - LevelHandler.Instance.lemmy.transform.position).magnitude < (double) radius;
    }

    protected void PushAndDamageLemmy(float _damage, Vector3 _push)
    {
      LevelHandler.Instance.lemmy.Push(_push);
      LevelHandler.Instance.lemmy.Damage(_damage, _push);
    }

    protected void ExplosiveDamageLemmy(
      float _damage,
      float _push,
      Vector3 _origin,
      float _radius,
      bool _breakConnections)
    {
      Vector3 _direction = LevelHandler.Instance.lemmy.transform.position - _origin;
      float magnitude = _direction.magnitude;
      if ((double) magnitude > (double) _radius)
        return;
      if (_breakConnections)
        LevelHandler.Instance.lemmy.BreakConnections();
      _direction = (_radius - magnitude) * _direction.normalized;
      LevelHandler.Instance.lemmy.Push(_direction * _push);
      LevelHandler.Instance.lemmy.Damage(_damage * (_radius - magnitude), _direction);
    }

    internal override void DoReset()
    {
      foreach (GameObject gameObject in this.objectsToTurnOffWhenDead)
        gameObject.SetActiveRecursively(true);
      this.ChangeMineState(Mine.MineState.idle);
    }

    public enum MineState
    {
      idle,
      charging,
      exploding,
      afterExplosion,
    }
  }
}
