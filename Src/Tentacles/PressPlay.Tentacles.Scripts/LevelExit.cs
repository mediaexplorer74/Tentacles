// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelExit
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelExit : TriggeredByLemmy
  {
    public Transform lemmyOutPosition;
    public Ease lemmyMoveEase = Ease.EaseCircOut;

    protected override void DoOnTrigger() => this.ActivateExit();

    public void ActivateExit() => LevelHandler.Instance.StartLevelExit();

    public Vector3 GetMoveOutTweenPosition(float _moveFraction)
    {
      return Vector3.Lerp(this.transform.position, this.lemmyOutPosition.position, Equations.ChangeFloat(_moveFraction, 0.0f, 1f, 1f, this.lemmyMoveEase));
    }
  }
}
