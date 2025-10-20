// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedModel.CpuSkinnedModelPart
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.SkinnedModel
{
  public class CpuSkinnedModelPart
  {
    public readonly string name;
    private readonly int triangleCount;
    private readonly int vertexCount;
    private readonly CpuVertex[] cpuVertices;
    internal Mesh mesh;

    public BasicEffect Effect { get; internal set; }

    internal CpuSkinnedModelPart(
      string name,
      int triangleCount,
      CpuVertex[] vertices,
      IndexBuffer indexBuffer)
    {
      this.name = name;
      this.triangleCount = triangleCount;
      this.vertexCount = vertices.Length;
      this.cpuVertices = vertices;
      this.mesh = new Mesh();
      this.mesh.vertices = new Microsoft.Xna.Framework.Vector3[this.cpuVertices.Length];
      this.mesh.normals = new Microsoft.Xna.Framework.Vector3[this.cpuVertices.Length];
      this.mesh.uv = new Microsoft.Xna.Framework.Vector2[this.cpuVertices.Length];
      this.mesh.triangles = new short[indexBuffer.IndexCount];
      indexBuffer.GetData<short>(this.mesh.triangles);
      for (int index = 0; index < this.cpuVertices.Length; ++index)
        this.mesh.uv[index] = this.cpuVertices[index].TextureCoordinate;
    }

    internal void InitializeMesh(Mesh newMesh)
    {
      newMesh.vertices = this.mesh.vertices;
      newMesh.normals = this.mesh.normals;
      newMesh.uv = this.mesh.uv;
      newMesh.triangles = this.mesh.triangles;
    }

    public void SetBones(Matrix[] bones, ref Matrix world, Mesh mesh)
    {
      for (int index = 0; index < this.vertexCount; ++index)
        CpuSkinningHelpers.SkinVertex(bones, ref this.cpuVertices[index].Position, ref this.cpuVertices[index].Normal, ref world, ref this.cpuVertices[index].BlendIndices, ref this.cpuVertices[index].BlendWeights, out mesh.vertices[index], out mesh.normals[index]);
    }
  }
}
