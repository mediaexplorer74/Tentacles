// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpikyWallCrawler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpikyWallCrawler : MovingCreature
  {
    public float moveSpeed = 1.7f;
    public float selectDirectionInterval = 0.45f;
    public bool doChangeDirectionTest = true;
    public bool moveTowardsLemmyOnChangeDirection = true;
    public WallCrawlMover crawlMover;
    private float spikyWallCrawlerStateDuration;
    private float spikyWallCrawlerStateChangeTime;
    private SpikyWallCrawler.SpikyWallCrawlerState spikyWallCrawlerState;
    private SpikyWallCrawler.SpikyWallCrawlerState nextSpikyWallCrawlerState;

    public override void Start()
    {
      base.Start();
      this.creatureMover.Initialize();
      this.crawlMover.Initialize(this.creatureMover.nodeWrapper);
      if (!this.animHandler.isInitialized)
        this.animHandler.Initialize();
      this.animHandler.Play("idle");
    }

    protected override void DoOnChangeStateActive()
    {
      this.ChangeSpikyWallCrawlerState(SpikyWallCrawler.SpikyWallCrawlerState.selectDirection);
    }

    protected override void UpdateStateActive()
    {
      base.UpdateStateActive();
      this.UpdateSpikyWallCrawlerState();
    }

    protected void ChangeSpikyWallCrawlerState(
      SpikyWallCrawler.SpikyWallCrawlerState _spikyWallCrawlerState)
    {
      this.spikyWallCrawlerStateChangeTime = Time.time;
      this.spikyWallCrawlerState = _spikyWallCrawlerState;
      this.spikyWallCrawlerStateDuration = -1f;
      switch (this.spikyWallCrawlerState)
      {
        case SpikyWallCrawler.SpikyWallCrawlerState.crawling:
          this.animHandler.Play("idle");
          this.crawlMover.moveSpeed = this.moveSpeed;
          this.nextSpikyWallCrawlerState = SpikyWallCrawler.SpikyWallCrawlerState.selectDirection;
          this.spikyWallCrawlerStateDuration = this.selectDirectionInterval;
          break;
        case SpikyWallCrawler.SpikyWallCrawlerState.selectDirection:
          if (this.doChangeDirectionTest)
          {
            if (this.moveTowardsLemmyOnChangeDirection)
              this.crawlMover.SetDirection(this.SelectMovementDir());
            else
              this.crawlMover.SwitchDirection();
          }
          this.ChangeSpikyWallCrawlerState(SpikyWallCrawler.SpikyWallCrawlerState.crawling);
          break;
      }
    }

    protected void UpdateSpikyWallCrawlerState()
    {
      if ((double) this.spikyWallCrawlerStateDuration != -1.0 && (double) Time.time > (double) this.spikyWallCrawlerStateChangeTime + (double) this.spikyWallCrawlerStateDuration)
        this.ChangeSpikyWallCrawlerState(this.nextSpikyWallCrawlerState);
      if (this.spikyWallCrawlerState != SpikyWallCrawler.SpikyWallCrawlerState.crawling)
        return;
      this.crawlMover.DoMovement();
    }

    private WallCrawlMover.MoveDir SelectMovementDir()
    {
      return (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position - this.transform.right).sqrMagnitude < (double) (LevelHandler.Instance.lemmy.transform.position - this.transform.position + this.transform.right).sqrMagnitude ? WallCrawlMover.MoveDir.right : WallCrawlMover.MoveDir.left;
    }

    public enum SpikyWallCrawlerState
    {
      crawling,
      selectDirection,
      attack,
    }
  }
}
