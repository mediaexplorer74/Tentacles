// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.SliderJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class SliderJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private float _bias;
    private float _gamma;
    private float _impulse;
    private float _mass;
    private Vector2 _u;

    internal SliderJoint() => this.JointType = JointType.Slider;

    public SliderJoint(
      Body bodyA,
      Body bodyB,
      Vector2 localAnchorA,
      Vector2 localAnchorB,
      float minLength,
      float maxlength)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Slider;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this.MaxLength = maxlength;
      this.MinLength = minLength;
    }

    public float MaxLength { get; set; }

    public float MinLength { get; set; }

    public float Frequency { get; set; }

    public float DampingRatio { get; set; }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this.BodyB.GetWorldPoint(this.LocalAnchorB);
      set
      {
      }
    }

    public override Vector2 GetReactionForce(float inv_dt) => inv_dt * this._impulse * this._u;

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
      this._u = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float num1 = this._u.Length();
      if ((double) num1 < (double) this.MaxLength && (double) num1 > (double) this.MinLength)
        return;
      if ((double) num1 > 0.004999999888241291)
        this._u *= 1f / num1;
      else
        this._u = Vector2.Zero;
      float num2 = MathUtils.Cross(a1, this._u);
      float num3 = MathUtils.Cross(a2, this._u);
      float num4 = (float) ((double) bodyA.InvMass + (double) bodyA.InvI * (double) num2 * (double) num2 + (double) bodyB.InvMass + (double) bodyB.InvI * (double) num3 * (double) num3);
      this._mass = (double) num4 != 0.0 ? 1f / num4 : 0.0f;
      if ((double) this.Frequency > 0.0)
      {
        float num5 = num1 - this.MaxLength;
        float num6 = 6.28318548f * this.Frequency;
        float num7 = 2f * this._mass * this.DampingRatio * num6;
        float num8 = this._mass * num6 * num6;
        this._gamma = step.dt * (num7 + step.dt * num8);
        this._gamma = (double) this._gamma != 0.0 ? 1f / this._gamma : 0.0f;
        this._bias = num5 * step.dt * num8 * this._gamma;
        this._mass = num4 + this._gamma;
        this._mass = (double) this._mass != 0.0 ? 1f / this._mass : 0.0f;
      }
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        Vector2 b = this._impulse * this._u;
        bodyA.LinearVelocityInternal -= bodyA.InvMass * b;
        bodyA.AngularVelocityInternal -= bodyA.InvI * MathUtils.Cross(a1, b);
        bodyB.LinearVelocityInternal += bodyB.InvMass * b;
        bodyB.AngularVelocityInternal += bodyB.InvI * MathUtils.Cross(a2, b);
      }
      else
        this._impulse = 0.0f;
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
      float num1 = (bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1).Length();
      if ((double) num1 < (double) this.MaxLength && (double) num1 > (double) this.MinLength)
        return;
      Vector2 vector2 = bodyA.LinearVelocityInternal + MathUtils.Cross(bodyA.AngularVelocityInternal, a1);
      float num2 = (float) (-(double) this._mass * ((double) Vector2.Dot(this._u, bodyB.LinearVelocityInternal + MathUtils.Cross(bodyB.AngularVelocityInternal, a2) - vector2) + (double) this._bias + (double) this._gamma * (double) this._impulse));
      this._impulse += num2;
      Vector2 b = num2 * this._u;
      bodyA.LinearVelocityInternal -= bodyA.InvMass * b;
      bodyA.AngularVelocityInternal -= bodyA.InvI * MathUtils.Cross(a1, b);
      bodyB.LinearVelocityInternal += bodyB.InvMass * b;
      bodyB.AngularVelocityInternal += bodyB.InvI * MathUtils.Cross(a2, b);
    }

    internal override bool SolvePositionConstraints()
    {
      if ((double) this.Frequency > 0.0)
        return true;
      Body bodyA = this.BodyA;
      Body bodyB = this.BodyB;
      Transform transform1;
      bodyA.GetTransform(out transform1);
      Transform transform2;
      bodyB.GetTransform(out transform2);
      Vector2 a1 = MathUtils.Multiply(ref transform1.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref transform2.R, this.LocalAnchorB - bodyB.LocalCenter);
      Vector2 vector2_1 = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float num1 = vector2_1.Length();
      if ((double) num1 < (double) this.MaxLength && (double) num1 > (double) this.MinLength || (double) num1 == 0.0)
        return true;
      Vector2 vector2_2 = vector2_1 / num1;
      float num2 = MathUtils.Clamp(num1 - this.MaxLength, -0.2f, 0.2f);
      float num3 = -this._mass * num2;
      this._u = vector2_2;
      Vector2 b = num3 * this._u;
      bodyA.Sweep.C -= bodyA.InvMass * b;
      bodyA.Sweep.A -= bodyA.InvI * MathUtils.Cross(a1, b);
      bodyB.Sweep.C += bodyB.InvMass * b;
      bodyB.Sweep.A += bodyB.InvI * MathUtils.Cross(a2, b);
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) Math.Abs(num2) < 0.004999999888241291;
    }
  }
}
