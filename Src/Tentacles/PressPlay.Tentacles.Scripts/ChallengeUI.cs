// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ChallengeUI
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI.Controls;
using System.Collections.Generic;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ChallengeUI : MonoBehaviour
  {
    [ContentSerializerIgnore]
    public UIProgressBar progressBar;
    [ContentSerializerIgnore]
    private Control uiContainer;
    [ContentSerializerIgnore]
    private TextControl textControl;
    public Vector3 hiddenPosition;
    public Vector3 visiblePosition;
    public iTween.EaseType easeShow = iTween.EaseType.spring;
    public iTween.EaseType easeHide;
    public float timeToShow = 0.35f;
    public float timeToHide = 0.3f;
    private GuiBarShowSettings nextSettings;
    public float speedToUpdate = 1f;
    private float time;
    private bool isVisible;
    private bool hidingBeforeShowing;
    private bool isTimedToHide;
    private bool isInitialized;
    private float progress;
    private AudioObject sndSwooshShow;
    private AudioObject sndSwooshHide;
    private List<GuiBarShowSettings> queue = new List<GuiBarShowSettings>();
    private GuiBarShowSettings currentSettings;

    public override void Start() => this.Initiatize();

    private void Initiatize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      this.uiContainer = new Control();
      this.uiContainer.size = new Vector2(800f, 69f);
      this.uiContainer.transform.parent = this.transform;
      AudioClip clip1 = new AudioClip(Application.Load<SoundEffect>("Sounds/dropSwoosh"));
      AudioClip clip2 = new AudioClip(Application.Load<SoundEffect>("Sounds/dropSwoosh"));
      this.sndSwooshShow = AudioManager.Instance.Add(new AudioSettings(clip1, 1f, false), "menu", 1);
      this.sndSwooshHide = AudioManager.Instance.Add(new AudioSettings(clip2, 1f, false), "menu", 1);
      ImageControl child = new ImageControl(Application.Load<Texture2D>("Textures/Menu/misc_assets"), SpritePositions.challengeBarBackground);
      this.textControl = new TextControl("", GUIAssets.berlinsSans40, Color.white);
      this.textControl.transform.localScale *= 0.75f;
      this.textControl.InvalidateAutoSize();
      this.progressBar = new UIProgressBar(new ImageControl(Application.Load<Texture2D>("Textures/Menu/misc_assets"), SpritePositions.challengeProgressBarBackground), new ImageControl(Application.Load<Texture2D>("Textures/Menu/misc_assets"), SpritePositions.challengeProgressBarForeground));
      this.uiContainer.AddChild((Control) child);
      this.uiContainer.AddChild((Control) this.textControl);
      this.uiContainer.AddChild((Control) this.progressBar);
      child.AlignCenter();
      this.textControl.AlignCenter();
      this.progressBar.AlignCenter(new Vector2(0.0f, -23f));
      this.hiddenPosition = new Vector3(0.0f, 0.0f, -120f);
      this.visiblePosition = new Vector3(0.0f, 0.0f, 0.0f);
      this.transform.position = this.hiddenPosition;
    }

    public override void Update()
    {
      if (!this.isVisible || !this.isTimedToHide)
        return;
      if ((double) this.time > 0.0)
        this.time -= Time.deltaTime;
      else
        this.Hide();
    }

    public void AddToQueue(GuiBarShowSettings settings)
    {
      if (this.isVisible || this.hidingBeforeShowing || this.queue.Count > 0)
        this.queue.Add(settings);
      else
        this.Show(settings);
    }

    public void Show(GuiBarShowSettings settings)
    {
      if (settings.doOnShow != null)
        settings.doOnShow();
      this.Show(settings.text, settings.color, settings.duration, settings.showProgressBar, settings.doOnShow, settings.doOnTransitionInComplete);
    }

    public void Show(string text, Color color, float duration)
    {
      this.Show(text, color, duration, false, (GuiBarShowSettings.DoOnTransition) null, (GuiBarShowSettings.DoOnTransition) null);
    }

    public void Show(string text, Color color, bool showProgressBar)
    {
      this.Show(text, color, -1f, showProgressBar, (GuiBarShowSettings.DoOnTransition) null, (GuiBarShowSettings.DoOnTransition) null);
    }

    public void Show(
      string text,
      Color color,
      float duration,
      bool showProgressBar,
      GuiBarShowSettings.DoOnTransition doOnShow,
      GuiBarShowSettings.DoOnTransition doOnTransitionInComplete)
    {
      if ((double) duration == -1.0)
      {
        this.isTimedToHide = false;
      }
      else
      {
        this.time = duration;
        this.isTimedToHide = true;
      }
      if (!this.isVisible)
      {
        this.sndSwooshShow.Play();
        this.currentSettings = new GuiBarShowSettings(text, color, duration, showProgressBar, doOnShow, doOnTransitionInComplete);
        this.textControl.text = text;
        this.textControl.SetColor(color);
        this.textControl.AlignCenter();
        this.textControl.transform.localPosition = this.textControl.transform.localPosition + new Vector3(0.0f, 0.0f, (float) ((double) this.textControl.size.y / 2.0 - 20.0));
        if (showProgressBar)
        {
          this.progressBar.gameObject.SetActiveRecursively(true);
          iTween.MoveTo(this.gameObject, iTween.Hash((object) "z", (object) this.visiblePosition.z, (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeShow, (object) "oncomplete", (object) "TransitionInComplete"));
        }
        else
        {
          this.progressBar.gameObject.SetActiveRecursively(false);
          iTween.MoveTo(this.gameObject, iTween.Hash((object) "z", (object) (float) ((double) this.visiblePosition.z - 22.0), (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeShow, (object) "oncomplete", (object) "TransitionInComplete"));
        }
        this.isVisible = true;
      }
      else
      {
        this.hidingBeforeShowing = true;
        this.sndSwooshHide.Play();
        this.nextSettings = new GuiBarShowSettings(text, color, duration, showProgressBar, doOnShow, doOnTransitionInComplete);
        iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.hiddenPosition, (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeHide, (object) "oncomplete", (object) "HideShowMedio"));
        this.isVisible = false;
      }
    }

    public void TransitionInComplete()
    {
      if (this.currentSettings.doOnTransitionInComplete == null)
        return;
      this.currentSettings.doOnTransitionInComplete();
    }

    public void SetText(string newText) => this.textControl.text = newText;

    public void DelayedHide(float delay)
    {
      this.time = delay;
      this.isTimedToHide = true;
    }

    public void Hide()
    {
      if (this.queue.Count > 0)
      {
        this.Show(this.queue[0]);
        this.queue.RemoveAt(0);
      }
      else
      {
        this.sndSwooshHide.Play();
        iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.hiddenPosition, (object) "time", (object) this.timeToHide, (object) "easetype", (object) this.easeHide));
        this.isVisible = false;
      }
    }

    public void HideShowMedio()
    {
      this.hidingBeforeShowing = false;
      this.Show(this.nextSettings);
    }

    public void SetProgress(float value)
    {
      this.progress = value;
      this.progressBar.SetProgress(Mathf.Clamp(value, 0.0f, 1f));
    }

    public void OnProgressUpdate(float value)
    {
      this.progress = value;
      this.progressBar.SetProgress(value);
    }

    public void AnimateTo(float value)
    {
      iTween.Stop(this.gameObject, "valuefloat");
      iTween.ValueTo(this.gameObject, iTween.Hash((object) "from", (object) this.progress, (object) "to", (object) value, (object) "onupdate", (object) "OnProgressUpdate", (object) "speed", (object) this.speedToUpdate));
    }

    public void AnimateTo(float value, float duration)
    {
      iTween.Stop(this.gameObject, "valuefloat");
      iTween.ValueTo(this.gameObject, iTween.Hash((object) "from", (object) this.progress, (object) "to", (object) value, (object) "onupdate", (object) "OnProgressUpdate", (object) "time", (object) duration));
    }
  }
}
