// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TriggerCondition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TriggerCondition : BaseCondition
  {
    public string triggerTag = "Lemmy";
    public Collider triggeredByCollider;

    public override void Start() => this.SetConditionStatus(false);

    public override void OnTriggerEnter(Collider collider)
    {
      if (!(collider.gameObject.tag == this.triggerTag) && collider != this.triggeredByCollider)
        return;
      this.SetConditionStatus(true);
    }

    public override void OnTriggerExit(Collider collider)
    {
      if (!(collider.gameObject.tag == this.triggerTag) && collider != this.triggeredByCollider)
        return;
      this.SetConditionStatus(false);
    }
  }
}
