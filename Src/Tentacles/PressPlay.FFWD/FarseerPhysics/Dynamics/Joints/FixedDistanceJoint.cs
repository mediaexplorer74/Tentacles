// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedDistanceJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedDistanceJoint : Joint
  {
    public Vector2 LocalAnchorA;
    private float _bias;
    private float _gamma;
    private float _impulse;
    private float _mass;
    private Vector2 _u;
    private Vector2 _worldAnchorB;

    public FixedDistanceJoint(Body body, Vector2 bodyAnchor, Vector2 worldAnchor)
      : base(body)
    {
      this.JointType = JointType.FixedDistance;
      this.LocalAnchorA = bodyAnchor;
      this._worldAnchorB = worldAnchor;
      this.Length = (this.WorldAnchorB - this.WorldAnchorA).Length();
    }

    public float Length { get; set; }

    public float Frequency { get; set; }

    public float DampingRatio { get; set; }

    public override sealed Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override sealed Vector2 WorldAnchorB
    {
      get => this._worldAnchorB;
      set => this._worldAnchorB = value;
    }

    public override Vector2 GetReactionForce(float invDt) => invDt * this._impulse * this._u;

    public override float GetReactionTorque(float invDt) => 0.0f;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 worldAnchorB = this._worldAnchorB;
      this._u = worldAnchorB - bodyA.Sweep.C - a;
      float num1 = this._u.Length();
      if ((double) num1 > 0.004999999888241291)
        this._u *= 1f / num1;
      else
        this._u = Vector2.Zero;
      float num2 = MathUtils.Cross(a, this._u);
      float num3 = MathUtils.Cross(worldAnchorB, this._u);
      float num4 = (float) ((double) bodyA.InvMass + (double) bodyA.InvI * (double) num2 * (double) num2 + 0.0 * (double) num3 * (double) num3);
      this._mass = (double) num4 != 0.0 ? 1f / num4 : 0.0f;
      if ((double) this.Frequency > 0.0)
      {
        float num5 = num1 - this.Length;
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
        bodyA.AngularVelocityInternal -= bodyA.InvI * MathUtils.Cross(a, b);
      }
      else
        this._impulse = 0.0f;
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      float num = (float) (-(double) this._mass * ((double) Vector2.Dot(this._u, Vector2.Zero - (bodyA.LinearVelocityInternal + MathUtils.Cross(bodyA.AngularVelocityInternal, a))) + (double) this._bias + (double) this._gamma * (double) this._impulse));
      this._impulse += num;
      Vector2 b = num * this._u;
      bodyA.LinearVelocityInternal -= bodyA.InvMass * b;
      bodyA.AngularVelocityInternal -= bodyA.InvI * MathUtils.Cross(a, b);
    }

    internal override bool SolvePositionConstraints()
    {
      if ((double) this.Frequency > 0.0)
        return true;
      Body bodyA = this.BodyA;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 vector2_1 = this._worldAnchorB - bodyA.Sweep.C - a;
      float num1 = vector2_1.Length();
      if ((double) num1 == 0.0)
        return true;
      Vector2 vector2_2 = vector2_1 / num1;
      float num2 = MathUtils.Clamp(num1 - this.Length, -0.2f, 0.2f);
      float num3 = -this._mass * num2;
      this._u = vector2_2;
      Vector2 b = num3 * this._u;
      bodyA.Sweep.C -= bodyA.InvMass * b;
      bodyA.Sweep.A -= bodyA.InvI * MathUtils.Cross(a, b);
      bodyA.SynchronizeTransform();
      return (double) Math.Abs(num2) < 0.004999999888241291;
    }
  }
}
