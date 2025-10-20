// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FrictionJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FrictionJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private float _angularImpulse;
    private float _angularMass;
    private Vector2 _linearImpulse;
    private Mat22 _linearMass;

    internal FrictionJoint() => this.JointType = JointType.Friction;

    public FrictionJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Friction;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public float MaxForce { get; set; }

    public float MaxTorque { get; set; }

    public override Vector2 GetReactionForce(float inv_dt) => inv_dt * this._linearImpulse;

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._angularImpulse;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      float invMass1 = bodyA.InvMass;
      float invMass2 = bodyB.InvMass;
      float invI1 = bodyA.InvI;
      float invI2 = bodyB.InvI;
      Mat22 A = new Mat22()
      {
        Col1 = {
          X = invMass1 + invMass2
        },
        Col2 = {
          X = 0.0f
        }
      };
      A.Col1.Y = 0.0f;
      A.Col2.Y = invMass1 + invMass2;
      Mat22 B1 = new Mat22()
      {
        Col1 = {
          X = invI1 * a1.Y * a1.Y
        },
        Col2 = {
          X = -invI1 * a1.X * a1.Y
        }
      };
      B1.Col1.Y = -invI1 * a1.X * a1.Y;
      B1.Col2.Y = invI1 * a1.X * a1.X;
      Mat22 B2 = new Mat22()
      {
        Col1 = {
          X = invI2 * a2.Y * a2.Y
        },
        Col2 = {
          X = -invI2 * a2.X * a2.Y
        }
      };
      B2.Col1.Y = -invI2 * a2.X * a2.Y;
      B2.Col2.Y = invI2 * a2.X * a2.X;
      Mat22 R1;
      Mat22.Add(ref A, ref B1, out R1);
      Mat22 R2;
      Mat22.Add(ref R1, ref B2, out R2);
      this._linearMass = R2.Inverse;
      this._angularMass = invI1 + invI2;
      if ((double) this._angularMass > 0.0)
        this._angularMass = 1f / this._angularMass;
      if (Settings.EnableWarmstarting)
      {
        this._linearImpulse *= step.dtRatio;
        this._angularImpulse *= step.dtRatio;
        Vector2 b = new Vector2(this._linearImpulse.X, this._linearImpulse.Y);
        bodyA.LinearVelocityInternal -= invMass1 * b;
        bodyA.AngularVelocityInternal -= invI1 * (MathUtils.Cross(a1, b) + this._angularImpulse);
        bodyB.LinearVelocityInternal += invMass2 * b;
        bodyB.AngularVelocityInternal += invI2 * (MathUtils.Cross(a2, b) + this._angularImpulse);
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
      Body bodyB = this.BodyB;
      Vector2 velocityInternal1 = bodyA.LinearVelocityInternal;
      float velocityInternal2 = bodyA.AngularVelocityInternal;
      Vector2 velocityInternal3 = bodyB.LinearVelocityInternal;
      float velocityInternal4 = bodyB.AngularVelocityInternal;
      float invMass1 = bodyA.InvMass;
      float invMass2 = bodyB.InvMass;
      float invI1 = bodyA.InvI;
      float invI2 = bodyB.InvI;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      float num1 = -this._angularMass * (velocityInternal4 - velocityInternal2);
      float angularImpulse = this._angularImpulse;
      float high = step.dt * this.MaxTorque;
      this._angularImpulse = MathUtils.Clamp(this._angularImpulse + num1, -high, high);
      float num2 = this._angularImpulse - angularImpulse;
      float s1 = velocityInternal2 - invI1 * num2;
      float s2 = velocityInternal4 + invI2 * num2;
      Vector2 vector2_1 = -MathUtils.Multiply(ref this._linearMass, velocityInternal3 + MathUtils.Cross(s2, a2) - velocityInternal1 - MathUtils.Cross(s1, a1));
      Vector2 linearImpulse = this._linearImpulse;
      this._linearImpulse += vector2_1;
      float num3 = step.dt * this.MaxForce;
      if ((double) this._linearImpulse.LengthSquared() > (double) num3 * (double) num3)
      {
        this._linearImpulse.Normalize();
        this._linearImpulse *= num3;
      }
      Vector2 b = this._linearImpulse - linearImpulse;
      Vector2 vector2_2 = velocityInternal1 - invMass1 * b;
      float num4 = s1 - invI1 * MathUtils.Cross(a1, b);
      Vector2 vector2_3 = velocityInternal3 + invMass2 * b;
      float num5 = s2 + invI2 * MathUtils.Cross(a2, b);
      bodyA.LinearVelocityInternal = vector2_2;
      bodyA.AngularVelocityInternal = num4;
      bodyB.LinearVelocityInternal = vector2_3;
      bodyB.AngularVelocityInternal = num5;
    }

    internal override bool SolvePositionConstraints() => true;
  }
}
