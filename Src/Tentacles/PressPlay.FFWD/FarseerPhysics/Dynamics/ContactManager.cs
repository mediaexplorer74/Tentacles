// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.ContactManager
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics.Contacts;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class ContactManager
  {
    public BeginContactDelegate BeginContact;
    public IBroadPhase BroadPhase;
    public CollisionFilterDelegate ContactFilter;
    public List<Contact> ContactList = new List<Contact>(128);
    public EndContactDelegate EndContact;
    public BroadphaseDelegate OnBroadphaseCollision;
    public PostSolveDelegate PostSolve;
    public PreSolveDelegate PreSolve;

    internal ContactManager(IBroadPhase broadPhase)
    {
      this.BroadPhase = broadPhase;
      this.OnBroadphaseCollision = new BroadphaseDelegate(this.AddPair);
    }

    private void AddPair(ref FixtureProxy proxyA, ref FixtureProxy proxyB)
    {
      Fixture fixture1 = proxyA.Fixture;
      Fixture fixture2 = proxyB.Fixture;
      int childIndex1 = proxyA.ChildIndex;
      int childIndex2 = proxyB.ChildIndex;
      Body body1 = fixture1.Body;
      Body body2 = fixture2.Body;
      if (body1 == body2)
        return;
      for (ContactEdge contactEdge = body2.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
      {
        if (contactEdge.Other == body1)
        {
          Fixture fixtureA = contactEdge.Contact.FixtureA;
          Fixture fixtureB = contactEdge.Contact.FixtureB;
          int childIndexA = contactEdge.Contact.ChildIndexA;
          int childIndexB = contactEdge.Contact.ChildIndexB;
          if (fixtureA == fixture1 && fixtureB == fixture2 && childIndexA == childIndex1 && childIndexB == childIndex2 || fixtureA == fixture2 && fixtureB == fixture1 && childIndexA == childIndex2 && childIndexB == childIndex1)
            return;
        }
      }
      if (!body2.ShouldCollide(body1) || !ContactManager.ShouldCollide(fixture1, fixture2) || this.ContactFilter != null && !this.ContactFilter(fixture1, fixture2) || fixture1.BeforeCollision != null && !fixture1.BeforeCollision(fixture1, fixture2) || fixture2.BeforeCollision != null && !fixture2.BeforeCollision(fixture2, fixture1))
        return;
      Contact contact = Contact.Create(fixture1, childIndex1, fixture2, childIndex2);
      Fixture fixtureA1 = contact.FixtureA;
      Fixture fixtureB1 = contact.FixtureB;
      Body body3 = fixtureA1.Body;
      Body body4 = fixtureB1.Body;
      this.ContactList.Add(contact);
      contact.NodeA.Contact = contact;
      contact.NodeA.Other = body4;
      contact.NodeA.Prev = (ContactEdge) null;
      contact.NodeA.Next = body3.ContactList;
      if (body3.ContactList != null)
        body3.ContactList.Prev = contact.NodeA;
      body3.ContactList = contact.NodeA;
      contact.NodeB.Contact = contact;
      contact.NodeB.Other = body3;
      contact.NodeB.Prev = (ContactEdge) null;
      contact.NodeB.Next = body4.ContactList;
      if (body4.ContactList != null)
        body4.ContactList.Prev = contact.NodeB;
      body4.ContactList = contact.NodeB;
    }

    internal void FindNewContacts() => this.BroadPhase.UpdatePairs(this.OnBroadphaseCollision);

    internal void Destroy(Contact contact)
    {
      Fixture fixtureA = contact.FixtureA;
      Fixture fixtureB = contact.FixtureB;
      Body body1 = fixtureA.Body;
      Body body2 = fixtureB.Body;
      if (this.EndContact != null && contact.IsTouching())
        this.EndContact(contact);
      this.ContactList.Remove(contact);
      if (contact.NodeA.Prev != null)
        contact.NodeA.Prev.Next = contact.NodeA.Next;
      if (contact.NodeA.Next != null)
        contact.NodeA.Next.Prev = contact.NodeA.Prev;
      if (contact.NodeA == body1.ContactList)
        body1.ContactList = contact.NodeA.Next;
      if (contact.NodeB.Prev != null)
        contact.NodeB.Prev.Next = contact.NodeB.Next;
      if (contact.NodeB.Next != null)
        contact.NodeB.Next.Prev = contact.NodeB.Prev;
      if (contact.NodeB == body2.ContactList)
        body2.ContactList = contact.NodeB.Next;
      contact.Destroy();
    }

    internal void Collide()
    {
      for (int index = 0; index < this.ContactList.Count; ++index)
      {
        Contact contact = this.ContactList[index];
        Fixture fixtureA = contact.FixtureA;
        Fixture fixtureB = contact.FixtureB;
        int childIndexA = contact.ChildIndexA;
        int childIndexB = contact.ChildIndexB;
        Body body1 = fixtureA.Body;
        Body body2 = fixtureB.Body;
        if (body1.Awake || body2.Awake)
        {
          if ((contact.Flags & ContactFlags.Filter) == ContactFlags.Filter)
          {
            if (!body2.ShouldCollide(body1))
            {
              this.Destroy(contact);
              continue;
            }
            if (!ContactManager.ShouldCollide(fixtureA, fixtureB))
            {
              this.Destroy(contact);
              continue;
            }
            if (this.ContactFilter != null && !this.ContactFilter(fixtureA, fixtureB))
            {
              this.Destroy(contact);
              continue;
            }
            contact.Flags &= ~ContactFlags.Filter;
          }
          if (!this.BroadPhase.TestOverlap(fixtureA.Proxies[childIndexA].ProxyId, fixtureB.Proxies[childIndexB].ProxyId))
            this.Destroy(contact);
          else
            contact.Update(this);
        }
      }
    }

    private static bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
    {
      if (Settings.UseFPECollisionCategories)
        return ((int) fixtureA.CollisionGroup != (int) fixtureB.CollisionGroup || fixtureA.CollisionGroup == (short) 0 || fixtureB.CollisionGroup == (short) 0) && !((fixtureA.CollisionCategories & fixtureB.CollidesWith) == Category.None & (fixtureB.CollisionCategories & fixtureA.CollidesWith) == Category.None) && !fixtureA.IsFixtureIgnored(fixtureB) && !fixtureB.IsFixtureIgnored(fixtureA);
      if ((int) fixtureA.CollisionGroup == (int) fixtureB.CollisionGroup && fixtureA.CollisionGroup != (short) 0)
        return fixtureA.CollisionGroup > (short) 0;
      bool flag = (fixtureA.CollidesWith & fixtureB.CollisionCategories) != Category.None && (fixtureA.CollisionCategories & fixtureB.CollidesWith) != Category.None;
      return (!flag || !fixtureA.IsFixtureIgnored(fixtureB) && !fixtureB.IsFixtureIgnored(fixtureA)) && flag;
    }
  }
}
