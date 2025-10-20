// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleTipDamager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleTipDamager : BasicTentacleTipCollider
  {
    public TentacleTipCollisionStats damageStats;
    public bool doDamage = true;

    public void OverrideDamageStats(TentacleTipCollisionStats _stats) => this.damageStats = _stats;

    protected override void DoOnHit(TentacleTip _tip, Vector3 _hitDir)
    {
      base.DoOnHit(_tip, _hitDir);
      if (!this.doDamage)
        return;
      LevelHandler.Instance.lemmy.Damage(this.damageStats.onHitdamage, _hitDir);
      if (this.damageStats.breakConnection)
        _tip.BreakConnection(this.damageStats.pushAwayFromConnection);
      _tip.Push(this.damageStats.pushAwayFromCollider * _hitDir * Time.deltaTime);
    }
  }
}
