// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.TOIInput
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common;

#nullable disable
namespace FarseerPhysics.Collision
{
  public class TOIInput
  {
    public DistanceProxy ProxyA = new DistanceProxy();
    public DistanceProxy ProxyB = new DistanceProxy();
    public Sweep SweepA;
    public Sweep SweepB;
    public float TMax;
  }
}
