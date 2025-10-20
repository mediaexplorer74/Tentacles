// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ABaseBehaviour
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class ABaseBehaviour : PoolableObject
  {
    public Collider[] colliders;

    public void IgnoreCollision(Collider other, bool ignore)
    {
      foreach (Collider collider in this.colliders)
        Physics.IgnoreCollision(other, collider, ignore);
    }

    public void IgnoreCollision(Collider[] others, bool ignore)
    {
      foreach (Collider other in others)
        this.IgnoreCollision(other, ignore);
    }

    public bool HasCollider(Collider other)
    {
      foreach (Collider collider in this.colliders)
      {
        if (other == collider)
          return true;
      }
      return false;
    }
  }
}
