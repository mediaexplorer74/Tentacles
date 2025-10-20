// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ChallengeHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Audio;
using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ChallengeHandler : TTBaseBehavior
  {
    private int _bonusPointsPending = -1;
    private int pickupCombo;
    private int pickups;
    private int kills;
    private float damage;
    private float time;
    private Challenge currentChallenge;
    private Challenge succededChallenge;
    public AudioWrapper sndStart;
    public AudioWrapper sndFail;
    public AudioWrapper sndSuccess;
    public AudioWrapper sndLoop;
    private AudioClip sndCountFinished;
    private AudioClip sndCount;
    private AudioObject sndCountObject;
    private bool isDoingRaceCountdownBonus;
    private float raceBonusCountdownStartTime;
    private float raceBonusCountdownDuration;
    private float raceBonusCountdownScoreFraction;
    private bool challengeIsRunning;

    public int bonusPointsPending => this._bonusPointsPending;

    public override void Start()
    {
      this.sndCountFinished = new AudioClip(Application.Load<SoundEffect>("Sounds/drop"));
      this.sndCount = new AudioClip(Application.Load<SoundEffect>("Sounds/menu/EndLevelScreen/countup7"));
      this.sndCountObject = AudioManager.Instance.Add(new AudioSettings(this.sndCount, 1f, true), "menu", 1);
    }

    public override void Update()
    {
      if (this.challengeIsRunning)
      {
        this.time += Time.deltaTime;
        if (this.currentChallenge.type == Challenge.ChallengeType.raceChallenge)
          LevelHandler.Instance.ingameGUI.guiBottomBar.SetProgress((float) (1.0 - (double) this.time / (double) this.currentChallenge.duration));
        if ((double) this.currentChallenge.duration > 0.0 && (double) this.time > (double) this.currentChallenge.duration)
          this.StopChallenge();
      }
      if (this.succededChallenge == null || this.succededChallenge.type != Challenge.ChallengeType.raceChallenge || !this.isDoingRaceCountdownBonus)
        return;
      float num1 = (float) (1.0 - ((double) Time.time - (double) this.raceBonusCountdownStartTime) / (double) this.raceBonusCountdownDuration);
      if ((double) Time.time > (double) this.raceBonusCountdownStartTime + (double) this.raceBonusCountdownDuration)
      {
        LevelHandler.Instance.ingameGUI.guiBottomBar.DelayedHide(0.5f);
        this.isDoingRaceCountdownBonus = false;
        AudioManager.Instance.Play(this.sndCountFinished);
        this.sndCountObject.Stop();
        this.AwardSuccededChallengeBonus();
      }
      else
      {
        if ((double) num1 < 0.0)
          num1 = 0.0f;
        int num2 = (int) ((double) this.succededChallenge.bonus + (double) this.succededChallenge.bonus * (double) this.raceBonusCountdownScoreFraction * (1.0 - (double) num1));
        LevelHandler.Instance.ingameGUI.guiBottomBar.SetProgress(num1 * this.raceBonusCountdownScoreFraction);
        LevelHandler.Instance.ingameGUI.guiBottomBar.SetText(LocalisationManager.Instance.GetString("menu_timebonus") + " " + num2.ToString());
      }
    }

    public void RegisterPickup()
    {
      ++this.pickups;
      if (this.currentChallenge == null || this.currentChallenge.type != Challenge.ChallengeType.getAllPickups || this.currentChallenge.numberOfPickups == 0)
        return;
      LevelHandler.Instance.ingameGUI.guiBottomBar.AnimateTo((float) this.pickups / (float) this.currentChallenge.numberOfPickups);
      if (this.pickups != this.currentChallenge.numberOfPickups)
        return;
      this.StopChallenge();
    }

    public void RegisterPickupCombo(int combo)
    {
      this.pickupCombo = Mathf.Max(this.pickupCombo, combo);
      if (this.currentChallenge == null || this.currentChallenge.type != Challenge.ChallengeType.pickupComboChallenge)
        return;
      this.StopChallenge();
    }

    public void RegisterKill()
    {
      ++this.kills;
      if (this.currentChallenge == null || this.currentChallenge.type != Challenge.ChallengeType.killAllEnemies || !this.currentChallenge.areAllEnemiesKilled)
        return;
      this.StopChallenge();
    }

    public void RegisterDamage(float value)
    {
      this.damage += value;
      if (this.currentChallenge == null || this.currentChallenge.type != Challenge.ChallengeType.noDamageChallenge)
        return;
      this.StopChallenge();
    }

    public void StartChallenge(Challenge challenge)
    {
      this.Reset();
      this.challengeIsRunning = true;
      this.currentChallenge = challenge;
      int num = this.currentChallenge.startText != "" ? 1 : 0;
      switch (this.currentChallenge.type)
      {
        case Challenge.ChallengeType.raceChallenge:
          LevelHandler.Instance.ingameGUI.guiBottomBar.SetProgress(1f);
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.startText), Color.green, true);
          break;
        case Challenge.ChallengeType.getAllPickups:
          LevelHandler.Instance.ingameGUI.guiBottomBar.SetProgress(0.0f);
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.startText), Color.green, true);
          break;
        default:
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.startText), Color.green, false);
          break;
      }
      this.sndStart.PlaySound();
      this.sndLoop.PlaySound();
    }

    public void StopChallenge()
    {
      if (this.currentChallenge == null)
        return;
      bool flag = false;
      this.challengeIsRunning = false;
      switch (this.currentChallenge.type)
      {
        case Challenge.ChallengeType.raceChallenge:
          if ((double) this.time <= (double) this.currentChallenge.duration)
          {
            this._bonusPointsPending = this.currentChallenge.bonus + (int) (this.time / this.currentChallenge.duration * (float) this.currentChallenge.bonus);
            this.raceBonusCountdownScoreFraction = (float) (1.0 - (double) this.time / (double) this.currentChallenge.duration);
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.successText), Color.green, 2f, true, (GuiBarShowSettings.DoOnTransition) null, (GuiBarShowSettings.DoOnTransition) null);
            LevelHandler.Instance.ingameGUI.guiBottomBar.AddToQueue(new GuiBarShowSettings(LocalisationManager.Instance.GetString("menu_timebonus") + " " + this.currentChallenge.bonus.ToString(), Color.green, -1f, true, (GuiBarShowSettings.DoOnTransition) null, new GuiBarShowSettings.DoOnTransition(this.SpeedChallengeCompletedStartBonusCount)));
            flag = true;
            break;
          }
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.failText), Color.red, 2f);
          break;
        case Challenge.ChallengeType.pickupComboChallenge:
          if (this.pickupCombo >= this.currentChallenge.numberOfPickups)
          {
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.successText), Color.green, 2f);
            this._bonusPointsPending = this.currentChallenge.bonus;
            LevelHandler.Instance.ingameGUI.guiBottomBar.AddToQueue(new GuiBarShowSettings(LocalisationManager.Instance.GetString("menu_bonus") + " " + this.currentChallenge.bonus.ToString(), Color.green, 2f, false, (GuiBarShowSettings.DoOnTransition) null, new GuiBarShowSettings.DoOnTransition(this.AwardSuccededChallengeBonus)));
            flag = true;
            break;
          }
          if (this.currentChallenge.failText != "")
          {
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.failText), Color.red, 2f);
            break;
          }
          LevelHandler.Instance.feedback.challengeBar.Hide();
          break;
        case Challenge.ChallengeType.noDamageChallenge:
          if ((double) this.damage == 0.0)
          {
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.successText), Color.green, 2f);
            this._bonusPointsPending = this.currentChallenge.bonus;
            LevelHandler.Instance.ingameGUI.guiBottomBar.AddToQueue(new GuiBarShowSettings(LocalisationManager.Instance.GetString("menu_bonus") + " " + this.currentChallenge.bonus.ToString(), Color.green, 2f, false, (GuiBarShowSettings.DoOnTransition) null, new GuiBarShowSettings.DoOnTransition(this.AwardSuccededChallengeBonus)));
            flag = true;
            break;
          }
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.failText), Color.red, 2f);
          break;
        case Challenge.ChallengeType.getAllPickups:
          if (this.pickups >= this.currentChallenge.numberOfPickups)
          {
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.successText), Color.green, 2f);
            this._bonusPointsPending = this.currentChallenge.bonus;
            LevelHandler.Instance.ingameGUI.guiBottomBar.AddToQueue(new GuiBarShowSettings(this.currentChallenge.bonus.ToString(), Color.green, 2f, false, (GuiBarShowSettings.DoOnTransition) null, new GuiBarShowSettings.DoOnTransition(this.AwardSuccededChallengeBonus)));
            flag = true;
            break;
          }
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.failText), Color.red, 2f);
          break;
        case Challenge.ChallengeType.killAllEnemies:
          if (this.currentChallenge.areAllEnemiesKilled)
          {
            LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.successText), Color.green, 2f);
            this._bonusPointsPending = this.currentChallenge.bonus;
            LevelHandler.Instance.ingameGUI.guiBottomBar.AddToQueue(new GuiBarShowSettings(this.currentChallenge.bonus.ToString(), Color.green, 2f, false, (GuiBarShowSettings.DoOnTransition) null, new GuiBarShowSettings.DoOnTransition(this.AwardSuccededChallengeBonus)));
            flag = true;
            break;
          }
          LevelHandler.Instance.ingameGUI.guiBottomBar.Show(this.__(this.currentChallenge.failText), Color.red, 2f);
          break;
      }
      if (flag)
      {
        LevelHandler.Instance.levelSession.RegisterCompletedChallenge(this.currentChallenge.type);
        this.succededChallenge = this.currentChallenge;
        this.sndSuccess.PlaySound();
      }
      else
        this.sndFail.PlaySound();
      this.sndLoop.Stop();
      this.Reset();
    }

    public void AwardSuccededChallengeBonus()
    {
      if (this.bonusPointsPending == -1)
        return;
      LevelHandler.Instance.levelSession.AddPointToScore(this.bonusPointsPending, Vector3.zero, false, false);
      this._bonusPointsPending = -1;
    }

    public void SpeedChallengeCompletedStartBonusCount()
    {
      this.sndCountObject.Play();
      this.isDoingRaceCountdownBonus = true;
      this.raceBonusCountdownDuration = this.raceBonusCountdownScoreFraction * 2.5f;
      this.raceBonusCountdownStartTime = Time.time;
    }

    private void Reset()
    {
      this.currentChallenge = (Challenge) null;
      this.pickupCombo = 0;
      this.pickups = 0;
      this.kills = 0;
      this.damage = 0.0f;
      this.time = 0.0f;
    }
  }
}
