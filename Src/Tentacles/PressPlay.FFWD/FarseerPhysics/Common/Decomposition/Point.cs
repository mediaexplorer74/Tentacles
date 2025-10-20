// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.Decomposition.Point
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.Decomposition
{
  internal class Point
  {
    public Point Next;
    public Point Prev;
    public float X;
    public float Y;

    public Point(float x, float y)
    {
      this.X = x;
      this.Y = y;
      this.Next = (Point) null;
      this.Prev = (Point) null;
    }

    public static Point operator -(Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

    public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

    public static Point operator -(Point p1, float f) => new Point(p1.X - f, p1.Y - f);

    public static Point operator +(Point p1, float f) => new Point(p1.X + f, p1.Y + f);

    public float Cross(Point p)
    {
      return (float) ((double) this.X * (double) p.Y - (double) this.Y * (double) p.X);
    }

    public float Dot(Point p)
    {
      return (float) ((double) this.X * (double) p.X + (double) this.Y * (double) p.Y);
    }

    public bool Neq(Point p) => (double) p.X != (double) this.X || (double) p.Y != (double) this.Y;

    public float Orient2D(Point pb, Point pc)
    {
      float num1 = this.X - pc.X;
      float num2 = pb.X - pc.X;
      float num3 = this.Y - pc.Y;
      float num4 = pb.Y - pc.Y;
      return (float) ((double) num1 * (double) num4 - (double) num3 * (double) num2);
    }
  }
}
