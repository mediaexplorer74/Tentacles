// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.DistanceJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class DistanceJoint : Joint
  {
    public Vector2 LocalAnchorA;
    public Vector2 LocalAnchorB;
    private float _bias;
    private float _gamma;
    private float _impulse;
    private float _mass;
    private float _tmpFloat1;
    private Vector2 _tmpVector1;
    private Vector2 _u;

    internal DistanceJoint() => this.JointType = JointType.Distance;

    public DistanceJoint(Body bodyA, Body bodyB, Vector2 localAnchorA, Vector2 localAnchorB)
      : base(bodyA, bodyB)
    {
      this.JointType = JointType.Distance;
      this.LocalAnchorA = localAnchorA;
      this.LocalAnchorB = localAnchorB;
      this.Length = (this.WorldAnchorB - this.WorldAnchorA).Length();
    }

    public float Length { get; set; }

    public float Frequency { get; set; }

    public float DampingRatio { get; set; }

    public override sealed Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override sealed Vector2 WorldAnchorB
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
      Vector2 a1 = MathUtils.Multiply(ref bodyA.Xf.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 a2 = MathUtils.Multiply(ref bodyB.Xf.R, this.LocalAnchorB - bodyB.LocalCenter);
      this._u = bodyB.Sweep.C + a2 - bodyA.Sweep.C - a1;
      float num1 = this._u.Length();
      if ((double) num1 > 0.004999999888241291)
        this._u *= 1f / num1;
      else
        this._u = Vector2.Zero;
      float c1;
      MathUtils.Cross(ref a1, ref this._u, out c1);
      float c2;
      MathUtils.Cross(ref a2, ref this._u, out c2);
      float num2 = (float) ((double) bodyA.InvMass + (double) bodyA.InvI * (double) c1 * (double) c1 + (double) bodyB.InvMass + (double) bodyB.InvI * (double) c2 * (double) c2);
      this._mass = (double) num2 != 0.0 ? 1f / num2 : 0.0f;
      if ((double) this.Frequency > 0.0)
      {
        float num3 = num1 - this.Length;
        float num4 = 6.28318548f * this.Frequency;
        float num5 = 2f * this._mass * this.DampingRatio * num4;
        float num6 = this._mass * num4 * num4;
        this._gamma = step.dt * (num5 + step.dt * num6);
        this._gamma = (double) this._gamma != 0.0 ? 1f / this._gamma : 0.0f;
        this._bias = num3 * step.dt * num6 * this._gamma;
        this._mass = num2 + this._gamma;
        this._mass = (double) this._mass != 0.0 ? 1f / this._mass : 0.0f;
      }
      if (Settings.EnableWarmstarting)
      {
        this._impulse *= step.dtRatio;
        Vector2 b = this._impulse * this._u;
        bodyA.LinearVelocityInternal -= bodyA.InvMass * b;
        MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
        bodyA.AngularVelocityInternal -= bodyA.InvI * this._tmpFloat1;
        bodyB.LinearVelocityInternal += bodyB.InvMass * b;
        MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
        bodyB.AngularVelocityInternal += bodyB.InvI * this._tmpFloat1;
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
      MathUtils.Cross(bodyA.AngularVelocityInternal, ref a1, out this._tmpVector1);
      Vector2 vector2 = bodyA.LinearVelocityInternal + this._tmpVector1;
      MathUtils.Cross(bodyB.AngularVelocityInternal, ref a2, out this._tmpVector1);
      float num = (float) (-(double) this._mass * ((double) Vector2.Dot(this._u, bodyB.LinearVelocityInternal + this._tmpVector1 - vector2) + (double) this._bias + (double) this._gamma * (double) this._impulse));
      this._impulse += num;
      Vector2 b = num * this._u;
      bodyA.LinearVelocityInternal -= bodyA.InvMass * b;
      MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
      bodyA.AngularVelocityInternal -= bodyA.InvI * this._tmpFloat1;
      bodyB.LinearVelocityInternal += bodyB.InvMass * b;
      MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
      bodyB.AngularVelocityInternal += bodyB.InvI * this._tmpFloat1;
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
      if ((double) num1 == 0.0)
        return true;
      Vector2 vector2_2 = vector2_1 / num1;
      float num2 = MathUtils.Clamp(num1 - this.Length, -0.2f, 0.2f);
      float num3 = -this._mass * num2;
      this._u = vector2_2;
      Vector2 b = num3 * this._u;
      bodyA.Sweep.C -= bodyA.InvMass * b;
      MathUtils.Cross(ref a1, ref b, out this._tmpFloat1);
      bodyA.Sweep.A -= bodyA.InvI * this._tmpFloat1;
      bodyB.Sweep.C += bodyB.InvMass * b;
      MathUtils.Cross(ref a2, ref b, out this._tmpFloat1);
      bodyB.Sweep.A += bodyB.InvI * this._tmpFloat1;
      bodyA.SynchronizeTransform();
      bodyB.SynchronizeTransform();
      return (double) Math.Abs(num2) < 0.004999999888241291;
    }
  }
}
