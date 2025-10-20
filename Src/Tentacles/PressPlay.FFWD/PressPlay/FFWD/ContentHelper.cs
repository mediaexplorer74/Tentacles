// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ContentHelper
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PressPlay.FFWD.SkinnedModel;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.FFWD
{
  public static class ContentHelper
  {
    public static ContentManager Content;
    public static ContentManager StaticContent;
    private static Dictionary<string, Texture2D> StaticTextures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, ContentHelper.ModelData> StaticModels = new Dictionary<string, ContentHelper.ModelData>();
    private static Dictionary<string, SoundEffect> StaticSounds = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, ContentHelper.ModelData> Models = new Dictionary<string, ContentHelper.ModelData>();
    private static Dictionary<ContentHelper.TextureToColor, Texture2D> coloredTextures = new Dictionary<ContentHelper.TextureToColor, Texture2D>();
    public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Song> Songs = new Dictionary<string, Song>();
    private static bool DeferredLoading = false;
    private static Queue<string> DeferredTextureQueue = new Queue<string>();
    private static Queue<string> DeferredStaticTextureQueue = new Queue<string>();
    private static Queue<string> DeferredModelQueue = new Queue<string>();
    private static Queue<string> DeferredStaticModelQueue = new Queue<string>();
    private static Queue<ContentHelper.TextureToColor> DeferredTextureColoringQueue = new Queue<ContentHelper.TextureToColor>();
    private static Queue<string> DeferredSoundQueue = new Queue<string>();
    private static Queue<string> DeferredStaticSoundQueue = new Queue<string>();
    private static string[] StaticTextureNames = new string[42]
    {
      "whitePixel",
      "debugCircle",
      "AtlasTiles_suburbia.png",
      "AtlasPulley_pirate.png",
      "AtlasTiles_pirate1.png",
      "AtlasTiles_pirate1_sketch.png",
      "AtlasTiles_pirate2.png",
      "AtlasTiles_pirate2_sketch.png",
      "AtlasTiles_pirate3.png",
      "AtlasTiles_pirate3_sketch.png",
      "AtlasTiles_sketch_suburbia.png",
      "AtlasTiles_suburbia.png",
      "Atlas_deco_pirate.png",
      "Atlas_deco_pirate_sketch.png",
      "AtlasCommon.png",
      "AtlasCommon_sketch.png",
      "AtlasGUI.png",
      "markerTexture.png",
      "max_spawn_flat.png",
      "maxSpriteSheet01.png",
      "maxSpriteSheet02.png",
      "maxSpriteSheet03.png",
      "maxSpriteSheet04.png",
      "monster_PauseGfx.png",
      "Monster_SpriteSheet.png",
      "Monster_SpriteSheet_2.png",
      "Monster_SpriteSheet_3.png",
      "monsterStealingInkSheet.png",
      "Hints\\hint_ink",
      "Hints\\hint_draw",
      "Hints\\hint_erase",
      "Hints\\hint_deleteAll",
      "Hints\\hint_pullPush",
      "Hints\\hint_combine",
      "Hints\\hint_seesaw",
      "Hints\\hint_erasePart",
      "Hints\\hint_killGobos",
      "Hints\\hint_pause",
      "Hints\\hint_letGo",
      "Hints\\hint_weightOfDrawings",
      "Hints\\hint_zoom",
      "Hints\\hint_hooks"
    };
    private static string[] StaticSoundNames = new string[10]
    {
      "freezeMode",
      "checkpoint",
      "maxClimb",
      "maxExtremeFall",
      "maxExtremeJump",
      "maxGrab",
      "maxJump01",
      "maxQuickClimb",
      "maxSlide",
      "checkpoint"
    };
    private static Dictionary<int, Dictionary<string, Color[]>> texturesToPreColor = new Dictionary<int, Dictionary<string, Color[]>>()
    {
      {
        1,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[11]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(254f, 251f, 248f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "WaterParticleTex.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        2,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[14]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(254f, 251f, 248f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color(48f, 12f, 129f, (float) byte.MaxValue),
              new Color(211f, 238f, 247f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[13]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(249f, 241f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(250f, 244f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(252f, 248f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(253f, 251f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        3,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[8]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[13]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color(249f, 241f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(250f, 244f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(252f, 248f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(253f, 251f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        4,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[14]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color(249f, 241f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(250f, 244f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(252f, 248f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(253f, 251f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        5,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[14]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color(249f, 241f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(250f, 244f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(252f, 248f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color(253f, 251f, (float) byte.MaxValue, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        6,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[6]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[9]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f)
            }
          },
          {
            "fallingRockPiece.png",
            new Color[1]
            {
              new Color(25f, 25f, 25f, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        7,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[8]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "fallingRockPiece.png",
            new Color[1]
            {
              new Color(25f, 25f, 25f, (float) byte.MaxValue)
            }
          },
          {
            "Default-Particle.png",
            new Color[9]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, 236f),
              new Color(0.0f, 0.0f, 0.0f, 218f),
              new Color(0.0f, 0.0f, 0.0f, 199f),
              new Color(0.0f, 0.0f, 0.0f, 181f),
              new Color(0.0f, 0.0f, 0.0f, 138f),
              new Color(0.0f, 0.0f, 0.0f, 95f),
              new Color(0.0f, 0.0f, 0.0f, 52f),
              new Color(0.0f, 0.0f, 0.0f, 10f)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        8,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[8]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "WaterParticleTex.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "fallingRockPiece.png",
            new Color[1]
            {
              new Color(25f, 25f, 25f, (float) byte.MaxValue)
            }
          },
          {
            "Default-Particle.png",
            new Color[9]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, 236f),
              new Color(0.0f, 0.0f, 0.0f, 218f),
              new Color(0.0f, 0.0f, 0.0f, 199f),
              new Color(0.0f, 0.0f, 0.0f, 181f),
              new Color(0.0f, 0.0f, 0.0f, 138f),
              new Color(0.0f, 0.0f, 0.0f, 95f),
              new Color(0.0f, 0.0f, 0.0f, 52f),
              new Color(0.0f, 0.0f, 0.0f, 10f)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        9,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "WaterParticleTex.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "fallingRockPiece.png",
            new Color[1]
            {
              new Color(25f, 25f, 25f, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        10,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "WaterParticleTex.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "fallingRockPiece.png",
            new Color[1]
            {
              new Color(25f, 25f, 25f, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        11,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        12,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        13,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        14,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      },
      {
        15,
        new Dictionary<string, Color[]>()
        {
          {
            "WhiteCircleHardAlpha.png",
            new Color[9]
            {
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(18f, 0.0f, 27f, (float) byte.MaxValue),
              new Color(200f, 14f, 221f, (float) byte.MaxValue),
              new Color(183f, 179f, 198f, (float) byte.MaxValue),
              new Color(227f, 225f, 216f, (float) byte.MaxValue),
              new Color(46f, 4f, 99f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 12f, (float) byte.MaxValue),
              new Color(254f, 251f, 248f, (float) byte.MaxValue)
            }
          },
          {
            "Default-Particle.png",
            new Color[9]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color(0.0f, 0.0f, 0.0f, 236f),
              new Color(0.0f, 0.0f, 0.0f, 218f),
              new Color(0.0f, 0.0f, 0.0f, 199f),
              new Color(0.0f, 0.0f, 0.0f, 181f),
              new Color(0.0f, 0.0f, 0.0f, 138f),
              new Color(0.0f, 0.0f, 0.0f, 95f),
              new Color(0.0f, 0.0f, 0.0f, 52f),
              new Color(0.0f, 0.0f, 0.0f, 10f)
            }
          },
          {
            "WhiteCircleSoftAlpha.png",
            new Color[10]
            {
              new Color(0.0f, 0.0f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(52f, 52f, 52f, 52f),
              new Color(95f, 95f, 95f, 95f),
              new Color(137f, 137f, 137f, 137f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 180f),
              new Color(198f, 198f, 198f, 198f),
              new Color(217f, 217f, 217f, 217f),
              new Color(236f, 236f, 236f, 236f),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue)
            }
          },
          {
            "AtlasCommon.png",
            new Color[5]
            {
              new Color(7f, 5f, 0.0f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, (float) byte.MaxValue, (float) byte.MaxValue, 10f),
              new Color(25f, 10f, 1f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 229f, 185f, (float) byte.MaxValue),
              new Color((float) byte.MaxValue, 136f, 64f, (float) byte.MaxValue)
            }
          }
        }
      }
    };

    public static GameServiceContainer Services { get; set; }

    public static bool IgnoreMissingAssets { get; set; }

    public static void StartDeferredLoading() => ContentHelper.DeferredLoading = true;

    public static void EndDeferredLoading() => ContentHelper.DeferredLoading = false;

    public static int DeferredQueueSize()
    {
      return ContentHelper.DeferredTextureQueue.Count + ContentHelper.DeferredStaticTextureQueue.Count + ContentHelper.DeferredSoundQueue.Count + ContentHelper.DeferredTextureColoringQueue.Count + ContentHelper.DeferredStaticSoundQueue.Count;
    }

    public static void ExecuteDeferredLoadingStep()
    {
      if (ContentHelper.DeferredStaticTextureQueue.Count > 0)
        ContentHelper.LoadStaticTexture(ContentHelper.DeferredStaticTextureQueue.Dequeue());
      else if (ContentHelper.DeferredTextureQueue.Count > 0)
        ContentHelper.LoadTexture(ContentHelper.DeferredTextureQueue.Dequeue());
      else if (ContentHelper.DeferredStaticSoundQueue.Count > 0)
        ContentHelper.LoadStaticSound(ContentHelper.DeferredStaticSoundQueue.Dequeue());
      else if (ContentHelper.DeferredSoundQueue.Count > 0)
      {
        ContentHelper.LoadSound(ContentHelper.DeferredSoundQueue.Dequeue());
      }
      else
      {
        if (ContentHelper.DeferredTextureColoringQueue.Count <= 0)
          return;
        ContentHelper.PreColorTexture(ContentHelper.DeferredTextureColoringQueue.Dequeue());
      }
    }

    public static void LoadTexture(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (ContentHelper.DeferredLoading)
      {
        if (ContentHelper.DeferredTextureQueue.Contains(name) || ContentHelper.DeferredStaticTextureQueue.Contains(name))
          return;
        ContentHelper.DeferredTextureQueue.Enqueue(name);
      }
      else
      {
        if (ContentHelper.Textures.ContainsKey(name) || ContentHelper.StaticTextures.ContainsKey(name))
          return;
        string path = name.Replace("-", "");
        if (path.Contains("."))
          path = Path.GetFileNameWithoutExtension(path);
        try
        {
          ContentHelper.Textures.Add(name, ContentHelper.Content.Load<Texture2D>("Textures\\" + path));
        }
        catch
        {
          Debug.Display("Missing texture", (object) name);
          if (ContentHelper.IgnoreMissingAssets)
            return;
          throw;
        }
      }
    }

    public static void LoadModel(string name) => ContentHelper.LoadModel(name, false);

    public static void LoadModel(string name, bool skinnedModel)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (ContentHelper.DeferredLoading)
      {
        if (ContentHelper.DeferredModelQueue.Contains(name) || ContentHelper.DeferredModelQueue.Contains(name))
          return;
        ContentHelper.DeferredTextureQueue.Enqueue(name);
      }
      else
      {
        if (ContentHelper.Models.ContainsKey(name) || ContentHelper.StaticModels.ContainsKey(name))
          return;
        string path = name.Replace("-", "");
        if (path.Contains("."))
          path = Path.GetFileNameWithoutExtension(path);
        try
        {
          ContentHelper.ModelData modelData = new ContentHelper.ModelData();
          if (skinnedModel)
            modelData.skinnedModel = ContentHelper.Content.Load<CpuSkinnedModel>("Models\\" + path);
          else
            modelData.model = ContentHelper.Content.Load<Model>("Models\\" + path);
          ContentHelper.Models.Add(name, modelData);
        }
        catch
        {
          Debug.Display("Missing model", (object) name);
          if (ContentHelper.IgnoreMissingAssets)
            return;
          throw;
        }
      }
    }

    public static SpriteFont LoadFont(string name)
    {
      return !string.IsNullOrEmpty(name) ? ContentHelper.StaticContent.Load<SpriteFont>(Path.GetFileNameWithoutExtension(name)) : (SpriteFont) null;
    }

    public static void LoadSound(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (ContentHelper.DeferredLoading)
      {
        if (ContentHelper.DeferredSoundQueue.Contains(name) || ContentHelper.DeferredStaticSoundQueue.Contains(name))
          return;
        ContentHelper.DeferredSoundQueue.Enqueue(name);
      }
      else
      {
        if (ContentHelper.Sounds.ContainsKey(name) || ContentHelper.StaticSounds.ContainsKey(name))
          return;
        SoundEffect soundEffect = (SoundEffect) null;
        try
        {
          soundEffect = ContentHelper.Content.Load<SoundEffect>("Sounds\\" + name);
        }
        catch
        {
          Debug.Display("Missing sound", (object) name);
        }
        if (soundEffect == null)
          return;
        ContentHelper.Sounds.Add(name, soundEffect);
      }
    }

    public static void LoadSong(string name)
    {
      if (string.IsNullOrEmpty(name) || ContentHelper.Songs.ContainsKey(name))
        return;
      Song song = (Song) null;
      try
      {
        song = ContentHelper.Content.Load<Song>("Sounds\\" + Path.GetFileNameWithoutExtension(name));
      }
      catch
      {
        Debug.Display("Missing song", (object) name);
      }
      if (!(song != (Song) null))
        return;
      ContentHelper.Songs.Add(name, song);
    }

    public static void LoadStaticSound(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (ContentHelper.DeferredLoading)
      {
        if (ContentHelper.DeferredStaticSoundQueue.Contains(name))
          return;
        ContentHelper.DeferredStaticSoundQueue.Enqueue(name);
      }
      else
      {
        if (ContentHelper.StaticSounds.ContainsKey(name))
          return;
        SoundEffect soundEffect = (SoundEffect) null;
        try
        {
          soundEffect = ContentHelper.StaticContent.Load<SoundEffect>("Sounds\\" + Path.GetFileNameWithoutExtension(name));
        }
        catch
        {
          Debug.Display("Missing sound", (object) name);
        }
        if (soundEffect == null)
          return;
        ContentHelper.StaticSounds.Add(name, soundEffect);
      }
    }

    public static void LoadStaticTexture(string name)
    {
      if (string.IsNullOrEmpty(name))
        return;
      if (ContentHelper.DeferredLoading)
      {
        if (ContentHelper.DeferredStaticTextureQueue.Contains(name))
          return;
        ContentHelper.DeferredStaticTextureQueue.Enqueue(name);
      }
      else
      {
        if (ContentHelper.StaticTextures.ContainsKey(name))
          return;
        string path = name.Replace("-", "");
        if (path.Contains("."))
          path = Path.GetFileNameWithoutExtension(path);
        Texture2D texture2D = ContentHelper.StaticContent.Load<Texture2D>("Textures\\" + path);
        ContentHelper.StaticTextures.Add(name, texture2D);
      }
    }

    public static Texture2D GetTexture(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (Texture2D) null;
      Texture2D texture;
      ContentHelper.Textures.TryGetValue(name, out texture);
      if (texture == null)
        ContentHelper.StaticTextures.TryGetValue(name, out texture);
      return texture;
    }

    public static Model GetModel(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (Model) null;
      ContentHelper.ModelData model;
      if (ContentHelper.Models.ContainsKey(name))
        model = ContentHelper.Models[name];
      else
        ContentHelper.StaticModels.TryGetValue(name, out model);
      return model.model;
    }

    public static CpuSkinnedModel GetSkinnedModel(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (CpuSkinnedModel) null;
      ContentHelper.ModelData model;
      if (ContentHelper.Models.ContainsKey(name))
        model = ContentHelper.Models[name];
      else
        ContentHelper.StaticModels.TryGetValue(name, out model);
      return model.skinnedModel;
    }

    public static SoundEffect GetSound(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (SoundEffect) null;
      SoundEffect sound;
      ContentHelper.Sounds.TryGetValue(name, out sound);
      if (sound == null)
        ContentHelper.StaticSounds.TryGetValue(name, out sound);
      return sound;
    }

    public static Song GetSong(string name)
    {
      if (string.IsNullOrEmpty(name))
        return (Song) null;
      Song song;
      ContentHelper.Songs.TryGetValue(name, out song);
      return song;
    }

    private static void PreColorTexture(ContentHelper.TextureToColor textureToColor)
    {
      ContentHelper.GetColoredTexture((Color) textureToColor.color, textureToColor.textureName, true);
    }

    private static void PreLoadContent(int level)
    {
      foreach (string staticTextureName in ContentHelper.StaticTextureNames)
        ContentHelper.LoadStaticTexture(staticTextureName);
      foreach (string staticSoundName in ContentHelper.StaticSoundNames)
        ContentHelper.LoadStaticSound(staticSoundName);
      Dictionary<string, Color[]> dictionary;
      ContentHelper.texturesToPreColor.TryGetValue(level, out dictionary);
      if (dictionary == null)
        return;
      foreach (string key in dictionary.Keys)
        ContentHelper.LoadTexture(key);
      foreach (KeyValuePair<string, Color[]> keyValuePair in dictionary)
      {
        for (int index = 0; index < keyValuePair.Value.Length; ++index)
        {
          ContentHelper.TextureToColor textureToColor = new ContentHelper.TextureToColor()
          {
            color = (Microsoft.Xna.Framework.Color) keyValuePair.Value[index],
            textureName = keyValuePair.Key
          };
          if (ContentHelper.DeferredLoading)
          {
            if (!ContentHelper.DeferredTextureColoringQueue.Contains(textureToColor))
              ContentHelper.DeferredTextureColoringQueue.Enqueue(textureToColor);
          }
          else
            ContentHelper.PreColorTexture(textureToColor);
        }
      }
    }

    internal static Texture2D GetColoredTexture(Color aColor, string name)
    {
      return ContentHelper.GetColoredTexture(aColor, name, false);
    }

    internal static Texture2D GetColoredTexture(Color aColor, string name, bool preColoring)
    {
      ContentHelper.TextureToColor key = new ContentHelper.TextureToColor()
      {
        color = (Microsoft.Xna.Framework.Color) aColor,
        textureName = name
      };
      Texture2D coloredTexture1;
      ContentHelper.coloredTextures.TryGetValue(key, out coloredTexture1);
      if (coloredTexture1 != null)
        return coloredTexture1;
      Texture2D texture = ContentHelper.GetTexture(name);
      Texture2D coloredTexture2 = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height);
      Color[] data = new Color[texture.Width * texture.Height];
      texture.GetData<Color>(data, 0, data.Length);
      for (int index = 0; index < data.Length; ++index)
      {
        data[index].R = (byte) ((int) data[index].R * (int) aColor.R / (int) byte.MaxValue);
        data[index].G = (byte) ((int) data[index].G * (int) aColor.G / (int) byte.MaxValue);
        data[index].B = (byte) ((int) data[index].B * (int) aColor.B / (int) byte.MaxValue);
        data[index].A = (byte) ((int) data[index].A * (int) aColor.A / (int) byte.MaxValue);
      }
      coloredTexture2.SetData<Color>(data);
      ContentHelper.coloredTextures.Add(key, coloredTexture2);
      return coloredTexture2;
    }

    public static void CleanUp()
    {
      foreach (KeyValuePair<string, Texture2D> texture in ContentHelper.Textures)
        texture.Value.Dispose();
      ContentHelper.Textures = new Dictionary<string, Texture2D>();
      ContentHelper.coloredTextures = new Dictionary<ContentHelper.TextureToColor, Texture2D>();
      ContentHelper.Sounds = new Dictionary<string, SoundEffect>();
      ContentHelper.DeferredTextureQueue = new Queue<string>();
      ContentHelper.DeferredSoundQueue = new Queue<string>();
      ContentHelper.Content.Unload();
      ContentHelper.Content.Dispose();
      ContentHelper.Content = new ContentManager((IServiceProvider) ContentHelper.Services, "Content");
    }

    private struct ModelData
    {
      public Model model;
      public CpuSkinnedModel skinnedModel;
    }

    private struct TextureToColor
    {
      public Microsoft.Xna.Framework.Color color;
      public string textureName;
    }
  }
}
