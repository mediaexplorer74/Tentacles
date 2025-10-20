// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.WeldJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class WeldJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private Vector3 _impulse;
    private Mat33 _mass;

    internal WeldJoint() => this.JointType = JointType.Weld;

    public WeldJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Weld;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this.ReferenceAngle = this.BodyB.Rotation - this.BodyA.Rotation;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public float ReferenceAngle { get; private set; }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      return inv_dt * new Vector2(this._impulse.X, this._impulse.Y);
    }

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._impulse.Z;

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
      this._mass.Col1.X = (float) ((double) invMass1 + (double) invMass2 + (double) a1.Y * (double) a1.Y * (double) invI1 + (double) a2.Y * (double) a2.Y * (double) invI2);
      this._mass.Col2.X = (float) (-(double) a1.Y * (double) a1.X * (double) invI1 - (double) a2.Y * (double) a2.X * (double) invI2);
      this._mass.Col3.X = (float) (-(double) a1.Y * (double) invI1 - (double) a2.Y * (double) invI2);
      this._mass.Col1.Y = this._mass.Col2.X;
      this._mass.Col2.Y = (float) ((double) invMass1 + (double) invMass2 + (double) a1.X * (double) a1.X * (double) invI1 + (double) a2.X * (double) a2.X * (double) invI2);
      this._mass.Col3.Y = (float) ((double) a1.X * (double) invI1 + (double) a2.X * (double) invI2);
      this._mass.Col1.Z = this._mass.Col3.X;
      this._mass.Col2.Z = this._mass.Col3.Y;
      this._mass.Col3.Z = invI1 + invI2;
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        Vector2 b = new Vector2(this._impulse.X, this._impulse.Y);
        bodyA.LinearVelocityInternal -= invMass1 * b;
        bodyA.AngularVelocityInternal -= invI1 * (MathUtils.Cross(a1, b) + this._impulse.Z);
        bodyB.LinearVelocityInternal += invMass2 * b;
        bodyB.AngularVelocityInternal += invI2 * (MathUtils.Cross(a2, b) + this._impulse.Z);
      }
      else
        this._impulse = Vector3.Zero;
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
      Vector2 vector2_1 = velocityInternal3 + MathUtils.Cross(velocityInternal4, a2) - velocityInternal1 - MathUtils.Cross(velocityInternal2, a1);
      float z = velocityInternal4 - velocityInternal2;
      Vector3 vector3 = this._mass.Solve33(-new Vector3(vector2_1.X, vector2_1.Y, z));
      this._impulse += vector3;
      Vector2 b = new Vector2(vector3.X, vector3.Y);
      Vector2 vector2_2 = velocityInternal1 - invMass1 * b;
      float num1 = velocityInternal2 - invI1 * (MathUtils.Cross(a1, b) + vector3.Z);
      Vector2 vector2_3 = velocityInternal3 + invMass2 * b;
      float num2 = velocityInternal4 + invI2 * (MathUtils.Cross(a2, b) + vector3.Z);
      bodyA.LinearVelocityInternal = vector2_2;
      bodyA.AngularVelocityInternal = num1;
      bodyB.LinearVelocityInternal = vector2_3;
      bodyB.AngularVelocityInternal = num2;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
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
      Vector2 vector2 = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float z = bodyB.Sweep.A - bodyA.Sweep.A - this.ReferenceAngle;
      float num1 = vector2.Length();
      float num2 = Math.Abs(z);
      if ((double) num1 > 0.049999997019767761)
      {
        invI1 *= 1f;
        invI2 *= 1f;
      }
      this._mass.Col1.X = (float) ((double) invMass1 + (double) invMass2 + (double) a1.Y * (double) a1.Y * (double) invI1 + (double) a2.Y * (double) a2.Y * (double) invI2);
      this._mass.Col2.X = (float) (-(double) a1.Y * (double) a1.X * (double) invI1 - (double) a2.Y * (double) a2.X * (double) invI2);
      this._mass.Col3.X = (float) (-(double) a1.Y * (double) invI1 - (double) a2.Y * (double) invI2);
      this._mass.Col1.Y = this._mass.Col2.X;
      this._mass.Col2.Y = (float) ((double) invMass1 + (double) invMass2 + (double) a1.X * (double) a1.X * (double) invI1 + (double) a2.X * (double) a2.X * (double) invI2);
      this._mass.Col3.Y = (float) ((double) a1.X * (double) invI1 + (double) a2.X * (double) invI2);
      this._mass.Col1.Z = this._mass.Col3.X;
      this._mass.Col2.Z = this._mass.Col3.Y;
      this._mass.Col3.Z = invI1 + invI2;
      Vector3 vector3 = this._mass.Solve33(-new Vector3(vector2.X, vector2.Y, z));
      Vector2 b = new Vector2(vector3.X, vector3.Y);
      bodyA.Sweep.C -= invMass1 * b;
      bodyA.Sweep.A -= invI1 * (MathUtils.Cross(a1, b) + vector3.Z);
      bodyB.Sweep.C += invMass2 * b;
      bodyB.Sweep.A += invI2 * (MathUtils.Cross(a2, b) + vector3.Z);
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) num1 <= 0.004999999888241291 && (double) num2 <= Math.PI / 90.0;
    }
  }
}
