// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.OutroSequence
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class OutroSequence : MonoBehaviour
  {
    private CreditsScreen credits;
    public BaseCondition bossDeadCondition;
    public float afterBossDeathConfettiDelay = 2.5f;
    public float afterConfettiEndScreenDelay = 2.5f;
    public FireworksSpawn[] fireworks;
    private int fireworksIndex;
    private float fireworksInterval = 0.2f;
    private float lastFireworksTime;
    private bool fireworksStarted;
    public MoviePlayer moviePlayer;
    public MoviePlayer.Movies outroMovie;
    private float stateChangeTime;
    public OutroSequence.OutroSequenceState outroSequenceState;

    public override void Start()
    {
    }

    private void ChangeState(
      OutroSequence.OutroSequenceState _outroSequenceState)
    {
      this.stateChangeTime = Time.time;
      this.outroSequenceState = _outroSequenceState;
      switch (this.outroSequenceState)
      {
        case OutroSequence.OutroSequenceState.bossDead:
          LevelHandler.Instance.lemmy.SetInvulnerable(-1f);
          LevelHandler.Instance.ChangeState(LevelHandler.LevelState.gameWonBeforeEndScreen);
          break;
        case OutroSequence.OutroSequenceState.youMadeItConfetti:
          this.fireworksStarted = true;
          break;
        case OutroSequence.OutroSequenceState.endScreenOpen:
          LevelHandler.Instance.ChangeState(LevelHandler.LevelState.gameWonEndScreenOpen);
          break;
        case OutroSequence.OutroSequenceState.fadeToMovie:
          GlobalManager.Instance.fullscreenImageHandler.FadeToBlack(0.8f, this.gameObject, "FadeToMovieCallback");
          break;
        case OutroSequence.OutroSequenceState.playingMovie:
          this.fireworksStarted = false;
          this.moviePlayer.TestZuneBeforePlayingMovie(this.outroMovie);
          break;
        case OutroSequence.OutroSequenceState.inCredits:
          this.credits = new CreditsScreen("Textures/Menu/Credits/credits_bg");
          Application.screenManager.AddScreen((GameScreen) this.credits);
          break;
        case OutroSequence.OutroSequenceState.done:
          GlobalManager.Instance.fullscreenImageHandler.DoInstantBlackScreen();
          break;
      }
    }

    public override void Update()
    {
      if (this.fireworksStarted)
      {
        double time = (double) Time.time;
        double num = (double) this.lastFireworksTime + (double) this.fireworksInterval;
      }
      switch (this.outroSequenceState)
      {
        case OutroSequence.OutroSequenceState.notStarted:
          if (!this.bossDeadCondition.GetConditionStatus())
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.bossDead);
          break;
        case OutroSequence.OutroSequenceState.bossDead:
          if ((double) Time.time <= (double) this.afterBossDeathConfettiDelay + (double) this.stateChangeTime)
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.youMadeItConfetti);
          break;
        case OutroSequence.OutroSequenceState.youMadeItConfetti:
          if ((double) Time.time <= (double) this.afterConfettiEndScreenDelay + (double) this.stateChangeTime)
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.endScreenOpen);
          break;
        case OutroSequence.OutroSequenceState.endScreenOpen:
          if (LevelHandler.Instance.state != LevelHandler.LevelState.gameWonAfterEndScreen)
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.fadeToMovie);
          break;
        case OutroSequence.OutroSequenceState.playingMovie:
          if (!this.moviePlayer.isDonePlaying)
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.inCredits);
          break;
        case OutroSequence.OutroSequenceState.inCredits:
          if (!this.credits.transitionExitComplete)
            break;
          this.ChangeState(OutroSequence.OutroSequenceState.done);
          break;
        case OutroSequence.OutroSequenceState.done:
          GlobalManager.Instance.OpenMainMenu();
          break;
      }
    }

    private void FireNextFireworks()
    {
      this.lastFireworksTime = Time.time;
      this.fireworks[this.fireworksIndex].camShakeAmount = new Vector3(0.5f, 0.5f, 0.5f);
      this.fireworks[this.fireworksIndex].camShakeDuration = 0.15f;
      this.fireworks[this.fireworksIndex].FireRandom();
      this.fireworksIndex = (this.fireworksIndex + 1) % this.fireworks.Length;
    }

    public void FadeToMovieCallback()
    {
      this.ChangeState(OutroSequence.OutroSequenceState.playingMovie);
    }

    public enum OutroSequenceState
    {
      notStarted,
      bossDead,
      youMadeItConfetti,
      endScreenOpen,
      fadeToMovie,
      playingMovie,
      inCredits,
      done,
    }
  }
}
