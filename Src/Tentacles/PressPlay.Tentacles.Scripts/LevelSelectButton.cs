// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelSelectButton
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelSelectButton : MenuButton
  {
    private int levelId = -1;
    private Level level;
    public PressPlay.FFWD.Vector2 levelTextOffset = PressPlay.FFWD.Vector2.zero;
    public PressPlay.FFWD.Vector2 scoreTextOffset = PressPlay.FFWD.Vector2.zero;
    public bool disabled;
    private bool unlocked;
    private LevelResult levelResult;
    public Rectangle onClickSourceRect = Rectangle.Empty;
    public TextControl levelText;
    public TextControl scoreText;
    private ImageControl glow;
    private bool useCustomClickRect;

    public LevelSelectButton(ButtonStyle buttonStyle, string link, Level level)
      : base(buttonStyle, link)
    {
      this.level = level;
      this.levelId = level.id;
      this.unlocked = GlobalManager.Instance.currentProfile.IsLevelUnlocked(level.id);
      if (!this.unlocked)
        return;
      this.levelResult = GlobalManager.Instance.currentProfile.GetLevelResult(level.worldId, level.id);
      if (this.levelResult == null)
      {
        this.levelText = new TextControl("", GUIAssets.berlinsSans40);
        this.scoreText = new TextControl("", GUIAssets.berlinsSans40);
        this.AddChild((Control) this.levelText);
        this.AddChild((Control) this.scoreText);
      }
      else
      {
        this.levelText = new TextControl(level.levelsIndex.ToString(), GUIAssets.berlinsSans40);
        this.AddChild((Control) this.levelText);
        this.scoreText = new TextControl(this.levelResult.score.ToString(), GUIAssets.berlinsSans40);
        this.scoreText.transform.localScale = PressPlay.FFWD.Vector3.one * 0.7f;
        this.scoreText.InvalidateAutoSize();
        this.AddChild((Control) this.scoreText);
      }
      this.levelText.ignoreSize = true;
      this.scoreText.ignoreSize = true;
    }

    public void AddGlow(Texture2D texture, Rectangle source)
    {
      this.glow = new ImageControl(texture, source);
      this.AddChild((Control) this.glow);
    }

    private void FadeOutColor()
    {
      PressPlay.FFWD.Color color = this.glow.gameObject.renderer.material.color;
      iTween.ColorTo(this.glow.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "color", (object) new PressPlay.FFWD.Color(color.r, color.g, color.b, 0.0f), (object) "oncomplete", (object) "FadeInColor"));
    }

    private void FadeInColor()
    {
      PressPlay.FFWD.Color color = this.glow.gameObject.renderer.material.color;
      iTween.ColorTo(this.glow.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "color", (object) new PressPlay.FFWD.Color(color.r, color.g, color.b, 1f), (object) "oncomplete", (object) "FadeOutColor"));
    }

    public void ShowMedals(Texture2D starTexture, int position)
    {
      if (!this.unlocked || this.levelResult == null)
        return;
      PanelControl panelControl = new PanelControl();
      panelControl.transform.localScale = PressPlay.FFWD.Vector3.one * 0.6f;
      if (this.levelResult.data.allPickupsStar)
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      if (this.levelResult.data.noDeathsStar)
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      if (this.levelResult.data.challengeStar)
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        panelControl.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      panelControl.LayoutRow(0.0f, 0.0f, 10f);
      this.AlignStars(panelControl, position);
      this.AddChild((Control) panelControl);
    }

    private void AlignStars(PanelControl starRow, int pos)
    {
      if (pos == 0)
        starRow.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) (this.bounds.Width / 2 - starRow.bounds.Width / 2 + 5), 60f);
      else if (pos == 3)
        starRow.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) (this.bounds.Width / 2 - starRow.bounds.Width / 2 - 5), 60f);
      else
        starRow.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) (this.bounds.Width / 2 - starRow.bounds.Width / 2), 60f);
    }

    public void SetText(string text) => this.levelText.text = text;

    public void CenterText()
    {
      if (this.levelText != null)
        this.levelText.CenterTextWithinBounds(new Rectangle(this.bounds.X + (int) this.levelTextOffset.x, this.bounds.Y + (int) this.levelTextOffset.y, this.bounds.Width, this.bounds.Height));
      if (this.scoreText == null)
        return;
      this.scoreText.CenterTextWithinBounds(new Rectangle(this.bounds.X + (int) this.scoreTextOffset.x, this.bounds.Y + (int) this.scoreTextOffset.y, this.bounds.Width, this.bounds.Height));
    }

    public override void HandleInput(InputState input)
    {
      if (!this.unlocked)
        return;
      base.HandleInput(input);
    }

    protected bool IsMouseClickWithinBounds(Point p)
    {
      return this.useCustomClickRect ? this.clickRect.Contains(p) : this.bounds.Contains(p);
    }

    protected override bool isTouchWithinBounds(InputState input)
    {
      return this.useCustomClickRect ? this.isTouchWithinBounds(input, this.clickRect) : base.isTouchWithinBounds(input);
    }
  }
}
