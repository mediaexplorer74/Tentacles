// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LeaderBoardTextRow
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LeaderBoardTextRow : PanelControl
  {
    private TextControl gamerTagText;
    private TextControl scoreText;

    public LeaderBoardTextRow(int place, string gamerTag, string score, bool isPlayer)
    {
      this.gamerTagText = new TextControl(place.ToString() + ". " + gamerTag, GUIAssets.berlinsSans40);
      this.gamerTagText.transform.localScale *= 0.39f;
      this.gamerTagText.InvalidateAutoSize();
      this.gamerTagText.transform.localPosition = new Vector3(-5f, this.gamerTagText.transform.localPosition.y, this.gamerTagText.transform.localPosition.z);
      this.AddChild((Control) this.gamerTagText);
      this.scoreText = new TextControl(score, GUIAssets.berlinsSans40);
      this.scoreText.transform.localScale = Vector3.one * 0.39f;
      this.scoreText.InvalidateAutoSize();
      this.scoreText.transform.localPosition = new Vector3(275f, this.scoreText.transform.localPosition.y, this.scoreText.transform.localPosition.z);
      this.AddChild((Control) this.scoreText);
      this.size = new Vector2(200f, 24f);
      if (!isPlayer)
        return;
      this.gamerTagText.SetColor(Color.cyan);
      this.scoreText.SetColor(Color.cyan);
    }

    public void SetText(string gamerTag, string score)
    {
      this.gamerTagText.text = gamerTag;
      this.scoreText.text = score;
    }
  }
}
