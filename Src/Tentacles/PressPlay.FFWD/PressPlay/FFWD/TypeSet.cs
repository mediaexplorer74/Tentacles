// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.TypeSet
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PressPlay.FFWD
{
  public class TypeSet : ICollection<string>, IEnumerable<string>, IEnumerable
  {
    private Dictionary<string, short> dict;

    public TypeSet() => this.dict = new Dictionary<string, short>();

    public TypeSet(int capacity) => this.dict = new Dictionary<string, short>(capacity);

    public void Add(Type tp) => this.Add(tp.Name);

    public void Add(string item)
    {
      if (this.dict.ContainsKey(item))
        return;
      this.dict.Add(item, (short) 0);
    }

    public void Clear() => this.dict.Clear();

    public bool Contains(Type tp) => this.Contains(tp.Name);

    public bool Contains(string item) => this.dict.ContainsKey(item);

    public void CopyTo(string[] array, int arrayIndex) => throw new NotImplementedException();

    public bool Remove(string item) => this.dict.Remove(item);

    public IEnumerator<string> GetEnumerator()
    {
      return (IEnumerator<string>) this.dict.Keys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.dict.Keys.GetEnumerator();

    public int Count => this.dict.Keys.Count<string>();

    public bool IsReadOnly => false;

    public void AddRange(IEnumerable<string> types)
    {
      foreach (string type in types)
        this.Add(type);
    }
  }
}
