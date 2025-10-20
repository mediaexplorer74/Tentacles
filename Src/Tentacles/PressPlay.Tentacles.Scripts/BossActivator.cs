// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BossActivator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BossActivator : BaseActivator
  {
    public BaseCreature creature;

    protected override void DoOnActivate()
    {
      if (this.creature == null)
        return;
      this.creature.Activate();
    }

    protected override void DoOnDeactivate()
    {
      if (this.creature == null)
        return;
      this.creature.DeActivate();
    }
  }
}
