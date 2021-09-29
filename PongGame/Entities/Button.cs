using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PongGame.Graphics;
using System;

namespace PongGame.Entities
{
    public class Button : IGameEntity
    {
        private bool _held;

        private readonly Sprite _sprite;
        private readonly Vector2 _position;
        private readonly Sprite _heldSprite;
        private SoundEffect _playSound;

        public Action OnClick { get; set; }

        public int DrawOrder => 0;

        public Button(Sprite sprite, Vector2 position, Sprite heldSprite, SoundEffect playSound)
        {
            _playSound = playSound;
            _sprite = sprite;
            _position = position;
            _heldSprite = heldSprite;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Pong.CurrentMouseState;
            var mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);

            _held = false;

            if (mouseRectangle.Intersects(GetBounds()))
            {
                if (mouse.LeftButton == ButtonState.Released && Pong.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    OnClick?.Invoke();
                }
                else if (mouse.LeftButton == ButtonState.Pressed && Pong.LastMouseState.LeftButton == ButtonState.Pressed)
                {
                    _held = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_held)
            {
                _heldSprite.Draw(spriteBatch, _position);
            }
            else
            {
                _sprite.Draw(spriteBatch, _position);
            }
        }

        public Rectangle GetBounds() => new Rectangle((int)(_position.X - _sprite.RenderWidth / 2), (int)(_position.Y - _sprite.RenderHeight / 2), _sprite.RenderWidth, _sprite.RenderHeight);
    }
}
