// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.Pair
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Collision
{
  internal struct Pair : IComparable<Pair>
  {
    public int ProxyIdA;
    public int ProxyIdB;

    public int CompareTo(Pair other)
    {
      if (this.ProxyIdA < other.ProxyIdA)
        return -1;
      if (this.ProxyIdA == other.ProxyIdA)
      {
        if (this.ProxyIdB < other.ProxyIdB)
          return -1;
        if (this.ProxyIdB == other.ProxyIdB)
          return 0;
      }
      return 1;
    }
  }
}
