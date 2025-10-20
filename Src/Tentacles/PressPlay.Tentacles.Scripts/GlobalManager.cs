// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.GlobalManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class GlobalManager : MonoBehaviour
  {
    public static bool titleUpdateDeclined = false;
    private bool useDebugLanguage = true;
    public GlobalManager.Languages debugLanguage;
    [ContentSerializerIgnore]
    public GlobalManager.Languages defaultLanguage;
    [ContentSerializerIgnore]
    public CheatManager cheatManager;
    public string buildId;
    public string revisionId;
    public string versionNumber;
    public bool includeFramerateCounter;
    public Level currentLevel;
    public GameSession currentSession;
    public UserProfile currentProfile;
    public List<string> levelsInBuildSettings;
    private static GlobalManager instance;
    public static bool isLoaded = false;
    public GameplaySettings gameplaySettings;
    public GameDatabase database;
    public Camera guiCamera;
    public FullScreenImageHandler fullscreenImageHandler;
    public bool useDebugMenu;
    [ContentSerializerIgnore]
    public bool isPaused;

    public static bool isTrialMode => TrialModeManager.Instance.TrialMode;

    public GlobalManager.Languages deviceLanguage
    {
      get
      {
        switch (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
        {
          case "en":
            return GlobalManager.Languages.en_GB;
          case "fr":
            return GlobalManager.Languages.fr_FR;
          case "it":
            return GlobalManager.Languages.it_IT;
          case "de":
            return GlobalManager.Languages.de_DE;
          case "es":
            return GlobalManager.Languages.es_ES;
          default:
            return this.defaultLanguage;
        }
      }
    }

    public GlobalManager.GlobalState globalState
    {
      get
      {
        if (LevelHandler.isLoaded)
          return GlobalManager.GlobalState.inLevel;
        if (!SceneLoaderManager.isLoaded || SceneLoaderManager.Instance.loadedSceneIsPreloader)
          return GlobalManager.GlobalState.loadingProgram;
        if (SceneLoaderManager.Instance.loadedSceneIsMainMenu)
          return GlobalManager.GlobalState.mainMenu;
        return SceneLoaderManager.Instance.loadedSceneIsSceneLoader ? GlobalManager.GlobalState.loadingScene : GlobalManager.GlobalState.limbo;
      }
    }

    public static GlobalManager Instance
    {
      get
      {
        if (GlobalManager.instance == null)
          Debug.LogError("Attempt to access instance of singleton earlier than Start or without it being attached to a GameObject.");
        return GlobalManager.instance;
      }
    }

    public override void Awake()
    {
      if (GlobalManager.instance != null)
      {
        Debug.LogError("Cannot have two instances of " + this.name + ". Self destruction in 3...");
        UnityObject.Destroy((UnityObject) this);
      }
      else
      {
        this.Initialize();
        GlobalManager.isLoaded = true;
        GlobalManager.instance = this;
        UnityObject.DontDestroyOnLoad((UnityObject) this);
      }
    }

    private void Initialize()
    {
      this.cheatManager = this.gameObject.AddComponent<CheatManager>();
      this.database.ConsolidateIndexes();
      AchievementsHandler.Instance.LoadAchievements();
      int num = GlobalManager.isTrialMode ? 1 : 0;
    }

    public override void Start()
    {
      LocalisationManager.Instance.SetLanguage(this.deviceLanguage.ToString());
    }

    public void DoStartUpLoad()
    {
    }

    public void Pause()
    {
      Time.timeScale = 0.0f;
      this.isPaused = true;
      InputHandler.Instance.turnOff = true;
    }

    public void UnPause()
    {
      Time.timeScale = 1f;
      this.isPaused = false;
      InputHandler.Instance.turnOff = false;
    }

    public void StartNewSession() => this.currentSession = new GameSession();

    public void ResetProfile()
    {
      this.currentProfile = new UserProfile(this.database.GetFirstLevel());
    }

    public void LoadProfile()
    {
      this.ResetProfile();
      try
      {
        SaveHandler.Instance.LoadGame(this.currentProfile, new SaveHandler.OnLoadComplete(this.LoadComplete));
      }
      catch (Exception ex)
      {
        Debug.Log((object) ex.Message);
        this.ResetProfile();
      }
      this.CheckForSendingTrialScoresToLeaderboard();
    }

    public void CheckForSendingTrialScoresToLeaderboard()
    {
      if (!this.currentProfile.scoreSavedInTrialMode || GlobalManager.isTrialMode)
        return;
      this.currentProfile.scoreSavedInTrialMode = false;
      if (this.currentProfile.GetLevelResult(1, 2) == null)
        return;
      this.SaveToLeaderboard(this.database.GetLevel(2));
    }

    public void SaveToDisk()
    {
      SaveHandler.Instance.SaveGame(this.currentProfile, new SaveHandler.OnSaveComplete(this.SaveComplete));
    }

    public void SaveToLeaderboard(Level level)
    {
      if (GlobalManager.titleUpdateDeclined)
        return;
      long totalScore = (long) GlobalManager.Instance.database.GetLevelsInBatch(level.worldId, level.batch).GetTotalScore();
      int leaderboardIdFromBatchId = GameDatabase.GetLeaderboardIdFromBatchId(level.batch);
      Debug.Log((object) ("level.batch : " + (object) level.batch + "   leaderboard id : " + (object) leaderboardIdFromBatchId));
      if (leaderboardIdFromBatchId < 0)
      {
        Debug.Log((object) "***************************************** TRYING TO SAVE LEADER BOARD ID -1 ************************************************");
      }
      else
      {
        if (!GlobalManager.isTrialMode)
          return;
        Gamer.SignedInGamers[PlayerIndex.One].LeaderboardWriter.GetLeaderboard(LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime, leaderboardIdFromBatchId)).Rating = totalScore;
      }
    }

    public void LoadComplete()
    {
    }

    public void SaveComplete()
    {
    }

    public void RestartCurrentLevel()
    {
      if (this.useDebugMenu)
      {
        AudioManager.Instance.StopAllSounds();
        SceneLoaderManager.Instance.LoadLevel(Application.loadedLevelName);
      }
      else
        this.OpenLevel(this.currentLevel);
    }

    public void OpenLevel(Level _level)
    {
      if (_level == null)
      {
        this.OpenMainMenu();
      }
      else
      {
        this.currentProfile.globalData.lastPlayedLevel = _level.id;
        this.currentProfile.globalData.playButtonOpensThisLevel = _level.id;
        this.currentLevel = _level;
        if (UnityObject.FindObjectOfType(typeof (AudioManager)) != null)
          AudioManager.Instance.StopAllSounds();
        SceneLoaderManager.Instance.LoadLevel(_level.sceneName);
      }
    }

    public void OpenNextLevel()
    {
      if (GlobalManager.isTrialMode && !this.currentProfile.IsLevelPartOfTrial(this.currentLevel.nextLevel))
        this.GotoTrialUpsellScreen(new UpsellScreen.DoOncancel(this.OpenMainMenu));
      else if (!this.database.IsLastLevelInWorld(this.currentLevel) && this.currentLevel.nextLevel != -1)
        this.OpenLevel(this.database.GetLevel(this.currentLevel.nextLevel));
      else
        this.OpenMainMenu();
    }

    public void OpenMainMenu()
    {
      this.UnPause();
      AudioManager.Instance.StopAllSounds();
      SceneLoaderManager.Instance.LoadMainMenu();
    }

    public void GotoTrialUpsellScreen(UpsellScreen.DoOncancel doOnCancel)
    {
      MainMenu.openUpsellOnMainMenuLoad = true;
      this.OpenMainMenu();
    }

    public enum GlobalState
    {
      loadingProgram,
      mainMenu,
      loadingScene,
      inLevel,
      limbo,
    }

    public enum Languages
    {
      en_GB,
      fr_FR,
      es_ES,
      de_DE,
      it_IT,
    }
  }
}
