// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DoorSequence
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DoorSequence
  {
    private float _sequenceStartTime;
    private float _totalSequenceTime;
    public float stayOpenTime = 2f;
    public float stayClosedTime = 1f;
    public float closeTime = -1f;
    public float openTime = -1f;

    public float sequenceStartTime => this._sequenceStartTime;

    public float totalSequenceTime => this._totalSequenceTime;

    public void Initialize(float _earlierSequencesTotalTime)
    {
      this._totalSequenceTime = this.stayOpenTime + this.closeTime + this.stayClosedTime + this.openTime;
      this._sequenceStartTime = _earlierSequencesTotalTime;
    }

    public DoorOpenClose.DoorState GetStateFromSequenceTime(float _sequenceTime)
    {
      if ((double) _sequenceTime <= (double) this.stayOpenTime)
        return DoorOpenClose.DoorState.open;
      if ((double) _sequenceTime <= (double) this.stayOpenTime + (double) this.closeTime)
        return DoorOpenClose.DoorState.closing;
      if ((double) _sequenceTime <= (double) this.stayOpenTime + (double) this.closeTime + (double) this.stayClosedTime)
        return DoorOpenClose.DoorState.closed;
      return (double) _sequenceTime <= (double) this.stayOpenTime + (double) this.closeTime + (double) this.stayClosedTime + (double) this.openTime ? DoorOpenClose.DoorState.opening : DoorOpenClose.DoorState.open;
    }
  }
}
