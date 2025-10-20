// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.PanelControl
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class PanelControl : Control
  {
    public Rectangle clipRect;
    protected float xMargin;
    protected float yMargin;
    protected float xSpacing;
    protected float ySpacing;

    public PanelControl() => this.gameObject.name = nameof (PanelControl);

    public void LayoutColumn() => this.LayoutColumn(0.0f, 0.0f, 0.0f);

    public virtual void LayoutColumn(float xMargin, float yMargin, float ySpacing)
    {
      this.xMargin = xMargin;
      this.yMargin = yMargin;
      this.ySpacing = ySpacing;
      PressPlay.FFWD.Vector2 position = (PressPlay.FFWD.Vector2) this.transform.position;
      float num = yMargin + position.y;
      for (int childIndex = 0; childIndex < this.childCount; ++childIndex)
      {
        Control control = this[childIndex];
        control.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2()
        {
          x = (position.x + xMargin),
          y = num
        };
        num += (float) control.bounds.Height + ySpacing;
      }
      this.InvalidateAutoSize();
    }

    public void LayoutRow() => this.LayoutRow(0.0f, 0.0f, 0.0f);

    public virtual void LayoutRow(float xMargin, float yMargin, float xSpacing)
    {
      this.xMargin = xMargin;
      this.yMargin = yMargin;
      this.xSpacing = xSpacing;
      PressPlay.FFWD.Vector2 zero = PressPlay.FFWD.Vector2.zero;
      float num = xMargin + zero.x;
      for (int childIndex = 0; childIndex < this.childCount; ++childIndex)
      {
        Control control = this[childIndex];
        control.transform.localPosition = (PressPlay.FFWD.Vector3) new PressPlay.FFWD.Vector2()
        {
          x = num,
          y = (zero.y + yMargin)
        };
        num += (float) control.bounds.Width + xSpacing;
      }
      this.InvalidateAutoSize();
    }

    public virtual void LayoutRowCentered(float xMargin, float yMargin, float xSpacing)
    {
      this.xMargin = xMargin;
      this.yMargin = yMargin;
      this.xSpacing = xSpacing;
      PressPlay.FFWD.Vector3 position = this.transform.position;
      float x = xMargin + position.x;
      float num1 = 0.0f;
      for (int childIndex = 0; childIndex < this.childCount; ++childIndex)
      {
        Control control = this[childIndex];
        control.transform.position = new PressPlay.FFWD.Vector3(x, position.y, position.z + yMargin);
        x += (float) control.bounds.Width + xSpacing;
        num1 += (float) control.bounds.Width + xSpacing;
      }
      float num2 = num1 - xSpacing;
      for (int childIndex = 0; childIndex < this.childCount; ++childIndex)
      {
        Control control = this[childIndex];
        control.transform.position = control.transform.position + new PressPlay.FFWD.Vector3((float) (-(double) num2 * 0.5), 0.0f, 0.0f);
      }
      this.InvalidateAutoSize();
    }

    protected override void OnChildAdded(int index, Control child)
    {
      base.OnChildAdded(index, child);
    }
  }
}
