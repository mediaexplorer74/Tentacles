// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TimedBlockElectric
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TimedBlockElectric : MonoBehaviour
  {
    public AudioWrapper sndElectricBlock;
    public bool playSound = true;
    public float fullVolumeRadius;
    public float volumeRadius;
    private AudioObject sndElectricObj;
    public float friendlyTime = 2f;
    public float dangerTime = 2f;
    private float sequenceLength;
    public float sequenceOffset;
    public LightningRenderer[] lightningRenderers;
    public Collider tentacleCollisionCollider;
    public BasicLemmyDamager[] damagers;
    public TentacleTipDamager[] tipColliders;
    private bool isDangerous;
    private bool isInitialized;

    public void Initialize()
    {
      this.isInitialized = true;
      this.sequenceLength = this.friendlyTime + this.dangerTime;
      if (this.GetIsDangerousFromGlobalTime(LevelHandler.Instance.globalLevelTime))
        this.MakeDangerous();
      else
        this.MakeFriendly();
    }

    public override void Update()
    {
      if (!LevelHandler.Instance.isInGamePlay)
        return;
      if (!this.isInitialized)
        this.Initialize();
      if (this.GetIsDangerousFromGlobalTime(LevelHandler.Instance.globalLevelTime))
      {
        if (this.isDangerous)
          return;
        this.MakeDangerous();
      }
      else
      {
        if (!this.isDangerous)
          return;
        this.MakeFriendly();
      }
    }

    private bool GetIsDangerousFromGlobalTime(float _globalTime)
    {
      return this.GetIsDangerousFromSequenceTime((_globalTime - this.sequenceOffset) % this.sequenceLength);
    }

    private bool GetIsDangerousFromSequenceTime(float _sequenceTime)
    {
      return (double) _sequenceTime >= (double) this.friendlyTime;
    }

    private void MakeDangerous()
    {
      this.isDangerous = true;
      foreach (LightningRenderer lightningRenderer in this.lightningRenderers)
        lightningRenderer.ToggleOn(true);
      foreach (BasicLemmyDamager damager in this.damagers)
        damager.doDamage = true;
      foreach (TentacleTipDamager tipCollider in this.tipColliders)
        tipCollider.doDamage = true;
      this.tentacleCollisionCollider.gameObject.layer = (int) GlobalSettings.Instance.tentacleBounceColliderLayerIndex;
      if (!this.playSound)
        return;
      this.sndElectricBlock.PlaySound();
    }

    private void MakeFriendly()
    {
      this.isDangerous = false;
      foreach (LightningRenderer lightningRenderer in this.lightningRenderers)
        lightningRenderer.ToggleOn(false);
      foreach (BasicLemmyDamager damager in this.damagers)
        damager.doDamage = false;
      foreach (TentacleTipDamager tipCollider in this.tipColliders)
        tipCollider.doDamage = false;
      this.tentacleCollisionCollider.gameObject.layer = GlobalSettings.Instance.tentacleColliderLayerIndex;
      if (!this.playSound)
        return;
      this.sndElectricBlock.Stop();
    }
  }
}
