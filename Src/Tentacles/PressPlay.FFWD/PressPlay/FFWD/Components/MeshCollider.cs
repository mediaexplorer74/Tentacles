// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.MeshCollider
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class MeshCollider : Collider
  {
    public List<Vertices> vertices { get; set; }

    protected override void DoAddCollider(Body body, float mass)
    {
      Microsoft.Xna.Framework.Vector2 lossyScale = (Microsoft.Xna.Framework.Vector2) this.transform.lossyScale;
      for (int index1 = 0; index1 < this.vertices.Count; ++index1)
      {
        Vertices vertex = this.vertices[index1];
        for (int index2 = 0; index2 < vertex.Count; ++index2)
          vertex[index2] = vertex[index2] * lossyScale;
      }
      this.connectedBody = body;
      Physics.AddMesh(body, this.isTrigger, this.vertices, mass);
    }
  }
}
