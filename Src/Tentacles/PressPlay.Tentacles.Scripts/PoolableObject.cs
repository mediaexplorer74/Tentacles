// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PoolableObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PoolableObject : MonoBehaviour, IPoolable
  {
    public string _guid;
    protected bool hasBeenActivated;

    [ContentSerializerIgnore]
    public string guid
    {
      get => this._guid;
      set => this._guid = value;
    }

    public virtual void Create() => this.gameObject.SetActiveRecursively(false);

    public virtual void Activate()
    {
      this.gameObject.SetActiveRecursively(true);
      this.hasBeenActivated = true;
    }

    public virtual void DeActivate()
    {
      this.transform.parent = (Transform) null;
      this.gameObject.SetActiveRecursively(false);
    }

    public virtual void Return() => ObjectPool.Instance.Return(this);

    public virtual void Remove()
    {
    }
  }
}
