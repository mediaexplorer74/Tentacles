// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.RopeJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class RopeJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private float _impulse;
    private float _length;
    private float _mass;
    private Vector2 _rA;
    private Vector2 _rB;
    private LimitState _state;
    private Vector2 _u;

    internal RopeJoint() => this.JointType = JointType.Rope;

    public RopeJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Rope;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this.MaxLength = (this.WorldAnchorB - this.WorldAnchorA).Length();
      this._mass = 0.0f;
      this._impulse = 0.0f;
      this._state = LimitState.Inactive;
      this._length = 0.0f;
    }

    public float MaxLength { get; set; }

    public LimitState State => this._state;

    public override sealed Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override sealed Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public override Vector2 GetReactionForce(float invDt) => invDt * this._impulse * this._u;

    public override float GetReactionTorque(float invDt) => 0.0f;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      this._rA = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      this._rB = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      this._u = bodyB.Sweep.C + this._rB - bodyA.Sweep.C - this._rA;
      this._length = this._u.Length();
      this._state = (double) (this._length - this.MaxLength) <= 0.0 ? LimitState.Inactive : LimitState.AtUpper;
      if ((double) this._length > 0.004999999888241291)
      {
        this._u *= 1f / this._length;
        float num1 = MathUtils.Cross(this._rA, this._u);
        float num2 = MathUtils.Cross(this._rB, this._u);
        float num3 = (float) ((double) bodyA.InvMass + (double) bodyA.InvI * (double) num1 * (double) num1 + (double) bodyB.InvMass + (double) bodyB.InvI * (double) num2 * (double) num2);
        this._mass = (double) num3 != 0.0 ? 1f / num3 : 0.0f;
        if (Settings.EnableWarmstarting)
        {
          this._impulse *= step.dtRatio;
          Vector2 b = this._impulse * this._u;
          bodyA.LinearVelocity -= bodyA.InvMass * b;
          bodyA.AngularVelocity -= bodyA.InvI * MathUtils.Cross(this._rA, b);
          bodyB.LinearVelocity += bodyB.InvMass * b;
          bodyB.AngularVelocity += bodyB.InvI * MathUtils.Cross(this._rB, b);
        }
        else
          this._impulse = 0.0f;
      }
      else
      {
        this._u = Vector2.Zero;
        this._mass = 0.0f;
        this._impulse = 0.0f;
      }
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Vector2 vector2_1 = bodyA.LinearVelocity + MathUtils.Cross(bodyA.AngularVelocity, this._rA);
      Vector2 vector2_2 = bodyB.LinearVelocity + MathUtils.Cross(bodyB.AngularVelocity, this._rB);
      float num1 = this._length - this.MaxLength;
      float num2 = Vector2.Dot(this._u, vector2_2 - vector2_1);
      if ((double) num1 < 0.0)
        num2 += step.inv_dt * num1;
      float num3 = -this._mass * num2;
      float impulse = this._impulse;
      this._impulse = Math.Min(0.0f, this._impulse + num3);
      Vector2 b = (this._impulse - impulse) * this._u;
      bodyA.LinearVelocity -= bodyA.InvMass * b;
      bodyA.AngularVelocity -= bodyA.InvI * MathUtils.Cross(this._rA, b);
      bodyB.LinearVelocity += bodyB.InvMass * b;
      bodyB.AngularVelocity += bodyB.InvI * MathUtils.Cross(this._rB, b);
    }

    internal override bool SolvePositionConstraints()
    {
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      Vector2 vector2 = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float num = vector2.Length();
      vector2.Normalize();
      Vector2 b = -this._mass * MathUtils.Clamp(num - this.MaxLength, 0.0f, 0.2f) * vector2;
      bodyA.Sweep.C -= bodyA.InvMass * b;
      bodyA.Sweep.A -= bodyA.InvI * MathUtils.Cross(a1, b);
      bodyB.Sweep.C += bodyB.InvMass * b;
      bodyB.Sweep.A += bodyB.InvI * MathUtils.Cross(a2, b);
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) num - (double) this.MaxLength < 0.004999999888241291;
    }
  }
}
