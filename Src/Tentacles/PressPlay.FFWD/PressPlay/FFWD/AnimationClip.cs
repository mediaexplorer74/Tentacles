// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.AnimationClip
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class AnimationClip
  {
    public string name;
    public WrapMode wrapMode;
    internal float timeOffset;

    internal AnimationClip(AnimationClip clip, string newName, int firstFrame, int lastFrame)
    {
      this.name = newName;
      if (firstFrame < 0)
        firstFrame = 0;
      float num1 = (float) firstFrame * 0.0333333351f;
      float num2 = (float) lastFrame * 0.0333333351f;
      if (clip.Keyframes != null)
      {
        this.Keyframes = new List<Keyframe>();
        this.timeOffset = num1;
        int count = clip.Keyframes.Count;
        for (int index = 0; index < count; ++index)
        {
          Keyframe keyframe = clip.Keyframes[index];
          float time = keyframe.Time;
          if ((double) time >= (double) num1 && (double) time < (double) num2)
            this.Keyframes.Add(keyframe);
        }
      }
      this.Duration = TimeSpan.FromSeconds((double) num2 - (double) num1);
    }

    [ContentSerializerIgnore]
    public float length => (float) this.Duration.TotalSeconds;

    [ContentSerializer]
    public TimeSpan Duration { get; private set; }

    [ContentSerializer]
    public List<Keyframe> Keyframes { get; private set; }

    public AnimationClip(TimeSpan duration, List<Keyframe> keyframes)
    {
      this.Duration = duration;
      this.Keyframes = keyframes;
    }

    private AnimationClip()
    {
    }
  }
}
