// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.TrialModeManager
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class TrialModeManager
  {
    private static TrialModeManager instance;
    private volatile bool trialMode = true;
    //private Thread trialThread;

    public static TrialModeManager Instance
    {
      get
      {
        if (TrialModeManager.instance == null)
          TrialModeManager.instance = new TrialModeManager();
        return TrialModeManager.instance;
      }
    }

    public bool TrialMode => this.trialMode;

    private TrialModeManager()
    {
      this.ForcedUpdateTrial();
      if (!this.trialMode)
        return;
      //this.trialThread = new Thread(new ThreadStart(this.CheckTrial));
      //this.trialThread.IsBackground = true;
      //this.trialThread.Start();
    }

    public void ForcedUpdateTrial()
    {
      if (!this.trialMode)
        return;
      // Guide.IsTrialMode is not available in MonoGame, so we skip this functionality
      // this.trialMode = Guide.IsTrialMode;
      this.trialMode = false; // Assume full version for now
    }

    private void CheckTrial()
    {
      while (this.trialMode)
      {
        // Guide.IsTrialMode is not available in MonoGame, so we skip this functionality
        // this.trialMode = Guide.IsTrialMode;
        this.trialMode = false; // Assume full version for now
        
       //if (this.trialMode)
       //  Thread.Sleep(TimeSpan.FromSeconds(3.0));
      }
    }
  }
}
