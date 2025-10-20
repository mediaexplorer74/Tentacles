// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Contacts.ContactConstraintPoint
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics.Contacts
{
  public sealed class ContactConstraintPoint
  {
    public Vector2 LocalPoint;
    public float NormalImpulse;
    public float NormalMass;
    public float TangentImpulse;
    public float TangentMass;
    public float VelocityBias;
    public Vector2 rA;
    public Vector2 rB;
  }
}
