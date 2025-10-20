// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TravelNode
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TravelNode
  {
    public int levelIndex;
    public Vector2 start;
    public Vector2 end;
    public Texture2D background;
    public LemmyTravelScreen.TravelEnvironment environment;

    public TravelNode(
      int levelIndex,
      Vector2 start,
      Vector2 end,
      Texture2D background,
      LemmyTravelScreen.TravelEnvironment environment)
    {
      this.levelIndex = levelIndex;
      this.start = start;
      this.end = end;
      this.background = background;
      this.environment = environment;
    }
  }
}
