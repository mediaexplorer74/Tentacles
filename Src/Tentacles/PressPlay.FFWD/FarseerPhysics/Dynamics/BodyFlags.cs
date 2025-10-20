// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.BodyFlags
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Dynamics
{
  [Flags]
  public enum BodyFlags
  {
    None = 0,
    Island = 1,
    Awake = 2,
    AutoSleep = 4,
    Bullet = 8,
    FixedRotation = 16, // 0x00000010
    Enabled = 32, // 0x00000020
    IgnoreGravity = 64, // 0x00000040
    IgnoreCCD = 128, // 0x00000080
  }
}
