// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.EPProxy
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace FarseerPhysics.Collision
{
  public class EPProxy
  {
    public Vector2 Centroid;
    public int Count;
    public Vector2[] Normals = new Vector2[Settings.MaxPolygonVertices];
    public Vector2[] Vertices = new Vector2[Settings.MaxPolygonVertices];
  }
}
