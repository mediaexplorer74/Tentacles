// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.LayerMask
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public struct LayerMask
  {
    public int value { get; set; }

    public static implicit operator int(LayerMask mask) => mask.value;

    public static implicit operator LayerMask(int mask)
    {
      return new LayerMask() { value = mask };
    }

    public override string ToString() => this.value.ToString();
  }
}
