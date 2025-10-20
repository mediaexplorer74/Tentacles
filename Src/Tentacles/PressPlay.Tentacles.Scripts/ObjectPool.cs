// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ObjectPool
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ObjectPool : MonoBehaviour
  {
    public static bool isLoaded = false;
    private static ObjectPool instance;
    private Dictionary<string, Queue<PoolableObject>> pool = new Dictionary<string, Queue<PoolableObject>>();
    public ObjectPoolPreset[] presets;
    public bool disableObjectCaching;
    public int defaultNumberOfInstances = 1;
    public int growFactor = 1;

    public static ObjectPool Instance
    {
      get
      {
        if (ObjectPool.instance == null)
          Debug.LogError("Attempt to access instance of ObjectPool singleton earlier than Start or without it being attached to a GameObject.");
        return ObjectPool.instance;
      }
    }

    public override void Awake()
    {
      if (ObjectPool.instance != null)
        Debug.LogError("Cannot have two instances of ObjectPool. Setting instance to new instance and initializing");
      this.Initialize();
      ObjectPool.isLoaded = true;
      ObjectPool.instance = this;
    }

    public override void Start()
    {
    }

    private void Initialize()
    {
    }

    public PoolableObject Draw(PoolableObject prefab)
    {
      return this.Draw(prefab, prefab.transform.position, prefab.transform.rotation);
    }

    public PoolableObject Draw(PoolableObject prefab, Vector3 position, Quaternion rotation)
    {
      if (!this.pool.ContainsKey(prefab.guid))
        this.Grow(prefab, this.defaultNumberOfInstances);
      else if (this.pool[prefab.guid].Count == 0)
        this.Grow(prefab, this.growFactor);
      PoolableObject poolableObject = this.pool[prefab.guid].Dequeue();
      poolableObject.transform.position = position;
      poolableObject.transform.rotation = rotation;
      poolableObject.Activate();
      return poolableObject;
    }

    public void Return(PoolableObject obj)
    {
      obj.DeActivate();
      if (!this.pool.ContainsKey(obj.guid))
        this.pool.Add(obj.guid, new Queue<PoolableObject>());
      this.pool[obj.guid].Enqueue(obj);
    }

    public void Grow(PoolableObject prefab, int size, int maxPoolSize)
    {
      if (prefab == null)
        return;
      if (!this.pool.ContainsKey(prefab.guid))
      {
        this.Grow(prefab, Mathf.Min(size, maxPoolSize));
      }
      else
      {
        int y = maxPoolSize - this.pool[prefab.guid].Count;
        if (y <= 0)
          return;
        this.Grow(prefab, Mathf.Min(size, y));
      }
    }

    public void Grow(PoolableObject prefab, int size)
    {
      if (prefab == null)
        return;
      if (!this.pool.ContainsKey(prefab.guid))
        this.pool.Add(prefab.guid, new Queue<PoolableObject>());
      for (int index = 0; index < size; ++index)
      {
        PoolableObject poolableObject = (PoolableObject) UnityObject.Instantiate((UnityObject) prefab);
        poolableObject.Create();
        poolableObject.DeActivate();
        this.pool[prefab.guid].Enqueue(poolableObject);
      }
    }

    protected override void Destroy()
    {
      base.Destroy();
      ObjectPool.isLoaded = false;
      ObjectPool.instance = (ObjectPool) null;
    }
  }
}
