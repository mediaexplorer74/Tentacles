// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedFrictionJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedFrictionJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public float MaxForce;
    public float MaxTorque;
    private float _angularImpulse;
    private float _angularMass;
    private Vector2 _linearImpulse;
    private Mat22 _linearMass;

    public FixedFrictionJoint(Body body, Vector2 localAnchorA)
      : base(body)
    {
      this.JointType = JointType.FixedFriction;
      this.LocalAnchorA = localAnchorA;
      float num = (float) Math.Sqrt(2.0 * ((double) body.Inertia / (double) body.Mass));
      this.MaxForce = body.Mass * 10f;
      this.MaxTorque = (float) ((double) body.Mass * (double) num * 10.0);
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => Vector2.Zero;
      set
      {
      }
    }

    public override Vector2 GetReactionForce(float invDT) => invDT * this._linearImpulse;

    public override float GetReactionTorque(float invDT) => invDT * this._angularImpulse;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      Mat22 A = new Mat22()
      {
        Col1 = {
          X = invMass
        },
        Col2 = {
          X = 0.0f
        }
      };
      A.Col1.Y = 0.0f;
      A.Col2.Y = invMass;
      Mat22 B = new Mat22()
      {
        Col1 = {
          X = invI * a.Y * a.Y
        },
        Col2 = {
          X = -invI * a.X * a.Y
        }
      };
      B.Col1.Y = -invI * a.X * a.Y;
      B.Col2.Y = invI * a.X * a.X;
      Mat22 R;
      Mat22.Add(ref A, ref B, out R);
      this._linearMass = R.Inverse;
      this._angularMass = invI;
      if ((double) this._angularMass > 0.0)
        this._angularMass = 1f / this._angularMass;
      if (Settings.EnableWarmstarting)
      {
        this._linearImpulse *= step.dtRatio;
        this._angularImpulse *= step.dtRatio;
        Vector2 b = new Vector2(this._linearImpulse.X, this._linearImpulse.Y);
        bodyA.LinearVelocityInternal -= invMass * b;
        bodyA.AngularVelocityInternal -= invI * (MathUtils.Cross(a, b) + this._angularImpulse);
      }
      else
      {
        this._linearImpulse = Vector2.Zero;
        this._angularImpulse = 0.0f;
      }
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Vector2 velocityInternal1 = bodyA.LinearVelocityInternal;
      float velocityInternal2 = bodyA.AngularVelocityInternal;
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      float num1 = -this._angularMass * -velocityInternal2;
      float angularImpulse = this._angularImpulse;
      float high = step.dt * this.MaxTorque;
      this._angularImpulse = MathUtils.Clamp(this._angularImpulse + num1, -high, high);
      float num2 = this._angularImpulse - angularImpulse;
      float s = velocityInternal2 - invI * num2;
      Vector2 vector2_1 = -MathUtils.Multiply(ref this._linearMass, -velocityInternal1 - MathUtils.Cross(s, a));
      Vector2 linearImpulse = this._linearImpulse;
      this._linearImpulse += vector2_1;
      float num3 = step.dt * this.MaxForce;
      if ((double) this._linearImpulse.LengthSquared() > (double) num3 * (double) num3)
      {
        this._linearImpulse.Normalize();
        this._linearImpulse *= num3;
      }
      Vector2 b = this._linearImpulse - linearImpulse;
      Vector2 vector2_2 = velocityInternal1 - invMass * b;
      float num4 = s - invI * MathUtils.Cross(a, b);
      bodyA.LinearVelocityInternal = vector2_2;
      bodyA.AngularVelocityInternal = num4;
    }

    internal override bool SolvePositionConstraints() => true;
  }
}
