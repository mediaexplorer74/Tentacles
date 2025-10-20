// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SimpleRenderObject
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public abstract class SimpleRenderObject
  {
    public Microsoft.Xna.Framework.Vector2 position = Microsoft.Xna.Framework.Vector2.Zero;
    public PressPlay.FFWD.Color color = PressPlay.FFWD.Color.white;
    public float alpha = 1f;
    public float rotation;
    public SpriteEffects effects;

    public abstract void Draw(SpriteBatch batch);
  }
}
