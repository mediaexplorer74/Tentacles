// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PPAnimationClip
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class PPAnimationClip
  {
    public PPAnimationClip()
    {
      this.speed = 1f;
      this.randomizeStartTime = false;
      this.wrapMode = PressPlay.FFWD.WrapMode.Default;
    }

    public string id { get; set; }

    public int firstFrame { get; set; }

    public int lastFrame { get; set; }

    public bool addLoopFrame { get; set; }

    public float speed { get; set; }

    public bool randomizeStartTime { get; set; }

    public PressPlay.FFWD.WrapMode wrapMode { get; set; }
  }
}
