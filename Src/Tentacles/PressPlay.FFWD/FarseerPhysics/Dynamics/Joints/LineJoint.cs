// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.LineJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class LineJoint : Joint
  {
    private Vector2 _ax;
    private Vector2 _ay;
    private float _bias;
    private bool _enableMotor;
    private float _gamma;
    private float _impulse;
    private Vector2 _localXAxis;
    private Vector2 _localYAxisA;
    private float _mass;
    private float _maxMotorTorque;
    private float _motorImpulse;
    private float _motorMass;
    private float _motorSpeed;
    private float _sAx;
    private float _sAy;
    private float _sBx;
    private float _sBy;
    private float _springImpulse;
    private float _springMass;

    internal LineJoint() => this.JointType = JointType.Line;

    public LineJoint(Body bA, Body bB, Vector2 anchor, Vector2 axis)
      : base(bA, bB)
    {
      this.JointType = JointType.Line;
      this.LocalAnchorA = bA.GetLocalPoint(anchor);
      this.LocalAnchorB = bB.GetLocalPoint(anchor);
      this.LocalXAxis = bA.GetLocalVector(axis);
    }

    public Vector2 LocalAnchorA { get; set; }

    public Vector2 LocalAnchorB { get; set; }

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
        Body bodyA = this.BodyA;
        Body bodyB = this.BodyB;
        Vector2 worldPoint = bodyA.GetWorldPoint(this.LocalAnchorA);
        return Vector2.Dot(bodyB.GetWorldPoint(this.LocalAnchorB) - worldPoint, bodyA.GetWorldVector(this.LocalXAxis));
      }
    }

    public float JointSpeed
    {
      get => this.BodyB.AngularVelocityInternal - this.BodyA.AngularVelocityInternal;
    }

    public bool MotorEnabled
    {
      get => this._enableMotor;
      set
      {
        this.BodyA.Awake = true;
        this.BodyB.Awake = true;
        this._enableMotor = value;
      }
    }

    public float MotorSpeed
    {
      set
      {
        this.BodyA.Awake = true;
        this.BodyB.Awake = true;
        this._motorSpeed = value;
      }
      get => this._motorSpeed;
    }

    public float MaxMotorTorque
    {
      set
      {
        this.BodyA.Awake = true;
        this.BodyB.Awake = true;
        this._maxMotorTorque = value;
      }
      get => this._maxMotorTorque;
    }

    public float Frequency { get; set; }

    public float DampingRatio { get; set; }

    public Vector2 LocalXAxis
    {
      get => this._localXAxis;
      set
      {
        this._localXAxis = value;
        this._localYAxisA = MathUtils.Cross(1f, this._localXAxis);
      }
    }

    public override Vector2 GetReactionForce(float invDt)
    {
      return invDt * (this._impulse * this._ay + this._springImpulse * this._ax);
    }

    public override float GetReactionTorque(float invDt) => invDt * this._motorImpulse;

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
      this._ay = MathUtils.Multiply(ref transform1.R, this._localYAxisA);
      this._sAy = MathUtils.Cross(vector2_2 + vector2_1, this._ay);
      this._sBy = MathUtils.Cross(a, this._ay);
      this._mass = (float) ((double) this.InvMassA + (double) this.InvMassB + (double) this.InvIA * (double) this._sAy * (double) this._sAy + (double) this.InvIB * (double) this._sBy * (double) this._sBy);
      if ((double) this._mass > 0.0)
        this._mass = 1f / this._mass;
      this._springMass = 0.0f;
      if ((double) this.Frequency > 0.0)
      {
        this._ax = MathUtils.Multiply(ref transform1.R, this.LocalXAxis);
        this._sAx = MathUtils.Cross(vector2_2 + vector2_1, this._ax);
        this._sBx = MathUtils.Cross(a, this._ax);
        float num1 = (float) ((double) this.InvMassA + (double) this.InvMassB + (double) this.InvIA * (double) this._sAx * (double) this._sAx + (double) this.InvIB * (double) this._sBx * (double) this._sBx);
        if ((double) num1 > 0.0)
        {
          this._springMass = 1f / num1;
          float num2 = Vector2.Dot(vector2_2, this._ax);
          float num3 = 6.28318548f * this.Frequency;
          float num4 = 2f * this._springMass * this.DampingRatio * num3;
          float num5 = this._springMass * num3 * num3;
          this._gamma = step.dt * (num4 + step.dt * num5);
          if ((double) this._gamma > 0.0)
            this._gamma = 1f / this._gamma;
          this._bias = num2 * step.dt * num5 * this._gamma;
          this._springMass = num1 + this._gamma;
          if ((double) this._springMass > 0.0)
            this._springMass = 1f / this._springMass;
        }
      }
      else
      {
        this._springImpulse = 0.0f;
        this._springMass = 0.0f;
      }
      if (this._enableMotor)
      {
        this._motorMass = this.InvIA + this.InvIB;
        if ((double) this._motorMass > 0.0)
          this._motorMass = 1f / this._motorMass;
      }
      else
      {
        this._motorMass = 0.0f;
        this._motorImpulse = 0.0f;
      }
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        this._springImpulse *= step.dtRatio;
        this._motorImpulse *= step.dtRatio;
        Vector2 vector2_3 = this._impulse * this._ay + this._springImpulse * this._ax;
        float num6 = (float) ((double) this._impulse * (double) this._sAy + (double) this._springImpulse * (double) this._sAx) + this._motorImpulse;
        float num7 = (float) ((double) this._impulse * (double) this._sBy + (double) this._springImpulse * (double) this._sBx) + this._motorImpulse;
        bodyA.LinearVelocityInternal -= this.InvMassA * vector2_3;
        bodyA.AngularVelocityInternal -= this.InvIA * num6;
        bodyB.LinearVelocityInternal += this.InvMassB * vector2_3;
        bodyB.AngularVelocityInternal += this.InvIB * num7;
      }
      else
      {
        this._impulse = 0.0f;
        this._springImpulse = 0.0f;
        this._motorImpulse = 0.0f;
      }
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Vector2 linearVelocity = bodyA.LinearVelocity;
      float velocityInternal1 = bodyA.AngularVelocityInternal;
      Vector2 velocityInternal2 = bodyB.LinearVelocityInternal;
      float velocityInternal3 = bodyB.AngularVelocityInternal;
      float num1 = (float) (-(double) this._springMass * ((double) Vector2.Dot(this._ax, velocityInternal2 - linearVelocity) + (double) this._sBx * (double) velocityInternal3 - (double) this._sAx * (double) velocityInternal1 + (double) this._bias + (double) this._gamma * (double) this._springImpulse));
      this._springImpulse += num1;
      Vector2 vector2_1 = num1 * this._ax;
      float num2 = num1 * this._sAx;
      float num3 = num1 * this._sBx;
      Vector2 vector2_2 = linearVelocity - this.InvMassA * vector2_1;
      float num4 = velocityInternal1 - this.InvIA * num2;
      Vector2 vector2_3 = velocityInternal2 + this.InvMassB * vector2_1;
      float num5 = velocityInternal3 + this.InvIB * num3;
      float num6 = -this._motorMass * (num5 - num4 - this._motorSpeed);
      float motorImpulse = this._motorImpulse;
      float high = step.dt * this._maxMotorTorque;
      this._motorImpulse = MathUtils.Clamp(this._motorImpulse + num6, -high, high);
      float num7 = this._motorImpulse - motorImpulse;
      float num8 = num4 - this.InvIA * num7;
      float num9 = num5 + this.InvIB * num7;
      float num10 = this._mass * (float) -((double) Vector2.Dot(this._ay, vector2_3 - vector2_2) + (double) this._sBy * (double) num9 - (double) this._sAy * (double) num8);
      this._impulse += num10;
      Vector2 vector2_4 = num10 * this._ay;
      float num11 = num10 * this._sAy;
      float num12 = num10 * this._sBy;
      Vector2 vector2_5 = vector2_2 - this.InvMassA * vector2_4;
      float num13 = num8 - this.InvIA * num11;
      Vector2 vector2_6 = vector2_3 + this.InvMassB * vector2_4;
      float num14 = num9 + this.InvIB * num12;
      bodyA.LinearVelocityInternal = vector2_5;
      bodyA.AngularVelocityInternal = num13;
      bodyB.LinearVelocityInternal = vector2_6;
      bodyB.AngularVelocityInternal = num14;
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Vector2 c1 = bodyA.Sweep.C;
      float a1 = bodyA.Sweep.A;
      Vector2 c2 = bodyB.Sweep.C;
      float a2 = bodyB.Sweep.A;
      Mat22 A1 = new Mat22(a1);
      Mat22 A2 = new Mat22(a2);
      Vector2 vector2_1 = MathUtils.Multiply(ref A1, this.LocalAnchorA - this.LocalCenterA);
      Vector2 a3 = MathUtils.Multiply(ref A2, this.LocalAnchorB - this.LocalCenterB);
      Vector2 vector2_2 = c2 + a3 - c1 - vector2_1;
      Vector2 b = MathUtils.Multiply(ref A1, this._localYAxisA);
      float num1 = MathUtils.Cross(vector2_2 + vector2_1, b);
      float num2 = MathUtils.Cross(a3, b);
      float num3 = Vector2.Dot(vector2_2, b);
      float num4 = (float) ((double) this.InvMassA + (double) this.InvMassB + (double) this.InvIA * (double) this._sAy * (double) this._sAy + (double) this.InvIB * (double) this._sBy * (double) this._sBy);
      float num5 = (double) num4 == 0.0 ? 0.0f : -num3 / num4;
      Vector2 vector2_3 = num5 * b;
      float num6 = num5 * num1;
      float num7 = num5 * num2;
      Vector2 vector2_4 = c1 - this.InvMassA * vector2_3;
      float num8 = a1 - this.InvIA * num6;
      Vector2 vector2_5 = c2 + this.InvMassB * vector2_3;
      float num9 = a2 + this.InvIB * num7;
      bodyA.Sweep.C = vector2_4;
      bodyA.Sweep.A = num8;
      bodyB.Sweep.C = vector2_5;
      bodyB.Sweep.A = num9;
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) Math.Abs(num3) <= 0.004999999888241291;
    }

    public float GetMotorTorque(float invDt) => invDt * this._motorImpulse;
  }
}
