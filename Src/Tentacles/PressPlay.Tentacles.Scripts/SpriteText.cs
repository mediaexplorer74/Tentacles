// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpriteText
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpriteText : MonoBehaviour
  {
    private TextRenderer textRenderer;
    private bool drawInWorld = true;
    public Camera renderCamera;
    public string text = "Hello World";
    public float characterSize = 1f;
    public SpriteText.Anchor_Pos anchor;
    public Color color = Color.white;

    public void SetColor(Color c)
    {
      this.color = c;
      this.textRenderer.material.color = c;
    }

    public string Text
    {
      get => this.text;
      set => this.text = value;
    }

    public void SetCamera(Camera c) => this.renderCamera = c;

    public override void Start()
    {
      base.Start();
      this.SetColor(this.color);
    }

    public override void Update()
    {
      base.Update();
      if (!this.drawInWorld)
        return;
      this.textRenderer.Position = (Vector2) this.renderCamera.WorldToViewportPoint(this.transform.position);
    }

    protected override void Destroy() => base.Destroy();

    public enum Anchor_Pos
    {
      Upper_Left,
      Upper_Center,
      Upper_Right,
      Middle_Left,
      Middle_Center,
      Middle_Right,
      Lower_Left,
      Lower_Center,
      Lower_Right,
    }
  }
}
