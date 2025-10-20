// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.PhysicsLogic.PhysicsLogicFilter
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Common.PhysicsLogic
{
  public struct PhysicsLogicFilter
  {
    public PhysicsLogicType ControllerIgnores;

    public void IgnorePhysicsLogic(PhysicsLogicType type) => this.ControllerIgnores |= type;

    public void RestorePhysicsLogic(PhysicsLogicType type) => this.ControllerIgnores &= ~type;

    public bool IsPhysicsLogicIgnored(PhysicsLogicType type)
    {
      return (this.ControllerIgnores & type) == type;
    }
  }
}
