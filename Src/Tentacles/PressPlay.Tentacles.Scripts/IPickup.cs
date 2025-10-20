// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.IPickup
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public interface IPickup
  {
    bool hasBeenPickedUp { get; }

    void DoOnCollision(Lemmy lemmy);

    void DoGrabPickUp(Lemmy lemmy);
  }
}
