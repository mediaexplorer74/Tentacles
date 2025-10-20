// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.WeaponSounds
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class WeaponSounds
  {
    public AudioClip[] sndOnFire;
    public AudioClip[] sndOnEquipped;
    public AudioClip[] sndOnTriggerStart;
    public AudioClip[] sndOnTriggerFinish;
    public AudioClip[] sndOnReload;
    public AudioClip[] sndOnCharge;
    public int currentOnChargeSnd = -1;
    public AudioClip[] sndOnChargeFull;
    public AudioClip[] sndOnChargeFail;
    public AudioClip[] sndWhileEmitting;
    public int currentWhileEmittingSnd = -1;

    public AudioClip getOnFireSnd => this.GetSound(this.sndOnFire);

    public AudioClip getOnEquippedSnd => this.GetSound(this.sndOnEquipped);

    public AudioClip getOnTriggerStartSnd => this.GetSound(this.sndOnTriggerStart);

    public AudioClip getOnTriggerFinishSnd => this.GetSound(this.sndOnTriggerFinish);

    public AudioClip getOnReloadSnd => this.GetSound(this.sndOnReload);

    public AudioClip getOnCharge => this.GetSound(this.sndOnCharge);

    public AudioClip getOnChargeFull => this.GetSound(this.sndOnChargeFull);

    public AudioClip getOnChargeFail => this.GetSound(this.sndOnChargeFail);

    public AudioClip getWhileEmitting => this.GetSound(this.sndWhileEmitting);

    public void StopSound(int id) => AudioManager.Instance.Stop(id);

    public int PlaySound(AudioClip snd, string category, int priority)
    {
      return snd == null ? -1 : AudioManager.Instance.Play(new AudioSettings(snd), category, priority);
    }

    public int PlaySound(AudioClip snd, string category, int priority, bool looping)
    {
      return snd == null ? -1 : AudioManager.Instance.Play(new AudioSettings(snd, 1f, true), category, priority);
    }

    private AudioClip GetSound(AudioClip[] sndArray)
    {
      return sndArray.Length == 0 ? (AudioClip) null : sndArray[Random.Range(0, sndArray.Length)];
    }
  }
}
