// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LemmySeekingBullet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LemmySeekingBullet : MovingBullet
  {
    public float turnRate = 1f;
    public Transform target;

    protected override void DoUpdate()
    {
      this.velocity = (this.velocity + (this.target.position - this.transform.position).normalized * this.turnRate).normalized * this.velocity.magnitude;
      base.DoUpdate();
    }

    public override void init(BulletData _data, Vector3 _direction, float _charge)
    {
      base.init(_data, _direction, _charge);
      this.target = LevelHandler.Instance.lemmy.transform;
    }
  }
}
