// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.BoxCollider
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class BoxCollider : Collider
  {
    public Vector3 center { get; set; }

    public Vector3 size { get; set; }

    protected override void DoAddCollider(Body body, float mass)
    {
      Vector2 vector2 = (Vector2) (this.size * this.gameObject.transform.lossyScale);
      this.connectedBody = body;
      Physics.AddBox(body, this.isTrigger, vector2.x, vector2.y, (Vector2) (this.center * this.gameObject.transform.lossyScale), mass);
    }
  }
}
