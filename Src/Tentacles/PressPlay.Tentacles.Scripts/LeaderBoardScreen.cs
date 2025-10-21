// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LeaderBoardScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LeaderBoardScreen : BackgroundScreen
  {
    private const int LeaderboardPageSize = 13;
    // SignedInGamer and LeaderboardReader classes are not available in MonoGame, so we replace them with placeholders
    // private SignedInGamer gamer;
    // private LeaderboardReader leaderboardReader;
    private object gamer;
    private object leaderboardReader;
    private List<TentacleLeaderboardRow> debugData = new List<TentacleLeaderboardRow>();
    private ScrollingPanelControl leaderboardPanel;
    private bool isCommuncating;
    private List<LeaderBoardButton> buttons = new List<LeaderBoardButton>();
    private TextControl displayText;
    private TextControl leaderboardHeadline;
    private TextControl leaderboardTitle;
    private int currentLoadedBatch = -1;

    public LeaderBoardScreen(string background)
      : base(background)
    {
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 10f, 0.0f);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.GenerateLeaderboardMenu();
    }

    private void GenerateLeaderboardMenu()
    {
      ImageControl child1 = new ImageControl(Application.Load<Texture2D>("Textures/Menu/LeaderBoards/Leaderboards_bg_overlay"));
      this.rootControl.AddChild((Control) child1);
      child1.transform.position = new PressPlay.FFWD.Vector3(0.0f, 500f, 0.0f);
      this.leaderboardHeadline = new TextControl(LocalisationManager.Instance.GetString("menu_leaderboards").ToUpperInvariant(), GUIAssets.berlinsSans40);
      this.leaderboardHeadline.transform.localScale *= 1f;
      this.leaderboardHeadline.InvalidateAutoSize();
      this.rootControl.AddChild((Control) this.leaderboardHeadline);
      this.leaderboardHeadline.transform.position = new PressPlay.FFWD.Vector3(0.0f, 501f, 0.0f);
      this.leaderboardHeadline.CenterTextWithinBounds(new Rectangle(PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 0, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 50));
      this.leaderboardTitle = new TextControl("", GUIAssets.berlinsSans40);
      this.rootControl.AddChild((Control) this.leaderboardTitle);
      this.leaderboardTitle.transform.localScale = PressPlay.FFWD.Vector3.one * 0.7f;
      this.leaderboardTitle.transform.position = new PressPlay.FFWD.Vector3(0.0f, 501f, 0.0f);
      this.displayText = new TextControl("", GUIAssets.berlinsSans40);
      this.displayText.transform.localScale = PressPlay.FFWD.Vector3.one * 0.4f;
      this.rootControl.AddChild((Control) this.displayText);
      this.SetDisplayText(LocalisationManager.Instance.GetString("FLB_select_leaderboard"));
      ScrollingPanelControl child2 = new ScrollingPanelControl(100, this.background.bounds.Height);
      child2.transform.position = child2.transform.position + new PressPlay.FFWD.Vector3(0.0f, 50f, 0.0f);
      this.rootControl.AddChild((Control) child2);
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/LeaderBoards/Leaderboards_buttonAtlas");
      foreach (int batch in GlobalManager.Instance.database.GetBatches(1))
      {
        if (batch != 0)
        {
          LeaderBoardButton child3 = new LeaderBoardButton(batch, new ButtonStyle(texture, this.GetButtonSpritePositionFromBatchId(batch), SpritePositions.leaderboardHighlighted)
          {
            [3] = this.GetSelectedButtonSpritePositionFromBatchId(batch)
          }, GameDatabase.GetLeaderboardIdFromBatchId(batch).ToString());
          child3.OnClickEvent += new EventHandler<EventArgs>(this.LoadLeaderboard);
          this.buttons.Add(child3);
          child2.AddChild((Control) child3);
        }
      }
      child2.LayoutColumn();
      this.SetTransitionPositionOnControls(1f);
    }

    private Rectangle GetSelectedButtonSpritePositionFromBatchId(int batchId)
    {
      switch (batchId)
      {
        case 1:
        case 3:
        case 5:
        case 8:
          return SpritePositions.leaderboardGreenSelected;
        case 2:
        case 6:
        case 9:
          return SpritePositions.leaderboardOrangeSelected;
        case 4:
        case 7:
        case 10:
          return SpritePositions.leaderboardBlueSelected;
        default:
          return SpritePositions.leaderboardBlueSelected;
      }
    }

    private Rectangle GetButtonSpritePositionFromBatchId(int batchId)
    {
      switch (batchId)
      {
        case 1:
        case 3:
        case 5:
        case 8:
          return SpritePositions.leaderboardGreen;
        case 2:
        case 6:
        case 9:
          return SpritePositions.leaderboardOrange;
        case 4:
        case 7:
        case 10:
          return SpritePositions.leaderboardBlue;
        default:
          return SpritePositions.leaderboardBlue;
      }
    }

    private void SetDisplayText(string text)
    {
      this.displayText.text = text;
      this.displayText.AlignCenter(new Rectangle(440, 100, 325, 350));
    }

    private void SetTitleText(string text)
    {
      this.leaderboardTitle.text = text;
      this.leaderboardTitle.AlignCenter(new Rectangle(440, 50, 325, 45));
    }

    private void GenerateDebugData(int number)
    {
      if (this.leaderboardPanel != null)
      {
        this.rootControl.RemoveChild((Control) this.leaderboardPanel);
        UnityObject.Destroy((UnityObject) this.leaderboardPanel.gameObject);
      }
      this.leaderboardPanel = new ScrollingPanelControl(200, 800);
      this.leaderboardPanel.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(440f, 90f);
      for (int index = 0; index < number; ++index)
      {
        if (index == 0)
          this.leaderboardPanel.AddChild((Control) new LeaderBoardTextRow(index + 1, "WWWWWWWWWWWWWWW", "00000", true));
        else
          this.leaderboardPanel.AddChild((Control) new LeaderBoardTextRow(index + 1, "WWWWWWWWWWWWWWW", "00000", false));
      }
      this.leaderboardPanel.LayoutColumn();
      this.rootControl.AddChild((Control) this.leaderboardPanel);
    }

    private void LoadLeaderboard(object sender, EventArgs e)
    {
      if (this.isCommuncating)
        Debug.Log((object) "We are already communicating. Wait a minute!");
      else if (int.Parse(((ButtonControlEventArgs) e).link) == -1)
      {
        Debug.Log((object) "LeaderBoardScreen: Leaderboard ID is invalid");
      }
      else
      {
        if (this.leaderboardPanel != null)
        {
          this.rootControl.RemoveChild((Control) this.leaderboardPanel);
          UnityObject.Destroy((UnityObject) this.leaderboardPanel.gameObject);
          this.leaderboardPanel = (ScrollingPanelControl) null;
        }
        this.displayText.gameObject.active = true;
        this.SetDisplayText(LocalisationManager.Instance.GetString("label_loading"));
        this.currentLoadedBatch = int.Parse(((ButtonControlEventArgs) e).link);
        this.SetTitleText(LocalisationManager.Instance.GetString("FLB_batch" + this.currentLoadedBatch.ToString()));
        int leaderboardIdFromBatchId = GameDatabase.GetLeaderboardIdFromBatchId(int.Parse(((ButtonControlEventArgs) e).link));
        this.SetHighlightedButton(int.Parse(((ButtonControlEventArgs) e).link));
        if (GlobalManager.titleUpdateDeclined)
        {
          this.AddCouldNotConnectMessage();
        }
        else
        {
          Debug.Log((object) "BeginLeaderboardRead");
          this.isCommuncating = true;
          // Gamer and LeaderboardReader classes are not available in MonoGame, so we skip this functionality
          // this.gamer = Gamer.SignedInGamers[PlayerIndex.One];
          // LeaderboardReader.BeginRead(LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, leaderboardIdFromBatchId), (Gamer) this.gamer, 13, new AsyncCallback(this.LeaderboardReadCallback), (object) this.gamer);
        }
      }
    }

    private void AddCouldNotConnectMessage()
    {
      this.SetDisplayText(LocalisationManager.Instance.GetString("FLB_could_not_connect"));
    }

    // LeaderboardReadCallback is not available in MonoGame, so we skip this functionality
    /*
    protected void LeaderboardReadCallback(IAsyncResult result)
    {
      if (this.ScreenState == ScreenState.TransitionOff)
        return;
      this.gamer = result.AsyncState as SignedInGamer;
      if (this.gamer != null)
      {
        try
        {
          this.leaderboardReader = LeaderboardReader.EndRead(result);
        }
        catch (Exception ex)
        {
          Debug.Log((object) ("The leaderboard recieved reader exception:  " + ex.ToString()));
          this.AddCouldNotConnectMessage();
        }
      }
      if (this.leaderboardReader != null)
        this.BuildLeaderboardList(this.leaderboardReader);
      this.isCommuncating = false;
    }
    */

    // BuildLeaderboardList is not available in MonoGame, so we skip this functionality
    /*
    private void BuildLeaderboardList(LeaderboardReader lr)
    {
      this.displayText.gameObject.active = false;
      this.SetTitleText(LocalisationManager.Instance.GetString("FLB_batch" + this.currentLoadedBatch.ToString()));
      if (this.leaderboardPanel != null)
      {
        this.rootControl.RemoveChild((Control) this.leaderboardPanel);
        UnityObject.Destroy((UnityObject) this.leaderboardPanel.gameObject);
        this.leaderboardPanel = (ScrollingPanelControl) null;
      }
      this.leaderboardPanel = new ScrollingPanelControl(200, 600);
      this.leaderboardPanel.transform.position = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2(440f, 90f);
      if (lr.Entries.Count == 0)
      {
        this.SetDisplayText(LocalisationManager.Instance.GetString("FLB_no_friends_in_leaderboard"));
      }
      else
      {
        for (int index = 0; index < lr.Entries.Count; ++index)
        {
          LeaderboardEntry entry = lr.Entries[index];
          this.leaderboardPanel.AddChild((Control) new LeaderBoardTextRow(index + 1, entry.Gamer.Gamertag, entry.Columns.GetValueInt32("BestScore").ToString(), entry.Gamer.Gamertag == this.gamer.Gamertag));
        }
      }
      this.leaderboardPanel.LayoutColumn();
      this.rootControl.AddChild((Control) this.leaderboardPanel);
    }
    */

    private void OnBackButton(object sender, ButtonControlEventArgs e) => this.ExitScreen();

    private void SetHighlightedButton(int id)
    {
      foreach (LeaderBoardButton button in this.buttons)
      {
        if (button.batchId == id)
          button.state = ButtonControlStates.disabled;
        else
          button.state = ButtonControlStates.normal;
      }
    }
  }
}
