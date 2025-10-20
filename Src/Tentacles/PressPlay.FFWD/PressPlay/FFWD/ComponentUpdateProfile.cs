// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ComponentUpdateProfile
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Diagnostics;

#nullable disable
namespace PressPlay.FFWD
{
  internal struct ComponentUpdateProfile(Component component) : IComparable<ComponentUpdateProfile>
  {
    public Component component = component;
    public int updateCalls = 0;
    public int lateUpdateCalls = 0;
    public int fixedUpdateCalls = 0;
    public Stopwatch updateStopwatch = new Stopwatch();

    public string name => this.component == null ? (string) null : this.component.name;

    public float totalMilliseconds => (float) (this.totalTicks / Stopwatch.Frequency) * 1000f;

    public long totalTicks => this.updateStopwatch == null ? 0L : this.updateStopwatch.ElapsedTicks;

    public void Flush()
    {
      this.updateCalls = 0;
      this.lateUpdateCalls = 0;
      this.fixedUpdateCalls = 0;
      this.updateStopwatch.Reset();
    }

    public int CompareTo(ComponentUpdateProfile other)
    {
      return (int) other.totalTicks - (int) this.totalTicks;
    }

    public override string ToString()
    {
      if (this.name == null)
        return "null " + this.totalMilliseconds.ToString();
      return this.name + " " + this.totalMilliseconds.ToString() + " ms  " + (object) this.totalTicks + " ticks";
    }
  }
}
