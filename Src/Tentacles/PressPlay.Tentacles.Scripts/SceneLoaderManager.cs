// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.SceneLoaderManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class SceneLoaderManager : MonoBehaviour
  {
    private static bool loadLevelAfterInit;
    private static SceneLoaderManager instance;
    public static bool isLoaded = false;
    private static string _previousLevel;
    private static int _nextLevelId;
    private static string _nextLevel;
    private bool loadLevelAtNextLateUpdate;
    public string sceneLoaderScene = "SceneLoader";
    public string mainMenuScene = "MainMenu";
    [ContentSerializerIgnore]
    public string upsellScreen = "Upsell";

    public string previousLevel => SceneLoaderManager._previousLevel;

    public int nextLevelId => SceneLoaderManager._nextLevelId;

    public string nextLevel => SceneLoaderManager._nextLevel;

    public bool loadedSceneIsMainMenu => Application.loadedLevelName == this.mainMenuScene;

    public bool loadedSceneIsSceneLoader => Application.loadedLevelName == this.sceneLoaderScene;

    public bool loadedSceneIsPreloader => Application.loadedLevelName == "Preloader";

    public static SceneLoaderManager Instance
    {
      get
      {
        if (SceneLoaderManager.instance == null)
          Debug.LogError("Attempt to access instance of singleton earlier than Start or without it being attached to a GameObject.");
        return SceneLoaderManager.instance;
      }
    }

    public override void Awake()
    {
      if (SceneLoaderManager.instance != null)
      {
        Debug.LogError("Cannot have two instances of " + this.name + ". Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        SceneLoaderManager.isLoaded = true;
        SceneLoaderManager.instance = this;
      }
    }

    public override void Start()
    {
      if (this.sceneLoaderScene == "")
        Debug.LogError("The SceneLoaderManager needs to have a SceneLoader scene defined!");
      if (!(this.mainMenuScene == ""))
        return;
      Debug.LogError("The SceneLoaderManager needs to have a MainMenu scene defined!");
    }

    public override void LateUpdate()
    {
      if (!this.loadLevelAtNextLateUpdate)
        return;
      this.loadLevelAtNextLateUpdate = false;
      this.LoadSceneLoader();
    }

    public static void LoadPreloader() => Application.LoadLevel("Preloader");

    public static void LoadPreloader(string levelName)
    {
      SceneLoaderManager._nextLevel = levelName;
      SceneLoaderManager.loadLevelAfterInit = true;
    }

    public void DoStartUpLoad()
    {
      if (SceneLoaderManager.loadLevelAfterInit)
      {
        Level levelFromSceneName = GlobalManager.Instance.database.GetCurrentLevelFromSceneName(SceneLoaderManager._nextLevel);
        if (levelFromSceneName != null)
          GlobalManager.Instance.OpenLevel(levelFromSceneName);
        else
          this.LoadLevel(SceneLoaderManager._nextLevel);
      }
      else
        this.LoadMainMenu();
    }

    public void LoadLevel(string levelName)
    {
      SceneLoaderManager._nextLevel = levelName;
      this.loadLevelAtNextLateUpdate = true;
    }

    public void LoadLevel(int levelId)
    {
      SceneLoaderManager._nextLevelId = levelId;
      this.loadLevelAtNextLateUpdate = true;
    }

    public void LoadMainMenu() => this.LoadLevel(this.mainMenuScene);

    private void LoadSceneLoader()
    {
      SceneLoaderManager._previousLevel = Application.loadedLevelName;
      if (SceneLoaderManager._nextLevel == this.mainMenuScene)
      {
        if (this.loadedSceneIsPreloader)
          Application.screenManager.AddScreenBelow((GameScreen) new MainMenu(), new PlayerIndex?());
        else
          LoadingScreen2.Load(Application.screenManager, true, new PlayerIndex?(), (GameScreen) new MainMenu());
        Application.LoadLevel(this.mainMenuScene);
      }
      else
        Application.screenManager.AddScreen((GameScreen) new LoadingScreen2(SceneLoaderManager._nextLevel), new PlayerIndex?());
    }
  }
}
