// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicPenetratorMainBody
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicPenetratorMainBody : MonoBehaviour
  {
    public AudioWrapper sndLoop;
    protected Transform[] upperPositions;
    protected Transform[] lowerPositions;
    protected Transform[] tailPositions;

    public virtual void Initialize(
      Transform[] _upperPositions,
      Transform[] _lowerPositions,
      Transform[] _tailPositions)
    {
      this.upperPositions = _upperPositions;
      this.lowerPositions = _lowerPositions;
      this.tailPositions = _tailPositions;
    }

    public virtual void UpdateRunning()
    {
    }

    public virtual void UpdateRunFinished()
    {
    }

    public virtual void DoReset()
    {
    }
  }
}
