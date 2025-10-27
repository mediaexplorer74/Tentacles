// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SuperWormSegment
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SuperWormSegment : MonoBehaviour
  {
    public AudioWrapper playOnRip;
    public PoolableObject createOnRip;
    public EnergyCell energyCell;
    public Microsoft.Xna.Framework.Vector3 axis;
    private bool deathSequenceStarted;
    private bool deathSequenceRunning;
    private float deathSequenceStartTime;
    private float deathSequenceDuration = 0.4f;
    private Microsoft.Xna.Framework.Vector3 startRotation;

    public override void Start() => this.startRotation = this.transform.rotation.eulerAngles;

    public override void Update()
    {
      if (!this.deathSequenceStarted && this.energyCell.isDead)
      {
        if (this.createOnRip != null)
          ObjectPool.Instance.Draw(this.createOnRip, this.transform.position, this.transform.rotation);
        this.playOnRip.PlaySound();
        this.deathSequenceStarted = true;
        this.deathSequenceStartTime = LevelHandler.Instance.globalLevelTime;
        this.deathSequenceRunning = true;
      }
      if (this.deathSequenceRunning && (double) LevelHandler.Instance.globalLevelTime - (double) this.deathSequenceStartTime > (double) this.deathSequenceDuration)
      {
        this.transform.rotation.eulerAngles = this.GetRotationFromSequenceFraction(1f);
        this.deathSequenceRunning = false;
      }
      if (!this.deathSequenceRunning)
        return;
      this.transform.rotation.eulerAngles = this.GetRotationFromSequenceFraction((LevelHandler.Instance.globalLevelTime - this.deathSequenceStartTime) / this.deathSequenceDuration);
    }

    private Microsoft.Xna.Framework.Vector3 GetRotationFromSequenceFraction(float _sequenceFraction)
    {
      return this.startRotation + 180f * _sequenceFraction * Microsoft.Xna.Framework.Vector3.Normalize(this.axis);
    }
  }
}
