// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.Contacts.ContactFlags
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Dynamics.Contacts
{
  [Flags]
  public enum ContactFlags
  {
    None = 0,
    Island = 1,
    Touching = 2,
    Enabled = 4,
    Filter = 8,
    BulletHit = 16, // 0x00000010
    TOI = 32, // 0x00000020
  }
}
