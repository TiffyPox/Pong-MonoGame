using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PongGame.Graphics;
using System;

namespace PongGame.Entities
{
    public class Ball : IGameEntity
    {
        private Random _random = new Random();

        public int Speed { get; set; }

        public Sprite Sprite { get; set; }

        private readonly Vector2 _startPosition;
        private readonly SoundEffect _scoreSound;

        public Vector2 Position { get; set; }

        private Paddle _lastHit;
        private Paddle _stuckTo;

        public int DrawOrder => 0;

        private double _moveDirectionX;
        private double _moveDirectionY;
        private float _ballSpeed = 5;
        private bool _released;

        public Ball(Sprite sprite, Vector2 position, SoundEffect scoreSound)
        {
            _startPosition = position;
            _scoreSound = scoreSound;
            Position = position;
            Sprite = sprite;
            Speed = 0;
        }

        public void Begin()
        {
            if (_released) return;

            // Get a random number between 1 and 3 ( which is either 1 or 2)
            // If it's 1, then we go left, if its 2 then we go right

            if (_stuckTo != null)
            {
                _moveDirectionX = (_stuckTo.Position.X < 100 ? 1 : -1) * _ballSpeed;
            }
            else
            {
                // Not stuck to anyone so random
                _moveDirectionX = (_random.Next(1, 3) == 1 ? -1 : 1) * _ballSpeed;
            }

            _moveDirectionY = (_random.Next(1, 3) == 1 ? -1 : 1) * _ballSpeed;

            _stuckTo = null;
            _released = true;
        }

        public void Reset(Paddle stickyPaddle)
        {
            _moveDirectionX = 0;
            _moveDirectionY = 0;

            if (stickyPaddle != null)
            {
                Position = new Vector2(stickyPaddle.GetFront().X, stickyPaddle.Position.Y);
                _stuckTo = stickyPaddle;
            }
            else
            {
                Position = _startPosition;
            }

            _released = false;
        }

        public bool Ended() => _moveDirectionX == 0 && _moveDirectionY == 0;

        public bool StuckToPlayer => _stuckTo != null;

        public void Update(GameTime gameTime)
        {
            if (_released)
            {
                Position += new Vector2((float)_moveDirectionX, (float)_moveDirectionY);

                if (Position.X < Sprite.RenderWidth / 2 || Position.X > Pong.ScreenWidth - Sprite.RenderWidth / 2)
                {
                    Position = new Vector2(Position.X - (float)_moveDirectionX, Position.Y);
                    _moveDirectionX *= -1;
                    _lastHit?.AwardPoint(1);
                    _scoreSound.Play();
                    Reset(_lastHit);
                }

                if (Position.Y < Sprite.RenderHeight / 2 || Position.Y > Pong.ScreenHeight - Sprite.RenderHeight / 2)
                {
                    Position = new Vector2(Position.X, Position.Y - (float)_moveDirectionY);
                    _moveDirectionY *= -1;
                }
            }
            else
            {
                if (_stuckTo != null)
                {
                    var frontX = _stuckTo.GetFront().X;
                    frontX = frontX < 100 ? frontX + Sprite.RenderWidth / 2 : frontX - Sprite.RenderWidth / 2;
                    Position = new Vector2(frontX, _stuckTo.Position.Y);
                }
            }
        }

        public bool CheckForPaddle(Paddle p1, Paddle p2, SoundEffect collision)
        {
            if (!_released) return false;

            Paddle intersected;

            if (GetBounds().Intersects(p1.GetBounds()))
            {
                intersected = p1;
            } 
            else if (GetBounds().Intersects(p2.GetBounds()))
            {
                intersected = p2;
            }
            else
            {
                return false;
            }

            var paddleCenterY = intersected.Position.Y;
            var paddleHeight = intersected.Sprite.RenderHeight;
            var centerY = Position.Y;
            var ballHeight = Sprite.RenderHeight;
            var ballXSpeed = _moveDirectionX;
            var ballYSpeed = _moveDirectionY;

            var ballSpeed = Math.Sqrt(ballXSpeed * ballXSpeed + ballYSpeed * ballYSpeed);
            var relativeBallPosition = (centerY - paddleCenterY) / paddleHeight / 2;
            var influenceY = 2;
            var maxXSpeed = 15;

            _moveDirectionY = ballSpeed * relativeBallPosition * influenceY;
            _moveDirectionX = Math.Sqrt(ballSpeed * ballSpeed - ballYSpeed * ballYSpeed) *
             (ballXSpeed > 0 ? -1 : 1) * 1.25;

            _moveDirectionX = MathHelper.Clamp((float)_moveDirectionX, -maxXSpeed, maxXSpeed);

            collision.Play();

            if (Position.X < 100)
            {
                Position = new Vector2(p1.GetBounds().Right + Sprite.RenderWidth / 2, Position.Y);
                    
            }
            else if (Position.X > 300)
            {
                Position = new Vector2(p2.GetBounds().Left - Sprite.RenderWidth / 2, Position.Y);
            }

            _lastHit = intersected;

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }
        public Rectangle GetBounds() => new Rectangle((int)(Position.X - Sprite.RenderWidth / 2), (int)(Position.Y - (Sprite.RenderHeight / 2)), Sprite.RenderWidth, Sprite.RenderHeight);
    }
}
