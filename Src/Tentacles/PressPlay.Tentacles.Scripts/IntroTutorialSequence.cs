// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.IntroTutorialSequence
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class IntroTutorialSequence : ResetOnLemmyDeath
  {
    public Color textColor = Color.white;
    public BaseCondition changeToBewareOfDanger;
    public BaseCondition changeToTapToKill;
    public BaseCondition changeToDone;
    private float stateChangeTime;
    private IntroTutorialSequence.TutorialSequenceState tutorialSequenceState;

    public bool isBeforeTutorial
    {
      get
      {
        return this.tutorialSequenceState == IntroTutorialSequence.TutorialSequenceState.beforeTutorial;
      }
    }

    public override void Start()
    {
      this.ChangeState(IntroTutorialSequence.TutorialSequenceState.beforeTutorial);
    }

    public override void Update()
    {
      switch (this.tutorialSequenceState)
      {
        case IntroTutorialSequence.TutorialSequenceState.tapToMove:
          if (!this.changeToBewareOfDanger.GetConditionStatus())
            break;
          this.ChangeState(IntroTutorialSequence.TutorialSequenceState.bewareOfDanger);
          break;
        case IntroTutorialSequence.TutorialSequenceState.bewareOfDanger:
          if (!this.changeToTapToKill.GetConditionStatus())
            break;
          this.ChangeState(IntroTutorialSequence.TutorialSequenceState.tapToKill);
          break;
        case IntroTutorialSequence.TutorialSequenceState.tapToKill:
          if (!this.changeToDone.GetConditionStatus())
            break;
          this.ChangeState(IntroTutorialSequence.TutorialSequenceState.done);
          break;
      }
    }

    public void StartTutorial()
    {
      this.ChangeState(IntroTutorialSequence.TutorialSequenceState.tapToMove);
    }

    private void ChangeState(
      IntroTutorialSequence.TutorialSequenceState _tutorialSequenceState)
    {
      this.stateChangeTime = Time.time;
      this.tutorialSequenceState = _tutorialSequenceState;
      switch (this.tutorialSequenceState)
      {
        case IntroTutorialSequence.TutorialSequenceState.tapToMove:
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__("tut_move"), this.textColor, false);
          break;
        case IntroTutorialSequence.TutorialSequenceState.bewareOfDanger:
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__("tut_danger"), this.textColor, false);
          break;
        case IntroTutorialSequence.TutorialSequenceState.tapToKill:
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__("tut_kill"), this.textColor, false);
          break;
      }
    }

    internal override void DoReset()
    {
      base.DoReset();
      this.StartTutorial();
    }

    public enum TutorialSequenceState
    {
      beforeTutorial,
      tapToMove,
      bewareOfDanger,
      tapToKill,
      done,
    }
  }
}
