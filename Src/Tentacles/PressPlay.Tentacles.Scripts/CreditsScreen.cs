// Decompiled with JetBrains decompiler
// Type: PressPlay.Tentacles.Scripts.CreditsScreen
// Assembly: PressPlay.Tentacles.Scripts, Version=1.2011.4.100, Culture=neutral, PublicKeyToken=null
// MVID: B6E1094A-B322-4665-8EA1-7734DAF1ACCB
// Assembly location: C:\Users\Admin\Desktop\RE\Tentacles\PressPlay.Tentacles.Scripts.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PressPlay.FFWD;
using PressPlay.FFWD.ScreenManager;
using PressPlay.FFWD.UI.Controls;

#nullable disable
namespace PressPlay.Tentacles.Scripts
{
  public class CreditsScreen : BackgroundScreen
  {
    private PanelControl creditsContainer;
    public bool transitionExitComplete;

    public CreditsScreen(string background)
      : base(background)
    {
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(0.0f, 11f, 0.0f);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.rootControl.AddChild((Control) this.background);
      this.background.transform.localPosition += new PressPlay.FFWD.Vector3(0.0f, -1f, 0.0f);
      this.rootControl.transform.position = new PressPlay.FFWD.Vector3(this.rootControl.transform.position.x, 1000f, this.rootControl.transform.position.z);
      this.controls.Add(this.rootControl);
      this.creditsContainer = new PanelControl();
      this.rootControl.AddChild((Control) this.creditsContainer);
      CreditTextEntry child1 = new CreditTextEntry("Tentacles", GUIAssets.berlinsSans40);
      child1.AddTextEntry("By Press Play", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child1);
      this.creditsContainer.AddChild((Control) new CreditTextEntry("", GUIAssets.berlinsSans40));
      CreditTextEntry child2 = new CreditTextEntry("Director &\nLead Game Design", GUIAssets.berlinsSans40);
      child2.AddTextEntry("Ole Stubbe Teglbjærg", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child2);
      CreditTextEntry child3 = new CreditTextEntry("Executive Producers", GUIAssets.berlinsSans40);
      child3.AddTextEntry("Rune Dittmer", GUIAssets.berlinsSans40);
      child3.AddTextEntry("Mikkel Thorsted", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child3);
      CreditTextEntry child4 = new CreditTextEntry("Lead Programming &\nGame Design", GUIAssets.berlinsSans40);
      child4.AddTextEntry("Klaus Hammerum Gregersen", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child4);
      CreditTextEntry child5 = new CreditTextEntry("Engine Programming", GUIAssets.berlinsSans40);
      child5.AddTextEntry("Thomas Gravgaard", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child5);
      CreditTextEntry child6 = new CreditTextEntry("Programming", GUIAssets.berlinsSans40);
      child6.AddTextEntry("Troels Johnsen", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child6);
      CreditTextEntry child7 = new CreditTextEntry("Level Design", GUIAssets.berlinsSans40);
      child7.AddTextEntry("Bjarne Kristiansen", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child7);
      CreditTextEntry child8 = new CreditTextEntry("Lead Artist", GUIAssets.berlinsSans40);
      child8.AddTextEntry("Robert Friis", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child8);
      CreditTextEntry child9 = new CreditTextEntry("Artists", GUIAssets.berlinsSans40);
      child9.AddTextEntry("Benjamin Maroti Magnussen", GUIAssets.berlinsSans40);
      child9.AddTextEntry("Lasse Jacob Middelbo Outzen", GUIAssets.berlinsSans40);
      child9.AddTextEntry("Nikolaj Severin", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child9);
      CreditTextEntry child10 = new CreditTextEntry("Music", GUIAssets.berlinsSans40);
      child10.AddTextEntry("Jens Christiansen", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child10);
      CreditTextEntry child11 = new CreditTextEntry("Sound Design", GUIAssets.berlinsSans40);
      child11.AddTextEntry("Hans Christian Kock", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child11);
      CreditTextEntry child12 = new CreditTextEntry("Special Thanks", GUIAssets.berlinsSans40);
      child12.AddTextEntry("Mikhail Akopyan", GUIAssets.berlinsSans40);
      child12.AddTextEntry("Bo \"Strandog\" Strandby", GUIAssets.berlinsSans40);
      child12.AddTextEntry("Marc-Trajan Caton", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child12);
      CreditTextEntry child13 = new CreditTextEntry("SUPPORTED BY", GUIAssets.berlinsSans40);
      child13.AddTextEntry("Nordic Game Program", GUIAssets.berlinsSans40);
      child13.AddTextEntry("New Danish Screen", GUIAssets.berlinsSans40);
      child13.AddTextEntry("Game Editor Simon Løvind", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child13);
      CreditTextEntry child14 = new CreditTextEntry("", GUIAssets.berlinsSans40);
      child14.AddTextEntry("", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child14);
      this.creditsContainer.AddChild((Control) new CreditTextEntry("MGS Mobile", GUIAssets.berlinsSans40));
      CreditTextEntry child15 = new CreditTextEntry("Production Team", GUIAssets.berlinsSans40);
      child15.AddTextEntry("Mathew Roberts", GUIAssets.berlinsSans40);
      child15.AddTextEntry("Maja Persson", GUIAssets.berlinsSans40);
      child15.AddTextEntry("Dennis Cheng", GUIAssets.berlinsSans40);
      child15.AddTextEntry("Cassie Townsend", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child15);
      CreditTextEntry child16 = new CreditTextEntry("Test Team", GUIAssets.berlinsSans40);
      child16.AddTextEntry("Chad Dylan Long", GUIAssets.berlinsSans40);
      child16.AddTextEntry("Paul Morris", GUIAssets.berlinsSans40);
      child16.AddTextEntry("Josiah Colborn", GUIAssets.berlinsSans40);
      child16.AddTextEntry("Brian Hicks", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child16);
      CreditTextEntry child17 = new CreditTextEntry("Product Planning &\nBusiness Team", GUIAssets.berlinsSans40);
      child17.AddTextEntry("Virginie Grange", GUIAssets.berlinsSans40);
      child17.AddTextEntry("Jeff Buckingham", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child17);
      CreditTextEntry child18 = new CreditTextEntry("Marketing", GUIAssets.berlinsSans40);
      child18.AddTextEntry("Kathy Richardson", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child18);
      CreditTextEntry child19 = new CreditTextEntry("Special Thanks", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Travis Howland", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Matt Booty", GUIAssets.berlinsSans40);
      child19.AddTextEntry("David Boker", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Oliver Miyashita", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Brian Weiss", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Erik Torgerson", GUIAssets.berlinsSans40);
      child19.AddTextEntry("Comsys", GUIAssets.berlinsSans40);
      child19.AddTextEntry("ATG Team", GUIAssets.berlinsSans40);
      this.creditsContainer.AddChild((Control) child19);
      Texture2D texture = Application.Load<Texture2D>("Textures/Menu/Credits/credits_gradient");
      ImageControl child20 = new ImageControl(texture);
      this.rootControl.AddChild((Control) child20);
      child20.transform.localPosition = (PressPlay.FFWD.Vector3) new Microsoft.Xna.Framework.Vector3(child20.transform.position.x, 100f, -60f);
      ImageControl child21 = new ImageControl(texture);
      this.rootControl.AddChild((Control) child21);
      child21.transform.localPosition = (PressPlay.FFWD.Vector3) new Microsoft.Xna.Framework.Vector3(child21.transform.position.x + (float) texture.Width, 100f, 540f);
      child21.transform.rotation = PressPlay.FFWD.Quaternion.Euler(0.0f, 3.14159274f, 0.0f);
      this.creditsContainer.LayoutColumn(10f, 0.0f, 15f);
      this.creditsContainer.transform.localPosition = new PressPlay.FFWD.Vector3(0.0f, 0.0f, (float) (PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Height - 200));
      TextControl child22 = new TextControl(LocalisationManager.Instance.GetString("menu_credits"), GUIAssets.berlinsSans40);
      this.rootControl.AddChild((Control) child22);
      child22.CenterTextWithinBounds(new Rectangle(PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 0, PressPlay.FFWD.ScreenManager.ScreenManager.Viewport.Width / 2, 40));
      this.SetTransitionPositionOnControls(1f);
    }

    private void OnButtonPress(object sender, ButtonControlEventArgs e) => this.ExitScreen();

    public override void HandleInput(InputState input) => base.HandleInput(input);

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      if (!this.IsOutOfBounds())
        this.creditsContainer.transform.localPosition += new PressPlay.FFWD.Vector3(0.0f, 0.0f, (float) (-85.0 * gameTime.ElapsedGameTime.TotalSeconds));
      else
        this.ExitScreen();
    }

    public override void OnTransitionExitComplete()
    {
      base.OnTransitionExitComplete();
      this.transitionExitComplete = true;
    }

    private bool IsOutOfBounds()
    {
      return this.creditsContainer.bounds.Y + this.creditsContainer.bounds.Height <= 0;
    }

    public override void Draw(GameTime gameTime) => base.Draw(gameTime);
  }
}
