// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinningData
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class SkinningData
  {
    [ContentSerializer]
    public Dictionary<string, AnimationClip> AnimationClips { get; private set; }

    [ContentSerializer]
    public List<Matrix> BindPose { get; private set; }

    [ContentSerializer]
    public List<Matrix> InverseBindPose { get; private set; }

    [ContentSerializer]
    public List<int> SkeletonHierarchy { get; private set; }

    [ContentSerializer]
    public Dictionary<string, int> BoneMap { get; private set; }

    public SkinningData(
      Dictionary<string, AnimationClip> animationClips,
      List<Matrix> bindPose,
      List<Matrix> inverseBindPose,
      List<int> skeletonHierarchy,
      Dictionary<string, int> boneMap)
    {
      this.AnimationClips = animationClips;
      this.BindPose = bindPose;
      this.InverseBindPose = inverseBindPose;
      this.SkeletonHierarchy = skeletonHierarchy;
      this.BoneMap = boneMap;
    }

    private SkinningData()
    {
    }
  }
}
