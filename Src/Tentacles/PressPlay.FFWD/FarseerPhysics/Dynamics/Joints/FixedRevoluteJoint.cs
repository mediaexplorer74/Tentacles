// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedRevoluteJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedRevoluteJoint : Joint
  {
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
    private float _upperAngle;
    private Vector2 _worldAnchor;

    public FixedRevoluteJoint(Body body, Vector2 bodyAnchor, Vector2 worldAnchor)
      : base(body)
    {
      this.JointType = JointType.FixedRevolute;
      this.LocalAnchorA = bodyAnchor;
      this._worldAnchor = worldAnchor;
      this.ReferenceAngle = -this.BodyA.Rotation;
      this._impulse = Vector3.Zero;
      this._limitState = LimitState.Inactive;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this._worldAnchor;
      set => this._worldAnchor = value;
    }

    public Vector2 LocalAnchorA { get; set; }

    public float ReferenceAngle { get; set; }

    public float JointAngle => this.BodyA.Sweep.A - this.ReferenceAngle;

    public float JointSpeed => this.BodyA.AngularVelocityInternal;

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
      return inv_dt * new Vector2(this._impulse.X, this._impulse.Y);
    }

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._impulse.Z;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      if (!this._enableMotor)
      {
        int num1 = this._enableLimit ? 1 : 0;
      }
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 worldAnchor = this._worldAnchor;
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      this._mass.Col1.X = (float) ((double) invMass + 0.0 + (double) a.Y * (double) a.Y * (double) invI + (double) worldAnchor.Y * (double) worldAnchor.Y * 0.0);
      this._mass.Col2.X = (float) (-(double) a.Y * (double) a.X * (double) invI - (double) worldAnchor.Y * (double) worldAnchor.X * 0.0);
      this._mass.Col3.X = (float) (-(double) a.Y * (double) invI - (double) worldAnchor.Y * 0.0);
      this._mass.Col1.Y = this._mass.Col2.X;
      this._mass.Col2.Y = (float) ((double) invMass + 0.0 + (double) a.X * (double) a.X * (double) invI + (double) worldAnchor.X * (double) worldAnchor.X * 0.0);
      this._mass.Col3.Y = (float) ((double) a.X * (double) invI + (double) worldAnchor.X * 0.0);
      this._mass.Col1.Z = this._mass.Col3.X;
      this._mass.Col2.Z = this._mass.Col3.Y;
      this._mass.Col3.Z = invI + 0.0f;
      this._motorMass = invI + 0.0f;
      if ((double) this._motorMass > 0.0)
        this._motorMass = 1f / this._motorMass;
      if (!this._enableMotor)
        this._motorImpulse = 0.0f;
      if (this._enableLimit)
      {
        float num2 = 0.0f - bodyA.Sweep.A - this.ReferenceAngle;
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
        bodyA.LinearVelocityInternal -= invMass * b;
        bodyA.AngularVelocityInternal -= invI * (MathUtils.Cross(a, b) + this._motorImpulse + this._impulse.Z);
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
      Vector2 velocityInternal1 = bodyA.LinearVelocityInternal;
      float velocityInternal2 = bodyA.AngularVelocityInternal;
      Vector2 zero = Vector2.Zero;
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      if (this._enableMotor && this._limitState != LimitState.Equal)
      {
        float num1 = this._motorMass * -(0.0f - velocityInternal2 - this._motorSpeed);
        float motorImpulse = this._motorImpulse;
        float high = step.dt * this._maxMotorTorque;
        this._motorImpulse = MathUtils.Clamp(this._motorImpulse + num1, -high, high);
        float num2 = this._motorImpulse - motorImpulse;
        velocityInternal2 -= invI * num2;
      }
      Vector2 vector2_1;
      float num;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        Transform transform;
        bodyA.GetTransform(out transform);
        Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
        Vector2 worldAnchor = this._worldAnchor;
        Vector2 vector2_2 = zero + MathUtils.Cross(0.0f, worldAnchor) - velocityInternal1 - MathUtils.Cross(velocityInternal2, a);
        float z = 0.0f - velocityInternal2;
        Vector3 vector3 = this._mass.Solve33(-new Vector3(vector2_2.X, vector2_2.Y, z));
        if (this._limitState == LimitState.Equal)
          this._impulse += vector3;
        else if (this._limitState == LimitState.AtLower)
        {
          if ((double) (this._impulse.Z + vector3.Z) < 0.0)
          {
            Vector2 vector2_3 = this._mass.Solve22(-vector2_2);
            vector3.X = vector2_3.X;
            vector3.Y = vector2_3.Y;
            vector3.Z = -this._impulse.Z;
            this._impulse.X += vector2_3.X;
            this._impulse.Y += vector2_3.Y;
            this._impulse.Z = 0.0f;
          }
        }
        else if (this._limitState == LimitState.AtUpper && (double) (this._impulse.Z + vector3.Z) > 0.0)
        {
          Vector2 vector2_4 = this._mass.Solve22(-vector2_2);
          vector3.X = vector2_4.X;
          vector3.Y = vector2_4.Y;
          vector3.Z = -this._impulse.Z;
          this._impulse.X += vector2_4.X;
          this._impulse.Y += vector2_4.Y;
          this._impulse.Z = 0.0f;
        }
        Vector2 b = new Vector2(vector3.X, vector3.Y);
        vector2_1 = velocityInternal1 - invMass * b;
        num = velocityInternal2 - invI * (MathUtils.Cross(a, b) + vector3.Z);
      }
      else
      {
        Transform transform;
        bodyA.GetTransform(out transform);
        Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
        Vector2 worldAnchor = this._worldAnchor;
        Vector2 b = this._mass.Solve22(-(zero + MathUtils.Cross(0.0f, worldAnchor) - velocityInternal1 - MathUtils.Cross(velocityInternal2, a)));
        this._impulse.X += b.X;
        this._impulse.Y += b.Y;
        vector2_1 = velocityInternal1 - invMass * b;
        num = velocityInternal2 - invI * MathUtils.Cross(a, b);
      }
      bodyA.LinearVelocityInternal = vector2_1;
      bodyA.AngularVelocityInternal = num;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      float num1 = 0.0f;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        float num2 = 0.0f - bodyA.Sweep.A - this.ReferenceAngle;
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
        bodyA.SynchronizeTransform();
      }
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 worldAnchor = this._worldAnchor;
      Vector2 vector2_1 = Vector2.Zero + worldAnchor - bodyA.Sweep.C - a;
      float num7 = vector2_1.Length();
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      if ((double) vector2_1.LengthSquared() > 1.0 / 400.0)
      {
        vector2_1.Normalize();
        Vector2 vector2_2 = 1f / (invMass + 0.0f) * -vector2_1;
        bodyA.Sweep.C -= 0.5f * invMass * vector2_2;
        vector2_1 = Vector2.Zero + worldAnchor - bodyA.Sweep.C - a;
      }
      Mat22 A = new Mat22(new Vector2(invMass + 0.0f, 0.0f), new Vector2(0.0f, invMass + 0.0f));
      Mat22 B1 = new Mat22(new Vector2(invI * a.Y * a.Y, -invI * a.X * a.Y), new Vector2(-invI * a.X * a.Y, invI * a.X * a.X));
      Mat22 B2 = new Mat22(new Vector2(0.0f * worldAnchor.Y * worldAnchor.Y, -0.0f * worldAnchor.X * worldAnchor.Y), new Vector2(-0.0f * worldAnchor.X * worldAnchor.Y, 0.0f * worldAnchor.X * worldAnchor.X));
      Mat22 R1;
      Mat22.Add(ref A, ref B1, out R1);
      Mat22 R2;
      Mat22.Add(ref R1, ref B2, out R2);
      Vector2 b = R2.Solve(-vector2_1);
      bodyA.Sweep.C -= bodyA.InvMass * b;
      bodyA.Sweep.A -= bodyA.InvI * MathUtils.Cross(a, b);
      bodyA.SynchronizeTransform();
      return (double) num7 <= 0.004999999888241291 && (double) num1 <= Math.PI / 90.0;
    }
  }
}
