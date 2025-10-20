// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.XNAParticleSpriteSheet
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class XNAParticleSpriteSheet : MonoBehaviour
  {
    public int xCount = 1;
    public int yCount = 1;

    public override void Awake()
    {
      base.Awake();
      if (this.xCount <= 0 || this.yCount <= 0)
        return;
      ParticleEmitter component = this.gameObject.GetComponent<ParticleEmitter>();
      component.textureTileCountX = this.xCount;
      component.textureTileCountY = this.yCount;
    }
  }
}
