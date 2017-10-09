using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    class State_Menu : GameState
    {
        public static State_Menu Singleton { get; private set; }

        int selectedText = 0;

        Color serverText = Color.Red;
        Color clientText = Color.White;

        public State_Menu()
        {
            Start();
            Singleton = this;
        }

        public override void Update(GameTime gameTime)
        {
            if (Pong.currKeyState != Pong.prevKeyState)
            {
                if (Pong.currKeyState.IsKeyDown(Keys.Down))
                    selectedText++;

                if (Pong.currKeyState.IsKeyDown(Keys.Up))
                    selectedText--;

                switch (selectedText)
                {
                    case 0:
                        serverText = Color.Red;
                        clientText = Color.White;
                        break;

                    case 1:
                        serverText = Color.White;
                        clientText = Color.Red;
                        break;
                }

                if (Pong.currKeyState.IsKeyDown(Keys.Enter))
                {
                    bool success;

                    switch (selectedText)
                    {
                        case 0:
                            success = PongConnection.StartListening(true);
                            break;

                        case 1:
                            success = PongConnection.StartListening(false);
                            break;

                        default:
                            success = false;
                            break;
                    }

                    if (success)
                        targetState = State_Playing.Singleton;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.DrawString(Pong.font1, "STATE: MENU", new Vector2(200, 5), Color.White);

            spriteBatch.DrawString(Pong.font1, "Connect as server", new Vector2(200, 45), serverText);
            spriteBatch.DrawString(Pong.font1, "Connect as client", new Vector2(200, 75), clientText);
        }
    }
}
