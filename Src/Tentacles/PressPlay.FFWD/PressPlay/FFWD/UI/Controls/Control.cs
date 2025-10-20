// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.Control
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD.ScreenManager;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class Control : Component, PressPlay.FFWD.Interfaces.IUpdateable
  {
    private PressPlay.FFWD.Vector2 _size;
    private bool sizeValid;
    private bool autoSize = true;
    public bool ignoreSize;
    protected List<Control> children;
    public PressPlay.FFWD.Vector2 drawOffset = PressPlay.FFWD.Vector2.zero;

    public Rectangle bounds
    {
      get
      {
        PressPlay.FFWD.Vector2 position = (PressPlay.FFWD.Vector2) this.transform.position;
        PressPlay.FFWD.Vector2 lossyScale = (PressPlay.FFWD.Vector2) this.transform.lossyScale;
        return new Rectangle((int) position.x, (int) position.y, (int) ((double) this.size.x * (double) lossyScale.x), (int) ((double) this.size.y * (double) lossyScale.y));
      }
    }

    public PressPlay.FFWD.Vector2 position
    {
      get => (PressPlay.FFWD.Vector2) this.gameObject.transform.localPosition;
      set
      {
        this.gameObject.transform.localPosition = (PressPlay.FFWD.Vector3) value;
        if (this.parent == null)
          return;
        this.parent.InvalidateAutoSize();
      }
    }

    public PressPlay.FFWD.Vector2 size
    {
      get
      {
        if (!this.sizeValid)
        {
          this._size = this.ComputeSize();
          this.sizeValid = true;
        }
        return this._size;
      }
      set
      {
        this._size = value;
        this.sizeValid = true;
        this.autoSize = false;
        if (this.parent == null)
          return;
        this.parent.InvalidateAutoSize();
      }
    }

    public void InvalidateAutoSize()
    {
      if (!this.autoSize)
        return;
      this.sizeValid = false;
      if (this.parent == null)
        return;
      this.parent.InvalidateAutoSize();
    }

    public Control parent { get; private set; }

    public int childCount => this.children != null ? this.children.Count : 0;

    public Control this[int childIndex] => this.children[childIndex];

    public Control()
    {
      if (this.gameObject != null)
        return;
      new GameObject("control").AddComponent<Control>(this);
    }

    public virtual void HandleInput(InputState input)
    {
      for (int index = 0; index < this.childCount; ++index)
        this.children[index].HandleInput(input);
    }

    public void SetScale(PressPlay.FFWD.Vector3 scale)
    {
      this.transform.localScale = scale;
      this.InvalidateAutoSize();
    }

    public virtual PressPlay.FFWD.Vector2 ComputeSize()
    {
      if (this.children == null || this.children.Count == 0)
        return PressPlay.FFWD.Vector2.zero;
      if (this is ScrollingPanelControl)
      {
        PressPlay.FFWD.Vector2 vector2_1 = new PressPlay.FFWD.Vector2(float.MaxValue, float.MaxValue);
        PressPlay.FFWD.Vector2 vector2_2 = new PressPlay.FFWD.Vector2(float.MinValue, float.MinValue);
        for (int index = 0; index < this.children.Count; ++index)
        {
          if (!this.children[index].ignoreSize)
          {
            vector2_1.x = Math.Min(vector2_1.x, this.children[index].position.x);
            vector2_1.y = Math.Min(vector2_1.y, this.children[index].position.y);
            vector2_2.x = Math.Max(vector2_2.x, this.children[index].position.x + this.children[index].size.x);
            vector2_2.y = Math.Max(vector2_2.y, this.children[index].position.y + this.children[index].size.y);
          }
        }
        return vector2_2 - vector2_1;
      }
      PressPlay.FFWD.Vector2 size = this.children[0].position + this.children[0].size;
      for (int index = 1; index < this.children.Count; ++index)
      {
        if (!this.children[index].ignoreSize)
        {
          PressPlay.FFWD.Vector2 vector2 = this.children[index].position + this.children[index].size;
          size.x = Math.Max(size.x, vector2.x);
          size.y = Math.Max(size.y, vector2.y);
        }
      }
      return size;
    }

    protected virtual bool isMouseWithinBounds(InputState input)
    {
      return this.isMouseWithinBounds(input, this.bounds);
    }

    protected virtual bool isMouseWithinBounds(InputState input, Rectangle box)
    {
      PressPlay.FFWD.Vector2 mousePosition = input.mousePosition;
      return box.Contains((int) mousePosition.x, (int) mousePosition.y);
    }

    protected virtual bool isTouchWithinBounds(InputState input)
    {
      return this.isTouchWithinBounds(input, this.bounds);
    }

    protected virtual bool isTouchWithinBounds(InputState input, Rectangle box)
    {
      for (int index = 0; index < input.TouchState.Count; ++index)
      {
        if (box.Contains((int) input.TouchState[index].Position.X, (int) input.TouchState[index].Position.Y))
          return true;
      }
      return false;
    }

    public virtual void DoTransition(float transitionTime)
    {
      if (this.gameObject != null && this.gameObject.renderer != null)
      {
        UIRenderer renderer = (UIRenderer) this.gameObject.renderer;
        renderer.material.color = new PressPlay.FFWD.Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 1f - transitionTime);
      }
      for (int index = 0; index < this.childCount; ++index)
        this.children[index].DoTransition(transitionTime);
    }

    public void AlignCenter() => this.AlignCenter(PressPlay.FFWD.Vector2.zero);

    public void AlignCenter(PressPlay.FFWD.Vector2 offset)
    {
      if (this.parent == null)
        return;
      bool flag = false;
      if (!this.ignoreSize)
      {
        this.ignoreSize = true;
        flag = true;
      }
      this.InvalidateAutoSize();
      this.transform.localPosition = new PressPlay.FFWD.Vector3(offset.x + (float) (this.parent.bounds.Width / 2) - (float) (this.bounds.Width / 2), this.transform.localPosition.y, offset.y + (float) (this.parent.bounds.Height / 2) - (float) (this.bounds.Height / 2));
      if (flag)
        this.ignoreSize = false;
      this.InvalidateAutoSize();
    }

    public void AlignCenter(Rectangle alignBounds)
    {
      this.transform.localPosition = new PressPlay.FFWD.Vector3((float) (alignBounds.X + alignBounds.Width / 2 - this.bounds.Width / 2), this.transform.localPosition.y, (float) (alignBounds.Y + alignBounds.Height / 2 - this.bounds.Height / 2));
    }

    public virtual void Update()
    {
    }

    public void LateUpdate()
    {
    }

    public void AddChild(Control child)
    {
      if (child.parent != null)
        child.parent.RemoveChild(child);
      this.AddChild(child, this.childCount);
    }

    public void AddChild(Control child, int index)
    {
      if (child.parent != null)
        child.parent.RemoveChild(child);
      if (this.children == null)
        this.children = new List<Control>();
      child.parent = this;
      child.transform.parent = this.transform;
      PressPlay.FFWD.Vector2 localPosition = (PressPlay.FFWD.Vector2) child.transform.localPosition;
      child.transform.localPosition = new PressPlay.FFWD.Vector3(localPosition, (float) (this.children.Count + 1));
      this.children.Insert(index, child);
      this.OnChildAdded(index, child);
    }

    public void RemoveChildAt(int index)
    {
      Control child = this.children[index];
      child.parent = (Control) null;
      child.transform.parent = (Transform) null;
      this.children.RemoveAt(index);
      this.OnChildRemoved(index, child);
    }

    public void RemoveChild(Control child)
    {
      if (child.parent != this)
        throw new InvalidOperationException();
      this.RemoveChildAt(this.children.IndexOf(child));
    }

    protected virtual void OnChildAdded(int index, Control child)
    {
    }

    protected virtual void OnChildRemoved(int index, Control child)
    {
    }
  }
}
