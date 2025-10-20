// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ButtonComponent
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ButtonComponent : Component, PressPlay.FFWD.Interfaces.IUpdateable
  {
    [ContentSerializer(Optional = true)]
    public string OnNormalTexture;
    public string OnActiveTexture;
    private Rectangle _bounds;
    private Texture2D normalTexture;
    private Texture2D activeTexture;
    public PressPlay.FFWD.Vector2 textOffset = PressPlay.FFWD.Vector2.zero;
    public SpriteFont font;
    public string buttonText = "";
    public float delayBeforeActivation = 0.1f;
    private float onSelectTime;
    private bool isTapped;

    public Rectangle bounds
    {
      get
      {
        return new Rectangle((int) this.transform.position.x, (int) this.transform.position.y, this._bounds.Width, this._bounds.Height);
      }
    }

    public event EventHandler<PlayerIndexEventArgs> Selected;

    protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
    {
      if (!this.isTapped)
      {
        this.onSelectTime = Time.time;
        this.isTapped = true;
      }
      ((SpriteRenderer) this.gameObject.renderer).texture = this.activeTexture;
    }

    public ButtonComponent()
    {
    }

    public ButtonComponent(string normalTextureSrc, string activeTextureSrc)
      : this()
    {
      this.OnNormalTexture = normalTextureSrc;
      this.OnActiveTexture = activeTextureSrc;
    }

    public override void Awake()
    {
      base.Awake();
      ContentHelper.LoadTexture(this.OnNormalTexture);
      ContentHelper.LoadTexture(this.OnActiveTexture);
      if (this.gameObject.renderer == null)
        this.gameObject.AddComponent<ButtonRenderer>(new ButtonRenderer());
      if (this.gameObject.renderer.material == null)
        this.gameObject.renderer.material = new Material();
      this.gameObject.renderer.material.renderQueue = 1000;
    }

    public override void Start()
    {
      base.Start();
      this.normalTexture = ContentHelper.GetTexture(this.OnNormalTexture);
      this.activeTexture = ContentHelper.GetTexture(this.OnActiveTexture);
      if (this.normalTexture != null)
        this._bounds = this.normalTexture.Bounds;
      if (this.gameObject.renderer == null)
        this.gameObject.AddComponent<SpriteRenderer>(new SpriteRenderer());
      ((SpriteRenderer) this.renderer).texture = this.normalTexture;
      ((SpriteRenderer) this.renderer).bounds = this.bounds;
    }

    public void OnTweenUpdate(float value)
    {
    }

    public void Update()
    {
      if (!this.isTapped || (double) Time.time <= (double) this.onSelectTime + (double) this.delayBeforeActivation)
        return;
      if (this.Selected != null)
        this.Selected((object) this, new PlayerIndexEventArgs(PlayerIndex.One));
      this.isTapped = false;
    }

    public void LateUpdate()
    {
    }
  }
}
