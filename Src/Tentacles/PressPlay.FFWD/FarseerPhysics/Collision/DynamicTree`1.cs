// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.DynamicTree`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using PressPlay.FFWD.Farseer.Collision;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Collision
{
  public class DynamicTree<T>
  {
    internal const int NullNode = -1;
    private static Stack<int> _stack = new Stack<int>(256);
    private int _freeList;
    private int _insertionCount;
    private int _nodeCapacity;
    private int _nodeCount;
    private DynamicTreeNode<T>[] _nodes;
    private int _path;
    private int _root;

    public DynamicTree()
    {
      this._root = -1;
      this._nodeCapacity = 16;
      this._nodes = new DynamicTreeNode<T>[this._nodeCapacity];
      for (int index = 0; index < this._nodeCapacity - 1; ++index)
        this._nodes[index].ParentOrNext = index + 1;
      this._nodes[this._nodeCapacity - 1].ParentOrNext = -1;
    }

    public int AddProxy(ref AABB aabb, T userData)
    {
      int leaf = this.AllocateNode();
      Vector2 vector2 = new Vector2(0.1f, 0.1f);
      this._nodes[leaf].AABB.LowerBound = aabb.LowerBound - vector2;
      this._nodes[leaf].AABB.UpperBound = aabb.UpperBound + vector2;
      this._nodes[leaf].UserData = userData;
      this._nodes[leaf].LeafCount = 1;
      this.InsertLeaf(leaf);
      return leaf;
    }

    public void RemoveProxy(int proxyId)
    {
      this.RemoveLeaf(proxyId);
      this.FreeNode(proxyId);
    }

    public bool MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
    {
      if (this._nodes[proxyId].AABB.Contains(ref aabb))
        return false;
      this.RemoveLeaf(proxyId);
      AABB aabb1 = aabb;
      Vector2 vector2_1 = new Vector2(0.1f, 0.1f);
      aabb1.LowerBound -= vector2_1;
      aabb1.UpperBound += vector2_1;
      Vector2 vector2_2 = 2f * displacement;
      if ((double) vector2_2.X < 0.0)
        aabb1.LowerBound.X += vector2_2.X;
      else
        aabb1.UpperBound.X += vector2_2.X;
      if ((double) vector2_2.Y < 0.0)
        aabb1.LowerBound.Y += vector2_2.Y;
      else
        aabb1.UpperBound.Y += vector2_2.Y;
      this._nodes[proxyId].AABB = aabb1;
      this.InsertLeaf(proxyId);
      return true;
    }

    public void Rebalance(int iterations)
    {
      if (this._root == -1)
        return;
      for (int index = 0; index < iterations; ++index)
      {
        int leaf = this._root;
        int num = 0;
        while (!this._nodes[leaf].IsLeaf())
        {
          leaf = (this._path >> num & 1) == 0 ? this._nodes[leaf].Child1 : this._nodes[leaf].Child2;
          num = num + 1 & 31;
        }
        ++this._path;
        this.RemoveLeaf(leaf);
        this.InsertLeaf(leaf);
      }
    }

    public T GetUserData(int proxyId) => this._nodes[proxyId].UserData;

    public void GetFatAABB(int proxyId, out AABB fatAABB) => fatAABB = this._nodes[proxyId].AABB;

    public int ComputeHeight() => this.ComputeHeight(this._root);

    public void Query(Func<int, bool> callback, ref AABB aabb)
    {
      DynamicTree<T>._stack.Clear();
      DynamicTree<T>._stack.Push(this._root);
      while (DynamicTree<T>._stack.Count > 0)
      {
        int index = DynamicTree<T>._stack.Pop();
        if (index != -1)
        {
          DynamicTreeNode<T> node = this._nodes[index];
          if (AABB.TestOverlap(ref node.AABB, ref aabb))
          {
            if (node.IsLeaf())
            {
              if (!callback(index))
                break;
            }
            else
            {
              DynamicTree<T>._stack.Push(node.Child1);
              DynamicTree<T>._stack.Push(node.Child2);
            }
          }
        }
      }
    }

    public void RayCast(IRayCastCallback callback, ref RayCastInput input)
    {
      Vector2 point1 = input.Point1;
      Vector2 point2 = input.Point2;
      Vector2 vector2_1 = point2 - point1;
      vector2_1.Normalize();
      Vector2 vector2_2 = MathUtils.Abs(new Vector2(-vector2_1.Y, vector2_1.X));
      float num1 = input.MaxFraction;
      AABB b = new AABB();
      Vector2 vector2_3 = point1 + num1 * (point2 - point1);
      Vector2.Min(ref point1, ref vector2_3, out b.LowerBound);
      Vector2.Max(ref point1, ref vector2_3, out b.UpperBound);
      DynamicTree<T>._stack.Clear();
      DynamicTree<T>._stack.Push(this._root);
      while (DynamicTree<T>._stack.Count > 0)
      {
        int proxyId = DynamicTree<T>._stack.Pop();
        if (proxyId != -1)
        {
          DynamicTreeNode<T> node = this._nodes[proxyId];
          if (AABB.TestOverlap(ref node.AABB, ref b))
          {
            Vector2 center = node.AABB.Center;
            Vector2 extents = node.AABB.Extents;
            if ((double) (Math.Abs(Vector2.Dot(new Vector2(-vector2_1.Y, vector2_1.X), point1 - center)) - Vector2.Dot(vector2_2, extents)) <= 0.0)
            {
              if (node.IsLeaf())
              {
                RayCastInput input1;
                input1.Point1 = input.Point1;
                input1.Point2 = input.Point2;
                input1.MaxFraction = num1;
                float num2 = callback.RayCastCallback(ref input1, proxyId);
                if ((double) num2 == 0.0)
                  break;
                if ((double) num2 > 0.0)
                {
                  num1 = num2;
                  Vector2 vector2_4 = point1 + num1 * (point2 - point1);
                  b.LowerBound = Vector2.Min(point1, vector2_4);
                  b.UpperBound = Vector2.Max(point1, vector2_4);
                }
              }
              else
              {
                DynamicTree<T>._stack.Push(node.Child1);
                DynamicTree<T>._stack.Push(node.Child2);
              }
            }
          }
        }
      }
    }

    private int CountLeaves(int nodeId)
    {
      if (nodeId == -1)
        return 0;
      DynamicTreeNode<T> node = this._nodes[nodeId];
      return node.IsLeaf() ? 1 : this.CountLeaves(node.Child1) + this.CountLeaves(node.Child2);
    }

    private void Validate() => this.CountLeaves(this._root);

    private int AllocateNode()
    {
      if (this._freeList == -1)
      {
        DynamicTreeNode<T>[] nodes = this._nodes;
        this._nodeCapacity *= 2;
        this._nodes = new DynamicTreeNode<T>[this._nodeCapacity];
        Array.Copy((Array) nodes, (Array) this._nodes, this._nodeCount);
        for (int nodeCount = this._nodeCount; nodeCount < this._nodeCapacity - 1; ++nodeCount)
          this._nodes[nodeCount].ParentOrNext = nodeCount + 1;
        this._nodes[this._nodeCapacity - 1].ParentOrNext = -1;
        this._freeList = this._nodeCount;
      }
      int freeList = this._freeList;
      this._freeList = this._nodes[freeList].ParentOrNext;
      this._nodes[freeList].ParentOrNext = -1;
      this._nodes[freeList].Child1 = -1;
      this._nodes[freeList].Child2 = -1;
      this._nodes[freeList].LeafCount = 0;
      ++this._nodeCount;
      return freeList;
    }

    private void FreeNode(int nodeId)
    {
      this._nodes[nodeId].ParentOrNext = this._freeList;
      this._freeList = nodeId;
      --this._nodeCount;
    }

    private void InsertLeaf(int leaf)
    {
      ++this._insertionCount;
      if (this._root == -1)
      {
        this._root = leaf;
        this._nodes[this._root].ParentOrNext = -1;
      }
      else
      {
        AABB aabb1 = this._nodes[leaf].AABB;
        int index1;
        int child1;
        int child2;
        float num1;
        float num2;
        for (index1 = this._root; !this._nodes[index1].IsLeaf(); index1 = (double) num1 >= (double) num2 ? child2 : child1)
        {
          child1 = this._nodes[index1].Child1;
          child2 = this._nodes[index1].Child2;
          this._nodes[index1].AABB.Combine(ref aabb1);
          ++this._nodes[index1].LeafCount;
          float perimeter1 = this._nodes[index1].AABB.Perimeter;
          AABB aabb2 = new AABB();
          aabb2.Combine(ref this._nodes[index1].AABB, ref aabb1);
          float perimeter2 = aabb2.Perimeter;
          float num3 = 2f * perimeter2;
          float num4 = (float) (2.0 * ((double) perimeter2 - (double) perimeter1));
          if (this._nodes[child1].IsLeaf())
          {
            AABB aabb3 = new AABB();
            aabb3.Combine(ref aabb1, ref this._nodes[child1].AABB);
            num1 = aabb3.Perimeter + num4;
          }
          else
          {
            AABB aabb4 = new AABB();
            aabb4.Combine(ref aabb1, ref this._nodes[child1].AABB);
            float perimeter3 = this._nodes[child1].AABB.Perimeter;
            num1 = aabb4.Perimeter - perimeter3 + num4;
          }
          if (this._nodes[child2].IsLeaf())
          {
            AABB aabb5 = new AABB();
            aabb5.Combine(ref aabb1, ref this._nodes[child2].AABB);
            num2 = aabb5.Perimeter + num4;
          }
          else
          {
            AABB aabb6 = new AABB();
            aabb6.Combine(ref aabb1, ref this._nodes[child2].AABB);
            float perimeter4 = this._nodes[child2].AABB.Perimeter;
            num2 = aabb6.Perimeter - perimeter4 + num4;
          }
          if ((double) num3 >= (double) num1 || (double) num3 >= (double) num2)
            this._nodes[index1].AABB.Combine(ref aabb1);
          else
            break;
        }
        int parentOrNext = this._nodes[index1].ParentOrNext;
        int index2 = this.AllocateNode();
        this._nodes[index2].ParentOrNext = parentOrNext;
        this._nodes[index2].UserData = default (T);
        this._nodes[index2].AABB.Combine(ref aabb1, ref this._nodes[index1].AABB);
        this._nodes[index2].LeafCount = this._nodes[index1].LeafCount + 1;
        if (parentOrNext != -1)
        {
          if (this._nodes[parentOrNext].Child1 == index1)
            this._nodes[parentOrNext].Child1 = index2;
          else
            this._nodes[parentOrNext].Child2 = index2;
          this._nodes[index2].Child1 = index1;
          this._nodes[index2].Child2 = leaf;
          this._nodes[index1].ParentOrNext = index2;
          this._nodes[leaf].ParentOrNext = index2;
        }
        else
        {
          this._nodes[index2].Child1 = index1;
          this._nodes[index2].Child2 = leaf;
          this._nodes[index1].ParentOrNext = index2;
          this._nodes[leaf].ParentOrNext = index2;
          this._root = index2;
        }
      }
    }

    private void RemoveLeaf(int leaf)
    {
      if (leaf == this._root)
      {
        this._root = -1;
      }
      else
      {
        int parentOrNext1 = this._nodes[leaf].ParentOrNext;
        int parentOrNext2 = this._nodes[parentOrNext1].ParentOrNext;
        int index1 = this._nodes[parentOrNext1].Child1 != leaf ? this._nodes[parentOrNext1].Child1 : this._nodes[parentOrNext1].Child2;
        if (parentOrNext2 != -1)
        {
          if (this._nodes[parentOrNext2].Child1 == parentOrNext1)
            this._nodes[parentOrNext2].Child1 = index1;
          else
            this._nodes[parentOrNext2].Child2 = index1;
          this._nodes[index1].ParentOrNext = parentOrNext2;
          this.FreeNode(parentOrNext1);
          for (int index2 = parentOrNext2; index2 != -1; index2 = this._nodes[index2].ParentOrNext)
          {
            this._nodes[index2].AABB.Combine(ref this._nodes[this._nodes[index2].Child1].AABB, ref this._nodes[this._nodes[index2].Child2].AABB);
            --this._nodes[index2].LeafCount;
          }
        }
        else
        {
          this._root = index1;
          this._nodes[index1].ParentOrNext = -1;
          this.FreeNode(parentOrNext1);
        }
      }
    }

    private int ComputeHeight(int nodeId)
    {
      if (nodeId == -1)
        return 0;
      DynamicTreeNode<T> node = this._nodes[nodeId];
      return 1 + Math.Max(this.ComputeHeight(node.Child1), this.ComputeHeight(node.Child2));
    }
  }
}
