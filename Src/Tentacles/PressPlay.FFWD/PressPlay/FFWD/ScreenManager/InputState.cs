// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.InputState
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class InputState
  {
    public const int MaxInputs = 4;
    public readonly KeyboardState[] CurrentKeyboardStates;
    public readonly GamePadState[] CurrentGamePadStates;
    public readonly KeyboardState[] LastKeyboardStates;
    public readonly GamePadState[] LastGamePadStates;
    public readonly bool[] GamePadWasConnected;
    private bool isMousePressed;
    private bool isMouseReleased = true;
    private bool _isMouseDown;
    private bool _isMouseUp;
    private PressPlay.FFWD.Vector2 _mousePosition = PressPlay.FFWD.Vector2.zero;
    public MouseState CurrentMouseState;
    public TouchCollection TouchState;
    public readonly List<GestureSample> Gestures = new List<GestureSample>();

    public bool isMouseDown
    {
      get
      {
        bool isMouseDown = this._isMouseDown;
        this._isMouseDown = false;
        return isMouseDown;
      }
      set => this._isMouseDown = value;
    }

    public bool isMouseUp
    {
      get
      {
        bool isMouseUp = this._isMouseUp;
        this._isMouseUp = false;
        return isMouseUp;
      }
      set => this._isMouseUp = value;
    }

    public PressPlay.FFWD.Vector2 mousePosition
    {
      get
      {
        if ((double) this._mousePosition.x != (double) this.CurrentMouseState.X || (double) this._mousePosition.y != (double) this.CurrentMouseState.Y)
          this._mousePosition = new PressPlay.FFWD.Vector2((float) this.CurrentMouseState.X, (float) this.CurrentMouseState.Y);
        return this._mousePosition;
      }
    }

    public InputState()
    {
      this.CurrentKeyboardStates = new KeyboardState[4];
      this.CurrentGamePadStates = new GamePadState[4];
      this.LastKeyboardStates = new KeyboardState[4];
      this.LastGamePadStates = new GamePadState[4];
      this.GamePadWasConnected = new bool[4];
    }

    public void Update()
    {
      for (int index = 0; index < 4; ++index)
      {
        this.LastKeyboardStates[index] = this.CurrentKeyboardStates[index];
        this.LastGamePadStates[index] = this.CurrentGamePadStates[index];
        this.CurrentKeyboardStates[index] = Keyboard.GetState((PlayerIndex) index);
        this.CurrentGamePadStates[index] = GamePad.GetState((PlayerIndex) index);
        if (this.CurrentGamePadStates[index].IsConnected)
          this.GamePadWasConnected[index] = true;
      }
      this.CurrentMouseState = Mouse.GetState();
      this.TouchState = TouchPanel.GetState();
      if (this.CurrentMouseState.LeftButton == ButtonState.Pressed)
      {
        if (this.isMousePressed)
        {
          this.isMouseDown = false;
        }
        else
        {
          this.isMouseDown = true;
          this.isMousePressed = true;
          this.isMouseReleased = false;
        }
      }
      if (this.CurrentMouseState.LeftButton == ButtonState.Released)
      {
        if (this.isMouseReleased)
        {
          this.isMouseUp = false;
        }
        else
        {
          this.isMouseUp = true;
          this.isMouseReleased = true;
          this.isMousePressed = false;
        }
      }
      this.Gestures.Clear();
      while (TouchPanel.IsGestureAvailable)
        this.Gestures.Add(TouchPanel.ReadGesture());
    }

    public bool IsNewKeyPress(
      Keys key,
      PlayerIndex? controllingPlayer,
      out PlayerIndex playerIndex)
    {
      if (controllingPlayer.HasValue)
      {
        playerIndex = controllingPlayer.Value;
        int index = (int) playerIndex;
        return this.CurrentKeyboardStates[index].IsKeyDown(key) && this.LastKeyboardStates[index].IsKeyUp(key);
      }
      return this.IsNewKeyPress(key, new PlayerIndex?(PlayerIndex.One), out playerIndex) || this.IsNewKeyPress(key, new PlayerIndex?(PlayerIndex.Two), out playerIndex) || this.IsNewKeyPress(key, new PlayerIndex?(PlayerIndex.Three), out playerIndex) || this.IsNewKeyPress(key, new PlayerIndex?(PlayerIndex.Four), out playerIndex);
    }

    public bool IsNewButtonPress(
      Buttons button,
      PlayerIndex? controllingPlayer,
      out PlayerIndex playerIndex)
    {
      if (controllingPlayer.HasValue)
      {
        playerIndex = controllingPlayer.Value;
        int index = (int) playerIndex;
        return this.CurrentGamePadStates[index].IsButtonDown(button) && this.LastGamePadStates[index].IsButtonUp(button);
      }
      return this.IsNewButtonPress(button, new PlayerIndex?(PlayerIndex.One), out playerIndex) || this.IsNewButtonPress(button, new PlayerIndex?(PlayerIndex.Two), out playerIndex) || this.IsNewButtonPress(button, new PlayerIndex?(PlayerIndex.Three), out playerIndex) || this.IsNewButtonPress(button, new PlayerIndex?(PlayerIndex.Four), out playerIndex);
    }

    public bool IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
    {
      return this.IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) || this.IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
    }

    public bool IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
    {
      return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
    }

    public bool IsMenuUp(PlayerIndex? controllingPlayer)
    {
      PlayerIndex playerIndex;
      return this.IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
    }

    public bool IsMenuDown(PlayerIndex? controllingPlayer)
    {
      PlayerIndex playerIndex;
      return this.IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
    }

    public bool IsPauseGame(PlayerIndex? controllingPlayer)
    {
      PlayerIndex playerIndex;
      return this.IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) || this.IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
    }
  }
}
