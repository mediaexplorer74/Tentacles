// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LightningDoor
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LightningDoor : DoorOpenClose
  {
    public LightningRenderer[] lightnings;
    public RaycastLemmyDamager[] damagers;

    protected override void DoOnChangeToClosed()
    {
      for (int index = 0; index < this.damagers.Length; ++index)
      {
        if (this.damagers[index] != null)
          this.damagers[index].doDamage = true;
      }
      for (int index = 0; index < this.lightnings.Length; ++index)
      {
        if (this.lightnings[index] != null)
          this.lightnings[index].ToggleOn(true);
      }
    }

    protected override void DoOnChangeToOpen()
    {
      for (int index = 0; index < this.lightnings.Length; ++index)
      {
        if (this.lightnings[index] != null)
          this.lightnings[index].ToggleOn(false);
      }
      for (int index = 0; index < this.damagers.Length; ++index)
      {
        if (this.damagers[index] != null)
          this.damagers[index].doDamage = false;
      }
    }
  }
}
