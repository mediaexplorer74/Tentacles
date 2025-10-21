// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.XnaMusicUtil
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public static class XnaMusicUtil
  {
    // Song class is not available in MonoGame, so we replace it with a placeholder
    // public static Song currSong;
    public static object currSong;
    public static bool isRadio;
    public static double radioFrequency;
    // MediaState enum is not available in MonoGame, so we replace it with a placeholder
    // public static MediaState currState;
    public static object currState;
    // Song class is not available in MonoGame, so we replace it with a placeholder
    // public static List<Song> currQueue = new List<Song>();
    public static List<object> currQueue = new List<object>();
    public static bool hasSaved = false;

    public static void SaveCurrentMediaState(bool isStopping)
    {
      XnaMusicUtil.currQueue.Clear();
      // Song class is not available in MonoGame, so we replace it with a placeholder
      // XnaMusicUtil.currSong = (Song) null;
      XnaMusicUtil.currSong = (object) null;
      XnaMusicUtil.isRadio = false;
      XnaMusicUtil.hasSaved = false;
            // MediaPlayer is not available in MonoGame, so we skip this functionality
            // XnaMusicUtil.currState = MediaPlayer.State;
      XnaMusicUtil.radioFrequency = default;//FMRadio.Instance.Frequency;
      if (MediaPlayer.Queue != null)
      {
        //TODO
        /*switch (MediaPlayer.Queue.Count)
        {
          case 0:
            break;
          case 1:
            if (1 == 0)//(FMRadio.Instance.PowerMode == RadioPowerMode.On || MediaPlayer.Queue.ActiveSongIndex == -1)
            {
              XnaMusicUtil.isRadio = true;
              XnaMusicUtil.hasSaved = true;
              break;
            }
            XnaMusicUtil.isRadio = false;
            XnaMusicUtil.currQueue.Add(MediaPlayer.Queue[0]);
            if (MediaPlayer.Queue.ActiveSong != (Song) null)
              // MediaPlayer is not available in MonoGame, so we skip this functionality
              // XnaMusicUtil.currSong = MediaPlayer.Queue.ActiveSong;
            XnaMusicUtil.hasSaved = true;
            break;
          default:
            XnaMusicUtil.isRadio = false;
            for (int index = 0; index < MediaPlayer.Queue.Count; ++index)
              XnaMusicUtil.currQueue.Add(MediaPlayer.Queue[index]);
            if (MediaPlayer.Queue.ActiveSong != (Song) null)
              XnaMusicUtil.currSong = MediaPlayer.Queue.ActiveSong;
            XnaMusicUtil.hasSaved = true;
            break;
        }*/
      }
      // MediaPlayer is not available in MonoGame, so we skip this functionality
      // if (MediaPlayer.State == MediaState.Playing && isStopping)
      //   MediaPlayer.Stop();
      
      // ?
      //if (FMRadio.Instance.PowerMode != RadioPowerMode.On || !isStopping)
      //  return;
      //FMRadio.Instance.PowerMode = RadioPowerMode.Off;
    }

    private static bool MatchAndPlay(SongCollection sc)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (sc.Count == XnaMusicUtil.currQueue.Count)
      {
        for (int index = 0; index < XnaMusicUtil.currQueue.Count; ++index)
        {
          // Song class is not available in MonoGame, so we skip this functionality
          // if (!((IEnumerable<Song>) sc).Contains<Song>(XnaMusicUtil.currQueue[index]))
          //   flag1 = false;
          flag1 = false; // Assume no match for now
        }
        flag2 = flag1;
        // Song class is not available in MonoGame, so we skip this functionality
        // ((IEnumerable<Song>) sc).ToList<Song>().Add(XnaMusicUtil.currSong);
        if (flag2)
        {
          for (int index1 = 0; index1 < sc.Count; ++index1)
          {
            // MediaPlayer is not available in MonoGame, so we skip this functionality
            // if (sc[index1] == XnaMusicUtil.currSong)
            // {
            //   int index2 = index1;
            //   MediaPlayer.Play(sc, index2);
            //   break;
            // }
          }
        }
      }
      return flag2;
    }

    public static void RestoreCurrentMediaState()
    {
      bool flag = true;
      if (XnaMusicUtil.hasSaved)
      {
        if (XnaMusicUtil.isRadio)
        {
          //FMRadio.Instance.PowerMode = RadioPowerMode.On;
          //FMRadio.Instance.Frequency = XnaMusicUtil.radioFrequency;
          //if (FMRadio.Instance.Frequency != XnaMusicUtil.radioFrequency)
          //  FMRadio.Instance.Frequency = XnaMusicUtil.radioFrequency;
        }
        else
        {
          MediaLibrary mediaLibrary = new MediaLibrary();
          switch (XnaMusicUtil.currQueue.Count)
          {
            case 0:
              break;
            case 1:
              // MediaPlayer is not available in MonoGame, so we skip this functionality
              // if (XnaMusicUtil.currSong != (Song) null && ((IEnumerable<Song>) mediaLibrary.Songs).Contains<Song>(XnaMusicUtil.currSong))
              // {
              //   MediaPlayer.Play(XnaMusicUtil.currSong);
              //   break;
              // }
              break;
            default:
             //TODO
              /*if (mediaLibrary.Playlists.Count > 0)
              {
                for (int index = 0; index < mediaLibrary.Playlists.Count; ++index)
                {
                  // MatchAndPlay is not available in MonoGame, so we skip this functionality
                  // flag = XnaMusicUtil.MatchAndPlay(mediaLibrary.Playlists[index].Songs);
                  // if (flag)
                  //   break;
                  flag = false; // Assume no match for now
                }
              }*/
              if (mediaLibrary.Albums.Count > 0 && !flag)
              {
                for (int index = 0; index < mediaLibrary.Albums.Count; ++index)
                {
                  // MatchAndPlay is not available in MonoGame, so we skip this functionality
                  // flag = XnaMusicUtil.MatchAndPlay(mediaLibrary.Albums[index].Songs);
                  // if (flag)
                  //   break;
                  flag = false; // Assume no match for now
                }
              }
              // MediaPlayer is not available in MonoGame, so we skip this functionality
              // if (XnaMusicUtil.currSong != (Song) null)
              // {
              //   int num = flag ? 1 : 0;
              //   break;
              // }
              break;
          }
        }
        switch (XnaMusicUtil.currState)
        {
          // MediaState and MediaPlayer are not available in MonoGame, so we skip this functionality
          // case MediaState.Stopped:
          //   MediaPlayer.Stop();
          //   break;
          // case MediaState.Paused:
          //   MediaPlayer.Pause();
          //   break;
        }
      }
      // Clear the queue and reset the saved state
      // XnaMusicUtil.currQueue.Clear();
      // XnaMusicUtil.hasSaved = false;
      XnaMusicUtil.currQueue = new List<object>(); // Reinitialize the list
      XnaMusicUtil.hasSaved = false;
    }
  }
}
