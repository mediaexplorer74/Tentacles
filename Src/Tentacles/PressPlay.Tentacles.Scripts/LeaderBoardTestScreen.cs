// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LeaderBoardTestScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.GamerServices;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LeaderBoardTestScreen : GameScreen
  {
    private const int BestTimeLeaderboard = 0;
    private const int LeaderboardPageSize = 600;
    private LeaderboardReader leaderboardReader;
    private LeaderBoardTestScreen.GameState gameState = LeaderBoardTestScreen.GameState.WaitingForSignIn;

    public LeaderBoardTestScreen()
    {
      SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(this.GamerSignedInCallback);
    }

    protected void GamerSignedInCallback(object sender, SignedInEventArgs args)
    {
      SignedInGamer gamer = args.Gamer;
      if (gamer != null && gamer.IsSignedInToLive)
      {
        LeaderboardReader.BeginRead(LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, 0), (Gamer) gamer, 600, new AsyncCallback(this.LeaderboardReadCallback), (object) gamer);
        this.gameState = LeaderBoardTestScreen.GameState.WaitingForBestScoreLeaderboard;
      }
      else
        this.gameState = LeaderBoardTestScreen.GameState.Idle;
    }

    protected void LeaderboardReadCallback(IAsyncResult result)
    {
      if (!(result.AsyncState is SignedInGamer))
        return;
      try
      {
        this.leaderboardReader = LeaderboardReader.EndRead(result);
        if (this.gameState != LeaderBoardTestScreen.GameState.WaitingForBestScoreLeaderboard)
          return;
        this.gameState = LeaderBoardTestScreen.GameState.ViewingBestScoreLeaderboard;
      }
      catch (Exception ex)
      {
        this.gameState = LeaderBoardTestScreen.GameState.Idle;
      }
    }

    public enum GameState
    {
      Error,
      WaitingForSignIn,
      PlayingGame,
      WaitingForBestScoreLeaderboard,
      ViewingBestScoreLeaderboard,
      WaitingForBestTimeLeaderboard,
      ViewingBestTimeLeaderboard,
      Idle,
    }
  }
}
