// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.XNAPerParticleRotation
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class XNAPerParticleRotation : MonoBehaviour
  {
    public float minRotationSpeed;
    public float maxRotationSpeed;
    public bool randomRotation;

    public override void Awake()
    {
      base.Awake();
      ParticleEmitter component = this.gameObject.GetComponent<ParticleEmitter>();
      component.minRotationSpeed = this.minRotationSpeed;
      component.maxRotationSpeed = this.maxRotationSpeed;
      component.randomRotation = this.randomRotation;
    }
  }
}
