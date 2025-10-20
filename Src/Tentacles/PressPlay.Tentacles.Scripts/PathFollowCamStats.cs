// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowCamStats
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PathFollowCamStats
  {
    public float maxRotationSpeed;
    public float maxMovementSpeed;
    public float moveStiffness = 0.8f;
    public float turnStiffness = 0.8f;
    public float lookAheadAndHeightStiffnes = 0.8f;
    public float placeObjectInViewPortStiffness = 2.5f;
    public float defaultHeight = 12f;
    public float defaultLookAhead = 4f;
    public float changeToLastConnectionThresshold = 4f;
    public float speedCurvePow = 0.4f;
    public float speedLookAhead = 4f;
    public float speedZoomOut = 0.2f;
    public float speedMoveStiffnessMod = 2f;
    public float speedTurnStiffnessMod = 0.5f;
    public float speedModThresshold = 2f;
  }
}
