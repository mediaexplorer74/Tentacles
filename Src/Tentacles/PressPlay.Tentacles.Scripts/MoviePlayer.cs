// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MoviePlayer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class MoviePlayer : MonoBehaviour
  {
    private MoviePlayer.Movies currentMovie;
    private string[] movieNames = new string[8]
    {
      "intro_part1",
      "intro_part2",
      "acidInjection",
      "helmet",
      "poisonDrinking",
      "brainPenetration",
      "outro",
      "intro"
    };
    private bool currentPlayingIsSkipable;
    private MoviePlayer.Mode mode;

    public bool isDonePlaying => this.mode == MoviePlayer.Mode.donePlaying;

    private string GetMovieName(MoviePlayer.Movies _movie)
    {
      int index = (int) _movie;
      return index < this.movieNames.Length - 1 ? this.movieNames[index] : "no movie name";
    }

    public void TestZuneBeforePlayingMovie(MoviePlayer.Movies _movie)
    {
      this.mode = MoviePlayer.Mode.beforePlaying;
      this.currentMovie = _movie;
      GlobalManager.Instance.Pause();
      if (!MediaPlayer.GameHasControl)
      {
        Application.screenManager.AddScreen((GameScreen) new ConfirmPopup(LocalisationManager.Instance.GetString("conf_zuneVideo"), new Action(this.ConfirmOverrideZune), new Action(this.DontOverrideZune)));
      }
      else
      {
        this.PlayCurrentMovie();
        GlobalManager.Instance.UnPause();
      }
    }

    public void ConfirmOverrideZune()
    {
      this.PlayCurrentMovie();
      GlobalManager.Instance.UnPause();
    }

    public void DontOverrideZune()
    {
      this.mode = MoviePlayer.Mode.donePlaying;
      GlobalManager.Instance.UnPause();
    }

    public void PlayCurrentMovie() => this.PlayMovie(this.currentMovie);

    private void PlayMovie(MoviePlayer.Movies _movie)
    {
      bool _skipable = GlobalManager.Instance.currentProfile.IsMovieUnlocked((int) _movie);
      this.PlayMovie(_movie, _skipable);
    }

    private void PlayMovie(MoviePlayer.Movies _movie, bool _skipable)
    {
      this.currentMovie = _movie;
      GlobalManager.Instance.currentProfile.UnlockMovie((int) _movie);
      this.currentPlayingIsSkipable = _skipable;
      new MediaPlayerLauncher()
      {
        Location = MediaLocationType.Install,
        Media = new Uri(this.GetMovieName(_movie) + ".wmv", UriKind.Relative),
        Controls = (!this.currentPlayingIsSkipable ? MediaPlaybackControls.None : MediaPlaybackControls.Skip)
      }.Show();
      this.mode = MoviePlayer.Mode.playingWP7Movie;
    }

    public override void Update()
    {
      if (this.mode != MoviePlayer.Mode.playingWP7Movie)
        return;
      this.mode = MoviePlayer.Mode.donePlaying;
    }

    public enum Movies
    {
      _01_intro_part1,
      _02_intro_part2,
      _03_acidInjection,
      _04_helmet,
      _05_poisonDrinking,
      _06_brainPenetration,
      _07_outro,
    }

    public enum Mode
    {
      idle,
      playingIphoneMovie,
      playingWP7Movie,
      playingMovieTextureMovie,
      donePlaying,
      beforePlaying,
    }
  }
}
