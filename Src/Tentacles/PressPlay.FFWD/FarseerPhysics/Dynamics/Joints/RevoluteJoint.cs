// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.RevoluteJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class RevoluteJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private bool _enableLimit;
    private bool _enableMotor;
    private Vector3 _impulse;
    private LimitState _limitState;
    private float _lowerAngle;
    private Mat33 _mass;
    private float _maxMotorTorque;
    private float _motorImpulse;
    private float _motorMass;
    private float _motorSpeed;
    private float _referenceAngle;
    private float _tmpFloat1;
    private Vector2 _tmpVector1;
    private Vector2 _tmpVector2;
    private float _upperAngle;

    internal RevoluteJoint() => this.JointType = JointType.Revolute;

    public RevoluteJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Revolute;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this.ReferenceAngle = this.BodyB.Rotation - this.BodyA.Rotation;
      this._impulse = Vector3.Zero;
      this._limitState = LimitState.Inactive;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public float ReferenceAngle
    {
      get => this._referenceAngle;
      set
      {
        this.WakeBodies();
        this._referenceAngle = value;
      }
    }

    public float JointAngle => this.BodyB.Sweep.A - this.BodyA.Sweep.A - this.ReferenceAngle;

    public float JointSpeed
    {
      get => this.BodyB.AngularVelocityInternal - this.BodyA.AngularVelocityInternal;
    }

    public bool LimitEnabled
    {
      get => this._enableLimit;
      set
      {
        this.WakeBodies();
        this._enableLimit = value;
      }
    }

    public float LowerLimit
    {
      get => this._lowerAngle;
      set
      {
        this.WakeBodies();
        this._lowerAngle = value;
      }
    }

    public float UpperLimit
    {
      get => this._upperAngle;
      set
      {
        this.WakeBodies();
        this._upperAngle = value;
      }
    }

    public bool MotorEnabled
    {
      get => this._enableMotor;
      set
      {
        this.WakeBodies();
        this._enableMotor = value;
      }
    }

    public float MotorSpeed
    {
      set
      {
        this.WakeBodies();
        this._motorSpeed = value;
      }
      get => this._motorSpeed;
    }

    public float MaxMotorTorque
    {
      set
      {
        this.WakeBodies();
        this._maxMotorTorque = value;
      }
      get => this._maxMotorTorque;
    }

    public float MotorTorque
    {
      get => this._motorImpulse;
      set
      {
        this.WakeBodies();
        this._motorImpulse = value;
      }
    }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      Vector2 vector2 = new Vector2(this._impulse.X, this._impulse.Y);
      return inv_dt * vector2;
    }

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._impulse.Z;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      if (!this._enableMotor)
      {
        int num1 = this._enableLimit ? 1 : 0;
      }
      Vector2 a1 = MathUtils.Multiply(ref bodyA.Xf.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref bodyB.Xf.R, this.LocalAnchorB - bodyB.LocalCenter);
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
      this._motorMass = invI1 + invI2;
      if ((double) this._motorMass > 0.0)
        this._motorMass = 1f / this._motorMass;
      if (!this._enableMotor)
        this._motorImpulse = 0.0f;
      if (this._enableLimit)
      {
        float num2 = bodyB.Sweep.A - bodyA.Sweep.A - this.ReferenceAngle;
        if ((double) Math.Abs(this._upperAngle - this._lowerAngle) < 0.069813169538974762)
          this._limitState = LimitState.Equal;
        else if ((double) num2 <= (double) this._lowerAngle)
        {
          if (this._limitState != LimitState.AtLower)
            this._impulse.Z = 0.0f;
          this._limitState = LimitState.AtLower;
        }
        else if ((double) num2 >= (double) this._upperAngle)
        {
          if (this._limitState != LimitState.AtUpper)
            this._impulse.Z = 0.0f;
          this._limitState = LimitState.AtUpper;
        }
        else
        {
          this._limitState = LimitState.Inactive;
          this._impulse.Z = 0.0f;
        }
      }
      else
        this._limitState = LimitState.Inactive;
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        this._motorImpulse *= step.dtRatio;
        Vector2 b = new Vector2(this._impulse.X, this._impulse.Y);
        bodyA.LinearVelocityInternal -= invMass1 * b;
        MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
        bodyA.AngularVelocityInternal -= invI1 * (this._tmpFloat1 + this._motorImpulse + this._impulse.Z);
        bodyB.LinearVelocityInternal += invMass2 * b;
        MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
        bodyB.AngularVelocityInternal += invI2 * (this._tmpFloat1 + this._motorImpulse + this._impulse.Z);
      }
      else
      {
        this._impulse = Vector3.Zero;
        this._motorImpulse = 0.0f;
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
      if (this._enableMotor && this._limitState != LimitState.Equal)
      {
        float num1 = this._motorMass * -(velocityInternal4 - velocityInternal2 - this._motorSpeed);
        float motorImpulse = this._motorImpulse;
        float high = step.dt * this._maxMotorTorque;
        this._motorImpulse = MathUtils.Clamp(this._motorImpulse + num1, -high, high);
        float num2 = this._motorImpulse - motorImpulse;
        velocityInternal2 -= invI1 * num2;
        velocityInternal4 += invI2 * num2;
      }
      Vector2 vector2_1;
      float num3;
      Vector2 vector2_2;
      float num4;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        Vector2 a1 = MathUtils.Multiply(ref bodyA.Xf.R, this.LocalAnchorA - bodyA.LocalCenter);
        Vector2 a2 = MathUtils.Multiply(ref bodyB.Xf.R, this.LocalAnchorB - bodyB.LocalCenter);
        MathUtils.Cross(velocityInternal4, ref a2, out this._tmpVector2);
        MathUtils.Cross(velocityInternal2, ref a1, out this._tmpVector1);
        Vector2 vector2_3 = velocityInternal3 + this._tmpVector2 - velocityInternal1 - this._tmpVector1;
        float z = velocityInternal4 - velocityInternal2;
        Vector3 vector3 = this._mass.Solve33(-new Vector3(vector2_3.X, vector2_3.Y, z));
        if (this._limitState == LimitState.Equal)
          this._impulse += vector3;
        else if (this._limitState == LimitState.AtLower)
        {
          if ((double) (this._impulse.Z + vector3.Z) < 0.0)
          {
            Vector2 vector2_4 = this._mass.Solve22(-vector2_3);
            vector3.X = vector2_4.X;
            vector3.Y = vector2_4.Y;
            vector3.Z = -this._impulse.Z;
            this._impulse.X += vector2_4.X;
            this._impulse.Y += vector2_4.Y;
            this._impulse.Z = 0.0f;
          }
        }
        else if (this._limitState == LimitState.AtUpper && (double) (this._impulse.Z + vector3.Z) > 0.0)
        {
          Vector2 vector2_5 = this._mass.Solve22(-vector2_3);
          vector3.X = vector2_5.X;
          vector3.Y = vector2_5.Y;
          vector3.Z = -this._impulse.Z;
          this._impulse.X += vector2_5.X;
          this._impulse.Y += vector2_5.Y;
          this._impulse.Z = 0.0f;
        }
        Vector2 b = new Vector2(vector3.X, vector3.Y);
        vector2_1 = velocityInternal1 - invMass1 * b;
        MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
        num3 = velocityInternal2 - invI1 * (this._tmpFloat1 + vector3.Z);
        vector2_2 = velocityInternal3 + invMass2 * b;
        MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
        num4 = velocityInternal4 + invI2 * (this._tmpFloat1 + vector3.Z);
      }
      else
      {
        this._tmpVector1 = this.LocalAnchorA - bodyA.LocalCenter;
        this._tmpVector2 = this.LocalAnchorB - bodyB.LocalCenter;
        Vector2 a3 = MathUtils.Multiply(ref bodyA.Xf.R, ref this._tmpVector1);
        Vector2 a4 = MathUtils.Multiply(ref bodyB.Xf.R, ref this._tmpVector2);
        MathUtils.Cross(velocityInternal4, ref a4, out this._tmpVector2);
        MathUtils.Cross(velocityInternal2, ref a3, out this._tmpVector1);
        Vector2 b = this._mass.Solve22(-(velocityInternal3 + this._tmpVector2 - velocityInternal1 - this._tmpVector1));
        this._impulse.X += b.X;
        this._impulse.Y += b.Y;
        vector2_1 = velocityInternal1 - invMass1 * b;
        MathUtils.Cross(ref a3, ref b, out this._tmpFloat1);
        num3 = velocityInternal2 - invI1 * this._tmpFloat1;
        vector2_2 = velocityInternal3 + invMass2 * b;
        MathUtils.Cross(ref a4, ref b, out this._tmpFloat1);
        num4 = velocityInternal4 + invI2 * this._tmpFloat1;
      }
      bodyA.LinearVelocityInternal = vector2_1;
      bodyA.AngularVelocityInternal = num3;
      bodyB.LinearVelocityInternal = vector2_2;
      bodyB.AngularVelocityInternal = num4;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      float num1 = 0.0f;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        float num2 = bodyB.Sweep.A - bodyA.Sweep.A - this.ReferenceAngle;
        float num3 = 0.0f;
        if (this._limitState == LimitState.Equal)
        {
          float num4 = MathUtils.Clamp(num2 - this._lowerAngle, -2f * (float) Math.PI / 45f, 2f * (float) Math.PI / 45f);
          num3 = -this._motorMass * num4;
          num1 = Math.Abs(num4);
        }
        else if (this._limitState == LimitState.AtLower)
        {
          float num5 = num2 - this._lowerAngle;
          num1 = -num5;
          num3 = -this._motorMass * MathUtils.Clamp(num5 + (float) Math.PI / 90f, -2f * (float) Math.PI / 45f, 0.0f);
        }
        else if (this._limitState == LimitState.AtUpper)
        {
          float num6 = num2 - this._upperAngle;
          num1 = num6;
          num3 = -this._motorMass * MathUtils.Clamp(num6 - (float) Math.PI / 90f, 0.0f, 2f * (float) Math.PI / 45f);
        }
        bodyA.Sweep.A -= bodyA.InvI * num3;
        bodyB.Sweep.A += bodyB.InvI * num3;
        bodyA.SynchronizeTransform();
        bodyB.SynchronizeTransform();
      }
      Vector2 a1 = MathUtils.Multiply(ref bodyA.Xf.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref bodyB.Xf.R, this.LocalAnchorB - bodyB.LocalCenter);
      Vector2 vector2_1 = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float num7 = vector2_1.Length();
      float invMass1 = bodyA.InvMass;
      float invMass2 = bodyB.InvMass;
      float invI1 = bodyA.InvI;
      float invI2 = bodyB.InvI;
      if ((double) vector2_1.LengthSquared() > 1.0 / 400.0)
      {
        vector2_1.Normalize();
        Vector2 vector2_2 = 1f / (invMass1 + invMass2) * -vector2_1;
        bodyA.Sweep.C -= 0.5f * invMass1 * vector2_2;
        bodyB.Sweep.C += 0.5f * invMass2 * vector2_2;
        vector2_1 = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      }
      Mat22 A = new Mat22(new Vector2(invMass1 + invMass2, 0.0f), new Vector2(0.0f, invMass1 + invMass2));
      Mat22 B1 = new Mat22(new Vector2(invI1 * a1.Y * a1.Y, -invI1 * a1.X * a1.Y), new Vector2(-invI1 * a1.X * a1.Y, invI1 * a1.X * a1.X));
      Mat22 B2 = new Mat22(new Vector2(invI2 * a2.Y * a2.Y, -invI2 * a2.X * a2.Y), new Vector2(-invI2 * a2.X * a2.Y, invI2 * a2.X * a2.X));
      Mat22 R1;
      Mat22.Add(ref A, ref B1, out R1);
      Mat22 R2;
      Mat22.Add(ref R1, ref B2, out R2);
      Vector2 b = R2.Solve(-vector2_1);
      bodyA.Sweep.C -= bodyA.InvMass * b;
      MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
      bodyA.Sweep.A -= bodyA.InvI * this._tmpFloat1;
      bodyB.Sweep.C += bodyB.InvMass * b;
      MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
      bodyB.Sweep.A += bodyB.InvI * this._tmpFloat1;
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) num7 <= 0.004999999888241291 && (double) num1 <= Math.PI / 90.0;
    }
  }
}
