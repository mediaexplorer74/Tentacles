// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelSelection
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelSelection : BackgroundScreen
  {
    private List<Level> levelList;
    private List<LevelSelectButton> buttons = new List<LevelSelectButton>();

    public LevelSelection()
      : base("Textures/Menu/LevelSelection/LevelSelectionBG")
    {
      this.EnabledGestures = GestureType.Tap | GestureType.VerticalDrag | GestureType.Flick | GestureType.DragComplete;
      this.rootControl.transform.position = new Vector3(0.0f, 10f, 0.0f);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.BuildLevelList();
    }

    private void BuildLevelList()
    {
      TrialModeManager.Instance.ForcedUpdateTrial();
      this.levelList = GlobalManager.Instance.database.GetAllLevels();
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/LevelSelection/LevelSelectButtonAtlas");
      ScrollingPanelControl child1 = new ScrollingPanelControl(PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width, this.background.bounds.Height);
      child1.transform.position = child1.transform.position + new Vector3(5f, 150f, 0.0f);
      this.rootControl.AddChild((Control) child1);
      Level level1 = this.levelList[0];
      this.levelList.Remove(level1);
      LevelSelectButton child2 = new LevelSelectButton(new ButtonStyle(texture, SpritePositions.levelSelectionRowTutorial, SpritePositions.levelSelectionRowTutorialHighlighted), level1.id.ToString(), level1);
      child2.OnClickEvent += new EventHandler<EventArgs>(this.OnLevelSelect);
      child2.levelText.text = LocalisationManager.Instance.GetString("menu_petridish");
      child2.levelText.transform.localScale *= 0.7f;
      child2.InvalidateAutoSize();
      child2.levelText.AlignCenter(new Vector2(17f, -2f));
      if (child2.scoreText != null)
        child2.scoreText.text = "";
      child1.AddChild((Control) child2);
      foreach (KeyValuePair<int, LevelBatch> levelsInBatch in GlobalManager.Instance.database.GetLevelsInBatches())
      {
        if (levelsInBatch.Key != 0)
        {
          LevelSelectRow child3 = new LevelSelectRow(levelsInBatch.Key, texture, SpritePositions.levelSelectionRowBackground);
          int num = 0;
          foreach (Level level2 in levelsInBatch.Value.levels)
          {
            LevelSelectButton btn = new LevelSelectButton(!GlobalManager.Instance.currentProfile.IsLevelUnlocked(level2.id) ? new ButtonStyle(texture, SpritePositions.GetLevelButtonGraphic(num, (Level) null)) : new ButtonStyle(texture, SpritePositions.GetLevelButtonGraphic(num, level2), SpritePositions.GetLevelButtonHighlight(num, (Level) null), SpritePositions.GetLevelButtonGraphic(num, level2), SpritePositions.GetLevelButtonGraphic(num, (Level) null)), level2.id.ToString(), level2);
            if (GlobalManager.Instance.currentProfile.IsLevelUnlocked(level2.id) && GlobalManager.Instance.currentProfile.GetLevelResult(level2.worldId, level2.id) == null)
              btn.AddGlow(Application.Load<Texture2D>("Textures/Menu/LevelSelection/LevelSelectButtonAtlas"), SpritePositions.glow[num]);
            btn.OnClickEvent += new EventHandler<EventArgs>(this.OnLevelSelect);
            btn.onClickSourceRect = SpritePositions.GetLevelButtonHighlight(num, (Level) null);
            btn.ShowMedals(Application.Load<Texture2D>("Textures/Menu/LevelSelection/LevelSelectionStarAtlas"), num);
            btn.disabled = true;
            if (btn.levelText != null)
              btn.levelText.AlignCenter(btn.bounds);
            if (btn.scoreText != null)
              btn.scoreText.AlignCenter(btn.bounds);
            switch (num)
            {
              case 0:
                if (btn.levelText != null)
                  btn.levelText.transform.localPosition += new Vector3(5f, 0.0f, -35f);
                if (btn.scoreText != null)
                {
                  btn.scoreText.transform.localPosition += new Vector3(5f, 0.0f, 38f);
                  break;
                }
                break;
              case 3:
                if (btn.levelText != null)
                  btn.levelText.transform.localPosition += new Vector3(-5f, 0.0f, -35f);
                if (btn.scoreText != null)
                {
                  btn.scoreText.transform.localPosition += new Vector3(-5f, 0.0f, 38f);
                  break;
                }
                break;
              default:
                if (btn.levelText != null)
                  btn.levelText.transform.localPosition += new Vector3(0.0f, 0.0f, -35f);
                if (btn.scoreText != null)
                {
                  btn.scoreText.transform.localPosition += new Vector3(0.0f, 0.0f, 38f);
                  break;
                }
                break;
            }
            child3.AddButtonToPanel((Control) btn);
            this.buttons.Add(btn);
            ++num;
          }
          child3.panel.LayoutRow();
          child1.AddChild((Control) child3);
        }
      }
      child1.LayoutColumn();
      this.SetTransitionPositionOnControls(1f);
    }

    public override void OnTransitionOnComplete()
    {
      base.OnTransitionOnComplete();
      foreach (LevelSelectButton button in this.buttons)
        button.disabled = false;
    }

    private void OnLevelSelect(object sender, EventArgs e)
    {
      Level level = GlobalManager.Instance.database.GetLevel(int.Parse(((ButtonControlEventArgs) e).link));
      MusicController.Instance.FadeTo(0.0f, 0.5f);
      GlobalManager.Instance.OpenLevel(level);
      this.ExitScreen();
    }

    private void OnButtonPress(object sender, EventArgs e)
    {
    }
  }
}
