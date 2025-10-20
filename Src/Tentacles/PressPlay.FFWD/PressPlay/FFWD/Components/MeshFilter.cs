// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.MeshFilter
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class MeshFilter : Component
  {
    private Mesh _mesh;

    [ContentSerializer(ElementName = "mesh", Optional = true)]
    public Mesh sharedMesh { get; set; }

    [ContentSerializerIgnore]
    public Mesh mesh
    {
      get
      {
        if (this._mesh == null && this.sharedMesh != null)
          this._mesh = (Mesh) this.sharedMesh.Clone();
        return this._mesh;
      }
      set => this._mesh = value;
    }

    public BoundingSphere boundingSphere
    {
      get
      {
        if (this._mesh != null)
          return this._mesh.boundingSphere;
        return this.sharedMesh != null ? this.sharedMesh.boundingSphere : new BoundingSphere();
      }
    }

    public override void Awake()
    {
      base.Awake();
      Mesh mesh = this.mesh;
    }

    internal bool CanBatch() => this.mesh != null && this.mesh.vertices != null;

    public ModelMesh GetModelMesh()
    {
      return this.sharedMesh != null ? this.sharedMesh.GetModelMesh() : (ModelMesh) null;
    }
  }
}
