// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.ParticleEmitter
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Interfaces;
using System;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class ParticleEmitter : Component, IFixedUpdateable
  {
    public bool emit;
    public float minSize;
    public float maxSize;
    public float minEnergy;
    public float maxEnergy;
    public float minEmission;
    public float maxEmission;
    public float emitterVelocityScale;
    public PressPlay.FFWD.Vector3 worldVelocity;
    public PressPlay.FFWD.Vector3 localVelocity;
    public PressPlay.FFWD.Vector3 rndVelocity;
    public bool useWorldSpace;
    public bool enabled;
    [ContentSerializerIgnore]
    public float minRotationSpeed;
    [ContentSerializerIgnore]
    public float maxRotationSpeed;
    [ContentSerializerIgnore]
    public bool randomRotation;
    [ContentSerializerIgnore]
    public int textureTileCountX = 1;
    [ContentSerializerIgnore]
    public int textureTileCountY = 1;
    [ContentSerializer(Optional = true)]
    internal PressPlay.FFWD.Vector3 ellipsoid;
    [ContentSerializer(Optional = true)]
    internal bool oneShot;
    [ContentSerializer(Optional = true)]
    internal PressPlay.FFWD.Vector3 tangentVelocity;
    [ContentSerializer(Optional = true)]
    internal float minEmitterRange;
    private Particle[] _particles;
    private float timeToNextEmit;
    internal bool yLimit;
    internal float fadeBelow;

    [ContentSerializerIgnore]
    public int particleCount { get; private set; }

    [ContentSerializerIgnore]
    public Particle[] particles
    {
      get => this._particles;
      set
      {
        this._particles = value;
        this.particleCount = 0;
        for (int index = 0; index < this._particles.Length; ++index)
        {
          if ((double) this._particles[index].Energy > 0.0)
            ++this.particleCount;
        }
      }
    }

    public override void Awake()
    {
      this.particles = new Particle[this.particlesToAllocate()];
      this.particleCount = 0;
      if ((double) this.minEmission <= 0.0)
        this.minEmission = 1f;
      if ((double) this.maxEmission <= (double) this.minEmission)
        this.maxEmission = this.minEmission;
      if (this.oneShot)
        return;
      this.timeToNextEmit = this.GetNewEmissionTime();
    }

    internal int particlesToAllocate()
    {
      return this.oneShot ? Mathf.FloorToInt(this.maxEmission) : Mathf.CeilToInt(this.maxEmission) * (int) Math.Max(1f, this.maxEnergy * 2f);
    }

    public void FixedUpdate()
    {
      if (!this.enabled)
        return;
      int num = 0;
      if (this.emit)
      {
        if (this.oneShot)
        {
          num = Mathf.FloorToInt(PressPlay.FFWD.Random.Range(this.minEmission, this.maxEmission));
          this.emit = false;
        }
        else
        {
          for (; (double) this.timeToNextEmit < 0.0; this.timeToNextEmit += this.GetNewEmissionTime())
            ++num;
          this.timeToNextEmit -= Time.deltaTime;
        }
      }
      if (num == 0 && this.particleCount == 0)
        return;
      int particleCount = this.particleCount;
      for (int index = 0; index < this.particles.Length; ++index)
      {
        if ((double) this.particles[index].Energy > 0.0)
        {
          if ((double) (this.particles[index].Energy -= Time.deltaTime) <= 0.0 || (double) this.particles[index].Size < 0.0)
          {
            this.particles[index].Energy = 0.0f;
            --this.particleCount;
          }
          --particleCount;
        }
        else if (num > 0)
        {
          --num;
          ++this.particleCount;
          this.EmitNewParticle(ref this.particles[index]);
        }
        if (particleCount == 0 && num == 0)
          break;
      }
    }

    private void EmitNewParticle(ref Particle particle)
    {
      particle.Energy = particle.StartingEnergy = PressPlay.FFWD.Random.Range(this.minEnergy, this.maxEnergy);
      PressPlay.FFWD.Vector3 insideUnitSphere = PressPlay.FFWD.Random.insideUnitSphere;
      particle.Position = (PressPlay.FFWD.Vector3) (Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) this.ellipsoid, (Microsoft.Xna.Framework.Quaternion) this.transform.rotation) * insideUnitSphere.x);
      PressPlay.FFWD.Vector3 zero = PressPlay.FFWD.Vector3.zero;
      if (this.useWorldSpace && this.gameObject.rigidbody != null && (double) this.emitterVelocityScale > 0.0)
        zero += this.gameObject.rigidbody.velocity * this.emitterVelocityScale;
      if ((double) this.rndVelocity.x != 0.0 || (double) this.rndVelocity.y != 0.0 || (double) this.rndVelocity.z != 0.0)
        zero += PressPlay.FFWD.Random.onUnitSphere * this.rndVelocity;
      if ((double) this.tangentVelocity.x != 0.0 || (double) this.tangentVelocity.y != 0.0 || (double) this.tangentVelocity.z != 0.0)
        zero += PressPlay.FFWD.Random.onUnitSphere * this.tangentVelocity;
      if ((double) this.localVelocity.x != 0.0 || (double) this.localVelocity.y != 0.0 || (double) this.localVelocity.z != 0.0)
        zero += (PressPlay.FFWD.Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) this.localVelocity, (Microsoft.Xna.Framework.Quaternion) this.transform.rotation);
      particle.Velocity = zero;
      if (this.useWorldSpace)
        particle.Position += this.gameObject.transform.position;
      particle.Size = PressPlay.FFWD.Random.Range(this.minSize, this.maxSize);
      particle.Color = (Microsoft.Xna.Framework.Color) this.renderer.material.color;
      particle.RotationSpeed = PressPlay.FFWD.Random.Range(this.minRotationSpeed, this.maxRotationSpeed);
      if (this.randomRotation)
        particle.Rotation = 6.28318548f * PressPlay.FFWD.Random.value;
      int num1 = PressPlay.FFWD.Random.Range(0, this.textureTileCountX);
      int num2 = PressPlay.FFWD.Random.Range(0, this.textureTileCountY);
      particle.TextureScale = new PressPlay.FFWD.Vector2(1f / (float) this.textureTileCountX, 1f / (float) this.textureTileCountY);
      particle.TextureOffset = new PressPlay.FFWD.Vector2((float) num1 * particle.TextureScale.x, (float) num2 * particle.TextureScale.y);
    }

    private float GetNewEmissionTime()
    {
      return PressPlay.FFWD.Random.Range(1f / this.minEmission, 1f / this.maxEmission);
    }

    public void ClearParticles()
    {
      if (this.particles == null)
        return;
      for (int index = 0; index < this.particles.Length; ++index)
        this.particles[index].Energy = 0.0f;
      this.particleCount = 0;
    }

    public void SetEllipsoid(PressPlay.FFWD.Vector3 _ellipsoid) => this.ellipsoid = _ellipsoid;
  }
}
