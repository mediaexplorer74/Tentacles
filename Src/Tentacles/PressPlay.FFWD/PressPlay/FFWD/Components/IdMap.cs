// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Components.IdMap
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace PressPlay.FFWD.Components
{
  internal class IdMap : Component
  {
    public string fieldName;
    public Dictionary<int, int> map;

    public void UpdateIdReferences(Dictionary<int, UnityObject> idMap)
    {
      foreach (KeyValuePair<int, int> keyValuePair in this.map)
      {
        UnityObject id1 = idMap[keyValuePair.Key];
        MemberInfo[] member = id1.GetType().GetMember(this.fieldName);
        if (member.Length > 0)
        {
          UnityObject id2 = idMap[keyValuePair.Value];
          (member[0] as FieldInfo).SetValue((object) id1, (object) id2);
        }
      }
    }
  }
}
