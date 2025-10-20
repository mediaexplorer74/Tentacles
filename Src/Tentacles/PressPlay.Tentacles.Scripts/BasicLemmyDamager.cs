// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BasicLemmyDamager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BasicLemmyDamager : MonoBehaviour
  {
    public DamageFeedback feedback;
    private bool _lemmyTouching;
    public DamageLemmyStats damageStats;
    public bool doDamage = true;

    public bool isLemmyTouching => this._lemmyTouching;

    public void OverrideDamageStats(DamageLemmyStats _stats) => this.damageStats = _stats;

    protected void HitLemmy(Vector3 _hitDir, Vector3 _position)
    {
      if (!this.doDamage)
        return;
      LevelHandler.Instance.lemmy.Damage(this.damageStats.damage, _hitDir);
      LevelHandler.Instance.lemmy.Push(this.damageStats.push * _hitDir);
      if (this.damageStats.breakLemmysConnections)
        LevelHandler.Instance.lemmy.BreakConnections();
      this.DoOnHitLemmy(_hitDir, _position);
      if (this.feedback == null)
        return;
      this.feedback.DoOnHitLemmy(_hitDir, _position);
    }

    internal virtual void DoOnHitLemmy(Vector3 _hitDir, Vector3 _position)
    {
    }

    protected void StayOnLemmy(Vector3 _hitDir, Vector3 _position)
    {
      if (!this.doDamage)
        return;
      if (this.damageStats.breakLemmysConnections)
        LevelHandler.Instance.lemmy.BreakConnections();
      LevelHandler.Instance.lemmy.Damage(this.damageStats.onStayDamage * Time.deltaTime, _hitDir);
      LevelHandler.Instance.lemmy.Push(this.damageStats.onStayPush * _hitDir * Time.deltaTime);
    }

    public override void OnCollisionEnter(Collision collision)
    {
      if (!(collision.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this._lemmyTouching = true;
      this.HitLemmy(-collision.contacts[0].normal, collision.contacts[0].point);
    }

    public override void OnCollisionStay(Collision collision)
    {
      if (!(collision.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this.StayOnLemmy(-collision.contacts[0].normal, collision.contacts[0].point);
    }

    public override void OnCollisionExit(Collision collision)
    {
      if (!(collision.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this._lemmyTouching = false;
    }

    public override void OnTriggerStay(Collider collider)
    {
      if (!(collider.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this.StayOnLemmy((collider.transform.position - this.transform.position).normalized, this.transform.position);
    }

    public override void OnTriggerEnter(Collider collider)
    {
      if (!(collider.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this._lemmyTouching = true;
      this.HitLemmy((collider.transform.position - this.transform.position).normalized, this.transform.position);
    }

    public override void OnTriggerExit(Collider collider)
    {
      if (!(collider.gameObject.tag == GlobalSettings.Instance.lemmyTag))
        return;
      this._lemmyTouching = false;
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }
  }
}
