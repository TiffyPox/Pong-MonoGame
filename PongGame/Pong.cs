using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PongGame.Entities;
using System.Collections.Generic;

namespace PongGame
{
    public class Pong : Game
    {
        public const string GAME_TITLE = "Tiff's Pong Game";
        public static int ScreenWidth;
        public static int ScreenHeight;

        public enum DisplayMode
        {
            Default,
            Zoomed
        }

        public const int DISPLAY_ZOOM_FACTOR = 2;

        private GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;

        public static MouseState LastMouseState { get; private set; }
        public static MouseState CurrentMouseState { get; private set; }

        public DisplayMode WindowDisplayMode { get; set; } = DisplayMode.Default;

        public float ZoomFactor => WindowDisplayMode == DisplayMode.Default ? 1 : DISPLAY_ZOOM_FACTOR;

        public Queue<IScreen> _screens = new Queue<IScreen>();

        public Pong()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenWidth = _graphics.PreferredBackBufferWidth;
            ScreenHeight = _graphics.PreferredBackBufferHeight;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Window.Title = GAME_TITLE;

            _screens.Enqueue(new MenuScreen());
            _screens.Enqueue(new GameScreen());

            foreach (var screen in _screens)
            {
                screen.Load(Content);
                screen.Initialize();

                screen.RequestScreenChange += () => {
                    _screens.Dequeue();
                };
            }
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentMouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _screens.Peek()?.Update(gameTime);

            base.Update(gameTime);

            LastMouseState = CurrentMouseState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_screens.Peek().BackgroundColor);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _screens.Peek()?.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
