// Decompiled with JetBrains decompiler
// Type: PressPlay.FFWD.Application
// Assembly: PressPlay.FFWD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 71C18607-4890-4187-AD5F-810BF86AC08E
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.FFWD.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.Interfaces;
using PressPlay.FFWD.UI.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace PressPlay.FFWD
{
  public class Application : DrawableGameComponent
  {
    public static bool isDeactivated = false;
    private int frameRate;
    private int frameCounter;
    private TimeSpan elapsedTime = TimeSpan.Zero;
    private static string sceneToLoad = "";
    public static PressPlay.FFWD.ScreenManager.ScreenManager screenManager;
    private static readonly Dictionary<int, UnityObject> objects = new Dictionary<int, UnityObject>(5000);
    internal static readonly List<Asset> newAssets = new List<Asset>(100);
    internal static readonly List<Component> newComponents = new List<Component>();
    private static readonly Queue<Component> componentsToAwake = new Queue<Component>(2500);
    private static readonly Queue<Component> instantiatedComponentsToAwake = new Queue<Component>(50);
    private static readonly List<Component> componentsToStart = new List<Component>();
    private static readonly List<PressPlay.FFWD.Interfaces.IUpdateable> updateComponents = new List<PressPlay.FFWD.Interfaces.IUpdateable>(500);
    private static readonly List<IFixedUpdateable> fixedUpdateComponents = new List<IFixedUpdateable>(100);
    private static readonly List<PressPlay.FFWD.Interfaces.IUpdateable> lateUpdateComponents = new List<PressPlay.FFWD.Interfaces.IUpdateable>(100);
    private static readonly List<Component> componentsChangingActivity = new List<Component>(50);
    private static readonly TypeSet isUpdateable = new TypeSet(100);
    private static readonly TypeSet isFixedUpdateable = new TypeSet(25);
    private static readonly TypeSet isLateUpdateable = new TypeSet(25);
    private static readonly TypeSet hasAwake = new TypeSet(50);
    internal static readonly TypeSet fixReferences = new TypeSet(5);
    private static readonly List<InvokeCall> invokeCalls = new List<InvokeCall>(10);
    internal static readonly List<UnityObject> markedForDestruction = new List<UnityObject>();
    internal static readonly List<GameObject> dontDestroyOnLoad = new List<GameObject>(50);
    internal static bool loadingScene = false;
    public static bool isLoadingAssetBeforeSceneInitialize = false;
    private static bool doGarbageCollectAfterAwake = false;
    internal static bool loadIsComplete = false;
    internal static bool hasDrawBeenCalled = false;
    private static int totalNumberOfAssetsToLoad = 0;
    private static int numberOfAssetsLoaded = 0;
    internal static StringBuilder progressString = new StringBuilder();
    internal static float _loadingProgess = 0.0f;
    private static Scene scene;
    private static Stopwatch stopWatch = new Stopwatch();
    internal static readonly List<Component> tempComponents = new List<Component>();
    internal static readonly List<Asset> tempAssets = new List<Asset>();
    private static AssetHelper assetHelper = new AssetHelper();
    private static bool quitNextUpdate = false;

    public Application(Game game)
      : base(game)
    {
      this.UpdateOrder = 1;
      this.DrawOrder = 0;
      Application.isUpdateable.Add(typeof (iTween));
      Application.isLateUpdateable.Add(typeof (iTween));
      Application.isFixedUpdateable.Add(typeof (iTween));
      Application.isUpdateable.Add(typeof (ScrollingPanelControl));
    }

    public static float loadingProgress => Application._loadingProgess;

    public override void Initialize()
    {
      base.Initialize();
      ContentHelper.Services = this.Game.Services;
      ContentHelper.StaticContent = new ContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory);
      ContentHelper.Content = new ContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory);
      ContentHelper.IgnoreMissingAssets = true;
      Camera.FullScreen = this.Game.GraphicsDevice.Viewport;
      Resources.AssetHelper = Application.assetHelper;
      Physics.Initialize();
      Time.Reset();
      Input.Initialize();
      Application.assetHelper.CreateContentManager = new Func<ContentManager>(this.CreateContentManager);
      Camera.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
      Camera.basicEffect = new BasicEffect(this.Game.GraphicsDevice);
      TextRenderer3D.basicEffect = new BasicEffect(this.Game.GraphicsDevice)
      {
        TextureEnabled = true,
        VertexColorEnabled = true,
        World = TextRenderer3D.invertY,
        View = Matrix.Identity
      };
      TextRenderer3D.batch = new SpriteBatch(this.Game.GraphicsDevice);
    }

    private ContentManager CreateContentManager()
    {
      return new ContentManager((IServiceProvider) this.Game.Services, this.Game.Content.RootDirectory);
    }

    private void StartComponents()
    {
      for (int index = 0; index < Application.componentsToStart.Count; ++index)
      {
        Component component = Application.componentsToStart[index];
        Application.componentsChangingActivity.Add(component);
        component.Start();
      }
      Application.componentsToStart.Clear();
    }

    public override void Update(GameTime gameTime)
    {
      if (Application.quitNextUpdate)
      {
        this.Game.Exit();
      }
      else
      {
        base.Update(gameTime);
        Time.FixedUpdate((float) gameTime.ElapsedGameTime.TotalSeconds, (float) gameTime.TotalGameTime.TotalSeconds);
        this.UpdateFPS(gameTime);
        if (Application.isLoadingAssetBeforeSceneInitialize)
        {
          if (Application.loadIsComplete)
          {
            this.OnSceneLoadComplete();
            return;
          }
          if (Application.hasDrawBeenCalled)
            this.LoadSceneAssets();
          this.CalculateLoadingProgress();
        }
        if (!string.IsNullOrEmpty(Application.sceneToLoad))
        {
          Application.CleanUp();
          this.DoSceneLoad();
        }
        Application.LoadNewAssets();
        Application.AwakeNewComponents(false);
        this.StartComponents();
        Application.ChangeComponentActivity();
        if ((double) Time.timeScale > 0.0)
        {
          int count = Application.fixedUpdateComponents.Count;
          for (int index = 0; index < count; ++index)
          {
            IFixedUpdateable fixedUpdateComponent = Application.fixedUpdateComponents[index];
            if (fixedUpdateComponent.gameObject.active)
              fixedUpdateComponent.FixedUpdate();
          }
        }
        Application.ChangeComponentActivity();
        Physics.Update(Time.deltaTime);
        Application.hasDrawBeenCalled = false;
      }
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      Time.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
      Application.hasDrawBeenCalled = true;
      ++this.frameCounter;
      this.StartComponents();
      Application.ChangeComponentActivity();
      int count1 = Application.updateComponents.Count;
      for (int index = 0; index < count1; ++index)
      {
        PressPlay.FFWD.Interfaces.IUpdateable updateComponent = Application.updateComponents[index];
        if (updateComponent.gameObject.active)
          updateComponent.Update();
      }
      Application.ChangeComponentActivity();
      Application.UpdateInvokeCalls();
      int count2 = Application.lateUpdateComponents.Count;
      for (int index = 0; index < count2; ++index)
        Application.lateUpdateComponents[index].LateUpdate();
      Application.ChangeComponentActivity();
      Application.CleanUp();
      Camera.DoRender(this.GraphicsDevice);
    }

    private void UpdateFPS(GameTime gameTime)
    {
      this.elapsedTime += gameTime.ElapsedGameTime;
      if (!(this.elapsedTime > TimeSpan.FromSeconds(1.0)))
        return;
      this.elapsedTime -= TimeSpan.FromSeconds(1.0);
      this.frameRate = this.frameCounter;
      this.frameCounter = 0;
    }

    private void DoSceneLoad()
    {
      Application._loadingProgess = 0.0f;
      if (!string.IsNullOrEmpty(Application.loadedLevelName))
      {
        Application.UnloadCurrentLevel();
        Application.CleanUp();
        Application.assetHelper.Unload(Application.loadedLevelName);
      }
      Application.loadingScene = true;
      Application.isLoadingAssetBeforeSceneInitialize = true;
      Application.loadIsComplete = false;
      Application.loadedLevelName = Application.sceneToLoad.Contains<char>('/') ? Application.sceneToLoad.Substring(Application.sceneToLoad.LastIndexOf('/') + 1) : Application.sceneToLoad;
      Application.scene = Application.assetHelper.Load<Scene>(Application.sceneToLoad);
      Application.sceneToLoad = "";
      Application.totalNumberOfAssetsToLoad = Application.tempAssets.Count;
      Application.numberOfAssetsLoaded = 0;
      if (Application.scene != null)
      {
        Application.isUpdateable.AddRange((IEnumerable<string>) Application.scene.isUpdateable);
        Application.isFixedUpdateable.AddRange((IEnumerable<string>) Application.scene.isFixedUpdateable);
        Application.isLateUpdateable.AddRange((IEnumerable<string>) Application.scene.isLateUpdateable);
        Application.hasAwake.AddRange((IEnumerable<string>) Application.scene.hasAwake);
        Application.fixReferences.AddRange((IEnumerable<string>) Application.scene.fixReferences);
      }
      if (Application.scene != null)
        return;
      Debug.Log((object) "Scene is NULL. Completing load!");
      this.OnSceneLoadComplete();
    }

    private void LoadSceneAssets()
    {
      Application.stopWatch.Start();
      int num = 0;
      for (int index = Application.tempAssets.Count - 1; index >= 0; --index)
      {
        if (Application.stopWatch.ElapsedMilliseconds > (long) ApplicationSettings.AssetLoadInterval)
        {
          Application.stopWatch.Stop();
          Application.stopWatch.Reset();
          return;
        }
        Application.tempAssets[index].LoadAsset(Application.assetHelper);
        Application.tempAssets.RemoveAt(index);
        ++Application.numberOfAssetsLoaded;
        ++num;
      }
      Application.loadIsComplete = true;
    }

    private void CalculateLoadingProgress()
    {
      if (Application.totalNumberOfAssetsToLoad == 0)
        Application._loadingProgess = 1f;
      else
        Application._loadingProgess = Mathf.Clamp01((float) Application.numberOfAssetsLoaded / (float) Application.totalNumberOfAssetsToLoad);
    }

    private void OnSceneLoadComplete()
    {
      Application.stopWatch.Stop();
      Application.stopWatch.Reset();
      Application.newComponents.AddRange((IEnumerable<Component>) Application.tempComponents);
      Application.tempComponents.Clear();
      Application.loadingScene = false;
      Application.isLoadingAssetBeforeSceneInitialize = false;
      Application.loadIsComplete = false;
      if (Application.scene != null)
        Application.scene.Initialize();
      Application.doGarbageCollectAfterAwake = true;
    }

    internal static void LoadNewAssets()
    {
      for (int index = Application.newAssets.Count - 1; index >= 0; --index)
      {
        Application.newAssets[index].LoadAsset(Application.assetHelper);
        Application.newAssets.RemoveAt(index);
      }
    }

    public static void LoadLevel(string name)
    {
      Application.sceneToLoad = name;
      Application.UnloadCurrentLevel();
    }

    public static void UnloadCurrentLevel()
    {
      foreach (UnityObject unityObject in Application.objects.Values)
      {
        if (unityObject is GameObject)
        {
          GameObject gameObject = (GameObject) unityObject;
          if (!Application.dontDestroyOnLoad.Contains(gameObject))
            UnityObject.Destroy((UnityObject) gameObject);
        }
      }
    }

    public static UnityObject Find(int id)
    {
      return Application.objects.ContainsKey(id) ? Application.objects[id] : (UnityObject) null;
    }

    internal static T[] FindObjectsOfType<T>() where T : UnityObject
    {
      List<T> objList = new List<T>();
      foreach (UnityObject unityObject in Application.objects.Values)
      {
        if (unityObject is T obj)
          objList.Add(obj);
      }
      return objList.ToArray();
    }

    internal static UnityObject[] FindObjectsOfType(Type type)
    {
      List<UnityObject> unityObjectList = new List<UnityObject>();
      foreach (UnityObject unityObject in Application.objects.Values)
      {
        if (unityObject.GetType() == type && !unityObject.isPrefab)
          unityObjectList.Add(unityObject);
      }
      return unityObjectList.ToArray();
    }

    internal static UnityObject FindObjectOfType(Type type)
    {
      foreach (UnityObject objectOfType in Application.objects.Values)
      {
        if (objectOfType.GetType() == type && !objectOfType.isPrefab)
          return objectOfType;
      }
      return (UnityObject) null;
    }

    internal static void AwakeNewComponents() => Application.AwakeNewComponents(false);

    internal static void AwakeNewComponents(bool onInstantiate)
    {
      int count = Application.newComponents.Count;
      for (int index = 0; index < count; ++index)
      {
        Component newComponent = Application.newComponents[index];
        if (newComponent.gameObject != null)
        {
          Application.objects.Add(newComponent.GetInstanceID(), (UnityObject) newComponent);
          if (!newComponent.isPrefab)
            Application.componentsToStart.Add(newComponent);
          if (!Application.objects.ContainsKey(newComponent.gameObject.GetInstanceID()))
            Application.objects.Add(newComponent.gameObject.GetInstanceID(), (UnityObject) newComponent.gameObject);
          if (!newComponent.isPrefab && Application.hasAwake.Contains(newComponent.GetType()))
          {
            if (onInstantiate)
              Application.instantiatedComponentsToAwake.Enqueue(newComponent);
            else
              Application.componentsToAwake.Enqueue(newComponent);
          }
        }
      }
      Application.newComponents.Clear();
      if (onInstantiate)
      {
        while (Application.instantiatedComponentsToAwake.Count > 0)
          Application.instantiatedComponentsToAwake.Dequeue().Awake();
      }
      else
      {
        while (Application.componentsToAwake.Count > 0)
          Application.componentsToAwake.Dequeue().Awake();
      }
      if (Application.newComponents.Count > 0)
        Application.AwakeNewComponents(onInstantiate);
      if (onInstantiate || !Application.doGarbageCollectAfterAwake)
        return;
      GC.Collect();
      Application.doGarbageCollectAfterAwake = false;
    }

    internal static void AddNewComponent(Component component)
    {
      if (Application.isLoadingAssetBeforeSceneInitialize)
        Application.tempComponents.Add(component);
      else
        Application.newComponents.Add(component);
    }

    internal static void AddNewAsset(Asset asset)
    {
      if (Application.isLoadingAssetBeforeSceneInitialize)
        Application.tempAssets.Add(asset);
      else
        Application.newAssets.Add(asset);
    }

    internal static void Reset()
    {
      Application.objects.Clear();
      Application.updateComponents.Clear();
      Application.fixedUpdateComponents.Clear();
      Application.lateUpdateComponents.Clear();
      Application.markedForDestruction.Clear();
    }

    internal static void CleanUp()
    {
      for (int index1 = 0; index1 < Application.markedForDestruction.Count; ++index1)
      {
        Application.objects.Remove(Application.markedForDestruction[index1].GetInstanceID());
        if (Application.markedForDestruction[index1] is Component)
        {
          Component component = Application.markedForDestruction[index1] as Component;
          if (component is Renderer)
            Camera.RemoveRenderer(component as Renderer);
          if (component is Camera)
            Camera.RemoveCamera(component as Camera);
          if (component.gameObject != null)
            component.gameObject.RemoveComponent(component);
          if (Application.newComponents.Contains(component))
            Application.newComponents.Remove(component);
          if (Application.componentsToStart.Contains(component))
            Application.componentsToStart.Remove(component);
          if (component is PressPlay.FFWD.Interfaces.IUpdateable)
          {
            PressPlay.FFWD.Interfaces.IUpdateable updateable = component as PressPlay.FFWD.Interfaces.IUpdateable;
            if (Application.updateComponents.Contains(updateable))
              Application.updateComponents.Remove(updateable);
            if (Application.lateUpdateComponents.Contains(updateable))
              Application.lateUpdateComponents.Remove(updateable);
          }
          if (component is IFixedUpdateable && Application.fixedUpdateComponents.Contains(component as IFixedUpdateable))
            Application.fixedUpdateComponents.Remove(component as IFixedUpdateable);
          for (int index2 = Application.invokeCalls.Count - 1; index2 >= 0; --index2)
          {
            if (Application.invokeCalls[index2].behaviour == component)
              Application.invokeCalls.RemoveAt(index2);
          }
        }
      }
      Application.markedForDestruction.Clear();
    }

    public static string loadedLevelName { get; private set; }

    internal static void DontDestroyOnLoad(UnityObject target)
    {
      if (target is Component && !Application.dontDestroyOnLoad.Contains(((Component) target).gameObject))
        Application.dontDestroyOnLoad.Add(((Component) target).gameObject);
      if (!(target is GameObject) || Application.dontDestroyOnLoad.Contains((GameObject) target))
        return;
      Application.dontDestroyOnLoad.Add((GameObject) target);
    }

    public static void Quit() => Application.quitNextUpdate = true;

    public static T Load<T>(string name) => Application.assetHelper.Load<T>(name);

    public static void AddStaticAsset(string name) => Application.assetHelper.AddStaticAsset(name);

    public static void Preload<T>(string name) => Application.assetHelper.Preload<T>(name);

    public static T PreloadInstant<T>(string name)
    {
      return Application.assetHelper.PreloadInstant<T>(name);
    }

    internal static void UpdateGameObjectActive(List<Component> components)
    {
      for (int index = 0; index < components.Count; ++index)
        Application.componentsChangingActivity.Add(components[index]);
    }

    private static void ChangeComponentActivity()
    {
      for (int index = 0; index < Application.componentsChangingActivity.Count; ++index)
      {
        Component component = Application.componentsChangingActivity[index];
        Type type = component.GetType();
        if (component.gameObject != null)
        {
          if (component.gameObject.active)
          {
            if (Application.isUpdateable.Contains(type) && !Application.updateComponents.Contains(component as PressPlay.FFWD.Interfaces.IUpdateable))
              Application.updateComponents.Add(component as PressPlay.FFWD.Interfaces.IUpdateable);
            if (Application.isLateUpdateable.Contains(type) && !Application.lateUpdateComponents.Contains(component as PressPlay.FFWD.Interfaces.IUpdateable))
              Application.lateUpdateComponents.Add(component as PressPlay.FFWD.Interfaces.IUpdateable);
            if (Application.isFixedUpdateable.Contains(type) && !Application.fixedUpdateComponents.Contains(component as IFixedUpdateable))
              Application.fixedUpdateComponents.Add(component as IFixedUpdateable);
            if (component is Renderer)
              Camera.AddRenderer(component as Renderer);
          }
          else
          {
            if (Application.isUpdateable.Contains(type) && Application.updateComponents.Contains(component as PressPlay.FFWD.Interfaces.IUpdateable))
              Application.updateComponents.Remove(component as PressPlay.FFWD.Interfaces.IUpdateable);
            if (Application.isLateUpdateable.Contains(type) && Application.lateUpdateComponents.Contains(component as PressPlay.FFWD.Interfaces.IUpdateable))
              Application.lateUpdateComponents.Remove(component as PressPlay.FFWD.Interfaces.IUpdateable);
            if (Application.isFixedUpdateable.Contains(type) && Application.fixedUpdateComponents.Contains(component as IFixedUpdateable))
              Application.fixedUpdateComponents.Remove(component as IFixedUpdateable);
            if (component is Renderer)
              Camera.RemoveRenderer(component as Renderer);
          }
        }
      }
      Application.componentsChangingActivity.Clear();
    }

    internal static void AddInvokeCall(
      MonoBehaviour behaviour,
      string methodName,
      float time,
      float repeatRate)
    {
      Application.invokeCalls.Add(new InvokeCall()
      {
        behaviour = behaviour,
        methodName = methodName,
        time = time,
        repeatRate = repeatRate
      });
    }

    internal static bool IsInvoking(MonoBehaviour behaviour, string methodName)
    {
      for (int index = 0; index < Application.invokeCalls.Count; ++index)
      {
        if (Application.invokeCalls[index].behaviour == behaviour && Application.invokeCalls[index].methodName == methodName)
          return true;
      }
      return false;
    }

    internal static void UpdateInvokeCalls()
    {
      for (int index = Application.invokeCalls.Count - 1; index >= 0; --index)
      {
        InvokeCall invokeCall = Application.invokeCalls[index];
        if (invokeCall.Update(Time.deltaTime))
        {
          invokeCall.behaviour.SendMessage(Application.invokeCalls[index].methodName, (object) null);
          Application.invokeCalls.RemoveAt(index);
        }
        else
          Application.invokeCalls[index] = invokeCall;
      }
    }
  }
}
