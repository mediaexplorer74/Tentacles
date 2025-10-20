// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Util.FixedArray3`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Util
{
  public struct FixedArray3<T> : IEnumerable<T>, IEnumerable where T : class
  {
    public T _0;
    public T _1;
    public T _2;

    public T this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this._0;
          case 1:
            return this._1;
          case 2:
            return this._2;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this._0 = value;
            break;
          case 1:
            this._1 = value;
            break;
          case 2:
            this._2 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }

    public IEnumerator<T> GetEnumerator() => this.Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool Contains(T value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if ((object) this[index] == (object) value)
          return true;
      }
      return false;
    }

    public int IndexOf(T value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if ((object) this[index] == (object) value)
          return index;
      }
      return -1;
    }

    public void Clear()
    {
      T obj = default (T);
      this._2 = obj;
      this._0 = this._1 = this._2 = obj;
    }

    public void Clear(T value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if ((object) this[index] == (object) value)
          this[index] = default (T);
      }
    }

    private IEnumerable<T> Enumerate()
    {
      for (int i = 0; i < 3; ++i)
        yield return this[i];
    }
  }
}
