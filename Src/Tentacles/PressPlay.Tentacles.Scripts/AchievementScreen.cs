// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AchievementScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD;
using PressPlay.FFWD.UI.Controls;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AchievementScreen : BackgroundScreen
  {
    private bool hasReceivedAchievements;
    private object achievementsLockObject = new object();
    private AchievementCollection achievements;
    private int earnedGamerScore;
    private int maxGamerScore;
    private ScrollingPanelControl achievementList;

    public AchievementScreen(string background)
      : base(background)
    {
      SignedInGamer signedInGamer = Gamer.SignedInGamers[PlayerIndex.One];
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 10f, 0.0f);
      TextControl child = new TextControl(LocalisationManager.Instance.GetString("menu_menu_btn_achievements").ToUpperInvariant(), GUIAssets.berlinsSans40);
      child.transform.localScale *= 1f;
      child.InvalidateAutoSize();
      this.rootControl.AddChild((Control) child);
      child.transform.position = new PressPlay.FFWD.Vector3(0.0f, 501f, 0.0f);
      child.CenterTextWithinBounds(new Rectangle(PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 0, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 50));
      signedInGamer?.BeginGetAchievements(new AsyncCallback(this.GetAchievementsCallback), (object) signedInGamer);
      this.EnabledGestures = GestureType.Tap | GestureType.VerticalDrag | GestureType.Flick | GestureType.DragComplete;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.SetTransitionPositionOnControls(1f);
    }

    private void BuildDebugList()
    {
      this.achievementList = new ScrollingPanelControl(100, 800);
      this.achievementList.size = new PressPlay.FFWD.Vector2(800f, 1500f);
      this.achievementList.transform.position = this.achievementList.transform.position + new PressPlay.FFWD.Vector3(0.0f, 50f, 0.0f);
      this.rootControl.AddChild((Control) this.achievementList);
      for (int index = 0; index < 20; ++index)
        this.achievementList.AddChild((Control) new AchievementListItem(Application.Load<Texture2D>("Textures/Menu/Achievements/achievementBoxAtlas"), SpritePositions.achievementBoxOpen, (Achievement) null));
      this.achievementList.LayoutColumn(0.0f, 0.0f, 20f);
      this.achievementList.Init();
    }

    protected void GetAchievementsCallback(IAsyncResult result)
    {
      if (!(result.AsyncState is SignedInGamer asyncState))
        return;
      lock (this.achievementsLockObject)
      {
        this.maxGamerScore = 0;
        this.earnedGamerScore = 0;
        this.achievements = asyncState.EndGetAchievements(result);
        for (int index = 0; index < this.achievements.Count; ++index)
        {
          Achievement achievement = this.achievements[index];
          this.maxGamerScore += achievement.GamerScore;
          if (achievement.IsEarned)
            this.earnedGamerScore += achievement.GamerScore;
        }
      }
      this.hasReceivedAchievements = true;
    }

    private void BuildAchievementList()
    {
      this.achievementList = new ScrollingPanelControl(100, 800);
      this.achievementList.transform.position = this.achievementList.transform.position + new PressPlay.FFWD.Vector3(0.0f, 50f, 0.0f);
      this.rootControl.AddChild((Control) this.achievementList);
      for (int index = 0; index < this.achievements.Count; ++index)
      {
        Achievement achievement = this.achievements[index];
        this.achievementList.AddChild(!achievement.IsEarned ? (Control) new AchievementListItem(Application.Load<Texture2D>("Textures/Menu/Achievements/achievementBoxAtlas"), SpritePositions.achievementBoxLocked, achievement) : (Control) new AchievementListItem(Application.Load<Texture2D>("Textures/Menu/Achievements/achievementBoxAtlas"), SpritePositions.achievementBoxOpen, achievement));
      }
      this.achievementList.LayoutColumn(0.0f, 0.0f, 20f);
      this.achievementList.Init();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      if (!this.hasReceivedAchievements)
        return;
      lock (this.achievementsLockObject)
      {
        this.BuildAchievementList();
        this.hasReceivedAchievements = false;
      }
    }

    private void OnBackButton(object sender, ButtonControlEventArgs e) => this.ExitScreen();
  }
}
