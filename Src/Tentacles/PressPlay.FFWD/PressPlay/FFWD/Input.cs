// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Input
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD.ScreenManager;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public static class Input
  {
    private static MouseState lastMouseState;
    private static MouseState currentMouseState;
    private static Vector2 lastTap;
    private static bool newTap = false;
    private static List<GestureSample> samples = new List<GestureSample>();

    internal static void Initialize()
    {
    }

    public static void Update(InputState inputState)
    {
      PressPlay.FFWD.Input.lastMouseState = PressPlay.FFWD.Input.currentMouseState;
      PressPlay.FFWD.Input.currentMouseState = Mouse.GetState();
      PressPlay.FFWD.Input.samples = inputState.Gestures;
      PressPlay.FFWD.Input.newTap = false;
      for (int index = 0; index < inputState.TouchState.Count; ++index)
      {
        if (inputState.TouchState[index].State == TouchLocationState.Pressed)
        {
          PressPlay.FFWD.Input.lastTap = (Vector2) inputState.TouchState[index].Position;
          PressPlay.FFWD.Input.newTap = true;
        }
      }
    }

    public static IEnumerable<GestureSample> GetSample(GestureType type)
    {
      for (int i = 0; i < PressPlay.FFWD.Input.samples.Count; ++i)
      {
        if ((PressPlay.FFWD.Input.samples[i].GestureType & type) != GestureType.None)
          yield return PressPlay.FFWD.Input.samples[i];
      }
    }

    public static bool HasSample(GestureType type)
    {
      for (int index = 0; index < PressPlay.FFWD.Input.samples.Count; ++index)
      {
        if ((PressPlay.FFWD.Input.samples[index].GestureType & type) != GestureType.None)
          return true;
      }
      return false;
    }

    public static Vector2 mousePosition => PressPlay.FFWD.Input.lastTap;

    public static float GetAxis(string axisName) => 0.0f;

    public static bool GetButton(string buttonName) => false;

    public static bool GetButtonUp(string buttonName) => false;

    public static bool GetButtonDown(string buttonName) => false;

    public static bool GetMouseButton(int button) => PressPlay.FFWD.Input.newTap;

    public static bool GetMouseButtonDown(int button) => PressPlay.FFWD.Input.newTap;

    public static bool GetMouseButtonUp(int button) => !PressPlay.FFWD.Input.newTap;
  }
}
