// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BaseActivator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BaseActivator : ResetOnLemmyDeath
  {
    private bool _isActivated;
    public BaseCondition[] conditions;

    public void Activate()
    {
      if (this._isActivated)
        return;
      this.DoOnActivate();
      this._isActivated = true;
    }

    public void Deactivate()
    {
      if (!this._isActivated)
        return;
      this.DoOnDeactivate();
      this._isActivated = false;
    }

    protected virtual void DoOnActivate()
    {
    }

    protected virtual void DoOnDeactivate()
    {
    }

    public override void Update()
    {
      for (int index = 0; index < this.conditions.Length; ++index)
      {
        if (this.conditions[index] != null && this.conditions[index].GetConditionStatus())
        {
          this.Activate();
          return;
        }
      }
      this.Deactivate();
    }

    internal override void DoReset() => this.Deactivate();
  }
}
