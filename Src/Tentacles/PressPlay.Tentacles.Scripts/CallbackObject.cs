// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CallbackObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CallbackObject : MonoBehaviour
  {
    public CallbackObject.CallbackCompleteMethod onComplete;
    public CallbackObject.CallbackUpdateMethod onUpdate;

    public void DoOnComplete()
    {
      if (this.onComplete == null)
        return;
      this.onComplete();
    }

    public void DoOnUpdate(float value)
    {
      if (this.onUpdate != null)
        this.onUpdate(value);
      else
        Debug.Log((object) "OnUpdate is NULL");
    }

    public void Reset()
    {
      this.onComplete = (CallbackObject.CallbackCompleteMethod) null;
      this.onUpdate = (CallbackObject.CallbackUpdateMethod) null;
    }

    public delegate void CallbackCompleteMethod();

    public delegate void CallbackUpdateMethod(float value);
  }
}
