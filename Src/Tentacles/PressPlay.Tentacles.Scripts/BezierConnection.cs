// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BezierConnection
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct BezierConnection
  {
    public float previousPathLength;
    public float endConnectionPathLength;
    public float length;
    public float bezierFractionStart;
    public float bezierFractionLength;
    public Vector3 previousPoint;
    public Vector3 nextPoint;
  }
}
