// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicTentacleTipCollider
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicTentacleTipCollider : MonoBehaviour
  {
    [ContentSerializerIgnore]
    public BasicTentacleTipCollider.DoOnHitDelegate doOnHitDelegate;

    protected void HitTip(TentacleTip _tip, Vector3 _hitDir)
    {
      if (this.doOnHitDelegate != null)
        this.doOnHitDelegate(_tip, _hitDir);
      this.DoOnHit(_tip, _hitDir);
    }

    protected virtual void DoOnHit(TentacleTip _tip, Vector3 _hitDir)
    {
    }

    protected virtual void DoOnStay(TentacleTip _tip, Vector3 _hitDir)
    {
    }

    internal void StayOnTip(TentacleTip _tip, Vector3 _hitDir) => this.DoOnStay(_tip, _hitDir);

    public override void OnCollisionEnter(Collision collision)
    {
      if (!(collision.gameObject.tag == GlobalSettings.Instance.tentacleTipTag))
        return;
      this.HitTip(collision.gameObject.GetComponent<TentacleTip>(), collision.contacts[0].normal);
    }

    public override void OnCollisionStay(Collision collision)
    {
      if (!(collision.gameObject.tag == GlobalSettings.Instance.tentacleTipTag))
        return;
      this.StayOnTip(collision.gameObject.GetComponent<TentacleTip>(), collision.contacts[0].normal);
    }

    public override void OnTriggerStay(Collider collider)
    {
      if (!(collider.gameObject.tag == GlobalSettings.Instance.tentacleTipTag))
        return;
      this.StayOnTip(collider.gameObject.GetComponent<TentacleTip>(), (collider.transform.position - this.transform.position).normalized);
    }

    public override void OnTriggerEnter(Collider collider)
    {
      if (!(collider.gameObject.tag == GlobalSettings.Instance.tentacleTipTag))
        return;
      this.HitTip(collider.gameObject.GetComponent<TentacleTip>(), (collider.transform.position - this.transform.position).normalized);
    }

    public delegate void DoOnHitDelegate(TentacleTip _tip, Vector3 _hitDir);
  }
}
