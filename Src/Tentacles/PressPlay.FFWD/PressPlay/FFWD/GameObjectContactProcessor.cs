// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.GameObjectContactProcessor
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.Interfaces;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  internal class GameObjectContactProcessor : IContactProcessor
  {
    private readonly List<Contact> beginContacts = new List<Contact>(50);
    private readonly List<Contact> endContacts = new List<Contact>(50);
    private readonly List<GameObjectContactProcessor.Stay> staying = new List<GameObjectContactProcessor.Stay>(50);
    private BitArray staysToRemove = new BitArray(64);

    public bool BeginContact(Contact contact)
    {
      this.beginContacts.Add(contact);
      return true;
    }

    public void EndContact(Contact contact) => this.endContacts.Add(contact);

    public void Update()
    {
      for (int index1 = 0; index1 < this.endContacts.Count; ++index1)
      {
        Contact endContact = this.endContacts[index1];
        Fixture fixtureA = endContact.FixtureA;
        Fixture fixtureB = endContact.FixtureB;
        if (fixtureA != null && fixtureB != null && (fixtureA.Body.BodyType != BodyType.Static || fixtureB.Body.BodyType != BodyType.Static))
        {
          Component userData1 = fixtureA.Body.UserData;
          Component userData2 = fixtureB.Body.UserData;
          if (userData1 != null && userData2 != null)
          {
            if (fixtureA.IsSensor || fixtureB.IsSensor)
            {
              this.RemoveStay(userData1.collider, userData2.collider);
              userData1.gameObject.OnTriggerExit(userData2.collider);
              userData2.gameObject.OnTriggerExit(userData1.collider);
            }
            else if ((userData1.rigidbody != null || userData2.rigidbody != null) && (userData1.rigidbody == null || userData2.rigidbody == null || !userData1.rigidbody.isKinematic || !userData2.rigidbody.isKinematic))
            {
              this.RemoveStay(userData1.collider, userData2.collider);
              Microsoft.Xna.Framework.Vector2 normal;
              FixedArray2<Microsoft.Xna.Framework.Vector2> points;
              endContact.GetWorldManifold(out normal, out points);
              Collision collision = new Collision()
              {
                collider = userData2.collider,
                relativeVelocity = (userData1.rigidbody != null ? userData1.rigidbody.velocity : Vector3.zero) - (userData2.rigidbody != null ? userData2.rigidbody.velocity : Vector3.zero),
                contacts = new ContactPoint[2]
              };
              for (int index2 = 0; index2 < 2; ++index2)
              {
                collision.contacts[index2].thisCollider = userData1.collider;
                collision.contacts[index2].otherCollider = userData2.collider;
                collision.contacts[index2].point = (Vector3) points[index2];
                collision.contacts[index2].normal = (Vector3) normal;
              }
              userData1.gameObject.OnCollisionExit(collision);
              collision.SetColliders(userData2.collider, userData1.collider);
              userData2.gameObject.OnCollisionExit(collision);
            }
          }
        }
      }
      for (int index = this.staying.Count - 1; index >= 0; --index)
      {
        if (this.staysToRemove[index] || this.staying[index].colliderToTriggerA.gameObject == null || this.staying[index].colliderToTriggerB.gameObject == null || !this.staying[index].colliderToTriggerA.gameObject.active || !this.staying[index].colliderToTriggerB.gameObject.active)
        {
          this.staysToRemove[index] = false;
          this.staying.RemoveAt(index);
        }
        else if (!this.staying[index].collision)
        {
          this.staying[index].gameObjectA.OnTriggerStay(this.staying[index].colliderToTriggerA);
          this.staying[index].gameObjectB.OnTriggerStay(this.staying[index].colliderToTriggerB);
        }
        else
        {
          this.staying[index].gameObjectA.OnCollisionStay(this.staying[index].collisionBToA);
          this.staying[index].gameObjectB.OnCollisionStay(this.staying[index].collisionAToB);
        }
      }
      for (int index3 = 0; index3 < this.beginContacts.Count; ++index3)
      {
        Contact beginContact = this.beginContacts[index3];
        Fixture fixtureA = beginContact.FixtureA;
        Fixture fixtureB = beginContact.FixtureB;
        if (fixtureA != null && fixtureB != null && (fixtureA.Body.BodyType != BodyType.Static || fixtureB.Body.BodyType != BodyType.Static))
        {
          Component userData3 = fixtureA.Body.UserData;
          Component userData4 = fixtureB.Body.UserData;
          if (userData3 != null && userData4 != null)
          {
            if (fixtureA.IsSensor || fixtureB.IsSensor)
            {
              this.staying.Add(new GameObjectContactProcessor.Stay(userData4.collider, userData3.collider, userData3.gameObject, userData4.gameObject));
              userData3.gameObject.OnTriggerEnter(userData4.collider);
              userData4.gameObject.OnTriggerEnter(userData3.collider);
            }
            else if ((userData3.rigidbody != null || userData4.rigidbody != null) && (userData3.rigidbody == null || userData4.rigidbody == null || !userData3.rigidbody.isKinematic || !userData4.rigidbody.isKinematic))
            {
              Microsoft.Xna.Framework.Vector2 normal;
              FixedArray2<Microsoft.Xna.Framework.Vector2> points;
              beginContact.GetWorldManifold(out normal, out points);
              Collision collisionBToA = new Collision()
              {
                collider = userData4.collider,
                relativeVelocity = (userData3.rigidbody != null ? userData3.rigidbody.velocity : Vector3.zero) - (userData4.rigidbody != null ? userData4.rigidbody.velocity : Vector3.zero),
                contacts = new ContactPoint[beginContact.Manifold.PointCount]
              };
              Collision collisionAToB = new Collision()
              {
                collider = userData3.collider,
                relativeVelocity = (userData4.rigidbody != null ? userData4.rigidbody.velocity : Vector3.zero) - (userData3.rigidbody != null ? userData3.rigidbody.velocity : Vector3.zero),
                contacts = new ContactPoint[beginContact.Manifold.PointCount]
              };
              for (int index4 = 0; index4 < collisionBToA.contacts.Length; ++index4)
              {
                collisionBToA.contacts[index4].thisCollider = userData4.collider;
                collisionBToA.contacts[index4].otherCollider = userData3.collider;
                collisionBToA.contacts[index4].point = (Vector3) points[index4];
                collisionBToA.contacts[index4].normal = (Vector3) -normal;
                collisionAToB.contacts[index4].thisCollider = userData3.collider;
                collisionAToB.contacts[index4].otherCollider = userData4.collider;
                collisionAToB.contacts[index4].point = (Vector3) points[index4];
                collisionAToB.contacts[index4].normal = (Vector3) normal;
              }
              GameObjectContactProcessor.Stay stay = new GameObjectContactProcessor.Stay(collisionAToB, collisionBToA, userData3.gameObject, userData4.gameObject);
              this.staying.Add(stay);
              stay.gameObjectA.OnCollisionEnter(stay.collisionBToA);
              stay.gameObjectB.OnCollisionEnter(stay.collisionAToB);
            }
          }
        }
      }
      this.beginContacts.Clear();
      this.endContacts.Clear();
    }

    internal void RemoveStay(Collider compA, Collider compB)
    {
      for (int index = this.staying.Count - 1; index >= 0; --index)
      {
        if (this.staying[index].colliderToTriggerA == compA && this.staying[index].colliderToTriggerB == compB || this.staying[index].colliderToTriggerA == compB && this.staying[index].colliderToTriggerB == compA)
          this.staysToRemove[index] = true;
      }
    }

    public void ResetStays(Collider collider)
    {
      for (int index = this.staying.Count - 1; index >= 0; --index)
      {
        if (this.staying[index].colliderToTriggerA == collider || this.staying[index].colliderToTriggerB == collider)
          this.staysToRemove[index] = true;
      }
    }

    private struct Stay
    {
      public Collider colliderToTriggerA;
      public Collider colliderToTriggerB;
      public Collision collisionAToB;
      public Collision collisionBToA;
      public GameObject gameObjectA;
      public GameObject gameObjectB;

      public bool collision => this.collisionAToB != null;

      public Stay(
        Collider colliderToTriggerA,
        Collider colliderToTriggerB,
        GameObject gameObjectA,
        GameObject gameObjectB)
      {
        this.colliderToTriggerA = colliderToTriggerA;
        this.colliderToTriggerB = colliderToTriggerB;
        this.gameObjectA = gameObjectA;
        this.gameObjectB = gameObjectB;
        this.collisionAToB = (Collision) null;
        this.collisionBToA = (Collision) null;
      }

      public Stay(
        Collision collisionAToB,
        Collision collisionBToA,
        GameObject gameObjectA,
        GameObject gameObjectB)
      {
        this.collisionAToB = collisionAToB;
        this.collisionBToA = collisionBToA;
        this.colliderToTriggerA = collisionAToB.collider;
        this.colliderToTriggerB = collisionBToA.collider;
        this.gameObjectA = gameObjectA;
        this.gameObjectB = gameObjectB;
      }
    }
  }
}
