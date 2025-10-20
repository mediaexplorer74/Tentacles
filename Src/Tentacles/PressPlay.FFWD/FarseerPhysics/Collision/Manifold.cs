// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Manifold
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision
{
  public struct Manifold
  {
    public Vector2 LocalNormal;
    public Vector2 LocalPoint;
    public int PointCount;
    public FixedArray2<ManifoldPoint> Points;
    public ManifoldType Type;
  }
}
