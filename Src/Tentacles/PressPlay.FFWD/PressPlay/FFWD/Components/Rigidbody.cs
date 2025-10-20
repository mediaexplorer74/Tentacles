// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.Rigidbody
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class Rigidbody : Component
  {
    private float _mass = 1f;
    private float _drag;
    private Body body;

    public float mass
    {
      get => this._mass;
      set
      {
        this._mass = value;
        this.RescaleMass();
      }
    }

    public float drag
    {
      get
      {
        if (this.body != null)
          this._drag = this.body.LinearDamping;
        return this._drag;
      }
      set
      {
        this._drag = value;
        if (this.body == null)
          return;
        this.body.LinearDamping = value;
      }
    }

    public float angularDrag { get; set; }

    public bool isKinematic { get; set; }

    public bool freezeRotation { get; set; }

    public override void Awake()
    {
      if (this.collider == null)
        return;
      this.body = Physics.AddBody();
      this.body.Position = (Microsoft.Xna.Framework.Vector2) this.transform.position;
      this.body.Rotation = -MathHelper.ToRadians(this.transform.rotation.eulerAngles.y);
      this.body.UserData = (Component) this;
      this.body.BodyType = this.isKinematic ? BodyType.Kinematic : BodyType.Dynamic;
      this.body.Enabled = this.gameObject.active;
      this.body.LinearDamping = this.drag;
      this.body.AngularDamping = this.angularDrag;
      this.body.FixedRotation = this.freezeRotation;
      this.collider.AddCollider(this.body, this.mass);
      this.RescaleMass();
    }

    protected override void Destroy()
    {
      base.Destroy();
      if (this.body == null)
        return;
      Physics.RemoveBody(this.body);
    }

    private void RescaleMass()
    {
      if (this.body == null || (double) this.body.Mass <= 0.0)
        return;
      float num = this.mass / this.body.Mass;
      for (int index = 0; index < this.body.FixtureList.Count; ++index)
        this.body.FixtureList[index].Shape.Density *= num;
      this.body.ResetMassData();
    }

    [ContentSerializerIgnore]
    public PressPlay.FFWD.Vector3 velocity
    {
      get => this.body == null ? PressPlay.FFWD.Vector3.zero : (PressPlay.FFWD.Vector3) this.body.LinearVelocity;
      set
      {
        if (this.body == null)
          return;
        this.body.LinearVelocity = (Microsoft.Xna.Framework.Vector2) value;
      }
    }

    public void AddForce(PressPlay.FFWD.Vector3 elasticityForce)
    {
      this.AddForce(elasticityForce, ForceMode.Force);
    }

    public void AddForce(PressPlay.FFWD.Vector3 elasticityForce, ForceMode mode)
    {
      switch (mode)
      {
        case ForceMode.Force:
          this.body.ApplyForce((Microsoft.Xna.Framework.Vector2) elasticityForce, (Microsoft.Xna.Framework.Vector2) this.gameObject.transform.position);
          break;
        case ForceMode.Impulse:
          this.body.ApplyLinearImpulse((Microsoft.Xna.Framework.Vector2) elasticityForce, (Microsoft.Xna.Framework.Vector2) this.gameObject.transform.position);
          break;
      }
    }

    public void MovePosition(PressPlay.FFWD.Vector3 position)
    {
      if (this.body == null)
        return;
      Microsoft.Xna.Framework.Vector2 position1 = (Microsoft.Xna.Framework.Vector2) position;
      this.body.SetTransformIgnoreContacts(ref position1, this.body.Rotation);
      Physics.RemoveStays(this.collider);
    }

    internal void MoveRotation(PressPlay.FFWD.Quaternion localRotation)
    {
    }
  }
}
