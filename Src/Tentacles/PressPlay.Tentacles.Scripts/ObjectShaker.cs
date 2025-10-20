// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ObjectShaker
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  internal class ObjectShaker : MonoBehaviour
  {
    private Vector3 rotationAmount;
    private float rotationStartTime;
    private float rotationDuration;
    private Quaternion originalRotation;
    private Vector3 originalEulerRotation;
    private bool doShakeRotation;
    private Transform rotationTarget;
    private Vector3 positionAmount;
    private float positionStartTime;
    private float positionDuration;
    private Vector3 originalPosition;
    private bool doShakePosition;

    public void ShakeLocalRotation(Vector3 amount, float duration, Transform target)
    {
      this.rotationStartTime = Time.time;
      this.rotationDuration = duration;
      this.originalRotation = this.transform.localRotation;
      this.originalEulerRotation = this.transform.localRotation.eulerAngles;
      this.rotationAmount = amount;
      this.rotationAmount.x = 0.0f;
      this.rotationAmount.z = 0.0f;
      this.doShakeRotation = true;
      this.rotationTarget = target;
    }

    private void HandleShakeLocalRotation()
    {
      if ((double) Time.timeScale == 0.0)
        return;
      if ((double) Time.time > (double) this.rotationStartTime + (double) this.rotationDuration)
      {
        this.doShakeRotation = false;
        this.rotationTarget.localRotation = this.originalRotation;
      }
      else
        this.rotationTarget.localRotation = Quaternion.Euler(Random.insideUnitSphere * this.rotationAmount * (float) (1.0 - ((double) Time.time - (double) this.rotationStartTime) / (double) this.rotationDuration) + this.originalEulerRotation);
    }

    public void ShakePosition(Vector3 amount, float duration, Transform target)
    {
    }

    private void HandleShakePosition()
    {
    }

    public override void Update()
    {
      if (!this.doShakeRotation)
        return;
      this.HandleShakeLocalRotation();
    }
  }
}
