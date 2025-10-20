// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Mesh
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.SkinnedModel;
using System.Collections.ObjectModel;

#nullable disable
namespace PressPlay.FFWD
{
  public class Mesh : Asset
  {
    [ContentSerializerIgnore]
    public Model model;
    [ContentSerializerIgnore]
    public CpuSkinnedModel skinnedModel;
    private int meshIndex;
    [ContentSerializerIgnore]
    public Microsoft.Xna.Framework.Vector3[] vertices;
    [ContentSerializerIgnore]
    public Microsoft.Xna.Framework.Vector3[] normals;
    [ContentSerializerIgnore]
    public Microsoft.Xna.Framework.Vector2[] uv;
    [ContentSerializerIgnore]
    public short[] triangles;
    internal BoundingSphere boundingSphere;

    public string asset { get; set; }

    protected override void DoLoadAsset(AssetHelper assetHelper)
    {
      if (string.IsNullOrEmpty(this.asset))
        return;
      MeshData meshData = assetHelper.Load<MeshData>("Models/" + this.asset);
      if (meshData == null)
        return;
      this.boundingSphere = meshData.boundingSphere;
      this.skinnedModel = meshData.skinnedModel;
      if (this.skinnedModel != null)
      {
        for (int index = 0; index < this.skinnedModel.Parts.Count; ++index)
        {
          if (this.skinnedModel.Parts[index].name == this.name)
          {
            this.meshIndex = index;
            this.skinnedModel.Parts[index].InitializeMesh(this);
            break;
          }
        }
      }
      this.model = meshData.model;
      if (this.model != null)
      {
        for (int index = 0; index < ((ReadOnlyCollection<ModelMesh>) this.model.Meshes).Count; ++index)
        {
          if (((ReadOnlyCollection<ModelMesh>) this.model.Meshes)[index].Name == this.name)
          {
            this.meshIndex = index;
            this.boundingSphere = ((ReadOnlyCollection<ModelMesh>) this.model.Meshes)[index].BoundingSphere;
            break;
          }
        }
      }
      if (meshData.meshParts.Count <= 0)
        return;
      MeshDataPart meshPart = meshData.meshParts[this.name];
      if (meshPart == null)
        return;
      this.vertices = (Microsoft.Xna.Framework.Vector3[]) meshPart.vertices.Clone();
      this.triangles = (short[]) meshPart.triangles.Clone();
      this.uv = (Microsoft.Xna.Framework.Vector2[]) meshPart.uv.Clone();
      if (meshPart.normals != null)
        this.normals = (Microsoft.Xna.Framework.Vector3[]) meshPart.normals.Clone();
      this.boundingSphere = meshPart.boundingSphere;
    }

    public void Clear()
    {
      this.vertices = (Microsoft.Xna.Framework.Vector3[]) null;
      this.normals = (Microsoft.Xna.Framework.Vector3[]) null;
      this.uv = (Microsoft.Xna.Framework.Vector2[]) null;
      this.triangles = (short[]) null;
    }

    internal ModelMesh GetModelMesh()
    {
      return this.model != null ? ((ReadOnlyCollection<ModelMesh>) this.model.Meshes)[this.meshIndex] : (ModelMesh) null;
    }

    public CpuSkinnedModelPart GetSkinnedModelPart()
    {
      return this.skinnedModel != null ? this.skinnedModel.Parts[this.meshIndex] : (CpuSkinnedModelPart) null;
    }

    internal override UnityObject Clone()
    {
      Mesh mesh = new Mesh();
      mesh.skinnedModel = this.skinnedModel;
      mesh.model = this.model;
      mesh.meshIndex = this.meshIndex;
      if (this.vertices != null)
      {
        mesh.vertices = (Microsoft.Xna.Framework.Vector3[]) this.vertices.Clone();
        mesh.triangles = (short[]) this.triangles.Clone();
        mesh.uv = (Microsoft.Xna.Framework.Vector2[]) this.uv.Clone();
        if (this.normals != null)
          mesh.normals = (Microsoft.Xna.Framework.Vector3[]) this.normals.Clone();
      }
      mesh.boundingSphere = this.boundingSphere;
      return (UnityObject) mesh;
    }

    public override string ToString()
    {
      return string.Format("{0} - {1} ({2})", (object) this.GetType().Name, (object) this.asset, (object) this.GetInstanceID());
    }
  }
}
