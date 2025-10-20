// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.UnityObject
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class UnityObject
  {
    [ContentSerializer(ElementName = "id")]
    private int _id = -1;
    private static int nextId = 1;
    [ContentSerializer(ElementName = "isPrefab", Optional = true)]
    internal bool isPrefab;

    public UnityObject()
    {
      if (!Application.loadingScene)
        this._id = UnityObject.nextId++;
      this.isPrefab = false;
    }

    public int GetInstanceID() => this._id;

    internal virtual void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      idMap?.Add(this._id, this);
    }

    public static void Destroy(UnityObject obj) => UnityObject.Destroy(obj, 0.0f);

    public static void Destroy(UnityObject obj, float time) => obj?.Destroy();

    protected virtual void Destroy() => Application.markedForDestruction.Add(this);

    public static UnityObject Instantiate(
      UnityObject original,
      Vector3 position,
      Quaternion rotation)
    {
      UnityObject unityObject = UnityObject.Instantiate(original);
      switch (unityObject)
      {
        case GameObject _:
          (unityObject as GameObject).transform.localPosition = position;
          (unityObject as GameObject).transform.localRotation = rotation;
          break;
        case Component _:
          (unityObject as Component).transform.localPosition = position;
          (unityObject as Component).transform.localRotation = rotation;
          break;
      }
      return unityObject;
    }

    public static Mesh Instantiate(Mesh original) => (Mesh) original.Clone();

    public static UnityObject Instantiate(UnityObject original)
    {
      if (original == null)
        return (UnityObject) null;
      GameObject gameObject = (GameObject) null;
      if (original is Component)
        gameObject = (original as Component).gameObject.Clone() as GameObject;
      else if (original is GameObject)
        gameObject = (original as GameObject).transform.root.gameObject.Clone() as GameObject;
      Dictionary<int, UnityObject> idMap = new Dictionary<int, UnityObject>();
      gameObject.SetNewId(idMap);
      gameObject.FixReferences(idMap);
      Application.AwakeNewComponents(true);
      return idMap[original.GetInstanceID()];
    }

    internal virtual void SetNewId(Dictionary<int, UnityObject> idMap)
    {
      if (!this.isPrefab && idMap != null)
        idMap[this._id] = this;
      this._id = UnityObject.nextId++;
    }

    internal virtual void FixReferences(Dictionary<int, UnityObject> idMap)
    {
    }

    internal virtual UnityObject Clone() => this.MemberwiseClone() as UnityObject;

    public static T[] FindObjectsOfType<T>() where T : UnityObject
    {
      return Application.FindObjectsOfType<T>();
    }

    public static UnityObject[] FindObjectsOfType(Type type) => Application.FindObjectsOfType(type);

    public static UnityObject FindObjectOfType(Type type) => Application.FindObjectOfType(type);

    public static implicit operator bool(UnityObject obj)
    {
      return obj != null && Application.Find(obj.GetInstanceID()) != null;
    }

    public static void DontDestroyOnLoad(UnityObject target)
    {
      Application.DontDestroyOnLoad(target);
    }
  }
}
