// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Color
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;
using System.Globalization;

#nullable disable
namespace PressPlay.FFWD
{
  public struct Color(float r, float g, float b, float a)
  {
    public float r = r;
    public float g = g;
    public float b = b;
    public float a = a;

    public byte R
    {
      get => (byte) ((double) this.r * (double) byte.MaxValue);
      set => this.r = (float) value / (float) byte.MaxValue;
    }

    public byte G
    {
      get => (byte) ((double) this.g * (double) byte.MaxValue);
      set => this.g = (float) value / (float) byte.MaxValue;
    }

    public byte B
    {
      get => (byte) ((double) this.b * (double) byte.MaxValue);
      set => this.b = (float) value / (float) byte.MaxValue;
    }

    public byte A
    {
      get => (byte) ((double) this.a * (double) byte.MaxValue);
      set => this.a = (float) value / (float) byte.MaxValue;
    }

    public float greyscale => throw new NotImplementedException("Not implemented");

    static Color()
    {
      _red = new Color(1f, 0.0f, 0.0f, 1f);
      _green = new Color(0.0f, 1f, 0.0f, 1f);
      _blue = new Color(0.0f, 0.0f, 1f, 1f);
      _white = new Color(1f, 1f, 1f, 1f);
      _black = new Color(0.0f, 0.0f, 0.0f, 1f);
      _yellow = new Color(1f, 0.0f, 0.0f, 1f);
      _cyan = new Color(0.0f, 1f, 1f, 1f);
      _magenta = new Color(1f, 0.0f, 1f, 1f);
      _gray = new Color(0.5f, 0.5f, 0.5f, 1f);
      _grey = new Color(0.5f, 0.5f, 0.5f, 1f);
      _clear = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }

    public static Color red => Color._red;

    public static Color green => Color._green;

    public static Color blue => Color._blue;

    public static Color white => Color._white;

    public static Color black => Color._black;

    public static Color yellow => Color._yellow;

    public static Color cyan => Color._cyan;

    public static Color magenta => Color._magenta;

    public static Color gray => Color._gray;

    public static Color grey => Color._grey;

    public static Color clear => Color._clear;

    public static Color operator *(float d, Color c)
    {
      return new Color(c.r * d, c.g * d, c.b * d, c.a * d);
    }

    public static Color operator *(Color c, float d)
    {
      return new Color(c.r * d, c.g * d, c.b * d, c.a * d);
    }

    public static implicit operator Color(Vector4 v) => new Color(v.X, v.Y, v.Z, v.W);

    public static implicit operator Microsoft.Xna.Framework.Color(Color c)
    {
      return Microsoft.Xna.Framework.Color.FromNonPremultiplied(new Vector4(c.r, c.g, c.b, c.a));
    }

    public static implicit operator Color(Microsoft.Xna.Framework.Color c) => (Color) c.ToVector4();

    public static implicit operator Vector3(Color c) => new Vector3(c.r, c.g, c.b);

    public static Color Lerp(Color a, Color b, float t)
    {
      return new Color(MathHelper.Lerp(a.r, b.r, t), MathHelper.Lerp(a.g, b.g, t), MathHelper.Lerp(a.b, b.b, t), MathHelper.Lerp(a.a, b.a, t));
    }

    public static Color Parse(string s)
    {
      Color color = new Color();
      if (s.Length == 8)
      {
        color.a = Color.ParseHexData(s, 0);
        s = s.Substring(2);
      }
      color.r = Color.ParseHexData(s, 0);
      color.g = Color.ParseHexData(s, 2);
      color.b = Color.ParseHexData(s, 4);
      return color;
    }

    private static float ParseHexData(string s, int start)
    {
      return (float) int.Parse(s.Substring(start, 2), NumberStyles.HexNumber) / (float) byte.MaxValue;
    }
  }
}
