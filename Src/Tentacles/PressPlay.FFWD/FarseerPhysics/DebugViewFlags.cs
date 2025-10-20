// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.DebugViewFlags
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics
{
  [Flags]
  public enum DebugViewFlags
  {
    Shape = 1,
    Joint = 2,
    AABB = 4,
    Pair = 8,
    CenterOfMass = 16, // 0x00000010
    DebugPanel = 32, // 0x00000020
    ContactPoints = 64, // 0x00000040
    ContactNormals = 128, // 0x00000080
    PolygonPoints = 256, // 0x00000100
    PerformanceGraph = 512, // 0x00000200
    Controllers = 1024, // 0x00000400
  }
}
