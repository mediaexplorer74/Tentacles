// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CutscenePlayer
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CutscenePlayer : MoviePlayer, ILevelHandlerCutscene
  {
    public MoviePlayer.Movies movie;
    private LevelHandler.CutsceneState cutsceneState;
    public bool isSkipable = true;

    public override void Update()
    {
      base.Update();
      if (this.cutsceneState != LevelHandler.CutsceneState.running || !this.isDonePlaying)
        return;
      this.cutsceneState = LevelHandler.CutsceneState.done;
    }

    public void StartCutscene()
    {
      this.cutsceneState = LevelHandler.CutsceneState.running;
      this.TestZuneBeforePlayingMovie(this.movie);
    }

    public LevelHandler.CutsceneState GetCutsceneState() => this.cutsceneState;
  }
}
