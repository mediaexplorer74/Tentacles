using Microsoft.Xna.Framework;

namespace PressPlay.Tentacles
{
    // Minimal MonoGame shell for UWP build without game logic
    public class GameShell : Game
    {
        private GraphicsDeviceManager _graphics;

        public GameShell()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);
            base.Draw(gameTime);
        }
    }
}
