// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.ScreenManager.PlayerIndexEventArgs
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace PressPlay.FFWD.ScreenManager
{
  public class PlayerIndexEventArgs : EventArgs
  {
    private PlayerIndex playerIndex;

    public PlayerIndexEventArgs(PlayerIndex playerIndex) => this.playerIndex = playerIndex;

    public PlayerIndex PlayerIndex => this.playerIndex;
  }
}
