// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.GameScreen
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.IO;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public abstract class GameScreen
  {
    private bool isPopup;
    private static CachedContent content;
    private TimeSpan transitionOnTime = TimeSpan.Zero;
    private TimeSpan transitionOffTime = TimeSpan.Zero;
    private float transitionPosition = 1f;
    private ScreenState screenState;
    private bool isExiting;
    private bool otherScreenHasFocus;
    private PressPlay.FFWD.ScreenManager.ScreenManager screenManager;
    private PlayerIndex? controllingPlayer;
    private GestureType enabledGestures;
    private bool isSerializable = true;
    public bool isLoading;

    public bool IsPopup
    {
      get => this.isPopup;
      protected set => this.isPopup = value;
    }

    public CachedContent Content
    {
      get => GameScreen.content;
      set => GameScreen.content = value;
    }

    public TimeSpan TransitionOnTime
    {
      get => this.transitionOnTime;
      protected set => this.transitionOnTime = value;
    }

    public TimeSpan TransitionOffTime
    {
      get => this.transitionOffTime;
      protected set => this.transitionOffTime = value;
    }

    public float TransitionPosition
    {
      get => this.transitionPosition;
      protected set => this.transitionPosition = value;
    }

    public float TransitionAlpha => 1f - this.TransitionPosition;

    public ScreenState ScreenState
    {
      get => this.screenState;
      protected set => this.screenState = value;
    }

    public bool IsExiting
    {
      get => this.isExiting;
      protected internal set => this.isExiting = value;
    }

    public bool IsActive
    {
      get
      {
        if (this.otherScreenHasFocus)
          return false;
        return this.screenState == ScreenState.TransitionOn || this.screenState == ScreenState.Active;
      }
    }

    public PressPlay.FFWD.ScreenManager.ScreenManager ScreenManager
    {
      get => this.screenManager;
      internal set => this.screenManager = value;
    }

    public PlayerIndex? ControllingPlayer
    {
      get => this.controllingPlayer;
      internal set => this.controllingPlayer = value;
    }

    public GestureType EnabledGestures
    {
      get => this.enabledGestures;
      protected set
      {
        this.enabledGestures = value;
        if (this.ScreenState != ScreenState.Active)
          return;
        TouchPanel.EnabledGestures = value;
      }
    }

    public bool IsSerializable
    {
      get => this.isSerializable;
      protected set => this.isSerializable = value;
    }

    public virtual void LoadContent()
    {
    }

    public virtual void UnloadContent()
    {
    }

    public virtual void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      this.otherScreenHasFocus = otherScreenHasFocus;
      if (this.isExiting)
      {
        this.screenState = ScreenState.TransitionOff;
        if (this.UpdateTransition(gameTime, this.transitionOffTime, 1))
          return;
        this.ScreenManager.RemoveScreen(this);
        this.OnTransitionExitComplete();
      }
      else if (coveredByOtherScreen)
      {
        if (this.UpdateTransition(gameTime, this.transitionOffTime, 1))
        {
          if (this.screenState != ScreenState.TransitionOff)
            this.OnTransitionOffBegin();
          this.screenState = ScreenState.TransitionOff;
        }
        else
        {
          if (this.screenState != ScreenState.Hidden)
            this.OnTransitionOffComplete();
          this.screenState = ScreenState.Hidden;
        }
      }
      else if (this.UpdateTransition(gameTime, this.transitionOnTime, -1))
      {
        if (this.screenState != ScreenState.TransitionOn)
          this.OnTransitionOnBegin();
        this.screenState = ScreenState.TransitionOn;
      }
      else
      {
        if (this.screenState != ScreenState.Active)
          this.OnTransitionOnComplete();
        this.screenState = ScreenState.Active;
      }
    }

    private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
    {
      this.transitionPosition += (!(time == TimeSpan.Zero) ? (float) (gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds) : 1f) * (float) direction;
      if ((direction >= 0 || (double) this.transitionPosition > 0.0) && (direction <= 0 || (double) this.transitionPosition < 1.0))
        return true;
      this.transitionPosition = MathHelper.Clamp(this.transitionPosition, 0.0f, 1f);
      return false;
    }

    public virtual void HandleInput(InputState input)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    public virtual void Serialize(Stream stream)
    {
    }

    public virtual void Deserialize(Stream stream)
    {
    }

    public void ExitScreen()
    {
      if (this.TransitionOffTime == TimeSpan.Zero)
        this.ScreenManager.RemoveScreen(this);
      else
        this.isExiting = true;
    }

    public virtual void OnNotifyCallback()
    {
    }

    public virtual void OnTransitionOnBegin()
    {
    }

    public virtual void OnTransitionOnComplete()
    {
    }

    public virtual void OnTransitionOffBegin()
    {
    }

    public virtual void OnTransitionOffComplete()
    {
    }

    public virtual void OnTransitionExitComplete()
    {
    }

    public virtual void OnActivated()
    {
    }

    public virtual void OnDeactivated()
    {
    }
  }
}
