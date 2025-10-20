// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.Collider
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public abstract class Collider : Component
  {
    public bool isTrigger;
    [ContentSerializer(Optional = true)]
    public string material;
    [ContentSerializerIgnore]
    public Body connectedBody;
    private bool _allowTurnOff;
    protected PressPlay.FFWD.Vector3 lastResizeScale;

    [ContentSerializerIgnore]
    public bool allowTurnOff
    {
      get => this._allowTurnOff;
      set
      {
        this._allowTurnOff = value;
        if (!this._allowTurnOff)
          return;
        Physics.AddMovingBody(this.connectedBody);
      }
    }

    public Bounds bounds
    {
      get
      {
        if (this.connectedBody == null)
          return new Bounds();
        AABB aabb;
        this.connectedBody.FixtureList[0].GetAABB(out aabb, 0);
        return Physics.BoundsFromAABB(aabb, 10f);
      }
    }

    public override void Awake()
    {
      if (this.rigidbody != null)
        return;
      this.connectedBody = Physics.AddBody();
      this.connectedBody.Position = (Microsoft.Xna.Framework.Vector2) this.transform.position;
      this.connectedBody.Rotation = -MathHelper.ToRadians(this.transform.rotation.eulerAngles.y);
      this.connectedBody.UserData = (Component) this;
      this.connectedBody.BodyType = this.gameObject.isStatic ? BodyType.Static : BodyType.Kinematic;
      this.AddCollider(this.connectedBody, 1f);
    }

    protected override void Destroy()
    {
      base.Destroy();
      if (this.connectedBody == null)
        return;
      Physics.RemoveBody(this.connectedBody);
    }

    internal void SetStatic(bool isStatic)
    {
      if (this.connectedBody == null)
        return;
      this.connectedBody.BodyType = isStatic ? BodyType.Static : BodyType.Kinematic;
      if (isStatic)
        return;
      Physics.AddMovingBody(this.connectedBody);
    }

    internal void AddCollider(Body body, float mass)
    {
      this.DoAddCollider(body, mass);
      if (body.BodyType == BodyType.Static)
        return;
      if (this.rigidbody == null)
        Physics.AddMovingBody(body);
      else
        Physics.AddRigidBody(body);
    }

    protected abstract void DoAddCollider(Body body, float mass);

    internal void ResizeConnectedBody()
    {
      if (this.lastResizeScale == this.transform.lossyScale)
        return;
      for (int index = 0; index < this.connectedBody.FixtureList.Count; ++index)
        this.connectedBody.DestroyFixture(this.connectedBody.FixtureList[index]);
      this.AddCollider(this.connectedBody, this.connectedBody.Mass);
    }

    internal void MovePosition(PressPlay.FFWD.Vector3 position)
    {
      if (this.connectedBody == null || this.connectedBody.BodyType == BodyType.Static)
        return;
      Microsoft.Xna.Framework.Vector2 position1 = (Microsoft.Xna.Framework.Vector2) position;
      this.connectedBody.SetTransformIgnoreContacts(ref position1, this.connectedBody.Rotation);
      Physics.RemoveStays(this);
    }

    public bool Raycast(PressPlay.FFWD.Ray ray, out RaycastHit hitInfo, float distance)
    {
      bool flag = Physics.Raycast(this.connectedBody, ray, out hitInfo, distance);
      hitInfo.collider = this;
      return flag;
    }
  }
}
