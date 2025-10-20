// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.TriangulationUtil
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Common.Decomposition.CDT;

#nullable disable
namespace Poly2Tri.Triangulation
{
  public class TriangulationUtil
  {
    public static double EPSILON = 1E-12;

    public static bool SmartIncircle(
      TriangulationPoint pa,
      TriangulationPoint pb,
      TriangulationPoint pc,
      TriangulationPoint pd)
    {
      double x = pd.X;
      double y = pd.Y;
      double num1 = pa.X - x;
      double num2 = pa.Y - y;
      double num3 = pb.X - x;
      double num4 = pb.Y - y;
      double num5 = num1 * num4 - num3 * num2;
      if (num5 <= 0.0)
        return false;
      double num6 = pc.X - x;
      double num7 = pc.Y - y;
      double num8 = num6 * num2 - num1 * num7;
      if (num8 <= 0.0)
        return false;
      double num9 = num3 * num7;
      double num10 = num6 * num4;
      double num11 = num1 * num1 + num2 * num2;
      double num12 = num3 * num3 + num4 * num4;
      double num13 = num6 * num6 + num7 * num7;
      return num11 * (num9 - num10) + num12 * num8 + num13 * num5 > 0.0;
    }

    public static bool InScanArea(
      TriangulationPoint pa,
      TriangulationPoint pb,
      TriangulationPoint pc,
      TriangulationPoint pd)
    {
      double x = pd.X;
      double y = pd.Y;
      double num1 = pa.X - x;
      double num2 = pa.Y - y;
      double num3 = pb.X - x;
      double num4 = pb.Y - y;
      if (num1 * num4 - num3 * num2 <= 0.0)
        return false;
      double num5 = pc.X - x;
      double num6 = pc.Y - y;
      return num5 * num2 - num1 * num6 > 0.0;
    }

    public static Orientation Orient2d(
      TriangulationPoint pa,
      TriangulationPoint pb,
      TriangulationPoint pc)
    {
      double num = (pa.X - pc.X) * (pb.Y - pc.Y) - (pa.Y - pc.Y) * (pb.X - pc.X);
      if (num > -TriangulationUtil.EPSILON && num < TriangulationUtil.EPSILON)
        return Orientation.Collinear;
      return num > 0.0 ? Orientation.CCW : Orientation.CW;
    }
  }
}
