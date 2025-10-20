// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.RotateThis
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class RotateThis : MonoBehaviour
  {
    public float speed;

    public override void Start()
    {
      if ((double) this.speed != 0.0)
        return;
      this.speed = Random.Range(-1f, 1f);
    }

    public override void FixedUpdate() => this.gameObject.transform.Rotate(0.0f, this.speed, 0.0f);
  }
}
