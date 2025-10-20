// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.HashSet`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace FarseerPhysics.Common
{
  public class HashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private Dictionary<T, short> _dict;

    public HashSet(int capacity) => this._dict = new Dictionary<T, short>(capacity);

    public HashSet() => this._dict = new Dictionary<T, short>();

    public void Add(T item) => this._dict.Add(item, (short) 0);

    public void Clear() => this._dict.Clear();

    public bool Contains(T item) => this._dict.ContainsKey(item);

    public void CopyTo(T[] array, int arrayIndex) => throw new NotImplementedException();

    public bool Remove(T item) => this._dict.Remove(item);

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) this._dict.Keys.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._dict.Keys.GetEnumerator();

    public int Count => this._dict.Keys.Count;

    public bool IsReadOnly => false;
  }
}
