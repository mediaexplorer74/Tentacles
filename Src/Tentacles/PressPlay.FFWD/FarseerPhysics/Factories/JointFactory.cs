// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Factories.JointFactory
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Factories
{
  public static class JointFactory
  {
    public static RevoluteJoint CreateRevoluteJoint(Body bodyA, Body bodyB, Vector2 localAnchorB)
    {
      Vector2 localPoint = bodyA.GetLocalPoint(bodyB.GetWorldPoint(localAnchorB));
      return new RevoluteJoint(bodyA, bodyB, localPoint, localAnchorB);
    }

    public static RevoluteJoint CreateRevoluteJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 anchor)
    {
      RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(bodyA, bodyB, anchor);
      world.AddJoint((Joint) revoluteJoint);
      return revoluteJoint;
    }

    public static FixedRevoluteJoint CreateFixedRevoluteJoint(
      World world,
      Body body,
      Vector2 bodyAnchor,
      Vector2 worldAnchor)
    {
      FixedRevoluteJoint fixedRevoluteJoint = new FixedRevoluteJoint(body, bodyAnchor, worldAnchor);
      world.AddJoint((Joint) fixedRevoluteJoint);
      return fixedRevoluteJoint;
    }

    public static WeldJoint CreateWeldJoint(Body bodyA, Body bodyB, Vector2 localAnchor)
    {
      return new WeldJoint(bodyA, bodyB, bodyA.GetLocalPoint(localAnchor), bodyB.GetLocalPoint(localAnchor));
    }

    public static WeldJoint CreateWeldJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 localanchorB)
    {
      WeldJoint weldJoint = JointFactory.CreateWeldJoint(bodyA, bodyB, localanchorB);
      world.AddJoint((Joint) weldJoint);
      return weldJoint;
    }

    public static WeldJoint CreateWeldJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 localAnchorA,
      Vector2 localAnchorB)
    {
      WeldJoint weldJoint = new WeldJoint(bodyA, bodyB, localAnchorA, localAnchorB);
      world.AddJoint((Joint) weldJoint);
      return weldJoint;
    }

    public static PrismaticJoint CreatePrismaticJoint(
      Body bodyA,
      Body bodyB,
      Vector2 localanchorB,
      Vector2 axis)
    {
      Vector2 localPoint = bodyA.GetLocalPoint(bodyB.GetWorldPoint(localanchorB));
      return new PrismaticJoint(bodyA, bodyB, localPoint, localanchorB, axis);
    }

    public static PrismaticJoint CreatePrismaticJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 localanchorB,
      Vector2 axis)
    {
      PrismaticJoint prismaticJoint = JointFactory.CreatePrismaticJoint(bodyA, bodyB, localanchorB, axis);
      world.AddJoint((Joint) prismaticJoint);
      return prismaticJoint;
    }

    public static FixedPrismaticJoint CreateFixedPrismaticJoint(
      World world,
      Body body,
      Vector2 worldAnchor,
      Vector2 axis)
    {
      FixedPrismaticJoint fixedPrismaticJoint = new FixedPrismaticJoint(body, worldAnchor, axis);
      world.AddJoint((Joint) fixedPrismaticJoint);
      return fixedPrismaticJoint;
    }

    public static LineJoint CreateLineJoint(Body bodyA, Body bodyB, Vector2 anchor, Vector2 axis)
    {
      return new LineJoint(bodyA, bodyB, anchor, axis);
    }

    public static LineJoint CreateLineJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 localanchorB,
      Vector2 axis)
    {
      LineJoint lineJoint = JointFactory.CreateLineJoint(bodyA, bodyB, localanchorB, axis);
      world.AddJoint((Joint) lineJoint);
      return lineJoint;
    }

    public static AngleJoint CreateAngleJoint(World world, Body bodyA, Body bodyB)
    {
      AngleJoint angleJoint = new AngleJoint(bodyA, bodyB);
      world.AddJoint((Joint) angleJoint);
      return angleJoint;
    }

    public static FixedAngleJoint CreateFixedAngleJoint(World world, Body body)
    {
      FixedAngleJoint fixedAngleJoint = new FixedAngleJoint(body);
      world.AddJoint((Joint) fixedAngleJoint);
      return fixedAngleJoint;
    }

    public static DistanceJoint CreateDistanceJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 anchorA,
      Vector2 anchorB)
    {
      DistanceJoint distanceJoint = new DistanceJoint(bodyA, bodyB, anchorA, anchorB);
      world.AddJoint((Joint) distanceJoint);
      return distanceJoint;
    }

    public static FixedDistanceJoint CreateFixedDistanceJoint(
      World world,
      Body body,
      Vector2 localAnchor,
      Vector2 worldAnchor)
    {
      FixedDistanceJoint fixedDistanceJoint = new FixedDistanceJoint(body, localAnchor, worldAnchor);
      world.AddJoint((Joint) fixedDistanceJoint);
      return fixedDistanceJoint;
    }

    public static FrictionJoint CreateFrictionJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 anchorA,
      Vector2 anchorB)
    {
      FrictionJoint frictionJoint = new FrictionJoint(bodyA, bodyB, anchorA, anchorB);
      world.AddJoint((Joint) frictionJoint);
      return frictionJoint;
    }

    public static FixedFrictionJoint CreateFixedFrictionJoint(
      World world,
      Body body,
      Vector2 bodyAnchor)
    {
      FixedFrictionJoint fixedFrictionJoint = new FixedFrictionJoint(body, bodyAnchor);
      world.AddJoint((Joint) fixedFrictionJoint);
      return fixedFrictionJoint;
    }

    public static GearJoint CreateGearJoint(World world, Joint jointA, Joint jointB, float ratio)
    {
      GearJoint gearJoint = new GearJoint(jointA, jointB, ratio);
      world.AddJoint((Joint) gearJoint);
      return gearJoint;
    }

    public static PulleyJoint CreatePulleyJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 groundAnchorA,
      Vector2 groundAnchorB,
      Vector2 anchorA,
      Vector2 anchorB,
      float ratio)
    {
      PulleyJoint pulleyJoint = new PulleyJoint(bodyA, bodyB, groundAnchorA, groundAnchorB, anchorA, anchorB, ratio);
      world.AddJoint((Joint) pulleyJoint);
      return pulleyJoint;
    }

    public static SliderJoint CreateSliderJoint(
      World world,
      Body bodyA,
      Body bodyB,
      Vector2 anchorA,
      Vector2 anchorB,
      float minLength,
      float maxLength)
    {
      SliderJoint sliderJoint = new SliderJoint(bodyA, bodyB, anchorA, anchorB, minLength, maxLength);
      world.AddJoint((Joint) sliderJoint);
      return sliderJoint;
    }
  }
}
