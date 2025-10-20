// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.PulleyJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class PulleyJoint : Joint
  {
    public Vector2 GroundAnchorA;
    public Vector2 GroundAnchorB;
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    public float MinPulleyLength = 2f;
    private float _ant;
    private float _impulse;
    private float _lengthA;
    private float _lengthB;
    private float _limitImpulse1;
    private float _limitImpulse2;
    private float _limitMass1;
    private float _limitMass2;
    private LimitState _limitState1;
    private LimitState _limitState2;
    private float _maxLengthA;
    private float _maxLengthB;
    private float _pulleyMass;
    private LimitState _state;
    private Vector2 _u1;
    private Vector2 _u2;

    internal PulleyJoint() => this.JointType = JointType.Pulley;

    public PulleyJoint(
      Body bodyA,
      Body bodyB,
      Vector2 groundAnchorA,
      Vector2 groundAnchorB,
      Vector2 localAnchorA,
      Vector2 localAnchorB,
      float ratio)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Pulley;
      this.GroundAnchorA = groundAnchorA;
      this.GroundAnchorB = groundAnchorB;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this._lengthA = (this.BodyA.GetWorldPoint(localAnchorA) - groundAnchorA).Length();
      this._lengthB = (this.BodyB.GetWorldPoint(localAnchorB) - groundAnchorB).Length();
      this.Ratio = ratio;
      float num = this._lengthA + this.Ratio * this._lengthB;
      this.MaxLengthA = num - this.Ratio * this.MinPulleyLength;
      this.MaxLengthB = (num - this.MinPulleyLength) / this.Ratio;
      this._ant = this._lengthA + this.Ratio * this._lengthB;
      this.MaxLengthA = Math.Min(this.MaxLengthA, this._ant - this.Ratio * this.MinPulleyLength);
      this.MaxLengthB = Math.Min(this.MaxLengthB, (this._ant - this.MinPulleyLength) / this.Ratio);
      this._impulse = 0.0f;
      this._limitImpulse1 = 0.0f;
      this._limitImpulse2 = 0.0f;
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public float LengthA
    {
      get => (this.BodyA.GetWorldPoint(this.LocalAnchorA) - this.GroundAnchorA).Length();
      set => this._lengthA = value;
    }

    public float LengthB
    {
      get => (this.BodyB.GetWorldPoint(this.LocalAnchorB) - this.GroundAnchorB).Length();
      set => this._lengthB = value;
    }

    public float Ratio { get; set; }

    public float MaxLengthA
    {
      get => this._maxLengthA;
      set => this._maxLengthA = value;
    }

    public float MaxLengthB
    {
      get => this._maxLengthB;
      set => this._maxLengthB = value;
    }

    public override Vector2 GetReactionForce(float inv_dt)
    {
      Vector2 vector2 = this._impulse * this._u2;
      return inv_dt * vector2;
    }

    public override float GetReactionTorque(float inv_dt) => 0.0f;

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
      Vector2 vector2_1 = bodyA.Sweep.C + a1;
      Vector2 vector2_2 = bodyB.Sweep.C + a2;
      Vector2 groundAnchorA = this.GroundAnchorA;
      Vector2 groundAnchorB = this.GroundAnchorB;
      this._u1 = vector2_1 - groundAnchorA;
      this._u2 = vector2_2 - groundAnchorB;
      float num1 = this._u1.Length();
      float num2 = this._u2.Length();
      if ((double) num1 > 0.004999999888241291)
        this._u1 *= 1f / num1;
      else
        this._u1 = Vector2.Zero;
      if ((double) num2 > 0.004999999888241291)
        this._u2 *= 1f / num2;
      else
        this._u2 = Vector2.Zero;
      if ((double) this._ant - (double) num1 - (double) this.Ratio * (double) num2 > 0.0)
      {
        this._state = LimitState.Inactive;
        this._impulse = 0.0f;
      }
      else
        this._state = LimitState.AtUpper;
      if ((double) num1 < (double) this.MaxLengthA)
      {
        this._limitState1 = LimitState.Inactive;
        this._limitImpulse1 = 0.0f;
      }
      else
        this._limitState1 = LimitState.AtUpper;
      if ((double) num2 < (double) this.MaxLengthB)
      {
        this._limitState2 = LimitState.Inactive;
        this._limitImpulse2 = 0.0f;
      }
      else
        this._limitState2 = LimitState.AtUpper;
      float num3 = MathUtils.Cross(a1, this._u1);
      float num4 = MathUtils.Cross(a2, this._u2);
      this._limitMass1 = bodyA.InvMass + bodyA.InvI * num3 * num3;
      this._limitMass2 = bodyB.InvMass + bodyB.InvI * num4 * num4;
      this._pulleyMass = this._limitMass1 + this.Ratio * this.Ratio * this._limitMass2;
      this._limitMass1 = 1f / this._limitMass1;
      this._limitMass2 = 1f / this._limitMass2;
      this._pulleyMass = 1f / this._pulleyMass;
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        this._limitImpulse1 *= step.dtRatio;
        this._limitImpulse2 *= step.dtRatio;
        Vector2 b1 = (float) -((double) this._impulse + (double) this._limitImpulse1) * this._u1;
        Vector2 b2 = (-this.Ratio * this._impulse - this._limitImpulse2) * this._u2;
        bodyA.LinearVelocityInternal += bodyA.InvMass * b1;
        bodyA.AngularVelocityInternal += bodyA.InvI * MathUtils.Cross(a1, b1);
        bodyB.LinearVelocityInternal += bodyB.InvMass * b2;
        bodyB.AngularVelocityInternal += bodyB.InvI * MathUtils.Cross(a2, b2);
      }
      else
      {
        this._impulse = 0.0f;
        this._limitImpulse1 = 0.0f;
        this._limitImpulse2 = 0.0f;
      }
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      if (this._state == LimitState.AtUpper)
      {
        Vector2 vector2_1 = bodyA.LinearVelocityInternal + MathUtils.Cross(bodyA.AngularVelocityInternal, a1);
        Vector2 vector2_2 = bodyB.LinearVelocityInternal + MathUtils.Cross(bodyB.AngularVelocityInternal, a2);
        float num1 = this._pulleyMass * (float) -(-(double) Vector2.Dot(this._u1, vector2_1) - (double) this.Ratio * (double) Vector2.Dot(this._u2, vector2_2));
        float impulse = this._impulse;
        this._impulse = Math.Max(0.0f, this._impulse + num1);
        float num2 = this._impulse - impulse;
        Vector2 b1 = -num2 * this._u1;
        Vector2 b2 = -this.Ratio * num2 * this._u2;
        bodyA.LinearVelocityInternal += bodyA.InvMass * b1;
        bodyA.AngularVelocityInternal += bodyA.InvI * MathUtils.Cross(a1, b1);
        bodyB.LinearVelocityInternal += bodyB.InvMass * b2;
        bodyB.AngularVelocityInternal += bodyB.InvI * MathUtils.Cross(a2, b2);
      }
      if (this._limitState1 == LimitState.AtUpper)
      {
        float num = -this._limitMass1 * -Vector2.Dot(this._u1, bodyA.LinearVelocityInternal + MathUtils.Cross(bodyA.AngularVelocityInternal, a1));
        float limitImpulse1 = this._limitImpulse1;
        this._limitImpulse1 = Math.Max(0.0f, this._limitImpulse1 + num);
        Vector2 b = -(this._limitImpulse1 - limitImpulse1) * this._u1;
        bodyA.LinearVelocityInternal += bodyA.InvMass * b;
        bodyA.AngularVelocityInternal += bodyA.InvI * MathUtils.Cross(a1, b);
      }
      if (this._limitState2 != LimitState.AtUpper)
        return;
      float num3 = -this._limitMass2 * -Vector2.Dot(this._u2, bodyB.LinearVelocityInternal + MathUtils.Cross(bodyB.AngularVelocityInternal, a2));
      float limitImpulse2 = this._limitImpulse2;
      this._limitImpulse2 = Math.Max(0.0f, this._limitImpulse2 + num3);
      Vector2 b3 = -(this._limitImpulse2 - limitImpulse2) * this._u2;
      bodyB.LinearVelocityInternal += bodyB.InvMass * b3;
      bodyB.AngularVelocityInternal += bodyB.InvI * MathUtils.Cross(a2, b3);
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Vector2 groundAnchorA = this.GroundAnchorA;
      Vector2 groundAnchorB = this.GroundAnchorB;
      float val1 = 0.0f;
      if (this._state == LimitState.AtUpper)
      {
        Transform transform1;
        bodyA.GetTransform(out transform1);
        Transform transform2;
        bodyB.GetTransform(out transform2);
        Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
        Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
        Vector2 vector2_1 = bodyA.Sweep.C + a1;
        Vector2 vector2_2 = bodyB.Sweep.C + a2;
        this._u1 = vector2_1 - groundAnchorA;
        this._u2 = vector2_2 - groundAnchorB;
        float num1 = this._u1.Length();
        float num2 = this._u2.Length();
        if ((double) num1 > 0.004999999888241291)
          this._u1 *= 1f / num1;
        else
          this._u1 = Vector2.Zero;
        if ((double) num2 > 0.004999999888241291)
          this._u2 *= 1f / num2;
        else
          this._u2 = Vector2.Zero;
        float num3 = (float) ((double) this._ant - (double) num1 - (double) this.Ratio * (double) num2);
        val1 = Math.Max(val1, -num3);
        float num4 = -this._pulleyMass * MathUtils.Clamp(num3 + 0.005f, -0.2f, 0.0f);
        Vector2 b1 = -num4 * this._u1;
        Vector2 b2 = -this.Ratio * num4 * this._u2;
        bodyA.Sweep.C += bodyA.InvMass * b1;
        bodyA.Sweep.A += bodyA.InvI * MathUtils.Cross(a1, b1);
        bodyB.Sweep.C += bodyB.InvMass * b2;
        bodyB.Sweep.A += bodyB.InvI * MathUtils.Cross(a2, b2);
        bodyA.SynchronizeTransform();
        bodyB.SynchronizeTransform();
      }
      if (this._limitState1 == LimitState.AtUpper)
      {
        Transform transform;
        bodyA.GetTransform(out transform);
        Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
        this._u1 = bodyA.Sweep.C + a - groundAnchorA;
        float num5 = this._u1.Length();
        if ((double) num5 > 0.004999999888241291)
          this._u1 *= 1f / num5;
        else
          this._u1 = Vector2.Zero;
        float num6 = this.MaxLengthA - num5;
        val1 = Math.Max(val1, -num6);
        Vector2 b = -(-this._limitMass1 * MathUtils.Clamp(num6 + 0.005f, -0.2f, 0.0f)) * this._u1;
        bodyA.Sweep.C += bodyA.InvMass * b;
        bodyA.Sweep.A += bodyA.InvI * MathUtils.Cross(a, b);
        bodyA.SynchronizeTransform();
      }
      if (this._limitState2 == LimitState.AtUpper)
      {
        Transform transform;
        bodyB.GetTransform(out transform);
        Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorB - bodyB.LocalCenter);
        this._u2 = bodyB.Sweep.C + a - groundAnchorB;
        float num7 = this._u2.Length();
        if ((double) num7 > 0.004999999888241291)
          this._u2 *= 1f / num7;
        else
          this._u2 = Vector2.Zero;
        float num8 = this.MaxLengthB - num7;
        val1 = Math.Max(val1, -num8);
        Vector2 b = -(-this._limitMass2 * MathUtils.Clamp(num8 + 0.005f, -0.2f, 0.0f)) * this._u2;
        bodyB.Sweep.C += bodyB.InvMass * b;
        bodyB.Sweep.A += bodyB.InvI * MathUtils.Cross(a, b);
        bodyB.SynchronizeTransform();
      }
      return (double) val1 < 0.004999999888241291;
    }
  }
}
