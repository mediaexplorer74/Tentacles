// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperWormEntryExit
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperWormEntryExit : MonoBehaviour
  {
    public WaypointWrapper entryPath;
    public WaypointWrapper exitPath;
    public PPAnimationHandler meshAnim;
    private SuperWormEntryExit.State state;

    private void ChangeState(SuperWormEntryExit.State _state)
    {
      if (this.state == _state)
        return;
      this.state = _state;
      switch (_state)
      {
        case SuperWormEntryExit.State.closing:
          this.meshAnim.Play("Close", new PPAnimationHandler.PPAnimationCallback(this.CloseCallback));
          break;
        case SuperWormEntryExit.State.opening:
          this.meshAnim.Play("Open", new PPAnimationHandler.PPAnimationCallback(this.OpenCallback));
          break;
        case SuperWormEntryExit.State.closed:
          this.meshAnim.Play("IdleClosed");
          break;
        case SuperWormEntryExit.State.open:
          this.meshAnim.Play("IdleOpen");
          break;
      }
    }

    public void Open()
    {
      if (this.state == SuperWormEntryExit.State.open || this.state == SuperWormEntryExit.State.opening)
        return;
      this.ChangeState(SuperWormEntryExit.State.opening);
    }

    public void Close()
    {
      if (this.state == SuperWormEntryExit.State.closed || this.state == SuperWormEntryExit.State.closing)
        return;
      this.ChangeState(SuperWormEntryExit.State.closing);
    }

    private void OpenCallback() => this.ChangeState(SuperWormEntryExit.State.open);

    private void CloseCallback() => this.ChangeState(SuperWormEntryExit.State.closed);

    public enum State
    {
      closing,
      opening,
      closed,
      open,
    }
  }
}
