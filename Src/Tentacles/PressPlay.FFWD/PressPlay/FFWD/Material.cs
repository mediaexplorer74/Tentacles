// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Material
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class Material : Asset
  {
    [ContentSerializer]
    private string shader;
    [ContentSerializer]
    public int renderQueue;
    [ContentSerializer(Optional = true)]
    public Color color;
    [ContentSerializer(Optional = true)]
    public string mainTexture;
    [ContentSerializer(Optional = true)]
    public Vector2 mainTextureOffset = Vector2.zero;
    [ContentSerializer(Optional = true)]
    public Vector2 mainTextureScale = Vector2.one;
    [ContentSerializer(Optional = true)]
    internal bool wrapRepeat;
    [ContentSerializerIgnore]
    public Texture2D texture;
    private static readonly Dictionary<string, int> textureRenderIndexes = new Dictionary<string, int>();
    internal float finalRenderQueue = float.MinValue;
    public static readonly Material Default = new Material();

    public void SetColor(string name, Color color) => this.color = color;

    protected override void DoLoadAsset(AssetHelper assetHelper)
    {
      if (this.mainTexture != null)
        this.texture = assetHelper.Load<Texture2D>("Textures/" + this.mainTexture);
      this.blendState = BlendState.Opaque;
      if (this.shader == "iPhone/Particles/Additive Culled")
        this.blendState = BlendState.Additive;
      else if (this.renderQueue == 3000 || this.shader == "TransperantNoLight")
        this.blendState = BlendState.AlphaBlend;
      if (this.shader == "Particles/Multiply (Double)")
        this.color = new Color(this.color.r, this.color.g, this.color.b, 0.5f);
      this.CalculateRenderQueue();
    }

    [ContentSerializerIgnore]
    public BlendState blendState { get; private set; }

    internal void SetBlendState(GraphicsDevice device)
    {
      if (device.BlendState != this.blendState)
        device.BlendState = this.blendState;
      device.DepthStencilState = this.renderQueue == 3000 || this.shader == "TransperantNoLight" ? DepthStencilState.DepthRead : DepthStencilState.Default;
      if (this.wrapRepeat)
        device.SamplerStates[0] = SamplerState.LinearWrap;
      else
        device.SamplerStates[0] = SamplerState.LinearClamp;
    }

    internal void CalculateRenderQueue()
    {
      this.finalRenderQueue = (float) (this.renderQueue * 10);
      if (this.blendState == BlendState.AlphaBlend)
        this.finalRenderQueue += 1000f;
      if (this.blendState == BlendState.Additive)
        this.finalRenderQueue += 2000f;
      if (!Material.textureRenderIndexes.ContainsKey(this.mainTexture ?? string.Empty))
        Material.textureRenderIndexes.Add(this.mainTexture ?? string.Empty, Material.textureRenderIndexes.Count);
      this.finalRenderQueue += (float) Material.textureRenderIndexes[this.mainTexture ?? string.Empty];
    }

    internal void SetTextureState(BasicEffect basicEffect)
    {
      if (this.texture != null)
      {
        basicEffect.TextureEnabled = true;
        basicEffect.Texture = this.texture;
        basicEffect.DiffuseColor = (Microsoft.Xna.Framework.Vector3) Color.white;
      }
      else
      {
        basicEffect.TextureEnabled = false;
        basicEffect.DiffuseColor = (Microsoft.Xna.Framework.Vector3) this.color;
      }
    }
  }
}
