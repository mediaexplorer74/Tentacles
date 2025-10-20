// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GunController
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GunController : ABaseBehaviour
  {
    public string shootAnimation;
    public RifleWeapon weapon;
    public CannonScript cannon;
    public ShootPattern[] shootPattern;
    public AudioWrapper sndShoot;
    public float sequenceOffset;
    protected float progress;
    protected int currentShootPattern;

    private float sequenceLength => this.shootPattern[this.currentShootPattern].duration;

    public override void Start()
    {
      if (this.shootPattern.Length == 0)
        this.shootPattern = new ShootPattern[1]
        {
          new ShootPattern(0.0f, new bool[1], new int[1])
        };
      this.weapon.Initialize((ABaseBehaviour) this, this.cannon);
    }

    public override void Update()
    {
      this.progress = (LevelHandler.Instance.globalLevelTime - this.sequenceOffset) % this.sequenceLength;
      if ((double) LevelHandler.Instance.globalLevelTime <= (double) this.sequenceOffset || (double) this.progress - (double) LevelHandler.Instance.globalLevelDeltaTime >= 0.0)
        return;
      this.Shoot();
    }

    private void Shoot()
    {
      bool flag = false;
      for (int index = 0; index < this.shootPattern[this.currentShootPattern].shootPattern.Length; ++index)
      {
        if (this.shootPattern[this.currentShootPattern].shootPattern[index])
        {
          this.weapon.DoInstantFire(true, index, this.shootPattern[this.currentShootPattern].ammoType[index]);
          this.cannon.PlayNozzleAnimation(this.shootAnimation, index);
          flag = true;
        }
      }
      if (flag)
        this.sndShoot.PlaySound();
      ++this.currentShootPattern;
      if (this.currentShootPattern <= this.shootPattern.Length - 1)
        return;
      this.currentShootPattern = 0;
    }
  }
}
