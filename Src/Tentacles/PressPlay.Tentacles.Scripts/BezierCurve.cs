// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.BezierCurve
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class BezierCurve
  {
    private CRSpline spline;

    public BezierCurve(Vector3[] path)
    {
      if (path.Length == 0)
        return;
      this.spline = new CRSpline(BezierCurve.PathControlPointGenerator(path));
    }

    public void ResetPath(Transform[] path)
    {
      Vector3[] path1 = new Vector3[path.Length];
      for (int index = 0; index < path.Length; ++index)
        path1[index] = path[index].position;
      this.ResetPath(path1);
    }

    public void ResetPath(Vector3[] path)
    {
      if (path.Length == 0)
        this.spline = (CRSpline) null;
      else
        this.spline = new CRSpline(BezierCurve.PathControlPointGenerator(path));
    }

    public Vector3 PointOnPath(float t) => this.spline.Interp(t);

    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
      Vector3[] sourceArray = path;
      int num = 2;
      Vector3[] vector3Array1 = new Vector3[sourceArray.Length + num];
      Array.Copy((Array) sourceArray, 0, (Array) vector3Array1, 1, sourceArray.Length);
      vector3Array1[0] = vector3Array1[1] + (vector3Array1[1] - vector3Array1[2]);
      vector3Array1[vector3Array1.Length - 1] = vector3Array1[vector3Array1.Length - 2] + (vector3Array1[vector3Array1.Length - 2] - vector3Array1[vector3Array1.Length - 3]);
      if (vector3Array1[1] == vector3Array1[vector3Array1.Length - 2])
      {
        Vector3[] vector3Array2 = new Vector3[vector3Array1.Length];
        Array.Copy((Array) vector3Array1, (Array) vector3Array2, vector3Array1.Length);
        vector3Array2[0] = vector3Array2[vector3Array2.Length - 3];
        vector3Array2[vector3Array2.Length - 1] = vector3Array2[2];
        vector3Array1 = new Vector3[vector3Array2.Length];
        Array.Copy((Array) vector3Array2, (Array) vector3Array1, vector3Array2.Length);
      }
      return vector3Array1;
    }
  }
}
