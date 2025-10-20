// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.PrimitiveBatch
// Assembly: Tentacles, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 94733B2D-6956-40B2-A474-EF03B0110429
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\Tentacles.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace PressPlay.Tentacles
{
  public class PrimitiveBatch : IDisposable
  {
    private const int DefaultBufferSize = 500;
    private BasicEffect _basicEffect;
    private GraphicsDevice _device;
    private bool _hasBegun;
    private bool _isDisposed;
    private VertexPositionColor[] _lineVertices;
    private int _lineVertsCount;
    private VertexPositionColor[] _triangleVertices;
    private int _triangleVertsCount;

    public PrimitiveBatch(GraphicsDevice graphicsDevice)
      : this(graphicsDevice, 500)
    {
    }

    public PrimitiveBatch(GraphicsDevice graphicsDevice, int bufferSize)
    {
      this._device = graphicsDevice != null ? graphicsDevice : throw new ArgumentNullException(nameof (graphicsDevice));
      this._triangleVertices = new VertexPositionColor[bufferSize - bufferSize % 3];
      this._lineVertices = new VertexPositionColor[bufferSize - bufferSize % 2];
      this._basicEffect = new BasicEffect(graphicsDevice);
      this._basicEffect.VertexColorEnabled = true;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public void SetProjection(ref Matrix projection) => this._basicEffect.Projection = projection;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this._isDisposed)
        return;
      if (this._basicEffect != null)
        this._basicEffect.Dispose();
      this._isDisposed = true;
    }

    public void Begin(ref Matrix projection, ref Matrix view)
    {
      if (this._hasBegun)
        throw new InvalidOperationException("End must be called before Begin can be called again.");
      this._basicEffect.Projection = projection;
      this._basicEffect.View = view;
      this._basicEffect.World = Matrix.Identity;
      this._basicEffect.CurrentTechnique.Passes[0].Apply();
      this._hasBegun = true;
    }

    public bool IsReady() => this._hasBegun;

    public void AddVertex(Vector2 vertex, Color color, PrimitiveType primitiveType)
    {
      if (!this._hasBegun)
        throw new InvalidOperationException("Begin must be called before AddVertex can be called.");
      if (primitiveType == PrimitiveType.LineStrip || primitiveType == PrimitiveType.TriangleStrip)
        throw new NotSupportedException("The specified primitiveType is not supported by PrimitiveBatch.");
      if (primitiveType == PrimitiveType.TriangleList)
      {
        if (this._triangleVertsCount >= this._triangleVertices.Length)
          this.FlushTriangles();
        this._triangleVertices[this._triangleVertsCount].Position = new Vector3(vertex, -0.1f);
        this._triangleVertices[this._triangleVertsCount].Color = color;
        ++this._triangleVertsCount;
      }
      if (primitiveType != PrimitiveType.LineList)
        return;
      if (this._lineVertsCount >= this._lineVertices.Length)
        this.FlushLines();
      this._lineVertices[this._lineVertsCount].Position = new Vector3(vertex, 0.0f);
      this._lineVertices[this._lineVertsCount].Color = color;
      ++this._lineVertsCount;
    }

    public void End()
    {
      if (!this._hasBegun)
        throw new InvalidOperationException("Begin must be called before End can be called.");
      this.FlushTriangles();
      this.FlushLines();
      this._hasBegun = false;
    }

    private void FlushTriangles()
    {
      if (!this._hasBegun)
        throw new InvalidOperationException("Begin must be called before Flush can be called.");
      if (this._triangleVertsCount < 3)
        return;
      int primitiveCount = this._triangleVertsCount / 3;
      this._device.SamplerStates[0] = SamplerState.AnisotropicClamp;
      this._device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, this._triangleVertices, 0, primitiveCount);
      this._triangleVertsCount -= primitiveCount * 3;
    }

    private void FlushLines()
    {
      if (!this._hasBegun)
        throw new InvalidOperationException("Begin must be called before Flush can be called.");
      if (this._lineVertsCount < 2)
        return;
      int primitiveCount = this._lineVertsCount / 2;
      this._device.SamplerStates[0] = SamplerState.AnisotropicClamp;
      this._device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, this._lineVertices, 0, primitiveCount);
      this._lineVertsCount -= primitiveCount * 2;
    }
  }
}
