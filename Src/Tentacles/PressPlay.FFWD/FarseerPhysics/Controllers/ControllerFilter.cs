// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.ControllerFilter
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Controllers
{
  public struct ControllerFilter
  {
    public ControllerType ControllerFlags;

    public void IgnoreController(ControllerType controller) => this.ControllerFlags |= controller;

    public void RestoreController(ControllerType controller) => this.ControllerFlags &= ~controller;

    public bool IsControllerIgnored(ControllerType controller)
    {
      return (this.ControllerFlags & controller) == controller;
    }
  }
}
