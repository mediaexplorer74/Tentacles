// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelStateChangeCondition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelStateChangeCondition : BaseCondition
  {
    public LevelHandler.LevelState[] changeFromStates;
    public LevelHandler.LevelState[] changeToStates;
    private LevelHandler.LevelState previousState;

    public override void Update()
    {
      if (LevelHandler.Instance.state == this.previousState)
        return;
      foreach (LevelHandler.LevelState changeToState in this.changeToStates)
      {
        if (LevelHandler.Instance.state == changeToState)
          this.SetConditionStatus(true);
      }
      foreach (LevelHandler.LevelState changeFromState in this.changeFromStates)
      {
        if (this.previousState == changeFromState)
          this.SetConditionStatus(true);
      }
      this.previousState = LevelHandler.Instance.state;
    }
  }
}
