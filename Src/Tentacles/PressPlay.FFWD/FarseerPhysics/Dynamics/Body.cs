// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Body
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using PressPlay.FFWD;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class Body : IDisposable
  {
    public ControllerFilter ControllerFilter;
    public PhysicsLogicFilter PhysicsLogicFilter;
    public int BodyId;
    private static int _bodyIdCounter;
    internal float AngularVelocityInternal;
    internal BodyFlags Flags;
    internal Microsoft.Xna.Framework.Vector2 Force;
    internal float InvI;
    internal float InvMass;
    internal Microsoft.Xna.Framework.Vector2 LinearVelocityInternal;
    internal float SleepTime;
    internal Sweep Sweep;
    internal float Torque;
    internal World World;
    internal FarseerPhysics.Common.Transform Xf;
    private float _angularDamping;
    private BodyType _bodyType;
    private float _inertia;
    private float _linearDamping;
    private float _mass;

    internal Body() => this.FixtureList = new List<Fixture>(32);

    public Body(World world)
      : this(world, (Component) null)
    {
    }

    public Body(World world, Component userData)
    {
      this.FixtureList = new List<Fixture>(32);
      this.BodyId = Body._bodyIdCounter++;
      this.World = world;
      this.UserData = userData;
      this.FixedRotation = false;
      this.IsBullet = false;
      this.SleepingAllowed = true;
      this.Awake = true;
      this.BodyType = BodyType.Static;
      this.Enabled = true;
      this.Xf.R.Set(0.0f);
      world.AddBody(this);
    }

    public float Revolutions => this.Rotation / 3.14159274f;

    public BodyType BodyType
    {
      get => this._bodyType;
      set
      {
        if (this._bodyType == value)
          return;
        this._bodyType = value;
        this.ResetMassData();
        if (this._bodyType == BodyType.Static)
        {
          this.LinearVelocityInternal = Microsoft.Xna.Framework.Vector2.Zero;
          this.AngularVelocityInternal = 0.0f;
        }
        this.Awake = true;
        this.Force = Microsoft.Xna.Framework.Vector2.Zero;
        this.Torque = 0.0f;
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].Refilter();
      }
    }

    public Microsoft.Xna.Framework.Vector2 LinearVelocity
    {
      set
      {
        if (this._bodyType == BodyType.Static)
          return;
        if ((double) Microsoft.Xna.Framework.Vector2.Dot(value, value) > 0.0)
          this.Awake = true;
        this.LinearVelocityInternal = value;
      }
      get => this.LinearVelocityInternal;
    }

    public float AngularVelocity
    {
      set
      {
        if (this._bodyType == BodyType.Static)
          return;
        if ((double) value * (double) value > 0.0)
          this.Awake = true;
        this.AngularVelocityInternal = value;
      }
      get => this.AngularVelocityInternal;
    }

    public float LinearDamping
    {
      get => this._linearDamping;
      set => this._linearDamping = value;
    }

    public float AngularDamping
    {
      get => this._angularDamping;
      set => this._angularDamping = value;
    }

    public bool IsBullet
    {
      set
      {
        if (value)
          this.Flags |= BodyFlags.Bullet;
        else
          this.Flags &= ~BodyFlags.Bullet;
      }
      get => (this.Flags & BodyFlags.Bullet) == BodyFlags.Bullet;
    }

    public bool SleepingAllowed
    {
      set
      {
        if (value)
        {
          this.Flags |= BodyFlags.AutoSleep;
        }
        else
        {
          this.Flags &= ~BodyFlags.AutoSleep;
          this.Awake = true;
        }
      }
      get => (this.Flags & BodyFlags.AutoSleep) == BodyFlags.AutoSleep;
    }

    public bool Awake
    {
      set
      {
        if (value)
        {
          if ((this.Flags & BodyFlags.Awake) != BodyFlags.None)
            return;
          this.Flags |= BodyFlags.Awake;
          this.SleepTime = 0.0f;
        }
        else
        {
          this.Flags &= ~BodyFlags.Awake;
          this.SleepTime = 0.0f;
          this.LinearVelocityInternal = Microsoft.Xna.Framework.Vector2.Zero;
          this.AngularVelocityInternal = 0.0f;
          this.Force = Microsoft.Xna.Framework.Vector2.Zero;
          this.Torque = 0.0f;
        }
      }
      get => (this.Flags & BodyFlags.Awake) == BodyFlags.Awake;
    }

    public bool Enabled
    {
      set
      {
        if (value == this.Enabled)
          return;
        if (value)
        {
          this.Flags |= BodyFlags.Enabled;
          IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
          for (int index = 0; index < this.FixtureList.Count; ++index)
            this.FixtureList[index].CreateProxies(broadPhase, ref this.Xf);
        }
        else
        {
          this.Flags &= ~BodyFlags.Enabled;
          IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
          for (int index = 0; index < this.FixtureList.Count; ++index)
            this.FixtureList[index].DestroyProxies(broadPhase);
          ContactEdge contactEdge1 = this.ContactList;
          while (contactEdge1 != null)
          {
            ContactEdge contactEdge2 = contactEdge1;
            contactEdge1 = contactEdge1.Next;
            this.World.ContactManager.Destroy(contactEdge2.Contact);
          }
          this.ContactList = (ContactEdge) null;
        }
      }
      get => (this.Flags & BodyFlags.Enabled) == BodyFlags.Enabled;
    }

    public bool FixedRotation
    {
      set
      {
        if (value)
          this.Flags |= BodyFlags.FixedRotation;
        else
          this.Flags &= ~BodyFlags.FixedRotation;
        this.ResetMassData();
      }
      get => (this.Flags & BodyFlags.FixedRotation) == BodyFlags.FixedRotation;
    }

    public List<Fixture> FixtureList { get; internal set; }

    public JointEdge JointList { get; internal set; }

    public ContactEdge ContactList { get; internal set; }

    public Component UserData { get; set; }

    public Microsoft.Xna.Framework.Vector2 Position
    {
      get => this.Xf.Position;
      set => this.SetTransform(ref value, this.Rotation);
    }

    public float Rotation
    {
      get => this.Sweep.A;
      set => this.SetTransform(ref this.Xf.Position, value);
    }

    public bool IsStatic
    {
      get => this._bodyType == BodyType.Static;
      set
      {
        if (value)
          this.BodyType = BodyType.Static;
        else
          this.BodyType = BodyType.Dynamic;
      }
    }

    public bool IgnoreGravity
    {
      get => (this.Flags & BodyFlags.IgnoreGravity) == BodyFlags.IgnoreGravity;
      set
      {
        if (value)
          this.Flags |= BodyFlags.IgnoreGravity;
        else
          this.Flags &= ~BodyFlags.IgnoreGravity;
      }
    }

    public Microsoft.Xna.Framework.Vector2 WorldCenter => this.Sweep.C;

    public Microsoft.Xna.Framework.Vector2 LocalCenter
    {
      get => this.Sweep.LocalCenter;
      set
      {
        if (this._bodyType != BodyType.Dynamic)
          return;
        Microsoft.Xna.Framework.Vector2 c = this.Sweep.C;
        this.Sweep.LocalCenter = value;
        this.Sweep.C0 = this.Sweep.C = MathUtils.Multiply(ref this.Xf, ref this.Sweep.LocalCenter);
        Microsoft.Xna.Framework.Vector2 vector2 = this.Sweep.C - c;
        this.LinearVelocityInternal += new Microsoft.Xna.Framework.Vector2(-this.AngularVelocityInternal * vector2.Y, this.AngularVelocityInternal * vector2.X);
      }
    }

    public float Mass
    {
      get => this._mass;
      set
      {
        if (this._bodyType != BodyType.Dynamic)
          return;
        this._mass = value;
        if ((double) this._mass <= 0.0)
          this._mass = 1f;
        this.InvMass = 1f / this._mass;
      }
    }

    public float Inertia
    {
      get
      {
        return this._inertia + this.Mass * Microsoft.Xna.Framework.Vector2.Dot(this.Sweep.LocalCenter, this.Sweep.LocalCenter);
      }
      set
      {
        if (this._bodyType != BodyType.Dynamic || (double) value <= 0.0 || (this.Flags & BodyFlags.FixedRotation) != BodyFlags.None)
          return;
        this._inertia = value - this.Mass * Microsoft.Xna.Framework.Vector2.Dot(this.LocalCenter, this.LocalCenter);
        this.InvI = 1f / this._inertia;
      }
    }

    public float Restitution
    {
      get
      {
        float num = 0.0f;
        for (int index = 0; index < this.FixtureList.Count; ++index)
        {
          Fixture fixture = this.FixtureList[index];
          num += fixture.Restitution;
        }
        return num / (float) this.FixtureList.Count;
      }
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].Restitution = value;
      }
    }

    public float Friction
    {
      get
      {
        float num = 0.0f;
        for (int index = 0; index < this.FixtureList.Count; ++index)
        {
          Fixture fixture = this.FixtureList[index];
          num += fixture.Friction;
        }
        return num / (float) this.FixtureList.Count;
      }
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].Friction = value;
      }
    }

    public Category CollisionCategories
    {
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].CollisionCategories = value;
      }
    }

    public Category CollidesWith
    {
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].CollidesWith = value;
      }
    }

    public short CollisionGroup
    {
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].CollisionGroup = value;
      }
    }

    public bool IsSensor
    {
      set
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].IsSensor = value;
      }
    }

    public bool IgnoreCCD
    {
      get => (this.Flags & BodyFlags.IgnoreCCD) == BodyFlags.IgnoreCCD;
      set
      {
        if (value)
          this.Flags |= BodyFlags.IgnoreCCD;
        else
          this.Flags &= ~BodyFlags.IgnoreCCD;
      }
    }

    public void Dispose()
    {
      this.World.RemoveBody(this);
      GC.SuppressFinalize((object) this);
    }

    public void ResetDynamics()
    {
      this.Torque = 0.0f;
      this.AngularVelocityInternal = 0.0f;
      this.Force = Microsoft.Xna.Framework.Vector2.Zero;
      this.LinearVelocityInternal = Microsoft.Xna.Framework.Vector2.Zero;
    }

    public Fixture CreateFixture(Shape shape) => new Fixture(this, shape);

    public Fixture CreateFixture(Shape shape, object userData)
    {
      return new Fixture(this, shape, userData);
    }

    public void DestroyFixture(Fixture fixture)
    {
      ContactEdge contactEdge = this.ContactList;
      while (contactEdge != null)
      {
        Contact contact = contactEdge.Contact;
        contactEdge = contactEdge.Next;
        Fixture fixtureA = contact.FixtureA;
        Fixture fixtureB = contact.FixtureB;
        if (fixture == fixtureA || fixture == fixtureB)
          this.World.ContactManager.Destroy(contact);
      }
      if ((this.Flags & BodyFlags.Enabled) == BodyFlags.Enabled)
      {
        IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
        fixture.DestroyProxies(broadPhase);
      }
      this.FixtureList.Remove(fixture);
      fixture.Destroy();
      fixture.Body = (Body) null;
      this.ResetMassData();
    }

    public void SetTransform(ref Microsoft.Xna.Framework.Vector2 position, float rotation)
    {
      this.SetTransformIgnoreContacts(ref position, rotation);
      this.World.ContactManager.FindNewContacts();
    }

    public void SetTransform(Microsoft.Xna.Framework.Vector2 position, float rotation)
    {
      this.SetTransform(ref position, rotation);
    }

    public void SetTransformIgnoreContacts(ref Microsoft.Xna.Framework.Vector2 position, float angle)
    {
      this.Xf.R.Set(angle);
      this.Xf.Position = position;
      this.Sweep.C0 = this.Sweep.C = new Microsoft.Xna.Framework.Vector2((float) ((double) this.Xf.Position.X + (double) this.Xf.R.Col1.X * (double) this.Sweep.LocalCenter.X + (double) this.Xf.R.Col2.X * (double) this.Sweep.LocalCenter.Y), (float) ((double) this.Xf.Position.Y + (double) this.Xf.R.Col1.Y * (double) this.Sweep.LocalCenter.X + (double) this.Xf.R.Col2.Y * (double) this.Sweep.LocalCenter.Y));
      this.Sweep.A0 = this.Sweep.A = angle;
      IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
      for (int index = 0; index < this.FixtureList.Count; ++index)
        this.FixtureList[index].Synchronize(broadPhase, ref this.Xf, ref this.Xf);
    }

    public void GetTransform(out FarseerPhysics.Common.Transform transform) => transform = this.Xf;

    public void ApplyForce(Microsoft.Xna.Framework.Vector2 force, Microsoft.Xna.Framework.Vector2 point)
    {
      this.ApplyForce(ref force, ref point);
    }

    public void ApplyForce(ref Microsoft.Xna.Framework.Vector2 force)
    {
      this.ApplyForce(ref force, ref this.Xf.Position);
    }

    public void ApplyForce(Microsoft.Xna.Framework.Vector2 force)
    {
      this.ApplyForce(ref force, ref this.Xf.Position);
    }

    public void ApplyForce(ref Microsoft.Xna.Framework.Vector2 force, ref Microsoft.Xna.Framework.Vector2 point)
    {
      if (this._bodyType != BodyType.Dynamic)
        return;
      if (!this.Awake)
        this.Awake = true;
      this.Force += force;
      this.Torque += (float) (((double) point.X - (double) this.Sweep.C.X) * (double) force.Y - ((double) point.Y - (double) this.Sweep.C.Y) * (double) force.X);
    }

    public void ApplyTorque(float torque)
    {
      if (this._bodyType != BodyType.Dynamic)
        return;
      if (!this.Awake)
        this.Awake = true;
      this.Torque += torque;
    }

    public void ApplyLinearImpulse(Microsoft.Xna.Framework.Vector2 impulse)
    {
      this.ApplyLinearImpulse(ref impulse);
    }

    public void ApplyLinearImpulse(Microsoft.Xna.Framework.Vector2 impulse, Microsoft.Xna.Framework.Vector2 point)
    {
      this.ApplyLinearImpulse(ref impulse, ref point);
    }

    public void ApplyLinearImpulse(ref Microsoft.Xna.Framework.Vector2 impulse)
    {
      if (this._bodyType != BodyType.Dynamic)
        return;
      if (!this.Awake)
        this.Awake = true;
      this.LinearVelocityInternal += this.InvMass * impulse;
    }

    public void ApplyLinearImpulse(ref Microsoft.Xna.Framework.Vector2 impulse, ref Microsoft.Xna.Framework.Vector2 point)
    {
      if (this._bodyType != BodyType.Dynamic)
        return;
      if (!this.Awake)
        this.Awake = true;
      this.LinearVelocityInternal += this.InvMass * impulse;
      this.AngularVelocityInternal += this.InvI * (float) (((double) point.X - (double) this.Sweep.C.X) * (double) impulse.Y - ((double) point.Y - (double) this.Sweep.C.Y) * (double) impulse.X);
    }

    public void ApplyAngularImpulse(float impulse)
    {
      if (this._bodyType != BodyType.Dynamic)
        return;
      if (!this.Awake)
        this.Awake = true;
      this.AngularVelocityInternal += this.InvI * impulse;
    }

    public void ResetMassData()
    {
      this._mass = 0.0f;
      this.InvMass = 0.0f;
      this._inertia = 0.0f;
      this.InvI = 0.0f;
      this.Sweep.LocalCenter = Microsoft.Xna.Framework.Vector2.Zero;
      if (this.BodyType == BodyType.Kinematic)
      {
        this.Sweep.C0 = this.Sweep.C = this.Xf.Position;
      }
      else
      {
        Microsoft.Xna.Framework.Vector2 zero = Microsoft.Xna.Framework.Vector2.Zero;
        foreach (Fixture fixture in this.FixtureList)
        {
          if ((double) fixture.Shape._density != 0.0)
          {
            MassData massData = fixture.Shape.MassData;
            this._mass += massData.Mass;
            zero += massData.Mass * massData.Centroid;
            this._inertia += massData.Inertia;
          }
        }
        if (this.BodyType == BodyType.Static)
        {
          this.Sweep.C0 = this.Sweep.C = this.Xf.Position;
        }
        else
        {
          if ((double) this._mass > 0.0)
          {
            this.InvMass = 1f / this._mass;
            zero *= this.InvMass;
          }
          else
          {
            this._mass = 1f;
            this.InvMass = 1f;
          }
          if ((double) this._inertia > 0.0 && (this.Flags & BodyFlags.FixedRotation) == BodyFlags.None)
          {
            this._inertia -= this._mass * Microsoft.Xna.Framework.Vector2.Dot(zero, zero);
            this.InvI = 1f / this._inertia;
          }
          else
          {
            this._inertia = 0.0f;
            this.InvI = 0.0f;
          }
          Microsoft.Xna.Framework.Vector2 c = this.Sweep.C;
          this.Sweep.LocalCenter = zero;
          this.Sweep.C0 = this.Sweep.C = MathUtils.Multiply(ref this.Xf, ref this.Sweep.LocalCenter);
          Microsoft.Xna.Framework.Vector2 vector2 = this.Sweep.C - c;
          this.LinearVelocityInternal += new Microsoft.Xna.Framework.Vector2(-this.AngularVelocityInternal * vector2.Y, this.AngularVelocityInternal * vector2.X);
        }
      }
    }

    public Microsoft.Xna.Framework.Vector2 GetWorldPoint(ref Microsoft.Xna.Framework.Vector2 localPoint)
    {
      return new Microsoft.Xna.Framework.Vector2((float) ((double) this.Xf.Position.X + (double) this.Xf.R.Col1.X * (double) localPoint.X + (double) this.Xf.R.Col2.X * (double) localPoint.Y), (float) ((double) this.Xf.Position.Y + (double) this.Xf.R.Col1.Y * (double) localPoint.X + (double) this.Xf.R.Col2.Y * (double) localPoint.Y));
    }

    public Microsoft.Xna.Framework.Vector2 GetWorldPoint(Microsoft.Xna.Framework.Vector2 localPoint)
    {
      return this.GetWorldPoint(ref localPoint);
    }

    public Microsoft.Xna.Framework.Vector2 GetWorldVector(ref Microsoft.Xna.Framework.Vector2 localVector)
    {
      return new Microsoft.Xna.Framework.Vector2((float) ((double) this.Xf.R.Col1.X * (double) localVector.X + (double) this.Xf.R.Col2.X * (double) localVector.Y), (float) ((double) this.Xf.R.Col1.Y * (double) localVector.X + (double) this.Xf.R.Col2.Y * (double) localVector.Y));
    }

    public Microsoft.Xna.Framework.Vector2 GetWorldVector(Microsoft.Xna.Framework.Vector2 localVector)
    {
      return this.GetWorldVector(ref localVector);
    }

    public Microsoft.Xna.Framework.Vector2 GetLocalPoint(ref Microsoft.Xna.Framework.Vector2 worldPoint)
    {
      return new Microsoft.Xna.Framework.Vector2((float) (((double) worldPoint.X - (double) this.Xf.Position.X) * (double) this.Xf.R.Col1.X + ((double) worldPoint.Y - (double) this.Xf.Position.Y) * (double) this.Xf.R.Col1.Y), (float) (((double) worldPoint.X - (double) this.Xf.Position.X) * (double) this.Xf.R.Col2.X + ((double) worldPoint.Y - (double) this.Xf.Position.Y) * (double) this.Xf.R.Col2.Y));
    }

    public Microsoft.Xna.Framework.Vector2 GetLocalPoint(Microsoft.Xna.Framework.Vector2 worldPoint)
    {
      return this.GetLocalPoint(ref worldPoint);
    }

    public Microsoft.Xna.Framework.Vector2 GetLocalVector(ref Microsoft.Xna.Framework.Vector2 worldVector)
    {
      return new Microsoft.Xna.Framework.Vector2((float) ((double) worldVector.X * (double) this.Xf.R.Col1.X + (double) worldVector.Y * (double) this.Xf.R.Col1.Y), (float) ((double) worldVector.X * (double) this.Xf.R.Col2.X + (double) worldVector.Y * (double) this.Xf.R.Col2.Y));
    }

    public Microsoft.Xna.Framework.Vector2 GetLocalVector(Microsoft.Xna.Framework.Vector2 worldVector)
    {
      return this.GetLocalVector(ref worldVector);
    }

    public Microsoft.Xna.Framework.Vector2 GetLinearVelocityFromWorldPoint(Microsoft.Xna.Framework.Vector2 worldPoint)
    {
      return this.GetLinearVelocityFromWorldPoint(ref worldPoint);
    }

    public Microsoft.Xna.Framework.Vector2 GetLinearVelocityFromWorldPoint(ref Microsoft.Xna.Framework.Vector2 worldPoint)
    {
      return this.LinearVelocityInternal + new Microsoft.Xna.Framework.Vector2((float) (-(double) this.AngularVelocityInternal * ((double) worldPoint.Y - (double) this.Sweep.C.Y)), this.AngularVelocityInternal * (worldPoint.X - this.Sweep.C.X));
    }

    public Microsoft.Xna.Framework.Vector2 GetLinearVelocityFromLocalPoint(Microsoft.Xna.Framework.Vector2 localPoint)
    {
      return this.GetLinearVelocityFromLocalPoint(ref localPoint);
    }

    public Microsoft.Xna.Framework.Vector2 GetLinearVelocityFromLocalPoint(ref Microsoft.Xna.Framework.Vector2 localPoint)
    {
      return this.GetLinearVelocityFromWorldPoint(this.GetWorldPoint(ref localPoint));
    }

    public Body DeepClone()
    {
      Body body = this.Clone();
      for (int index = 0; index < this.FixtureList.Count; ++index)
        this.FixtureList[index].Clone(body);
      return body;
    }

    public Body Clone()
    {
      Body body = new Body();
      body.World = this.World;
      body.UserData = this.UserData;
      body.LinearDamping = this.LinearDamping;
      body.LinearVelocityInternal = this.LinearVelocityInternal;
      body.AngularDamping = this.AngularDamping;
      body.AngularVelocityInternal = this.AngularVelocityInternal;
      body.Position = this.Position;
      body.Rotation = this.Rotation;
      body._bodyType = this._bodyType;
      body.Flags = this.Flags;
      this.World.AddBody(body);
      return body;
    }

    internal void SynchronizeFixtures()
    {
      FarseerPhysics.Common.Transform transform1 = new FarseerPhysics.Common.Transform();
      float num1 = (float) Math.Cos((double) this.Sweep.A0);
      float num2 = (float) Math.Sin((double) this.Sweep.A0);
      transform1.R.Col1.X = num1;
      transform1.R.Col2.X = -num2;
      transform1.R.Col1.Y = num2;
      transform1.R.Col2.Y = num1;
      transform1.Position.X = this.Sweep.C0.X - (float) ((double) transform1.R.Col1.X * (double) this.Sweep.LocalCenter.X + (double) transform1.R.Col2.X * (double) this.Sweep.LocalCenter.Y);
      transform1.Position.Y = this.Sweep.C0.Y - (float) ((double) transform1.R.Col1.Y * (double) this.Sweep.LocalCenter.X + (double) transform1.R.Col2.Y * (double) this.Sweep.LocalCenter.Y);
      IBroadPhase broadPhase = this.World.ContactManager.BroadPhase;
      for (int index = 0; index < this.FixtureList.Count; ++index)
        this.FixtureList[index].Synchronize(broadPhase, ref transform1, ref this.Xf);
    }

    internal void SynchronizeTransform()
    {
      this.Xf.R.Set(this.Sweep.A);
      float num1 = (float) ((double) this.Xf.R.Col1.X * (double) this.Sweep.LocalCenter.X + (double) this.Xf.R.Col2.X * (double) this.Sweep.LocalCenter.Y);
      float num2 = (float) ((double) this.Xf.R.Col1.Y * (double) this.Sweep.LocalCenter.X + (double) this.Xf.R.Col2.Y * (double) this.Sweep.LocalCenter.Y);
      this.Xf.Position.X = this.Sweep.C.X - num1;
      this.Xf.Position.Y = this.Sweep.C.Y - num2;
    }

    internal bool ShouldCollide(Body other)
    {
      if (this._bodyType != BodyType.Dynamic && other._bodyType != BodyType.Dynamic)
        return false;
      for (JointEdge jointEdge = this.JointList; jointEdge != null; jointEdge = jointEdge.Next)
      {
        if (jointEdge.Other == other && !jointEdge.Joint.CollideConnected)
          return false;
      }
      return true;
    }

    internal void Advance(float alpha)
    {
      this.Sweep.Advance(alpha);
      this.Sweep.C = this.Sweep.C0;
      this.Sweep.A = this.Sweep.A0;
      this.SynchronizeTransform();
    }

    public event OnCollisionEventHandler OnCollision
    {
      add
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].OnCollision += value;
      }
      remove
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].OnCollision -= value;
      }
    }

    public event OnSeparationEventHandler OnSeparation
    {
      add
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].OnSeparation += value;
      }
      remove
      {
        for (int index = 0; index < this.FixtureList.Count; ++index)
          this.FixtureList[index].OnSeparation -= value;
      }
    }

    public void IgnoreCollisionWith(Body other)
    {
      for (int index1 = 0; index1 < this.FixtureList.Count; ++index1)
      {
        Fixture fixture1 = this.FixtureList[index1];
        for (int index2 = 0; index2 < other.FixtureList.Count; ++index2)
        {
          Fixture fixture2 = other.FixtureList[index2];
          fixture1.IgnoreCollisionWith(fixture2);
        }
      }
    }

    public void RestoreCollisionWith(Body other)
    {
      for (int index1 = 0; index1 < this.FixtureList.Count; ++index1)
      {
        Fixture fixture1 = this.FixtureList[index1];
        for (int index2 = 0; index2 < other.FixtureList.Count; ++index2)
        {
          Fixture fixture2 = other.FixtureList[index2];
          fixture1.RestoreCollisionWith(fixture2);
        }
      }
    }
  }
}
