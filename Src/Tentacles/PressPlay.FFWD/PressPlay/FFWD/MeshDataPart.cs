// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.MeshDataPart
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#nullable disable
namespace PressPlay.FFWD
{
  internal class MeshDataPart
  {
    [ContentSerializer]
    internal Microsoft.Xna.Framework.Vector3[] vertices;
    [ContentSerializer]
    internal Microsoft.Xna.Framework.Vector3[] normals;
    [ContentSerializer]
    internal Microsoft.Xna.Framework.Vector2[] uv;
    [ContentSerializer]
    internal short[] triangles;
    [ContentSerializer]
    internal BoundingSphere boundingSphere;
  }
}
