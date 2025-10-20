// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ShootPattern
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ShootPattern
  {
    public float duration;
    public int[] ammoType;
    public bool[] shootPattern;

    public ShootPattern()
    {
    }

    public ShootPattern(float duration, bool[] shootPattern, int[] ammoType)
    {
      this.duration = duration;
      this.shootPattern = shootPattern;
      this.ammoType = ammoType;
    }
  }
}
