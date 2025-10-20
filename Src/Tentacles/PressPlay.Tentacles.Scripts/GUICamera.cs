// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GUICamera
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GUICamera : MonoBehaviour
  {
    public Camera bottomCamera;
    public Camera topCamera;

    public void ShakeCamera(Vector3 amount, float time)
    {
      iTween.ShakeRotation(this.gameObject, iTween.Hash((object) nameof (amount), (object) amount, (object) nameof (time), (object) time));
    }
  }
}
