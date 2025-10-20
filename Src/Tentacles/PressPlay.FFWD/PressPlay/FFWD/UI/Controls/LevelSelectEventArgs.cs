// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UI.Controls.LevelSelectEventArgs
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System;

#nullable disable
namespace PressPlay.FFWD.UI.Controls
{
  public class LevelSelectEventArgs : EventArgs
  {
    private int _levelId;

    public LevelSelectEventArgs(int levelId) => this._levelId = levelId;

    public int levelId => this._levelId;
  }
}
