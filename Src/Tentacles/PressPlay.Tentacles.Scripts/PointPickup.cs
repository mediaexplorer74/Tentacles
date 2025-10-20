// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PointPickup
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PointPickup : PoolablePickup
  {
    public PointPickup.PickupType type;
    public int point = 1;
    public int health = 5;
    public float multiplierFactor = 0.1f;

    protected override void DoOnPickUpAction(Lemmy lemmy)
    {
      LevelHandler.Instance.levelSession.RegisterPickup(this.point, this.type, this.transform.position);
      lemmy.AddHealth((float) this.health);
    }

    public override void StartLemmyDrag() => base.StartLemmyDrag();

    public enum PickupType
    {
      collectable,
      point,
    }
  }
}
