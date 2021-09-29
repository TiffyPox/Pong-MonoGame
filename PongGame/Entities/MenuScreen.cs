using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PongGame.Graphics;
using System;
using System.Collections.Generic;

namespace PongGame.Entities
{
    public class MenuScreen : IScreen
    {
        public List<Button> _buttons = new List<Button>();

        private Sprite _titleSprite;
        private SpriteFont _menuText;

        private SoundEffect _menuSong;
        private SoundEffect _playSound;

        private readonly string line1 = "P1  press 'w' and 's' to move";
        private readonly string line2 = "\nP2  press the 'up' and 'down' keys to move";
        private readonly string credits = "Created by @Tiffypox";


        public int DrawOrder { get; }

        public Action RequestScreenChange { get; set; }

        public Color BackgroundColor => new Color(72, 61, 139, 255);

        public MenuScreen()
        {
        }

        public void Load(ContentManager content)
        {
            var spriteSheet = content.Load<Texture2D>("PongSpriteSheet");
            _menuText = content.Load<SpriteFont>("MenuText");
            _menuSong = content.Load<SoundEffect>("HappyDaySong");
            _playSound = content.Load<SoundEffect>("PlaySound");

            _titleSprite = new Sprite(spriteSheet, 176, 192, 88, 29, 2);

            var playButtonHeldSprite = new Sprite(spriteSheet, 162, 104, 109, 25, 2);
            var playButtonSprite = new Sprite(spriteSheet, 162, 148, 109, 27, 2);
            var playButtonPosition = new Vector2(Pong.ScreenWidth / 2, Pong.ScreenHeight / 2 - 40);
            var playButton = new Button(playButtonSprite, playButtonPosition, playButtonHeldSprite, _playSound);
            _buttons.Add(playButton);

            playButton.OnClick += OnPlayButtonClicked;
        }
        public void OnPlayButtonClicked()
        {
            RequestScreenChange?.Invoke();
            _menuSong.Dispose();
            _playSound.Play();
        }

        public void Initialize()
        {
            _menuSong.Play();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var button in _buttons)
            {
                button.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            _titleSprite.Draw(spriteBatch, new Vector2(Pong.ScreenWidth / 2, 70));
            spriteBatch.DrawString(_menuText, line1, new Vector2(Pong.ScreenWidth / 2 - 120, Pong.ScreenHeight / 2), Color.White);
            spriteBatch.DrawString(_menuText, line2, new Vector2(Pong.ScreenWidth / 2 - 190, Pong.ScreenHeight / 2 + 10), Color.White);
            spriteBatch.DrawString(_menuText, credits, new Vector2(580, 450), Color.Black);
        }
    }
}
