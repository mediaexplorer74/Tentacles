// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LevelHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.ScreenManager;
using PressPlay.Tentacles.Scripts.Menu.MainMenu;
using System;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LevelHandler : TurnOffAtDistanceHandler
  {
    private int introUpdates;
    private LevelHandler.LevelState _state;
    private float _lastStateChange;
    private ILevelHandlerCutscene cutscene;
    public LevelSession levelSession;
    public Level.LevelType debugLevelType;
    public LevelTypeSettings levelTypeSettings;
    public Lemmy lemmyPrefab;
    public PathFollowCam cameraPrefab;
    public IngameGUI ingameGUIPrefab;
    public FeedbackHandler feedback;
    [ContentSerializerIgnore]
    public GUICamera GUICamera;
    [ContentSerializerIgnore]
    public PPAnimationHandler lemmyPainAnimation;
    [ContentSerializerIgnore]
    public Lemmy lemmy;
    [ContentSerializerIgnore]
    public PathFollowCam cam;
    [ContentSerializerIgnore]
    public Camera feedbackCam;
    [ContentSerializerIgnore]
    public IngameGUI ingameGUI;
    private ResetOnLemmyDeath[] resetOnDeathObjects;
    [ContentSerializerIgnore]
    public GameObject inGameMenuPrefab;
    private bool _isPlayingCinematicSequence;
    private CheckPoint startingCheckPoint;
    [ContentSerializerIgnore]
    public LevelStartCheckPoint levelStartCheckPoint;
    public CheckPoint[] checkpointOrder;
    [ContentSerializerIgnore]
    public CheckPoint lastSpawnedAtCheckpoint;
    private List<CheckPoint> activatedCheckPoints = new List<CheckPoint>();
    private CheckPoint[] checkPoints;
    private CheckPoint lastActivatedCheckpoint;
    private LevelExit levelExit;
    public static bool isLoaded = false;
    private static LevelHandler instance;
    public PathFollowObject lemmyHunter;
    private GameObject followObject;
    public GameLibrary library;
    public PoolablePickup enemyDeathPickup;
    private float frameRateCalcCnt;
    private float averageFrameRate;
    private float averageFramerate10Frames;
    private float[] lastFrames = new float[10];
    private int frameIndex;
    private float currentFramerate;
    public float criticalFrameRateDropLimit = 20f;
    public bool doCriticalFrameRateDropCheck;
    private int criticalFrameRateDropCounter;
    public int framesBetweenCriticalFrameChecks = 10;
    private Level _currentLevel;
    public ChallengeHandler challengeHandler;
    private float _globalTimeSinceLemmySpawn;
    private float _globalLevelTime;
    private float _globalLevelDeltaTime;
    [ContentSerializerIgnore]
    public MusicController musicController;
    [ContentSerializerIgnore]
    public InGamePauseMenu pauseMenu;
    [ContentSerializerIgnore]
    public EndLevelScreen endLevelScreen;

    public bool isInGamePlay
    {
      get
      {
        return this.state == LevelHandler.LevelState.intro || this.state == LevelHandler.LevelState.playing;
      }
    }

    [ContentSerializerIgnore]
    public LevelHandler.LevelState state
    {
      get => this._state;
      set => this.ChangeState(value);
    }

    public float lastStateChange => this._lastStateChange;

    public bool isPlayingCinematicSequence => this._isPlayingCinematicSequence;

    [ContentSerializerIgnore]
    public Level currentLevel
    {
      get
      {
        if (this._currentLevel == null)
          Debug.LogError("LevelHandler> CurrentLevel is NULL");
        return this._currentLevel;
      }
      set => this._currentLevel = value;
    }

    public float globalTimeSinceLemmySpawn => this._globalTimeSinceLemmySpawn;

    public float globalLevelTime => this._globalLevelTime;

    public float globalLevelDeltaTime => this._globalLevelDeltaTime;

    public static LevelHandler Instance
    {
      get
      {
        if (LevelHandler.instance == null)
          Debug.LogError("Attempt to access instance of LevelHandler singleton earlier than Start or without it being attached to a GameObject.");
        return LevelHandler.instance;
      }
    }

    public override void Awake()
    {
      if (LevelHandler.instance != null)
        Debug.LogError("Old instance of LevelHandler. Destroying old, and Initializing new");
      LevelHandler.isLoaded = true;
      LevelHandler.instance = this;
      this.Initialize();
    }

    protected override void Destroy()
    {
      base.Destroy();
      LevelHandler.isLoaded = false;
      LevelHandler.instance = (LevelHandler) null;
    }

    public bool CheckHitUIElements() => false;

    private void Initialize()
    {
      if (this.cutscene == null)
        this.cutscene = (ILevelHandlerCutscene) UnityObject.FindObjectOfType(typeof (CutscenePlayer));
      if (this.cutscene == null)
        this.cutscene = (ILevelHandlerCutscene) UnityObject.FindObjectOfType(typeof (IntroSequence));
      this.ingameGUI = (IngameGUI) UnityObject.Instantiate((UnityObject) this.ingameGUIPrefab);
      this.levelStartCheckPoint = (LevelStartCheckPoint) UnityObject.FindObjectOfType(typeof (LevelStartCheckPoint));
      this.CreateArrayOfCheckpoints();
      this.lemmy = (Lemmy) UnityObject.Instantiate((UnityObject) this.lemmyPrefab, this.startingCheckPoint.GetSpawnPosition(), this.lemmyPrefab.transform.rotation);
      this.cam = (PathFollowCam) UnityObject.Instantiate((UnityObject) this.cameraPrefab);
      this.feedbackCam = this.cam.GUICamera;
      this.levelExit = (LevelExit) UnityObject.FindObjectOfType(typeof (LevelExit));
      PoolableText poolableText = new GameObject().AddComponent<PoolableText>();
      poolableText._guid = Guid.NewGuid().ToString();
      poolableText.gameObject.layer = 25;
      LevelHandler.Instance.library.pointFeedback = (PoolableObject) poolableText;
      this.lemmy.lemmyFollowCamera = this.cam.raycastCamera;
      this.lemmy.pathFollowCam = this.cam;
      this.cam.followObject = this.lemmy.gameObject;
      this.followObject = this.cam.gameObject;
      this.resetOnDeathObjects = UnityObject.FindObjectsOfType<ResetOnLemmyDeath>();
      this.InitializeDistanceHandling(this.followObject);
      if (GlobalManager.isLoaded)
        GlobalManager.Instance.fullscreenImageHandler.DoInstantBlackScreen();
      if (this.levelStartCheckPoint != null)
        this.lemmy.transform.position = this.levelStartCheckPoint.GetStartTweenPosition(0.0f);
      this.levelSession = (LevelSession) this.gameObject.AddComponent(typeof (LevelSession));
      this.currentLevel = GlobalManager.Instance.database.GetCurrentLevelFromSceneName(Application.loadedLevelName);
      if (this.currentLevel == null)
      {
        this.currentLevel = Level.CreateDummyLevel(Application.loadedLevelName);
        this.currentLevel.numberOfLives = GlobalManager.Instance.gameplaySettings.defaultNumberOfLives;
        this.currentLevel.levelType = this.debugLevelType;
      }
      this.levelSession.Initialize(this.currentLevel);
      this.pauseMenu = new InGamePauseMenu();
      this.pauseMenu.Init();
      this.endLevelScreen = new EndLevelScreen(new Action(this.DoEndLevelCallback));
      this.endLevelScreen.Init();
      AudioManager.Instance.FadeAllSounds(0.0f, 0.0f);
      this.ChangeState(LevelHandler.LevelState.preloading);
    }

    public override void Start()
    {
      if (!GlobalManager.isLoaded)
        return;
      this.musicController = this.GetComponent<MusicController>();
    }

    public void HandleBackButtonDown()
    {
      if (this.state != LevelHandler.LevelState.playing)
        return;
      this.ingameGUI.ToggleScoreText(false);
      Application.screenManager.AddScreen((GameScreen) this.pauseMenu);
    }

    public void ChangeStateToIntro()
    {
      this.ChangeState(LevelHandler.LevelState.cutsceneBeforeIntro);
    }

    public void ChangeState(LevelHandler.LevelState newState)
    {
      this._lastStateChange = Time.time;
      this._state = newState;
      this.lemmy.isInputLocked = true;
      this.levelSession.Pause(true);
      if (!GlobalManager.isLoaded)
        return;
      switch (newState)
      {
        case LevelHandler.LevelState.preloading:
          GlobalManager.Instance.fullscreenImageHandler.DoInstantBlackScreen();
          break;
        case LevelHandler.LevelState.preloadingDone:
          GC.Collect();
          break;
        case LevelHandler.LevelState.cutsceneBeforeIntro:
          if (this.cutscene == null)
            break;
          Debug.Log((object) "StartCutscene");
          this.cutscene.StartCutscene();
          break;
        case LevelHandler.LevelState.intro:
          this.introUpdates = 0;
          this.cam.SetBackgroundColor(this.levelTypeSettings.backgroundColor);
          if (this.currentLevel == null)
            this.lemmy.SetNumberOfLives(GlobalManager.Instance.gameplaySettings.defaultNumberOfLives);
          else
            this.lemmy.SetNumberOfLives(this.currentLevel.numberOfLives);
          this.lemmy.SpawnAt(this.startingCheckPoint);
          if (this.levelStartCheckPoint != null)
            this.lemmy.transform.position = this.levelStartCheckPoint.GetStartTweenPosition(0.0f);
          this.lemmy.ChangeState(Lemmy.State.dormantBeforeSpawn);
          if (!this.isPlayingCinematicSequence)
          {
            this.cam.ActivateClosestConnection(this.lemmy.transform.position);
            this.cam.MoveToStablePosition();
            this.UpdateAllObjectsImmediatly(this.cam.gameObject);
          }
          this.lemmy.mainBody.LookRight();
          this.lemmy.isInputLocked = false;
          this.musicController.Init(this.levelTypeSettings.audio.loopStream1, this.levelTypeSettings.audio.loopStream2, this.levelTypeSettings.audio.loopStream3, this.levelTypeSettings.audio.loopStream12, this.levelTypeSettings.audio.loopStream13, this.levelTypeSettings.audio.loopStream23, this.levelTypeSettings.audio.loopStream123);
          this.musicController.Pause();
          break;
        case LevelHandler.LevelState.playing:
          this.lemmy.isInputLocked = false;
          this.levelSession.Pause(false);
          this.lemmy.ChangeState(Lemmy.State.normalActivity);
          this.levelSession.StartTime();
          if (this.isPlayingCinematicSequence)
            break;
          this.cam.followObject = this.lemmy.gameObject;
          this.cam.FollowPathDefaultStats();
          break;
        case LevelHandler.LevelState.outro:
          this.levelSession.SaveScore();
          this.cam.PlaceObjectInViewPort(new Vector3(0.0f, 0.0f, 8f));
          break;
        case LevelHandler.LevelState.endscreen:
          this.levelSession.StopTime();
          this.endLevelScreen.callback = new Action(this.DoEndLevelCallback);
          Application.screenManager.AddScreen((GameScreen) this.endLevelScreen);
          LevelHandler.Instance.ingameGUI.ToggleScoreText(false);
          break;
        case LevelHandler.LevelState.finalizeEndScreen:
          this.cam.FollowPathDefaultStats();
          this.lemmy.BreakConnections();
          this.musicController.FadeTo(0.0f, 0.4f);
          break;
        case LevelHandler.LevelState.openNextLevel:
          GlobalManager.Instance.OpenNextLevel();
          break;
        case LevelHandler.LevelState.gameWonEndScreenOpen:
          this.levelSession.SaveScore();
          this.levelSession.StopTime();
          this.cam.PlaceObjectInViewPort(new Vector3(-5f, 0.0f, 11f));
          this.endLevelScreen.callback = new Action(this.DoEndLevelGameWonCallback);
          Application.screenManager.AddScreen((GameScreen) this.endLevelScreen);
          break;
      }
    }

    private int CountPickupsInLevel()
    {
      int length = UnityObject.FindObjectsOfType(typeof (PointPickup)).Length;
      foreach (BaseCreature baseCreature in UnityObject.FindObjectsOfType<BaseCreature>())
        length += baseCreature.numberOfPointPickups;
      return length;
    }

    public override void FixedUpdate()
    {
      this._globalLevelTime += Time.deltaTime;
      this._globalLevelDeltaTime = Time.deltaTime;
      this._globalTimeSinceLemmySpawn += Time.deltaTime;
      if (this.state == LevelHandler.LevelState.preloading)
        return;
      this.UpdateTurnOffAtDistanceObjects(this.followObject);
    }

    public override void Update()
    {
      if (!GlobalManager.isLoaded)
        return;
      switch (this.state)
      {
        case LevelHandler.LevelState.preloading:
          this.DoStatePreloadingUpdate();
          break;
        case LevelHandler.LevelState.preloadingDone:
          this.DoStatePreloadingDoneUpdate();
          break;
        case LevelHandler.LevelState.cutsceneBeforeIntro:
          if (this.cutscene != null)
          {
            if (this.cutscene.GetCutsceneState() != LevelHandler.CutsceneState.done)
              break;
            this.ChangeState(LevelHandler.LevelState.intro);
            break;
          }
          this.ChangeState(LevelHandler.LevelState.intro);
          break;
        case LevelHandler.LevelState.intro:
          this.DoStateIntroUpdate();
          break;
        case LevelHandler.LevelState.playing:
          this.DoStatePlayingUpdate();
          break;
        case LevelHandler.LevelState.respawning:
          this.DoStateRespawnUpdate();
          break;
        case LevelHandler.LevelState.outro:
          this.DoStateOutroUpdate();
          break;
        case LevelHandler.LevelState.endscreen:
          this.DoStateEndscreenUpdate();
          break;
        case LevelHandler.LevelState.finalizeEndScreen:
          this.DoStateFinalizeEndScreenUpdate();
          break;
      }
    }

    public void SkipLevel() => this.StartLevelExit();

    public void NextCheckpoint()
    {
      int num = 0;
      for (int index = 0; index < this.checkPoints.Length; ++index)
      {
        if (this.lastSpawnedAtCheckpoint == this.checkPoints[index])
        {
          num = index;
          break;
        }
      }
      CheckPoint checkPoint = this.checkPoints[(num + 1) % this.checkPoints.Length];
      this.ActivateCheckpoint(checkPoint);
      this.lastActivatedCheckpoint = checkPoint;
      this.RespawnAtLastCheckpoint();
      this.DoStateRespawnUpdate();
      this.DoRespawnAtLastCheckpoint();
    }

    private void DoFramerateCheck()
    {
      this.currentFramerate = 1f / Time.deltaTime;
      if (this.doCriticalFrameRateDropCheck)
      {
        if (this.criticalFrameRateDropCounter > this.framesBetweenCriticalFrameChecks && (double) this.currentFramerate < (double) this.criticalFrameRateDropLimit)
          this.criticalFrameRateDropCounter = 0;
        ++this.criticalFrameRateDropCounter;
      }
      this.averageFrameRate = (float) (((double) this.averageFrameRate * (double) this.frameRateCalcCnt + (double) this.currentFramerate) / ((double) this.frameRateCalcCnt + 1.0));
      ++this.frameRateCalcCnt;
      this.lastFrames[this.frameIndex] = this.currentFramerate;
      this.averageFramerate10Frames = 0.0f;
      for (int index = 0; index < this.lastFrames.Length; ++index)
        this.averageFramerate10Frames += this.lastFrames[index];
      this.averageFramerate10Frames /= (float) this.lastFrames.Length;
      this.frameIndex = (this.frameIndex + 1) % this.lastFrames.Length;
    }

    protected void PreloadLevelRelevantPoolableObjects()
    {
      ObjectPool.Instance.Grow((PoolableObject) this.levelTypeSettings.OnTentacleConnectParticle, 8);
      ObjectPool.Instance.Grow((PoolableObject) this.levelTypeSettings.OnTentacleBounceParticle, 5);
      ObjectPool.Instance.Grow((PoolableObject) this.levelTypeSettings.OnTentacleShieldParticle, 5);
      ObjectPool.Instance.Grow((PoolableObject) this.enemyDeathPickup, 25);
      ObjectPool.Instance.Grow((PoolableObject) this.enemyDeathPickup.GetComponent<MoveToAndPullLemmy>().miniTentaclePrefab, 7);
      ObjectPool.Instance.Grow(this.library.pointFeedback, 25);
      ObjectPool.Instance.Grow(this.lemmy.createOnLowDamage, 2);
      ObjectPool.Instance.Grow(this.lemmy.createOnMediumDamage, 2);
      ObjectPool.Instance.Grow(this.lemmy.createOnHighDamage, 2);
      ObjectPool.Instance.Grow(this.lemmy.createOnDeath, 1);
      foreach (BaseCreature baseCreature in UnityObject.FindObjectsOfType<BaseCreature>())
        ObjectPool.Instance.Grow(baseCreature.createOnDeath, 1, 2);
    }

    protected void DoStatePreloadingUpdate()
    {
      this.levelTypeSettings = (LevelTypeSettings) UnityObject.Instantiate((UnityObject) this.GetLevelTypeSettings(this.currentLevel.levelType));
      this.lemmy.Initialize(this.levelTypeSettings.lemmyTentacleMaterial, this.levelTypeSettings.lemmyGlowMaterial);
      this.levelSession.totalNumberOfPickups = this.CountPickupsInLevel();
      this.PreloadLevelRelevantPoolableObjects();
      this.ChangeState(LevelHandler.LevelState.preloadingDone);
    }

    protected void DoStatePreloadingDoneUpdate()
    {
    }

    protected void DoStateIntroUpdate()
    {
      ++this.introUpdates;
      if (this.introUpdates < 4)
        return;
      if (this.introUpdates == 4)
      {
        this.musicController.Resume();
        this.musicController.FadeFromTo(0.0f, 1f, 0.85f);
        GlobalManager.Instance.fullscreenImageHandler.FadeFromBlack(0.85f);
        AudioManager.Instance.TurnOnAllSounds(AudioSettings.AudioType.SoundEffect);
        AudioManager.Instance.FadeAllSounds(1f, 0.85f);
      }
      if (InputHandler.Instance.GetShootTentacle() || (double) Time.time > (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.levelIntroDuration)
      {
        this.ChangeState(LevelHandler.LevelState.playing);
      }
      else
      {
        float _moveFraction = Mathf.Min(1f, (Time.time - this.lastStateChange) / GlobalManager.Instance.gameplaySettings.levelIntroDuration);
        if (this.levelStartCheckPoint == null)
          return;
        this.lemmy.Push((this.levelStartCheckPoint.GetStartTweenPosition(_moveFraction) - this.lemmy.transform.position) * 350f * Time.deltaTime);
      }
    }

    protected void DoStatePlayingUpdate()
    {
    }

    protected void DoStateOutroUpdate()
    {
      if ((double) Time.time <= (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.levelOutroDuration)
        return;
      this.state = LevelHandler.LevelState.endscreen;
    }

    protected void DoStateEndscreenUpdate()
    {
    }

    protected void DoStateFinalizeEndScreenUpdate()
    {
      if ((double) Time.time > (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.finalizeEndScreenDuration)
      {
        if (this.currentLevel.id == -1)
        {
          GlobalManager.Instance.SaveToDisk();
          GlobalManager.Instance.OpenMainMenu();
        }
        else
        {
          GlobalManager.Instance.SaveToDisk();
          this.ChangeState(LevelHandler.LevelState.openNextLevel);
        }
      }
      else
      {
        if ((double) Time.time > (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.finalizeEndScreenDuration * 0.5 && GlobalManager.Instance.fullscreenImageHandler.fadeToBlackState == FullScreenImageHandler.FadeToBlackState.clearScreen)
          GlobalManager.Instance.fullscreenImageHandler.FadeToBlack(GlobalManager.Instance.gameplaySettings.finalizeEndScreenDuration * 0.5f);
        float _moveFraction = Mathf.Min(1f, (Time.time - this.lastStateChange) / GlobalManager.Instance.gameplaySettings.finalizeEndScreenDuration);
        if (this.levelExit == null)
          return;
        this.lemmy.Push((this.levelExit.GetMoveOutTweenPosition(_moveFraction) - this.lemmy.transform.position) * 5f);
      }
    }

    protected void DoStateRespawnUpdate()
    {
      if (GlobalManager.Instance.fullscreenImageHandler.fadeToBlackState == FullScreenImageHandler.FadeToBlackState.clearScreen && (double) Time.time > (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.timeToRespawn - 0.34999999403953552)
        GlobalManager.Instance.fullscreenImageHandler.FadeToBlack(0.35f);
      if ((double) Time.time <= (double) this.lastStateChange + (double) GlobalManager.Instance.gameplaySettings.timeToRespawn)
        return;
      this.DoRespawnAtLastCheckpoint();
    }

    private void CreateArrayOfCheckpoints()
    {
      if (this.levelStartCheckPoint == null)
        Debug.LogError("No levelStartCheckPoint set. This is BAD!!! ALL LEVELS MUST HAVE A START CHECKPOINT!!");
      CheckPoint[] objectsOfType = UnityObject.FindObjectsOfType<CheckPoint>();
      this.checkPoints = new CheckPoint[objectsOfType.Length];
      for (int index = 0; index < objectsOfType.Length; ++index)
        this.checkPoints[index] = objectsOfType[index];
      if (this.startingCheckPoint != null)
        return;
      this.startingCheckPoint = (CheckPoint) this.levelStartCheckPoint;
    }

    public void DoOnLemmyDeath()
    {
      for (int index = 0; index < this.resetOnDeathObjects.Length; ++index)
      {
        if (this.resetOnDeathObjects[index].resetTiming == ResetOnLemmyDeath.ResetTiming.OnDeath)
          this.resetOnDeathObjects[index].Reset();
      }
    }

    public void DoBeforeLemmyRespawn()
    {
      GC.Collect();
      for (int index = 0; index < this.resetOnDeathObjects.Length; ++index)
      {
        if (this.resetOnDeathObjects[index].resetTiming == ResetOnLemmyDeath.ResetTiming.BeforeRespawn)
          this.resetOnDeathObjects[index].Reset();
      }
    }

    public void DoAfterLemmyRespawn()
    {
      for (int index = 0; index < this.resetOnDeathObjects.Length; ++index)
      {
        if (this.resetOnDeathObjects[index].resetTiming == ResetOnLemmyDeath.ResetTiming.AfterRespawn)
          this.resetOnDeathObjects[index].Reset();
      }
    }

    public void ActivateCheckpoint(CheckPoint _checkpoint)
    {
      if (this.activatedCheckPoints.Contains(_checkpoint))
        return;
      this.activatedCheckPoints.Add(_checkpoint);
      if (!_checkpoint.isCheckPointActive)
        _checkpoint.ActivateCheckPoint();
      this.lastActivatedCheckpoint = _checkpoint;
    }

    public CheckPoint GetStartingCheckPoint()
    {
      if ((bool) (UnityObject) this.startingCheckPoint)
        return this.startingCheckPoint;
      if (this.checkPoints.Length <= 0)
        return (CheckPoint) null;
      this.startingCheckPoint = this.checkPoints[0];
      return this.startingCheckPoint;
    }

    public void RespawnAtLastCheckpoint() => this.ChangeState(LevelHandler.LevelState.respawning);

    private void DoRespawnAtLastCheckpoint()
    {
      this.DoBeforeLemmyRespawn();
      GlobalManager.Instance.fullscreenImageHandler.FadeFromBlack(0.35f);
      this.RespawnAtCheckpoint(this.GetLastActivatedCheckPoint());
      this.ChangeState(LevelHandler.LevelState.playing);
      this.cam.MoveToStablePosition();
      this.UpdateAllObjectsImmediatly(this.followObject);
      this._globalTimeSinceLemmySpawn = 0.0f;
      this.DoAfterLemmyRespawn();
    }

    private void RespawnAtCheckpoint(CheckPoint _checkPoint)
    {
      if (this.lemmyHunter != null)
      {
        if (this.GetLastActivatedCheckPoint().connectionedNode != null)
          this.lemmyHunter.GotoNode(_checkPoint.connectionedNode);
        else
          this.lemmyHunter.Reset();
      }
      this.lemmy.SpawnAt(_checkPoint);
      this.cam.MoveToStablePosition();
      this.UpdateAllObjectsImmediatly(this.cam.gameObject);
    }

    public CheckPoint GetLastActivatedCheckPoint() => this.lastActivatedCheckpoint;

    public void RestartLevel()
    {
      GlobalManager.Instance.OpenLevel(GlobalManager.Instance.currentLevel);
    }

    public void StartLevelExit() => this.ChangeState(LevelHandler.LevelState.outro);

    private void DoEndLevelCallback()
    {
      this.SubmitStatistics();
      this.ChangeState(LevelHandler.LevelState.finalizeEndScreen);
    }

    private void DoEndLevelGameWonCallback()
    {
      this.SubmitStatistics();
      this.ChangeState(LevelHandler.LevelState.gameWonAfterEndScreen);
    }

    private void SubmitStatistics()
    {
    }

    private void DoGameOverCallback() => GlobalManager.Instance.RestartCurrentLevel();

    public Quaternion GetCameraRelativeRotation(Vector3 direction)
    {
      return Quaternion.LookRotation(LevelHandler.Instance.feedbackCam.transform.TransformDirection(direction));
    }

    private LevelTypeSettings GetLevelTypeSettings(Level.LevelType _type)
    {
      switch (_type)
      {
        case Level.LevelType.veins:
          return ((GameObject) Resources.Load("LevelSettings/VEINS_settings")).GetComponent<LevelTypeSettings>();
        case Level.LevelType.intestines:
          return ((GameObject) Resources.Load("LevelSettings/INTESTINES_settings")).GetComponent<LevelTypeSettings>();
        case Level.LevelType.brain:
          return ((GameObject) Resources.Load("LevelSettings/BRAIN_settings")).GetComponent<LevelTypeSettings>();
        case Level.LevelType.desatGreen:
          return ((GameObject) Resources.Load("LevelSettings/DESATGREEN_settings")).GetComponent<LevelTypeSettings>();
        case Level.LevelType.petriDish:
          return ((GameObject) Resources.Load("LevelSettings/PETRIDISH_settings")).GetComponent<LevelTypeSettings>();
        default:
          return (LevelTypeSettings) null;
      }
    }

    public enum LevelState
    {
      idle,
      preloading,
      preloadingDone,
      cutsceneBeforeIntro,
      intro,
      playing,
      respawning,
      gameover,
      outro,
      endscreen,
      finalizeEndScreen,
      openNextLevel,
      gameWonBeforeEndScreen,
      gameWonEndScreenOpen,
      gameWonAfterEndScreen,
    }

    public enum CutsceneState
    {
      notStarted,
      running,
      done,
    }
  }
}
