// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.XnaMusicUtil
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Devices.Radio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public static class XnaMusicUtil
  {
    public static Song currSong;
    public static bool isRadio;
    public static double radioFrequency;
    public static MediaState currState;
    public static List<Song> currQueue = new List<Song>();
    public static bool hasSaved = false;

    public static void SaveCurrentMediaState(bool isStopping)
    {
      XnaMusicUtil.currQueue.Clear();
      XnaMusicUtil.currSong = (Song) null;
      XnaMusicUtil.isRadio = false;
      XnaMusicUtil.hasSaved = false;
      XnaMusicUtil.currState = MediaPlayer.State;
      XnaMusicUtil.radioFrequency = FMRadio.Instance.Frequency;
      if (MediaPlayer.Queue != null)
      {
        switch (MediaPlayer.Queue.Count)
        {
          case 0:
            break;
          case 1:
            if (FMRadio.Instance.PowerMode == RadioPowerMode.On || MediaPlayer.Queue.ActiveSongIndex == -1)
            {
              XnaMusicUtil.isRadio = true;
              XnaMusicUtil.hasSaved = true;
              break;
            }
            XnaMusicUtil.isRadio = false;
            XnaMusicUtil.currQueue.Add(MediaPlayer.Queue[0]);
            if (MediaPlayer.Queue.ActiveSong != (Song) null)
              XnaMusicUtil.currSong = MediaPlayer.Queue.ActiveSong;
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
        }
      }
      if (MediaPlayer.State == MediaState.Playing && isStopping)
        MediaPlayer.Stop();
      if (FMRadio.Instance.PowerMode != RadioPowerMode.On || !isStopping)
        return;
      FMRadio.Instance.PowerMode = RadioPowerMode.Off;
    }

    private static bool MatchAndPlay(SongCollection sc)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (sc.Count == XnaMusicUtil.currQueue.Count)
      {
        for (int index = 0; index < XnaMusicUtil.currQueue.Count; ++index)
        {
          if (!((IEnumerable<Song>) sc).Contains<Song>(XnaMusicUtil.currQueue[index]))
            flag1 = false;
        }
        flag2 = flag1;
        ((IEnumerable<Song>) sc).ToList<Song>().Add(XnaMusicUtil.currSong);
        if (flag2)
        {
          for (int index1 = 0; index1 < sc.Count; ++index1)
          {
            if (sc[index1] == XnaMusicUtil.currSong)
            {
              int index2 = index1;
              MediaPlayer.Play(sc, index2);
              break;
            }
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
          FMRadio.Instance.PowerMode = RadioPowerMode.On;
          FMRadio.Instance.Frequency = XnaMusicUtil.radioFrequency;
          if (FMRadio.Instance.Frequency != XnaMusicUtil.radioFrequency)
            FMRadio.Instance.Frequency = XnaMusicUtil.radioFrequency;
        }
        else
        {
          MediaLibrary mediaLibrary = new MediaLibrary();
          switch (XnaMusicUtil.currQueue.Count)
          {
            case 0:
              break;
            case 1:
              if (XnaMusicUtil.currSong != (Song) null && ((IEnumerable<Song>) mediaLibrary.Songs).Contains<Song>(XnaMusicUtil.currSong))
              {
                MediaPlayer.Play(XnaMusicUtil.currSong);
                break;
              }
              break;
            default:
              if (mediaLibrary.Playlists.Count > 0)
              {
                for (int index = 0; index < mediaLibrary.Playlists.Count; ++index)
                {
                  flag = XnaMusicUtil.MatchAndPlay(mediaLibrary.Playlists[index].Songs);
                  if (flag)
                    break;
                }
              }
              if (mediaLibrary.Albums.Count > 0 && !flag)
              {
                for (int index = 0; index < mediaLibrary.Albums.Count; ++index)
                {
                  flag = XnaMusicUtil.MatchAndPlay(mediaLibrary.Albums[index].Songs);
                  if (flag)
                    break;
                }
              }
              if (XnaMusicUtil.currSong != (Song) null)
              {
                int num = flag ? 1 : 0;
                break;
              }
              break;
          }
        }
        switch (XnaMusicUtil.currState)
        {
          case MediaState.Stopped:
            MediaPlayer.Stop();
            break;
          case MediaState.Paused:
            MediaPlayer.Pause();
            break;
        }
      }
      XnaMusicUtil.currQueue.Clear();
      XnaMusicUtil.hasSaved = false;
    }
  }
}
