// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ScrollTracker
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD.ScreenManager;
using System;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ScrollTracker
  {
    public const GestureType GesturesNeeded = GestureType.VerticalDrag | GestureType.Flick | GestureType.DragComplete;
    private const float SpringMaxDrag = 400f;
    private const float SpringMaxOffset = 133.333328f;
    private const float SpringReturnRate = 0.1f;
    private const float SpringReturnMin = 2f;
    private const float Deceleration = 500f;
    private const float MaxVelocity = 2000f;
    public Rectangle CanvasRect;
    public Rectangle ViewRect;
    private PressPlay.FFWD.Vector2 Velocity;
    private PressPlay.FFWD.Vector2 ViewOrigin;
    private PressPlay.FFWD.Vector2 UnclampedViewOrigin;

    public Rectangle FullCanvasRect
    {
      get
      {
        Rectangle canvasRect = this.CanvasRect;
        if (canvasRect.Width < this.ViewRect.Width)
          canvasRect.Width = this.ViewRect.Width;
        if (canvasRect.Height < this.ViewRect.Height)
          canvasRect.Height = this.ViewRect.Height;
        return canvasRect;
      }
    }

    public bool IsTracking { get; private set; }

    public bool IsMoving
    {
      get
      {
        return this.IsTracking || (double) this.Velocity.x != 0.0 || (double) this.Velocity.y != 0.0 || !this.FullCanvasRect.Contains(this.ViewRect);
      }
    }

    public ScrollTracker()
    {
      this.ViewRect = new Rectangle()
      {
        Width = TouchPanel.DisplayWidth,
        Height = TouchPanel.DisplayHeight
      };
      this.CanvasRect = this.ViewRect;
    }

    public void Update()
    {
      float deltaTime = Time.deltaTime;
      PressPlay.FFWD.Vector2 vector2_1 = new PressPlay.FFWD.Vector2()
      {
        x = 0.0f,
        y = 0.0f
      };
      PressPlay.FFWD.Vector2 vector2_2 = new PressPlay.FFWD.Vector2()
      {
        x = (float) (this.CanvasRect.Width - this.ViewRect.Width),
        y = (float) (this.CanvasRect.Height - this.ViewRect.Height)
      };
      vector2_2.x = Math.Max(vector2_1.x, vector2_2.x);
      vector2_2.y = Math.Max(vector2_1.y, vector2_2.y);
      if (this.IsTracking)
      {
        this.ViewOrigin.x = ScrollTracker.SoftClamp(this.UnclampedViewOrigin.x, vector2_1.x, vector2_2.x);
        this.ViewOrigin.y = ScrollTracker.SoftClamp(this.UnclampedViewOrigin.y, vector2_1.y, vector2_2.y);
      }
      else
      {
        this.ApplyVelocity(deltaTime, ref this.ViewOrigin.x, ref this.Velocity.x, vector2_1.x, vector2_2.x);
        this.ApplyVelocity(deltaTime, ref this.ViewOrigin.y, ref this.Velocity.y, vector2_1.y, vector2_2.y);
      }
      this.ViewRect.X = (int) this.ViewOrigin.x;
      this.ViewRect.Y = (int) this.ViewOrigin.y;
    }

    public void MoveScrollTracker(float value) => this.ViewRect.Y += (int) value;

    public void HandleInput(InputState input)
    {
      if (!this.IsTracking)
      {
        for (int index = 0; index < input.TouchState.Count; ++index)
        {
          if (input.TouchState[index].State == TouchLocationState.Pressed)
          {
            this.Velocity = PressPlay.FFWD.Vector2.zero;
            this.UnclampedViewOrigin = this.ViewOrigin;
            this.IsTracking = true;
            break;
          }
        }
      }
      foreach (GestureSample gesture in input.Gestures)
      {
        switch (gesture.GestureType)
        {
          case GestureType.VerticalDrag:
            this.UnclampedViewOrigin.y -= gesture.Delta.Y;
            continue;
          case GestureType.Flick:
            if ((double) Math.Abs(gesture.Delta.X) < (double) Math.Abs(gesture.Delta.Y))
            {
              this.IsTracking = false;
              this.Velocity = (PressPlay.FFWD.Vector2)(-gesture.Delta);
              continue;
            }
            continue;
          case GestureType.DragComplete:
            this.IsTracking = false;
            continue;
          default:
            continue;
        }
      }
    }

    private static float SoftClamp(float x, float min, float max)
    {
      if ((double) x < (double) min)
        return (float) ((double) Math.Max(x - min, -400f) * 133.33332824707031 / 400.0) + min;
      return (double) x > (double) max ? (float) ((double) Math.Min(x - max, 400f) * 133.33332824707031 / 400.0) + max : x;
    }

    private void ApplyVelocity(float dt, ref float x, ref float v, float min, float max)
    {
      x += v * dt;
      v = MathHelper.Clamp(v, -2000f, 2000f);
      v = Math.Max(Math.Abs(v) - dt * 500f, 0.0f) * (float) Math.Sign(v);
      if ((double) x < (double) min)
      {
        x = Math.Min((float) ((double) x + ((double) min - (double) x) * 0.10000000149011612 + 2.0), min);
        v = 0.0f;
      }
      if ((double) x <= (double) max)
        return;
      x = Math.Max((float) ((double) x - ((double) x - (double) max) * 0.10000000149011612 - 2.0), max);
      v = 0.0f;
    }
  }
}
