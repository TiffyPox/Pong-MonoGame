using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PongGame.Entities
{
    public interface IScreen
    {
        Color BackgroundColor { get; }
        Action RequestScreenChange { get; set; }

        void Load(ContentManager content);
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
