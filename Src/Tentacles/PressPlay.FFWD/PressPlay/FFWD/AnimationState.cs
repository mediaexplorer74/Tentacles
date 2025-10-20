// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.AnimationState
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public class AnimationState
  {
    public bool enabled;
    public float time;
    public float speed = 1f;
    public float length;
    public WrapMode wrapMode;

    public float normalizedTime
    {
      get => (double) this.length == 0.0 ? 0.0f : this.time / this.length;
      set => this.time = Mathf.Clamp01(value) * this.length;
    }
  }
}
