// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AchievementListItem
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.UI.Controls;
using System.IO;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AchievementListItem : Control
  {
    private TextControl name;
    private TextControl description;
    private string nameText = "";
    private string descriptionText = "";
    // Achievement class is not available in MonoGame, so we replace it with a placeholder
    // private Achievement achievement;
    private object achievement;
    private Texture2D icon;
    private PressPlay.FFWD.Color lockedColor = PressPlay.FFWD.Color.black;
    private PressPlay.FFWD.Color unlockedColor = PressPlay.FFWD.Color.white;

    public AchievementListItem(Texture2D texture, Rectangle sourceRect, object achievement)
    {
      // this.achievement = achievement;
      this.AddChild((Control) new ImageControl(texture, sourceRect));
      // Achievement class is not available in MonoGame, so we skip this functionality
      /*
      if (achievement != null)
      {
        using (Stream picture = achievement.GetPicture())
          this.icon = Texture2D.FromStream(PressPlay.FFWD.ScreenManager.ScreenManager.Graphics, picture);
      }
      if (achievement == null)
      {
        this.nameText = "Debug!";
        this.descriptionText = "Continue playing to unlock this secret achievement.";
      }
      else if (achievement.IsEarned)
      {
        this.nameText = achievement.Name;
        this.descriptionText = achievement.Description;
        if (this.icon != null)
          this.SetIcon(this.icon);
      }
      else if (achievement.DisplayBeforeEarned)
      {
        this.nameText = achievement.Name;
        this.descriptionText = achievement.HowToEarn;
      }
      else
      {
        this.nameText = "Secret";
        this.descriptionText = "Continue playing to unlock this secret achievement.";
      }
      */
      // Set default values for nameText and descriptionText
      this.nameText = "Secret";
      this.descriptionText = "Continue playing to unlock this secret achievement.";
      this.name = new TextControl(this.nameText, GUIAssets.berlinsSans40);
      this.name.useWordWrap = false;
      this.name.ignoreSize = true;
      this.name.transform.localPosition = new PressPlay.FFWD.Vector3(90f, 0.0f, 10f);
      this.name.transform.localScale = new PressPlay.FFWD.Vector3(0.45f);
      this.AddChild((Control) this.name);
      this.description = new TextControl(this.descriptionText, GUIAssets.defaultFont);
      this.description.useWordWrap = true;
      this.description.ignoreSize = true;
      this.description.transform.localPosition = new PressPlay.FFWD.Vector3(90f, 0.0f, 30f);
      this.description.transform.localScale = new PressPlay.FFWD.Vector3(0.9f);
      if (achievement != null)
      {
        /*
        if (achievement.IsEarned)
          this.description.SetColor(this.unlockedColor);
        else
          this.description.SetColor(this.lockedColor);
        */
      }
      this.description.size = new PressPlay.FFWD.Vector2(300f, 50f);
      this.AddChild((Control) this.description);
    }

    public void SetIcon(Texture2D texture)
    {
      ImageControl child = new ImageControl(texture);
      child.transform.localPosition = new PressPlay.FFWD.Vector3(8f, 0.0f, 8f);
      this.AddChild((Control) child);
    }

    public void SetText(string gamerTag, string score)
    {
      this.name.text = gamerTag;
      this.description.text = score;
    }
  }
}
