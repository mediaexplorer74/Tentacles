// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.BreakableBody
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class BreakableBody
  {
    public bool Broken;
    public Body MainBody;
    public List<Fixture> Parts = new List<Fixture>(8);
    public float Strength = 500f;
    private float[] _angularVelocitiesCache = new float[8];
    private bool _break;
    private Vector2[] _velocitiesCache = new Vector2[8];
    private World _world;

    public BreakableBody(IEnumerable<Vertices> vertices, World world, float density)
      : this(vertices, world, density, (object) null)
    {
    }

    public BreakableBody(
      IEnumerable<Vertices> vertices,
      World world,
      float density,
      object userData)
    {
      this._world = world;
      this._world.ContactManager.PostSolve += new PostSolveDelegate(this.PostSolve);
      this.MainBody = new Body(this._world);
      this.MainBody.BodyType = BodyType.Dynamic;
      foreach (Vertices vertex in vertices)
        this.Parts.Add(this.MainBody.CreateFixture((Shape) new PolygonShape(vertex, density), userData));
    }

    private void PostSolve(Contact contact, ContactConstraint impulse)
    {
      if (this.Broken || !this.Parts.Contains(contact.FixtureA) && !this.Parts.Contains(contact.FixtureB))
        return;
      float val1 = 0.0f;
      int pointCount = contact.Manifold.PointCount;
      for (int index = 0; index < pointCount; ++index)
        val1 = Math.Max(val1, impulse.Points[index].NormalImpulse);
      if ((double) val1 <= (double) this.Strength)
        return;
      this._break = true;
    }

    public void Update()
    {
      if (this._break)
      {
        this.Decompose();
        this.Broken = true;
        this._break = false;
      }
      if (this.Broken)
        return;
      if (this.Parts.Count > this._angularVelocitiesCache.Length)
      {
        this._velocitiesCache = new Vector2[this.Parts.Count];
        this._angularVelocitiesCache = new float[this.Parts.Count];
      }
      for (int index = 0; index < this.Parts.Count; ++index)
      {
        this._velocitiesCache[index] = this.Parts[index].Body.LinearVelocity;
        this._angularVelocitiesCache[index] = this.Parts[index].Body.AngularVelocity;
      }
    }

    private void Decompose()
    {
      this._world.ContactManager.PostSolve -= new PostSolveDelegate(this.PostSolve);
      for (int index = 0; index < this.Parts.Count; ++index)
      {
        Fixture part = this.Parts[index];
        Shape shape = part.Shape.Clone();
        object userData = part.UserData;
        this.MainBody.DestroyFixture(part);
        Body body = BodyFactory.CreateBody(this._world);
        body.BodyType = BodyType.Dynamic;
        body.Position = this.MainBody.Position;
        body.Rotation = this.MainBody.Rotation;
        body.UserData = this.MainBody.UserData;
        body.CreateFixture(shape, userData);
        body.AngularVelocity = this._angularVelocitiesCache[index];
        body.LinearVelocity = this._velocitiesCache[index];
      }
      this._world.RemoveBody(this.MainBody);
      this._world.RemoveBreakableBody(this);
    }

    public void Break() => this._break = true;
  }
}
