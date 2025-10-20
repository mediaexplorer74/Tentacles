// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LeaderBoardButton
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LeaderBoardButton : MenuButton
  {
    public PanelControl panel;
    public int batchId;
    private TextControl batchName;
    private TextControl batchScore;

    public LeaderBoardButton(int batch, ButtonStyle buttonStyle, string link)
      : base(buttonStyle, link)
    {
      this.batchId = batch;
      this.panel = new PanelControl();
      this.panel.transform.position = new PressPlay.FFWD.Vector3(this.panel.transform.position.x, this.panel.transform.position.y, 35f);
      this.AddChild((Control) this.panel);
      LevelBatch levelsInBatch = GlobalManager.Instance.database.GetLevelsInBatch(1, batch);
      this.batchName = new TextControl(LocalisationManager.Instance.GetString("FLB_batch" + batch.ToString()), GUIAssets.berlinsSans40);
      this.batchName.transform.localScale *= 0.6f;
      this.batchName.InvalidateAutoSize();
      this.AddChild((Control) this.batchName);
      this.batchName.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, -63f));
      this.batchScore = new TextControl(LocalisationManager.Instance.GetString("FLB_Total") + ": " + GlobalManager.Instance.database.GetLevelsInBatch(1, batch).GetTotalScore().ToString(), GUIAssets.berlinsSans40);
      this.batchScore.transform.localScale *= 0.6f;
      this.batchScore.InvalidateAutoSize();
      this.AddChild((Control) this.batchScore);
      this.batchScore.AlignCenter(new Rectangle(0, 110, 390, 110));
      this.DisplayLevelData(levelsInBatch);
    }

    private void DisplayLevelData(LevelBatch levelBatch)
    {
      PanelControl child = new PanelControl();
      child.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(10f, 30f);
      this.AddChild((Control) child);
      foreach (Level level in levelBatch.levels)
        child.AddChild((Control) new LeaderboardLevelData(level));
      child.LayoutRow(20f, 0.0f, 0.0f);
    }
  }
}
