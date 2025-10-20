// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WallJumper
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WallJumper : MovingCreature
  {
    public bool neverJump;
    public GameObject gfxObject;
    public float moveSpeed = 1.7f;
    public float jumpSpeed = 1.7f;
    public float testForJumpsDuration = 0.3f;
    public float beforeJumpDelay = 0.1f;
    public float afterJumpDelay = 0.3f;
    public float checkForJumpInterval;
    public float maxJumpLength;
    public WallCrawlMover crawlMover;
    private JumpPosition currentJumpPosition;
    private CreatureMoverNode jumpTargetNode;
    private float wallJumperStateDuration;
    private float wallJumperStateChangeTime;
    private WallJumper.WallJumperState wallJumperState;
    private WallJumper.WallJumperState nextWallJumperState;

    public override void Start()
    {
      base.Start();
      if (this.jumpTargetNode == null)
        this.jumpTargetNode = CreatureToolbox.CreateCreatureMoverNode();
      this.creatureMover.Initialize();
      this.crawlMover.Initialize(this.creatureMover.nodeWrapper);
      if ((bool) (UnityObject) this.animHandler && !this.animHandler.isInitialized)
        this.animHandler.Initialize();
      if (!(bool) (UnityObject) this.animHandler)
        return;
      this.animHandler.Play("idle");
    }

    protected override void DoOnChangeStateActive()
    {
      this.ChangeWallJumperState(WallJumper.WallJumperState.crawling);
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      this.UpdateWallJumperState();
    }

    protected void ChangeWallJumperState(WallJumper.WallJumperState _wallJumperState)
    {
      this.wallJumperStateChangeTime = Time.time;
      this.wallJumperState = _wallJumperState;
      this.wallJumperStateDuration = -1f;
      switch (this.wallJumperState)
      {
        case WallJumper.WallJumperState.crawling:
          this.SetShieldStatus(true);
          this.crawlMover.moveSpeed = this.moveSpeed;
          this.crawlMover.SetDirection(this.SelectMovementDir());
          if (this.neverJump)
            break;
          this.nextWallJumperState = WallJumper.WallJumperState.testForJump;
          this.wallJumperStateDuration = this.checkForJumpInterval;
          break;
        case WallJumper.WallJumperState.beforeJump:
          this.SetShieldStatus(false);
          if ((bool) (UnityObject) this.animHandler)
            this.animHandler.Play("jumpPrepare", new PPAnimationHandler.PPAnimationCallback(this.JumpPrepareAnimCallback));
          this.nextWallJumperState = WallJumper.WallJumperState.jumping;
          this.wallJumperStateDuration = this.beforeJumpDelay;
          break;
        case WallJumper.WallJumperState.jumping:
          this.SetShieldStatus(false);
          if ((bool) (UnityObject) this.animHandler)
            this.animHandler.Play("jump", new PPAnimationHandler.PPAnimationCallback(this.JumpOpenAnimCallback));
          this.transform.rotation = this.currentJumpPosition.rotation;
          this.jumpTargetNode.transform.position = this.currentJumpPosition.position;
          this.jumpTargetNode.transform.rotation = this.currentJumpPosition.rotation;
          this.creatureMover.BurstMoveToNode(this.jumpTargetNode, new CreatureMover.MoveFinished(this.JumpCompletedCallback), this.jumpSpeed);
          break;
        case WallJumper.WallJumperState.afterJump:
          this.SetShieldStatus(false);
          if ((bool) (UnityObject) this.animHandler)
            this.animHandler.Play("land", new PPAnimationHandler.PPAnimationCallback(this.JumpCloseAnimCallback));
          this.nextWallJumperState = WallJumper.WallJumperState.crawling;
          this.wallJumperStateDuration = this.afterJumpDelay;
          break;
      }
    }

    public void JumpPrepareAnimCallback()
    {
      if (!(bool) (UnityObject) this.animHandler)
        return;
      this.animHandler.Play("jumpSuspense");
    }

    public void JumpOpenAnimCallback()
    {
      if (!(bool) (UnityObject) this.animHandler)
        return;
      this.animHandler.Play("airIdle");
    }

    public void JumpCloseAnimCallback()
    {
      if (!(bool) (UnityObject) this.animHandler)
        return;
      this.animHandler.Play("idle");
    }

    public void JumpCompletedCallback()
    {
      this.ChangeWallJumperState(WallJumper.WallJumperState.afterJump);
    }

    protected void UpdateWallJumperState()
    {
      if ((double) this.wallJumperStateDuration != -1.0 && (double) Time.time > (double) this.wallJumperStateChangeTime + (double) this.wallJumperStateDuration)
        this.ChangeWallJumperState(this.nextWallJumperState);
      switch (this.wallJumperState)
      {
        case WallJumper.WallJumperState.crawling:
          if (this.crawlMover.moveDir == WallCrawlMover.MoveDir.right)
            this.gfxObject.transform.Rotate(Vector3.up, 60f * this.moveSpeed * Time.deltaTime, Space.Self);
          else
            this.gfxObject.transform.Rotate(Vector3.up, -60f * this.moveSpeed * Time.deltaTime, Space.Self);
          this.crawlMover.DoMovement();
          break;
        case WallJumper.WallJumperState.testForJump:
          this.ray.origin = this.transform.position;
          this.ray.direction = this.transform.forward;
          this.currentJumpPosition = this.RaycastForJumpPosition(this.ray, this.maxJumpLength);
          if (this.currentJumpPosition.jumpPossible)
          {
            this.ChangeWallJumperState(WallJumper.WallJumperState.beforeJump);
            break;
          }
          this.ChangeWallJumperState(WallJumper.WallJumperState.crawling);
          break;
      }
    }

    private WallCrawlMover.MoveDir SelectMovementDir()
    {
      return (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position - this.transform.right).sqrMagnitude < (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position + this.transform.right).sqrMagnitude ? WallCrawlMover.MoveDir.right : WallCrawlMover.MoveDir.left;
    }

    protected JumpPosition RaycastForJumpPosition(Ray _ray, float _maxDistance)
    {
      Debug.DrawRay(_ray.origin, _ray.direction * _maxDistance, Color.green);
      JumpPosition jumpPosition = new JumpPosition();
      if (Physics.Raycast(_ray, out this.rh, _maxDistance, (int) GlobalSettings.Instance.allWallsLayers))
      {
        if (!this.creatureMover.nodeWrapper.IsPositionInMovementArea(this.rh.point))
        {
          jumpPosition.jumpPossible = false;
          return jumpPosition;
        }
        jumpPosition.position = this.rh.point + this.rh.normal * this.crawlMover.wallDist;
        jumpPosition.rotation = Quaternion.LookRotation(this.rh.normal);
        jumpPosition.jumpPossible = true;
        return jumpPosition;
      }
      jumpPosition.jumpPossible = false;
      return jumpPosition;
    }

    protected void SetShieldStatus(bool _shieldIsOn) => this.life.SetVulnerability(!_shieldIsOn);

    public enum WallJumperState
    {
      crawling,
      testForJump,
      beforeJump,
      jumping,
      afterJump,
    }
  }
}
