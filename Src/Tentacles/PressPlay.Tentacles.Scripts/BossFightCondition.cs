// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BossFightCondition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BossFightCondition : BaseCondition
  {
    public BaseCreature[] enemies;
    protected bool isComplete;

    private bool isDead
    {
      get
      {
        foreach (BaseCreature enemy in this.enemies)
        {
          if (enemy != null && !enemy.isInitialized || enemy != null && enemy.gameObject != null && !enemy.gameObject.active || enemy != null && !enemy.life.isDead)
            return false;
        }
        return true;
      }
    }

    public override void Update()
    {
      if (!this.conditionReversible && this.GetConditionStatus() || !this.isDead)
        return;
      this.SetConditionStatus(true);
    }
  }
}
