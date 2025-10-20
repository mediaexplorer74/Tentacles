// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Keyframe
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public class Keyframe
  {
    [ContentSerializer]
    public int Bone { get; private set; }

    [ContentSerializer]
    public float Time { get; private set; }

    [ContentSerializer]
    public Matrix Transform { get; private set; }

    public Keyframe(int bone, TimeSpan time, Matrix transform)
    {
      this.Bone = bone;
      this.Time = (float) time.TotalSeconds;
      this.Transform = transform;
    }

    private Keyframe()
    {
    }
  }
}
