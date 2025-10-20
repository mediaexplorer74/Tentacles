// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpritePositions
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpritePositions
  {
    public static List<Rectangle> green = new List<Rectangle>()
    {
      new Rectangle(584, 58, 97, 139),
      new Rectangle(680, 58, 87, 139),
      new Rectangle(768, 58, 87, 139),
      new Rectangle(855, 58, 97, 139)
    };
    public static List<Rectangle> orange = new List<Rectangle>()
    {
      new Rectangle(71, 314, 97, 139),
      new Rectangle(168, 314, 87, 139),
      new Rectangle(256, 314, 87, 139),
      new Rectangle(343, 314, 97, 139)
    };
    public static List<Rectangle> blue = new List<Rectangle>()
    {
      new Rectangle(584, 314, 97, 139),
      new Rectangle(680, 314, 87, 139),
      new Rectangle(768, 314, 87, 139),
      new Rectangle(855, 314, 97, 139)
    };
    public static List<Rectangle> locked = new List<Rectangle>()
    {
      new Rectangle(71, 58, 97, 139),
      new Rectangle(168, 58, 87, 139),
      new Rectangle(256, 58, 87, 139),
      new Rectangle(343, 58, 97, 139)
    };
    public static List<Rectangle> highlighted = new List<Rectangle>()
    {
      new Rectangle(71, 826, 97, 139),
      new Rectangle(168, 826, 87, 139),
      new Rectangle(256, 826, 87, 139),
      new Rectangle(343, 826, 97, 139)
    };
    public static List<Rectangle> glow = new List<Rectangle>()
    {
      new Rectangle(584, 826, 97, 139),
      new Rectangle(680, 826, 87, 139),
      new Rectangle(768, 826, 87, 139),
      new Rectangle(855, 826, 97, 139)
    };
    public static Rectangle logo = new Rectangle(0, 0, 520, 179);
    public static Rectangle endLevelScreenButtonBig = new Rectangle(125, 9, 260, 111);
    public static Rectangle endLevelScreenButtonBigHighlight = new Rectangle(125, 135, 260, 111);
    public static Rectangle endLevelScreenButtonSmall = new Rectangle(125, 250, 260, 111);
    public static Rectangle endLevelScreenButtonSmallHighlight = new Rectangle(125, 379, 260, 111);
    public static Rectangle leaderboardBlue = new Rectangle(61, 25, 390, 206);
    public static Rectangle leaderboardGreen = new Rectangle(573, 25, 390, 206);
    public static Rectangle leaderboardOrange = new Rectangle(61, 281, 390, 206);
    public static Rectangle leaderboardBlueSelected = new Rectangle(61, 539, 390, 206);
    public static Rectangle leaderboardGreenSelected = new Rectangle(573, 539, 390, 206);
    public static Rectangle leaderboardOrangeSelected = new Rectangle(61, 795, 390, 206);
    public static Rectangle leaderboardHighlighted = new Rectangle(573, 281, 390, 206);
    public static Rectangle leaderboardHighlightedSelected = new Rectangle(573, 795, 390, 206);
    public static Rectangle levelSelectionRowBackground = new Rectangle(584, 538, 368, 204);
    public static Rectangle levelSelectionRowTutorial = new Rectangle(76, 574, 354, 76);
    public static Rectangle levelSelectionRowTutorialHighlighted = new Rectangle(76, 654, 354, 76);
    public static Rectangle achievementUnlocked = new Rectangle(0, 0, 512, 128);
    public static Rectangle achievementLocked = new Rectangle(512, 0, 512, 128);
    public static Rectangle levelSelectStarFilled = new Rectangle(0, 0, 32, 32);
    public static Rectangle levelSelectStarEmpty = new Rectangle(32, 0, 32, 32);
    public static Rectangle endlevelScoreBackground = new Rectangle(0, 164, 589, 70);
    public static Rectangle endlevelStarBackgroundGreen = new Rectangle(0, 234, 499, 139);
    public static Rectangle endlevelStarBackgroundRed = new Rectangle(499, 234, 499, 139);
    public static Rectangle star = new Rectangle(203, 373, 75, 72);
    public static Rectangle ingameMenuBackgroundBottom = new Rectangle(0, 0, 800, 164);
    public static Rectangle ingameMenuBackgroundTop = new Rectangle(0, 500, 800, 164);
    public static Rectangle ingameMenuButtonNormal = new Rectangle(0, 373, 203, 63);
    public static Rectangle ingameMenuButtonHighlighted = new Rectangle(0, 437, 203, 63);
    public static Rectangle onOffSliderOn = new Rectangle(0, 0, 256, 128);
    public static Rectangle onOffSliderOff = new Rectangle(256, 0, 256, 128);
    public static Rectangle onOffSliderOffHover = new Rectangle(0, 128, 256, 128);
    public static Rectangle onOffSliderOnHover = new Rectangle(256, 128, 256, 128);
    public static Rectangle starPanelNeutralOne = new Rectangle(0, 667, 169, 139);
    public static Rectangle starPanelNeutralTwo = new Rectangle(169, 667, 162, 139);
    public static Rectangle starPanelNeutralThree = new Rectangle(330, 667, 169, 139);
    public static Rectangle starPanelGreenOne = new Rectangle(0, 234, 169, 139);
    public static Rectangle starPanelGreenTwo = new Rectangle(169, 234, 162, 139);
    public static Rectangle starPanelGreenThree = new Rectangle(330, 234, 169, 139);
    public static Rectangle starPanelRedOne = new Rectangle(499, 234, 169, 139);
    public static Rectangle starPanelRedTwo = new Rectangle(668, 234, 162, 139);
    public static Rectangle starPanelRedThree = new Rectangle(830, 234, 169, 139);
    public static Rectangle achievementBoxOpen = new Rectangle(69, 24, 374, 80);
    public static Rectangle achievementBoxLocked = new Rectangle(69, 152, 374, 80);
    public static Rectangle mainMenuButtonOneNormal = new Rectangle(5, 5, 280, 87);
    public static Rectangle mainMenuButtonTwoNormal = new Rectangle(5, 95, 280, 87);
    public static Rectangle mainMenuButtonThreeNormal = new Rectangle(5, 186, 280, 87);
    public static Rectangle mainMenuButtonFourNormal = new Rectangle(5, 276, 280, 87);
    public static Rectangle mainMenuButtonOneActive = new Rectangle(292, 5, 280, 87);
    public static Rectangle mainMenuButtonTwoActive = new Rectangle(292, 95, 280, 87);
    public static Rectangle mainMenuButtonThreeActive = new Rectangle(292, 186, 280, 87);
    public static Rectangle mainMenuButtonFourActive = new Rectangle(292, 276, 280, 87);
    public static Rectangle mainMenuPlayButtonNormal = new Rectangle(655, 11, 284, 191);
    public static Rectangle mainMenuPlayButtonActive = new Rectangle(655, 216, 284, 191);
    public static Rectangle mainMenuPurchaseButtonNormal = new Rectangle(0, 421, 509, 86);
    public static Rectangle mainMenuPurchaseButtonActive = new Rectangle(512, 421, 509, 86);
    public static Rectangle challengeBarBackground = new Rectangle(0, 38, 800, 69);
    public static Rectangle challengeProgressBarBackground = new Rectangle(0, 18, 524, 16);
    public static Rectangle challengeProgressBarForeground = new Rectangle(0, 1, 524, 16);
    public static Rectangle loadingScreenGreenButton = new Rectangle(0, (int) sbyte.MaxValue, 63, 63);
    public static Rectangle loadingScreenRedButton = new Rectangle(0, 193, 63, 63);
    public static Rectangle upsellPurchaseButtonNormal = new Rectangle(0, 0, 578, 87);
    public static Rectangle upsellPurchaseButtonActive = new Rectangle(0, 100, 578, 87);
    public static Rectangle loadingScreenBluePin = new Rectangle(63, 0, 128, 128);
    public static Rectangle loadingScreenGreenPin = new Rectangle(191, 0, 128, 128);
    public static Rectangle loadingScreenBrownPin = new Rectangle(63, 128, 128, 128);
    public static Rectangle loadingScreenRedPin = new Rectangle(191, 128, 128, 128);
    public static Rectangle loadingScreenDot = new Rectangle(0, 0, 20, 20);
    public static Rectangle loadingScreenLemmy = new Rectangle(0, 20, 42, 40);

    public static Rectangle GetLevelButtonHighlight(int index, Level level)
    {
      return SpritePositions.highlighted[index];
    }

    public static Rectangle GetLevelButtonGraphic(int index, Level level)
    {
      if (level != null)
      {
        switch (level.levelType)
        {
          case Level.LevelType.veins:
            return SpritePositions.blue[index];
          case Level.LevelType.intestines:
            return SpritePositions.orange[index];
          case Level.LevelType.brain:
            return SpritePositions.blue[index];
          case Level.LevelType.desatGreen:
            return SpritePositions.green[index];
          case Level.LevelType.petriDish:
            return SpritePositions.green[index];
        }
      }
      return SpritePositions.locked[index];
    }
  }
}
