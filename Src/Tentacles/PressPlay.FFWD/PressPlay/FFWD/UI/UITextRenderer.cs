// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.UITextRenderer
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Text;

#nullable disable
namespace PressPlay.FFWD.UI
{
  public class UITextRenderer : UIRenderer
  {
    public SpriteFont font;
    private PressPlay.FFWD.Vector2 renderPosition = PressPlay.FFWD.Vector2.zero;
    private PressPlay.FFWD.Vector2 textSize = PressPlay.FFWD.Vector2.zero;
    public TextControl.TextOrigin textOrigin;
    public PressPlay.FFWD.Vector2 textOffset = PressPlay.FFWD.Vector2.zero;
    public PressPlay.FFWD.Color color = PressPlay.FFWD.Color.white;
    public SpriteEffects effects;
    private bool hasDoneWordWrap = true;
    private string _text = "";
    protected static char[] splitTokens = new char[2]
    {
      ' ',
      '-'
    };
    protected static string spaceString = " ";

    public string text
    {
      get
      {
        if (!this.hasDoneWordWrap && this.control != null && ((TextControl) this.control).useWordWrap)
        {
          this._text = this.WordWrap(this._text, this.control.bounds.Width, this.font);
          this.hasDoneWordWrap = true;
        }
        return this._text;
      }
      set
      {
        if (!(value != this._text))
          return;
        value = value.Replace("”", "");
        if (!(this._text != value))
          return;
        if (this.font != null)
        {
          this.textSize = (PressPlay.FFWD.Vector2) this.font.MeasureString(this._text);
          if (this.control != null && ((TextControl) this.control).useWordWrap)
          {
            this._text = this.WordWrap(value, this.control.bounds.Width, this.font);
          }
          else
          {
            this._text = value;
            this.hasDoneWordWrap = false;
          }
        }
        else
          this._text = value;
      }
    }

    public UITextRenderer(SpriteFont font)
      : this("", font)
    {
    }

    public UITextRenderer(string text, SpriteFont font)
    {
      this.font = font;
      this.text = text;
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      if (this.font == null)
        return 0;
      float layerDepth = (float) (1.0 - (double) (float) this.transform.position / 10000.0);
      Camera.spriteBatch.DrawString(this.font, this.text, (Microsoft.Xna.Framework.Vector2) this.transform.position, (Microsoft.Xna.Framework.Color) this.material.color, this.transform.rotation.eulerAngles.y, this.GetOrigin(), (Microsoft.Xna.Framework.Vector2) this.transform.lossyScale, this.effects, layerDepth);
      return 0;
    }

    protected Microsoft.Xna.Framework.Vector2 GetOrigin()
    {
      return this.textOrigin == TextControl.TextOrigin.center ? new Microsoft.Xna.Framework.Vector2((float) (this.control.bounds.Width / 2), 0.0f) : Microsoft.Xna.Framework.Vector2.Zero;
    }

    protected string WordWrap(string input, int width, SpriteFont font)
    {
      if (font == null)
        return input;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Length = 0;
      string[] strArray = input.Split(UITextRenderer.splitTokens, StringSplitOptions.None);
      int num1 = (int) ((double) font.MeasureString(UITextRenderer.spaceString).X * (double) this.transform.lossyScale.x);
      int num2 = 0;
      int num3 = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        int num4 = (int) ((double) font.MeasureString(strArray[index]).X * (double) this.transform.lossyScale.x);
        if (num3 > 0 && num4 + num2 > width)
        {
          stringBuilder.Append(Environment.NewLine);
          num2 = 0;
          num3 = 0;
        }
        stringBuilder.Append(strArray[index]);
        stringBuilder.Append(UITextRenderer.spaceString);
        num2 += num4 + num1;
        ++num3;
      }
      return stringBuilder.ToString();
    }
  }
}
