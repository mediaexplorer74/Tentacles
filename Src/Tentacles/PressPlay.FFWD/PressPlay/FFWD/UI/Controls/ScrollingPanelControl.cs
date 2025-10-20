// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ScrollingPanelControl
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using PressPlay.FFWD.ScreenManager;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ScrollingPanelControl : PanelControl
  {
    private ScrollTracker scrollTracker = new ScrollTracker();
    private Vector3 startPosition;
    private bool hasScrolled;

    public ScrollingPanelControl(int width, int height)
    {
      this.scrollTracker.ViewRect.Width = width;
      this.scrollTracker.ViewRect.Width = height;
      this.startPosition = this.transform.position;
      this.gameObject.name = nameof (ScrollingPanelControl);
    }

    public override void Update()
    {
      this.scrollTracker.CanvasRect.X = this.bounds.X;
      this.scrollTracker.CanvasRect.Y = this.bounds.Y;
      this.scrollTracker.CanvasRect.Width = this.bounds.Width;
      this.scrollTracker.CanvasRect.Height = this.bounds.Height;
      this.scrollTracker.Update();
      base.Update();
    }

    public override void HandleInput(InputState input)
    {
      base.HandleInput(input);
      bool flag = false;
      for (int index = 0; index < input.TouchState.Count; ++index)
      {
        if (this.scrollTracker.CanvasRect.Contains((int) input.TouchState[index].Position.X, (int) input.TouchState[index].Position.Y))
          flag = true;
      }
      if (flag)
        this.scrollTracker.HandleInput(input);
      if (this.hasScrolled && this.children != null)
        this.PositionChildControls(new Vector2(0.0f, (float) -this.scrollTracker.ViewRect.Y));
      this.hasScrolled = this.scrollTracker.IsMoving;
    }

    public void Init()
    {
      this.PositionChildControls(new Vector2(0.0f, (float) -this.scrollTracker.ViewRect.Y));
    }

    private void PositionChildControls(Vector2 topPosition)
    {
      if (this.children != null && this.children.Count > 0)
      {
        float num = this.yMargin + topPosition.y;
        for (int childIndex = 0; childIndex < this.childCount; ++childIndex)
        {
          Control control = this[childIndex];
          control.transform.localPosition = (Vector3) new Vector2()
          {
            x = this.xMargin,
            y = num
          };
          num += (float) control.bounds.Height + this.ySpacing;
        }
      }
      this.InvalidateAutoSize();
    }
  }
}
