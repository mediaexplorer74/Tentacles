// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperShooterTentacle
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperShooterTentacle : BaseCreature
  {
    public GameObject nozzle;
    public GameObject[] objectsToDeactivate;
    public TentacleJoint body;
    public TentacleJoint tip;
    public float tentacleLength = 2f;
    private SimpleLineDrawer lineDrawer;
    public TentacleVisualStats visualStats;
    private Vector3 activePosition;
    private Transform startPosition;
    private Transform endPosition;
    private SuperShooter boss;
    private GameObject gotoObj;
    private bool isExtended;

    public bool isFullyExtended
    {
      get
      {
        return (double) (this.tip.transform.position - this.endPosition.position).sqrMagnitude < 0.10000000149011612;
      }
    }

    public bool isDead
    {
      get
      {
        return this.life.isDead || this.creatureState == BaseCreature.CreatureState.dying || this.creatureState == BaseCreature.CreatureState.sleeping;
      }
    }

    public void Initialize(SuperShooter boss, Transform startPosition, Transform endPosition)
    {
      this.boss = boss;
      this.startPosition = startPosition;
      this.endPosition = endPosition;
      this.gotoObj = new GameObject();
      this.gotoObj.transform.parent = this.transform;
      this.gotoObj.transform.position = startPosition.position;
      this.lineDrawer = (SimpleLineDrawer) this.GetComponent(typeof (SimpleLineDrawer));
      this.lineDrawer.startWidth = this.visualStats.startWidth;
      this.lineDrawer.endWidth = this.visualStats.endWidth;
      this.lineDrawer.start = this.body.transform;
      this.lineDrawer.end = this.tip.transform;
      this.lineDrawer.Initialize();
      this.body.transform.parent = startPosition;
      this.tip.transform.position = startPosition.position;
      this.tip.collider.allowTurnOff = true;
      this.Extend();
      this.ChangeState(BaseCreature.CreatureState.active);
    }

    public void Extend()
    {
      this.gotoObj.transform.position = this.endPosition.position;
      this.gotoObj.transform.parent = this.transform;
      this.isExtended = true;
    }

    public void Contract()
    {
      this.gotoObj.transform.position = this.startPosition.position;
      this.gotoObj.transform.parent = this.startPosition;
      this.isExtended = false;
    }

    public void BringToLife()
    {
      if (this.life.isDead)
        this.life.ResetHealth();
      this.Extend();
      this.ChangeState(BaseCreature.CreatureState.active);
    }

    public override void OnLemmyAttack()
    {
      base.OnLemmyAttack();
      this.boss.PlayDamageAnimation();
      if (!this.life.isDead)
        return;
      this.Contract();
    }

    public override void FixedUpdate()
    {
      this.Update();
      if (this.isExtended)
      {
        this.gotoObj.transform.position = this.endPosition.position;
        this.gotoObj.transform.parent = this.transform;
      }
      else
      {
        this.gotoObj.transform.position = this.startPosition.position;
        this.gotoObj.transform.parent = this.startPosition;
      }
      this.tip.transform.position = Vector3.Lerp(this.tip.transform.position, this.gotoObj.transform.position, Time.deltaTime * 5f);
    }

    private void ToggleVisibility(bool on)
    {
      foreach (GameObject gameObject in this.objectsToDeactivate)
        gameObject.SetActiveRecursively(on);
    }

    protected override void Kill()
    {
    }

    protected override void UpdateStateDying() => this.ChangeState(BaseCreature.CreatureState.dead);

    protected override void UpdateStateDead()
    {
      this.ChangeState(BaseCreature.CreatureState.sleeping);
    }

    protected override void DoOnChangeStateSleeping()
    {
    }

    protected override void DoOnChangeStateWaking()
    {
    }

    protected override void DoOnChangeStateDying()
    {
    }

    protected override void DoOnChangeStateDead()
    {
    }
  }
}
