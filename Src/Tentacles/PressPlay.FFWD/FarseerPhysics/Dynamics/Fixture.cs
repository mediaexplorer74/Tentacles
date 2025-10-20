// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Fixture
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class Fixture : IDisposable
  {
    private static int _fixtureIdCounter;
    public AfterCollisionEventHandler AfterCollision;
    public BeforeCollisionEventHandler BeforeCollision;
    public OnCollisionEventHandler OnCollision;
    public OnSeparationEventHandler OnSeparation;
    public FixtureProxy[] Proxies;
    public int ProxyCount;
    internal Category _collidesWith;
    internal Category _collisionCategories;
    internal short _collisionGroup;
    internal Dictionary<int, bool> _collisionIgnores;
    private float _friction;
    private float _restitution;

    internal Fixture()
    {
    }

    public Fixture(Body body, Shape shape)
      : this(body, shape, (object) null)
    {
    }

    public Fixture(Body body, Shape shape, object userData)
    {
      this._collisionCategories = !Settings.UseFPECollisionCategories ? Category.Cat1 : Category.All;
      this._collidesWith = Category.All;
      this._collisionGroup = (short) 0;
      this.Friction = 0.2f;
      this.Restitution = 0.0f;
      this.IsSensor = false;
      this.Body = body;
      this.UserData = userData;
      this.Shape = shape.Clone();
      this.RegisterFixture();
    }

    public short CollisionGroup
    {
      set
      {
        if ((int) this._collisionGroup == (int) value)
          return;
        this._collisionGroup = value;
        this.Refilter();
      }
      get => this._collisionGroup;
    }

    public Category CollidesWith
    {
      get => this._collidesWith;
      set
      {
        if (this._collidesWith == value)
          return;
        this._collidesWith = value;
        this.Refilter();
      }
    }

    public Category CollisionCategories
    {
      get => this._collisionCategories;
      set
      {
        if (this._collisionCategories == value)
          return;
        this._collisionCategories = value;
        this.Refilter();
      }
    }

    public ShapeType ShapeType => this.Shape.ShapeType;

    public Shape Shape { get; internal set; }

    public bool IsSensor { get; set; }

    public Body Body { get; internal set; }

    public object UserData { get; set; }

    public float Friction
    {
      get => this._friction;
      set => this._friction = value;
    }

    public float Restitution
    {
      get => this._restitution;
      set => this._restitution = value;
    }

    public int FixtureId { get; private set; }

    public void Dispose()
    {
      this.Body.Dispose();
      GC.SuppressFinalize((object) this);
    }

    public void RestoreCollisionWith(Fixture fixture)
    {
      if (this._collisionIgnores == null || !this._collisionIgnores.ContainsKey(fixture.FixtureId))
        return;
      this._collisionIgnores[fixture.FixtureId] = false;
      this.Refilter();
    }

    public void IgnoreCollisionWith(Fixture fixture)
    {
      if (this._collisionIgnores == null)
        this._collisionIgnores = new Dictionary<int, bool>();
      if (this._collisionIgnores.ContainsKey(fixture.FixtureId))
        this._collisionIgnores[fixture.FixtureId] = true;
      else
        this._collisionIgnores.Add(fixture.FixtureId, true);
      this.Refilter();
    }

    public bool IsFixtureIgnored(Fixture fixture)
    {
      return this._collisionIgnores != null && this._collisionIgnores.ContainsKey(fixture.FixtureId) && this._collisionIgnores[fixture.FixtureId];
    }

    internal void Refilter()
    {
      for (ContactEdge contactEdge = this.Body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
      {
        Contact contact = contactEdge.Contact;
        Fixture fixtureA = contact.FixtureA;
        Fixture fixtureB = contact.FixtureB;
        if (fixtureA == this || fixtureB == this)
          contact.FlagForFiltering();
      }
      World world = this.Body.World;
      if (world == null)
        return;
      IBroadPhase broadPhase = world.ContactManager.BroadPhase;
      for (int index = 0; index < this.ProxyCount; ++index)
        broadPhase.TouchProxy(this.Proxies[index].ProxyId);
    }

    private void RegisterFixture()
    {
      this.Proxies = new FixtureProxy[this.Shape.ChildCount];
      this.ProxyCount = 0;
      this.FixtureId = Fixture._fixtureIdCounter++;
      if ((this.Body.Flags & BodyFlags.Enabled) == BodyFlags.Enabled)
        this.CreateProxies(this.Body.World.ContactManager.BroadPhase, ref this.Body.Xf);
      this.Body.FixtureList.Add(this);
      if ((double) this.Shape._density > 0.0)
        this.Body.ResetMassData();
      this.Body.World.Flags |= WorldFlags.NewFixture;
      if (this.Body.World.FixtureAdded == null)
        return;
      this.Body.World.FixtureAdded(this);
    }

    public bool TestPoint(ref Vector2 point) => this.Shape.TestPoint(ref this.Body.Xf, ref point);

    public bool RayCast(out RayCastOutput output, ref RayCastInput input, int childIndex)
    {
      return this.Shape.RayCast(out output, ref input, ref this.Body.Xf, childIndex);
    }

    public void GetAABB(out AABB aabb, int childIndex) => aabb = this.Proxies[childIndex].AABB;

    public Fixture Clone(Body body)
    {
      Fixture fixture = new Fixture();
      fixture.Body = body;
      fixture.Shape = this.Shape.Clone();
      fixture.UserData = this.UserData;
      fixture.Restitution = this.Restitution;
      fixture.Friction = this.Friction;
      fixture.IsSensor = this.IsSensor;
      fixture._collisionGroup = this.CollisionGroup;
      fixture._collisionCategories = this.CollisionCategories;
      fixture._collidesWith = this.CollidesWith;
      if (this._collisionIgnores != null)
      {
        fixture._collisionIgnores = new Dictionary<int, bool>();
        foreach (KeyValuePair<int, bool> collisionIgnore in this._collisionIgnores)
          fixture._collisionIgnores.Add(collisionIgnore.Key, collisionIgnore.Value);
      }
      fixture.RegisterFixture();
      return fixture;
    }

    public Fixture DeepClone() => this.Clone(this.Body.Clone());

    internal void Destroy()
    {
      this.Proxies = (FixtureProxy[]) null;
      this.Shape = (Shape) null;
      this.BeforeCollision = (BeforeCollisionEventHandler) null;
      this.OnCollision = (OnCollisionEventHandler) null;
      this.OnSeparation = (OnSeparationEventHandler) null;
      this.AfterCollision = (AfterCollisionEventHandler) null;
      if (this.Body.World.FixtureRemoved == null)
        return;
      this.Body.World.FixtureRemoved(this);
    }

    internal void CreateProxies(IBroadPhase broadPhase, ref Transform xf)
    {
      this.ProxyCount = this.Shape.ChildCount;
      for (int childIndex = 0; childIndex < this.ProxyCount; ++childIndex)
      {
        FixtureProxy proxy = new FixtureProxy();
        this.Shape.ComputeAABB(out proxy.AABB, ref xf, childIndex);
        proxy.Fixture = this;
        proxy.ChildIndex = childIndex;
        proxy.ProxyId = broadPhase.AddProxy(ref proxy);
        this.Proxies[childIndex] = proxy;
      }
    }

    internal void DestroyProxies(IBroadPhase broadPhase)
    {
      for (int index = 0; index < this.ProxyCount; ++index)
      {
        broadPhase.RemoveProxy(this.Proxies[index].ProxyId);
        this.Proxies[index].ProxyId = -1;
      }
      this.ProxyCount = 0;
    }

    internal void Synchronize(
      IBroadPhase broadPhase,
      ref Transform transform1,
      ref Transform transform2)
    {
      if (this.ProxyCount == 0)
        return;
      for (int index = 0; index < this.ProxyCount; ++index)
      {
        FixtureProxy proxy = this.Proxies[index];
        AABB aabb1;
        this.Shape.ComputeAABB(out aabb1, ref transform1, proxy.ChildIndex);
        AABB aabb2;
        this.Shape.ComputeAABB(out aabb2, ref transform2, proxy.ChildIndex);
        proxy.AABB.Combine(ref aabb1, ref aabb2);
        Vector2 displacement = transform2.Position - transform1.Position;
        broadPhase.MoveProxy(proxy.ProxyId, ref proxy.AABB, displacement);
      }
    }

    internal bool CompareTo(Fixture fixture)
    {
      return this.CollidesWith == fixture.CollidesWith && this.CollisionCategories == fixture.CollisionCategories && (int) this.CollisionGroup == (int) fixture.CollisionGroup && (double) this.Friction == (double) fixture.Friction && this.IsSensor == fixture.IsSensor && (double) this.Restitution == (double) fixture.Restitution && this.Shape.CompareTo(fixture.Shape) && this.UserData == fixture.UserData;
    }
  }
}
