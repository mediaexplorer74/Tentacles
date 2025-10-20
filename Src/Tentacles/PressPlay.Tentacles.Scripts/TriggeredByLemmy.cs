// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TriggeredByLemmy
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TriggeredByLemmy : MonoBehaviour
  {
    protected bool _hasBeenTriggered;

    public void Trigger()
    {
      if (this._hasBeenTriggered)
        return;
      this.DoOnTrigger();
      this._hasBeenTriggered = true;
    }

    protected virtual void DoOnTrigger()
    {
    }
  }
}
