// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.ScreenManager
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class ScreenManager : DrawableGameComponent
  {
    private List<GameScreen> screens = new List<GameScreen>();
    private List<GameScreen> screensToUpdate = new List<GameScreen>();
    public static InputState input = new InputState();
    private SpriteBatch spriteBatch;
    private SpriteFont font;
    private Texture2D blankTexture;
    public string fontSource;
    public string blankTextureSource;
    private bool isInitialized;
    private bool traceEnabled;

    public static GraphicsDevice Graphics { get; private set; }

    public static Viewport Viewport { get; private set; }

    public SpriteBatch SpriteBatch => this.spriteBatch;

    public SpriteFont Font => this.font;

    public Texture2D BlankTexture => this.blankTexture;

    public bool TraceEnabled
    {
      get => this.traceEnabled;
      set => this.traceEnabled = value;
    }

    public ScreenManager(Game game)
      : base(game)
    {
      TouchPanel.EnabledGestures = GestureType.None;
      this.UpdateOrder = 0;
      this.DrawOrder = 1;
      PressPlay.FFWD.ScreenManager.ScreenManager.Viewport = game.GraphicsDevice.Viewport;
      PressPlay.FFWD.ScreenManager.ScreenManager.Graphics = game.GraphicsDevice;
      Application.screenManager = this;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.isInitialized = true;
    }

    protected override void LoadContent()
    {
      ContentManager content = this.Game.Content;
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      if (this.fontSource != null)
        this.font = content.Load<SpriteFont>(this.fontSource);
      if (this.blankTextureSource != null)
        this.blankTexture = content.Load<Texture2D>(this.blankTextureSource);
      foreach (GameScreen screen in this.screens)
        screen.LoadContent();
    }

    protected override void UnloadContent()
    {
      foreach (GameScreen screen in this.screens)
        screen.UnloadContent();
    }

    public override void Update(GameTime gameTime)
    {
      PressPlay.FFWD.ScreenManager.ScreenManager.input.Update();
      this.screensToUpdate.Clear();
      foreach (GameScreen screen in this.screens)
        this.screensToUpdate.Add(screen);
      bool otherScreenHasFocus = !this.Game.IsActive;
      bool coveredByOtherScreen = false;
      while (this.screensToUpdate.Count > 0)
      {
        GameScreen gameScreen = this.screensToUpdate[this.screensToUpdate.Count - 1];
        this.screensToUpdate.RemoveAt(this.screensToUpdate.Count - 1);
        gameScreen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        if (gameScreen.ScreenState == ScreenState.TransitionOn || gameScreen.ScreenState == ScreenState.Active)
        {
          if (!otherScreenHasFocus)
          {
            gameScreen.HandleInput(PressPlay.FFWD.ScreenManager.ScreenManager.input);
            otherScreenHasFocus = true;
          }
          if (!gameScreen.IsPopup)
            coveredByOtherScreen = true;
        }
      }
      if (!this.traceEnabled)
        return;
      this.TraceScreens();
    }

    private void TraceScreens()
    {
      List<string> stringList = new List<string>();
      foreach (GameScreen screen in this.screens)
        stringList.Add(screen.GetType().Name);
      Debug.Log((object) string.Join(", ", stringList.ToArray()));
    }

    public override void Draw(GameTime gameTime)
    {
      for (int index = 0; index < this.screens.Count; ++index)
      {
        if (this.screens[index].ScreenState != ScreenState.Hidden)
          this.screens[index].Draw(gameTime);
      }
    }

    public void AddScreen(GameScreen screen) => this.AddScreen(screen, new PlayerIndex?());

    public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
    {
      screen.ControllingPlayer = controllingPlayer;
      screen.ScreenManager = this;
      screen.IsExiting = false;
      if (screen.Content == null)
        screen.Content = new CachedContent(new ContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory));
      if (this.isInitialized)
        screen.LoadContent();
      this.screens.Add(screen);
      TouchPanel.EnabledGestures = screen.EnabledGestures;
    }

    public void AddScreenBelow(GameScreen screen, PlayerIndex? controllingPlayer)
    {
      screen.ControllingPlayer = controllingPlayer;
      screen.ScreenManager = this;
      screen.IsExiting = false;
      if (this.isInitialized)
        screen.LoadContent();
      this.screens.Insert(Math.Max(0, this.screens.Count - 1), screen);
    }

    public void RemoveScreen(GameScreen screen)
    {
      if (this.isInitialized)
        screen.UnloadContent();
      this.screens.Remove(screen);
      this.screensToUpdate.Remove(screen);
      if (this.screens.Count <= 0)
        return;
      TouchPanel.EnabledGestures = this.screens[this.screens.Count - 1].EnabledGestures;
    }

    public void NotifyOtherScreens(GameScreen self)
    {
      for (int index = 0; index < this.screens.Count; ++index)
      {
        if (this.screens[index] != self)
          this.screens[index].OnNotifyCallback();
      }
    }

    public void RemoveAllOtherScreens(GameScreen self)
    {
      for (int index = this.screens.Count - 1; index >= 0; --index)
      {
        if (this.screens[index] != self)
          this.RemoveScreen(this.screens[index]);
      }
    }

    public GameScreen[] GetScreens() => this.screens.ToArray();

    public void FadeBackBufferToBlack(float alpha)
    {
      this.spriteBatch.Begin();
      this.spriteBatch.Draw(this.blankTexture, new Rectangle(0, 0, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height), (Microsoft.Xna.Framework.Color) (PressPlay.FFWD.Color.black * alpha));
      this.spriteBatch.End();
    }

    public void OnActivated()
    {
      foreach (GameScreen screen in this.screens)
        screen.OnActivated();
    }

    public void OnDeactivated()
    {
      foreach (GameScreen screen in this.screens)
        screen.OnDeactivated();
    }

    public void SerializeState()
    {
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        if (storeForApplication.DirectoryExists(nameof (ScreenManager)))
          this.DeleteState(storeForApplication);
        else
          storeForApplication.CreateDirectory(nameof (ScreenManager));
        using (IsolatedStorageFileStream file = storeForApplication.CreateFile("ScreenManager\\ScreenList.dat"))
        {
          using (BinaryWriter binaryWriter = new BinaryWriter((Stream) file))
          {
            foreach (GameScreen screen in this.screens)
            {
              if (screen.IsSerializable)
                binaryWriter.Write(screen.GetType().AssemblyQualifiedName);
            }
          }
        }
        int num = 0;
        foreach (GameScreen screen in this.screens)
        {
          if (screen.IsSerializable)
          {
            string path = string.Format("ScreenManager\\Screen{0}.dat", (object) num);
            using (IsolatedStorageFileStream file = storeForApplication.CreateFile(path))
              screen.Serialize((Stream) file);
            ++num;
          }
        }
      }
    }

    public bool DeserializeState()
    {
      using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
      {
        if (storeForApplication.DirectoryExists(nameof (ScreenManager)))
        {
          try
          {
            if (storeForApplication.FileExists("ScreenManager\\ScreenList.dat"))
            {
              using (IsolatedStorageFileStream input = storeForApplication.OpenFile("ScreenManager\\ScreenList.dat", FileMode.Open, FileAccess.Read))
              {
                using (BinaryReader binaryReader = new BinaryReader((Stream) input))
                {
                  while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                  {
                    string typeName = binaryReader.ReadString();
                    if (!string.IsNullOrEmpty(typeName))
                      this.AddScreen(Activator.CreateInstance(Type.GetType(typeName)) as GameScreen, new PlayerIndex?(PlayerIndex.One));
                  }
                }
              }
            }
            for (int index = 0; index < this.screens.Count; ++index)
            {
              string path = string.Format("ScreenManager\\Screen{0}.dat", (object) index);
              using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(path, FileMode.Open, FileAccess.Read))
                this.screens[index].Deserialize((Stream) storageFileStream);
            }
            return true;
          }
          catch (Exception ex)
          {
            this.DeleteState(storeForApplication);
          }
        }
      }
      return false;
    }

    private void DeleteState(IsolatedStorageFile storage)
    {
      foreach (string fileName in storage.GetFileNames("ScreenManager\\*"))
        storage.DeleteFile(Path.Combine(nameof (ScreenManager), fileName));
    }
  }
}
