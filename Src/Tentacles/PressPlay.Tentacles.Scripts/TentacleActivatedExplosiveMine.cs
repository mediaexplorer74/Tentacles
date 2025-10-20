// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TentacleActivatedExplosiveMine
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using FarseerPhysics.Dynamics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TentacleActivatedExplosiveMine : Mine
  {
    public OnClawBehaviourConnect[] tentacleTipColliders;
    public PoolableParticle particlesOnExplosion;
    public float explosionRadius = 6f;
    public float mineDamage = 20f;
    public float minePush = 20f;
    private bool tentacleWasConnected;

    public override void Start()
    {
      Collider[] componentsInChildren = this.GetComponentsInChildren<Collider>();
      for (int index = 0; index < componentsInChildren.Length; ++index)
      {
        componentsInChildren[index].connectedBody.BodyType = BodyType.Kinematic;
        componentsInChildren[index].allowTurnOff = true;
      }
      foreach (OnClawBehaviourConnect tentacleTipCollider in this.tentacleTipColliders)
        tentacleTipCollider.doOnConnectDelegate = new OnClawBehaviourConnect.DoOnConnectDelegate(this.OnTentacleConnect);
      ObjectPool.Instance.Grow((PoolableObject) this.particlesOnExplosion, 1, 5);
    }

    public void OnTentacleConnect(ClawBehaviour _clawBehaviour, Vector3 _hitDir)
    {
      Debug.Log((object) "TentacleActivatedExplosiveMine.OnTentacleConnect");
      this.tentacleWasConnected = true;
    }

    protected override void DoOnChangeMineToExploding()
    {
      ObjectPool.Instance.Draw((PoolableObject) this.particlesOnExplosion, this.transform.position, this.transform.rotation);
      this.ExplosiveDamageLemmy(this.mineDamage, this.minePush, this.transform.position, this.explosionRadius, true);
      this.currentCharge = 0.0f;
      this.ChangeMineState(Mine.MineState.afterExplosion);
    }

    protected override bool MineActivationCheck() => this.tentacleWasConnected;

    protected override void DoOnChangeMineToIdle()
    {
      base.DoOnChangeMineToIdle();
      this.tentacleWasConnected = false;
    }

    internal override void DoReset()
    {
      base.DoReset();
      this.tentacleWasConnected = false;
    }
  }
}
