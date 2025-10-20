// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.AssetHelper
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.FFWD
{
  public class AssetHelper
  {
    private Dictionary<string, ContentManager> contentManagers = new Dictionary<string, ContentManager>();
    private Dictionary<string, List<string>> managerContent = new Dictionary<string, List<string>>();
    private Dictionary<string, object> content = new Dictionary<string, object>();
    public Func<ContentManager> CreateContentManager;
    private static List<string> staticAssets = new List<string>();

    public T Load<T>(string contentPath)
    {
      return AssetHelper.staticAssets.Contains(contentPath) ? this.Load<T>("Static", contentPath) : this.Load<T>(Application.loadedLevelName, contentPath);
    }

    public T Load<T>(string category, string contentPath)
    {
      if (!this.content.ContainsKey(contentPath))
      {
        if (AssetHelper.staticAssets.Contains(contentPath) || AssetHelper.staticAssets.Contains(category))
          category = "Static";
        ContentManager contentManager = this.GetContentManager(category);
        try
        {
          this.content.Add(contentPath, (object) contentManager.Load<T>(contentPath));
          this.managerContent[category].Add(contentPath);
        }
        catch
        {
          return default (T);
        }
      }
      else if (!(this.content[contentPath] is T))
        return default (T);
      return (T) this.content[contentPath];
    }

    public void Unload(string category)
    {
      if (!this.contentManagers.ContainsKey(category))
        return;
      ContentManager contentManager = this.GetContentManager(category);
      this.contentManagers.Remove(category);
      List<string> stringList = this.managerContent[category];
      for (int index = 0; index < stringList.Count; ++index)
        this.content.Remove(stringList[index]);
      this.managerContent.Remove(category);
      contentManager.Unload();
    }

    private ContentManager GetContentManager(string category)
    {
      if (!this.contentManagers.ContainsKey(category))
      {
        this.contentManagers.Add(category, this.CreateContentManager());
        this.managerContent.Add(category, new List<string>());
      }
      return this.contentManagers[category];
    }

    public void AddStaticAsset(string name) => AssetHelper.staticAssets.Add(name);

    public void Preload<T>(string name) => AssetHelper.staticAssets.Add(name);

    public T PreloadInstant<T>(string name)
    {
      AssetHelper.staticAssets.Add(name);
      return this.Load<T>(name);
    }
  }
}
