// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ShootingWallCrawler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ShootingWallCrawler : MovingCreature
  {
    public AudioWrapper sndOnOpenShield;
    public AudioWrapper sndOnCloseShield;
    public RifleWeapon weapon;
    public CannonScript cannon;
    public GameObject gfxObject;
    public GameObject shieldObject;
    public float moveSpeed = 1.7f;
    public float beforeShootDelay;
    public float afterShootDelay;
    public float crawlStateLength = 1.3f;
    public WallCrawlMover crawlMover;
    private float shootingWallCrawlerStateDuration;
    private float shootingWallCrawlerStateChangeTime;
    private ShootingWallCrawler.ShootingWallCrawlerState shootingWallCrawlerState;
    private ShootingWallCrawler.ShootingWallCrawlerState nextShootingWallCrawlerState;

    public override void Start()
    {
      base.Start();
      this.weapon.Initialize((ABaseBehaviour) this, this.cannon);
      this.creatureMover.Initialize();
      this.crawlMover.Initialize(this.creatureMover.nodeWrapper);
      if (!this.animHandler.isInitialized)
        this.animHandler.Initialize();
      this.animHandler.Play("idle");
      foreach (Collider componentsInChild in this.shieldObject.GetComponentsInChildren<Collider>())
        componentsInChild.allowTurnOff = true;
    }

    protected override void DoOnChangeStateActive()
    {
      this.ChangeShootingWallCrawlerState(ShootingWallCrawler.ShootingWallCrawlerState.crawling);
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      this.UpdateShootingWallCrawlerState();
    }

    protected void ChangeShootingWallCrawlerState(
      ShootingWallCrawler.ShootingWallCrawlerState _shootingWallCrawlerState)
    {
      this.shootingWallCrawlerStateChangeTime = Time.time;
      this.shootingWallCrawlerState = _shootingWallCrawlerState;
      this.shootingWallCrawlerStateDuration = -1f;
      switch (this.shootingWallCrawlerState)
      {
        case ShootingWallCrawler.ShootingWallCrawlerState.crawling:
          this.animHandler.Play("idle");
          this.SetShieldStatus(true);
          this.crawlMover.moveSpeed = this.moveSpeed;
          this.crawlMover.SetDirection(this.SelectMovementDir());
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.testForShot;
          this.shootingWallCrawlerStateDuration = this.crawlStateLength;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.testForShot:
          this.ChangeShootingWallCrawlerState(ShootingWallCrawler.ShootingWallCrawlerState.openShell);
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.openShell:
          this.sndOnOpenShield.PlaySound();
          this.animHandler.Play("open", new PPAnimationHandler.PPAnimationCallback(this.OpenAnimCallback));
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.beforeShot;
          this.shootingWallCrawlerStateDuration = this.animHandler.currentClipDuration;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.beforeShot:
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.shooting;
          this.shootingWallCrawlerStateDuration = this.beforeShootDelay;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.shooting:
          this.SetShieldStatus(false);
          this.animHandler.Play("shoot");
          this.weapon.DoInstantFire(true, 0);
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.afterShot;
          this.shootingWallCrawlerStateDuration = this.animHandler.currentClipDuration;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.afterShot:
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.closeShell;
          this.shootingWallCrawlerStateDuration = this.afterShootDelay;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.closeShell:
          this.sndOnCloseShield.PlaySound();
          this.animHandler.Play("close", new PPAnimationHandler.PPAnimationCallback(this.CloseAnimCallback));
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.crawling;
          this.shootingWallCrawlerStateDuration = this.animHandler.currentClipDuration;
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.closeShellFast:
          this.animHandler.Play("closeFast", new PPAnimationHandler.PPAnimationCallback(this.CloseAnimCallback));
          this.nextShootingWallCrawlerState = ShootingWallCrawler.ShootingWallCrawlerState.crawling;
          this.shootingWallCrawlerStateDuration = this.animHandler.currentClipDuration;
          break;
      }
    }

    protected void UpdateShootingWallCrawlerState()
    {
      if ((double) this.shootingWallCrawlerStateDuration != -1.0 && (double) Time.time > (double) this.shootingWallCrawlerStateChangeTime + (double) this.shootingWallCrawlerStateDuration)
        this.ChangeShootingWallCrawlerState(this.nextShootingWallCrawlerState);
      switch (this.shootingWallCrawlerState)
      {
        case ShootingWallCrawler.ShootingWallCrawlerState.crawling:
          this.crawlMover.DoMovement();
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.closeShell:
          if ((double) Time.time <= (double) this.shootingWallCrawlerStateChangeTime + 0.5)
            break;
          this.SetShieldStatus(true);
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.closeShellFast:
          if ((double) Time.time <= (double) this.shootingWallCrawlerStateChangeTime + 0.15000000596046448)
            break;
          this.SetShieldStatus(true);
          break;
      }
    }

    public void OpenAnimCallback() => this.SetShieldStatus(false);

    public void CloseAnimCallback() => this.SetShieldStatus(true);

    private WallCrawlMover.MoveDir SelectMovementDir()
    {
      return (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position - this.transform.right).sqrMagnitude < (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position + this.transform.right).sqrMagnitude ? WallCrawlMover.MoveDir.right : WallCrawlMover.MoveDir.left;
    }

    protected void SetShieldStatus(bool _shieldIsOn)
    {
      if (this.shieldObject.active == _shieldIsOn)
        return;
      this.shieldObject.SetActiveRecursively(_shieldIsOn);
    }

    public override void OnLemmyAttack()
    {
      base.OnLemmyAttack();
      switch (this.shootingWallCrawlerState)
      {
        case ShootingWallCrawler.ShootingWallCrawlerState.beforeShot:
          this.ChangeShootingWallCrawlerState(ShootingWallCrawler.ShootingWallCrawlerState.closeShellFast);
          break;
        case ShootingWallCrawler.ShootingWallCrawlerState.afterShot:
          this.ChangeShootingWallCrawlerState(ShootingWallCrawler.ShootingWallCrawlerState.closeShellFast);
          break;
      }
    }

    public enum ShootingWallCrawlerState
    {
      crawling,
      testForShot,
      openShell,
      beforeShot,
      shooting,
      afterShot,
      closeShell,
      closeShellFast,
    }
  }
}
