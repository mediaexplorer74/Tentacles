// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TimedRotation
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TimedRotation : MonoBehaviour
  {
    public float speed;
    public float sequenceLengthOverride = -1f;
    private float sequenceLength;
    public float sequenceOffset;
    private Microsoft.Xna.Framework.Vector3 startRotation;
    public Transform objectToRotate;
    private Microsoft.Xna.Framework.Vector3 tmpVector = Microsoft.Xna.Framework.Vector3.Zero;
    public bool doRotation = true;
    public TimedRotation.Direction direction;

    public override void Start()
    {
      if (this.objectToRotate == null)
        this.objectToRotate = this.transform;
      this.startRotation = this.objectToRotate.rotation.eulerAngles;
      if ((double) this.sequenceLengthOverride == -1.0)
        this.sequenceLength = 360f / this.speed;
      else
        this.sequenceLength = this.sequenceLengthOverride;
    }

    public override void FixedUpdate()
    {
      if (!this.doRotation)
        return;
      this.objectToRotate.rotation.eulerAngles = this.GetRotationFromGlobalTime(LevelHandler.Instance.globalLevelTime);
    }

    private Microsoft.Xna.Framework.Vector3 GetRotationFromGlobalTime(float _globalTime)
    {
      return this.GetRotationFromSequenceTime((_globalTime - this.sequenceOffset) % this.sequenceLength);
    }

    private Microsoft.Xna.Framework.Vector3 GetRotationFromSequenceTime(float _sequenceTime)
    {
      this.tmpVector.Y = (float) (360.0 * ((double) _sequenceTime / (double) this.sequenceLength));
      return this.direction == TimedRotation.Direction.clockwise ? this.startRotation + this.tmpVector : this.startRotation - this.tmpVector;
    }

    public enum Direction
    {
      clockwise,
      counterClockwise,
    }
  }
}
