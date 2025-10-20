// Decompiled with JetBrains decompiler
// Type: QuadTreeBroadPhase
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using PressPlay.FFWD.Farseer.Collision;
using System;
using System.Collections.Generic;

#nullable disable
public class QuadTreeBroadPhase : IBroadPhase
{
  private const int TreeUpdateThresh = 10000;
  private int _currID;
  private Dictionary<int, Element<FixtureProxy>> _idRegister;
  private List<Element<FixtureProxy>> _moveBuffer;
  private List<Pair> _pairBuffer;
  private QuadTree<FixtureProxy> _quadTree;
  private int _treeMoveNum;

  public QuadTreeBroadPhase(AABB span)
  {
    this._quadTree = new QuadTree<FixtureProxy>(span, 5, 10);
    this._idRegister = new Dictionary<int, Element<FixtureProxy>>();
    this._moveBuffer = new List<Element<FixtureProxy>>();
    this._pairBuffer = new List<Pair>();
  }

  public int ProxyCount => this._idRegister.Count;

  public void GetFatAABB(int proxyID, out AABB aabb)
  {
    if (!this._idRegister.ContainsKey(proxyID))
      throw new KeyNotFoundException("proxyID not found in register");
    aabb = this._idRegister[proxyID].Span;
  }

  public void UpdatePairs(BroadphaseDelegate callback)
  {
    this._pairBuffer.Clear();
    using (List<Element<FixtureProxy>>.Enumerator enumerator = this._moveBuffer.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        Element<FixtureProxy> qtnode = enumerator.Current;
        this.Query((Func<int, bool>) (proxyID => this.PairBufferQueryCallback(proxyID, qtnode.Value.ProxyId)), ref qtnode.Span);
      }
    }
    this._moveBuffer.Clear();
    this._pairBuffer.Sort();
    int index = 0;
label_10:
    while (index < this._pairBuffer.Count)
    {
      Pair pair = this._pairBuffer[index];
      FixtureProxy proxy1 = this.GetProxy(pair.ProxyIdA);
      FixtureProxy proxy2 = this.GetProxy(pair.ProxyIdB);
      callback(ref proxy1, ref proxy2);
      ++index;
      while (true)
      {
        if (index < this._pairBuffer.Count && this._pairBuffer[index].ProxyIdA == pair.ProxyIdA && this._pairBuffer[index].ProxyIdB == pair.ProxyIdB)
          ++index;
        else
          goto label_10;
      }
    }
  }

  public bool TestOverlap(int proxyIdA, int proxyIdB)
  {
    AABB aabb1;
    this.GetFatAABB(proxyIdA, out aabb1);
    AABB aabb2;
    this.GetFatAABB(proxyIdB, out aabb2);
    return AABB.TestOverlap(ref aabb1, ref aabb2);
  }

  public int AddProxy(ref FixtureProxy proxy)
  {
    int key = this._currID++;
    proxy.ProxyId = key;
    AABB span = this.Fatten(ref proxy.AABB);
    Element<FixtureProxy> node = new Element<FixtureProxy>(proxy, span);
    this._idRegister.Add(key, node);
    this._quadTree.AddNode(node);
    return key;
  }

  public void RemoveProxy(int proxyId)
  {
    Element<FixtureProxy> element = this._idRegister.ContainsKey(proxyId) ? this._idRegister[proxyId] : throw new KeyNotFoundException("proxyID not found in register");
    this.UnbufferMove(element);
    this._idRegister.Remove(proxyId);
    this._quadTree.RemoveNode(element);
  }

  public void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
  {
    AABB aabb1;
    this.GetFatAABB(proxyId, out aabb1);
    if (aabb1.Contains(ref aabb))
      return;
    AABB aabb2 = aabb;
    Vector2 vector2_1 = new Vector2(0.1f, 0.1f);
    aabb2.LowerBound -= vector2_1;
    aabb2.UpperBound += vector2_1;
    Vector2 vector2_2 = 2f * displacement;
    if ((double) vector2_2.X < 0.0)
      aabb2.LowerBound.X += vector2_2.X;
    else
      aabb2.UpperBound.X += vector2_2.X;
    if ((double) vector2_2.Y < 0.0)
      aabb2.LowerBound.Y += vector2_2.Y;
    else
      aabb2.UpperBound.Y += vector2_2.Y;
    Element<FixtureProxy> element = this._idRegister[proxyId];
    element.Value.AABB = aabb2;
    element.Span = aabb2;
    this.ReinsertNode(element);
    this.BufferMove(element);
  }

  public FixtureProxy GetProxy(int proxyId)
  {
    if (this._idRegister.ContainsKey(proxyId))
      return this._idRegister[proxyId].Value;
    throw new KeyNotFoundException("proxyID not found in register");
  }

  public void TouchProxy(int proxyId)
  {
    if (!this._idRegister.ContainsKey(proxyId))
      throw new KeyNotFoundException("proxyID not found in register");
    this.BufferMove(this._idRegister[proxyId]);
  }

  public void Query(Func<int, bool> callback, ref AABB query)
  {
    this._quadTree.QueryAABB(this.TransformPredicate(callback), ref query);
  }

  public void RayCast(IRayCastCallback callback, ref RayCastInput input)
  {
    this._quadTree.RayCast((IElementRayCastCallback<FixtureProxy>) new QuadTreeBroadPhase.ElementRayCastCallbackHelper()
    {
      callback = callback
    }, ref input);
  }

  private AABB Fatten(ref AABB aabb)
  {
    Vector2 vector2 = new Vector2(0.1f, 0.1f);
    return new AABB(aabb.LowerBound - vector2, aabb.UpperBound + vector2);
  }

  private Func<Element<FixtureProxy>, bool> TransformPredicate(Func<int, bool> idPredicate)
  {
    return (Func<Element<FixtureProxy>, bool>) (qtnode => idPredicate(qtnode.Value.ProxyId));
  }

  private bool PairBufferQueryCallback(int proxyID, int baseID)
  {
    if (proxyID == baseID)
      return true;
    this._pairBuffer.Add(new Pair()
    {
      ProxyIdA = Math.Min(proxyID, baseID),
      ProxyIdB = Math.Max(proxyID, baseID)
    });
    return true;
  }

  private void ReconstructTree()
  {
    this._quadTree.Clear();
    foreach (Element<FixtureProxy> node in this._idRegister.Values)
      this._quadTree.AddNode(node);
  }

  private void ReinsertNode(Element<FixtureProxy> qtnode)
  {
    this._quadTree.RemoveNode(qtnode);
    this._quadTree.AddNode(qtnode);
    if (++this._treeMoveNum <= 10000)
      return;
    this.ReconstructTree();
    this._treeMoveNum = 0;
  }

  private void BufferMove(Element<FixtureProxy> proxy) => this._moveBuffer.Add(proxy);

  private void UnbufferMove(Element<FixtureProxy> proxy) => this._moveBuffer.Remove(proxy);

  private struct ElementRayCastCallbackHelper : IElementRayCastCallback<FixtureProxy>
  {
    internal IRayCastCallback callback;

    public float RayCastCallback(ref RayCastInput input, Element<FixtureProxy> proxyId)
    {
      return this.callback.RayCastCallback(ref input, proxyId.Value.ProxyId);
    }
  }
}
