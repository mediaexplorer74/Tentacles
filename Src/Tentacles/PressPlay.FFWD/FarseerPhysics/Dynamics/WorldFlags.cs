// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.WorldFlags
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  [Flags]
  public enum WorldFlags
  {
    NewFixture = 1,
    ClearForces = 4,
    SubStepping = 16, // 0x00000010
  }
}
