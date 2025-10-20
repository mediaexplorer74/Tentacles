// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.Animation
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class Animation : Behaviour, IFixedUpdateable
  {
    public bool playAutomatically;
    [ContentSerializer]
    private string[] animations;
    private Dictionary<string, AnimationClip> clips = new Dictionary<string, AnimationClip>();
    private Dictionary<string, AnimationState> states = new Dictionary<string, AnimationState>();
    private string animationIndex;
    private SkinnedAnimationPlayer animationPlayer;

    [ContentSerializerIgnore]
    public AnimationClip clip
    {
      get => this.clips.Count == 0 ? (AnimationClip) null : this.clips[this.animationIndex];
    }

    public override void Awake()
    {
      SkinnedMeshRenderer componentInChildren = this.GetComponentInChildren<SkinnedMeshRenderer>();
      if (componentInChildren == null || componentInChildren.sharedMesh == null || componentInChildren.sharedMesh.skinnedModel == null || componentInChildren.sharedMesh.skinnedModel.SkinningData.AnimationClips == null)
        return;
      this.Initialize(componentInChildren.sharedMesh.skinnedModel.SkinningData, componentInChildren.sharedMesh.skinnedModel.BakedTransform);
    }

    public AnimationState this[string index]
    {
      get => this.states.ContainsKey(index) ? this.states[index] : (AnimationState) null;
    }

    public AnimationClip GetClip(string name) => this.clips[name];

    public bool isPlaying
    {
      get => true;
      private set
      {
      }
    }

    public void Rewind() => throw new NotImplementedException("Method not implemented.");

    public void Play() => this.Play(this.animations[0]);

    public void Play(string name)
    {
      if (this.animationPlayer == null)
        return;
      this.animationPlayer.StartClip(this.clips[name], this.states[name]);
    }

    public void PlayQueued(string name) => this.PlayQueued(name, QueueMode.CompleteOthers);

    public void PlayQueued(string name, QueueMode mode)
    {
    }

    public void Stop()
    {
      foreach (AnimationState animationState in this.states.Values)
        animationState.enabled = false;
    }

    public void Stop(string name)
    {
      if (!this.states.ContainsKey(name))
        return;
      this.states[name].enabled = false;
    }

    public void AddClip(AnimationClip clip, string newName)
    {
      if (string.IsNullOrEmpty(newName))
        return;
      clip.name = newName;
      this.clips[newName] = clip;
      this.states[newName] = new AnimationState()
      {
        length = (float) clip.Duration.TotalSeconds,
        wrapMode = clip.wrapMode
      };
      if (!string.IsNullOrEmpty(this.animationIndex))
        return;
      this.animationIndex = newName;
    }

    public void AddClip(AnimationClip clip, string newName, int firstFrame, int lastFrame)
    {
      this.AddClip(new AnimationClip(clip, newName, firstFrame, lastFrame), newName);
    }

    public void AddClip(
      AnimationClip clip,
      string newName,
      int firstFrame,
      int lastFrame,
      bool addLoopFrame)
    {
      this.AddClip(clip, newName, firstFrame, lastFrame);
    }

    public void Blend(string name) => this.Blend(name, 1f, 1f);

    public void Blend(string name, float weight) => this.Blend(name, weight, 1f);

    public void Blend(string name, float weight, float length)
    {
      this.Stop();
      this.Play(name);
    }

    public void CrossFade(string name) => this.CrossFade(name, 0.3f);

    public void CrossFade(string name, float fadeLength)
    {
      this.Stop();
      this.Play(name);
    }

    internal void Initialize(SkinningData modelData, Matrix bakedTransform)
    {
      foreach (string key in modelData.AnimationClips.Keys)
        this.AddClip(modelData.AnimationClips[key], key);
      this.animationPlayer = new SkinnedAnimationPlayer(modelData, bakedTransform);
      this.animationPlayer.SetTransforms(this.transform.GetComponentsInChildren<Transform>());
      if (!this.playAutomatically)
        return;
      this.animationPlayer.StartClip(this.clip, this.states[this.clip.name]);
    }

    public void FixedUpdate()
    {
      if (this.animationPlayer == null)
        return;
      this.animationPlayer.FixedUpdate();
    }

    internal Matrix[] GetTransforms()
    {
      return this.animationPlayer != null ? this.animationPlayer.SkinTransforms : (Matrix[]) null;
    }

    internal override void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      base.AfterLoad(idMap);
      foreach (string animation in this.animations)
        this.AddClip(new AnimationClip(new TimeSpan(), (List<Keyframe>) null), animation);
    }
  }
}
