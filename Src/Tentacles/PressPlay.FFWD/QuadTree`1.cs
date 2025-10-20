// Decompiled with JetBrains decompiler
// Type: QuadTree`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;
using PressPlay.FFWD.Farseer.Collision;
using System;
using System.Collections.Generic;

#nullable disable
public class QuadTree<T>
{
  public int MaxBucket;
  public int MaxDepth;
  public List<Element<T>> Nodes;
  public AABB Span;
  public QuadTree<T>[] SubTrees;

  public QuadTree(AABB span, int maxbucket, int maxdepth)
  {
    this.Span = span;
    this.Nodes = new List<Element<T>>();
    this.MaxBucket = maxbucket;
    this.MaxDepth = maxdepth;
  }

  public bool IsPartitioned => this.SubTrees != null;

  private int Partition(AABB span, AABB test)
  {
    if (span.Q1.Contains(ref test))
      return 1;
    if (span.Q2.Contains(ref test))
      return 2;
    if (span.Q3.Contains(ref test))
      return 3;
    return span.Q4.Contains(ref test) ? 4 : 0;
  }

  public void AddNode(Element<T> node)
  {
    if (!this.IsPartitioned)
    {
      if (this.Nodes.Count >= this.MaxBucket && this.MaxDepth > 0)
      {
        this.Nodes.Add(node);
        this.SubTrees = new QuadTree<T>[4];
        this.SubTrees[0] = new QuadTree<T>(this.Span.Q1, this.MaxBucket, this.MaxDepth - 1);
        this.SubTrees[1] = new QuadTree<T>(this.Span.Q2, this.MaxBucket, this.MaxDepth - 1);
        this.SubTrees[2] = new QuadTree<T>(this.Span.Q3, this.MaxBucket, this.MaxDepth - 1);
        this.SubTrees[3] = new QuadTree<T>(this.Span.Q4, this.MaxBucket, this.MaxDepth - 1);
        List<Element<T>> elementList = new List<Element<T>>();
        foreach (Element<T> node1 in this.Nodes)
        {
          switch (this.Partition(this.Span, node1.Span))
          {
            case 1:
              this.SubTrees[0].AddNode(node1);
              continue;
            case 2:
              this.SubTrees[1].AddNode(node1);
              continue;
            case 3:
              this.SubTrees[2].AddNode(node1);
              continue;
            case 4:
              this.SubTrees[3].AddNode(node1);
              continue;
            default:
              node1.Parent = this;
              elementList.Add(node1);
              continue;
          }
        }
        this.Nodes = elementList;
      }
      else
      {
        node.Parent = this;
        this.Nodes.Add(node);
      }
    }
    else
    {
      switch (this.Partition(this.Span, node.Span))
      {
        case 1:
          this.SubTrees[0].AddNode(node);
          break;
        case 2:
          this.SubTrees[1].AddNode(node);
          break;
        case 3:
          this.SubTrees[2].AddNode(node);
          break;
        case 4:
          this.SubTrees[3].AddNode(node);
          break;
        default:
          node.Parent = this;
          this.Nodes.Add(node);
          break;
      }
    }
  }

  public static bool RayCastAABB(AABB aabb, Vector2 p1, Vector2 p2)
  {
    AABB b = new AABB();
    Vector2.Min(ref p1, ref p2, out b.LowerBound);
    Vector2.Max(ref p1, ref p2, out b.UpperBound);
    if (!AABB.TestOverlap(aabb, b))
      return false;
    Vector2 vector2_1 = p2 - p1;
    Vector2 vector2_2 = p1;
    Vector2 vector2_3 = new Vector2(-vector2_1.Y, vector2_1.X);
    if ((double) vector2_3.Length() == 0.0)
      return true;
    vector2_3.Normalize();
    float num1 = Vector2.Dot(vector2_2, vector2_3);
    Vector2[] vertices = aabb.GetVertices();
    float num2 = Vector2.Dot(vertices[0], vector2_3) - num1;
    for (int index = 1; index < 4; ++index)
    {
      if (Math.Sign(Vector2.Dot(vertices[index], vector2_3) - num1) != Math.Sign(num2))
        return true;
    }
    return false;
  }

  public void QueryAABB(Func<Element<T>, bool> callback, ref AABB searchR)
  {
    Stack<QuadTree<T>> quadTreeStack = new Stack<QuadTree<T>>();
    quadTreeStack.Push(this);
    while (quadTreeStack.Count > 0)
    {
      QuadTree<T> quadTree = quadTreeStack.Pop();
      if (AABB.TestOverlap(ref searchR, ref quadTree.Span))
      {
        foreach (Element<T> node in quadTree.Nodes)
        {
          if (AABB.TestOverlap(ref searchR, ref node.Span) && !callback(node))
            return;
        }
        if (quadTree.IsPartitioned)
        {
          foreach (QuadTree<T> subTree in quadTree.SubTrees)
            quadTreeStack.Push(subTree);
        }
      }
    }
  }

  public void RayCast(IElementRayCastCallback<T> callback, ref RayCastInput input)
  {
    Stack<QuadTree<T>> quadTreeStack = new Stack<QuadTree<T>>();
    quadTreeStack.Push(this);
    float num1 = input.MaxFraction;
    Vector2 point1 = input.Point1;
    Vector2 p2 = point1 + (input.Point2 - input.Point1) * num1;
    while (quadTreeStack.Count > 0)
    {
      QuadTree<T> quadTree = quadTreeStack.Pop();
      if (QuadTree<T>.RayCastAABB(quadTree.Span, point1, p2))
      {
        foreach (Element<T> node in quadTree.Nodes)
        {
          if (QuadTree<T>.RayCastAABB(node.Span, point1, p2))
          {
            RayCastInput input1;
            input1.Point1 = input.Point1;
            input1.Point2 = input.Point2;
            input1.MaxFraction = num1;
            float num2 = callback.RayCastCallback(ref input1, node);
            if ((double) num2 == 0.0)
              return;
            if ((double) num2 > 0.0)
            {
              num1 = num2;
              p2 = point1 + (input.Point2 - input.Point1) * num1;
            }
          }
        }
        if (this.IsPartitioned)
        {
          foreach (QuadTree<T> subTree in quadTree.SubTrees)
            quadTreeStack.Push(subTree);
        }
      }
    }
  }

  public void GetAllNodesR(ref List<Element<T>> nodes)
  {
    nodes.AddRange((IEnumerable<Element<T>>) this.Nodes);
    if (!this.IsPartitioned)
      return;
    foreach (QuadTree<T> subTree in this.SubTrees)
      subTree.GetAllNodesR(ref nodes);
  }

  public void RemoveNode(Element<T> node) => node.Parent.Nodes.Remove(node);

  public void Reconstruct()
  {
    List<Element<T>> nodes = new List<Element<T>>();
    this.GetAllNodesR(ref nodes);
    this.Clear();
    nodes.ForEach(new Action<Element<T>>(this.AddNode));
  }

  public void Clear()
  {
    this.Nodes.Clear();
    this.SubTrees = (QuadTree<T>[]) null;
  }
}
