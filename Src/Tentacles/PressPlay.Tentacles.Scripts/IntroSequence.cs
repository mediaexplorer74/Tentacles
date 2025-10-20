// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.IntroSequence
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class IntroSequence : MonoBehaviour, ILevelHandlerCutscene
  {
    public IntroTutorialSequence tutorialSequence;
    public BaseCondition continueFromPetriDishCondition;
    public MoviePlayer moviePlayer;
    public MoviePlayer.Movies firstMovie;
    public MoviePlayer.Movies secondMovie;
    private LevelHandler.CutsceneState cutsceneState;
    private float stateChangeTime;
    private IntroSequence.IntroSequenceState introSequenceState;

    public override void Start() => this.ChangeState(IntroSequence.IntroSequenceState.beforeIntro);

    public void StartCutscene()
    {
      this.cutsceneState = LevelHandler.CutsceneState.running;
      this.ChangeState(IntroSequence.IntroSequenceState.showingFirstMovie);
    }

    public LevelHandler.CutsceneState GetCutsceneState() => this.cutsceneState;

    public override void LateUpdate()
    {
      switch (this.introSequenceState)
      {
        case IntroSequence.IntroSequenceState.showingFirstMovie:
          if (!this.moviePlayer.isDonePlaying)
            break;
          this.ChangeState(IntroSequence.IntroSequenceState.playingInPetriDish);
          break;
        case IntroSequence.IntroSequenceState.playingInPetriDish:
          if (this.tutorialSequence.isBeforeTutorial && (double) Time.time > (double) this.stateChangeTime + 2.0)
            this.tutorialSequence.StartTutorial();
          if (!this.continueFromPetriDishCondition.GetConditionStatus())
            break;
          this.ChangeState(IntroSequence.IntroSequenceState.fadeToShowSecondMovie);
          break;
        case IntroSequence.IntroSequenceState.showingSecondMovie:
          if (!this.moviePlayer.isDonePlaying)
            break;
          this.ChangeState(IntroSequence.IntroSequenceState.exit);
          break;
      }
    }

    private void ChangeState(
      IntroSequence.IntroSequenceState _introSequenceState)
    {
      this.stateChangeTime = Time.time;
      this.introSequenceState = _introSequenceState;
      switch (this.introSequenceState)
      {
        case IntroSequence.IntroSequenceState.showingFirstMovie:
          this.moviePlayer.TestZuneBeforePlayingMovie(this.firstMovie);
          break;
        case IntroSequence.IntroSequenceState.playingInPetriDish:
          this.cutsceneState = LevelHandler.CutsceneState.done;
          break;
        case IntroSequence.IntroSequenceState.fadeToShowSecondMovie:
          GlobalManager.Instance.fullscreenImageHandler.FadeToBlack(0.8f, this.gameObject, "FadeToSecondMovieCallback");
          LevelHandler.Instance.musicController.FadeTo(0.0f, 0.7f);
          break;
        case IntroSequence.IntroSequenceState.showingSecondMovie:
          this.moviePlayer.TestZuneBeforePlayingMovie(this.secondMovie);
          break;
        case IntroSequence.IntroSequenceState.exit:
          LevelHandler.Instance.levelSession.SaveScore();
          GlobalManager.Instance.OpenNextLevel();
          break;
      }
    }

    public void FadeToSecondMovieCallback()
    {
      this.ChangeState(IntroSequence.IntroSequenceState.showingSecondMovie);
    }

    public enum IntroSequenceState
    {
      beforeIntro,
      showingFirstMovie,
      playingInPetriDish,
      fadeToShowSecondMovie,
      showingSecondMovie,
      exit,
    }
  }
}
