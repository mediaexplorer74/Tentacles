// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ChallengeTrigger
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ChallengeTrigger : TriggeredByLemmy
  {
    public ChallengeTrigger.ChallengeTriggerMode mode;
    public Challenge challenge;
    protected bool challengeHasBeenTriggered;

    protected override void DoOnTrigger()
    {
      if (this.challengeHasBeenTriggered)
        return;
      if (this.mode == ChallengeTrigger.ChallengeTriggerMode.start)
        LevelHandler.Instance.challengeHandler.StartChallenge(this.challenge);
      else
        LevelHandler.Instance.challengeHandler.StopChallenge();
      this.challengeHasBeenTriggered = true;
    }

    public void Copy(ChallengeTrigger c)
    {
      this.challenge = c.challenge;
      this.mode = c.mode;
    }

    public enum ChallengeTriggerMode
    {
      start,
      end,
    }
  }
}
