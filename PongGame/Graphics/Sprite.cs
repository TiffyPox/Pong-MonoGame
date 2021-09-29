using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame.Graphics
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Scale { get; }
        public Color TintColor { get; set; } = Color.White;

        public int RenderWidth => Width * Scale;
        public int RenderHeight => Height * Scale;

        public Sprite(Texture2D texture, int x, int y, int width, int height, int scale = 1)
        {
            Texture = texture;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Scale = scale;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(Texture, position, new Rectangle(X, Y, Width, Height), TintColor, 0, new Vector2(Width / 2, Height / 2), Scale, SpriteEffects.None, 0);
        }
    }
}
