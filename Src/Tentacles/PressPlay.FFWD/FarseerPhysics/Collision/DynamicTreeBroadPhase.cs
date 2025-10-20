// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.DynamicTreeBroadPhase
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using PressPlay.FFWD.Farseer.Collision;
using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  public class DynamicTreeBroadPhase : IBroadPhase
  {
    private int[] _moveBuffer;
    private int _moveCapacity;
    private int _moveCount;
    private Pair[] _pairBuffer;
    private int _pairCapacity;
    private int _pairCount;
    private int _proxyCount;
    private Func<int, bool> _queryCallback;
    private int _queryProxyId;
    private DynamicTree<FixtureProxy> _tree = new DynamicTree<FixtureProxy>();

    public DynamicTreeBroadPhase()
    {
      this._queryCallback = new Func<int, bool>(this.QueryCallback);
      this._pairCapacity = 16;
      this._pairBuffer = new Pair[this._pairCapacity];
      this._moveCapacity = 16;
      this._moveBuffer = new int[this._moveCapacity];
    }

    public int ProxyCount => this._proxyCount;

    public int AddProxy(ref FixtureProxy proxy)
    {
      int proxyId = this._tree.AddProxy(ref proxy.AABB, proxy);
      ++this._proxyCount;
      this.BufferMove(proxyId);
      return proxyId;
    }

    public void RemoveProxy(int proxyId)
    {
      this.UnBufferMove(proxyId);
      --this._proxyCount;
      this._tree.RemoveProxy(proxyId);
    }

    public void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement)
    {
      if (!this._tree.MoveProxy(proxyId, ref aabb, displacement))
        return;
      this.BufferMove(proxyId);
    }

    public void GetFatAABB(int proxyId, out AABB aabb) => this._tree.GetFatAABB(proxyId, out aabb);

    public FixtureProxy GetProxy(int proxyId) => this._tree.GetUserData(proxyId);

    public bool TestOverlap(int proxyIdA, int proxyIdB)
    {
      AABB fatAABB1;
      this._tree.GetFatAABB(proxyIdA, out fatAABB1);
      AABB fatAABB2;
      this._tree.GetFatAABB(proxyIdB, out fatAABB2);
      return AABB.TestOverlap(ref fatAABB1, ref fatAABB2);
    }

    public void UpdatePairs(BroadphaseDelegate callback)
    {
      this._pairCount = 0;
      for (int index = 0; index < this._moveCount; ++index)
      {
        this._queryProxyId = this._moveBuffer[index];
        if (this._queryProxyId != -1)
        {
          AABB fatAABB;
          this._tree.GetFatAABB(this._queryProxyId, out fatAABB);
          this._tree.Query(this._queryCallback, ref fatAABB);
        }
      }
      this._moveCount = 0;
      Array.Sort<Pair>(this._pairBuffer, 0, this._pairCount);
      int index1 = 0;
      while (index1 < this._pairCount)
      {
        Pair pair1 = this._pairBuffer[index1];
        FixtureProxy userData1 = this._tree.GetUserData(pair1.ProxyIdA);
        FixtureProxy userData2 = this._tree.GetUserData(pair1.ProxyIdB);
        callback(ref userData1, ref userData2);
        for (++index1; index1 < this._pairCount; ++index1)
        {
          Pair pair2 = this._pairBuffer[index1];
          if (pair2.ProxyIdA != pair1.ProxyIdA || pair2.ProxyIdB != pair1.ProxyIdB)
            break;
        }
      }
      this._tree.Rebalance(4);
    }

    public void Query(Func<int, bool> callback, ref AABB aabb)
    {
      this._tree.Query(callback, ref aabb);
    }

    public void RayCast(IRayCastCallback callback, ref RayCastInput input)
    {
      this._tree.RayCast(callback, ref input);
    }

    public int ComputeHeight() => this._tree.ComputeHeight();

    private void BufferMove(int proxyId)
    {
      if (this._moveCount == this._moveCapacity)
      {
        int[] moveBuffer = this._moveBuffer;
        this._moveCapacity *= 2;
        this._moveBuffer = new int[this._moveCapacity];
        Array.Copy((Array) moveBuffer, (Array) this._moveBuffer, this._moveCount);
      }
      this._moveBuffer[this._moveCount] = proxyId;
      ++this._moveCount;
    }

    private void UnBufferMove(int proxyId)
    {
      for (int index = 0; index < this._moveCount; ++index)
      {
        if (this._moveBuffer[index] == proxyId)
        {
          this._moveBuffer[index] = -1;
          break;
        }
      }
    }

    private bool QueryCallback(int proxyId)
    {
      if (proxyId == this._queryProxyId)
        return true;
      if (this._pairCount == this._pairCapacity)
      {
        Pair[] pairBuffer = this._pairBuffer;
        this._pairCapacity *= 2;
        this._pairBuffer = new Pair[this._pairCapacity];
        Array.Copy((Array) pairBuffer, (Array) this._pairBuffer, this._pairCount);
      }
      this._pairBuffer[this._pairCount].ProxyIdA = Math.Min(proxyId, this._queryProxyId);
      this._pairBuffer[this._pairCount].ProxyIdB = Math.Max(proxyId, this._queryProxyId);
      ++this._pairCount;
      return true;
    }

    public void TouchProxy(int proxyId) => this.BufferMove(proxyId);
  }
}
