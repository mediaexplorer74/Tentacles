// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.VelocityLimitController
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public class VelocityLimitController : Controller
  {
    public bool LimitAngularVelocity = true;
    public bool LimitLinearVelocity = true;
    private List<Body> _bodies = new List<Body>();
    private float _maxAngularSqared;
    private float _maxAngularVelocity;
    private float _maxLinearSqared;
    private float _maxLinearVelocity;

    public VelocityLimitController()
      : base(ControllerType.VelocityLimitController)
    {
      this._maxLinearVelocity = 2f;
      this._maxAngularVelocity = 1.57079637f;
    }

    public VelocityLimitController(float maxLinearVelocity, float maxAngularVelocity)
      : base(ControllerType.VelocityLimitController)
    {
      if ((double) maxLinearVelocity == 0.0 || (double) maxLinearVelocity == 3.4028234663852886E+38)
        this.LimitLinearVelocity = false;
      if ((double) maxAngularVelocity == 0.0 || (double) maxAngularVelocity == 3.4028234663852886E+38)
        this.LimitAngularVelocity = false;
      this._maxLinearVelocity = maxLinearVelocity;
      this._maxAngularVelocity = maxAngularVelocity;
    }

    public float MaxAngularVelocity
    {
      get => this._maxAngularVelocity;
      set
      {
        this._maxAngularVelocity = value;
        this._maxAngularSqared = this._maxAngularVelocity * this._maxAngularVelocity;
      }
    }

    public float MaxLinearVelocity
    {
      get => this._maxLinearVelocity;
      set
      {
        this._maxLinearVelocity = value;
        this._maxLinearSqared = this._maxLinearVelocity * this._maxLinearVelocity;
      }
    }

    public override void Update(float dt)
    {
      foreach (Body body in this._bodies)
      {
        if (this.IsActiveOn(body))
        {
          if (this.LimitLinearVelocity)
          {
            float num1 = dt * body.LinearVelocityInternal.X;
            float num2 = dt * body.LinearVelocityInternal.Y;
            float d = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
            if ((double) d > (double) this._maxLinearSqared)
            {
              float num3 = this._maxLinearVelocity / (float) Math.Sqrt((double) d);
              body.LinearVelocityInternal.X *= num3;
              body.LinearVelocityInternal.Y *= num3;
            }
          }
          if (this.LimitAngularVelocity)
          {
            float num4 = dt * body.AngularVelocityInternal;
            if ((double) num4 * (double) num4 > (double) this._maxAngularSqared)
            {
              float num5 = this._maxAngularVelocity / Math.Abs(num4);
              body.AngularVelocityInternal *= num5;
            }
          }
        }
      }
    }

    public void AddBody(Body body) => this._bodies.Add(body);
  }
}
