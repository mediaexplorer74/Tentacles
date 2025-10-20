// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.UVSpriteSheet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class UVSpriteSheet : MonoBehaviour
  {
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public MeshFilter meshFilter;
    protected Mesh mesh;
    protected Microsoft.Xna.Framework.Vector2[] baseUVs;
    protected Microsoft.Xna.Framework.Vector2[] tmpUVs;
    private static Dictionary<string, Texture2D[,]> textureFrames = new Dictionary<string, Texture2D[,]>();
    public int xCount;
    public int yCount;
    private int currentX = -1;
    private int currentY = -1;
    protected PressPlay.FFWD.Vector2 tileSize;
    public bool scaleMeshUVsOnInitialize = true;
    protected PressPlay.FFWD.Vector2 offset;
    protected bool isInitialized;
    public bool autoInitializeOnStart = true;
    private Texture2D[,] frames;

    public int frameCount => this.xCount * this.yCount;

    public override void Start()
    {
      if (!this.autoInitializeOnStart)
        return;
      this.Initialize();
    }

    public virtual void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      this.tileSize = new PressPlay.FFWD.Vector2(1f / (float) this.xCount, 1f / (float) this.yCount);
      if (this.skinnedMeshRenderer != null)
      {
        this.mesh = UnityObject.Instantiate(this.skinnedMeshRenderer.sharedMesh);
        this.skinnedMeshRenderer.sharedMesh = this.mesh;
      }
      else if (this.meshFilter != null)
        this.mesh = this.meshFilter.mesh;
      if (this.mesh != null)
        this.baseUVs = this.mesh.uv;
      if (this.baseUVs != null)
      {
        this.tmpUVs = new Microsoft.Xna.Framework.Vector2[this.baseUVs.Length];
        for (int index = 0; index < this.baseUVs.Length; ++index)
        {
          if (this.scaleMeshUVsOnInitialize)
          {
            this.baseUVs[index].X /= (float) this.xCount;
            this.baseUVs[index].Y /= (float) this.yCount;
          }
        }
      }
      this.currentX = -1;
      this.currentY = -1;
      this.UpdateUVs(0, 0);
    }

    protected void UpdateUVs(int _x, int _y)
    {
      if (_x == this.currentX && _y == this.currentY)
        return;
      this.currentX = _x;
      this.currentY = _y;
      if (this.frames != null)
      {
        if (this.skinnedMeshRenderer != null)
        {
          this.skinnedMeshRenderer.renderer.material.texture = this.frames[this.currentX, this.currentY];
        }
        else
        {
          if (this.meshFilter == null)
            return;
          this.meshFilter.renderer.material.texture = this.frames[this.currentX, this.currentY];
        }
      }
      else
      {
        if (this.mesh == null)
          return;
        this.mesh.uv = this.CreateUVs(this.currentX, this.currentY);
      }
    }

    protected Microsoft.Xna.Framework.Vector2[] CreateUVs(int _x, int _y)
    {
      if (this.baseUVs == null)
        return (Microsoft.Xna.Framework.Vector2[]) null;
      this.offset = new PressPlay.FFWD.Vector2((float) _x * this.tileSize.x, (float) _y * this.tileSize.y);
      for (int index = 0; index < this.baseUVs.Length; ++index)
      {
        this.tmpUVs[index].X = this.baseUVs[index].X + this.offset.x;
        this.tmpUVs[index].Y = this.baseUVs[index].Y + this.offset.y;
      }
      return this.tmpUVs;
    }

    public int XPosFromIndex(int _index) => _index % this.xCount;

    public int YPosFromIndex(int _index) => _index / this.xCount;

    private void CreateTextureFrames(Material material)
    {
      if (UVSpriteSheet.textureFrames.ContainsKey(material.name))
      {
        this.frames = UVSpriteSheet.textureFrames[material.name];
      }
      else
      {
        this.frames = new Texture2D[this.xCount, this.yCount];
        UVSpriteSheet.textureFrames.Add(material.name, this.frames);
        Texture2D texture = material.texture;
        if (texture == null)
          return;
        int width = (int) ((double) texture.Width * (double) this.tileSize.x);
        int height = (int) ((double) texture.Height * (double) this.tileSize.y);
        for (int index1 = 0; index1 < this.xCount; ++index1)
        {
          for (int index2 = 0; index2 < this.yCount; ++index2)
          {
            Texture2D texture2D = new Texture2D(texture.GraphicsDevice, width, height);
            Microsoft.Xna.Framework.Color[] data = new Microsoft.Xna.Framework.Color[width * height];
            texture.GetData<Microsoft.Xna.Framework.Color>(0, new Rectangle?(new Rectangle(index1 * width, index2 * height, width, height)), data, 0, width * height);
            texture2D.SetData<Microsoft.Xna.Framework.Color>(data);
            this.frames[index1, index2] = texture2D;
          }
        }
      }
    }
  }
}
