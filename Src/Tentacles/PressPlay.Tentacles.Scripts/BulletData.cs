// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BulletData
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BulletData
  {
    public float damage;
    private float _charge = 1f;
    public ABaseBehaviour shooter;
    public string weaponName = "NoWeapon";
    public Vector3 startPosition;
    public float killDistance;

    public float charge
    {
      get => this._charge;
      set
      {
        this.damage *= value;
        this._charge = value;
      }
    }

    public BulletData(float damage, ABaseBehaviour shooter, string weaponName)
    {
      this.damage = damage;
      this.shooter = shooter;
      this.weaponName = weaponName;
      this.startPosition = shooter.transform.position;
    }
  }
}
