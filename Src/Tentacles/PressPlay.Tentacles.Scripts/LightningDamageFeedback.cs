// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.LightningDamageFeedback
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using PressPlay.FFWD;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class LightningDamageFeedback : DamageFeedback
  {
    public LightningRenderer lightningFeedbackRenderer;
    private Transform startPosition;
    public float feedbackTime = 0.35f;
    private float feedbackStartTime;
    private bool showingFeedback;

    public override void Start()
    {
      this.startPosition = new GameObject()
      {
        name = "lightning feedback start position"
      }.transform;
      this.startPosition.parent = this.transform;
      this.lightningFeedbackRenderer.ToggleOn(false);
    }

    public override void DoOnHitLemmy(Vector3 _hitDir, Vector3 _position)
    {
      this.startPosition.position = _position;
      this.lightningFeedbackRenderer.positions = new Transform[2]
      {
        this.startPosition,
        LevelHandler.Instance.lemmy.transform
      };
      this.lightningFeedbackRenderer.ToggleOn(true);
      this.showingFeedback = true;
      this.feedbackStartTime = Time.time;
    }

    public override void Update()
    {
      if (!this.showingFeedback || (double) Time.time <= (double) this.feedbackStartTime + (double) this.feedbackTime)
        return;
      this.showingFeedback = false;
      this.lightningFeedbackRenderer.ToggleOn(false);
    }
  }
}
