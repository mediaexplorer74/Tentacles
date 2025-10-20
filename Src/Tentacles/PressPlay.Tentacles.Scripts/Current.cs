// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.Current
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class Current : MonoBehaviour
  {
    public float particleDensityMod;
    public float particleSpeedMod;
    public float particleEnergyMod;
    public float currentSpeed;
    public float currentStrength;
    public float edgeFallOff = 0.3f;
    protected BoxCollider boxCollider;
    protected Transform positionObj;
    protected float edgeFallOffNormalized;
    protected float distToCurrentEnd;
    protected float distToCurrentEndNormalized;
    protected float forceModifier;
    protected float distToCurrentCenter;
    protected float distToCurrentSide;
    protected float distToCurrentSideNormalized;
    protected Vector3 tmpVelocityDifference;
    protected Vector3 tmpForce;
    public ParticleEmitter particleEmitter;
    public ParticleAnimator particleAnimator;
    public bool attenuateOverDistance;
    public float attenuationPower = 0.2f;

    public override void Start()
    {
      base.Start();
      this.UpdateParticleAnimation();
    }

    public virtual Vector3 GetForce(Rigidbody _rigidBody)
    {
      if (this.boxCollider == null)
        this.boxCollider = this.GetComponent<BoxCollider>();
      if (this.positionObj == null)
      {
        this.positionObj = new GameObject().transform;
        this.positionObj.transform.parent = this.transform;
        this.positionObj.rotation = this.transform.rotation;
        this.positionObj.name = "Current position object";
      }
      Vector3 position = this.transform.InverseTransformDirection(_rigidBody.velocity) with
      {
        z = 0.0f,
        y = 0.0f
      };
      position = this.transform.TransformDirection(position);
      this.edgeFallOffNormalized = this.edgeFallOff / (this.transform.localScale.x * this.boxCollider.size.x);
      this.tmpVelocityDifference = -(position - this.transform.right * this.currentSpeed);
      this.positionObj.position = _rigidBody.transform.position;
      this.distToCurrentCenter = Mathf.Abs(this.positionObj.localPosition.z);
      this.distToCurrentSideNormalized = Mathf.Max(this.boxCollider.size.z * 0.5f - this.distToCurrentCenter, 0.0f);
      this.distToCurrentSide = this.distToCurrentSideNormalized * this.transform.localScale.z * this.boxCollider.size.z;
      this.distToCurrentEndNormalized = Mathf.Max(1f - this.positionObj.localPosition.x, 0.0f);
      this.distToCurrentEnd = this.distToCurrentEndNormalized * this.transform.localScale.x * this.boxCollider.size.x;
      this.forceModifier = 1f;
      if ((double) this.distToCurrentSideNormalized < (double) this.edgeFallOffNormalized)
        this.forceModifier = this.distToCurrentSideNormalized / this.edgeFallOffNormalized;
      if ((double) this.distToCurrentEnd < (double) this.edgeFallOff)
        this.forceModifier *= this.distToCurrentEndNormalized / this.edgeFallOffNormalized;
      if (this.attenuateOverDistance)
        this.forceModifier *= this.distToCurrentEndNormalized;
      this.tmpForce = this.currentStrength * Time.deltaTime * this.forceModifier * this.tmpVelocityDifference;
      return this.tmpForce;
    }

    public override void Update()
    {
    }

    public void DebugDrawCurrent()
    {
      if (this.boxCollider == null)
        return;
      this.boxCollider.bounds.DebugDraw(Color.grey);
      Vector3 vector3_1 = this.transform.right * this.boxCollider.size.x * this.transform.localScale.x;
      Vector3 vector3_2 = this.transform.forward * this.boxCollider.size.z * this.transform.localScale.z * 0.5f;
      Debug.DrawLine(this.transform.position + vector3_2, this.transform.position - vector3_2, Color.blue);
      Debug.DrawLine(this.transform.position - vector3_2, this.transform.position + vector3_1 - vector3_2, Color.blue);
      Debug.DrawLine(this.transform.position - vector3_2 + this.transform.forward * this.edgeFallOff, this.transform.position + vector3_1 - vector3_2 + this.transform.forward * this.edgeFallOff, Color.blue);
      Debug.DrawLine(this.transform.position, this.transform.position + vector3_1, Color.blue);
      Debug.DrawLine(this.transform.position + vector3_2 - this.transform.forward * this.edgeFallOff, this.transform.position + vector3_1 + vector3_2 - this.transform.forward * this.edgeFallOff, Color.blue);
      Debug.DrawLine(this.transform.position + vector3_2, this.transform.position + vector3_1 + vector3_2, Color.blue);
      Debug.DrawLine(this.transform.position + vector3_1 + vector3_2 - this.transform.right * this.edgeFallOff, this.transform.position + vector3_1 - vector3_2 - this.transform.right * this.edgeFallOff, Color.blue);
    }

    private void UpdateParticleAnimation()
    {
      if (this.particleEmitter == null || this.particleAnimator == null)
        return;
      (this.particleEmitter.renderer as ParticleRenderer).doViewportCulling = true;
      if (this.boxCollider == null)
        this.boxCollider = this.GetComponent<BoxCollider>();
      float x = this.currentSpeed * this.particleSpeedMod;
      float num1 = (float) ((double) this.boxCollider.size.x * (double) this.transform.localScale.x * (double) this.particleEnergyMod * (1.0 / (double) x));
      float num2 = this.particleDensityMod * this.boxCollider.size.z * this.transform.localScale.z;
      if (this.attenuateOverDistance)
      {
        this.particleEmitter.minEnergy = 0.0f;
        this.particleAnimator.doesAnimateColor = true;
      }
      else
        this.particleEmitter.minEnergy = num1;
      this.particleEmitter.maxEnergy = num1;
      this.particleEmitter.localVelocity = new Vector3(x, 0.0f, 0.0f);
      this.particleEmitter.maxEmission = num2;
      this.particleEmitter.minEmission = num2;
      this.particleEmitter.SetEllipsoid(new Vector3(0.0f, 0.0f, (float) ((double) this.boxCollider.size.z * (double) this.transform.lossyScale.z * 0.5)));
    }
  }
}
