// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.SphereCollider
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using System;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class SphereCollider : Collider
  {
    public Vector3 center { get; set; }

    public float radius { get; set; }

    protected override void DoAddCollider(Body body, float mass)
    {
      float radius = this.radius * Math.Max(this.transform.lossyScale.x, Math.Max(this.transform.lossyScale.y, this.transform.lossyScale.z));
      this.connectedBody = body;
      Physics.AddCircle(body, this.isTrigger, radius, (Vector2) (this.center * this.transform.lossyScale), mass);
    }
  }
}
