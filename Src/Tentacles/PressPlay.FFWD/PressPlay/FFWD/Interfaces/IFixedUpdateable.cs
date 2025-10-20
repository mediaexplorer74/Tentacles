// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Interfaces.IFixedUpdateable
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace PressPlay.FFWD.Interfaces
{
  public interface IFixedUpdateable
  {
    GameObject gameObject { get; }

    void FixedUpdate();
  }
}
