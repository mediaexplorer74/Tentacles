// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.OnClawBehaviourConnect
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class OnClawBehaviourConnect : MonoBehaviour
  {
    [ContentSerializerIgnore]
    private OnClawBehaviourConnect.DoOnConnectDelegate _doOnConnectDelegate;

    [ContentSerializerIgnore]
    public OnClawBehaviourConnect.DoOnConnectDelegate doOnConnectDelegate
    {
      get => this._doOnConnectDelegate;
      set => this._doOnConnectDelegate = value;
    }

    public virtual void DoOnClawBehaviourConnect(ClawBehaviour _clawBehaviour, Vector3 _hitDir)
    {
      if (this.doOnConnectDelegate == null)
        return;
      this.doOnConnectDelegate(_clawBehaviour, _hitDir);
    }

    public delegate void DoOnConnectDelegate(ClawBehaviour _clawBehaviour, Vector3 _hitDir);
  }
}
