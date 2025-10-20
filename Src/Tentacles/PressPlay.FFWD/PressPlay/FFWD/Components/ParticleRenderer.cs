// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.ParticleRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class ParticleRenderer : Renderer
  {
    public float lengthScale;
    public float velocityScale;
    public float maxParticleSize;
    public PressPlay.FFWD.Vector3 uvAnimation;
    [ContentSerializerIgnore]
    public bool doViewportCulling;
    private ParticleEmitter emitter;
    private VertexPositionColorTexture[] vertices;
    private short[] triangles;
    [ContentSerializerIgnore]
    public Rectangle ParticleBounds;

    public override void Awake()
    {
      base.Awake();
      this.emitter = this.gameObject.GetComponent<ParticleEmitter>();
      this.vertices = new VertexPositionColorTexture[this.emitter.particlesToAllocate() * 4];
      this.triangles = new short[this.emitter.particles.Length * 6];
      int num = 0;
      int index = 0;
      while (index < this.emitter.particles.Length * 6)
      {
        this.triangles[index] = (short) num;
        this.triangles[index + 1] = (short) (num + 2);
        this.triangles[index + 2] = (short) (num + 1);
        this.triangles[index + 3] = (short) (num + 1);
        this.triangles[index + 4] = (short) (num + 2);
        this.triangles[index + 5] = (short) (num + 3);
        index += 6;
        num += 4;
      }
    }

    public Rectangle GetSourceRect()
    {
      return new Rectangle(0, 0, this.material.texture.Width, this.material.texture.Height);
    }

    public bool IsVisible(Viewport viewport)
    {
      if (this.ParticleBounds.Left == int.MinValue)
        return false;
      bool result = true;
      viewport.Bounds.Intersects(ref this.ParticleBounds, out result);
      return result;
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.emitter.particles == null || this.emitter.particleCount == 0)
        return 0;
      cam.BasicEffect.World = Matrix.Identity;
      cam.BasicEffect.VertexColorEnabled = true;
      this.material.SetTextureState(cam.BasicEffect);
      this.material.SetBlendState(device);
      int num = 0;
      for (int index = 0; index < this.emitter.particles.Length && num < this.emitter.particleCount; ++index)
      {
        Particle particle = this.emitter.particles[index];
        if ((double) particle.Energy > 0.0 && this.RenderParticle(cam, num * 4, num * 6, ref particle))
          ++num;
      }
      if (num == 0)
        return 0;
      foreach (EffectPass pass in cam.BasicEffect.CurrentTechnique.Passes)
      {
        pass.Apply();
        device.DrawUserIndexedPrimitives<VertexPositionColorTexture>(PrimitiveType.TriangleList, this.vertices, 0, num * 4, this.triangles, 0, num * 2);
      }
      return 1;
    }

    private bool RenderParticle(
      Camera cam,
      int vertexIndex,
      int triangleIndex,
      ref Particle particle)
    {
      float num = particle.Size / 2f;
      if ((double) num <= 0.0)
        return false;
      Microsoft.Xna.Framework.Vector3 position = (Microsoft.Xna.Framework.Vector3) particle.Position;
      if (!this.emitter.useWorldSpace)
        position += (Microsoft.Xna.Framework.Vector3) this.transform.position;
      if (this.doViewportCulling)
      {
        BoundingSphere sphere = new BoundingSphere(position, num);
        if (cam.DoFrustumCulling(ref sphere))
          return false;
      }
      this.vertices[vertexIndex].TextureCoordinate = new Microsoft.Xna.Framework.Vector2(particle.TextureOffset.x, particle.TextureOffset.y + particle.TextureScale.y);
      this.vertices[vertexIndex].Color = particle.Color;
      this.vertices[vertexIndex + 1].TextureCoordinate = new Microsoft.Xna.Framework.Vector2(particle.TextureOffset.x, particle.TextureOffset.y);
      this.vertices[vertexIndex + 1].Color = particle.Color;
      this.vertices[vertexIndex + 2].TextureCoordinate = new Microsoft.Xna.Framework.Vector2(particle.TextureOffset.x + particle.TextureScale.x, particle.TextureOffset.y + particle.TextureScale.y);
      this.vertices[vertexIndex + 2].Color = particle.Color;
      this.vertices[vertexIndex + 3].TextureCoordinate = new Microsoft.Xna.Framework.Vector2(particle.TextureOffset.x + particle.TextureScale.x, particle.TextureOffset.y);
      this.vertices[vertexIndex + 3].Color = particle.Color;
      if ((double) particle.Rotation != 0.0)
      {
        Matrix rotationY = Matrix.CreateRotationY(particle.Rotation);
        Microsoft.Xna.Framework.Vector3 result = new Microsoft.Xna.Framework.Vector3(num, (float) vertexIndex * 0.0001f, num);
        Microsoft.Xna.Framework.Vector3.Transform(ref result, ref rotationY, out result);
        this.vertices[vertexIndex].Position = position + new Microsoft.Xna.Framework.Vector3(-result.Z, (float) vertexIndex * 0.0001f, result.X);
        this.vertices[vertexIndex + 1].Position = position + new Microsoft.Xna.Framework.Vector3(-result.X, (float) vertexIndex * 0.0001f, -result.Z);
        this.vertices[vertexIndex + 2].Position = position + new Microsoft.Xna.Framework.Vector3(result.X, (float) vertexIndex * 0.0001f, result.Z);
        this.vertices[vertexIndex + 3].Position = position + new Microsoft.Xna.Framework.Vector3(result.Z, (float) vertexIndex * 0.0001f, -result.X);
      }
      else
      {
        this.vertices[vertexIndex].Position = position + new Microsoft.Xna.Framework.Vector3(-num, (float) vertexIndex * 0.0001f, num);
        this.vertices[vertexIndex + 1].Position = position + new Microsoft.Xna.Framework.Vector3(-num, (float) vertexIndex * 0.0001f, -num);
        this.vertices[vertexIndex + 2].Position = position + new Microsoft.Xna.Framework.Vector3(num, (float) vertexIndex * 0.0001f, num);
        this.vertices[vertexIndex + 3].Position = position + new Microsoft.Xna.Framework.Vector3(num, (float) vertexIndex * 0.0001f, -num);
      }
      return true;
    }
  }
}
