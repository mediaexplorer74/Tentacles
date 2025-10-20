// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CheatManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Input;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CheatManager : MonoBehaviour
  {
    private bool spaceDownLastFrame;
    private bool nDownLastFrame;
    private Ray ray;
    private RaycastHit rh;
    private float lastTimeFourFingersNotOn;
    private Vector3 swipeOffset;
    private bool swipeOffsetIsSet;
    private bool debugMoveLemmyOn;
    public CheatManager.CheatMode cheatMode;
    private bool _cheatsEnabled;

    public bool isCheatsEnabled => this._cheatsEnabled;

    public override void LateUpdate() => this.DoCheatTests();

    private void DoCheatTests()
    {
      if (!GlobalManager.isLoaded)
        return;
      switch (GlobalManager.Instance.globalState)
      {
        case GlobalManager.GlobalState.mainMenu:
          if (this.cheatMode == CheatManager.CheatMode.startWithCheatsOn && !this.isCheatsEnabled)
            this.EnableCheats();
          if (!this.isCheatsEnabled && this.cheatMode == CheatManager.CheatMode.doCheatCheck)
            this.DoMainMenuEnableCheatsTest();
          if (!this.isCheatsEnabled)
            break;
          this.DoMainMenuCheatsTest();
          break;
        case GlobalManager.GlobalState.inLevel:
          if (this.cheatMode == CheatManager.CheatMode.startWithCheatsOn && !this.isCheatsEnabled)
            this.EnableCheats();
          if (!this.isCheatsEnabled && this.cheatMode == CheatManager.CheatMode.doCheatCheck)
            this.DoMainMenuEnableCheatsTest();
          if (!this.isCheatsEnabled)
            break;
          this.DoInLevelCheatsTest();
          break;
      }
    }

    private void DoMainMenuCheatsTest()
    {
      if ((double) Time.time <= (double) this.lastTimeFourFingersNotOn + 0.30000001192092896 || this.swipeOffsetIsSet)
        return;
      this.swipeOffsetIsSet = true;
      this.swipeOffset = (Vector3) PressPlay.FFWD.Input.mousePosition;
    }

    private void DoMainMenuEnableCheatsTest()
    {
      KeyboardState state = Keyboard.GetState();
      if (!state.IsKeyDown(Keys.N) || !state.IsKeyDown(Keys.U))
        return;
      this.EnableCheats();
    }

    private void DoInLevelCheatsTest()
    {
      this.HandleDebugMoveLemmy();
      KeyboardState state = Keyboard.GetState();
      if (state.IsKeyDown(Keys.N) && !this.nDownLastFrame)
        this.NextCheckpoint();
      this.nDownLastFrame = state.IsKeyDown(Keys.N);
      if (state.IsKeyDown(Keys.C))
        this.CompleteLevel();
      if (state.IsKeyDown(Keys.K))
        this.DamageLemmy();
      if (state.IsKeyDown(Keys.Space) && !this.spaceDownLastFrame)
        DebugCameraControl.Instance.ToggleDebugCam();
      this.spaceDownLastFrame = state.IsKeyDown(Keys.Space);
    }

    private void HandleDebugMoveLemmy()
    {
      if (!this.debugMoveLemmyOn && !InputHandler.Instance.GetControlTentacle())
        return;
      this.ray = LevelHandler.Instance.cam.raycastCamera.ScreenPointToRay(InputHandler.Instance.GetInputScreenPosition());
      ScreenRayCheckHit screenRayCheckHit1 = InputHandler.Instance.ScreenRayCheck(this.ray, GlobalSettings.Instance.lemmyLayer);
      ScreenRayCheckHit screenRayCheckHit2 = InputHandler.Instance.ScreenRayCheck(this.ray, GlobalSettings.Instance.inputLayer);
      if (!this.debugMoveLemmyOn && screenRayCheckHit1.hitObjectInLayer)
        this.debugMoveLemmyOn = true;
      if (!InputHandler.Instance.GetControlTentacle())
        this.debugMoveLemmyOn = false;
      if (!this.debugMoveLemmyOn)
        return;
      LevelHandler.Instance.lemmy.BreakConnections();
      LevelHandler.Instance.lemmy.SetInvulnerable(0.5f);
      if (float.IsNaN(screenRayCheckHit2.position.magnitude) || float.IsInfinity(screenRayCheckHit2.position.magnitude))
        return;
      LevelHandler.Instance.lemmy.transform.position = Vector3.Lerp(LevelHandler.Instance.lemmy.transform.position, screenRayCheckHit2.position, 5f * Time.deltaTime);
    }

    private void EnableCheats()
    {
    }

    private void NextCheckpoint()
    {
      if (!LevelHandler.isLoaded)
        return;
      LevelHandler.Instance.NextCheckpoint();
    }

    private void CompleteLevel()
    {
      if (!LevelHandler.isLoaded)
        return;
      LevelHandler.Instance.SkipLevel();
    }

    private void DamageLemmy() => LevelHandler.Instance.lemmy.Damage(60f, Vector3.zero);

    public enum CheatMode
    {
      noCheats,
      doCheatCheck,
      startWithCheatsOn,
    }
  }
}
