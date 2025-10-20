// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.SimplexCache
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;

#nullable disable
namespace FarseerPhysics.Collision
{
  public struct SimplexCache
  {
    public ushort Count;
    public FixedArray3<byte> IndexA;
    public FixedArray3<byte> IndexB;
    public float Metric;
  }
}
