// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.Renderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public abstract class Renderer : Component
  {
    internal float renderQueue;

    public Renderer() => this.enabled = true;

    [ContentSerializer(CollectionItemName = "material")]
    public Material[] materials { get; set; }

    [ContentSerializerIgnore]
    public bool enabled { get; set; }

    [ContentSerializerIgnore]
    public Material sharedMaterial
    {
      get => this.material;
      set => this.material = value;
    }

    [ContentSerializerIgnore]
    public Material material
    {
      get
      {
        return this.materials == null || this.materials.Length == 0 ? (Material) null : this.materials[0];
      }
      set
      {
        if (this.materials == null)
          this.materials = new Material[1];
        this.materials[0] = value;
      }
    }

    public abstract int Draw(GraphicsDevice device, Camera cam);

    public override void Awake()
    {
      if (this.material == null)
        this.renderQueue = 0.0f;
      else
        this.renderQueue = this.material.finalRenderQueue;
    }
  }
}
