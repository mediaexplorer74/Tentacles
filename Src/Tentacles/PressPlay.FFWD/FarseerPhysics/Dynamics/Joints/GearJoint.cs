// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.GearJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class GearJoint : Joint
  {
    private Jacobian _J;
    private float _ant;
    private FixedPrismaticJoint _fixedPrismatic1;
    private FixedPrismaticJoint _fixedPrismatic2;
    private FixedRevoluteJoint _fixedRevolute1;
    private FixedRevoluteJoint _fixedRevolute2;
    private float _impulse;
    private float _mass;
    private PrismaticJoint _prismatic1;
    private PrismaticJoint _prismatic2;
    private RevoluteJoint _revolute1;
    private RevoluteJoint _revolute2;

    public GearJoint(Joint jointA, Joint jointB, float ratio)
      : base(jointA.BodyA, jointA.BodyB)
    {
      this.JointType = JointType.Gear;
      this.JointA = jointA;
      this.JointB = jointB;
      this.Ratio = ratio;
      JointType jointType1 = jointA.JointType;
      JointType jointType2 = jointB.JointType;
      if (jointType1 != JointType.Revolute)
        ;
      if (jointType2 != JointType.Revolute)
        ;
      float num1 = 0.0f;
      float num2 = 0.0f;
      switch (jointType1)
      {
        case JointType.Revolute:
          this.BodyA = jointA.BodyB;
          this._revolute1 = (RevoluteJoint) jointA;
          this.LocalAnchor1 = this._revolute1.LocalAnchorB;
          num1 = this._revolute1.JointAngle;
          break;
        case JointType.Prismatic:
          this.BodyA = jointA.BodyB;
          this._prismatic1 = (PrismaticJoint) jointA;
          this.LocalAnchor1 = this._prismatic1.LocalAnchorB;
          num1 = this._prismatic1.JointTranslation;
          break;
        case JointType.FixedRevolute:
          this.BodyA = jointA.BodyA;
          this._fixedRevolute1 = (FixedRevoluteJoint) jointA;
          this.LocalAnchor1 = this._fixedRevolute1.LocalAnchorA;
          num1 = this._fixedRevolute1.JointAngle;
          break;
        case JointType.FixedPrismatic:
          this.BodyA = jointA.BodyA;
          this._fixedPrismatic1 = (FixedPrismaticJoint) jointA;
          this.LocalAnchor1 = this._fixedPrismatic1.LocalAnchorA;
          num1 = this._fixedPrismatic1.JointTranslation;
          break;
      }
      switch (jointType2)
      {
        case JointType.Revolute:
          this.BodyB = jointB.BodyB;
          this._revolute2 = (RevoluteJoint) jointB;
          this.LocalAnchor2 = this._revolute2.LocalAnchorB;
          num2 = this._revolute2.JointAngle;
          break;
        case JointType.Prismatic:
          this.BodyB = jointB.BodyB;
          this._prismatic2 = (PrismaticJoint) jointB;
          this.LocalAnchor2 = this._prismatic2.LocalAnchorB;
          num2 = this._prismatic2.JointTranslation;
          break;
        case JointType.FixedRevolute:
          this.BodyB = jointB.BodyA;
          this._fixedRevolute2 = (FixedRevoluteJoint) jointB;
          this.LocalAnchor2 = this._fixedRevolute2.LocalAnchorA;
          num2 = this._fixedRevolute2.JointAngle;
          break;
        case JointType.FixedPrismatic:
          this.BodyB = jointB.BodyA;
          this._fixedPrismatic2 = (FixedPrismaticJoint) jointB;
          this.LocalAnchor2 = this._fixedPrismatic2.LocalAnchorA;
          num2 = this._fixedPrismatic2.JointTranslation;
          break;
      }
      this._ant = num1 + this.Ratio * num2;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchor1);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchor2);
      set
      {
      }
    }

    public float Ratio { get; set; }

    public Joint JointA { get; set; }

    public Joint JointB { get; set; }

    public Vector2 LocalAnchor1 { get; private set; }

    public Vector2 LocalAnchor2 { get; private set; }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      Vector2 vector2 = this._impulse * this._J.LinearB;
      return inv_dt * vector2;
    }

    public override float GetReactionTorque(float inv_dt)
    {
      Transform transform;
      this.BodyB.GetTransform(out transform);
      float num = this._impulse * this._J.AngularB - MathUtils.Cross(MathUtils.Multiply(ref transform.R, this.LocalAnchor2 - this.BodyB.LocalCenter), this._impulse * this._J.LinearB);
      return inv_dt * num;
    }

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      float num1 = 0.0f;
      this._J.SetZero();
      float num2;
      if (this._revolute1 != null || this._fixedRevolute1 != null)
      {
        this._J.AngularA = -1f;
        num2 = num1 + bodyA.InvI;
      }
      else
      {
        Vector2 b = this._prismatic1 == null ? this._fixedPrismatic1.LocalXAxis1 : this._prismatic1.LocalXAxis1;
        Transform transform;
        bodyA.GetTransform(out transform);
        float num3 = MathUtils.Cross(MathUtils.Multiply(ref transform.R, this.LocalAnchor1 - bodyA.LocalCenter), b);
        this._J.LinearA = -b;
        this._J.AngularA = -num3;
        num2 = num1 + (bodyA.InvMass + bodyA.InvI * num3 * num3);
      }
      float num4;
      if (this._revolute2 != null || this._fixedRevolute2 != null)
      {
        this._J.AngularB = -this.Ratio;
        num4 = num2 + this.Ratio * this.Ratio * bodyB.InvI;
      }
      else
      {
        Vector2 b = this._prismatic2 == null ? this._fixedPrismatic2.LocalXAxis1 : this._prismatic2.LocalXAxis1;
        Transform transform;
        bodyB.GetTransform(out transform);
        float num5 = MathUtils.Cross(MathUtils.Multiply(ref transform.R, this.LocalAnchor2 - bodyB.LocalCenter), b);
        this._J.LinearB = -this.Ratio * b;
        this._J.AngularB = -this.Ratio * num5;
        num4 = num2 + (float) ((double) this.Ratio * (double) this.Ratio * ((double) bodyB.InvMass + (double) bodyB.InvI * (double) num5 * (double) num5));
      }
      this._mass = (double) num4 > 0.0 ? 1f / num4 : 0.0f;
      if (Settings.EnableWarmstarting)
      {
        bodyA.LinearVelocityInternal += bodyA.InvMass * this._impulse * this._J.LinearA;
        bodyA.AngularVelocityInternal += bodyA.InvI * this._impulse * this._J.AngularA;
        bodyB.LinearVelocityInternal += bodyB.InvMass * this._impulse * this._J.LinearB;
        bodyB.AngularVelocityInternal += bodyB.InvI * this._impulse * this._J.AngularB;
      }
      else
        this._impulse = 0.0f;
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      float num = this._mass * -this._J.Compute(bodyA.LinearVelocityInternal, bodyA.AngularVelocityInternal, bodyB.LinearVelocityInternal, bodyB.AngularVelocityInternal);
      this._impulse += num;
      bodyA.LinearVelocityInternal += bodyA.InvMass * num * this._J.LinearA;
      bodyA.AngularVelocityInternal += bodyA.InvI * num * this._J.AngularA;
      bodyB.LinearVelocityInternal += bodyB.InvMass * num * this._J.LinearB;
      bodyB.AngularVelocityInternal += bodyB.InvI * num * this._J.AngularB;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      float num1 = 0.0f;
      float num2 = 0.0f;
      if (this._revolute1 != null)
        num1 = this._revolute1.JointAngle;
      else if (this._fixedRevolute1 != null)
        num1 = this._fixedRevolute1.JointAngle;
      else if (this._prismatic1 != null)
        num1 = this._prismatic1.JointTranslation;
      else if (this._fixedPrismatic1 != null)
        num1 = this._fixedPrismatic1.JointTranslation;
      if (this._revolute2 != null)
        num2 = this._revolute2.JointAngle;
      else if (this._fixedRevolute2 != null)
        num2 = this._fixedRevolute2.JointAngle;
      else if (this._prismatic2 != null)
        num2 = this._prismatic2.JointTranslation;
      else if (this._fixedPrismatic2 != null)
        num2 = this._fixedPrismatic2.JointTranslation;
      float num3 = this._mass * -(this._ant - (num1 + this.Ratio * num2));
      bodyA.Sweep.C += bodyA.InvMass * num3 * this._J.LinearA;
      bodyA.Sweep.A += bodyA.InvI * num3 * this._J.AngularA;
      bodyB.Sweep.C += bodyB.InvMass * num3 * this._J.LinearB;
      bodyB.Sweep.A += bodyB.InvI * num3 * this._J.AngularB;
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return true;
    }
  }
}
