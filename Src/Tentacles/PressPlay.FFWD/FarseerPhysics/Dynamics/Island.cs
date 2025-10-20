// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Island
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class Island
  {
    private const float LinTolSqr = 0.0001f;
    private const float AngTolSqr = 0.0012184697f;
    public Body[] Bodies;
    public int BodyCount;
    public int ContactCount;
    public int JointCount;
    private int _bodyCapacity;
    private int _contactCapacity;
    private ContactManager _contactManager;
    private ContactSolver _contactSolver = new ContactSolver();
    private Contact[] _contacts;
    private int _jointCapacity;
    private Joint[] _joints;
    public float JointUpdateTime;
    private Stopwatch _watch = new Stopwatch();
    private float _tmpTime;

    public void Reset(
      int bodyCapacity,
      int contactCapacity,
      int jointCapacity,
      ContactManager contactManager)
    {
      this._bodyCapacity = bodyCapacity;
      this._contactCapacity = contactCapacity;
      this._jointCapacity = jointCapacity;
      this.BodyCount = 0;
      this.ContactCount = 0;
      this.JointCount = 0;
      this._contactManager = contactManager;
      if (this.Bodies == null || this.Bodies.Length < bodyCapacity)
        this.Bodies = new Body[bodyCapacity];
      if (this._contacts == null || this._contacts.Length < contactCapacity)
        this._contacts = new Contact[contactCapacity * 2];
      if (this._joints != null && this._joints.Length >= jointCapacity)
        return;
      this._joints = new Joint[jointCapacity * 2];
    }

    public void Clear()
    {
      this.BodyCount = 0;
      this.ContactCount = 0;
      this.JointCount = 0;
    }

    public void Solve(ref TimeStep step, ref Vector2 gravity)
    {
      for (int index = 0; index < this.BodyCount; ++index)
      {
        Body body = this.Bodies[index];
        if (body.BodyType == BodyType.Dynamic)
        {
          if (body.IgnoreGravity)
          {
            body.LinearVelocityInternal.X += step.dt * (body.InvMass * body.Force.X);
            body.LinearVelocityInternal.Y += step.dt * (body.InvMass * body.Force.Y);
            body.AngularVelocityInternal += step.dt * body.InvI * body.Torque;
          }
          else
          {
            body.LinearVelocityInternal.X += step.dt * (gravity.X + body.InvMass * body.Force.X);
            body.LinearVelocityInternal.Y += step.dt * (gravity.Y + body.InvMass * body.Force.Y);
            body.AngularVelocityInternal += step.dt * body.InvI * body.Torque;
          }
          body.LinearVelocityInternal *= MathUtils.Clamp((float) (1.0 - (double) step.dt * (double) body.LinearDamping), 0.0f, 1f);
          body.AngularVelocityInternal *= MathUtils.Clamp((float) (1.0 - (double) step.dt * (double) body.AngularDamping), 0.0f, 1f);
        }
      }
      int index1 = -1;
      for (int index2 = 0; index2 < this.ContactCount; ++index2)
      {
        Fixture fixtureA = this._contacts[index2].FixtureA;
        Fixture fixtureB = this._contacts[index2].FixtureB;
        Body body1 = fixtureA.Body;
        Body body2 = fixtureB.Body;
        if (body1.BodyType != BodyType.Static && body2.BodyType != BodyType.Static)
        {
          ++index1;
          Contact contact = this._contacts[index1];
          this._contacts[index1] = this._contacts[index2];
          this._contacts[index2] = contact;
        }
      }
      this._contactSolver.Reset(this._contacts, this.ContactCount, step.dtRatio, Settings.EnableWarmstarting);
      this._contactSolver.InitializeVelocityConstraints();
      if (Settings.EnableWarmstarting)
        this._contactSolver.WarmStart();
      if (Settings.EnableDiagnostics)
      {
        this._watch.Start();
        this._tmpTime = 0.0f;
      }
      for (int index3 = 0; index3 < this.JointCount; ++index3)
      {
        if (this._joints[index3].Enabled)
          this._joints[index3].InitVelocityConstraints(ref step);
      }
      if (Settings.EnableDiagnostics)
        this._tmpTime += (float) this._watch.ElapsedTicks;
      for (int index4 = 0; index4 < Settings.VelocityIterations; ++index4)
      {
        if (Settings.EnableDiagnostics)
          this._watch.Start();
        for (int index5 = 0; index5 < this.JointCount; ++index5)
        {
          Joint joint = this._joints[index5];
          if (joint.Enabled)
          {
            joint.SolveVelocityConstraints(ref step);
            joint.Validate(step.inv_dt);
          }
        }
        if (Settings.EnableDiagnostics)
        {
          this._watch.Stop();
          this._tmpTime += (float) this._watch.ElapsedTicks;
          this._watch.Reset();
        }
        this._contactSolver.SolveVelocityConstraints();
      }
      this._contactSolver.StoreImpulses();
      for (int index6 = 0; index6 < this.BodyCount; ++index6)
      {
        Body body = this.Bodies[index6];
        if (body.BodyType != BodyType.Static)
        {
          float num1 = step.dt * body.LinearVelocityInternal.X;
          float num2 = step.dt * body.LinearVelocityInternal.Y;
          float d = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
          if ((double) d > 4.0)
          {
            float num3 = 2f / (float) Math.Sqrt((double) d);
            body.LinearVelocityInternal.X *= num3;
            body.LinearVelocityInternal.Y *= num3;
          }
          float num4 = step.dt * body.AngularVelocityInternal;
          if ((double) num4 * (double) num4 > 2.4674012660980225)
          {
            float num5 = 1.57079637f / Math.Abs(num4);
            body.AngularVelocityInternal *= num5;
          }
          body.Sweep.C0.X = body.Sweep.C.X;
          body.Sweep.C0.Y = body.Sweep.C.Y;
          body.Sweep.A0 = body.Sweep.A;
          body.Sweep.C.X += step.dt * body.LinearVelocityInternal.X;
          body.Sweep.C.Y += step.dt * body.LinearVelocityInternal.Y;
          body.Sweep.A += step.dt * body.AngularVelocityInternal;
          body.SynchronizeTransform();
        }
      }
      for (int index7 = 0; index7 < Settings.PositionIterations; ++index7)
      {
        bool flag1 = this._contactSolver.SolvePositionConstraints(0.2f);
        bool flag2 = true;
        if (Settings.EnableDiagnostics)
          this._watch.Start();
        for (int index8 = 0; index8 < this.JointCount; ++index8)
        {
          Joint joint = this._joints[index8];
          if (joint.Enabled)
          {
            bool flag3 = joint.SolvePositionConstraints();
            flag2 = flag2 && flag3;
          }
        }
        if (Settings.EnableDiagnostics)
        {
          this._watch.Stop();
          this._tmpTime += (float) this._watch.ElapsedTicks;
          this._watch.Reset();
        }
        if (flag1 && flag2)
          break;
      }
      if (Settings.EnableDiagnostics)
        this.JointUpdateTime = this._tmpTime;
      this.Report(this._contactSolver.Constraints);
      if (!Settings.AllowSleep)
        return;
      float val1 = float.MaxValue;
      for (int index9 = 0; index9 < this.BodyCount; ++index9)
      {
        Body body = this.Bodies[index9];
        if (body.BodyType != BodyType.Static)
        {
          if ((body.Flags & BodyFlags.AutoSleep) == BodyFlags.None)
          {
            body.SleepTime = 0.0f;
            val1 = 0.0f;
          }
          if ((body.Flags & BodyFlags.AutoSleep) == BodyFlags.None || (double) body.AngularVelocityInternal * (double) body.AngularVelocityInternal > 0.0012184696970507503 || (double) Vector2.Dot(body.LinearVelocityInternal, body.LinearVelocityInternal) > 9.9999997473787516E-05)
          {
            body.SleepTime = 0.0f;
            val1 = 0.0f;
          }
          else
          {
            body.SleepTime += step.dt;
            val1 = Math.Min(val1, body.SleepTime);
          }
        }
      }
      if ((double) val1 < 0.5)
        return;
      for (int index10 = 0; index10 < this.BodyCount; ++index10)
        this.Bodies[index10].Awake = false;
    }

    internal void SolveTOI(ref TimeStep subStep)
    {
      this._contactSolver.Reset(this._contacts, this.ContactCount, subStep.dtRatio, false);
      for (int index = 0; index < Settings.TOIPositionIterations && !this._contactSolver.SolvePositionConstraints(0.75f); ++index)
      {
        if (index == Settings.TOIPositionIterations - 1)
          index = index;
      }
      for (int index = 0; index < this.BodyCount; ++index)
      {
        Body body = this.Bodies[index];
        body.Sweep.A0 = body.Sweep.A;
        body.Sweep.C0 = body.Sweep.C;
      }
      this._contactSolver.InitializeVelocityConstraints();
      for (int index = 0; index < Settings.TOIVelocityIterations; ++index)
        this._contactSolver.SolveVelocityConstraints();
      for (int index = 0; index < this.BodyCount; ++index)
      {
        Body body = this.Bodies[index];
        if (body.BodyType != BodyType.Static)
        {
          float num1 = subStep.dt * body.LinearVelocityInternal.X;
          float num2 = subStep.dt * body.LinearVelocityInternal.Y;
          float d = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
          if ((double) d > 4.0)
          {
            float num3 = 1f / (float) Math.Sqrt((double) d);
            float num4 = 2f * subStep.inv_dt;
            body.LinearVelocityInternal.X = num4 * (num1 * num3);
            body.LinearVelocityInternal.Y = num4 * (num2 * num3);
          }
          float num5 = subStep.dt * body.AngularVelocity;
          if ((double) num5 * (double) num5 > 2.4674012660980225)
            body.AngularVelocityInternal = (double) num5 >= 0.0 ? subStep.inv_dt * 1.57079637f : (float) (-(double) subStep.inv_dt * 1.5707963705062866);
          body.Sweep.C.X += subStep.dt * body.LinearVelocityInternal.X;
          body.Sweep.C.Y += subStep.dt * body.LinearVelocityInternal.Y;
          body.Sweep.A += subStep.dt * body.AngularVelocityInternal;
          body.SynchronizeTransform();
        }
      }
      this.Report(this._contactSolver.Constraints);
    }

    public void Add(Body body) => this.Bodies[this.BodyCount++] = body;

    public void Add(Contact contact) => this._contacts[this.ContactCount++] = contact;

    public void Add(Joint joint) => this._joints[this.JointCount++] = joint;

    private void Report(ContactConstraint[] constraints)
    {
      if (this._contactManager == null)
        return;
      for (int index = 0; index < this.ContactCount; ++index)
      {
        Contact contact = this._contacts[index];
        if (contact.FixtureA.AfterCollision != null)
          contact.FixtureA.AfterCollision(contact.FixtureA, contact.FixtureB, contact);
        if (contact.FixtureB.AfterCollision != null)
          contact.FixtureB.AfterCollision(contact.FixtureB, contact.FixtureA, contact);
        if (this._contactManager.PostSolve != null)
        {
          ContactConstraint constraint = constraints[index];
          this._contactManager.PostSolve(contact, constraint);
        }
      }
    }
  }
}
