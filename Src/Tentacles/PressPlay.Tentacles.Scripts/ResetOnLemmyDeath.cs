// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ResetOnLemmyDeath
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ResetOnLemmyDeath : TTBaseBehavior
  {
    public ResetOnLemmyDeath.ResetTiming resetTiming = ResetOnLemmyDeath.ResetTiming.BeforeRespawn;
    public bool doResetOnLemmyDeath = true;

    public void Reset()
    {
      if (!this.doResetOnLemmyDeath)
        return;
      if (this == null)
        Debug.Log((object) "Trying to reset something that has been destroyed!!");
      else
        this.DoReset();
    }

    internal virtual void DoReset()
    {
    }

    public enum ResetTiming
    {
      OnDeath,
      BeforeRespawn,
      AfterRespawn,
    }
  }
}
