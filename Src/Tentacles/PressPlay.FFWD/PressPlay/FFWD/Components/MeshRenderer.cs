// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.MeshRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class MeshRenderer : Renderer
  {
    private MeshFilter filter;

    public override void Start()
    {
      base.Start();
      this.filter = (MeshFilter) this.GetComponent(typeof (MeshFilter));
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.filter == null)
        return 0;
      BoundingSphere sphere = new BoundingSphere((Microsoft.Xna.Framework.Vector3) this.transform.position, this.filter.boundingSphere.Radius * this.transform.lossyScale.sqrMagnitude);
      if (cam.DoFrustumCulling(ref sphere))
        return 0;
      if (this.filter.CanBatch())
        return cam.BatchRender<MeshFilter>(this.filter, this.material, this.transform);
      ModelMesh modelMesh = this.filter.GetModelMesh();
      if (modelMesh == null)
        return 0;
      Matrix world = this.transform.world;
      cam.BasicEffect.World = world;
      cam.BasicEffect.VertexColorEnabled = false;
      this.material.SetBlendState(device);
      this.material.SetTextureState(cam.BasicEffect);
      foreach (EffectPass pass in cam.BasicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        for (int index = 0; index < ((ReadOnlyCollection<ModelMeshPart>) modelMesh.MeshParts).Count; ++index)
        {
          ModelMeshPart meshPart = ((ReadOnlyCollection<ModelMeshPart>) modelMesh.MeshParts)[index];
          device.SetVertexBuffer(meshPart.VertexBuffer);
          device.Indices = meshPart.IndexBuffer;
          device.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
        }
      }
      return 1;
    }
  }
}
