// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Contacts.ContactSolver
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Contacts
{
  public class ContactSolver
  {
    public ContactConstraint[] Constraints;
    private int _constraintCount;
    private Contact[] _contacts;

    public void Reset(Contact[] contacts, int contactCount, float impulseRatio, bool warmstarting)
    {
      this._contacts = contacts;
      this._constraintCount = contactCount;
      if (this.Constraints == null || this.Constraints.Length < this._constraintCount)
      {
        this.Constraints = new ContactConstraint[this._constraintCount * 2];
        for (int index = 0; index < this.Constraints.Length; ++index)
          this.Constraints[index] = new ContactConstraint();
      }
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        Contact contact = contacts[index1];
        Fixture fixtureA = contact.FixtureA;
        Fixture fixtureB = contact.FixtureB;
        Shape shape1 = fixtureA.Shape;
        Shape shape2 = fixtureB.Shape;
        float radius1 = shape1.Radius;
        float radius2 = shape2.Radius;
        Body body1 = fixtureA.Body;
        Body body2 = fixtureB.Body;
        Manifold manifold = contact.Manifold;
        ContactConstraint constraint = this.Constraints[index1];
        constraint.Friction = Settings.MixFriction(fixtureA.Friction, fixtureB.Friction);
        constraint.Restitution = Settings.MixRestitution(fixtureA.Restitution, fixtureB.Restitution);
        constraint.BodyA = body1;
        constraint.BodyB = body2;
        constraint.Manifold = manifold;
        constraint.Normal = Vector2.Zero;
        constraint.PointCount = manifold.PointCount;
        constraint.LocalNormal = manifold.LocalNormal;
        constraint.LocalPoint = manifold.LocalPoint;
        constraint.RadiusA = radius1;
        constraint.RadiusB = radius2;
        constraint.Type = manifold.Type;
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          ManifoldPoint point1 = manifold.Points[index2];
          ContactConstraintPoint point2 = constraint.Points[index2];
          if (warmstarting)
          {
            point2.NormalImpulse = impulseRatio * point1.NormalImpulse;
            point2.TangentImpulse = impulseRatio * point1.TangentImpulse;
          }
          else
          {
            point2.NormalImpulse = 0.0f;
            point2.TangentImpulse = 0.0f;
          }
          point2.LocalPoint = point1.LocalPoint;
          point2.rA = Vector2.Zero;
          point2.rB = Vector2.Zero;
          point2.NormalMass = 0.0f;
          point2.TangentMass = 0.0f;
          point2.VelocityBias = 0.0f;
        }
        constraint.K.SetZero();
        constraint.NormalMass.SetZero();
      }
    }

    public void InitializeVelocityConstraints()
    {
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        ContactConstraint constraint = this.Constraints[index1];
        float radiusA = constraint.RadiusA;
        float radiusB = constraint.RadiusB;
        Body bodyA = constraint.BodyA;
        Body bodyB = constraint.BodyB;
        Manifold manifold = constraint.Manifold;
        Vector2 linearVelocity1 = bodyA.LinearVelocity;
        Vector2 linearVelocity2 = bodyB.LinearVelocity;
        float angularVelocity1 = bodyA.AngularVelocity;
        float angularVelocity2 = bodyB.AngularVelocity;
        FixedArray2<Vector2> points;
        FarseerPhysics.Collision.Collision.GetWorldManifold(ref manifold, ref bodyA.Xf, radiusA, ref bodyB.Xf, radiusB, out constraint.Normal, out points);
        Vector2 vector2 = new Vector2(constraint.Normal.Y, -constraint.Normal.X);
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          ContactConstraintPoint point = constraint.Points[index2];
          point.rA = points[index2] - bodyA.Sweep.C;
          point.rB = points[index2] - bodyB.Sweep.C;
          float num1 = (float) ((double) point.rA.X * (double) constraint.Normal.Y - (double) point.rA.Y * (double) constraint.Normal.X);
          float num2 = (float) ((double) point.rB.X * (double) constraint.Normal.Y - (double) point.rB.Y * (double) constraint.Normal.X);
          float num3 = num1 * num1;
          float num4 = num2 * num2;
          float num5 = (float) ((double) bodyA.InvMass + (double) bodyB.InvMass + (double) bodyA.InvI * (double) num3 + (double) bodyB.InvI * (double) num4);
          point.NormalMass = 1f / num5;
          float num6 = (float) ((double) point.rA.X * (double) vector2.Y - (double) point.rA.Y * (double) vector2.X);
          float num7 = (float) ((double) point.rB.X * (double) vector2.Y - (double) point.rB.Y * (double) vector2.X);
          float num8 = num6 * num6;
          float num9 = num7 * num7;
          float num10 = (float) ((double) bodyA.InvMass + (double) bodyB.InvMass + (double) bodyA.InvI * (double) num8 + (double) bodyB.InvI * (double) num9);
          point.TangentMass = 1f / num10;
          point.VelocityBias = 0.0f;
          float num11 = (float) ((double) constraint.Normal.X * ((double) linearVelocity2.X + -(double) angularVelocity2 * (double) point.rB.Y - (double) linearVelocity1.X - -(double) angularVelocity1 * (double) point.rA.Y) + (double) constraint.Normal.Y * ((double) linearVelocity2.Y + (double) angularVelocity2 * (double) point.rB.X - (double) linearVelocity1.Y - (double) angularVelocity1 * (double) point.rA.X));
          if ((double) num11 < -1.0)
            point.VelocityBias = -constraint.Restitution * num11;
        }
        if (constraint.PointCount == 2)
        {
          ContactConstraintPoint point1 = constraint.Points[0];
          ContactConstraintPoint point2 = constraint.Points[1];
          float invMass1 = bodyA.InvMass;
          float invI1 = bodyA.InvI;
          float invMass2 = bodyB.InvMass;
          float invI2 = bodyB.InvI;
          float num12 = (float) ((double) point1.rA.X * (double) constraint.Normal.Y - (double) point1.rA.Y * (double) constraint.Normal.X);
          float num13 = (float) ((double) point1.rB.X * (double) constraint.Normal.Y - (double) point1.rB.Y * (double) constraint.Normal.X);
          float num14 = (float) ((double) point2.rA.X * (double) constraint.Normal.Y - (double) point2.rA.Y * (double) constraint.Normal.X);
          float num15 = (float) ((double) point2.rB.X * (double) constraint.Normal.Y - (double) point2.rB.Y * (double) constraint.Normal.X);
          float num16 = (float) ((double) invMass1 + (double) invMass2 + (double) invI1 * (double) num12 * (double) num12 + (double) invI2 * (double) num13 * (double) num13);
          float num17 = (float) ((double) invMass1 + (double) invMass2 + (double) invI1 * (double) num14 * (double) num14 + (double) invI2 * (double) num15 * (double) num15);
          float num18 = (float) ((double) invMass1 + (double) invMass2 + (double) invI1 * (double) num12 * (double) num14 + (double) invI2 * (double) num13 * (double) num15);
          if ((double) num16 * (double) num16 < 100.0 * ((double) num16 * (double) num17 - (double) num18 * (double) num18))
          {
            constraint.K.Col1.X = num16;
            constraint.K.Col1.Y = num18;
            constraint.K.Col2.X = num18;
            constraint.K.Col2.Y = num17;
            float x1 = constraint.K.Col1.X;
            float x2 = constraint.K.Col2.X;
            float y1 = constraint.K.Col1.Y;
            float y2 = constraint.K.Col2.Y;
            float num19 = (float) ((double) x1 * (double) y2 - (double) x2 * (double) y1);
            if ((double) num19 != 0.0)
              num19 = 1f / num19;
            constraint.NormalMass.Col1.X = num19 * y2;
            constraint.NormalMass.Col1.Y = -num19 * y1;
            constraint.NormalMass.Col2.X = -num19 * x2;
            constraint.NormalMass.Col2.Y = num19 * x1;
          }
          else
            constraint.PointCount = 1;
        }
      }
    }

    public void WarmStart()
    {
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        ContactConstraint constraint = this.Constraints[index1];
        float y = constraint.Normal.Y;
        float num1 = -constraint.Normal.X;
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          ContactConstraintPoint point = constraint.Points[index2];
          float num2 = (float) ((double) point.NormalImpulse * (double) constraint.Normal.X + (double) point.TangentImpulse * (double) y);
          float num3 = (float) ((double) point.NormalImpulse * (double) constraint.Normal.Y + (double) point.TangentImpulse * (double) num1);
          constraint.BodyA.AngularVelocityInternal -= constraint.BodyA.InvI * (float) ((double) point.rA.X * (double) num3 - (double) point.rA.Y * (double) num2);
          constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num2;
          constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num3;
          constraint.BodyB.AngularVelocityInternal += constraint.BodyB.InvI * (float) ((double) point.rB.X * (double) num3 - (double) point.rB.Y * (double) num2);
          constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num2;
          constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num3;
        }
      }
    }

    public void SolveVelocityConstraints()
    {
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        ContactConstraint constraint = this.Constraints[index1];
        float velocityInternal1 = constraint.BodyA.AngularVelocityInternal;
        float velocityInternal2 = constraint.BodyB.AngularVelocityInternal;
        float y = constraint.Normal.Y;
        float num1 = -constraint.Normal.X;
        float friction = constraint.Friction;
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          ContactConstraintPoint point = constraint.Points[index2];
          float num2 = point.TangentMass * (float) -(((double) constraint.BodyB.LinearVelocityInternal.X + -(double) velocityInternal2 * (double) point.rB.Y - (double) constraint.BodyA.LinearVelocityInternal.X - -(double) velocityInternal1 * (double) point.rA.Y) * (double) y + ((double) constraint.BodyB.LinearVelocityInternal.Y + (double) velocityInternal2 * (double) point.rB.X - (double) constraint.BodyA.LinearVelocityInternal.Y - (double) velocityInternal1 * (double) point.rA.X) * (double) num1);
          float val2 = friction * point.NormalImpulse;
          float num3 = Math.Max(-val2, Math.Min(point.TangentImpulse + num2, val2));
          float num4 = num3 - point.TangentImpulse;
          float num5 = num4 * y;
          float num6 = num4 * num1;
          constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num5;
          constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num6;
          velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point.rA.X * (double) num6 - (double) point.rA.Y * (double) num5);
          constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num5;
          constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num6;
          velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point.rB.X * (double) num6 - (double) point.rB.Y * (double) num5);
          point.TangentImpulse = num3;
        }
        if (constraint.PointCount == 1)
        {
          ContactConstraintPoint point = constraint.Points[0];
          float num7 = (float) (-(double) point.NormalMass * (((double) constraint.BodyB.LinearVelocityInternal.X + -(double) velocityInternal2 * (double) point.rB.Y - (double) constraint.BodyA.LinearVelocityInternal.X - -(double) velocityInternal1 * (double) point.rA.Y) * (double) constraint.Normal.X + ((double) constraint.BodyB.LinearVelocityInternal.Y + (double) velocityInternal2 * (double) point.rB.X - (double) constraint.BodyA.LinearVelocityInternal.Y - (double) velocityInternal1 * (double) point.rA.X) * (double) constraint.Normal.Y - (double) point.VelocityBias));
          float num8 = Math.Max(point.NormalImpulse + num7, 0.0f);
          float num9 = num8 - point.NormalImpulse;
          float num10 = num9 * constraint.Normal.X;
          float num11 = num9 * constraint.Normal.Y;
          constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num10;
          constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num11;
          velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point.rA.X * (double) num11 - (double) point.rA.Y * (double) num10);
          constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num10;
          constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num11;
          velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point.rB.X * (double) num11 - (double) point.rB.Y * (double) num10);
          point.NormalImpulse = num8;
        }
        else
        {
          ContactConstraintPoint point1 = constraint.Points[0];
          ContactConstraintPoint point2 = constraint.Points[1];
          float normalImpulse1 = point1.NormalImpulse;
          float normalImpulse2 = point2.NormalImpulse;
          float num12 = (float) (((double) constraint.BodyB.LinearVelocityInternal.X + -(double) velocityInternal2 * (double) point1.rB.Y - (double) constraint.BodyA.LinearVelocityInternal.X - -(double) velocityInternal1 * (double) point1.rA.Y) * (double) constraint.Normal.X + ((double) constraint.BodyB.LinearVelocityInternal.Y + (double) velocityInternal2 * (double) point1.rB.X - (double) constraint.BodyA.LinearVelocityInternal.Y - (double) velocityInternal1 * (double) point1.rA.X) * (double) constraint.Normal.Y);
          float num13 = (float) (((double) constraint.BodyB.LinearVelocityInternal.X + -(double) velocityInternal2 * (double) point2.rB.Y - (double) constraint.BodyA.LinearVelocityInternal.X - -(double) velocityInternal1 * (double) point2.rA.Y) * (double) constraint.Normal.X + ((double) constraint.BodyB.LinearVelocityInternal.Y + (double) velocityInternal2 * (double) point2.rB.X - (double) constraint.BodyA.LinearVelocityInternal.Y - (double) velocityInternal1 * (double) point2.rA.X) * (double) constraint.Normal.Y);
          float num14 = (float) ((double) num12 - (double) point1.VelocityBias - ((double) constraint.K.Col1.X * (double) normalImpulse1 + (double) constraint.K.Col2.X * (double) normalImpulse2));
          float num15 = (float) ((double) num13 - (double) point2.VelocityBias - ((double) constraint.K.Col1.Y * (double) normalImpulse1 + (double) constraint.K.Col2.Y * (double) normalImpulse2));
          float num16 = (float) -((double) constraint.NormalMass.Col1.X * (double) num14 + (double) constraint.NormalMass.Col2.X * (double) num15);
          float num17 = (float) -((double) constraint.NormalMass.Col1.Y * (double) num14 + (double) constraint.NormalMass.Col2.Y * (double) num15);
          if ((double) num16 >= 0.0 && (double) num17 >= 0.0)
          {
            float num18 = num16 - normalImpulse1;
            float num19 = num17 - normalImpulse2;
            float num20 = num18 * constraint.Normal.X;
            float num21 = num18 * constraint.Normal.Y;
            float num22 = num19 * constraint.Normal.X;
            float num23 = num19 * constraint.Normal.Y;
            float num24 = num20 + num22;
            float num25 = num21 + num23;
            constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num24;
            constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num25;
            velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point1.rA.X * (double) num21 - (double) point1.rA.Y * (double) num20 + ((double) point2.rA.X * (double) num23 - (double) point2.rA.Y * (double) num22));
            constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num24;
            constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num25;
            velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point1.rB.X * (double) num21 - (double) point1.rB.Y * (double) num20 + ((double) point2.rB.X * (double) num23 - (double) point2.rB.Y * (double) num22));
            point1.NormalImpulse = num16;
            point2.NormalImpulse = num17;
          }
          else
          {
            float num26 = -point1.NormalMass * num14;
            float num27 = 0.0f;
            float num28 = constraint.K.Col1.Y * num26 + num15;
            if ((double) num26 >= 0.0 && (double) num28 >= 0.0)
            {
              float num29 = num26 - normalImpulse1;
              float num30 = num27 - normalImpulse2;
              float num31 = num29 * constraint.Normal.X;
              float num32 = num29 * constraint.Normal.Y;
              float num33 = num30 * constraint.Normal.X;
              float num34 = num30 * constraint.Normal.Y;
              float num35 = num31 + num33;
              float num36 = num32 + num34;
              constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num35;
              constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num36;
              velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point1.rA.X * (double) num32 - (double) point1.rA.Y * (double) num31 + ((double) point2.rA.X * (double) num34 - (double) point2.rA.Y * (double) num33));
              constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num35;
              constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num36;
              velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point1.rB.X * (double) num32 - (double) point1.rB.Y * (double) num31 + ((double) point2.rB.X * (double) num34 - (double) point2.rB.Y * (double) num33));
              point1.NormalImpulse = num26;
              point2.NormalImpulse = num27;
            }
            else
            {
              float num37 = 0.0f;
              float num38 = -point2.NormalMass * num15;
              float num39 = constraint.K.Col2.X * num38 + num14;
              if ((double) num38 >= 0.0 && (double) num39 >= 0.0)
              {
                float num40 = num37 - normalImpulse1;
                float num41 = num38 - normalImpulse2;
                float num42 = num40 * constraint.Normal.X;
                float num43 = num40 * constraint.Normal.Y;
                float num44 = num41 * constraint.Normal.X;
                float num45 = num41 * constraint.Normal.Y;
                float num46 = num42 + num44;
                float num47 = num43 + num45;
                constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num46;
                constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num47;
                velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point1.rA.X * (double) num43 - (double) point1.rA.Y * (double) num42 + ((double) point2.rA.X * (double) num45 - (double) point2.rA.Y * (double) num44));
                constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num46;
                constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num47;
                velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point1.rB.X * (double) num43 - (double) point1.rB.Y * (double) num42 + ((double) point2.rB.X * (double) num45 - (double) point2.rB.Y * (double) num44));
                point1.NormalImpulse = num37;
                point2.NormalImpulse = num38;
              }
              else
              {
                float num48 = 0.0f;
                float num49 = 0.0f;
                float num50 = num14;
                float num51 = num15;
                if ((double) num50 >= 0.0 && (double) num51 >= 0.0)
                {
                  float num52 = num48 - normalImpulse1;
                  float num53 = num49 - normalImpulse2;
                  float num54 = num52 * constraint.Normal.X;
                  float num55 = num52 * constraint.Normal.Y;
                  float num56 = num53 * constraint.Normal.X;
                  float num57 = num53 * constraint.Normal.Y;
                  float num58 = num54 + num56;
                  float num59 = num55 + num57;
                  constraint.BodyA.LinearVelocityInternal.X -= constraint.BodyA.InvMass * num58;
                  constraint.BodyA.LinearVelocityInternal.Y -= constraint.BodyA.InvMass * num59;
                  velocityInternal1 -= constraint.BodyA.InvI * (float) ((double) point1.rA.X * (double) num55 - (double) point1.rA.Y * (double) num54 + ((double) point2.rA.X * (double) num57 - (double) point2.rA.Y * (double) num56));
                  constraint.BodyB.LinearVelocityInternal.X += constraint.BodyB.InvMass * num58;
                  constraint.BodyB.LinearVelocityInternal.Y += constraint.BodyB.InvMass * num59;
                  velocityInternal2 += constraint.BodyB.InvI * (float) ((double) point1.rB.X * (double) num55 - (double) point1.rB.Y * (double) num54 + ((double) point2.rB.X * (double) num57 - (double) point2.rB.Y * (double) num56));
                  point1.NormalImpulse = num48;
                  point2.NormalImpulse = num49;
                }
              }
            }
          }
        }
        constraint.BodyA.AngularVelocityInternal = velocityInternal1;
        constraint.BodyB.AngularVelocityInternal = velocityInternal2;
      }
    }

    public void StoreImpulses()
    {
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        ContactConstraint constraint = this.Constraints[index1];
        Manifold manifold = constraint.Manifold;
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          ManifoldPoint point1 = manifold.Points[index2];
          ContactConstraintPoint point2 = constraint.Points[index2];
          point1.NormalImpulse = point2.NormalImpulse;
          point1.TangentImpulse = point2.TangentImpulse;
          manifold.Points[index2] = point1;
        }
        constraint.Manifold = manifold;
        this._contacts[index1].Manifold = manifold;
      }
    }

    public bool SolvePositionConstraints(float baumgarte)
    {
      float val1 = 0.0f;
      for (int index1 = 0; index1 < this._constraintCount; ++index1)
      {
        ContactConstraint constraint = this.Constraints[index1];
        Body bodyA = constraint.BodyA;
        Body bodyB = constraint.BodyB;
        float num1 = bodyA.Mass * bodyA.InvMass;
        float num2 = bodyA.Mass * bodyA.InvI;
        float num3 = bodyB.Mass * bodyB.InvMass;
        float num4 = bodyB.Mass * bodyB.InvI;
        for (int index2 = 0; index2 < constraint.PointCount; ++index2)
        {
          Vector2 normal;
          Vector2 point;
          float separation;
          ContactSolver.Solve(constraint, index2, out normal, out point, out separation);
          float num5 = point.X - bodyA.Sweep.C.X;
          float num6 = point.Y - bodyA.Sweep.C.Y;
          float num7 = point.X - bodyB.Sweep.C.X;
          float num8 = point.Y - bodyB.Sweep.C.Y;
          val1 = Math.Min(val1, separation);
          float num9 = Math.Max(-0.2f, Math.Min(baumgarte * (separation + 0.005f), 0.0f));
          float num10 = (float) ((double) num5 * (double) normal.Y - (double) num6 * (double) normal.X);
          float num11 = (float) ((double) num7 * (double) normal.Y - (double) num8 * (double) normal.X);
          float num12 = (float) ((double) num1 + (double) num3 + (double) num2 * (double) num10 * (double) num10 + (double) num4 * (double) num11 * (double) num11);
          float num13 = (double) num12 > 0.0 ? -num9 / num12 : 0.0f;
          float num14 = num13 * normal.X;
          float num15 = num13 * normal.Y;
          bodyA.Sweep.C.X -= num1 * num14;
          bodyA.Sweep.C.Y -= num1 * num15;
          bodyA.Sweep.A -= num2 * (float) ((double) num5 * (double) num15 - (double) num6 * (double) num14);
          bodyB.Sweep.C.X += num3 * num14;
          bodyB.Sweep.C.Y += num3 * num15;
          bodyB.Sweep.A += num4 * (float) ((double) num7 * (double) num15 - (double) num8 * (double) num14);
          bodyA.SynchronizeTransform();
          bodyB.SynchronizeTransform();
        }
      }
      return (double) val1 >= -0.0074999998323619366;
    }

    private static void Solve(
      ContactConstraint cc,
      int index,
      out Vector2 normal,
      out Vector2 point,
      out float separation)
    {
      normal = Vector2.Zero;
      switch (cc.Type)
      {
        case ManifoldType.Circles:
          Vector2 worldPoint1 = cc.BodyA.GetWorldPoint(ref cc.LocalPoint);
          Vector2 worldPoint2 = cc.BodyB.GetWorldPoint(ref cc.Points[0].LocalPoint);
          if (((double) worldPoint1.X - (double) worldPoint2.X) * ((double) worldPoint1.X - (double) worldPoint2.X) + ((double) worldPoint1.Y - (double) worldPoint2.Y) * ((double) worldPoint1.Y - (double) worldPoint2.Y) > 1.4210854715202004E-14)
          {
            Vector2 vector2 = worldPoint2 - worldPoint1;
            float num = 1f / (float) Math.Sqrt((double) vector2.X * (double) vector2.X + (double) vector2.Y * (double) vector2.Y);
            normal.X = vector2.X * num;
            normal.Y = vector2.Y * num;
          }
          else
          {
            normal.X = 1f;
            normal.Y = 0.0f;
          }
          point = 0.5f * (worldPoint1 + worldPoint2);
          separation = (float) (((double) worldPoint2.X - (double) worldPoint1.X) * (double) normal.X + ((double) worldPoint2.Y - (double) worldPoint1.Y) * (double) normal.Y) - cc.RadiusA - cc.RadiusB;
          break;
        case ManifoldType.FaceA:
          normal = cc.BodyA.GetWorldVector(ref cc.LocalNormal);
          Vector2 worldPoint3 = cc.BodyA.GetWorldPoint(ref cc.LocalPoint);
          Vector2 worldPoint4 = cc.BodyB.GetWorldPoint(ref cc.Points[index].LocalPoint);
          separation = (float) (((double) worldPoint4.X - (double) worldPoint3.X) * (double) normal.X + ((double) worldPoint4.Y - (double) worldPoint3.Y) * (double) normal.Y) - cc.RadiusA - cc.RadiusB;
          point = worldPoint4;
          break;
        case ManifoldType.FaceB:
          normal = cc.BodyB.GetWorldVector(ref cc.LocalNormal);
          Vector2 worldPoint5 = cc.BodyB.GetWorldPoint(ref cc.LocalPoint);
          Vector2 worldPoint6 = cc.BodyA.GetWorldPoint(ref cc.Points[index].LocalPoint);
          separation = (float) (((double) worldPoint6.X - (double) worldPoint5.X) * (double) normal.X + ((double) worldPoint6.Y - (double) worldPoint5.Y) * (double) normal.Y) - cc.RadiusA - cc.RadiusB;
          point = worldPoint6;
          normal = -normal;
          break;
        default:
          point = Vector2.Zero;
          separation = 0.0f;
          break;
      }
    }
  }
}
