// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.SkinnedAnimationPlayer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.FFWD
{
  public class SkinnedAnimationPlayer
  {
    private AnimationClip currentClipValue;
    private AnimationState currentStateValue;
    private float currentTimeValue;
    private int currentKeyframe;
    private Matrix[] boneTransforms;
    private Matrix[] worldTransforms;
    private Matrix[] skinTransforms;
    private Matrix bakedTransform;
    private Transform[] transforms;
    private SkinningData skinningDataValue;

    public Matrix[] BoneTransforms => this.boneTransforms;

    public Matrix[] WorldTransforms => this.worldTransforms;

    public Matrix[] SkinTransforms => this.skinTransforms;

    public SkinnedAnimationPlayer(SkinningData skinningData, Matrix bakedTransform)
    {
      this.skinningDataValue = skinningData != null ? skinningData : throw new ArgumentNullException(nameof (skinningData));
      this.boneTransforms = new Matrix[skinningData.BindPose.Count];
      this.worldTransforms = new Matrix[skinningData.BindPose.Count];
      this.skinTransforms = new Matrix[skinningData.BindPose.Count];
      this.transforms = new Transform[this.worldTransforms.Length];
      this.bakedTransform = bakedTransform;
    }

    public void StartClip(AnimationClip clip, AnimationState state)
    {
      if (clip == null)
        throw new ArgumentNullException(nameof (clip));
      if (this.currentClipValue == clip)
        return;
      this.currentClipValue = clip;
      this.currentStateValue = state;
      state.time = 0.0f;
      state.enabled = true;
      this.currentTimeValue = state.time;
      this.currentKeyframe = 0;
      this.ResetAnimation();
    }

    private void ResetAnimation()
    {
      if (this.currentClipValue.Keyframes.Count > 0)
      {
        float time = this.currentClipValue.Keyframes[0].Time;
        for (int index = 0; index < this.currentClipValue.Keyframes.Count; ++index)
        {
          Keyframe keyframe = this.currentClipValue.Keyframes[index];
          if ((double) keyframe.Time != (double) time)
            break;
          this.boneTransforms[keyframe.Bone] = keyframe.Transform;
        }
      }
      else
        this.skinningDataValue.BindPose.CopyTo(this.boneTransforms, 0);
    }

    public void FixedUpdate()
    {
      this.UpdateBoneTransforms(Time.deltaTime * this.currentStateValue.speed);
      this.UpdateWorldTransforms(this.bakedTransform);
      this.UpdateSkinTransforms();
    }

    public void UpdateBoneTransforms(float time)
    {
      if (this.currentClipValue == null)
        throw new InvalidOperationException("AnimationPlayer.Update was called before StartClip");
      if (!this.currentStateValue.enabled)
        return;
      this.currentTimeValue = this.currentStateValue.time;
      time += this.currentTimeValue;
      if ((double) time > (double) this.currentStateValue.length)
      {
        switch (this.currentStateValue.wrapMode)
        {
          case WrapMode.Once:
          case WrapMode.Clamp:
            time = this.currentStateValue.length;
            break;
          case WrapMode.Loop:
            time = 0.0f;
            break;
          case WrapMode.PingPong:
            this.currentStateValue.speed *= -1f;
            break;
          case WrapMode.Default:
            break;
          default:
            throw new NotImplementedException("What to do here?");
        }
      }
      if ((double) time < (double) this.currentTimeValue)
      {
        this.currentKeyframe = 0;
        this.ResetAnimation();
      }
      this.currentStateValue.time = time;
      this.currentTimeValue = time + this.currentClipValue.timeOffset;
      for (int count = this.currentClipValue.Keyframes.Count; this.currentKeyframe < count; ++this.currentKeyframe)
      {
        Keyframe keyframe = this.currentClipValue.Keyframes[this.currentKeyframe];
        if ((double) keyframe.Time > (double) this.currentTimeValue)
          break;
        this.boneTransforms[keyframe.Bone] = keyframe.Transform;
      }
    }

    public void UpdateWorldTransforms(Matrix rootTransform)
    {
      this.worldTransforms[0] = this.boneTransforms[0] * rootTransform;
      for (int index1 = 1; index1 < this.worldTransforms.Length; ++index1)
      {
        int index2 = this.skinningDataValue.SkeletonHierarchy[index1];
        this.worldTransforms[index1] = this.boneTransforms[index1] * this.worldTransforms[index2];
        if (this.transforms[index1] != null)
          this.transforms[index1].localPosition = (Vector3) (Matrix.Invert(this.worldTransforms[index1]).Translation * 0.01f);
      }
    }

    public void UpdateSkinTransforms()
    {
      for (int index = 0; index < this.skinTransforms.Length; ++index)
        this.skinTransforms[index] = this.skinningDataValue.InverseBindPose[index] * this.worldTransforms[index];
    }

    internal void SetTransforms(Transform[] transform)
    {
      for (int index = 0; index < transform.Length; ++index)
      {
        if (this.skinningDataValue.BoneMap.ContainsKey(transform[index].name))
          this.transforms[this.skinningDataValue.BoneMap[transform[index].name]] = transform[index];
      }
    }
  }
}
