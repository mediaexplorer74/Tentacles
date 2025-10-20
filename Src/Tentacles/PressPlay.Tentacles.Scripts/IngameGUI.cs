// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.IngameGUI
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.Components;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class IngameGUI : MonoBehaviour
  {
    private int shownScoreGoal;
    private int currentlyShownScore;
    private float lastShownHealth;
    private float lastDelta;
    [ContentSerializerIgnore]
    public ChallengeUI guiBottomBar;
    [ContentSerializerIgnore]
    public TextControl scoreText;
    private Texture2D[] damageBorderTextures;
    private ImageControl upperBorder;
    private ImageControl lowerBorder;
    private ImageControl rightBorder;
    private ImageControl leftBorder;

    public override void Start()
    {
      this.lastShownHealth = 100f;
      this.guiBottomBar = new GameObject("ChallengeBar").AddComponent<ChallengeUI>(new ChallengeUI());
      this.scoreText = new TextControl("0", GUIAssets.berlinsSans40);
      this.scoreText.transform.localScale *= 1.25f;
      this.scoreText.position = (Vector2) new Vector3(5f, 10f, 413f);
      this.scoreText.transform.parent = this.transform;
      this.damageBorderTextures = new Texture2D[16];
      for (int index = 0; index < this.damageBorderTextures.Length; ++index)
        this.damageBorderTextures[index] = Application.Load<Texture2D>("Textures/damage_gradient" + (index + 1).ToString());
      this.upperBorder = new ImageControl(this.damageBorderTextures[7]);
      this.upperBorder.transform.parent = this.transform;
      this.lowerBorder = new ImageControl(this.damageBorderTextures[7]);
      this.lowerBorder.transform.parent = this.transform;
      this.lowerBorder.transform.position = new Vector3(800f, 0.0f, 480f);
      this.lowerBorder.transform.rotation = Quaternion.Euler(0.0f, 3.14159274f, 0.0f);
      this.rightBorder = new ImageControl(this.damageBorderTextures[7]);
      this.rightBorder.transform.parent = this.transform;
      this.rightBorder.transform.position = new Vector3(800f, 0.0f, 0.0f);
      this.rightBorder.transform.rotation = Quaternion.Euler(0.0f, 1.57079637f, 0.0f);
      this.leftBorder = new ImageControl(this.damageBorderTextures[7]);
      this.leftBorder.transform.parent = this.transform;
      this.leftBorder.transform.position = new Vector3(0.0f, 0.0f, 480f);
      this.leftBorder.transform.rotation = Quaternion.Euler(0.0f, -1.57079637f, 0.0f);
    }

    public void ToggleScoreText(bool visible)
    {
      if (visible)
        iTween.MoveTo(this.scoreText.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "x", (object) 5, (object) "ignoretimescale", (object) true));
      else
        iTween.MoveTo(this.scoreText.gameObject, iTween.Hash((object) "time", (object) 0.3f, (object) "x", (object) (-this.scoreText.bounds.Width - 15), (object) "ignoretimescale", (object) true));
    }

    public void ShowScore(int _score) => this.shownScoreGoal = _score;

    public override void Update()
    {
      this.HandleHealth();
      float num = (float) (this.shownScoreGoal - this.currentlyShownScore);
      if ((double) num == 0.0)
      {
        this.scoreText.SetColor(Color.white);
      }
      else
      {
        this.currentlyShownScore += (int) ((double) num * (double) Mathf.Clamp01(3f * Time.deltaTime) + (double) Mathf.Sign(num));
        if ((double) num < 0.0)
          this.scoreText.SetColor(Color.Lerp(Color.white, Color.red, -num / 20f));
        else
          this.scoreText.SetColor(Color.Lerp(Color.white, Color.green, num / 20f));
        this.scoreText.text = this.currentlyShownScore.ToString();
      }
    }

    public void HandleHealth()
    {
      float num1 = LevelHandler.Instance.lemmy.health - this.lastShownHealth;
      if ((double) num1 < (double) this.lastDelta)
        this.lastDelta = num1;
      this.lastShownHealth += (float) ((double) num1 * (double) Time.deltaTime * 10.0);
      float num2 = this.lastShownHealth / 95f;
      if ((double) num1 < 0.0)
        num2 += num1 * 0.02f;
      float num3 = num1 * 0.85f;
      int index = (int) ((double) Mathf.Clamp01(num2) * 15.0);
      float a = 0.5f;
      if ((double) num3 < 0.0)
        a += (float) (-(double) num3 * 0.0099999997764825821);
      Material material = this.upperBorder.renderer.material;
      Color color = new Color(material.color.r, material.color.g, material.color.b, a);
      this.upperBorder.renderer.material.color = color;
      this.lowerBorder.renderer.material.color = color;
      this.rightBorder.renderer.material.color = color;
      this.leftBorder.renderer.material.color = color;
      this.upperBorder.texture = this.damageBorderTextures[index];
      this.lowerBorder.texture = this.damageBorderTextures[index];
      this.rightBorder.texture = this.damageBorderTextures[index];
      this.leftBorder.texture = this.damageBorderTextures[index];
    }
  }
}
