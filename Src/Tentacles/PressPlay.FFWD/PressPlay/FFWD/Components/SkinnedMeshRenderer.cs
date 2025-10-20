// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.SkinnedMeshRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.SkinnedModel;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class SkinnedMeshRenderer : Renderer
  {
    private Animation animation;

    public Mesh sharedMesh { get; set; }

    public override void Awake()
    {
      base.Awake();
      this.animation = this.GetComponentInParents<Animation>();
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.sharedMesh == null || this.sharedMesh.skinnedModel == null)
        return 0;
      BoundingSphere sphere = new BoundingSphere((Microsoft.Xna.Framework.Vector3) this.transform.position, this.sharedMesh.boundingSphere.Radius * this.transform.lossyScale.sqrMagnitude);
      if (cam.DoFrustumCulling(ref sphere))
        return 0;
      CpuSkinnedModelPart skinnedModelPart = this.sharedMesh.GetSkinnedModelPart();
      Matrix world = this.transform.world;
      skinnedModelPart.SetBones(this.animation.GetTransforms(), ref world, this.sharedMesh);
      return cam.BatchRender<Mesh>(this.sharedMesh, this.sharedMaterial, (Transform) null);
    }
  }
}
