// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedPrismaticJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedPrismaticJoint : Joint
  {
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
    private float _motorMass;
    private float _motorSpeed;
    private Vector2 _perp;
    private float _refAngle;
    private float _s1;
    private float _s2;
    private float _upperTranslation;

    public FixedPrismaticJoint(Body body, Vector2 worldAnchor, Vector2 axis)
      : base(body)
    {
      this.JointType = JointType.FixedPrismatic;
      this.BodyB = this.BodyA;
      this.LocalAnchorA = worldAnchor;
      this.LocalAnchorB = this.BodyB.GetLocalPoint(worldAnchor);
      this._localXAxis1 = axis;
      this._localYAxis1 = MathUtils.Cross(1f, this._localXAxis1);
      this._refAngle = this.BodyB.Rotation;
      this._limitState = LimitState.Inactive;
    }

    public Vector2 LocalAnchorA { get; set; }

    public Vector2 LocalAnchorB { get; set; }

    public override Vector2 WorldAnchorA => this.LocalAnchorA;

    public override Vector2 WorldAnchorB
    {
      get => this.BodyA.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public float JointTranslation
    {
      get
      {
        return Vector2.Dot(this.BodyB.GetWorldPoint(this.LocalAnchorB) - this.LocalAnchorA, this._localXAxis1);
      }
    }

    public float JointSpeed
    {
      get
      {
        Transform transform;
        this.BodyB.GetTransform(out transform);
        Vector2 localAnchorA = this.LocalAnchorA;
        Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorB - this.BodyB.LocalCenter);
        Vector2 vector2_1 = localAnchorA;
        Vector2 vector2_2 = this.BodyB.Sweep.C + a - vector2_1;
        Vector2 localXaxis1 = this._localXAxis1;
        Vector2 zero = Vector2.Zero;
        Vector2 velocityInternal1 = this.BodyB.LinearVelocityInternal;
        float velocityInternal2 = this.BodyB.AngularVelocityInternal;
        return Vector2.Dot(vector2_2, MathUtils.Cross(0.0f, localXaxis1)) + Vector2.Dot(localXaxis1, velocityInternal1 + MathUtils.Cross(velocityInternal2, a) - zero - MathUtils.Cross(0.0f, localAnchorA));
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
      set
      {
        this.WakeBodies();
        this._maxMotorForce = value;
      }
    }

    public float MotorForce { get; set; }

    public Vector2 LocalXAxis1
    {
      get => this._localXAxis1;
      set
      {
        this._localXAxis1 = value;
        this._localYAxis1 = MathUtils.Cross(1f, this._localXAxis1);
      }
    }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      return inv_dt * (this._impulse.X * this._perp + (this.MotorForce + this._impulse.Z) * this._axis);
    }

    public override float GetReactionTorque(float inv_dt) => inv_dt * this._impulse.Y;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyB = this.BodyB;
      this.LocalCenterA = Vector2.Zero;
      this.LocalCenterB = bodyB.LocalCenter;
      Transform transform;
      bodyB.GetTransform(out transform);
      Vector2 localAnchorA = this.LocalAnchorA;
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorB - this.LocalCenterB);
      Vector2 vector2_1 = bodyB.Sweep.C + a - localAnchorA;
      this.InvMassA = 0.0f;
      this.InvIA = 0.0f;
      this.InvMassB = bodyB.InvMass;
      this.InvIB = bodyB.InvI;
      this._axis = this._localXAxis1;
      this._a1 = MathUtils.Cross(vector2_1 + localAnchorA, this._axis);
      this._a2 = MathUtils.Cross(a, this._axis);
      this._motorMass = (float) ((double) this.InvMassA + (double) this.InvMassB + (double) this.InvIA * (double) this._a1 * (double) this._a1 + (double) this.InvIB * (double) this._a2 * (double) this._a2);
      if ((double) this._motorMass > 1.1920928955078125E-07)
        this._motorMass = 1f / this._motorMass;
      this._perp = this._localYAxis1;
      this._s1 = MathUtils.Cross(vector2_1 + localAnchorA, this._perp);
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
        float num4 = Vector2.Dot(this._axis, vector2_1);
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
        this.MotorForce = 0.0f;
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        this.MotorForce *= step.dtRatio;
        Vector2 vector2_2 = this._impulse.X * this._perp + (this.MotorForce + this._impulse.Z) * this._axis;
        float num5 = (float) ((double) this._impulse.X * (double) this._s2 + (double) this._impulse.Y + ((double) this.MotorForce + (double) this._impulse.Z) * (double) this._a2);
        bodyB.LinearVelocityInternal += this.InvMassB * vector2_2;
        bodyB.AngularVelocityInternal += this.InvIB * num5;
      }
      else
      {
        this._impulse = Vector3.Zero;
        this.MotorForce = 0.0f;
      }
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyB = this.BodyB;
      Vector2 zero = Vector2.Zero;
      float num1 = 0.0f;
      Vector2 velocityInternal1 = bodyB.LinearVelocityInternal;
      float velocityInternal2 = bodyB.AngularVelocityInternal;
      if (this._enableMotor && this._limitState != LimitState.Equal)
      {
        float num2 = this._motorMass * (this._motorSpeed - (float) ((double) Vector2.Dot(this._axis, velocityInternal1 - zero) + (double) this._a2 * (double) velocityInternal2 - (double) this._a1 * (double) num1));
        float motorForce = this.MotorForce;
        float high = step.dt * this._maxMotorForce;
        this.MotorForce = MathUtils.Clamp(this.MotorForce + num2, -high, high);
        float num3 = this.MotorForce - motorForce;
        Vector2 vector2 = num3 * this._axis;
        float num4 = num3 * this._a1;
        float num5 = num3 * this._a2;
        zero -= this.InvMassA * vector2;
        num1 -= this.InvIA * num4;
        velocityInternal1 += this.InvMassB * vector2;
        velocityInternal2 += this.InvIB * num5;
      }
      Vector2 vector2_1 = new Vector2((float) ((double) Vector2.Dot(this._perp, velocityInternal1 - zero) + (double) this._s2 * (double) velocityInternal2 - (double) this._s1 * (double) num1), velocityInternal2 - num1);
      Vector2 vector2_2;
      float num6;
      if (this._enableLimit && this._limitState != LimitState.Inactive)
      {
        float z = (float) ((double) Vector2.Dot(this._axis, velocityInternal1 - zero) + (double) this._a2 * (double) velocityInternal2 - (double) this._a1 * (double) num1);
        Vector3 vector3_1 = new Vector3(vector2_1.X, vector2_1.Y, z);
        Vector3 impulse = this._impulse;
        this._impulse += this._K.Solve33(-vector3_1);
        if (this._limitState == LimitState.AtLower)
          this._impulse.Z = Math.Max(this._impulse.Z, 0.0f);
        else if (this._limitState == LimitState.AtUpper)
          this._impulse.Z = Math.Min(this._impulse.Z, 0.0f);
        Vector2 vector2_3 = this._K.Solve22(-vector2_1 - (this._impulse.Z - impulse.Z) * new Vector2(this._K.Col3.X, this._K.Col3.Y)) + new Vector2(impulse.X, impulse.Y);
        this._impulse.X = vector2_3.X;
        this._impulse.Y = vector2_3.Y;
        Vector3 vector3_2 = this._impulse - impulse;
        Vector2 vector2_4 = vector3_2.X * this._perp + vector3_2.Z * this._axis;
        float num7 = (float) ((double) vector3_2.X * (double) this._s2 + (double) vector3_2.Y + (double) vector3_2.Z * (double) this._a2);
        vector2_2 = velocityInternal1 + this.InvMassB * vector2_4;
        num6 = velocityInternal2 + this.InvIB * num7;
      }
      else
      {
        Vector2 vector2_5 = this._K.Solve22(-vector2_1);
        this._impulse.X += vector2_5.X;
        this._impulse.Y += vector2_5.Y;
        Vector2 vector2_6 = vector2_5.X * this._perp;
        float num8 = vector2_5.X * this._s2 + vector2_5.Y;
        vector2_2 = velocityInternal1 + this.InvMassB * vector2_6;
        num6 = velocityInternal2 + this.InvIB * num8;
      }
      bodyB.LinearVelocityInternal = vector2_2;
      bodyB.AngularVelocityInternal = num6;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyB = this.BodyB;
      Vector2 zero = Vector2.Zero;
      float angle = 0.0f;
      Vector2 c = bodyB.Sweep.C;
      float a1 = bodyB.Sweep.A;
      float val1 = 0.0f;
      bool flag = false;
      float num1 = 0.0f;
      Mat22 A1 = new Mat22(angle);
      Mat22 A2 = new Mat22(a1);
      Vector2 vector2_1 = MathUtils.Multiply(ref A1, this.LocalAnchorA - this.LocalCenterA);
      Vector2 a2 = MathUtils.Multiply(ref A2, this.LocalAnchorB - this.LocalCenterB);
      Vector2 vector2_2 = c + a2 - zero - vector2_1;
      if (this._enableLimit)
      {
        this._axis = MathUtils.Multiply(ref A1, this._localXAxis1);
        this._a1 = MathUtils.Cross(vector2_2 + vector2_1, this._axis);
        this._a2 = MathUtils.Cross(a2, this._axis);
        float a3 = Vector2.Dot(this._axis, vector2_2);
        if ((double) Math.Abs(this._upperTranslation - this._lowerTranslation) < 0.0099999997764825821)
        {
          num1 = MathUtils.Clamp(a3, -0.2f, 0.2f);
          val1 = Math.Abs(a3);
          flag = true;
        }
        else if ((double) a3 <= (double) this._lowerTranslation)
        {
          num1 = MathUtils.Clamp((float) ((double) a3 - (double) this._lowerTranslation + 0.004999999888241291), -0.2f, 0.0f);
          val1 = this._lowerTranslation - a3;
          flag = true;
        }
        else if ((double) a3 >= (double) this._upperTranslation)
        {
          num1 = MathUtils.Clamp((float) ((double) a3 - (double) this._upperTranslation - 0.004999999888241291), 0.0f, 0.2f);
          val1 = a3 - this._upperTranslation;
          flag = true;
        }
      }
      this._perp = MathUtils.Multiply(ref A1, this._localYAxis1);
      this._s1 = MathUtils.Cross(vector2_2 + vector2_1, this._perp);
      this._s2 = MathUtils.Cross(a2, this._perp);
      Vector2 vector2_3 = new Vector2(Vector2.Dot(this._perp, vector2_2), a1 - angle - this._refAngle);
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
      float num8 = (float) ((double) vector3.X * (double) this._s2 + (double) vector3.Y + (double) vector3.Z * (double) this._a2);
      Vector2 vector2_6 = c + this.InvMassB * vector2_5;
      float num9 = a1 + this.InvIB * num8;
      bodyB.Sweep.C = vector2_6;
      bodyB.Sweep.A = num9;
      bodyB.SynchronizeTransform();
      return (double) num2 <= 0.004999999888241291 && (double) num3 <= Math.PI / 90.0;
    }
  }
}
