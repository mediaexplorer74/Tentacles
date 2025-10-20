// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Resources
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace PressPlay.FFWD
{
  public static class Resources
  {
    private static Dictionary<string, GameObject> cachedResources = new Dictionary<string, GameObject>();
    internal static AssetHelper AssetHelper;

    public static UnityObject Load(string name)
    {
      Application.loadingScene = true;
      Scene scene = Resources.AssetHelper.Load<Scene>(Path.Combine(nameof (Resources), name));
      scene.Initialize();
      Application.loadingScene = false;
      Application.LoadNewAssets();
      if (scene.gameObjects.Count > 0)
        return (UnityObject) scene.gameObjects[0];
      return scene.prefabs.Count > 0 ? (UnityObject) scene.prefabs[0] : (UnityObject) null;
    }
  }
}
