// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LeaderboardLevelData
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LeaderboardLevelData : Control
  {
    private Level level;
    public PressPlay.FFWD.Vector2 levelTextOffset = PressPlay.FFWD.Vector2.zero;
    public PressPlay.FFWD.Vector2 scoreTextOffset = PressPlay.FFWD.Vector2.zero;
    private LevelResult levelResult;
    public TextControl levelText;
    public TextControl scoreText;

    public LeaderboardLevelData(Level level)
    {
      this.level = level;
      this.levelResult = GlobalManager.Instance.currentProfile.GetLevelResult(level.worldId, level.id);
      this.levelText = new TextControl(level.levelsIndex.ToString(), GUIAssets.berlinsSans40);
      this.levelText.transform.localScale = PressPlay.FFWD.Vector3.one * 0.5f;
      this.AddChild((Control) this.levelText);
      this.scoreText = this.levelResult != null ? new TextControl(this.levelResult.score.ToString(), GUIAssets.berlinsSans40) : new TextControl("0", GUIAssets.berlinsSans40);
      this.AddChild((Control) this.scoreText);
      this.levelText.InvalidateAutoSize();
      this.levelText.AlignCenter(new Rectangle(0, 38, 75, 20));
      this.scoreText.transform.localScale *= 0.6f;
      this.scoreText.InvalidateAutoSize();
      this.scoreText.AlignCenter(new Rectangle(0, 85, 75, 20));
      this.ShowMedals(Application.Load<Texture2D>("Textures/Menu/LevelSelection/LevelSelectionStarAtlas"));
      this.size = new PressPlay.FFWD.Vector2(85f, 50f);
    }

    public void ShowMedals(Texture2D starTexture)
    {
      if (this.levelResult == null)
        return;
      PanelControl child = new PanelControl();
      child.transform.localScale = PressPlay.FFWD.Vector3.one * 0.6f;
      if (this.levelResult.data.allPickupsStar)
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      if (this.levelResult.data.noDeathsStar)
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      if (this.levelResult.data.challengeStar)
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarFilled));
      else
        child.AddChild((Control) new ImageControl(starTexture, SpritePositions.levelSelectStarEmpty));
      child.LayoutRow(0.0f, 0.0f, 10f);
      child.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2((float) (38 - child.bounds.Width / 2), 61f);
      this.AddChild((Control) child);
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
  }
}
