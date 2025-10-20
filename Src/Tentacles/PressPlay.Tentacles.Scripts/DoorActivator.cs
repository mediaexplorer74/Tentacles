// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DoorActivator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DoorActivator : BaseActivator
  {
    public BaseCondition overrideOpenCondition;
    public DoorOpenClose door;
    public DoorOpenClose.DoorState activatedState;

    protected override void DoOnActivate()
    {
      base.DoOnActivate();
      if (this.activatedState == DoorOpenClose.DoorState.open)
        this.door.Open();
      else
        this.door.Close();
    }

    protected override void DoOnDeactivate()
    {
      base.DoOnDeactivate();
      if (this.activatedState == DoorOpenClose.DoorState.open)
        this.door.Close();
      else
        this.door.Open();
    }
  }
}
