// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BaseCondition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BaseCondition : ResetOnLemmyDeath
  {
    public bool conditionReversible;
    private bool _conditionStatus;

    public bool GetConditionStatus() => this._conditionStatus;

    protected void SetConditionStatus(bool _status)
    {
      if (this._conditionStatus && !this.conditionReversible)
        return;
      if (_status != this._conditionStatus)
        this.DoOnChangeConditionStatus(_status);
      this._conditionStatus = _status;
    }

    protected virtual void DoOnChangeConditionStatus(bool newStatus)
    {
    }

    internal override void DoReset()
    {
      base.DoReset();
      this.DoOnChangeConditionStatus(false);
      this._conditionStatus = false;
    }
  }
}
