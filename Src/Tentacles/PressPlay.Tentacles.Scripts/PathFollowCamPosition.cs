// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.PathFollowCamPosition
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public struct PathFollowCamPosition
  {
    public Microsoft.Xna.Framework.Vector3 gotoPos;
    private Microsoft.Xna.Framework.Quaternion targetRotation;
    public Microsoft.Xna.Framework.Vector3 childCamPos;
    public float speedMod;

    public override string ToString()
    {
      return "gotoPos " + this.gotoPos.ToString() + " rotation " + this.targetRotation.ToString() + " childCamPos " + this.childCamPos.ToString();
    }
  }
}
