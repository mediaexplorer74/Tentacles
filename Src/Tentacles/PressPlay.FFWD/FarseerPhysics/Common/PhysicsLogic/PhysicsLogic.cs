// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PhysicsLogic.PhysicsLogic
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using FarseerPhysics.Dynamics;

#nullable disable
namespace FarseerPhysics.Common.PhysicsLogic
{
  public abstract class PhysicsLogic : FilterData
  {
    private PhysicsLogicType _type;
    public World World;

    public override bool IsActiveOn(Body body)
    {
      return !body.PhysicsLogicFilter.IsPhysicsLogicIgnored(this._type) && base.IsActiveOn(body);
    }

    public PhysicsLogic(World world, PhysicsLogicType type)
    {
      this._type = type;
      this.World = world;
    }
  }
}
