// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.PrismaticJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class PrismaticJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private Mat33 _K;
    private float _a1;
    private float _a2;
    private Vector2 _axis;
    private bool _enableLimit;
    private bool _enableMotor;
    private Vector3 _impulse;
    private LimitState _limitState;
    private Vector2 _localXAxis1;
    private Vector2 _localYAxis1;
    private float _lowerTranslation;
    private float _maxMotorForce;
    private float _motorImpulse;
    private float _motorMass;
    private float _motorSpeed;
    private Vector2 _perp;
    private float _refAngle;
    private float _s1;
    private float _s2;
    private float _upperTranslation;

    internal PrismaticJoint() => this.JointType = JointType.Prismatic;

    public PrismaticJoint(
      Body bodyA,
      Body bodyB,
      Vector2 localAnchorA,
      Vector2 localAnchorB,
      Vector2 axis)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Prismatic;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this._localXAxis1 = this.BodyA.GetLocalVector(axis);
      this._localYAxis1 = MathUtils.Cross(1f, this._localXAxis1);
      this._refAngle = this.BodyB.Rotation - this.BodyA.Rotation;
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

    public float JointTranslation
    {
      get
      {
        return Vector2.Dot(this.BodyB.GetWorldPoint(this.LocalAnchorB) - this.BodyA.GetWorldPoint(this.LocalAnchorA), this.BodyA.GetWorldVector(ref this._localXAxis1));
      }
    }

    public float JointSpeed
    {
      get
      {
        Transform transform1;
        this.BodyA.GetTransform(out transform1);
        Transform transform2;
        this.BodyB.GetTransform(out transform2);
        Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - this.BodyA.LocalCenter);
        Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - this.BodyB.LocalCenter);
        Vector2 vector2_1 = this.BodyA.Sweep.C + a1;
        Vector2 vector2_2 = this.BodyB.Sweep.C + a2 - vector2_1;
        Vector2 worldVector = this.BodyA.GetWorldVector(ref this._localXAxis1);
        Vector2 velocityInternal1 = this.BodyA.LinearVelocityInternal;
        Vector2 velocityInternal2 = this.BodyB.LinearVelocityInternal;
        float velocityInternal3 = this.BodyA.AngularVelocityInternal;
        float velocityInternal4 = this.BodyB.AngularVelocityInternal;
        return Vector2.Dot(vector2_2, MathUtils.Cross(velocityInternal3, worldVector)) + Vector2.Dot(worldVector, velocityInternal2 + MathUtils.Cross(velocityInternal4, a2) - velocityInternal1 - MathUtils.Cross(velocityInternal3, a1));
      }
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
      get => this._lowerTranslation;
      set
      {
        this.WakeBodies();
        this._lowerTranslation = value;
      }
    }

    public float UpperLimit
    {
      get => this._upperTranslation;
      set
      {
        this.WakeBodies();
        this._upperTranslation = value;
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

    public float MaxMotorForce
    {
      get => this._maxMotorForce;
      set
      {
        this.WakeBodies();
        this._maxMotorForce = value;
      }
    }

    public float MotorForce
    {
      get => this._motorImpulse;
      set => this._motorImpulse = value;
    }

    public Vector2 LocalXAxis1
    {
      get => this._localXAxis1;
      set
      {
        this._localXAxis1 = this.BodyA.GetLocalVector(value);
        this._localYAxis1 = MathUtils.Cross(1f, this._localXAxis1);
      }
    }

    public float ReferenceAngle
    {
      get => this._refAngle;
      set => this._refAngle = value;
    }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      return inv_dt * (this._impulse.X * this._perp + (this._motorImpulse + this._impulse.Z) * this._axis);
    }

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._impulse.Y;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      this.LocalCenterA = bodyA.LocalCenter;
      this.LocalCenterB = bodyB.LocalCenter;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 vector2_1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - this.LocalCenterA);
      Vector2 a = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - this.LocalCenterB);
      Vector2 vector2_2 = bodyB.Sweep.C + a - bodyA.Sweep.C - vector2_1;
      this.InvMassA = bodyA.InvMass;
      this.InvIA = bodyA.InvI;
      this.InvMassB = bodyB.InvMass;
      this.InvIB = bodyB.InvI;
      this._axis = MathUtils.Multiply(ref transform1.R, this._localXAxis1);
      this._a1 = MathUtils.Cross(vector2_2 + vector2_1, this._axis);
      this._a2 = MathUtils.Cross(a, this._axis);
      this._motorMass = (float) ((double) this.InvMassA + (double) this.InvMassB + (double) this.InvIA * (double) this._a1 * (double) this._a1 + (double) this.InvIB * (double) this._a2 * (double) this._a2);
      if ((double) this._motorMass > 1.1920928955078125E-07)
        this._motorMass = 1f / this._motorMass;
      this._perp = MathUtils.Multiply(ref transform1.R, this._localYAxis1);
      this._s1 = MathUtils.Cross(vector2_2 + vector2_1, this._perp);
      this._s2 = MathUtils.Cross(a, this._perp);
      float invMassA = this.InvMassA;
      float invMassB = this.InvMassB;
      float invIa = this.InvIA;
      float invIb = this.InvIB;
      float x = (float) ((double) invMassA + (double) invMassB + (double) invIa * (double) this._s1 * (double) this._s1 + (double) invIb * (double) this._s2 * (double) this._s2);
      float num1 = (float) ((double) invIa * (double) this._s1 + (double) invIb * (double) this._s2);
      float num2 = (float) ((double) invIa * (double) this._s1 * (double) this._a1 + (double) invIb * (double) this._s2 * (double) this._a2);
      float y = invIa + invIb;
      float num3 = (float) ((double) invIa * (double) this._a1 + (double) invIb * (double) this._a2);
      float z = (float) ((double) invMassA + (double) invMassB + (double) invIa * (double) this._a1 * (double) this._a1 + (double) invIb * (double) this._a2 * (double) this._a2);
      this._K.Col1 = new Vector3(x, num1, num2);
      this._K.Col2 = new Vector3(num1, y, num3);
      this._K.Col3 = new Vector3(num2, num3, z);
      if (this._enableLimit)
      {
        float num4 = Vector2.Dot(this._axis, vector2_2);
        if ((double) Math.Abs(this._upperTranslation - this._lowerTranslation) < 0.0099999997764825821)
          this._limitState = LimitState.Equal;
        else if ((double) num4 <= (double) this._lowerTranslation)
        {
          if (this._limitState != LimitState.AtLower)
          {
            this._limitState = LimitState.AtLower;
            this._impulse.Z = 0.0f;
          }
        }
        else if ((double) num4 >= (double) this._upperTranslation)
        {
          if (this._limitState != LimitState.AtUpper)
          {
            this._limitState = LimitState.AtUpper;
            this._impulse.Z = 0.0f;
          }
        }
        else
        {
          this._limitState = LimitState.Inactive;
          this._impulse.Z = 0.0f;
        }
      }
      else
        this._limitState = LimitState.Inactive;
      if (!this._enableMotor)
        this._motorImpulse = 0.0f;
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        this._motorImpulse *= step.dtRatio;
        Vector2 vector2_3 = this._impulse.X * this._perp + (this._motorImpulse + this._impulse.Z) * this._axis;
        float num5 = (float) ((double) this._impulse.X * (double) this._s1 + (double) this._impulse.Y + ((double) this._motorImpulse + (double) this._impulse.Z) * (double) this._a1);
        float num6 = (float) ((double) this._impulse.X * (double) this._s2 + (double) this._impulse.Y + ((double) this._motorImpulse + (double) this._impulse.Z) * (double) this._a2);
        bodyA.LinearVelocityInternal -= this.InvMassA * vector2_3;
        bodyA.AngularVelocityInternal -= this.InvIA * num5;
        bodyB.LinearVelocityInternal += this.InvMassB * vector2_3;
        bodyB.AngularVelocityInternal += this.InvIB * num6;
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
      if (this._enableMotor && this._limitState != LimitState.Equal)
      {
        float num1 = this._motorMass * (this._motorSpeed - (float) ((double) Vector2.Dot(this._axis, velocityInternal3 - velocityInternal1) + (double) this._a2 * (double) velocityInternal4 - (double) this._a1 * (double) velocityInternal2));
        float motorImpulse = this._motorImpulse;
        float high = step.dt * this._maxMotorForce;
        this._motorImpulse = MathUtils.Clamp(this._motorImpulse + num1, -high, high);
        float num2 = this._motorImpulse - motorImpulse;
        Vector2 vector2 = num2 * this._axis;
        float num3 = num2 * this._a1;
        float num4 = num2 * this._a2;
        velocityInternal1 -= this.InvMassA * vector2;
        velocityInternal2 -= this.InvIA * num3;
        velocityInternal3 += this.InvMassB * vector2;
        velocityInternal4 += this.InvIB * num4;
      }
      Vector2 vector2_1 = new Vector2((float) ((double) Vector2.Dot(this._perp, velocityInternal3 - velocityInternal1) + (double) this._s2 * (double) velocityInternal4 - (double) this._s1 * (double) velocityInternal2), velocityInternal4 - velocityInternal2);
      Vector2 vector2_2;
      float num5;
      Vector2 vector2_3;
      float num6;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        float z = (float) ((double) Vector2.Dot(this._axis, velocityInternal3 - velocityInternal1) + (double) this._a2 * (double) velocityInternal4 - (double) this._a1 * (double) velocityInternal2);
        Vector3 vector3_1 = new Vector3(vector2_1.X, vector2_1.Y, z);
        Vector3 impulse = this._impulse;
        this._impulse += this._K.Solve33(-vector3_1);
        if (this._limitState == LimitState.AtLower)
          this._impulse.Z = Math.Max(this._impulse.Z, 0.0f);
        else if (this._limitState == LimitState.AtUpper)
          this._impulse.Z = Math.Min(this._impulse.Z, 0.0f);
        Vector2 vector2_4 = this._K.Solve22(-vector2_1 - (this._impulse.Z - impulse.Z) * new Vector2(this._K.Col3.X, this._K.Col3.Y)) + new Vector2(impulse.X, impulse.Y);
        this._impulse.X = vector2_4.X;
        this._impulse.Y = vector2_4.Y;
        Vector3 vector3_2 = this._impulse - impulse;
        Vector2 vector2_5 = vector3_2.X * this._perp + vector3_2.Z * this._axis;
        float num7 = (float) ((double) vector3_2.X * (double) this._s1 + (double) vector3_2.Y + (double) vector3_2.Z * (double) this._a1);
        float num8 = (float) ((double) vector3_2.X * (double) this._s2 + (double) vector3_2.Y + (double) vector3_2.Z * (double) this._a2);
        vector2_2 = velocityInternal1 - this.InvMassA * vector2_5;
        num5 = velocityInternal2 - this.InvIA * num7;
        vector2_3 = velocityInternal3 + this.InvMassB * vector2_5;
        num6 = velocityInternal4 + this.InvIB * num8;
      }
      else
      {
        Vector2 vector2_6 = this._K.Solve22(-vector2_1);
        this._impulse.X += vector2_6.X;
        this._impulse.Y += vector2_6.Y;
        Vector2 vector2_7 = vector2_6.X * this._perp;
        float num9 = vector2_6.X * this._s1 + vector2_6.Y;
        float num10 = vector2_6.X * this._s2 + vector2_6.Y;
        vector2_2 = velocityInternal1 - this.InvMassA * vector2_7;
        num5 = velocityInternal2 - this.InvIA * num9;
        vector2_3 = velocityInternal3 + this.InvMassB * vector2_7;
        num6 = velocityInternal4 + this.InvIB * num10;
      }
      bodyA.LinearVelocityInternal = vector2_2;
      bodyA.AngularVelocityInternal = num5;
      bodyB.LinearVelocityInternal = vector2_3;
      bodyB.AngularVelocityInternal = num6;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Vector2 c1 = bodyA.Sweep.C;
      float a1 = bodyA.Sweep.A;
      Vector2 c2 = bodyB.Sweep.C;
      float a2 = bodyB.Sweep.A;
      float val1 = 0.0f;
      bool flag = false;
      float num1 = 0.0f;
      Mat22 A1 = new Mat22(a1);
      Mat22 A2 = new Mat22(a2);
      Vector2 vector2_1 = MathUtils.Multiply(ref A1, this.LocalAnchorA - this.LocalCenterA);
      Vector2 a3 = MathUtils.Multiply(ref A2, this.LocalAnchorB - this.LocalCenterB);
      Vector2 vector2_2 = c2 + a3 - c1 - vector2_1;
      if (this._enableLimit)
      {
        this._axis = MathUtils.Multiply(ref A1, this._localXAxis1);
        this._a1 = MathUtils.Cross(vector2_2 + vector2_1, this._axis);
        this._a2 = MathUtils.Cross(a3, this._axis);
        float a4 = Vector2.Dot(this._axis, vector2_2);
        if ((double) Math.Abs(this._upperTranslation - this._lowerTranslation) < 0.0099999997764825821)
        {
          num1 = MathUtils.Clamp(a4, -0.2f, 0.2f);
          val1 = Math.Abs(a4);
          flag = true;
        }
        else if ((double) a4 <= (double) this._lowerTranslation)
        {
          num1 = MathUtils.Clamp((float) ((double) a4 - (double) this._lowerTranslation + 0.004999999888241291), -0.2f, 0.0f);
          val1 = this._lowerTranslation - a4;
          flag = true;
        }
        else if ((double) a4 >= (double) this._upperTranslation)
        {
          num1 = MathUtils.Clamp((float) ((double) a4 - (double) this._upperTranslation - 0.004999999888241291), 0.0f, 0.2f);
          val1 = a4 - this._upperTranslation;
          flag = true;
        }
      }
      this._perp = MathUtils.Multiply(ref A1, this._localYAxis1);
      this._s1 = MathUtils.Cross(vector2_2 + vector2_1, this._perp);
      this._s2 = MathUtils.Cross(a3, this._perp);
      Vector2 vector2_3 = new Vector2(Vector2.Dot(this._perp, vector2_2), a2 - a1 - this.ReferenceAngle);
      float num2 = Math.Max(val1, Math.Abs(vector2_3.X));
      float num3 = Math.Abs(vector2_3.Y);
      Vector3 vector3;
      if (flag)
      {
        float invMassA = this.InvMassA;
        float invMassB = this.InvMassB;
        float invIa = this.InvIA;
        float invIb = this.InvIB;
        float x = (float) ((double) invMassA + (double) invMassB + (double) invIa * (double) this._s1 * (double) this._s1 + (double) invIb * (double) this._s2 * (double) this._s2);
        float num4 = (float) ((double) invIa * (double) this._s1 + (double) invIb * (double) this._s2);
        float num5 = (float) ((double) invIa * (double) this._s1 * (double) this._a1 + (double) invIb * (double) this._s2 * (double) this._a2);
        float y = invIa + invIb;
        float num6 = (float) ((double) invIa * (double) this._a1 + (double) invIb * (double) this._a2);
        float z = (float) ((double) invMassA + (double) invMassB + (double) invIa * (double) this._a1 * (double) this._a1 + (double) invIb * (double) this._a2 * (double) this._a2);
        this._K.Col1 = new Vector3(x, num4, num5);
        this._K.Col2 = new Vector3(num4, y, num6);
        this._K.Col3 = new Vector3(num5, num6, z);
        vector3 = this._K.Solve33(new Vector3(-vector2_3.X, -vector2_3.Y, -num1));
      }
      else
      {
        float invMassA = this.InvMassA;
        float invMassB = this.InvMassB;
        float invIa = this.InvIA;
        float invIb = this.InvIB;
        float x = (float) ((double) invMassA + (double) invMassB + (double) invIa * (double) this._s1 * (double) this._s1 + (double) invIb * (double) this._s2 * (double) this._s2);
        float num7 = (float) ((double) invIa * (double) this._s1 + (double) invIb * (double) this._s2);
        float y = invIa + invIb;
        this._K.Col1 = new Vector3(x, num7, 0.0f);
        this._K.Col2 = new Vector3(num7, y, 0.0f);
        Vector2 vector2_4 = this._K.Solve22(-vector2_3);
        vector3.X = vector2_4.X;
        vector3.Y = vector2_4.Y;
        vector3.Z = 0.0f;
      }
      Vector2 vector2_5 = vector3.X * this._perp + vector3.Z * this._axis;
      float num8 = (float) ((double) vector3.X * (double) this._s1 + (double) vector3.Y + (double) vector3.Z * (double) this._a1);
      float num9 = (float) ((double) vector3.X * (double) this._s2 + (double) vector3.Y + (double) vector3.Z * (double) this._a2);
      Vector2 vector2_6 = c1 - this.InvMassA * vector2_5;
      float num10 = a1 - this.InvIA * num8;
      Vector2 vector2_7 = c2 + this.InvMassB * vector2_5;
      float num11 = a2 + this.InvIB * num9;
      bodyA.Sweep.C = vector2_6;
      bodyA.Sweep.A = num10;
      bodyB.Sweep.C = vector2_7;
      bodyB.Sweep.A = num11;
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) num2 <= 0.004999999888241291 && (double) num3 <= Math.PI / 90.0;
    }
  }
}
