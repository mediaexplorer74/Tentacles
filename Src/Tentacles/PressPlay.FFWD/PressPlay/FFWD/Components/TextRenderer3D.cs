// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.TextRenderer3D
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.FFWD.Components
{
  public class TextRenderer3D : Renderer
  {
    public TextRenderer3D.RenderMethod renderMethod;
    private string _text;
    private SpriteFont _font;
    public static SpriteBatch batch;
    public static BasicEffect basicEffect;
    public static Matrix invertY = Matrix.CreateScale(1f, -1f, 1f);
    private PressPlay.FFWD.Vector3 cameraFront = new PressPlay.FFWD.Vector3(0.0f, 0.0f, -1f);
    public float textSize = 1f;

    public string text
    {
      get => this._text;
      set => this._text = value;
    }

    public SpriteFont font
    {
      get => this._font;
      set => this._font = value;
    }

    public TextRenderer3D(SpriteFont font)
      : this("", font)
    {
    }

    public TextRenderer3D(string text, SpriteFont font)
    {
      this.font = font;
      this.text = text;
      this.material = new Material();
      this.material.color = PressPlay.FFWD.Color.white;
    }

    public override int Draw(GraphicsDevice device, Camera cam)
    {
      TextRenderer3D.basicEffect.Projection = cam.projectionMatrix;
      PressPlay.FFWD.Vector3 vector3 = (PressPlay.FFWD.Vector3) Microsoft.Xna.Framework.Vector3.Transform((Microsoft.Xna.Framework.Vector3) this.transform.position, cam.view * TextRenderer3D.invertY);
      PressPlay.FFWD.Vector2 origin = (PressPlay.FFWD.Vector2) (this.font.MeasureString(this.text) / 2f);
      TextRenderer3D.batch.DrawString(this.font, this.text, (Microsoft.Xna.Framework.Vector2) new PressPlay.FFWD.Vector2(vector3.x, vector3.y), (Microsoft.Xna.Framework.Color) this.material.color, 0.0f, (Microsoft.Xna.Framework.Vector2) origin, 0.03f * this.textSize, SpriteEffects.None, vector3.z);
      return 0;
    }

    public enum RenderMethod
    {
      normal,
      billboard,
    }
  }
}
