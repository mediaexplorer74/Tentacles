// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Collision.ContactID
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Runtime.InteropServices;

#nullable disable
namespace FarseerPhysics.Collision
{
  [StructLayout(LayoutKind.Explicit)]
  public struct ContactID
  {
    [FieldOffset(0)]
    public ContactFeature Features;
    [FieldOffset(0)]
    public uint Key;
  }
}
