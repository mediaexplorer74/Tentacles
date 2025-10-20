// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PolygonTools
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public static class PolygonTools
  {
    public static Vertices CreateRectangle(float hx, float hy)
    {
      Vertices rectangle = new Vertices(4);
      rectangle.Add(new Vector2(-hx, -hy));
      rectangle.Add(new Vector2(hx, -hy));
      rectangle.Add(new Vector2(hx, hy));
      rectangle.Add(new Vector2(-hx, hy));
      return rectangle;
    }

    public static Vertices CreateRectangle(float hx, float hy, Vector2 center, float angle)
    {
      Vertices rectangle = PolygonTools.CreateRectangle(hx, hy);
      Transform T = new Transform();
      T.Position = center;
      T.R.Set(angle);
      for (int index = 0; index < 4; ++index)
        rectangle[index] = MathUtils.Multiply(ref T, rectangle[index]);
      return rectangle;
    }

    public static Vertices CreateRoundedRectangle(
      float width,
      float height,
      float xRadius,
      float yRadius,
      int segments)
    {
      if ((double) yRadius > (double) height / 2.0 || (double) xRadius > (double) width / 2.0)
        throw new Exception("Rounding amount can't be more than half the height and width respectively.");
      if (segments < 0)
        throw new Exception("Segments must be zero or more.");
      Vertices roundedRectangle = new Vertices();
      if (segments == 0)
      {
        roundedRectangle.Add(new Vector2(width * 0.5f - xRadius, (float) (-(double) height * 0.5)));
        roundedRectangle.Add(new Vector2(width * 0.5f, (float) (-(double) height * 0.5) + yRadius));
        roundedRectangle.Add(new Vector2(width * 0.5f, height * 0.5f - yRadius));
        roundedRectangle.Add(new Vector2(width * 0.5f - xRadius, height * 0.5f));
        roundedRectangle.Add(new Vector2((float) (-(double) width * 0.5) + xRadius, height * 0.5f));
        roundedRectangle.Add(new Vector2((float) (-(double) width * 0.5), height * 0.5f - yRadius));
        roundedRectangle.Add(new Vector2((float) (-(double) width * 0.5), (float) (-(double) height * 0.5) + yRadius));
        roundedRectangle.Add(new Vector2((float) (-(double) width * 0.5) + xRadius, (float) (-(double) height * 0.5)));
      }
      else
      {
        int num1 = segments * 4 + 8;
        float num2 = 6.28318548f / (float) (num1 - 4);
        int num3 = num1 / 4;
        Vector2 vector2 = new Vector2(width / 2f - xRadius, height / 2f - yRadius);
        roundedRectangle.Add(vector2 + new Vector2(xRadius, -yRadius + yRadius));
        short num4 = 0;
        for (int index = 1; index < num1; ++index)
        {
          if (index - num3 == 0 || index - num3 * 3 == 0)
          {
            vector2.X *= -1f;
            --num4;
          }
          else if (index - num3 * 2 == 0)
          {
            vector2.Y *= -1f;
            --num4;
          }
          roundedRectangle.Add(vector2 + new Vector2(xRadius * (float) Math.Cos((double) num2 * (double) -(index + (int) num4)), -yRadius * (float) Math.Sin((double) num2 * (double) -(index + (int) num4))));
        }
      }
      return roundedRectangle;
    }

    public static Vertices CreateLine(Vector2 start, Vector2 end)
    {
      Vertices line = new Vertices(2);
      line.Add(start);
      line.Add(end);
      return line;
    }

    public static Vertices CreateCircle(float radius, int numberOfEdges)
    {
      return PolygonTools.CreateEllipse(radius, radius, numberOfEdges);
    }

    public static Vertices CreateEllipse(float xRadius, float yRadius, int numberOfEdges)
    {
      Vertices ellipse = new Vertices();
      float num = 6.28318548f / (float) numberOfEdges;
      ellipse.Add(new Vector2(xRadius, 0.0f));
      for (int index = numberOfEdges - 1; index > 0; --index)
        ellipse.Add(new Vector2(xRadius * (float) Math.Cos((double) num * (double) index), -yRadius * (float) Math.Sin((double) num * (double) index)));
      return ellipse;
    }

    public static Vertices CreateArc(float radians, int sides, float radius)
    {
      Vertices arc = new Vertices();
      float num = radians / (float) sides;
      for (int index = sides - 1; index > 0; --index)
        arc.Add(new Vector2(radius * (float) Math.Cos((double) num * (double) index), radius * (float) Math.Sin((double) num * (double) index)));
      return arc;
    }

    public static Vertices CreateCapsule(float height, float endRadius, int edges)
    {
      if ((double) endRadius >= (double) height / 2.0)
        throw new ArgumentException("The radius must be lower than height / 2. Higher values of radius would create a circle, and not a half circle.", nameof (endRadius));
      return PolygonTools.CreateCapsule(height, endRadius, edges, endRadius, edges);
    }

    public static Vertices CreateCapsule(
      float height,
      float topRadius,
      int topEdges,
      float bottomRadius,
      int bottomEdges)
    {
      if ((double) height <= 0.0)
        throw new ArgumentException("Height must be longer than 0", nameof (height));
      if ((double) topRadius <= 0.0)
        throw new ArgumentException("The top radius must be more than 0", nameof (topRadius));
      if (topEdges <= 0)
        throw new ArgumentException("Top edges must be more than 0", nameof (topEdges));
      if ((double) bottomRadius <= 0.0)
        throw new ArgumentException("The bottom radius must be more than 0", nameof (bottomRadius));
      if (bottomEdges <= 0)
        throw new ArgumentException("Bottom edges must be more than 0", nameof (bottomEdges));
      if ((double) topRadius >= (double) height / 2.0)
        throw new ArgumentException("The top radius must be lower than height / 2. Higher values of top radius would create a circle, and not a half circle.", nameof (topRadius));
      if ((double) bottomRadius >= (double) height / 2.0)
        throw new ArgumentException("The bottom radius must be lower than height / 2. Higher values of bottom radius would create a circle, and not a half circle.", nameof (bottomRadius));
      Vertices capsule = new Vertices();
      float y = (float) (((double) height - (double) topRadius - (double) bottomRadius) * 0.5);
      capsule.Add(new Vector2(topRadius, y));
      float num1 = 3.14159274f / (float) topEdges;
      for (int index = 1; index < topEdges; ++index)
        capsule.Add(new Vector2(topRadius * (float) Math.Cos((double) num1 * (double) index), topRadius * (float) Math.Sin((double) num1 * (double) index) + y));
      capsule.Add(new Vector2(-topRadius, y));
      capsule.Add(new Vector2(-bottomRadius, -y));
      float num2 = 3.14159274f / (float) bottomEdges;
      for (int index = 1; index < bottomEdges; ++index)
        capsule.Add(new Vector2(-bottomRadius * (float) Math.Cos((double) num2 * (double) index), -bottomRadius * (float) Math.Sin((double) num2 * (double) index) - y));
      capsule.Add(new Vector2(bottomRadius, -y));
      return capsule;
    }

    public static Vertices CreateGear(
      float radius,
      int numberOfTeeth,
      float tipPercentage,
      float toothHeight)
    {
      Vertices gear = new Vertices();
      float num1 = 6.28318548f / (float) numberOfTeeth;
      tipPercentage /= 100f;
      double num2 = (double) MathHelper.Clamp(tipPercentage, 0.0f, 1f);
      float num3 = num1 / 2f * tipPercentage;
      float num4 = (float) (((double) num1 - (double) num3 * 2.0) / 2.0);
      for (int index = numberOfTeeth - 1; index >= 0; --index)
      {
        if ((double) num3 > 0.0)
        {
          gear.Add(new Vector2(radius * (float) Math.Cos((double) num1 * (double) index + (double) num4 * 2.0 + (double) num3), -radius * (float) Math.Sin((double) num1 * (double) index + (double) num4 * 2.0 + (double) num3)));
          gear.Add(new Vector2((radius + toothHeight) * (float) Math.Cos((double) num1 * (double) index + (double) num4 + (double) num3), (float) -((double) radius + (double) toothHeight) * (float) Math.Sin((double) num1 * (double) index + (double) num4 + (double) num3)));
        }
        gear.Add(new Vector2((radius + toothHeight) * (float) Math.Cos((double) num1 * (double) index + (double) num4), (float) -((double) radius + (double) toothHeight) * (float) Math.Sin((double) num1 * (double) index + (double) num4)));
        gear.Add(new Vector2(radius * (float) Math.Cos((double) num1 * (double) index), -radius * (float) Math.Sin((double) num1 * (double) index)));
      }
      return gear;
    }

    public static Vertices CreatePolygon(uint[] data, int width)
    {
      return TextureConverter.DetectVertices(data, width);
    }

    public static Vertices CreatePolygon(uint[] data, int width, bool holeDetection)
    {
      return TextureConverter.DetectVertices(data, width, holeDetection);
    }

    public static List<Vertices> CreatePolygon(
      uint[] data,
      int width,
      float hullTolerance,
      byte alphaTolerance,
      bool multiPartDetection,
      bool holeDetection)
    {
      return TextureConverter.DetectVertices(data, width, hullTolerance, alphaTolerance, multiPartDetection, holeDetection);
    }
  }
}
