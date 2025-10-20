// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ComponentProfiler
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  internal class ComponentProfiler
  {
    private List<ComponentUpdateProfile> componentUpdateProfiles = new List<ComponentUpdateProfile>();
    private ComponentUpdateProfile currentUpdateProfile;

    private ComponentUpdateProfile GetComponentProfileFromList(Component _component)
    {
      for (int index = 0; index < this.componentUpdateProfiles.Count; ++index)
      {
        if (this.componentUpdateProfiles[index].component.GetType() == _component.GetType())
          return this.componentUpdateProfiles[index];
      }
      ComponentUpdateProfile componentProfileFromList = new ComponentUpdateProfile(_component);
      this.componentUpdateProfiles.Add(componentProfileFromList);
      return componentProfileFromList;
    }

    public void StartUpdateCall(Component _component)
    {
      this.currentUpdateProfile = this.GetComponentProfileFromList(_component);
      ++this.currentUpdateProfile.updateCalls;
      this.currentUpdateProfile.updateStopwatch.Start();
    }

    public void StartFixedUpdateCall(Component _component)
    {
      this.currentUpdateProfile = this.GetComponentProfileFromList(_component);
      ++this.currentUpdateProfile.fixedUpdateCalls;
      this.currentUpdateProfile.updateStopwatch.Start();
    }

    public void StartLateUpdateCall(Component _component)
    {
      this.currentUpdateProfile = this.GetComponentProfileFromList(_component);
      ++this.currentUpdateProfile.lateUpdateCalls;
      this.currentUpdateProfile.updateStopwatch.Start();
    }

    public void EndUpdateCall() => this.currentUpdateProfile.updateStopwatch.Stop();

    public void EndFixedUpdateCall() => this.currentUpdateProfile.updateStopwatch.Stop();

    public void EndLateUpdateCall() => this.currentUpdateProfile.updateStopwatch.Stop();

    public void FlushData() => this.componentUpdateProfiles.Clear();

    public List<ComponentUpdateProfile> Sort()
    {
      this.componentUpdateProfiles.Sort();
      return this.componentUpdateProfiles;
    }

    public ComponentUpdateProfile GetWorst()
    {
      ComponentUpdateProfile componentUpdateProfile = new ComponentUpdateProfile((Component) null);
      return this.componentUpdateProfiles.Count == 0 ? new ComponentUpdateProfile() : this.componentUpdateProfiles[0];
    }
  }
}
