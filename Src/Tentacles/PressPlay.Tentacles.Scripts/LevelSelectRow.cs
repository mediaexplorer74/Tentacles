// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelSelectRow
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelSelectRow : Control
  {
    public PanelControl panel;
    private TextControl batchName;
    private TextControl batchScore;

    public LevelSelectRow(int batch, Texture2D texture, Rectangle sourceRect)
    {
      this.AddChild((Control) new ImageControl(texture, sourceRect));
      this.panel = new PanelControl();
      this.panel.transform.position = new PressPlay.FFWD.Vector3(this.panel.transform.position.x, this.panel.transform.position.y, 35f);
      this.AddChild((Control) this.panel);
      this.batchName = new TextControl(LocalisationManager.Instance.GetString("FLB_batch" + batch.ToString()), GUIAssets.berlinsSans40);
      this.batchName.transform.localScale *= 0.6f;
      this.batchName.InvalidateAutoSize();
      this.AddChild((Control) this.batchName);
      this.batchName.AlignCenter(new PressPlay.FFWD.Vector2(0.0f, -78f));
      this.batchScore = new TextControl(LocalisationManager.Instance.GetString("FLB_Total") + ": " + GlobalManager.Instance.database.GetLevelsInBatch(1, batch).GetTotalScore().ToString(), GUIAssets.berlinsSans40);
      this.batchScore.transform.localScale *= 0.6f;
      this.batchScore.InvalidateAutoSize();
      this.AddChild((Control) this.batchScore);
      this.batchScore.AlignCenter(new Rectangle(0, 131, 375, 100));
    }

    public void AddButtonToPanel(Control btn) => this.panel.AddChild(btn);
  }
}
