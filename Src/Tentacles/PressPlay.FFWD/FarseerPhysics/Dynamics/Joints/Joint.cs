// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Joints.Joint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Joints
{
  public abstract class Joint
  {
    public float Breakpoint = float.MaxValue;
    internal JointEdge EdgeA = new JointEdge();
    internal JointEdge EdgeB = new JointEdge();
    public bool Enabled = true;
    protected float InvIA;
    protected float InvIB;
    protected float InvMassA;
    protected float InvMassB;
    internal bool IslandFlag;
    protected Vector2 LocalCenterA;
    protected Vector2 LocalCenterB;

    protected Joint()
    {
    }

    protected Joint(Body body, Body bodyB)
    {
      this.BodyA = body;
      this.BodyB = bodyB;
      this.CollideConnected = false;
    }

    protected Joint(Body body)
    {
      this.BodyA = body;
      this.CollideConnected = false;
    }

    public JointType JointType { get; protected set; }

    public Body BodyA { get; set; }

    public Body BodyB { get; set; }

    public abstract Vector2 WorldAnchorA { get; }

    public abstract Vector2 WorldAnchorB { get; set; }

    public object UserData { get; set; }

    public bool Active => this.BodyA.Enabled && this.BodyB.Enabled;

    public bool CollideConnected { get; set; }

    public event Action<Joint, float> Broke;

    public abstract Vector2 GetReactionForce(float inv_dt);

    public abstract float GetReactionTorque(float inv_dt);

    protected void WakeBodies()
    {
      this.BodyA.Awake = true;
      if (this.BodyB == null)
        return;
      this.BodyB.Awake = true;
    }

    public bool IsFixedType()
    {
      return this.JointType == JointType.FixedRevolute || this.JointType == JointType.FixedDistance || this.JointType == JointType.FixedPrismatic || this.JointType == JointType.FixedLine || this.JointType == JointType.FixedMouse || this.JointType == JointType.FixedAngle || this.JointType == JointType.FixedFriction;
    }

    internal abstract void InitVelocityConstraints(ref TimeStep step);

    internal void Validate(float invDT)
    {
      if (!this.Enabled)
        return;
      float num = this.GetReactionForce(invDT).Length();
      if ((double) Math.Abs(num) <= (double) this.Breakpoint)
        return;
      this.Enabled = false;
      if (this.Broke == null)
        return;
      this.Broke(this, num);
    }

    internal abstract void SolveVelocityConstraints(ref TimeStep step);

    internal abstract bool SolvePositionConstraints();
  }
}
