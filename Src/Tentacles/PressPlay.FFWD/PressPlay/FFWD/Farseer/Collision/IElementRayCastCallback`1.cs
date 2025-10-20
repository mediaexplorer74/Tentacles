// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Farseer.Collision.IElementRayCastCallback`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Collision;

#nullable disable
namespace PressPlay.FFWD.Farseer.Collision
{
  public interface IElementRayCastCallback<T>
  {
    float RayCastCallback(ref RayCastInput input, Element<T> proxyId);
  }
}
