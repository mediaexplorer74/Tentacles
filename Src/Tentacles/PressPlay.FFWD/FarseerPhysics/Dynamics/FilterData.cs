// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Dynamics.FilterData
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

#nullable disable
namespace FarseerPhysics.Dynamics
{
  public abstract class FilterData
  {
    public Category DisabledOnCategories;
    public int DisabledOnGroup;
    public Category EnabledOnCategories = Category.All;
    public int EnabledOnGroup;

    public virtual bool IsActiveOn(Body body)
    {
      if (body == null || !body.Enabled || body.IsStatic || body.FixtureList == null)
        return false;
      foreach (Fixture fixture in body.FixtureList)
      {
        if ((int) fixture.CollisionGroup == this.DisabledOnGroup && fixture.CollisionGroup != (short) 0 && this.DisabledOnGroup != 0 || (fixture.CollisionCategories & this.DisabledOnCategories) != Category.None)
          return false;
        if (this.EnabledOnGroup == 0 && this.EnabledOnCategories == Category.All || (int) fixture.CollisionGroup == this.EnabledOnGroup && fixture.CollisionGroup != (short) 0 && this.EnabledOnGroup != 0 || (fixture.CollisionCategories & this.EnabledOnCategories) != Category.None && this.EnabledOnCategories != Category.All)
          return true;
      }
      return false;
    }

    public void AddDisabledCategory(Category category) => this.DisabledOnCategories |= category;

    public void RemoveDisabledCategory(Category category) => this.DisabledOnCategories &= ~category;

    public bool IsInDisabledCategory(Category category)
    {
      return (this.DisabledOnCategories & category) == category;
    }

    public void AddEnabledCategory(Category category) => this.EnabledOnCategories |= category;

    public void RemoveEnabledCategory(Category category) => this.EnabledOnCategories &= ~category;

    public bool IsInEnabledCategory(Category category)
    {
      return (this.EnabledOnCategories & category) == category;
    }
  }
}
