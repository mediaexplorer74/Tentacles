// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SimpleRotate
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SimpleRotate : MonoBehaviour
  {
    public float rotationSpeed = 0.3f;
    public bool switchDirection;

    public override void Start()
    {
    }

    public override void Update()
    {
      if (!this.switchDirection)
        this.transform.Rotate(Vector3.up, this.rotationSpeed * -Time.deltaTime, Space.Self);
      else
        this.transform.Rotate(Vector3.up, this.rotationSpeed * Time.deltaTime, Space.Self);
    }
  }
}
