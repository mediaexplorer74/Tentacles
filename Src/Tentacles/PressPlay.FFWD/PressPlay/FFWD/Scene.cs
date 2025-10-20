// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Scene
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD.Components;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class Scene
  {
    [ContentSerializer(Optional = true, ElementName = "up")]
    internal List<string> isUpdateable = new List<string>();
    [ContentSerializer(Optional = true, ElementName = "fup")]
    internal List<string> isFixedUpdateable = new List<string>();
    [ContentSerializer(Optional = true, ElementName = "lup")]
    internal List<string> isLateUpdateable = new List<string>();
    [ContentSerializer(Optional = true, ElementName = "awk")]
    internal List<string> hasAwake = new List<string>();
    [ContentSerializer(Optional = true, ElementName = "fix")]
    internal List<string> fixReferences = new List<string>();

    internal Scene()
    {
      this.gameObjects = new List<GameObject>();
      this.prefabs = new List<GameObject>();
    }

    [ContentSerializer(FlattenContent = true, CollectionItemName = "go")]
    public List<GameObject> gameObjects { get; set; }

    [ContentSerializer(FlattenContent = true, CollectionItemName = "p")]
    public List<GameObject> prefabs { get; set; }

    public void AfterLoad(Dictionary<int, UnityObject> idMap)
    {
      for (int index = 0; index < this.gameObjects.Count; ++index)
        this.gameObjects[index].AfterLoad(idMap);
      for (int index = 0; index < this.prefabs.Count; ++index)
      {
        this.prefabs[index].isPrefab = true;
        this.prefabs[index].AfterLoad(idMap);
      }
    }

    internal void Initialize()
    {
      Dictionary<int, UnityObject> idMap = new Dictionary<int, UnityObject>();
      this.AfterLoad(idMap);
      List<IdMap> idMapList = new List<IdMap>();
      for (int index = 0; index < Application.newComponents.Count; ++index)
      {
        Application.newComponents[index].FixReferences(idMap);
        if (Application.newComponents[index] is IdMap)
          idMapList.Add(Application.newComponents[index] as IdMap);
      }
      idMap.Clear();
      for (int index = 0; index < this.gameObjects.Count; ++index)
        this.gameObjects[index].SetNewId(idMap);
      for (int index = 0; index < this.prefabs.Count; ++index)
        this.prefabs[index].SetNewId(idMap);
      for (int index = 0; index < idMapList.Count; ++index)
      {
        idMapList[index].UpdateIdReferences(idMap);
        Application.newComponents.Remove((Component) idMapList[index]);
      }
    }
  }
}
