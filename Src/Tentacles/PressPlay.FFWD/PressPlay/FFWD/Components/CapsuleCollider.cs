// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.CapsuleCollider
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class CapsuleCollider : Collider
  {
    public Vector3 center;
    public float radius;
    public float height;
    public int direction;

    protected override void DoAddCollider(Body body, float mass)
    {
      this.connectedBody = body;
      Vector3 position = this.center * this.transform.lossyScale;
      if (this.direction == 0 || (double) this.height <= (double) this.radius * 2.0)
      {
        float radius = this.radius * this.transform.lossyScale.z;
        Physics.AddCircle(body, this.isTrigger, radius, (Vector2) position, mass);
      }
      else if (this.direction == 1)
      {
        Vector2 vector2_1 = new Vector2(this.height - this.radius * 2f, this.radius * 2f) * (Vector2) this.gameObject.transform.lossyScale;
        Physics.AddBox(body, this.isTrigger, vector2_1.x, vector2_1.y, (Vector2) position, mass);
        Vector2 vector2_2 = vector2_1 / 2f;
        float y = vector2_2.y;
        vector2_2.y = 0.0f;
        Physics.AddCircle(body, this.isTrigger, y, (Vector2) (position + (Vector3) vector2_2), mass);
        Physics.AddCircle(body, this.isTrigger, y, (Vector2) (position - (Vector3) vector2_2), mass);
      }
      else
      {
        Vector2 vector2_3 = new Vector2(this.radius * 2f, this.height - this.radius * 2f) * (Vector2) this.gameObject.transform.lossyScale;
        Physics.AddBox(body, this.isTrigger, vector2_3.x, vector2_3.y, (Vector2) position, mass);
        Vector2 vector2_4 = vector2_3 / 2f;
        float x = vector2_4.x;
        vector2_4.x = 0.0f;
        Physics.AddCircle(body, this.isTrigger, x, (Vector2) (position + (Vector3) vector2_4), mass);
        Physics.AddCircle(body, this.isTrigger, x, (Vector2) (position - (Vector3) vector2_4), mass);
      }
    }
  }
}
