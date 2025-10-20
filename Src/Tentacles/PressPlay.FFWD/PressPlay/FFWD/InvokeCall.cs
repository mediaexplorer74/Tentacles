// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.InvokeCall
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.FFWD
{
  internal struct InvokeCall
  {
    internal string methodName;
    internal float time;
    internal float repeatRate;
    internal MonoBehaviour behaviour;

    public bool Update(float deltaTime)
    {
      this.time -= deltaTime;
      return (double) this.time <= 0.0;
    }
  }
}
