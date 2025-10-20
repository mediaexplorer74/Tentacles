// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.Sprite
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;
using System;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class Sprite : Component, PressPlay.FFWD.Interfaces.IUpdateable
  {
    [ContentSerializer(Optional = true)]
    public string Texture;
    private Rectangle _bounds;
    private Texture2D texture;

    public Rectangle bounds
    {
      get
      {
        return new Rectangle((int) this.transform.position.x, (int) this.transform.position.y, this._bounds.Width, this._bounds.Height);
      }
    }

    public event EventHandler<PlayerIndexEventArgs> Selected;

    public Sprite()
    {
    }

    public Sprite(string textureSrc)
      : this()
    {
      this.Texture = textureSrc;
    }

    public override void Awake()
    {
      base.Awake();
      ContentHelper.LoadTexture(this.Texture);
      if (this.gameObject.renderer != null)
        return;
      this.gameObject.AddComponent<SpriteRenderer>(new SpriteRenderer());
    }

    public override void Start()
    {
      base.Start();
      this.texture = ContentHelper.GetTexture(this.Texture);
      if (this.texture != null)
        this._bounds = this.texture.Bounds;
      if (this.gameObject.renderer == null)
        this.gameObject.AddComponent<SpriteRenderer>(new SpriteRenderer());
      ((SpriteRenderer) this.gameObject.renderer).texture = this.texture;
    }

    public void Update()
    {
    }

    public void LateUpdate()
    {
    }
  }
}
