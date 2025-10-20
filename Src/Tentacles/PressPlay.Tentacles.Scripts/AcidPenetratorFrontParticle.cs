// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AcidPenetratorFrontParticle
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AcidPenetratorFrontParticle : PoolableObject
  {
    public UVSpriteSheetAnimator uvAnim;
    public MeshFilter meshFilter;
    public float growthSizeMin = 3f;
    public float growthSizeMax = 3f;
    private float growthSize = 3f;
    public float growthTimeMin = 0.7f;
    public float growthTimeMax = 1.5f;
    private float growTime = 1f;
    public float lingerTime = 1f;
    public Ease growthEase;
    private float activationTime;

    public override void Create()
    {
      base.Create();
      if (this.uvAnim == null)
        return;
      this.uvAnim.Initialize();
    }

    public override void DeActivate() => base.DeActivate();

    public override void Activate()
    {
      base.Activate();
      this.growTime = Random.Range(this.growthTimeMin, this.growthTimeMax);
      this.growthSize = Random.Range(this.growthSizeMin, this.growthSizeMax);
      this.activationTime = Time.time;
      this.ScaleVertices(0.0f);
    }

    public override void Return() => base.Return();

    public override void Update()
    {
      this.ScaleVertices(Equations.ChangeFloat(Time.time - this.activationTime, 0.0f, this.growthSize, this.growTime, this.growthEase));
      if ((double) Time.time <= (double) this.growTime + (double) this.lingerTime + (double) this.activationTime)
        return;
      this.Return();
    }

    private void ScaleVertices(float _scale) => this.transform.localScale = Vector3.one * _scale;
  }
}
