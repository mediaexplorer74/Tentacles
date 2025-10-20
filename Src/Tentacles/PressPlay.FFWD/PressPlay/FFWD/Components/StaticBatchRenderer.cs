// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.StaticBatchRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class StaticBatchRenderer : Renderer
  {
    [ContentSerializer]
    internal BoundingSphere boundingSphere;
    [ContentSerializer]
    internal VertexPositionTexture[] vertices;
    [ContentSerializer]
    internal short[] indices;
    private VertexBuffer vertexBuffer;
    private IndexBuffer indexBuffer;

    public override void Awake()
    {
      base.Awake();
      this.vertexBuffer = new VertexBuffer(Application.screenManager.GraphicsDevice, typeof (VertexPositionTexture), this.vertices.Length, BufferUsage.WriteOnly);
      this.vertexBuffer.SetData<VertexPositionTexture>(this.vertices);
      this.indexBuffer = new IndexBuffer(Application.screenManager.GraphicsDevice, IndexElementSize.SixteenBits, this.indices.Length, BufferUsage.WriteOnly);
      this.indexBuffer.SetData<short>(this.indices);
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (cam.DoFrustumCulling(ref this.boundingSphere))
        return 0;
      cam.BasicEffect.World = Matrix.Identity;
      cam.BasicEffect.VertexColorEnabled = false;
      this.material.SetTextureState(cam.BasicEffect);
      this.material.SetBlendState(device);
      device.SetVertexBuffer(this.vertexBuffer);
      device.Indices = this.indexBuffer;
      foreach (EffectPass pass in cam.BasicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, this.vertexBuffer.VertexCount, 0, this.indexBuffer.IndexCount / 3);
      }
      return 1;
    }
  }
}
