// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.DelayedInstantiator
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class DelayedInstantiator : MonoBehaviour
  {
    private float duration;
    private float interval;
    private PoolableObject[] prefabs;
    private Transform[] positions;
    private int index;
    private float timeSinceLastInstatiate;
    private bool isStarted;

    public void Init(
      float duration,
      float interval,
      PoolableObject[] prefabs,
      Transform[] positions)
    {
      this.duration = duration;
      this.interval = interval;
      this.prefabs = prefabs;
      this.positions = positions;
      this.isStarted = true;
    }

    private new void Update()
    {
      if (!this.isStarted)
        return;
      if ((double) this.timeSinceLastInstatiate > (double) this.interval)
        this.CreateObject();
      this.timeSinceLastInstatiate += Time.deltaTime;
      if (this.index != this.prefabs.Length - 1)
        return;
      UnityObject.Destroy((UnityObject) this);
    }

    protected virtual void CreateObject()
    {
      ObjectPool.Instance.Draw(this.prefabs[this.index], this.positions[this.index].position, this.positions[this.index].rotation);
      this.timeSinceLastInstatiate = 0.0f;
      ++this.index;
    }

    private void OnDisable()
    {
      while (this.index < this.positions.Length - 1)
        this.CreateObject();
    }
  }
}
