using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using PongGame.Entities;

namespace PongGame.System
{
    public class InputController
    {
        private Paddle _paddle;

        public GameState State { get; private set; }

        public Keys UpKey { get; }
        public Keys DownKey { get; }
        public Keys SpaceKey { get; }

        public InputController(Paddle paddle, Keys upKey, Keys downKey)
        {
            UpKey = upKey;
            DownKey = downKey;
            _paddle = paddle;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

                if (keyboardState.IsKeyDown(UpKey))
                {
                    _paddle.Move(0, -1);
                }
                else if (keyboardState.IsKeyDown(DownKey))
                {
                    _paddle.Move(0, 1);
                }

            State = GameState.Playing;
        }
    }
}
