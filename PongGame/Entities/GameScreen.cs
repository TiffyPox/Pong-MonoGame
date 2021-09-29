using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PongGame.Graphics;
using PongGame.System;
using System;

namespace PongGame.Entities
{
    public class GameScreen : IScreen
    {
        private const string AssetName = "PongSpriteSheet";
  
        private Texture2D _spriteSheetTexture;
        public SoundEffect _scoreSound { get; private set; }
        private SoundEffect _collisionSound;
        private SoundEffect _replaySound;

        private SpriteFont _font;

        private Sprite _ballSprite;
        private Sprite _paddleSprite;
        private Sprite _paddleSpriteTwo;
        private Sprite _boardSprite;
        

        private InputController _inputController;
        private InputController _inputController2;

        private Ball _ball;
        private Paddle _paddle;
        private Paddle _paddleTwo;


        private KeyboardState _previousKeyboardState;

        public GameState State { get; private set; }

        public Action RequestScreenChange { get; set; }

        public Color BackgroundColor => new Color(0, 0, 0, 0);

        public void Load(ContentManager content)
        {
            _spriteSheetTexture = content.Load<Texture2D>(AssetName);
            _collisionSound = content.Load<SoundEffect>("CollisionSound");
            _replaySound = content.Load<SoundEffect>("ReplaySound");
            _font = content.Load<SpriteFont>("MenuText");

            _ballSprite = new Sprite(_spriteSheetTexture, 164, 36, 54, 54);
            _paddleSprite = new Sprite(_spriteSheetTexture, 32, 0, 32, 191);
            _paddleSpriteTwo = new Sprite(_spriteSheetTexture, 96, 0, 32, 191);
            _boardSprite = new Sprite(_spriteSheetTexture, 142, 0, 10, 384);
            _scoreSound = content.Load<SoundEffect>("ScoreSound");
        }
        public void Initialize()
        {
            _ball = new Ball(_ballSprite, new Vector2(Pong.ScreenWidth / 2, Pong.ScreenHeight / 2), _scoreSound);
            _paddle = new Paddle(_paddleSprite, new Vector2(10 + _paddleSprite.RenderWidth, 480 / 2));
            _paddleTwo = new Paddle(_paddleSpriteTwo, new Vector2(Pong.ScreenWidth - 10 - _paddleSpriteTwo.RenderWidth, 480 / 2));
            _inputController = new InputController(_paddleTwo, Keys.Up, Keys.Down);
            _inputController2 = new InputController(_paddle, Keys.W, Keys.S);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _boardSprite.Draw(spriteBatch, new Vector2(Pong.ScreenWidth / 2, Pong.ScreenHeight / 2));
            _paddle.Draw(spriteBatch);
            _paddleTwo.Draw(spriteBatch);
            _ball.Draw(spriteBatch);

            var paddle1Text = $"{_paddle.Points}";
            var pointsDimension = _font.MeasureString(paddle1Text);
            spriteBatch.DrawString(_font, $"{_paddle.Points}", new Vector2(Pong.ScreenWidth / 2 - 20 - pointsDimension.X, 10), Color.White);
            spriteBatch.DrawString(_font, $"{_paddleTwo.Points}", new Vector2(Pong.ScreenWidth / 2 + 20, 10), Color.White);

            if (_ball.Ended())
            {
                var textToDraw = "Press [Space] to Release the Ball!";

                if(!_ball.StuckToPlayer)
                {
                    textToDraw = $"Press [Space] to Play";
                }

                var textDimensions = _font.MeasureString(textToDraw);
                spriteBatch.DrawString(_font, textToDraw, new Vector2(Pong.ScreenWidth / 2 - textDimensions.X / 2, Pong.ScreenHeight - textDimensions.Y / 2 - 20), Color.White);
                State = GameState.GameOver;
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            State = GameState.Playing;

            _inputController.ProcessControls(gameTime);
            _inputController2.ProcessControls(gameTime);

            _ball.Update(gameTime);
            _ball.CheckForPaddle(_paddle, _paddleTwo, _collisionSound);

            if (keyboardState.IsKeyDown(Keys.Space) && _previousKeyboardState.IsKeyUp(Keys.Space) && _ball.Ended())
            {
                _replaySound.Play();
                _ball.Begin();
            }

            _previousKeyboardState = keyboardState;
        }
    }
}
