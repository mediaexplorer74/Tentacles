// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LemmyTravelScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LemmyTravelScreen : GameScreen
  {
    private int levelIndex;
    private TravelNode currentTravelNode;
    private List<TravelNode> nodes;
    private Texture2D gfxDot;
    private SimpleSprite gfxStart;
    private SimpleSprite gfxEnd;
    private SimpleSprite gfxLemmy;
    private SimpleSprite gfxBottomBar;
    private SimpleSprite gfxTutorialBar;
    private Texture2D atlas;
    private List<SimpleRenderObject> sprites = new List<SimpleRenderObject>();
    private float timeToTravel = 5f;
    private Microsoft.Xna.Framework.Vector2 lastDotPosition;
    private float distanceBetweenDots = 23f;
    private float elapsedTime;
    private LemmyTravelScreen.TravelEnvironment currentEnvironment;
    private string batchName = "";
    private string loadingText = "Loading";
    private LoadingScreen2 loadingScreen;
    private bool isComplete;

    public LemmyTravelScreen(Level level, LoadingScreen2 loadingScreen)
    {
      this.levelIndex = level.levelsIndex;
      this.loadingScreen = loadingScreen;
      this.batchName = LocalisationManager.Instance.GetString("FLB_batch" + (object) level.batch);
      this.loadingText = LocalisationManager.Instance.GetString("label_loading");
      this.BuildTravelNodeList();
      this.LoadTextures();
      this.currentTravelNode = this.GetTravelNode(this.levelIndex);
      this.lastDotPosition = (Microsoft.Xna.Framework.Vector2) this.currentTravelNode.start;
      this.AddSprite((SimpleRenderObject) new SimpleSprite(this.currentTravelNode.background));
      if (this.currentTravelNode.environment != LemmyTravelScreen.TravelEnvironment.tutorial)
      {
        switch ("pins")
        {
          case "buttons":
            SimpleSprite sprite1 = new SimpleSprite(this.atlas, SpritePositions.loadingScreenGreenButton);
            sprite1.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x, this.currentTravelNode.end.y);
            sprite1.drawFromCenter = true;
            this.AddSprite((SimpleRenderObject) sprite1);
            SimpleSprite sprite2 = new SimpleSprite(this.atlas, SpritePositions.loadingScreenRedButton);
            sprite2.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x, this.currentTravelNode.start.y);
            sprite2.drawFromCenter = true;
            this.AddSprite((SimpleRenderObject) sprite2);
            this.AddSprite((SimpleRenderObject) new SimpleText(this.currentTravelNode.levelIndex.ToString(), (Microsoft.Xna.Framework.Vector2) (this.currentTravelNode.start + new PressPlay.FFWD.Vector2(0.0f, 45f))));
            this.AddSprite((SimpleRenderObject) new SimpleText(this.currentTravelNode.levelIndex.ToString(), (Microsoft.Xna.Framework.Vector2) (this.currentTravelNode.end + new PressPlay.FFWD.Vector2(0.0f, 45f))));
            this.gfxLemmy = new SimpleSprite(this.atlas, SpritePositions.loadingScreenLemmy);
            this.gfxLemmy.position = (Microsoft.Xna.Framework.Vector2) this.currentTravelNode.start;
            this.gfxLemmy.drawFromCenter = true;
            this.AddSprite((SimpleRenderObject) this.gfxLemmy);
            break;
          case "pins":
            Rectangle loadingScreenBluePin = SpritePositions.loadingScreenBluePin;
            Microsoft.Xna.Framework.Vector2 vector2_1 = new Microsoft.Xna.Framework.Vector2(108f, 105f);
            Microsoft.Xna.Framework.Vector2 vector2_2 = new Microsoft.Xna.Framework.Vector2(20f, 105f);
            Microsoft.Xna.Framework.Vector2 vector2_3 = new Microsoft.Xna.Framework.Vector2(20f, 17f);
            SimpleSprite sprite3 = new SimpleSprite(this.atlas, loadingScreenBluePin);
            this.AddSprite((SimpleRenderObject) sprite3);
            SimpleSprite sprite4 = new SimpleSprite(this.atlas, loadingScreenBluePin);
            this.AddSprite((SimpleRenderObject) sprite4);
            SimpleText sprite5 = new SimpleText(this.GetPreviousLevelNumber(this.currentTravelNode.levelIndex));
            this.AddSprite((SimpleRenderObject) sprite5);
            SimpleText sprite6 = new SimpleText(this.currentTravelNode.levelIndex.ToString());
            this.AddSprite((SimpleRenderObject) sprite6);
            if ((double) this.currentTravelNode.end.x > (double) this.currentTravelNode.start.x)
            {
              sprite3.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x, this.currentTravelNode.end.y) - vector2_2;
              sprite4.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x, this.currentTravelNode.start.y) - vector2_1;
              sprite4.effects = SpriteEffects.FlipHorizontally;
              sprite5.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x - 70.2f, this.currentTravelNode.start.y - 70.2f);
              sprite6.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x + 70.2f, this.currentTravelNode.end.y - 70.2f);
            }
            else
            {
              sprite3.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x, this.currentTravelNode.end.y) - vector2_1;
              sprite3.effects = SpriteEffects.FlipHorizontally;
              sprite4.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x, this.currentTravelNode.start.y) - vector2_2;
              sprite5.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x + 70.2f, this.currentTravelNode.start.y - 70.2f);
              sprite6.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x - 70.2f, this.currentTravelNode.end.y - 70.2f);
            }
            if ((double) sprite4.position.Y < 0.0)
            {
              sprite4.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x, this.currentTravelNode.start.y) - vector2_3;
              sprite5.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.start.x + 70.2f, this.currentTravelNode.start.y + 70.2f);
              sprite4.effects = SpriteEffects.FlipVertically;
            }
            if ((double) sprite3.position.Y < 0.0)
            {
              sprite3.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x, this.currentTravelNode.end.y) - vector2_3;
              sprite6.position = new Microsoft.Xna.Framework.Vector2(this.currentTravelNode.end.x + 70.2f, this.currentTravelNode.end.y + 70.2f);
              sprite3.effects = SpriteEffects.FlipVertically;
            }
            this.gfxLemmy = new SimpleSprite(this.atlas, SpritePositions.loadingScreenLemmy);
            this.gfxLemmy.position = (Microsoft.Xna.Framework.Vector2) this.currentTravelNode.start;
            this.gfxLemmy.drawFromCenter = true;
            this.AddSprite((SimpleRenderObject) this.gfxLemmy);
            break;
        }
      }
      else
        this.AddSprite((SimpleRenderObject) this.gfxTutorialBar);
      this.gfxBottomBar.position = new Microsoft.Xna.Framework.Vector2(0.0f, 410f);
      this.AddSprite((SimpleRenderObject) this.gfxBottomBar);
      this.TransitionOnTime = TimeSpan.FromSeconds(0.25);
      this.TransitionOffTime = TimeSpan.FromSeconds(0.25);
      this.EnabledGestures = GestureType.Tap;
    }

    private string GetPreviousLevelNumber(int index) => index - 1 < 1 ? "" : (index - 1).ToString();

    private void AddSprite(SimpleRenderObject sprite) => this.sprites.Add(sprite);

    private void AddSprite(int index, SimpleRenderObject sprite)
    {
      this.sprites.Insert(index, sprite);
    }

    private void AddDot(PressPlay.FFWD.Vector2 position)
    {
      SimpleSprite sprite = new SimpleSprite(this.atlas, (Microsoft.Xna.Framework.Vector2) position, SpritePositions.loadingScreenDot);
      sprite.drawFromCenter = true;
      this.lastDotPosition = sprite.position;
      this.AddSprite(1, (SimpleRenderObject) sprite);
    }

    private void BuildTravelNodeList()
    {
      this.nodes = new List<TravelNode>();
      this.nodes.Add(new TravelNode(0, new PressPlay.FFWD.Vector2(100f, 240f), new PressPlay.FFWD.Vector2(700f, 240f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.tutorial), LemmyTravelScreen.TravelEnvironment.tutorial));
      this.nodes.Add(new TravelNode(1, new PressPlay.FFWD.Vector2(400f, -110f), new PressPlay.FFWD.Vector2(400f, 341f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(2, new PressPlay.FFWD.Vector2(227f, 263f), new PressPlay.FFWD.Vector2(369f, 210f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(3, new PressPlay.FFWD.Vector2(369f, 210f), new PressPlay.FFWD.Vector2(495f, 265f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(4, new PressPlay.FFWD.Vector2(438f, 341f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(5, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(400f, 400f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(6, new PressPlay.FFWD.Vector2(539f, 235f), new PressPlay.FFWD.Vector2(323f, 217f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(7, new PressPlay.FFWD.Vector2(323f, 217f), new PressPlay.FFWD.Vector2(452f, 290f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(8, new PressPlay.FFWD.Vector2(400f, 400f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(9, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(438f, 341f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(10, new PressPlay.FFWD.Vector2(385f, 263f), new PressPlay.FFWD.Vector2(259f, 221f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(11, new PressPlay.FFWD.Vector2(259f, 221f), new PressPlay.FFWD.Vector2(509f, 199f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(12, new PressPlay.FFWD.Vector2(438f, 341f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(13, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(410f, 44f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(14, new PressPlay.FFWD.Vector2(410f, 207f), new PressPlay.FFWD.Vector2(279f, 157f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(15, new PressPlay.FFWD.Vector2(279f, 157f), new PressPlay.FFWD.Vector2(463f, 137f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(16, new PressPlay.FFWD.Vector2(410f, 44f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(17, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(438f, 341f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(18, new PressPlay.FFWD.Vector2(307f, 281f), new PressPlay.FFWD.Vector2(430f, 169f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(19, new PressPlay.FFWD.Vector2(430f, 169f), new PressPlay.FFWD.Vector2(575f, 293f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(20, new PressPlay.FFWD.Vector2(438f, 341f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(21, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(400f, 400f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(22, new PressPlay.FFWD.Vector2(401f, 209f), new PressPlay.FFWD.Vector2(246f, (float) byte.MaxValue), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(23, new PressPlay.FFWD.Vector2(246f, (float) byte.MaxValue), new PressPlay.FFWD.Vector2(468f, 230f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(24, new PressPlay.FFWD.Vector2(400f, 400f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(25, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(410f, 44f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(26, new PressPlay.FFWD.Vector2(521f, 167f), new PressPlay.FFWD.Vector2(430f, 167f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(27, new PressPlay.FFWD.Vector2(430f, 167f), new PressPlay.FFWD.Vector2(331f, 137f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(28, new PressPlay.FFWD.Vector2(410f, 44f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(29, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(438f, 341f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(30, new PressPlay.FFWD.Vector2(577f, 231f), new PressPlay.FFWD.Vector2(317f, 186f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(31, new PressPlay.FFWD.Vector2(317f, 186f), new PressPlay.FFWD.Vector2(438f, 230f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.stomach), LemmyTravelScreen.TravelEnvironment.stomach));
      this.nodes.Add(new TravelNode(32, new PressPlay.FFWD.Vector2(438f, 341f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(33, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(400f, 400f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(34, new PressPlay.FFWD.Vector2(384f, 265f), new PressPlay.FFWD.Vector2(261f, 187f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(35, new PressPlay.FFWD.Vector2(261f, 187f), new PressPlay.FFWD.Vector2(326f, 289f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.intestines), LemmyTravelScreen.TravelEnvironment.intestines));
      this.nodes.Add(new TravelNode(36, new PressPlay.FFWD.Vector2(400f, 400f), new PressPlay.FFWD.Vector2(396f, 270f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(37, new PressPlay.FFWD.Vector2(396f, 270f), new PressPlay.FFWD.Vector2(410f, 44f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.veins), LemmyTravelScreen.TravelEnvironment.veins));
      this.nodes.Add(new TravelNode(38, new PressPlay.FFWD.Vector2(390f, 137f), new PressPlay.FFWD.Vector2(473f, 187f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(39, new PressPlay.FFWD.Vector2(473f, 187f), new PressPlay.FFWD.Vector2(570f, 293f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
      this.nodes.Add(new TravelNode(40, new PressPlay.FFWD.Vector2(570f, 293f), new PressPlay.FFWD.Vector2(405f, 185f), this.GetTexture(LemmyTravelScreen.TravelEnvironment.brain), LemmyTravelScreen.TravelEnvironment.brain));
    }

    private TravelNode GetTravelNode(int index)
    {
      foreach (TravelNode node in this.nodes)
      {
        if (node.levelIndex == index)
          return node;
      }
      return (TravelNode) null;
    }

    private void LoadTextures()
    {
      this.atlas = Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_atlas");
      this.gfxBottomBar = new SimpleSprite(Application.Load<Texture2D>("Textures/Menu/EndLevel/endlevel_pausemenu_atlas"), SpritePositions.ingameMenuBackgroundBottom);
      this.gfxTutorialBar = new SimpleSprite(Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bar_tutorial"));
    }

    private Texture2D GetTexture(LemmyTravelScreen.TravelEnvironment id)
    {
      this.currentEnvironment = id;
      switch (id)
      {
        case LemmyTravelScreen.TravelEnvironment.brain:
          return Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_brain");
        case LemmyTravelScreen.TravelEnvironment.intestines:
          return Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_intestines");
        case LemmyTravelScreen.TravelEnvironment.stomach:
          return Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_stomach");
        case LemmyTravelScreen.TravelEnvironment.veins:
          return Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_veins");
        default:
          return Application.Load<Texture2D>("Textures/Menu/LevelIntroTransition/loadingScreen_bg_tutorial");
      }
    }

    public override void HandleInput(InputState input)
    {
      base.HandleInput(input);
      if (this.loadingScreen.state != LoadingScreen2.LoadingState.complete || !LevelHandler.isLoaded || LevelHandler.Instance.state != LevelHandler.LevelState.preloadingDone)
        return;
      this.batchName = LocalisationManager.Instance.GetString("label_tap_to_cont");
      this.loadingText = "";
      PlayerIndex playerIndex;
      if (input.IsNewButtonPress(Buttons.Back, this.ControllingPlayer, out playerIndex) || input.IsNewKeyPress(Keys.Escape, this.ControllingPlayer, out playerIndex))
      {
        this.loadingScreen.StartGame();
      }
      else
      {
        foreach (GestureSample gesture in input.Gestures)
        {
          Debug.Log((object) ("gesture" + gesture.ToString()));
          if (gesture.GestureType == GestureType.Tap)
          {
            this.loadingScreen.StartGame();
            break;
          }
        }
      }
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      this.elapsedTime += Time.deltaTime;
      if ((double) this.elapsedTime > (double) this.timeToTravel)
        this.elapsedTime = this.timeToTravel;
      float amount = Application.loadingProgress * 0.9f;
      if (LevelHandler.isLoaded)
        amount += 0.05f;
      if (LevelHandler.isLoaded && LevelHandler.Instance.state != LevelHandler.LevelState.preloading && LevelHandler.Instance.state != LevelHandler.LevelState.idle)
        amount += 0.05f;
      if (this.currentTravelNode.environment == LemmyTravelScreen.TravelEnvironment.tutorial)
      {
        this.gfxTutorialBar.mask.Width = (int) ((double) this.gfxTutorialBar.gfx.Width * (double) Application.loadingProgress);
      }
      else
      {
        this.gfxLemmy.position = Microsoft.Xna.Framework.Vector2.Lerp((Microsoft.Xna.Framework.Vector2) this.currentTravelNode.start, (Microsoft.Xna.Framework.Vector2) this.currentTravelNode.end, amount);
        Microsoft.Xna.Framework.Vector2 vector2 = this.gfxLemmy.position - this.lastDotPosition;
        float num = vector2.Length();
        vector2.Normalize();
        if ((double) num > (double) this.distanceBetweenDots)
        {
          do
          {
            this.AddDot((PressPlay.FFWD.Vector2) (this.lastDotPosition + vector2 * this.distanceBetweenDots));
          }
          while ((double) (this.gfxLemmy.position - this.lastDotPosition).Length() > (double) this.distanceBetweenDots);
        }
      }
      if (this.isComplete || (double) Application.loadingProgress != 1.0)
        return;
      this.isComplete = true;
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      this.ScreenManager.FadeBackBufferToBlack(1f);
      this.ScreenManager.SpriteBatch.Begin();
      foreach (SimpleRenderObject sprite in this.sprites)
      {
        sprite.alpha = 1f - this.TransitionPosition;
        sprite.Draw(this.ScreenManager.SpriteBatch);
      }
      this.ScreenManager.SpriteBatch.DrawString(GUIAssets.berlinsSans40, this.loadingText, new Microsoft.Xna.Framework.Vector2(5f, 5f), Microsoft.Xna.Framework.Color.White, 0.0f, Microsoft.Xna.Framework.Vector2.Zero, 0.65f, SpriteEffects.None, 0.0f);
      this.ScreenManager.SpriteBatch.DrawString(GUIAssets.berlinsSans40, this.batchName, new Microsoft.Xna.Framework.Vector2(400f - (GUIAssets.berlinsSans40.MeasureString(this.batchName) / 2f).X, 426f), Microsoft.Xna.Framework.Color.White);
      this.ScreenManager.SpriteBatch.End();
    }

    public enum TravelEnvironment
    {
      tutorial,
      brain,
      intestines,
      stomach,
      veins,
    }
  }
}
