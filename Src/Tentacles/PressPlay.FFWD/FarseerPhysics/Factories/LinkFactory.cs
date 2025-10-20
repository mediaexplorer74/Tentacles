// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Factories.LinkFactory
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Factories
{
  public static class LinkFactory
  {
    public static Path CreateChain(
      World world,
      Vector2 start,
      Vector2 end,
      float linkWidth,
      float linkHeight,
      bool fixStart,
      bool fixEnd,
      int numberOfLinks,
      float linkDensity)
    {
      Path path = new Path();
      path.Add(start);
      path.Add(end);
      PolygonShape polygonShape = new PolygonShape(PolygonTools.CreateRectangle(linkWidth, linkHeight), linkDensity);
      List<Body> bodies = PathManager.EvenlyDistributeShapesAlongPath(world, path, (Shape) polygonShape, BodyType.Dynamic, numberOfLinks);
      if (fixStart)
        JointFactory.CreateFixedRevoluteJoint(world, bodies[0], new Vector2(0.0f, (float) -((double) linkHeight / 2.0)), bodies[0].Position);
      if (fixEnd)
        JointFactory.CreateFixedRevoluteJoint(world, bodies[bodies.Count - 1], new Vector2(0.0f, linkHeight / 2f), bodies[bodies.Count - 1].Position);
      PathManager.AttachBodiesWithRevoluteJoint(world, bodies, new Vector2(0.0f, -linkHeight), new Vector2(0.0f, linkHeight), false, false);
      return path;
    }
  }
}
