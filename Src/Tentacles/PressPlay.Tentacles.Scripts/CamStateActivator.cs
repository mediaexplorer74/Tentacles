// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CamStateActivator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CamStateActivator : BaseActivator
  {
    public bool dontChangeStats = true;
    public bool includeLemmyInFollowObjects = true;
    public CamState camState;
    public Transform[] followObjects;
    public Transform rotationAnchor;
    public Vector3 viewportPosition;

    protected override void DoOnActivate()
    {
      base.DoOnActivate();
      if (this.dontChangeStats)
        this.camState.stats = LevelHandler.Instance.cam.stats;
      LevelHandler.Instance.cam.SetCamState(this.camState);
      switch (this.camState.state)
      {
        case PathFollowCam.State.placeFollowObjectInViewPort:
          LevelHandler.Instance.cam.PlaceObjectInViewPort(this.viewportPosition);
          break;
        case PathFollowCam.State.placeBetweenFollowObjects:
          Transform[] _followObjects = this.followObjects;
          if (this.includeLemmyInFollowObjects)
          {
            _followObjects = new Transform[this.followObjects.Length + 1];
            for (int index = 0; index < this.followObjects.Length; ++index)
              _followObjects[index] = this.followObjects[index];
            _followObjects[_followObjects.Length - 1] = LevelHandler.Instance.lemmy.transform;
          }
          LevelHandler.Instance.cam.PlaceBetweenFollowObjects(_followObjects, this.rotationAnchor);
          break;
      }
    }
  }
}
