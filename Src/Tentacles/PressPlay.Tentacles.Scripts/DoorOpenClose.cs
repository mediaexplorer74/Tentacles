// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DoorOpenClose
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DoorOpenClose : MonoBehaviour
  {
    private Collider[] debugCollidersToTurnOff;
    public BasicLemmyDamager[] damagersToTurnOff;
    public AudioWrapper sndOpen;
    public AudioWrapper sndClose;
    public PPAnimationHandler anim;
    public bool useSequence = true;
    public DoorSequence sequence;
    private float totalSequenceTime;
    public float sequenceOffset;
    private float stateChangeTime;
    private DoorOpenClose.DoorState state;
    public DoorOpenClose.DoorState startState = DoorOpenClose.DoorState.closed;

    public override void Start()
    {
      this.debugCollidersToTurnOff = this.GetComponentsInChildren<Collider>();
      if (this.anim != null)
      {
        if (!this.anim.isInitialized)
          this.anim.Initialize();
        if ((double) this.sequence.openTime == -1.0)
          this.sequence.openTime = this.anim.GetAnimation("opening").length;
        if ((double) this.sequence.closeTime == -1.0)
          this.sequence.closeTime = this.anim.GetAnimation("closing").length;
      }
      else
      {
        this.sequence.openTime = 0.0f;
        this.sequence.closeTime = 0.0f;
      }
      this.sequence.Initialize(0.0f);
      this.totalSequenceTime = this.sequence.totalSequenceTime;
      if (!this.useSequence)
        this.ChangeState(this.startState);
      this.Update();
    }

    public override void Update()
    {
      if (this.useSequence)
      {
        DoorOpenClose.DoorState stateFromGlobalTime = this.GetStateFromGlobalTime(LevelHandler.Instance.globalLevelTime);
        if (stateFromGlobalTime != this.state)
          this.ChangeState(stateFromGlobalTime);
      }
      if (this.anim == null)
        return;
      switch (this.state)
      {
        case DoorOpenClose.DoorState.open:
          this.SetDamagerStatus(false);
          this.anim.Play("opening");
          this.anim.animationComponent["opening"].normalizedTime = 1f;
          break;
        case DoorOpenClose.DoorState.closing:
          this.SetDamagerStatus(true);
          this.anim.Play("closing");
          this.anim.animationComponent["closing"].normalizedTime = (Time.time - this.stateChangeTime) / this.sequence.closeTime;
          break;
        case DoorOpenClose.DoorState.closed:
          this.SetDamagerStatus(true);
          this.anim.Play("closing");
          this.anim.animationComponent["closing"].normalizedTime = 1f;
          break;
        case DoorOpenClose.DoorState.opening:
          this.SetDamagerStatus(false);
          this.anim.Play("opening");
          this.anim.animationComponent["opening"].normalizedTime = (Time.time - this.stateChangeTime) / this.sequence.openTime;
          break;
      }
      this.anim.animationComponent[this.anim.currentClipName].speed = 0.0f;
    }

    public void Open()
    {
      if (this.state == DoorOpenClose.DoorState.open)
        return;
      this.ChangeState(DoorOpenClose.DoorState.opening);
    }

    public void Close()
    {
      if (this.state == DoorOpenClose.DoorState.closed)
        return;
      this.ChangeState(DoorOpenClose.DoorState.closing);
    }

    private void ChangeState(DoorOpenClose.DoorState _state)
    {
      this.stateChangeTime = Time.time;
      this.state = _state;
      switch (this.state)
      {
        case DoorOpenClose.DoorState.open:
          this.DoOnChangeToOpen();
          break;
        case DoorOpenClose.DoorState.closing:
          this.sndClose.PlaySound();
          break;
        case DoorOpenClose.DoorState.closed:
          this.DoOnChangeToClosed();
          break;
        case DoorOpenClose.DoorState.opening:
          this.sndOpen.PlaySound();
          break;
      }
    }

    protected virtual void DoOnChangeToOpen()
    {
    }

    protected virtual void DoOnChangeToClosed()
    {
    }

    private DoorOpenClose.DoorState GetStateFromSequenceTime(float _sequenceTime)
    {
      return this.sequence.GetStateFromSequenceTime(_sequenceTime);
    }

    private DoorOpenClose.DoorState GetStateFromGlobalTime(float _globalTime)
    {
      return this.GetStateFromSequenceTime((_globalTime - this.sequenceOffset) % this.totalSequenceTime);
    }

    private void SetDamagerStatus(bool _status)
    {
      for (int index = 0; index < this.damagersToTurnOff.Length; ++index)
        this.damagersToTurnOff[index].doDamage = _status;
    }

    public enum DoorState
    {
      open,
      closing,
      closed,
      opening,
    }
  }
}
