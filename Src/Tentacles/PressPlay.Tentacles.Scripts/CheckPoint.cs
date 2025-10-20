// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CheckPoint
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CheckPoint : TriggeredByLemmy
  {
    public PathFollowCamNode connectionedNode;
    public bool start;
    private bool _isCheckPointActive;

    public bool isCheckPointActive => this._isCheckPointActive;

    protected override void DoOnTrigger() => this.ActivateCheckPoint();

    public void ActivateCheckPoint()
    {
      if (this.isCheckPointActive)
        return;
      this._isCheckPointActive = true;
      LevelHandler.Instance.ActivateCheckpoint(this);
      this.DoOnActivate();
    }

    public virtual Vector3 GetSpawnPosition() => this.transform.position;

    public virtual void DoOnActivate()
    {
    }

    public virtual void DoOnSpawnLemmy()
    {
    }
  }
}
