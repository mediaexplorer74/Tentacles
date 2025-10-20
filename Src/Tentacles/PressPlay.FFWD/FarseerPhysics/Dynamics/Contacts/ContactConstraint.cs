// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Contacts.ContactConstraint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Contacts
{
  public sealed class ContactConstraint
  {
    public Body BodyA;
    public Body BodyB;
    public float Friction;
    public Mat22 K;
    public Vector2 LocalNormal;
    public Vector2 LocalPoint;
    public Manifold Manifold;
    public Vector2 Normal;
    public Mat22 NormalMass;
    public int PointCount;
    public ContactConstraintPoint[] Points = new ContactConstraintPoint[Settings.MaxPolygonVertices];
    public float RadiusA;
    public float RadiusB;
    public float Restitution;
    public ManifoldType Type;

    public ContactConstraint()
    {
      for (int index = 0; index < 2; ++index)
        this.Points[index] = new ContactConstraintPoint();
    }
  }
}
