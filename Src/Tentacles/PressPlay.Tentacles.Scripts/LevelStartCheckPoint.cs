// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelStartCheckPoint
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelStartCheckPoint : CheckPoint
  {
    public Transform lemmyStartPosition;
    public Ease lemmyMoveEase = Ease.EaseCircOut;

    public Vector3 GetStartTweenPosition(float _moveFraction)
    {
      return Vector3.Lerp(this.lemmyStartPosition.position, this.transform.position, Equations.ChangeFloat(_moveFraction, 0.0f, 1f, 1f, this.lemmyMoveEase));
    }

    public override Vector3 GetSpawnPosition() => base.GetSpawnPosition();
  }
}
