// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PoolableText
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PoolableText : PoolableObject
  {
    private float moveStartTime;
    private float moveDuration;
    private Vector3 moveStartPosition;
    private Vector3 moveEndPosition;
    private bool isMoving;
    private float lingerStartTime;
    private float lingerDuration;
    private bool isLingering;
    private float fadeStartTime;
    private float fadeDuration;
    private Color fadeStartColor;
    private Color fadeEndColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    private bool isFading;
    private TextRenderer3D _textRenderer;

    private TextRenderer3D textRenderer
    {
      get
      {
        if (this._textRenderer == null)
          this._textRenderer = this.gameObject.AddComponent<TextRenderer3D>(new TextRenderer3D(GUIAssets.berlinsSans40));
        return this._textRenderer;
      }
    }

    public string text
    {
      set => this.textRenderer.text = value;
    }

    public SpriteFont font
    {
      set => this.textRenderer.font = value;
    }

    public override void Update()
    {
      base.Update();
      if (this.isMoving)
      {
        if ((double) Time.time > (double) this.moveStartTime + (double) this.moveDuration)
        {
          this.isMoving = false;
          this.transform.position = this.moveEndPosition;
          this.StartLinger();
        }
        else
          this.transform.position = Vector3.Lerp(this.moveStartPosition, this.moveEndPosition, (Time.time - this.moveStartTime) / this.moveDuration);
      }
      if (this.isLingering && (double) Time.time > (double) this.lingerStartTime + (double) this.lingerDuration)
      {
        this.isLingering = false;
        this.StartFade();
      }
      if (!this.isFading)
        return;
      if ((double) Time.time > (double) this.fadeStartTime + (double) this.fadeDuration)
      {
        this.isFading = false;
        this.DeActivate();
      }
      else
        this.SetColor(Color.Lerp(this.fadeStartColor, this.fadeEndColor, (Time.time - this.fadeStartTime) / this.fadeDuration));
    }

    public void SetTiming(float moveDuration, float lingerDuration, float fadeDuration)
    {
      this.moveDuration = moveDuration;
      this.fadeDuration = fadeDuration;
      this.lingerDuration = lingerDuration;
    }

    public override void Activate()
    {
      base.Activate();
      this.textRenderer.textSize = 0.8f;
      this.StartMove();
    }

    private void StartMove()
    {
      this.moveStartTime = Time.time;
      this.moveStartPosition = this.transform.position;
      this.moveEndPosition = this.transform.position + this.transform.forward * 1.1f;
      this.isMoving = true;
    }

    private void StartLinger()
    {
      this.lingerStartTime = Time.time;
      this.isLingering = true;
    }

    private void StartFade()
    {
      this.fadeStartTime = Time.time;
      this.fadeStartColor = this.renderer.material.color;
      this.fadeEndColor.r = this.fadeStartColor.r;
      this.fadeEndColor.g = this.fadeStartColor.g;
      this.fadeEndColor.b = this.fadeStartColor.b;
      this.isFading = true;
    }

    public override void DeActivate() => base.DeActivate();

    public override void Return() => base.Return();

    public void SetText(string txt) => this.text = txt;

    public void SetColor(Color color) => this.renderer.material.color = color;
  }
}
