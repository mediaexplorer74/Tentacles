// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.CachedContent
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class CachedContent
  {
    public ContentManager content;
    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    public static Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

    public CachedContent(ContentManager content) => this.content = content;

    public Texture2D LoadInstantTexture(string source)
    {
      this.LoadTexture(source);
      return this.GetTexture(source);
    }

    public void LoadTexture(string source)
    {
      if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(source) || this.textures.ContainsKey(source))
        return;
      this.textures.Add(source, this.content.Load<Texture2D>(source));
    }

    public Texture2D GetTexture(string source)
    {
      if (string.IsNullOrEmpty(source))
        return (Texture2D) null;
      Texture2D texture;
      this.textures.TryGetValue(source, out texture);
      return texture;
    }

    public SoundEffect LoadInstantSound(string source)
    {
      this.LoadSound(source);
      return this.GetSound(source);
    }

    public void LoadSound(string source)
    {
      if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(source) || CachedContent.sounds.ContainsKey(source))
        return;
      CachedContent.sounds.Add(source, this.content.Load<SoundEffect>(source));
    }

    public SoundEffect GetSound(string source)
    {
      if (string.IsNullOrEmpty(source))
        return (SoundEffect) null;
      SoundEffect sound;
      CachedContent.sounds.TryGetValue(source, out sound);
      return sound;
    }
  }
}
