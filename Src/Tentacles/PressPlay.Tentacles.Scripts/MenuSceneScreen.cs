// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.MenuSceneScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class MenuSceneScreen : GameScreen
  {
    protected List<Control> controls = new List<Control>();
    protected Control rootControl;
    private List<ButtonComponent> menuEntries = new List<ButtonComponent>();
    private string menuTitle;

    protected IList<ButtonComponent> MenuEntries => (IList<ButtonComponent>) this.menuEntries;

    public MenuSceneScreen(string menuTitle)
    {
      this.EnabledGestures = GestureType.Tap | GestureType.VerticalDrag | GestureType.Flick | GestureType.DragComplete;
      this.menuTitle = menuTitle;
      this.TransitionOnTime = TimeSpan.FromSeconds(0.25);
      this.TransitionOffTime = TimeSpan.FromSeconds(0.25);
      this.rootControl = new Control();
      this.controls.Add(this.rootControl);
    }

    protected void SetTransitionPositionOnControls(float position)
    {
      for (int index = 0; index < this.controls.Count; ++index)
        this.controls[index].DoTransition(position);
    }

    public override void HandleInput(InputState input)
    {
      base.HandleInput(input);
      PlayerIndex playerIndex;
      if (input.IsMenuCancel(this.ControllingPlayer, out playerIndex))
        this.OnCancel(playerIndex);
      foreach (Control control in this.controls)
        control.HandleInput(input);
    }

    public bool IsMouseWithinBounds(GameObject go, Point pointer)
    {
      SpriteRenderer renderer = (SpriteRenderer) go.renderer;
      return pointer.X > renderer.bounds.Left && pointer.X < renderer.bounds.Right && pointer.Y > renderer.bounds.Top && pointer.Y < renderer.bounds.Bottom;
    }

    protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
    {
      this.menuEntries[entryIndex].OnSelectEntry(playerIndex);
    }

    protected virtual void OnCancel(PlayerIndex playerIndex)
    {
      AudioManager.Instance.Play(new AudioClip(GUIAssets.menuBackSound));
      this.ExitScreen();
    }

    protected void OnCancel(object sender, PlayerIndexEventArgs e) => this.OnCancel(e.PlayerIndex);

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      foreach (Control control in this.controls)
      {
        if (control.gameObject != null)
          control.Update();
      }
    }

    public override void Draw(GameTime gameTime)
    {
      GraphicsDevice graphicsDevice = this.ScreenManager.GraphicsDevice;
      SpriteBatch spriteBatch = this.ScreenManager.SpriteBatch;
      SpriteFont font = this.ScreenManager.Font;
      foreach (Control control in this.controls)
      {
        if (control.gameObject != null)
          control.DoTransition(this.TransitionPosition);
      }
    }

    public override void LoadContent() => base.LoadContent();

    public override void UnloadContent()
    {
      base.UnloadContent();
      this.Dispose();
    }

    public void Dispose()
    {
      foreach (Control control in this.controls)
      {
        if (control.gameObject != null)
          UnityObject.Destroy((UnityObject) control.gameObject);
      }
      this.controls.Clear();
    }

    public override void OnTransitionOnBegin()
    {
      base.OnTransitionOnBegin();
      this.rootControl.gameObject.SetActiveRecursively(true);
    }

    public override void OnTransitionExitComplete()
    {
      base.OnTransitionExitComplete();
      if (this.rootControl.gameObject == null)
        return;
      this.rootControl.gameObject.SetActiveRecursively(false);
    }
  }
}
