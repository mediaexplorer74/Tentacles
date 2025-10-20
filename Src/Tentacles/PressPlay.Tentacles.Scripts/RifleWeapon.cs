// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.RifleWeapon
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class RifleWeapon : ABaseWeapon
  {
    public BaseBullet ammo;
    public BaseBullet[] ammoList;
    public int preloadAmmo;
    public int[] preloadAmmoList;

    public override void Initialize(ABaseBehaviour _shooter, CannonScript _cannonScript)
    {
      base.Initialize(_shooter, _cannonScript);
      if (!ObjectPool.isLoaded)
      {
        Debug.LogWarning("WARNING. Rifleweapon cannot preload ammo, because ObjectPool is not yet loaded");
      }
      else
      {
        ObjectPool.Instance.Grow((PoolableObject) this.ammo, this.preloadAmmo);
        for (int index = 0; index < this.preloadAmmoList.Length; ++index)
        {
          if (this.ammoList.Length > index)
            ObjectPool.Instance.Grow((PoolableObject) this.ammoList[index], this.preloadAmmoList[index]);
        }
      }
    }

    public void DoInstantFire(bool triggerWeapon, int nozzle, int ammoType)
    {
      this.useSpecificNozzle = nozzle;
      this.ammo = this.GetAmmo(ammoType);
      if (this.ammo == null)
        return;
      this.FireAutomatic();
    }

    private BaseBullet GetAmmo(int ammoType)
    {
      if (ammoType <= this.ammoList.Length - 1)
        return this.ammoList[ammoType];
      Debug.LogError("There is no ammo for index " + (object) ammoType + " defined");
      return (BaseBullet) null;
    }

    protected override void FireWeapon(Vector3 _position, Vector3 _direction, Quaternion _rotation)
    {
      BaseBullet baseBullet = !this.useAmmoCache ? (BaseBullet) UnityObject.Instantiate((UnityObject) this.ammo, _position, _rotation) : (BaseBullet) ObjectPool.Instance.Draw((PoolableObject) this.ammo, _position, _rotation);
      baseBullet.init(this.bulletData, _direction);
      this.shooter.IgnoreCollision(baseBullet.collider, true);
    }
  }
}
