// Decompiled with JetBrains decompiler
// Type: Poly2Tri.Triangulation.Util.FixedBitArray3
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Poly2Tri.Triangulation.Util
{
  public struct FixedBitArray3 : IEnumerable<bool>, IEnumerable
  {
    public bool _0;
    public bool _1;
    public bool _2;

    public bool this[int index]
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

    public IEnumerator<bool> GetEnumerator() => this.Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public bool Contains(bool value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (this[index] == value)
          return true;
      }
      return false;
    }

    public int IndexOf(bool value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (this[index] == value)
          return index;
      }
      return -1;
    }

    public void Clear() => this._0 = this._1 = this._2 = false;

    public void Clear(bool value)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (this[index] == value)
          this[index] = false;
      }
    }

    private IEnumerable<bool> Enumerate()
    {
      for (int i = 0; i < 3; ++i)
        yield return this[i];
    }
  }
}
