// Decompiled with JetBrains decompiler
// Type: Element`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;

#nullable disable
public class Element<T>
{
  public QuadTree<T> Parent;
  public AABB Span;
  public T Value;

  public Element(T value, AABB span)
  {
    this.Span = span;
    this.Value = value;
    this.Parent = (QuadTree<T>) null;
  }
}
