// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.IBroadPhase
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
  public interface IBroadPhase
  {
    void UpdatePairs(BroadphaseDelegate callback);

    bool TestOverlap(int proxyIdA, int proxyIdB);

    int AddProxy(ref FixtureProxy proxy);

    void RemoveProxy(int proxyId);

    void MoveProxy(int proxyId, ref AABB aabb, Vector2 displacement);

    FixtureProxy GetProxy(int proxyId);

    void TouchProxy(int proxyId);

    int ProxyCount { get; }

    void GetFatAABB(int proxyId, out AABB aabb);

    void Query(Func<int, bool> callback, ref AABB aabb);

    void RayCast(IRayCastCallback raycastCallback, ref RayCastInput input);
  }
}
