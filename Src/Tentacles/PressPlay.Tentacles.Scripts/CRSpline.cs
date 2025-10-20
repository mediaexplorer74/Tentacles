// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CRSpline
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CRSpline
  {
    public Vector3[] pts;

    public CRSpline(params Vector3[] pts)
    {
      this.pts = new Vector3[pts.Length];
      Array.Copy((Array) pts, (Array) this.pts, pts.Length);
    }

    public Vector3 Interp(float t)
    {
      int num1 = this.pts.Length - 3;
      int index = Mathf.Min(Mathf.FloorToInt(t * (float) num1), num1 - 1);
      float num2 = t * (float) num1 - (float) index;
      Vector3 pt1 = this.pts[index];
      Vector3 pt2 = this.pts[index + 1];
      Vector3 pt3 = this.pts[index + 2];
      Vector3 pt4 = this.pts[index + 3];
      return 0.5f * ((-pt1 + 3f * pt2 - 3f * pt3 + pt4) * (num2 * num2 * num2) + (2f * pt1 - 5f * pt2 + 4f * pt3 - pt4) * (num2 * num2) + (-pt1 + pt3) * num2 + 2f * pt2);
    }
  }
}
