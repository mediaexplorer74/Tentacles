// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PhysicsLogic.RayDataComparer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common.PhysicsLogic
{
  internal class RayDataComparer : IComparer<float>
  {
    int IComparer<float>.Compare(float a, float b)
    {
      float num = a - b;
      if ((double) num > 0.0)
        return 1;
      return (double) num < 0.0 ? -1 : 0;
    }
  }
}
