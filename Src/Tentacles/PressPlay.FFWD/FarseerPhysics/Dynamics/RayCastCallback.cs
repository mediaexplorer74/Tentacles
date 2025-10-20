// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.RayCastCallback
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public delegate float RayCastCallback(
    Fixture fixture,
    Vector2 point,
    Vector2 normal,
    float fraction);
}
