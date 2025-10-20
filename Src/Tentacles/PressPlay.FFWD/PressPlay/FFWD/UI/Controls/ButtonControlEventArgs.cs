// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.ButtonControlEventArgs
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class ButtonControlEventArgs : EventArgs
  {
    private string _link;

    public ButtonControlEventArgs(string link) => this._link = link;

    public string link => this._link;
  }
}
