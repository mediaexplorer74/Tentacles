// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LemmySquishedTester
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LemmySquishedTester : MonoBehaviour
  {
    public Lemmy lemmy;
    public float damageOnEnter = 50f;
    public float damageOnStay = 400f;
    public bool instaKill;
    public float pushOnEnter = 150f;
    public float pushOnStay = 500f;
    private bool isInitialized;
    private List<Collider> squishingColliders = new List<Collider>();

    public void Initialize() => this.isInitialized = true;

    public override void OnCollisionEnter(Collision collision) => base.OnCollisionEnter(collision);

    public override void OnTriggerExit(Collider collider)
    {
      if (!this.isInitialized)
        return;
      if (!LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.squishLemmyLayers, collider.gameObject.layer) && collider.gameObject.active && this.gameObject.active)
      {
        Physics.IgnoreCollision(collider, this.collider);
      }
      else
      {
        if (!this.squishingColliders.Contains(collider))
          return;
        this.squishingColliders.Remove(collider);
      }
    }

    public override void OnTriggerStay(Collider collider)
    {
      if (!this.isInitialized)
        return;
      if (!LayerMaskOperations.CheckLayerMaskContainsLayer(GlobalSettings.Instance.squishLemmyLayers, collider.gameObject.layer))
      {
        if (collider == null || this.collider == null || !collider.gameObject.active || !this.gameObject.active)
          return;
        Physics.IgnoreCollision(collider, this.collider);
      }
      else if (!this.squishingColliders.Contains(collider))
      {
        this.squishingColliders.Add(collider);
      }
      else
      {
        if (this.squishingColliders.Count <= 1)
          return;
        this.Squish();
      }
    }

    public void Squish()
    {
      if (this.instaKill)
      {
        this.lemmy.Kill();
      }
      else
      {
        if ((double) this.pushOnStay > 0.0)
          this.lemmy.Push((this.lemmy.transform.position - this.collider.transform.position).normalized * this.pushOnStay * Time.deltaTime);
        this.lemmy.Damage(this.damageOnStay * Time.deltaTime, Vector3.zero);
      }
    }
  }
}
