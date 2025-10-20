// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Controllers.Controller
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;

#nullable disable
namespace FarseerPhysics.Controllers
{
  public abstract class Controller : FilterData
  {
    public bool Enabled;
    private ControllerType _type;
    public World World;

    public override bool IsActiveOn(Body body)
    {
      return !body.ControllerFilter.IsControllerIgnored(this._type) && base.IsActiveOn(body);
    }

    public Controller(ControllerType controllerType) => this._type = controllerType;

    public abstract void Update(float dt);
  }
}
