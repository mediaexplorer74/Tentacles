// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.DynamicTreeNode`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Collision
{
  internal struct DynamicTreeNode<T>
  {
    internal AABB AABB;
    internal int Child1;
    internal int Child2;
    internal int LeafCount;
    internal int ParentOrNext;
    internal T UserData;

    internal bool IsLeaf() => this.Child1 == -1;
  }
}
