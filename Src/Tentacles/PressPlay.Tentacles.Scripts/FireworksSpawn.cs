// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.FireworksSpawn
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class FireworksSpawn : MonoBehaviour
  {
    public AudioWrapper sounds;
    public PoolableParticle[] spawns;
    public Vector3 camShakeAmount = new Vector3(2f, 2f, 2f);
    public float camShakeDuration = 0.6f;
    private int spawnIndex;

    public override void Start()
    {
      for (int index = 0; index < this.spawns.Length; ++index)
        ObjectPool.Instance.Grow((PoolableObject) this.spawns[index], 1);
    }

    public void FireRandom()
    {
      ObjectPool.Instance.Draw((PoolableObject) this.spawns[Random.Range(0, this.spawns.Length)], this.transform.position, this.transform.rotation);
      this.sounds.PlaySound();
      LevelHandler.Instance.cam.ShakeCamera(this.camShakeAmount, this.camShakeDuration);
    }

    public void FireNext()
    {
      ObjectPool.Instance.Draw((PoolableObject) this.spawns[this.spawnIndex], this.transform.position, this.transform.rotation);
      this.spawnIndex = (this.spawnIndex + 1) % this.spawns.Length;
      this.sounds.PlaySound();
      LevelHandler.Instance.cam.ShakeCamera(this.camShakeAmount, this.camShakeDuration);
    }
  }
}
