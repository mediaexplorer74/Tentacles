// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.HitLumpPosition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct HitLumpPosition(
    Transform parent,
    Vector3 localScale,
    Vector3 position,
    Vector3 localPosition,
    Quaternion rotation)
  {
    public Transform parent = parent;
    public Vector3 localScale = localScale;
    public Vector3 position = position;
    public Vector3 localPosition = localPosition;
    public Quaternion rotation = rotation;
  }
}
