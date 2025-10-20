// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CamState
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CamState
  {
    public PathFollowCamStats stats;
    public PathFollowCam.State mode;
    public float stateDuration;
    public Vector3 objectPositionInViewport;

    public PathFollowCam.State state => this.mode;
  }
}
