// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.BuoyancyController
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public sealed class BuoyancyController : Controller
  {
    private Vector2 _gravity;
    private Vector2 _normal;
    private float _offset;
    private AABB _container;
    public float Density;
    public float LinearDragCoefficient;
    public float AngularDragCoefficient;
    public Vector2 Velocity;
    private Dictionary<int, Body> _uniqueBodies = new Dictionary<int, Body>();

    public BuoyancyController(
      AABB container,
      float density,
      float linearDragCoefficient,
      float rotationalDragCoefficient,
      Vector2 gravity)
      : base(ControllerType.BuoyancyController)
    {
      this.Container = container;
      this._normal = new Vector2(0.0f, 1f);
      this.Density = density;
      this.LinearDragCoefficient = linearDragCoefficient;
      this.AngularDragCoefficient = rotationalDragCoefficient;
      this._gravity = gravity;
    }

    public AABB Container
    {
      get => this._container;
      set
      {
        this._container = value;
        this._offset = this._container.UpperBound.Y;
      }
    }

    public override void Update(float dt)
    {
      this._uniqueBodies.Clear();
      this.World.QueryAABB((Func<Fixture, bool>) (fixture =>
      {
        if (fixture.Body.IsStatic || !fixture.Body.Awake || this._uniqueBodies.ContainsKey(fixture.Body.BodyId))
          return true;
        this._uniqueBodies.Add(fixture.Body.BodyId, fixture.Body);
        return true;
      }), ref this._container);
      foreach (KeyValuePair<int, Body> uniqueBody in this._uniqueBodies)
      {
        Body body = uniqueBody.Value;
        Vector2 zero1 = Vector2.Zero;
        Vector2 zero2 = Vector2.Zero;
        float num1 = 0.0f;
        float num2 = 0.0f;
        for (int index = 0; index < body.FixtureList.Count; ++index)
        {
          Fixture fixture = body.FixtureList[index];
          if (fixture.Shape.ShapeType == ShapeType.Polygon || fixture.Shape.ShapeType == ShapeType.Circle)
          {
            Shape shape = fixture.Shape;
            Vector2 sc;
            float submergedArea = shape.ComputeSubmergedArea(this._normal, this._offset, body.Xf, out sc);
            num1 += submergedArea;
            zero1.X += submergedArea * sc.X;
            zero1.Y += submergedArea * sc.Y;
            num2 += submergedArea * shape.Density;
            zero2.X += submergedArea * sc.X * shape.Density;
            zero2.Y += submergedArea * sc.Y * shape.Density;
          }
        }
        zero1.X /= num1;
        zero1.Y /= num1;
        zero2.X /= num2;
        zero2.Y /= num2;
        if ((double) num1 >= 1.1920928955078125E-07)
        {
          Vector2 force1 = -this.Density * num1 * this._gravity;
          body.ApplyForce(force1, zero2);
          Vector2 force2 = (body.GetLinearVelocityFromWorldPoint(zero1) - this.Velocity) * (-this.LinearDragCoefficient * num1);
          body.ApplyForce(force2, zero1);
          body.ApplyTorque(-body.Inertia / body.Mass * num1 * body.AngularVelocity * this.AngularDragCoefficient);
        }
      }
    }
  }
}
