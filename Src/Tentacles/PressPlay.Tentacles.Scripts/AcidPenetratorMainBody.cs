// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.AcidPenetratorMainBody
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class AcidPenetratorMainBody : BasicPenetratorMainBody
  {
    private RaycastHit rh;
    private Ray ray = new Ray();
    public List<PoolableObject> returnToPoolOnLemmyDeath;
    public PoolableObject frontSearcherAcidPool;
    public PoolableObject mainBodyAcidPool;
    public float spawnAcidPoolInterval = 0.1f;
    private float lastSpawnFrontUpperTime;
    private float lastSpawnFrontLowerTime;
    private int upperSpawnPositionIndex;
    private int lowerSpawnPositionIndex;
    private float lastSpawnMainTime;
    private float lastSpawnMainCircularTime;

    public override void Initialize(
      Transform[] _upperPositions,
      Transform[] _lowerPositions,
      Transform[] _tailPositions)
    {
      base.Initialize(_upperPositions, _lowerPositions, _tailPositions);
      ObjectPool.Instance.Grow(this.frontSearcherAcidPool, 60, 60);
      ObjectPool.Instance.Grow(this.mainBodyAcidPool, 70, 70);
      this.returnToPoolOnLemmyDeath = new List<PoolableObject>(130);
    }

    public override void UpdateRunning()
    {
      base.UpdateRunning();
      if ((double) Time.time > (double) this.lastSpawnFrontUpperTime + (double) this.spawnAcidPoolInterval)
      {
        this.AddAcidPool(this.frontSearcherAcidPool, this.upperPositions[this.upperSpawnPositionIndex].position + this.upperPositions[this.upperSpawnPositionIndex].forward * Random.Range(-0.4f, 0.4f), this.upperPositions[this.upperSpawnPositionIndex].rotation);
        ++this.upperSpawnPositionIndex;
        this.upperSpawnPositionIndex %= this.lowerPositions.Length;
        this.lastSpawnFrontUpperTime = Time.time + Random.Range(-0.5f * this.spawnAcidPoolInterval, 0.5f * this.spawnAcidPoolInterval);
      }
      if ((double) Time.time > (double) this.lastSpawnFrontLowerTime + (double) this.spawnAcidPoolInterval)
      {
        this.AddAcidPool(this.frontSearcherAcidPool, this.lowerPositions[this.lowerSpawnPositionIndex].position + this.lowerPositions[this.lowerSpawnPositionIndex].forward * Random.Range(-0.4f, 0.4f), this.lowerPositions[this.lowerSpawnPositionIndex].rotation);
        ++this.lowerSpawnPositionIndex;
        this.lowerSpawnPositionIndex %= this.lowerPositions.Length;
        this.lastSpawnFrontLowerTime = Time.time + Random.Range(-0.5f * this.spawnAcidPoolInterval, 0.5f * this.spawnAcidPoolInterval);
      }
      if ((double) Time.time <= (double) this.lastSpawnMainTime + 0.20000000298023224 * (double) this.spawnAcidPoolInterval)
        return;
      this.AddAcidPool(this.mainBodyAcidPool, Vector3.Lerp(this.upperPositions[0].position, this.lowerPositions[0].position, Random.Range(0.01f, 0.99f)), this.lowerPositions[0].rotation);
      ++this.lowerSpawnPositionIndex;
      this.lowerSpawnPositionIndex %= this.lowerPositions.Length;
      this.lastSpawnMainTime = Time.time + Random.Range(-0.5f * this.spawnAcidPoolInterval, 0.5f * this.spawnAcidPoolInterval);
    }

    public override void UpdateRunFinished()
    {
      if ((double) Time.time <= (double) this.lastSpawnMainCircularTime + (double) this.spawnAcidPoolInterval)
        return;
      this.ray.origin = this.transform.position;
      this.ray.direction = Random.onUnitSphere;
      this.ray.direction = new Vector3(this.ray.direction.x, 0.0f, this.ray.direction.z);
      if (Physics.Raycast(this.ray, out this.rh, 4f, (int) GlobalSettings.Instance.allWallsLayers))
      {
        Debug.DrawLine(this.ray.origin, this.rh.point, Color.red);
        this.AddAcidPool(this.mainBodyAcidPool, this.ray.origin + (this.rh.point - this.ray.origin) * Mathf.Pow(Random.Range(0.0f, 0.95f), 0.7f), Quaternion.LookRotation(this.rh.normal));
      }
      else
        this.AddAcidPool(this.mainBodyAcidPool, this.ray.origin + this.ray.direction * 4f * Mathf.Pow(Random.Range(0.0f, 0.95f), 0.7f), this.transform.rotation);
      this.lastSpawnMainCircularTime = Time.time + Random.Range(-0.5f * this.spawnAcidPoolInterval, 0.5f * this.spawnAcidPoolInterval) * 0.2f;
    }

    private void AddAcidPool(PoolableObject _acidPool, Vector3 _position, Quaternion _rotation)
    {
      PoolableObject poolableObject = ObjectPool.Instance.Draw(_acidPool, _position, _rotation);
      if (this.returnToPoolOnLemmyDeath.Contains(poolableObject))
        return;
      this.returnToPoolOnLemmyDeath.Add(poolableObject);
    }

    public override void DoReset()
    {
      foreach (PoolableObject poolableObject in this.returnToPoolOnLemmyDeath)
        poolableObject.Return();
      this.returnToPoolOnLemmyDeath.Clear();
    }
  }
}
