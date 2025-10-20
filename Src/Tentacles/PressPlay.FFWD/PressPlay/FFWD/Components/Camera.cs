// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.Camera
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.UI;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class Camera : Component, IComparer<Camera>, IComparer<Renderer>
  {
    public static bool wireframeRender = false;
    private static int estimatedDrawCalls = 0;
    private static DynamicBatchRenderer dynamicBatchRenderer;
    internal static SpriteBatch spriteBatch;
    internal static BasicEffect basicEffect;
    private PressPlay.FFWD.Color _backgroundColor = PressPlay.FFWD.Color.black;
    private static List<Camera> _allCameras = new List<Camera>();
    public static Viewport FullScreen;
    private Matrix _projectionMatrix = Matrix.Identity;
    internal static List<Renderer> nonAssignedRenderers = new List<Renderer>();
    private readonly List<Renderer> renderQueue = new List<Renderer>(50);

    public Camera()
    {
      this.fieldOfView = MathHelper.ToRadians(60f);
      this.nearClipPlane = 0.3f;
      this.farClipPlane = 1000f;
    }

    public float fieldOfView { get; set; }

    public float nearClipPlane { get; set; }

    public float farClipPlane { get; set; }

    public float orthographicSize { get; set; }

    public bool orthographic { get; set; }

    public int depth { get; set; }

    public float aspect { get; set; }

    public int cullingMask { get; set; }

    [ContentSerializerIgnore]
    public BoundingFrustum frustum { get; private set; }

    [ContentSerializerIgnore]
    public SpriteBatch SpriteBatch => Camera.spriteBatch;

    [ContentSerializerIgnore]
    public BasicEffect BasicEffect => Camera.basicEffect;

    public PressPlay.FFWD.Color backgroundColor
    {
      get => this._backgroundColor;
      set => this._backgroundColor = new PressPlay.FFWD.Color(value.r, value.g, value.b, 1f);
    }

    public Rectangle rect { get; set; }

    public Camera.ClearFlags clearFlags { get; set; }

    public override void Awake()
    {
      this.frustum = new BoundingFrustum(this.view * this.projectionMatrix);
      for (int index = Camera.nonAssignedRenderers.Count - 1; index >= 0; --index)
      {
        if (Camera.nonAssignedRenderers[index] == null || Camera.nonAssignedRenderers[index].gameObject == null || this.addRenderer(Camera.nonAssignedRenderers[index]))
          Camera.nonAssignedRenderers.RemoveAt(index);
      }
      for (int index1 = 0; index1 < Camera._allCameras.Count; ++index1)
      {
        for (int index2 = 0; index2 < Camera._allCameras[index1].renderQueue.Count; ++index2)
          this.addRenderer(Camera._allCameras[index1].renderQueue[index2]);
      }
      Camera._allCameras.Add(this);
      Camera._allCameras.Sort((IComparer<Camera>) this);
      if (!this.gameObject.CompareTag("MainCamera") || Camera.main != null)
        return;
      Camera.main = this;
    }

    public static void RemoveCamera(Camera cam) => Camera._allCameras.Remove(cam);

    public static IEnumerable<Camera> allCameras => (IEnumerable<Camera>) Camera._allCameras;

    public static Camera main { get; private set; }

    public Matrix view { get; private set; }

    [ContentSerializerIgnore]
    public Viewport viewPort => Camera.FullScreen;

    [ContentSerializerIgnore]
    public Matrix projectionMatrix
    {
      get
      {
        if (this._projectionMatrix == Matrix.Identity)
          Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(this.fieldOfView), Camera.FullScreen.AspectRatio, this.nearClipPlane, this.farClipPlane, out this._projectionMatrix);
        return this._projectionMatrix;
      }
    }

    public PressPlay.FFWD.Ray ScreenPointToRay(PressPlay.FFWD.Vector2 screen)
    {
      PressPlay.FFWD.Vector3 origin = (PressPlay.FFWD.Vector3) this.viewPort.Unproject((Microsoft.Xna.Framework.Vector3) new PressPlay.FFWD.Vector3(screen.x, screen.y, 0.0f), this.projectionMatrix, this.view, Matrix.Identity);
      PressPlay.FFWD.Vector3 vector3 = (PressPlay.FFWD.Vector3) this.viewPort.Unproject((Microsoft.Xna.Framework.Vector3) new PressPlay.FFWD.Vector3(screen.x, screen.y, 1f), this.projectionMatrix, this.view, Matrix.Identity);
      return new PressPlay.FFWD.Ray(origin, (vector3 - origin).normalized);
    }

    public PressPlay.FFWD.Vector3 WorldToViewportPoint(PressPlay.FFWD.Vector3 position)
    {
      return (PressPlay.FFWD.Vector3) this.viewPort.Project((Microsoft.Xna.Framework.Vector3) position, this.projectionMatrix, this.view, Matrix.Identity);
    }

    internal static void AddRenderer(Renderer renderer)
    {
      if (renderer is UIRenderer)
      {
        UIRenderer.AddRenderer(renderer as UIRenderer);
      }
      else
      {
        bool flag = false;
        for (int index = 0; index < Camera._allCameras.Count; ++index)
          flag |= Camera._allCameras[index].addRenderer(renderer);
        if (flag || Camera.nonAssignedRenderers.Contains(renderer))
          return;
        Camera.nonAssignedRenderers.Add(renderer);
      }
    }

    private bool addRenderer(Renderer renderer)
    {
      if (this.renderQueue.Contains(renderer))
        return true;
      if ((this.cullingMask & 1 << renderer.gameObject.layer) <= 0)
        return false;
      int index = this.renderQueue.BinarySearch(renderer, (IComparer<Renderer>) this);
      if (index < 0)
        this.renderQueue.Insert(~index, renderer);
      else
        this.renderQueue.Insert(index, renderer);
      return true;
    }

    internal static void RemoveRenderer(Renderer renderer)
    {
      if (renderer is UIRenderer)
      {
        UIRenderer.RemoveRenderer(renderer as UIRenderer);
      }
      else
      {
        for (int index = 0; index < Camera._allCameras.Count; ++index)
          Camera._allCameras[index].removeRenderer(renderer);
      }
    }

    private void removeRenderer(Renderer renderer) => this.renderQueue.Remove(renderer);

    internal static void DoRender(GraphicsDevice device)
    {
      if (Camera.dynamicBatchRenderer == null)
        Camera.dynamicBatchRenderer = new DynamicBatchRenderer(device);
      Camera.estimatedDrawCalls = 0;
      if (device == null)
        return;
      device.BlendState = BlendState.Opaque;
      if (Camera.wireframeRender)
        device.RasterizerState = new RasterizerState()
        {
          FillMode = FillMode.WireFrame
        };
      for (int index = 0; index < Camera._allCameras.Count; ++index)
      {
        if (Camera._allCameras[index].gameObject.active)
          Camera._allCameras[index].doRender(device);
      }
      if (Camera.wireframeRender)
        device.RasterizerState = RasterizerState.CullCounterClockwise;
      Camera.estimatedDrawCalls += UIRenderer.doRender(device);
    }

    internal void doRender(GraphicsDevice device)
    {
      this.Clear(device);
      this.view = Matrix.CreateLookAt((Microsoft.Xna.Framework.Vector3) this.transform.position, (Microsoft.Xna.Framework.Vector3) (this.transform.position + this.transform.forward), (Microsoft.Xna.Framework.Vector3) this.transform.up);
      this.frustum.Matrix = this.view * this.projectionMatrix;
      if (Camera.wireframeRender)
        TextRenderer3D.batch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, DepthStencilState.DepthRead, new RasterizerState()
        {
          FillMode = FillMode.WireFrame,
          CullMode = CullMode.None
        }, (Effect) TextRenderer3D.basicEffect);
      else
        TextRenderer3D.batch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, DepthStencilState.DepthRead, RasterizerState.CullNone, (Effect) TextRenderer3D.basicEffect);
      this.BasicEffect.View = this.view;
      this.BasicEffect.Projection = this.projectionMatrix;
      int num = 0;
      for (int index = 0; index < this.renderQueue.Count; ++index)
      {
        if (this.renderQueue[index].gameObject != null)
        {
          if (this.renderQueue[index].material.renderQueue != num)
          {
            if (num > 0)
              Camera.estimatedDrawCalls += Camera.dynamicBatchRenderer.DoDraw(device, this);
            num = this.renderQueue[index].material.renderQueue;
          }
          if (this.renderQueue[index].gameObject.active && this.renderQueue[index].enabled)
            Camera.estimatedDrawCalls += this.renderQueue[index].Draw(device, this);
        }
      }
      TextRenderer3D.batch.End();
      Camera.estimatedDrawCalls += Camera.dynamicBatchRenderer.DoDraw(device, this);
    }

    private void Clear(GraphicsDevice device)
    {
      switch (this.clearFlags)
      {
        case Camera.ClearFlags.Skybox:
          device.Clear((Microsoft.Xna.Framework.Color) this.backgroundColor);
          break;
        case Camera.ClearFlags.Color:
          device.Clear((Microsoft.Xna.Framework.Color) this.backgroundColor);
          break;
        case Camera.ClearFlags.Depth:
          device.Clear(ClearOptions.DepthBuffer, (Microsoft.Xna.Framework.Color) this.backgroundColor, 1f, 0);
          break;
      }
    }

    public int Compare(Camera x, Camera y) => x.depth.CompareTo(y.depth);

    public int Compare(Renderer x, Renderer y) => x.renderQueue.CompareTo(y.renderQueue);

    internal static Camera FindByName(string name)
    {
      for (int index = 0; index < Camera._allCameras.Count; ++index)
      {
        if (Camera._allCameras[index].name == name)
          return Camera._allCameras[index];
      }
      return (Camera) null;
    }

    internal int BatchRender<T>(T data, Material material, Transform transform)
    {
      return Camera.dynamicBatchRenderer.Draw<T>(this, material, data, transform);
    }

    internal bool DoFrustumCulling(ref BoundingSphere sphere)
    {
      if ((double) sphere.Radius == 0.0)
        return false;
      ContainmentType result;
      this.frustum.Contains(ref sphere, out result);
      return result == ContainmentType.Disjoint;
    }

    public enum ClearFlags
    {
      Skybox,
      Color,
      Depth,
      Nothing,
    }
  }
}
