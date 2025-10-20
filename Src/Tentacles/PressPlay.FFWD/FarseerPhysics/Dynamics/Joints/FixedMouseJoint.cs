// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.FixedMouseJoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public class FixedMouseJoint : Joint
  {
    public Vector2 LocalAnchorA;
    private Vector2 _C;
    private float _beta;
    private float _gamma;
    private Vector2 _impulse;
    private Mat22 _mass;
    private Vector2 _worldAnchor;

    public FixedMouseJoint(Body body, Vector2 worldAnchor)
      : base(body)
    {
      this.JointType = JointType.FixedMouse;
      this.Frequency = 5f;
      this.DampingRatio = 0.7f;
      this.BodyA.GetTransform(out Transform _);
      this._worldAnchor = worldAnchor;
      this.LocalAnchorA = this.BodyA.GetLocalPoint(worldAnchor);
    }

    public override Vector2 WorldAnchorA => this.BodyA.GetWorldPoint(this.LocalAnchorA);

    public override Vector2 WorldAnchorB
    {
      get => this._worldAnchor;
      set
      {
        this.BodyA.Awake = true;
        this._worldAnchor = value;
      }
    }

    public float MaxForce { get; set; }

    public float Frequency { get; set; }

    public float DampingRatio { get; set; }

    public override Vector2 GetReactionForce(float inv_dt) => inv_dt * this._impulse;

    public override float GetReactionTorque(float inv_dt) => inv_dt * 0.0f;

    internal override void InitVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      float mass = bodyA.Mass;
      float num1 = 6.28318548f * this.Frequency;
      float num2 = 2f * mass * this.DampingRatio * num1;
      float num3 = mass * (num1 * num1);
      this._gamma = step.dt * (num2 + step.dt * num3);
      if ((double) this._gamma != 0.0)
        this._gamma = 1f / this._gamma;
      this._beta = step.dt * num3 * this._gamma;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      float invMass = bodyA.InvMass;
      float invI = bodyA.InvI;
      Mat22 A = new Mat22(new Vector2(invMass, 0.0f), new Vector2(0.0f, invMass));
      Mat22 B = new Mat22(new Vector2(invI * a.Y * a.Y, -invI * a.X * a.Y), new Vector2(-invI * a.X * a.Y, invI * a.X * a.X));
      Mat22 R;
      Mat22.Add(ref A, ref B, out R);
      R.Col1.X += this._gamma;
      R.Col2.Y += this._gamma;
      this._mass = R.Inverse;
      this._C = bodyA.Sweep.C + a - this._worldAnchor;
      bodyA.AngularVelocityInternal *= 0.98f;
      this._impulse *= step.dtRatio;
      bodyA.LinearVelocityInternal += invMass * this._impulse;
      bodyA.AngularVelocityInternal += invI * MathUtils.Cross(a, this._impulse);
    }

    internal override void SolveVelocityConstraints(ref TimeStep step)
    {
      Body bodyA = this.BodyA;
      Transform transform;
      bodyA.GetTransform(out transform);
      Vector2 a = MathUtils.Multiply(ref transform.R, this.LocalAnchorA - bodyA.LocalCenter);
      Vector2 vector2 = MathUtils.Multiply(ref this._mass, -(bodyA.LinearVelocityInternal + MathUtils.Cross(bodyA.AngularVelocityInternal, a) + this._beta * this._C + this._gamma * this._impulse));
      Vector2 impulse = this._impulse;
      this._impulse += vector2;
      float num = step.dt * this.MaxForce;
      if ((double) this._impulse.LengthSquared() > (double) num * (double) num)
        this._impulse *= num / this._impulse.Length();
      Vector2 b = this._impulse - impulse;
      bodyA.LinearVelocityInternal += bodyA.InvMass * b;
      bodyA.AngularVelocityInternal += bodyA.InvI * MathUtils.Cross(a, b);
    }

    internal override bool SolvePositionConstraints() => true;
  }
}
