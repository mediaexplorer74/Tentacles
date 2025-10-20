// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.FullScreenImageHandler
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI;
using System;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class FullScreenImageHandler : MonoBehaviour
  {
    public string[] hintScreenImages;
    public int layerToShow = 23;
    public Vector3 sizeOfPlane;
    public string clickToContinueText;
    public float blinkSpeed = 0.75f;
    private GUICamera _guiCamera;
    private GameObject plane;
    private string texture;
    private GameObject fadePlane;
    private FullScreenImageHandler.FadeToBlackState _fadeToBlackState;

    private GUICamera guiCamera
    {
      get
      {
        if (this._guiCamera == null)
        {
          GameObject gameObjectWithTag = GameObject.FindGameObjectWithTag("GUICamera");
          if (gameObjectWithTag != null)
            this._guiCamera = gameObjectWithTag.GetComponent<GUICamera>();
        }
        return this._guiCamera;
      }
    }

    public event EventHandler<EventArgs> OnFadeCompleteEvent;

    public FullScreenImageHandler.FadeToBlackState fadeToBlackState => this._fadeToBlackState;

    public override void Start()
    {
      this.sizeOfPlane = (Vector3) new Vector2((float) Camera.FullScreen.Width, (float) Camera.FullScreen.Height);
      this.CreateFadePlane();
    }

    public void ShowClickToContinue() => this.FadeText(true);

    public void DoInstantBlackScreen()
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      iTween.Stop(this.fadePlane);
      this.fadePlane.SetActiveRecursively(true);
      this.fadePlane.renderer.material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, 1f));
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.blackScreen;
    }

    public void DoInstantClearScreen()
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      iTween.Stop(this.fadePlane);
      this.fadePlane.SetActiveRecursively(true);
      this.fadePlane.renderer.material.SetColor("_Color", new Color(0.0f, 0.0f, 0.0f, 0.0f));
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.clearScreen;
    }

    public void FadeToBlack(float duration, GameObject target, string callback)
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      this.fadePlane.SetActiveRecursively(true);
      iTween.Stop(this.fadePlane);
      iTween.ColorTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "color", (object) new Color(0.0f, 0.0f, 0.0f, 1f), (object) "oncomplete", (object) "OnFadeToBlackComplete", (object) "oncompletetarget", (object) this.gameObject));
      iTween.MoveTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "position", (object) this.fadePlane.transform.position, (object) "oncomplete", (object) callback, (object) "oncompletetarget", (object) target));
      AudioManager.Instance.FadeAllSounds(0.0f, duration);
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.fadeToBlack;
    }

    public void FadeToBlack(float duration, EventHandler<EventArgs> onCompleteEvent)
    {
      this.OnFadeCompleteEvent = onCompleteEvent;
      this.FadeToBlack(duration);
    }

    public void FadeToBlack(float duration)
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      this.fadePlane.SetActiveRecursively(true);
      iTween.Stop(this.fadePlane);
      iTween.ColorTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "color", (object) new Color(0.0f, 0.0f, 0.0f, 1f)));
      iTween.MoveTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "position", (object) this.fadePlane.transform.position, (object) "oncomplete", (object) "OnFadeToBlackComplete", (object) "oncompletetarget", (object) this.gameObject));
      AudioManager.Instance.FadeAllSounds(0.0f, duration);
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.fadeToBlack;
    }

    public void FadeFromBlack(float duration, GameObject target, string callback)
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      iTween.Stop(this.fadePlane);
      iTween.ColorTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "color", (object) new Color(0.0f, 0.0f, 0.0f, 0.0f), (object) "oncomplete", (object) "OnFadeInComplete", (object) "oncompletetarget", (object) this.gameObject));
      iTween.MoveTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "position", (object) this.fadePlane.transform.position, (object) "oncomplete", (object) callback, (object) "oncompletetarget", (object) target));
      AudioManager.Instance.FadeAllSounds(1f, duration);
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.fadeFromBlack;
    }

    public void FadeFromBlack(float duration)
    {
      if (this.fadePlane == null)
        this.CreateFadePlane();
      iTween.Stop(this.fadePlane);
      iTween.ColorTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "color", (object) new Color(0.0f, 0.0f, 0.0f, 0.0f)));
      iTween.MoveTo(this.fadePlane, iTween.Hash((object) "time", (object) duration, (object) "position", (object) this.fadePlane.transform.position, (object) "oncomplete", (object) "OnFadeInComplete", (object) "oncompletetarget", (object) this.gameObject));
      AudioManager.Instance.FadeAllSounds(1f, duration);
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.fadeFromBlack;
    }

    public void OnFadeToBlackComplete()
    {
      iTween.Stop(this.fadePlane);
      if (this.OnFadeCompleteEvent != null)
      {
        this.OnFadeCompleteEvent((object) this, new EventArgs());
        this.OnFadeCompleteEvent = (EventHandler<EventArgs>) null;
      }
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.blackScreen;
    }

    public void OnFadeInComplete()
    {
      iTween.Stop(this.fadePlane);
      this.fadePlane.SetActiveRecursively(false);
      this._fadeToBlackState = FullScreenImageHandler.FadeToBlackState.clearScreen;
    }

    private void FadeText(bool state)
    {
    }

    public void ShowLastHintScreen()
    {
      if (this.texture == null)
        return;
      this.ShowTextureOnPlane();
    }

    public void ShowHintScreen(string id)
    {
      if (this.texture != null)
        this.ShowTextureOnPlane();
      else
        this.ClearScreen();
    }

    private void ShowTextureOnPlane()
    {
      this.CreateHintPlane();
      this.plane.renderer.material.mainTexture = this.texture;
    }

    private string GetRandomHintScreen()
    {
      if (this.hintScreenImages.Length > 0)
        return this.hintScreenImages[PressPlay.FFWD.Random.Range(0, this.hintScreenImages.Length)];
      Debug.LogError("We have no hintscreen images!");
      return "";
    }

    public void ClearScreen()
    {
      if (this.plane == null)
        return;
      UnityObject.Destroy((UnityObject) this.plane);
      this.plane = (GameObject) null;
      this.texture = (string) null;
    }

    private void CreateHintPlane()
    {
      if (this.plane != null)
        UnityObject.Destroy((UnityObject) this.plane);
      this.plane = this.CreatePlane(this.sizeOfPlane, this.layerToShow);
    }

    private void CreateFadePlane()
    {
      if (this.fadePlane != null)
        return;
      this.fadePlane = this.CreatePlane(this.sizeOfPlane, this.layerToShow);
    }

    private GameObject CreatePlane(Vector3 _sizeOfPlane, int _layer)
    {
      GameObject target = new GameObject("Full screen fade plane");
      target.AddComponent<UISpriteRenderer>();
      target.layer = _layer;
      target.transform.position = new Vector3(0.0f, 900f, 0.0f);
      target.transform.localScale = _sizeOfPlane;
      target.renderer.material = new Material()
      {
        mainTexture = "blank_texture"
      };
      target.renderer.material.color = Color.clear;
      target.SetActiveRecursively(false);
      UnityObject.DontDestroyOnLoad((UnityObject) target);
      return target;
    }

    public enum FadeToBlackState
    {
      clearScreen,
      fadeToBlack,
      fadeFromBlack,
      blackScreen,
    }
  }
}
