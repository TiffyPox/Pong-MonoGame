using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PongGame.Graphics;
using System;

namespace PongGame.Entities
{
    public class Paddle : IGameEntity
    {
        public Sprite Sprite { get; set; }

        public Vector2 Position { get; set; }
        private Vector2 _startPosition;

        private float Speed = 5f;

        public int DrawOrder { get; set; }

        private int _points;
        public int Points => _points;

        public Paddle(Sprite sprite, Vector2 position)
        {
            _startPosition = position;
            Position = position;
            Sprite = sprite;
        }

        public void Move(int x, int y)
        {
            Position += new Vector2(x, y * Speed);

            if (Position.Y < Sprite.RenderHeight / 2)
            {
                Position = new Vector2(Position.X, Sprite.RenderHeight / 2);
            }
            else if (Position.Y > 480 - Sprite.RenderHeight / 2)
            {
                Position = new Vector2(Position.X, 480 - Sprite.RenderHeight / 2);
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        public Rectangle GetBounds() => new Rectangle((int)(Position.X - Sprite.RenderWidth / 2), (int)(Position.Y - (Sprite.RenderHeight / 2)), Sprite.RenderWidth, Sprite.RenderHeight);

        public Vector2 GetFront()
        {
            if (Position.X < 100)
            {
                return new Vector2(GetBounds().Right, 0);
            }

            return new Vector2(GetBounds().Left, 0);
        }

        public void AwardPoint(int point)
        {
            _points += point;
        }

        internal void Reset()
        {
            Position = _startPosition;
        }
    }
}
