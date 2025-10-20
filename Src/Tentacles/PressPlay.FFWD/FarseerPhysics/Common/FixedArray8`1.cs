// Decompiled with JetBrains decompiler
// Type: FarseerPhysics.Common.FixedArray8`1
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace FarseerPhysics.Common
{
  public struct FixedArray8<T>
  {
    private T _value0;
    private T _value1;
    private T _value2;
    private T _value3;
    private T _value4;
    private T _value5;
    private T _value6;
    private T _value7;

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
          case 2:
            return this._value2;
          case 3:
            return this._value3;
          case 4:
            return this._value4;
          case 5:
            return this._value5;
          case 6:
            return this._value6;
          case 7:
            return this._value7;
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
          case 2:
            this._value2 = value;
            break;
          case 3:
            this._value3 = value;
            break;
          case 4:
            this._value4 = value;
            break;
          case 5:
            this._value5 = value;
            break;
          case 6:
            this._value6 = value;
            break;
          case 7:
            this._value7 = value;
            break;
          default:
            throw new IndexOutOfRangeException();
        }
      }
    }
  }
}
