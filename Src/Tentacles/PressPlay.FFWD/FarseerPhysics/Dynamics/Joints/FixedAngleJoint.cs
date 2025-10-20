// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedAngleJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedAngleJoint : Joint
  {
    public float BiasFactor;
    public float MaxImpulse;
    public float Softness;
    private float _bias;
    private float _jointError;
    private float _massFactor;
    private float _targetAngle;

    public FixedAngleJoint(Body bodyA)
      : base(bodyA)
    {
      this.JointType = JointType.FixedAngle;
      this.TargetAngle = 0.0f;
      this.BiasFactor = 0.2f;
      this.Softness = 0.0f;
      this.MaxImpulse = float.MaxValue;
    }

    public float TargetAngle
    {
      get => this._targetAngle;
      set
      {
        if ((double) value == (double) this._targetAngle)
          return;
        this._targetAngle = value;
        this.WakeBodies();
      }
    }

    public override Vector2 WorldAnchorA => this.BodyA.Position;

    public override Vector2 WorldAnchorB
    {
      get => this.BodyA.Position;
      set
      {
      }
    }

    public override Vector2 GetReactionForce(float inv_dt) => Vector2.Zero;

    public override float GetReactionTorque(float inv_dt) => 0.0f;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      this._jointError = this.BodyA.Sweep.A - this.TargetAngle;
      this._bias = -this.BiasFactor * step.inv_dt * this._jointError;
      this._massFactor = (1f - this.Softness) / this.BodyA.InvI;
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      float num = (this._bias - this.BodyA.AngularVelocity) * this._massFactor;
      this.BodyA.AngularVelocity += this.BodyA.InvI * (float) Math.Sign(num) * Math.Min(Math.Abs(num), this.MaxImpulse);
    }

    internal override bool SolvePositionConstraints() => true;
  }
}
