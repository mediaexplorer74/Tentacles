// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PoolableParticle
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PoolableParticle : PoolableObject
  {
    public ParticleEmitter[] emitters;
    public bool autodestroy = true;
    private bool delayedReturn;
    private float startTime;
    private float maxEnergy;
    private float[] maxEnergies;

    public override void Start()
    {
      this.emitters = this.GetComponents<ParticleEmitter>();
      if (this.emitters.Length == 0)
        this.emitters = this.GetComponentsInChildren<ParticleEmitter>();
      if (this.emitters.Length == 0)
        Debug.LogError(this.name + " can't find a ParticleSystem");
      this.maxEnergies = new float[this.emitters.Length];
      int index = 0;
      foreach (ParticleEmitter emitter in this.emitters)
      {
        this.maxEnergies[index] = emitter.maxEnergy;
        this.maxEnergy = Mathf.Max(this.maxEnergy, emitter.maxEnergy + 0.02f);
        ++index;
      }
    }

    public override void Activate()
    {
      base.Activate();
      if (this.emitters.Length > 0)
      {
        foreach (ParticleEmitter emitter in this.emitters)
          emitter.emit = true;
      }
      this.startTime = Time.time;
      this.delayedReturn = false;
    }

    public override void DeActivate()
    {
      base.DeActivate();
      if (this.emitters.Length > 0 && this.emitters.Length > 0)
      {
        foreach (ParticleEmitter emitter in this.emitters)
        {
          emitter.emit = false;
          emitter.ClearParticles();
        }
      }
      this.delayedReturn = false;
    }

    public override void Return()
    {
      this.transform.parent = (Transform) null;
      foreach (ParticleEmitter emitter in this.emitters)
        emitter.emit = false;
      this.startTime = Time.time;
      this.delayedReturn = true;
    }

    public override void Update()
    {
      if (this.emitters.Length == 0 || !this.autodestroy && !this.delayedReturn)
        return;
      int index = 0;
      foreach (float maxEnergy in this.maxEnergies)
      {
        if ((double) Time.time > (double) this.startTime + (double) maxEnergy)
          this.emitters[index].emit = false;
        ++index;
      }
      if ((double) Time.time <= (double) this.startTime + (double) this.maxEnergy || !this.hasBeenActivated)
        return;
      ObjectPool.Instance.Return((PoolableObject) this);
    }
  }
}
