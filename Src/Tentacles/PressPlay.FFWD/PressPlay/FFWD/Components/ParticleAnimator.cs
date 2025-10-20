// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.ParticleAnimator
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Interfaces;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class ParticleAnimator : Component, IFixedUpdateable
  {
    public bool doesAnimateColor;
    public PressPlay.FFWD.Vector3 worldRotationAxis;
    public PressPlay.FFWD.Vector3 localRotationAxis;
    public float sizeGrow;
    public PressPlay.FFWD.Vector3 rndForce;
    public PressPlay.FFWD.Vector3 force;
    public float damping;
    public bool autodestruct;
    [ContentSerializer(ElementName = "colorAnimation")]
    private Microsoft.Xna.Framework.Color[] _colorAnimation;
    private ParticleEmitter emitter;
    private bool hasHadParticles;

    [ContentSerializerIgnore]
    public PressPlay.FFWD.Color[] colorAnimation
    {
      get
      {
        if (this._colorAnimation == null)
          return (PressPlay.FFWD.Color[]) null;
        PressPlay.FFWD.Color[] colorAnimation = new PressPlay.FFWD.Color[this._colorAnimation.Length];
        for (int index = 0; index < this._colorAnimation.Length; ++index)
          colorAnimation[index] = (PressPlay.FFWD.Color) this._colorAnimation[index];
        return colorAnimation;
      }
      set
      {
        if (value == null)
        {
          this._colorAnimation = (Microsoft.Xna.Framework.Color[]) null;
        }
        else
        {
          this._colorAnimation = new Microsoft.Xna.Framework.Color[value.Length];
          for (int index = 0; index < value.Length; ++index)
            this._colorAnimation[index] = (Microsoft.Xna.Framework.Color) value[index];
        }
      }
    }

    public override void Awake()
    {
      base.Awake();
      this.emitter = this.gameObject.GetComponent<ParticleEmitter>();
    }

    public void FixedUpdate()
    {
      bool flag1 = (double) this.damping != 1.0;
      bool flag2 = this.force != PressPlay.FFWD.Vector3.zero || this.rndForce != PressPlay.FFWD.Vector3.zero;
      bool flag3 = (double) this.sizeGrow != 0.0;
      int num = this.emitter.tangentVelocity != PressPlay.FFWD.Vector3.zero ? 1 : 0;
      bool flag4 = this.hasHadParticles;
      int particleCount = this.emitter.particleCount;
      for (int index = 0; index < this.emitter.particles.Length; ++index)
      {
        if ((double) this.emitter.particles[index].Energy > 0.0)
        {
          this.hasHadParticles = true;
          flag4 = false;
          this.emitter.particles[index].Position += this.emitter.particles[index].Velocity * Time.deltaTime;
          if (flag1)
            this.emitter.particles[index].Velocity *= Mathf.Pow(this.damping, Time.deltaTime);
          if (flag2)
          {
            PressPlay.FFWD.Vector3 vector3 = Random.insideUnitSphere * this.rndForce / 2f;
            this.emitter.particles[index].Velocity += (this.force + vector3) * Time.deltaTime;
          }
          this.emitter.particles[index].Rotation += Time.deltaTime * this.emitter.particles[index].RotationSpeed;
          if (flag3)
            this.emitter.particles[index].Size += this.sizeGrow * Time.deltaTime;
          this.UpdateParticleColor(ref this.emitter.particles[index]);
          if (--particleCount == 0)
            break;
        }
      }
      if (!flag4 || !this.autodestruct)
        return;
      UnityObject.Destroy((UnityObject) this.gameObject);
    }

    public void UpdateParticleColor(ref Particle particle)
    {
      if (this.doesAnimateColor)
      {
        float index = (float) (1.0 - (double) particle.Energy / (double) particle.StartingEnergy) * 4f;
        if ((double) index == 4.0)
          index = 3f;
        float amount = index - (float) (int) index;
        Microsoft.Xna.Framework.Color color = Microsoft.Xna.Framework.Color.Lerp(this._colorAnimation[(int) index], this._colorAnimation[(int) index + 1], amount);
        particle.Color = Microsoft.Xna.Framework.Color.FromNonPremultiplied((int) color.R, (int) color.G, (int) color.B, (int) color.A);
      }
      else
        particle.Color = (Microsoft.Xna.Framework.Color) this.renderer.material.color;
    }
  }
}
