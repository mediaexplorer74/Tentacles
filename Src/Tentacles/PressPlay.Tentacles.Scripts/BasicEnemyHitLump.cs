// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicEnemyHitLump
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicEnemyHitLump : RipableObject
  {
    public PoolableObject createdOnRip;
    public string animationToPlayOnRip;
    public int numberOfTimesToRepeatAnimation;
    private EnergyCell host;
    public PPAnimationHandler anim;

    public override void Start()
    {
      base.Start();
      GameObject gameObject = this.gameObject;
      while (this.host == null)
      {
        this.host = gameObject.GetComponent<EnergyCell>();
        if (this.host == null)
        {
          if (gameObject.transform.parent != null)
            gameObject = gameObject.transform.parent.gameObject;
          else
            break;
        }
      }
      ObjectPool.Instance.Grow(this.createdOnRip, 1, 1);
    }

    internal override void DoOnRip(ClawBehaviour claw)
    {
      if ((bool) (UnityObject) this.createdOnRip)
        ObjectPool.Instance.Draw(this.createdOnRip, this.transform.position, Quaternion.LookRotation(-claw.transform.forward));
      if (this.trailPrefab != null)
      {
        this.trail = ObjectPool.Instance.Draw(this.trailPrefab, this.transform.position, this.transform.rotation);
        this.trail.transform.parent = this.transform;
      }
      this.soundOnRip.PlaySound();
      LevelHandler.Instance.levelSession.RegisterEyeRip(this);
      this.host.RemoveLump(this);
      claw.BreakConnection();
      claw.Eat((RipableObject) this);
    }
  }
}
