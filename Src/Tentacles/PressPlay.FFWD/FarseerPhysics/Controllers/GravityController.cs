// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.GravityController
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public class GravityController : Controller
  {
    public List<Body> Bodies = new List<Body>();
    public List<Vector2> Points = new List<Vector2>();

    public GravityController(float strength)
      : base(ControllerType.GravityController)
    {
      this.Strength = strength;
      this.MaxRadius = float.MaxValue;
    }

    public GravityController(float strength, float maxRadius, float minRadius)
      : base(ControllerType.GravityController)
    {
      this.MinRadius = minRadius;
      this.MaxRadius = maxRadius;
      this.Strength = strength;
    }

    public float MinRadius { get; set; }

    public float MaxRadius { get; set; }

    public float Strength { get; set; }

    public GravityType GravityType { get; set; }

    public override void Update(float dt)
    {
      Vector2 result = Vector2.Zero;
      foreach (Body body1 in this.World.BodyList)
      {
        if (this.IsActiveOn(body1))
        {
          foreach (Body body2 in this.Bodies)
          {
            if (body1 != body2 && (!body1.IsStatic || !body2.IsStatic) && body2.Enabled)
            {
              Vector2 vector2 = body2.Position - body1.Position;
              float d = vector2.LengthSquared();
              if ((double) d >= 1.1920928955078125E-07)
              {
                float num = vector2.Length();
                if ((double) num < (double) this.MaxRadius && (double) num > (double) this.MinRadius)
                {
                  switch (this.GravityType)
                  {
                    case GravityType.Linear:
                      result = this.Strength / d * body1.Mass * body2.Mass * vector2;
                      break;
                    case GravityType.DistanceSquared:
                      result = (float) ((double) this.Strength / (double) d / Math.Sqrt((double) d)) * body1.Mass * body2.Mass * vector2;
                      break;
                  }
                  body1.ApplyForce(ref result);
                  Vector2.Negate(ref result, out result);
                  body2.ApplyForce(ref result);
                }
              }
            }
          }
          foreach (Vector2 point in this.Points)
          {
            Vector2 vector2 = point - body1.Position;
            float d = vector2.LengthSquared();
            if ((double) d >= 1.1920928955078125E-07)
            {
              float num = vector2.Length();
              if ((double) num < (double) this.MaxRadius && (double) num > (double) this.MinRadius)
              {
                switch (this.GravityType)
                {
                  case GravityType.Linear:
                    result = this.Strength / d * body1.Mass * vector2;
                    break;
                  case GravityType.DistanceSquared:
                    result = this.Strength / d / (float) Math.Sqrt((double) d) * body1.Mass * vector2;
                    break;
                }
                body1.ApplyForce(ref result);
              }
            }
          }
        }
      }
    }

    public void AddBody(Body body) => this.Bodies.Add(body);

    public void AddPoint(Vector2 point) => this.Points.Add(point);
  }
}
