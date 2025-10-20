// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.FixedArray2`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct FixedArray2<T>
  {
    private T _value0;
    private T _value1;

    public T this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this._value0;
          case 1:
            return this._value1;
          default:
            throw new IndexOutOfRangeException();
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this._value0 = value;
            break;
          case 1:
            this._value1 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }
  }
}
