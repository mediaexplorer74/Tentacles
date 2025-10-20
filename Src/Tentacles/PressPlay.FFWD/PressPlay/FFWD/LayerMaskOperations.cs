// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.LayerMaskOperations
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD
{
  public class LayerMaskOperations
  {
    public static bool CheckLayerMaskContainsLayer(LayerMask _mask, int _layer)
    {
      LayerMask _contains = (LayerMask) (1 << _layer);
      return LayerMaskOperations.CheckLayerMaskOverlap(_mask, _contains);
    }

    public static bool CheckLayerMaskOverlap(LayerMask _mask, LayerMask _contains)
    {
      return ((int) _mask & (int) _contains) == _contains.value;
    }
  }
}
