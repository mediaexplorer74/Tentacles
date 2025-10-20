// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.DynamicBatchRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class DynamicBatchRenderer
  {
    private Material currentMaterial = Material.Default;
    private GraphicsDevice device;
    private int batchVertexSize;
    private int batchIndexSize;
    private int currentVertexIndex;
    private int currentIndexIndex;
    private VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[20000];
    private Microsoft.Xna.Framework.Vector3[] positionData = new Microsoft.Xna.Framework.Vector3[600];
    private short[] indexData = new short[60000];

    public DynamicBatchRenderer(GraphicsDevice device) => this.device = device;

    internal int Draw<T>(Camera cam, Material material, T verts, Transform transform)
    {
      int num = 0;
      if (this.currentMaterial.name != material.name)
      {
        num = this.DoDraw(this.device, cam);
        this.currentMaterial = material;
      }
      this.Add<T>(verts, transform);
      return num;
    }

    private void EndBatch()
    {
      this.currentMaterial = Material.Default;
      this.batchVertexSize = 0;
      this.batchIndexSize = 0;
      this.currentVertexIndex = 0;
      this.currentIndexIndex = 0;
    }

    private void Add<T>(T model, Transform transform)
    {
      Matrix transform1 = transform != null ? transform.world : Matrix.Identity;
      if (model is MeshFilter meshFilter)
        this.PrepareMesh(meshFilter.mesh, ref transform1);
      if (!(model is Mesh mesh))
        return;
      this.PrepareMesh(mesh, ref transform1);
    }

    internal int DoDraw(GraphicsDevice device, Camera cam)
    {
      if (this.currentIndexIndex == 0)
        return 0;
      cam.BasicEffect.World = Matrix.Identity;
      cam.BasicEffect.VertexColorEnabled = false;
      this.currentMaterial.SetTextureState(cam.BasicEffect);
      this.currentMaterial.SetBlendState(device);
      foreach (EffectPass pass in cam.BasicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        device.DrawUserIndexedPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, this.vertexData, 0, this.currentVertexIndex, this.indexData, 0, this.currentIndexIndex / 3);
      }
      this.EndBatch();
      return 1;
    }

    private void PrepareMesh(Mesh mesh, ref Matrix transform)
    {
      this.batchVertexSize += mesh.vertices.Length;
      if (this.vertexData.Length < this.batchVertexSize)
      {
        VertexPositionNormalTexture[] positionNormalTextureArray = new VertexPositionNormalTexture[this.batchVertexSize];
        this.vertexData.CopyTo((Array) positionNormalTextureArray, 0);
        this.vertexData = positionNormalTextureArray;
      }
      this.batchIndexSize += mesh.triangles.Length;
      if (this.indexData.Length < this.batchIndexSize)
      {
        short[] numArray = new short[this.batchIndexSize];
        this.indexData.CopyTo((Array) numArray, 0);
        this.indexData = numArray;
      }
      if (this.positionData.Length < mesh.vertices.Length)
        this.positionData = new Microsoft.Xna.Framework.Vector3[mesh.vertices.Length];
      if (transform != Matrix.Identity)
        Microsoft.Xna.Framework.Vector3.Transform(mesh.vertices, ref transform, this.positionData);
      else
        mesh.vertices.CopyTo((Array) this.positionData, 0);
      for (int index = 0; index < mesh.vertices.Length; ++index)
      {
        this.vertexData[this.currentVertexIndex + index].Position = this.positionData[index];
        this.vertexData[this.currentVertexIndex + index].TextureCoordinate = mesh.uv == null ? Microsoft.Xna.Framework.Vector2.Zero : mesh.uv[index];
        this.vertexData[this.currentVertexIndex + index].Normal = mesh.normals == null ? Microsoft.Xna.Framework.Vector3.Zero : mesh.normals[index];
      }
      for (int index = 0; index < mesh.triangles.Length; ++index)
        this.indexData[this.currentIndexIndex + index] = (short) ((int) mesh.triangles[index] + this.currentVertexIndex);
      this.currentVertexIndex += mesh.vertices.Length;
      this.currentIndexIndex += mesh.triangles.Length;
    }
  }
}
