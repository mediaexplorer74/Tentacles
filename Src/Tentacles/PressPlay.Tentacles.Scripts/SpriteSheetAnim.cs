// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SpriteSheetAnim
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SpriteSheetAnim
  {
    public int startFrameIndex;
    public int endFrameIndex;
    public float timePerFrame;
    public string name;
    public SpriteSheetAnim.WrapMode wrapMode;

    public float length => this.timePerFrame * (float) (this.endFrameIndex - this.startFrameIndex);

    public enum WrapMode
    {
      clamp,
      loop,
    }
  }
}
