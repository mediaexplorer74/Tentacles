// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Contacts.Contact
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Dynamics.Contacts
{
  public class Contact
  {
    private static EdgeShape _edge = new EdgeShape();
    private static Contact.ContactType[,] _registers = new Contact.ContactType[4, 4]
    {
      {
        Contact.ContactType.Circle,
        Contact.ContactType.EdgeAndCircle,
        Contact.ContactType.PolygonAndCircle,
        Contact.ContactType.LoopAndCircle
      },
      {
        Contact.ContactType.EdgeAndCircle,
        Contact.ContactType.NotSupported,
        Contact.ContactType.EdgeAndPolygon,
        Contact.ContactType.NotSupported
      },
      {
        Contact.ContactType.PolygonAndCircle,
        Contact.ContactType.EdgeAndPolygon,
        Contact.ContactType.Polygon,
        Contact.ContactType.LoopAndPolygon
      },
      {
        Contact.ContactType.LoopAndCircle,
        Contact.ContactType.NotSupported,
        Contact.ContactType.LoopAndPolygon,
        Contact.ContactType.NotSupported
      }
    };
    public Fixture FixtureA;
    public Fixture FixtureB;
    internal ContactFlags Flags;
    public Manifold Manifold;
    internal ContactEdge NodeA = new ContactEdge();
    internal ContactEdge NodeB = new ContactEdge();
    public float TOI;
    internal int TOICount;
    private Contact.ContactType _type;

    private Contact(Fixture fA, int indexA, Fixture fB, int indexB)
    {
      this.Reset(fA, indexA, fB, indexB);
    }

    public bool Enabled
    {
      set
      {
        if (value)
          this.Flags |= ContactFlags.Enabled;
        else
          this.Flags &= ~ContactFlags.Enabled;
      }
      get => (this.Flags & ContactFlags.Enabled) == ContactFlags.Enabled;
    }

    public int ChildIndexA { get; internal set; }

    public int ChildIndexB { get; internal set; }

    public void GetManifold(out Manifold manifold) => manifold = this.Manifold;

    public void GetWorldManifold(out Vector2 normal, out FixedArray2<Vector2> points)
    {
      Body body1 = this.FixtureA.Body;
      Body body2 = this.FixtureB.Body;
      Shape shape1 = this.FixtureA.Shape;
      Shape shape2 = this.FixtureB.Shape;
      FarseerPhysics.Collision.Collision.GetWorldManifold(ref this.Manifold, ref body1.Xf, shape1.Radius, ref body2.Xf, shape2.Radius, out normal, out points);
    }

    public bool IsTouching() => (this.Flags & ContactFlags.Touching) == ContactFlags.Touching;

    public void FlagForFiltering() => this.Flags |= ContactFlags.Filter;

    private void Reset(Fixture fA, int indexA, Fixture fB, int indexB)
    {
      this.Flags = ContactFlags.Enabled;
      this.FixtureA = fA;
      this.FixtureB = fB;
      this.ChildIndexA = indexA;
      this.ChildIndexB = indexB;
      this.Manifold.PointCount = 0;
      this.NodeA.Contact = (Contact) null;
      this.NodeA.Prev = (ContactEdge) null;
      this.NodeA.Next = (ContactEdge) null;
      this.NodeA.Other = (Body) null;
      this.NodeB.Contact = (Contact) null;
      this.NodeB.Prev = (ContactEdge) null;
      this.NodeB.Next = (ContactEdge) null;
      this.NodeB.Other = (Body) null;
      this.TOICount = 0;
    }

    internal void Update(ContactManager contactManager)
    {
      Manifold manifold = this.Manifold;
      this.Flags |= ContactFlags.Enabled;
      bool flag1 = (this.Flags & ContactFlags.Touching) == ContactFlags.Touching;
      bool flag2 = this.FixtureA.IsSensor || this.FixtureB.IsSensor;
      Body body1 = this.FixtureA.Body;
      Body body2 = this.FixtureB.Body;
      bool flag3;
      if (flag2)
      {
        flag3 = AABB.TestOverlap(this.FixtureA.Shape, this.ChildIndexA, this.FixtureB.Shape, this.ChildIndexB, ref body1.Xf, ref body2.Xf);
        this.Manifold.PointCount = 0;
      }
      else
      {
        this.Evaluate(ref this.Manifold, ref body1.Xf, ref body2.Xf);
        flag3 = this.Manifold.PointCount > 0;
        for (int index1 = 0; index1 < this.Manifold.PointCount; ++index1)
        {
          ManifoldPoint point1 = this.Manifold.Points[index1] with
          {
            NormalImpulse = 0.0f,
            TangentImpulse = 0.0f
          };
          ContactID id = point1.Id;
          bool flag4 = false;
          for (int index2 = 0; index2 < manifold.PointCount; ++index2)
          {
            ManifoldPoint point2 = manifold.Points[index2];
            if ((int) point2.Id.Key == (int) id.Key)
            {
              point1.NormalImpulse = point2.NormalImpulse;
              point1.TangentImpulse = point2.TangentImpulse;
              flag4 = true;
              break;
            }
          }
          if (!flag4)
          {
            point1.NormalImpulse = 0.0f;
            point1.TangentImpulse = 0.0f;
          }
          this.Manifold.Points[index1] = point1;
        }
        if (flag3 != flag1)
        {
          body1.Awake = true;
          body2.Awake = true;
        }
      }
      if (flag3)
        this.Flags |= ContactFlags.Touching;
      else
        this.Flags &= ~ContactFlags.Touching;
      if (!flag1 && flag3)
      {
        if (this.FixtureA.OnCollision != null)
          this.Enabled = this.FixtureA.OnCollision(this.FixtureA, this.FixtureB, this);
        if (this.FixtureB.OnCollision != null)
          this.Enabled = this.FixtureB.OnCollision(this.FixtureB, this.FixtureA, this);
        if (contactManager.BeginContact != null)
          this.Enabled = contactManager.BeginContact(this);
        if (!this.Enabled)
          this.Flags &= ~ContactFlags.Touching;
      }
      if (flag1 && !flag3)
      {
        if (this.FixtureA.OnSeparation != null)
          this.FixtureA.OnSeparation(this.FixtureA, this.FixtureB);
        if (this.FixtureB.OnSeparation != null)
          this.FixtureB.OnSeparation(this.FixtureB, this.FixtureA);
        if (contactManager.EndContact != null)
          contactManager.EndContact(this);
      }
      if (flag2 || contactManager.PreSolve == null)
        return;
      contactManager.PreSolve(this, ref manifold);
    }

    private void Evaluate(
      ref Manifold manifold,
      ref Transform transformA,
      ref Transform transformB)
    {
      switch (this._type)
      {
        case Contact.ContactType.Polygon:
          FarseerPhysics.Collision.Collision.CollidePolygons(ref manifold, (PolygonShape) this.FixtureA.Shape, ref transformA, (PolygonShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.PolygonAndCircle:
          FarseerPhysics.Collision.Collision.CollidePolygonAndCircle(ref manifold, (PolygonShape) this.FixtureA.Shape, ref transformA, (CircleShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.Circle:
          FarseerPhysics.Collision.Collision.CollideCircles(ref manifold, (CircleShape) this.FixtureA.Shape, ref transformA, (CircleShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.EdgeAndPolygon:
          FarseerPhysics.Collision.Collision.CollideEdgeAndPolygon(ref manifold, (EdgeShape) this.FixtureA.Shape, ref transformA, (PolygonShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.EdgeAndCircle:
          FarseerPhysics.Collision.Collision.CollideEdgeAndCircle(ref manifold, (EdgeShape) this.FixtureA.Shape, ref transformA, (CircleShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.LoopAndPolygon:
          ((LoopShape) this.FixtureA.Shape).GetChildEdge(ref Contact._edge, this.ChildIndexA);
          FarseerPhysics.Collision.Collision.CollideEdgeAndPolygon(ref manifold, Contact._edge, ref transformA, (PolygonShape) this.FixtureB.Shape, ref transformB);
          break;
        case Contact.ContactType.LoopAndCircle:
          ((LoopShape) this.FixtureA.Shape).GetChildEdge(ref Contact._edge, this.ChildIndexA);
          FarseerPhysics.Collision.Collision.CollideEdgeAndCircle(ref manifold, Contact._edge, ref transformA, (CircleShape) this.FixtureB.Shape, ref transformB);
          break;
      }
    }

    internal static Contact Create(Fixture fixtureA, int indexA, Fixture fixtureB, int indexB)
    {
      ShapeType shapeType1 = fixtureA.ShapeType;
      ShapeType shapeType2 = fixtureB.ShapeType;
      Queue<Contact> contactPool = fixtureA.Body.World.ContactPool;
      Contact contact;
      if (contactPool.Count > 0)
      {
        contact = contactPool.Dequeue();
        if ((shapeType1 >= shapeType2 || shapeType1 == ShapeType.Edge && shapeType2 == ShapeType.Polygon) && (shapeType2 != ShapeType.Edge || shapeType1 != ShapeType.Polygon))
          contact.Reset(fixtureA, indexA, fixtureB, indexB);
        else
          contact.Reset(fixtureB, indexB, fixtureA, indexA);
      }
      else
        contact = shapeType1 < shapeType2 && (shapeType1 != ShapeType.Edge || shapeType2 != ShapeType.Polygon) || shapeType2 == ShapeType.Edge && shapeType1 == ShapeType.Polygon ? new Contact(fixtureB, indexB, fixtureA, indexA) : new Contact(fixtureA, indexA, fixtureB, indexB);
      contact._type = Contact._registers[(int) shapeType1, (int) shapeType2];
      return contact;
    }

    internal void Destroy()
    {
      this.FixtureA.Body.World.ContactPool.Enqueue(this);
      this.Reset((Fixture) null, 0, (Fixture) null, 0);
    }

    private enum ContactType
    {
      NotSupported,
      Polygon,
      PolygonAndCircle,
      Circle,
      EdgeAndPolygon,
      EdgeAndCircle,
      LoopAndPolygon,
      LoopAndCircle,
    }
  }
}
