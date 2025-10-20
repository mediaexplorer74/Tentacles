// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpikeWall
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpikeWall : BasicLemmyDamager
  {
    public GameObject teeth;
    public Vector3 teethMovement;
    private Vector3 teethStartPosition;
    private Vector3 teethEndPosition;
    public BasicTentacleTipCollider tipCollider;
    private float attackStartTime = -1f;
    private float attackDuration = 0.05f;
    private float drawBackDuration = 0.3f;

    public override void Start()
    {
      if (this.tipCollider == null)
        this.tipCollider = this.GetComponent<BasicTentacleTipCollider>();
      this.tipCollider.doOnHitDelegate = new BasicTentacleTipCollider.DoOnHitDelegate(this.DoOnTentacleHit);
      this.teethStartPosition = this.teeth.transform.localPosition;
      this.teethEndPosition = this.teeth.transform.localPosition + this.teethMovement;
    }

    public void DoOnTentacleHit(TentacleTip _tip, Vector3 _hitDir)
    {
      this.attackStartTime = -1f;
      this.Attack();
    }

    internal override void DoOnHitLemmy(Vector3 _hitDir, Vector3 _position)
    {
      base.DoOnHitLemmy(_hitDir, _position);
      this.Attack();
    }

    public override void Update()
    {
      base.Update();
      if ((double) this.attackStartTime == -1.0)
        return;
      if ((double) Time.time < (double) this.attackStartTime + (double) this.attackDuration)
        this.teeth.transform.localPosition = Vector3.Lerp(this.teethStartPosition, this.teethEndPosition, (Time.time - this.attackStartTime) / this.attackDuration);
      else if ((double) Time.time < (double) this.attackStartTime + (double) this.attackDuration + (double) this.drawBackDuration)
      {
        this.teeth.transform.localPosition = Vector3.Lerp(this.teethEndPosition, this.teethStartPosition, (Time.time - (this.attackStartTime + this.attackDuration)) / this.drawBackDuration);
      }
      else
      {
        this.teeth.transform.localPosition = this.teethStartPosition;
        this.attackStartTime = -1f;
        if (!this.isLemmyTouching)
          return;
        this.Attack();
      }
    }

    private void Attack() => this.attackStartTime = Time.time;
  }
}
