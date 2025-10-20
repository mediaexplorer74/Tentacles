// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.ChallengeBar
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class ChallengeBar : MonoBehaviour
  {
    public Vector3 hiddenPosition;
    public Vector3 visiblePosition;
    public iTween.EaseType easeShow = iTween.EaseType.spring;
    public iTween.EaseType easeHide;
    public float timeToShow = 0.35f;
    public float timeToHide = 0.3f;
    private string nextText;
    private Color nextColor;
    private TextControl textControl;
    private float time;
    private bool isVisible;
    private bool isTimedToHide;
    private bool isInitialized;

    public override void Start() => this.Initialize();

    private void Initialize()
    {
      if (this.isInitialized)
        return;
      this.isInitialized = true;
      this.hiddenPosition = new Vector3(8f, 10f, -150f);
      this.visiblePosition = new Vector3(8f, 10f, -4f);
      this.textControl = new TextControl("", GUIAssets.berlinsSans40, Color.white);
      this.textControl.transform.parent = this.transform;
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

    public void Show(string text, Color color, float duration)
    {
      this.time = duration;
      this.isTimedToHide = true;
      this.Show(text, color);
    }

    public void Show(string text, Color color)
    {
      this.Initialize();
      if (!this.isVisible)
      {
        this.textControl.SetColor(color);
        this.textControl.text = text;
        iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.visiblePosition, (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeShow));
        this.isVisible = true;
      }
      else
      {
        this.nextColor = color;
        this.nextText = text;
        iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.hiddenPosition, (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeShow, (object) "oncomplete", (object) "HideShowMedio"));
      }
    }

    public void Hide()
    {
      iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.hiddenPosition, (object) "time", (object) this.timeToHide, (object) "easetype", (object) this.easeHide));
      this.isVisible = false;
    }

    public void HideShowMedio()
    {
      this.textControl.SetColor(this.nextColor);
      this.textControl.text = this.nextText;
      iTween.MoveTo(this.gameObject, iTween.Hash((object) "position", (object) this.visiblePosition, (object) "time", (object) this.timeToShow, (object) "easetype", (object) this.easeShow));
      this.isVisible = true;
    }

    public void ShakeText()
    {
      iTween.ShakePosition(this.gameObject, iTween.Hash((object) "amount", (object) new Vector3(2f, 2f, 0.0f), (object) "time", (object) 1));
    }
  }
}
