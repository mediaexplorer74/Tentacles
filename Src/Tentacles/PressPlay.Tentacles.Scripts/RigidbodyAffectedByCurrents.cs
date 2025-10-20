// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.RigidbodyAffectedByCurrents
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class RigidbodyAffectedByCurrents : ResetOnLemmyDeath
  {
    public float multiplyCurrentPower = 1f;
    private List<Current> activeCurrents = new List<Current>();
    private Vector3 _force;
    private Vector3 tmpVelocityDifference;
    private Current tmpCurrent;

    public Vector3 force => this._force;

    public override void OnTriggerEnter(Collider other)
    {
      if (!(other.gameObject.tag == "Current"))
        return;
      Current component = (Current) other.GetComponent(typeof (Current));
      if (component == null || this.activeCurrents.Contains(component))
        return;
      this.activeCurrents.Add(component);
    }

    public override void OnTriggerExit(Collider other)
    {
      if (!(other.gameObject.tag == "Current"))
        return;
      this.tmpCurrent = (Current) other.GetComponent(typeof (Current));
      if (this.tmpCurrent == null || !this.activeCurrents.Contains(this.tmpCurrent))
        return;
      this.activeCurrents.Remove(this.tmpCurrent);
    }

    public void UpdateForces()
    {
      if (this.rigidbody == null)
        return;
      Vector3 force = this._force;
      this._force = new Vector3();
      for (int index = 0; index < this.activeCurrents.Count; ++index)
      {
        this.tmpCurrent = this.activeCurrents[index];
        this._force += this.tmpCurrent.GetForce(this.rigidbody) * this.multiplyCurrentPower;
      }
      this._force = (force + this._force) * 0.5f;
      this.rigidbody.AddForce(this._force);
    }

    internal override void DoReset() => this.activeCurrents.Clear();
  }
}
