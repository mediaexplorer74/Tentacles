// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.UIRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI.Controls;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.UI
{
  public abstract class UIRenderer : Renderer
  {
    public Rectangle clipRect = Rectangle.Empty;
    protected Control _control;
    private static List<UIRenderer> uiRenderQueue = new List<UIRenderer>();

    public UIRenderer()
    {
      if (this.material != null)
        return;
      this.material = new Material();
      this.material.renderQueue = 1000;
      this.material.SetColor("", PressPlay.FFWD.Color.white);
    }

    protected Control control
    {
      get
      {
        if (this._control == null && this.gameObject != null)
          this._control = this.gameObject.GetComponent<Control>();
        return this._control;
      }
    }

    internal static int doRender(GraphicsDevice device)
    {
      int num = 0;
      Camera.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
      for (int index = 0; index < UIRenderer.uiRenderQueue.Count; ++index)
      {
        if (UIRenderer.uiRenderQueue[index].gameObject != null && UIRenderer.uiRenderQueue[index].enabled && UIRenderer.uiRenderQueue[index].gameObject.active)
        {
          ++num;
          UIRenderer.uiRenderQueue[index].Draw(device, (Camera) null);
        }
      }
      Camera.spriteBatch.End();
      return num;
    }

    internal static void AddRenderer(UIRenderer renderer)
    {
      if (UIRenderer.uiRenderQueue.Contains(renderer))
        return;
      UIRenderer.uiRenderQueue.Add(renderer);
    }

    internal static void RemoveRenderer(UIRenderer renderer)
    {
      if (!UIRenderer.uiRenderQueue.Contains(renderer))
        return;
      UIRenderer.uiRenderQueue.Remove(renderer);
    }
  }
}
