// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.World
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using PressPlay.FFWD;
using PressPlay.FFWD.Farseer.Collision;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public class World : IRayCastCallback
  {
    public BodyDelegate BodyAdded;
    public BodyDelegate BodyRemoved;
    internal Queue<Contact> ContactPool = new Queue<Contact>(256);
    public FixtureDelegate FixtureAdded;
    public FixtureDelegate FixtureRemoved;
    internal WorldFlags Flags;
    public JointDelegate JointAdded;
    public JointDelegate JointRemoved;
    public ControllerDelegate ControllerAdded;
    public ControllerDelegate ControllerRemoved;
    private float _invDt0;
    public Island Island = new Island();
    private Body[] _stack = new Body[64];
    private bool _stepComplete;
    private FarseerPhysics.Common.HashSet<Body> _bodyAddList = new FarseerPhysics.Common.HashSet<Body>();
    private FarseerPhysics.Common.HashSet<Body> _bodyRemoveList = new FarseerPhysics.Common.HashSet<Body>();
    private FarseerPhysics.Common.HashSet<Joint> _jointAddList = new FarseerPhysics.Common.HashSet<Joint>();
    private FarseerPhysics.Common.HashSet<Joint> _jointRemoveList = new FarseerPhysics.Common.HashSet<Joint>();
    private TOIInput _input = new TOIInput();
    public bool Enabled = true;
    private Stopwatch _watch = new Stopwatch();
    public Microsoft.Xna.Framework.Vector2 Gravity;
    private FarseerPhysics.Dynamics.RayCastCallback rayCastCallback;

    private World()
    {
      this.Flags = WorldFlags.ClearForces;
      this.ControllerList = new List<Controller>();
      this.BreakableBodyList = new List<BreakableBody>();
      this.BodyList = new List<Body>(32);
      this.JointList = new List<Joint>(32);
    }

    public World(Microsoft.Xna.Framework.Vector2 gravity, AABB span)
      : this()
    {
      this.Gravity = gravity;
      this.ContactManager = new ContactManager((IBroadPhase) new QuadTreeBroadPhase(span));
    }

    public World(Microsoft.Xna.Framework.Vector2 gravity)
      : this()
    {
      this.ContactManager = new ContactManager((IBroadPhase) new DynamicTreeBroadPhase());
      this.Gravity = gravity;
    }

    public List<Controller> ControllerList { get; private set; }

    public List<BreakableBody> BreakableBodyList { get; private set; }

    public float UpdateTime { get; private set; }

    public float ContinuousPhysicsTime { get; private set; }

    public float ControllersUpdateTime { get; private set; }

    public float AddRemoveTime { get; private set; }

    public float ContactsUpdateTime { get; private set; }

    public float SolveUpdateTime { get; private set; }

    public int ProxyCount => this.ContactManager.BroadPhase.ProxyCount;

    public bool AutoClearForces
    {
      set
      {
        if (value)
          this.Flags |= WorldFlags.ClearForces;
        else
          this.Flags &= ~WorldFlags.ClearForces;
      }
      get => (this.Flags & WorldFlags.ClearForces) == WorldFlags.ClearForces;
    }

    public ContactManager ContactManager { get; private set; }

    public List<Body> BodyList { get; private set; }

    public List<Joint> JointList { get; private set; }

    public List<Contact> ContactList => this.ContactManager.ContactList;

    public bool EnableSubStepping
    {
      set
      {
        if (value)
          this.Flags |= WorldFlags.SubStepping;
        else
          this.Flags &= ~WorldFlags.SubStepping;
      }
      get => (this.Flags & WorldFlags.SubStepping) == WorldFlags.SubStepping;
    }

    internal void AddBody(Body body)
    {
      if (this._bodyAddList.Contains(body))
        return;
      this._bodyAddList.Add(body);
    }

    public void RemoveBody(Body body)
    {
      if (this._bodyRemoveList.Contains(body))
        return;
      this._bodyRemoveList.Add(body);
    }

    public void AddJoint(Joint joint)
    {
      if (this._jointAddList.Contains(joint))
        return;
      this._jointAddList.Add(joint);
    }

    private void RemoveJoint(Joint joint, bool doCheck)
    {
      int num = doCheck ? 1 : 0;
      if (this._jointRemoveList.Contains(joint))
        return;
      this._jointRemoveList.Add(joint);
    }

    public void RemoveJoint(Joint joint) => this.RemoveJoint(joint, true);

    public void ProcessChanges()
    {
      this.ProcessAddedBodies();
      this.ProcessAddedJoints();
      this.ProcessRemovedBodies();
      this.ProcessRemovedJoints();
    }

    private void ProcessRemovedJoints()
    {
      if (this._jointRemoveList.Count <= 0)
        return;
      foreach (Joint jointRemove in this._jointRemoveList)
      {
        bool collideConnected = jointRemove.CollideConnected;
        this.JointList.Remove(jointRemove);
        Body bodyA = jointRemove.BodyA;
        Body bodyB = jointRemove.BodyB;
        bodyA.Awake = true;
        if (!jointRemove.IsFixedType())
          bodyB.Awake = true;
        if (jointRemove.EdgeA.Prev != null)
          jointRemove.EdgeA.Prev.Next = jointRemove.EdgeA.Next;
        if (jointRemove.EdgeA.Next != null)
          jointRemove.EdgeA.Next.Prev = jointRemove.EdgeA.Prev;
        if (jointRemove.EdgeA == bodyA.JointList)
          bodyA.JointList = jointRemove.EdgeA.Next;
        jointRemove.EdgeA.Prev = (JointEdge) null;
        jointRemove.EdgeA.Next = (JointEdge) null;
        if (!jointRemove.IsFixedType())
        {
          if (jointRemove.EdgeB.Prev != null)
            jointRemove.EdgeB.Prev.Next = jointRemove.EdgeB.Next;
          if (jointRemove.EdgeB.Next != null)
            jointRemove.EdgeB.Next.Prev = jointRemove.EdgeB.Prev;
          if (jointRemove.EdgeB == bodyB.JointList)
            bodyB.JointList = jointRemove.EdgeB.Next;
          jointRemove.EdgeB.Prev = (JointEdge) null;
          jointRemove.EdgeB.Next = (JointEdge) null;
        }
        if (!jointRemove.IsFixedType() && !collideConnected)
        {
          for (ContactEdge contactEdge = bodyB.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
          {
            if (contactEdge.Other == bodyA)
              contactEdge.Contact.FlagForFiltering();
          }
        }
        if (this.JointRemoved != null)
          this.JointRemoved(jointRemove);
      }
      this._jointRemoveList.Clear();
    }

    private void ProcessAddedJoints()
    {
      if (this._jointAddList.Count <= 0)
        return;
      foreach (Joint jointAdd in this._jointAddList)
      {
        this.JointList.Add(jointAdd);
        jointAdd.EdgeA.Joint = jointAdd;
        jointAdd.EdgeA.Other = jointAdd.BodyB;
        jointAdd.EdgeA.Prev = (JointEdge) null;
        jointAdd.EdgeA.Next = jointAdd.BodyA.JointList;
        if (jointAdd.BodyA.JointList != null)
          jointAdd.BodyA.JointList.Prev = jointAdd.EdgeA;
        jointAdd.BodyA.JointList = jointAdd.EdgeA;
        if (!jointAdd.IsFixedType())
        {
          jointAdd.EdgeB.Joint = jointAdd;
          jointAdd.EdgeB.Other = jointAdd.BodyA;
          jointAdd.EdgeB.Prev = (JointEdge) null;
          jointAdd.EdgeB.Next = jointAdd.BodyB.JointList;
          if (jointAdd.BodyB.JointList != null)
            jointAdd.BodyB.JointList.Prev = jointAdd.EdgeB;
          jointAdd.BodyB.JointList = jointAdd.EdgeB;
          Body bodyA = jointAdd.BodyA;
          Body bodyB = jointAdd.BodyB;
          if (!jointAdd.CollideConnected)
          {
            for (ContactEdge contactEdge = bodyB.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
              if (contactEdge.Other == bodyA)
                contactEdge.Contact.FlagForFiltering();
            }
          }
        }
        if (this.JointAdded != null)
          this.JointAdded(jointAdd);
      }
      this._jointAddList.Clear();
    }

    private void ProcessAddedBodies()
    {
      if (this._bodyAddList.Count <= 0)
        return;
      foreach (Body bodyAdd in this._bodyAddList)
      {
        this.BodyList.Add(bodyAdd);
        if (this.BodyAdded != null)
          this.BodyAdded(bodyAdd);
      }
      this._bodyAddList.Clear();
    }

    private void ProcessRemovedBodies()
    {
      if (this._bodyRemoveList.Count <= 0)
        return;
      foreach (Body bodyRemove in this._bodyRemoveList)
      {
        JointEdge jointEdge1 = bodyRemove.JointList;
        while (jointEdge1 != null)
        {
          JointEdge jointEdge2 = jointEdge1;
          jointEdge1 = jointEdge1.Next;
          this.RemoveJoint(jointEdge2.Joint, false);
        }
        bodyRemove.JointList = (JointEdge) null;
        ContactEdge contactEdge1 = bodyRemove.ContactList;
        while (contactEdge1 != null)
        {
          ContactEdge contactEdge2 = contactEdge1;
          contactEdge1 = contactEdge1.Next;
          this.ContactManager.Destroy(contactEdge2.Contact);
        }
        bodyRemove.ContactList = (ContactEdge) null;
        for (int index = 0; index < bodyRemove.FixtureList.Count; ++index)
        {
          bodyRemove.FixtureList[index].DestroyProxies(this.ContactManager.BroadPhase);
          bodyRemove.FixtureList[index].Destroy();
        }
        bodyRemove.FixtureList = (List<Fixture>) null;
        this.BodyList.Remove(bodyRemove);
        if (this.BodyRemoved != null)
          this.BodyRemoved(bodyRemove);
      }
      this._bodyRemoveList.Clear();
    }

    public void Step(float dt)
    {
      if (Settings.EnableDiagnostics)
        this._watch.Start();
      this.ProcessChanges();
      if (Settings.EnableDiagnostics)
        this.AddRemoveTime = (float) this._watch.ElapsedTicks;
      if ((double) dt == 0.0 || !this.Enabled)
      {
        if (!Settings.EnableDiagnostics)
          return;
        this._watch.Stop();
        this._watch.Reset();
      }
      else
      {
        if ((this.Flags & WorldFlags.NewFixture) == WorldFlags.NewFixture)
        {
          this.ContactManager.FindNewContacts();
          this.Flags &= ~WorldFlags.NewFixture;
        }
        TimeStep step;
        step.inv_dt = 1f / dt;
        step.dt = dt;
        step.dtRatio = this._invDt0 * dt;
        for (int index = 0; index < this.ControllerList.Count; ++index)
          this.ControllerList[index].Update(dt);
        if (Settings.EnableDiagnostics)
          this.ControllersUpdateTime = (float) this._watch.ElapsedTicks - this.AddRemoveTime;
        this.ContactManager.Collide();
        if (Settings.EnableDiagnostics)
          this.ContactsUpdateTime = (float) this._watch.ElapsedTicks - (this.AddRemoveTime + this.ControllersUpdateTime);
        this.Solve(ref step);
        if (Settings.EnableDiagnostics)
          this.SolveUpdateTime = (float) this._watch.ElapsedTicks - (this.AddRemoveTime + this.ControllersUpdateTime + this.ContactsUpdateTime);
        if (Settings.ContinuousPhysics)
          this.SolveTOI(ref step);
        if (Settings.EnableDiagnostics)
          this.ContinuousPhysicsTime = (float) this._watch.ElapsedTicks - (this.AddRemoveTime + this.ControllersUpdateTime + this.ContactsUpdateTime + this.SolveUpdateTime);
        this._invDt0 = step.inv_dt;
        if ((this.Flags & WorldFlags.ClearForces) != (WorldFlags) 0)
          this.ClearForces();
        for (int index = 0; index < this.BreakableBodyList.Count; ++index)
          this.BreakableBodyList[index].Update();
        if (!Settings.EnableDiagnostics)
          return;
        this._watch.Stop();
        this.UpdateTime = (float) this._watch.ElapsedTicks;
        this._watch.Reset();
      }
    }

    public void ClearForces()
    {
      for (int index = 0; index < this.BodyList.Count; ++index)
      {
        Body body = this.BodyList[index];
        body.Force = Microsoft.Xna.Framework.Vector2.Zero;
        body.Torque = 0.0f;
      }
    }

    public void QueryAABB(Func<Fixture, bool> callback, ref AABB aabb)
    {
      this.ContactManager.BroadPhase.Query((Func<int, bool>) (proxyId => callback(this.ContactManager.BroadPhase.GetProxy(proxyId).Fixture)), ref aabb);
    }

    public void RayCast(FarseerPhysics.Dynamics.RayCastCallback callback, Microsoft.Xna.Framework.Vector2 point1, Microsoft.Xna.Framework.Vector2 point2)
    {
      RayCastInput input = new RayCastInput();
      input.MaxFraction = 1f;
      input.Point1 = point1;
      input.Point2 = point2;
      this.rayCastCallback = callback;
      this.ContactManager.BroadPhase.RayCast((IRayCastCallback) this, ref input);
    }

    public float RayCastCallback(ref RayCastInput input, int proxyId)
    {
      FixtureProxy proxy = this.ContactManager.BroadPhase.GetProxy(proxyId);
      Fixture fixture = proxy.Fixture;
      int childIndex = proxy.ChildIndex;
      RayCastOutput output;
      if (!fixture.RayCast(out output, ref input, childIndex))
        return input.MaxFraction;
      float fraction = output.Fraction;
      Microsoft.Xna.Framework.Vector2 point = (1f - fraction) * input.Point1 + fraction * input.Point2;
      return this.rayCastCallback != null ? this.rayCastCallback(fixture, point, output.Normal, fraction) : RaycastHelper.rayCastCallback(fixture, point, output.Normal, fraction);
    }

    private void Solve(ref TimeStep step)
    {
      this.Island.Reset(this.BodyList.Count, this.ContactManager.ContactList.Count, this.JointList.Count, this.ContactManager);
      foreach (Body body in this.BodyList)
        body.Flags &= ~BodyFlags.Island;
      for (int index = 0; index < this.ContactManager.ContactList.Count; ++index)
        this.ContactManager.ContactList[index].Flags &= ~ContactFlags.Island;
      foreach (Joint joint in this.JointList)
        joint.IslandFlag = false;
      int count = this.BodyList.Count;
      if (count > this._stack.Length)
        this._stack = new Body[Math.Max(this._stack.Length * 2, count)];
      for (int index1 = this.BodyList.Count - 1; index1 >= 0; --index1)
      {
        Body body1 = this.BodyList[index1];
        if ((body1.Flags & BodyFlags.Island) == BodyFlags.None && body1.Awake && body1.Enabled && body1.BodyType != BodyType.Static)
        {
          this.Island.Clear();
          int num1 = 0;
          Body[] stack = this._stack;
          int index2 = num1;
          int num2 = index2 + 1;
          Body body2 = body1;
          stack[index2] = body2;
          body1.Flags |= BodyFlags.Island;
          while (num2 > 0)
          {
            Body body3 = this._stack[--num2];
            this.Island.Add(body3);
            body3.Awake = true;
            if (body3.BodyType != BodyType.Static)
            {
              for (ContactEdge contactEdge = body3.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
              {
                Contact contact = contactEdge.Contact;
                if ((contact.Flags & ContactFlags.Island) == ContactFlags.None && contactEdge.Contact.Enabled && contactEdge.Contact.IsTouching())
                {
                  bool isSensor1 = contact.FixtureA.IsSensor;
                  bool isSensor2 = contact.FixtureB.IsSensor;
                  if (!isSensor1 && !isSensor2)
                  {
                    this.Island.Add(contact);
                    contact.Flags |= ContactFlags.Island;
                    Body other = contactEdge.Other;
                    if ((other.Flags & BodyFlags.Island) == BodyFlags.None)
                    {
                      this._stack[num2++] = other;
                      other.Flags |= BodyFlags.Island;
                    }
                  }
                }
              }
              for (JointEdge jointEdge = body3.JointList; jointEdge != null; jointEdge = jointEdge.Next)
              {
                if (!jointEdge.Joint.IslandFlag)
                {
                  Body other = jointEdge.Other;
                  if (other != null)
                  {
                    if (other.Enabled)
                    {
                      this.Island.Add(jointEdge.Joint);
                      jointEdge.Joint.IslandFlag = true;
                      if ((other.Flags & BodyFlags.Island) == BodyFlags.None)
                      {
                        this._stack[num2++] = other;
                        other.Flags |= BodyFlags.Island;
                      }
                    }
                  }
                  else
                  {
                    this.Island.Add(jointEdge.Joint);
                    jointEdge.Joint.IslandFlag = true;
                  }
                }
              }
            }
          }
          this.Island.Solve(ref step, ref this.Gravity);
          for (int index3 = 0; index3 < this.Island.BodyCount; ++index3)
          {
            Body body4 = this.Island.Bodies[index3];
            if (body4.BodyType == BodyType.Static)
              body4.Flags &= ~BodyFlags.Island;
          }
        }
      }
      foreach (Body body in this.BodyList)
      {
        if ((body.Flags & BodyFlags.Island) == BodyFlags.Island && body.BodyType != BodyType.Static)
          body.SynchronizeFixtures();
      }
      this.ContactManager.FindNewContacts();
    }

    private void SolveTOI(ref TimeStep step)
    {
      this.Island.Reset(64, 32, 0, this.ContactManager);
      if (this._stepComplete)
      {
        for (int index = 0; index < this.BodyList.Count; ++index)
        {
          this.BodyList[index].Flags &= ~BodyFlags.Island;
          this.BodyList[index].Sweep.Alpha0 = 0.0f;
        }
        for (int index = 0; index < this.ContactManager.ContactList.Count; ++index)
        {
          Contact contact = this.ContactManager.ContactList[index];
          contact.Flags &= ~(ContactFlags.Island | ContactFlags.TOI);
          contact.TOICount = 0;
          contact.TOI = 1f;
        }
      }
      do
      {
        Contact contact1 = (Contact) null;
        float alpha = 1f;
        for (int index = 0; index < this.ContactManager.ContactList.Count; ++index)
        {
          Contact contact2 = this.ContactManager.ContactList[index];
          if (contact2.Enabled && contact2.TOICount <= 8)
          {
            float num;
            if ((contact2.Flags & ContactFlags.TOI) == ContactFlags.TOI)
            {
              num = contact2.TOI;
            }
            else
            {
              Fixture fixtureA = contact2.FixtureA;
              Fixture fixtureB = contact2.FixtureB;
              if (!fixtureA.IsSensor && !fixtureB.IsSensor)
              {
                Body body1 = fixtureA.Body;
                Body body2 = fixtureB.Body;
                BodyType bodyType1 = body1.BodyType;
                BodyType bodyType2 = body2.BodyType;
                bool flag1 = body1.Awake && bodyType1 != BodyType.Static;
                bool flag2 = body2.Awake && bodyType2 != BodyType.Static;
                if (flag1 || flag2)
                {
                  bool flag3 = (body1.IsBullet || bodyType1 != BodyType.Dynamic) && !body1.IgnoreCCD;
                  bool flag4 = (body2.IsBullet || bodyType2 != BodyType.Dynamic) && !body2.IgnoreCCD;
                  if (flag3 || flag4)
                  {
                    float alpha0 = body1.Sweep.Alpha0;
                    if ((double) body1.Sweep.Alpha0 < (double) body2.Sweep.Alpha0)
                    {
                      alpha0 = body2.Sweep.Alpha0;
                      body1.Sweep.Advance(alpha0);
                    }
                    else if ((double) body2.Sweep.Alpha0 < (double) body1.Sweep.Alpha0)
                    {
                      alpha0 = body1.Sweep.Alpha0;
                      body2.Sweep.Advance(alpha0);
                    }
                    this._input.ProxyA.Set(fixtureA.Shape, contact2.ChildIndexA);
                    this._input.ProxyB.Set(fixtureB.Shape, contact2.ChildIndexB);
                    this._input.SweepA = body1.Sweep;
                    this._input.SweepB = body2.Sweep;
                    this._input.TMax = 1f;
                    TOIOutput output;
                    TimeOfImpact.CalculateTimeOfImpact(out output, this._input);
                    float t = output.T;
                    num = output.State != TOIOutputState.Touching ? 1f : Math.Min(alpha0 + (1f - alpha0) * t, 1f);
                    contact2.TOI = num;
                    contact2.Flags |= ContactFlags.TOI;
                  }
                  else
                    continue;
                }
                else
                  continue;
              }
              else
                continue;
            }
            if ((double) num < (double) alpha)
            {
              contact1 = contact2;
              alpha = num;
            }
          }
        }
        if (contact1 == null || 0.99999880790710449 < (double) alpha)
        {
          this._stepComplete = true;
          return;
        }
        Fixture fixtureA1 = contact1.FixtureA;
        Fixture fixtureB1 = contact1.FixtureB;
        Body body3 = fixtureA1.Body;
        Body body4 = fixtureB1.Body;
        Sweep sweep1 = body3.Sweep;
        Sweep sweep2 = body4.Sweep;
        body3.Advance(alpha);
        body4.Advance(alpha);
        contact1.Update(this.ContactManager);
        contact1.Flags &= ~ContactFlags.TOI;
        ++contact1.TOICount;
        if (!contact1.Enabled || !contact1.IsTouching())
        {
          contact1.Enabled = false;
          body3.Sweep = sweep1;
          body4.Sweep = sweep2;
          body3.SynchronizeTransform();
          body4.SynchronizeTransform();
        }
        else
        {
          body3.Awake = true;
          body4.Awake = true;
          this.Island.Clear();
          this.Island.Add(body3);
          this.Island.Add(body4);
          this.Island.Add(contact1);
          body3.Flags |= BodyFlags.Island;
          body4.Flags |= BodyFlags.Island;
          contact1.Flags |= ContactFlags.Island;
          Body[] bodyArray = new Body[2]{ body3, body4 };
          for (int index = 0; index < 2; ++index)
          {
            Body body5 = bodyArray[index];
            if (body5.BodyType == BodyType.Dynamic)
            {
              for (ContactEdge contactEdge = body5.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
              {
                Contact contact3 = contactEdge.Contact;
                if ((contact3.Flags & ContactFlags.Island) != ContactFlags.Island)
                {
                  Body other = contactEdge.Other;
                  if ((other.BodyType != BodyType.Dynamic || body5.IsBullet || other.IsBullet) && !contact3.FixtureA.IsSensor && !contact3.FixtureB.IsSensor)
                  {
                    Sweep sweep3 = other.Sweep;
                    if ((other.Flags & BodyFlags.Island) == BodyFlags.None)
                      other.Advance(alpha);
                    contact3.Update(this.ContactManager);
                    if (!contact3.Enabled)
                    {
                      other.Sweep = sweep3;
                      other.SynchronizeTransform();
                    }
                    else if (!contact3.IsTouching())
                    {
                      other.Sweep = sweep3;
                      other.SynchronizeTransform();
                    }
                    else
                    {
                      contact3.Flags |= ContactFlags.Island;
                      this.Island.Add(contact3);
                      if ((other.Flags & BodyFlags.Island) != BodyFlags.Island)
                      {
                        other.Flags |= BodyFlags.Island;
                        if (other.BodyType != BodyType.Static)
                          other.Awake = true;
                        this.Island.Add(other);
                      }
                    }
                  }
                }
              }
            }
          }
          TimeStep subStep;
          subStep.dt = (1f - alpha) * step.dt;
          subStep.inv_dt = 1f / subStep.dt;
          subStep.dtRatio = 1f;
          this.Island.SolveTOI(ref subStep);
          for (int index = 0; index < this.Island.BodyCount; ++index)
          {
            Body body6 = this.Island.Bodies[index];
            body6.Flags &= ~BodyFlags.Island;
            if (body6.BodyType == BodyType.Dynamic)
            {
              body6.SynchronizeFixtures();
              for (ContactEdge contactEdge = body6.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
                contactEdge.Contact.Flags &= ~(ContactFlags.Island | ContactFlags.TOI);
            }
          }
          this.ContactManager.FindNewContacts();
        }
      }
      while (!this.EnableSubStepping);
      this._stepComplete = false;
    }

    public void AddController(Controller controller)
    {
      controller.World = this;
      this.ControllerList.Add(controller);
      if (this.ControllerAdded == null)
        return;
      this.ControllerAdded(controller);
    }

    public void RemoveController(Controller controller)
    {
      if (!this.ControllerList.Contains(controller))
        return;
      this.ControllerList.Remove(controller);
      if (this.ControllerRemoved == null)
        return;
      this.ControllerRemoved(controller);
    }

    public void AddBreakableBody(BreakableBody breakableBody)
    {
      this.BreakableBodyList.Add(breakableBody);
    }

    public void RemoveBreakableBody(BreakableBody breakableBody)
    {
      this.BreakableBodyList.Remove(breakableBody);
    }

    public Fixture TestPoint(Microsoft.Xna.Framework.Vector2 point)
    {
      Microsoft.Xna.Framework.Vector2 vector2 = new Microsoft.Xna.Framework.Vector2(1.1920929E-07f, 1.1920929E-07f);
      AABB aabb;
      aabb.LowerBound = point - vector2;
      aabb.UpperBound = point + vector2;
      Fixture myFixture = (Fixture) null;
      this.QueryAABB((Func<Fixture, bool>) (fixture =>
      {
        if (!fixture.TestPoint(ref point))
          return true;
        myFixture = fixture;
        return false;
      }), ref aabb);
      return myFixture;
    }

    public List<Fixture> TestPointAll(Microsoft.Xna.Framework.Vector2 point)
    {
      Microsoft.Xna.Framework.Vector2 vector2 = new Microsoft.Xna.Framework.Vector2(1.1920929E-07f, 1.1920929E-07f);
      AABB aabb;
      aabb.LowerBound = point - vector2;
      aabb.UpperBound = point + vector2;
      List<Fixture> fixtures = new List<Fixture>();
      this.QueryAABB((Func<Fixture, bool>) (fixture =>
      {
        if (fixture.TestPoint(ref point))
          fixtures.Add(fixture);
        return true;
      }), ref aabb);
      return fixtures;
    }

    public void Clear()
    {
      this.ProcessChanges();
      for (int index = this.BodyList.Count - 1; index >= 0; --index)
        this.RemoveBody(this.BodyList[index]);
      for (int index = this.ControllerList.Count - 1; index >= 0; --index)
        this.RemoveController(this.ControllerList[index]);
      for (int index = this.BreakableBodyList.Count - 1; index >= 0; --index)
        this.RemoveBreakableBody(this.BreakableBodyList[index]);
      this.ProcessChanges();
    }
  }
}
