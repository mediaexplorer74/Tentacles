// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ABaseWeapon
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class ABaseWeapon : MonoBehaviour
  {
    [ContentSerializerIgnore]
    public ABaseWeapon.ABaseWeaponCallback OnWeaponFireCallback;
    public bool useAmmoCache = true;
    public int bulletDamage = 50;
    public Microsoft.Xna.Framework.Vector3 random = new Microsoft.Xna.Framework.Vector3();
    protected int useSpecificNozzle = -1;
    protected CannonScript _cannon;
    protected ABaseBehaviour shooter;
    protected BulletData bulletData;

    [ContentSerializerIgnore]
    public CannonScript cannon => this._cannon;

    public virtual void Initialize(ABaseBehaviour _shooter, CannonScript _cannonScript)
    {
      this.shooter = _shooter;
      this._cannon = _cannonScript;
    }

    public virtual void DoInstantFire(bool triggerWeapon, int nozzle)
    {
      this.useSpecificNozzle = nozzle;
      this.FireAutomatic();
    }

    protected virtual void FireAutomatic()
    {
      this.Fire(this.cannon.getNozzlePosition(), this.cannon.getNozzleDirection(), this.cannon.getNozzleRotation());
    }

    protected void Fire(Microsoft.Xna.Framework.Vector3 _position, Microsoft.Xna.Framework.Vector3 _direction, Microsoft.Xna.Framework.Quaternion _rotation)
    {
      if (this.useSpecificNozzle != -1)
      {
        _position = this.cannon.getNozzlePosition(this.useSpecificNozzle);
        _direction = this.cannon.getNozzleDirection(this.useSpecificNozzle);
        _rotation = this.cannon.getNozzleRotation(this.useSpecificNozzle);
      }
      this.bulletData = new BulletData((float) this.bulletDamage, this.shooter, "");
      if ((double) this.random.Length() > 0.0)
      {
        Microsoft.Xna.Framework.Vector3 vector3 = new Microsoft.Xna.Framework.Vector3(_direction.X + Microsoft.Xna.Framework.MathHelper.Lerp(-this.random.X, this.random.X, (float) new System.Random().NextDouble()), _direction.Y + Microsoft.Xna.Framework.MathHelper.Lerp(-this.random.Y, this.random.Y, (float) new System.Random().NextDouble()), _direction.Z + Microsoft.Xna.Framework.MathHelper.Lerp(-this.random.Z, this.random.Z, (float) new System.Random().NextDouble()));
        this.FireWeapon(_position, _direction + vector3, _rotation);
      }
      else
        this.FireWeapon(_position, _direction, _rotation);
      if (this.OnWeaponFireCallback != null)
        this.OnWeaponFireCallback();
      this.cannon.NextNozzle();
    }

    protected abstract void FireWeapon(Microsoft.Xna.Framework.Vector3 _position, Microsoft.Xna.Framework.Vector3 _direction, Microsoft.Xna.Framework.Quaternion _rotation);

    public virtual void DestroyWeapon() => UnityObject.Destroy((UnityObject) this.gameObject);

    public delegate void ABaseWeaponCallback();
  }
}
