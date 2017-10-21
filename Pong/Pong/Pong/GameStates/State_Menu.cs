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
    public class State_Menu : GameState
    {
        public static State_Menu Singleton { get; private set; }

        int selectedText = 0;

        Color serverText = Color.Red;
        Color clientText = Color.White;

        /// <summary>
        /// The inputted data by the user. Used to set (server) or send password (client).
        /// </summary>
        public string input;

        /// <summary>
        /// The error code to be showed on screen
        /// </summary>
        public string info = "";

        /// <summary>
        /// The class constructor.
        /// </summary>
        public State_Menu()
        {
            Start();
            Singleton = this;
        }

        /// <summary>
        /// Updates the Menu State.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Pong.currKeyState != Pong.prevKeyState)
            {
                Keys[] keys = Pong.currKeyState.GetPressedKeys();

                if (keys.Length > 0)
                {
                    if (keys[0].Equals(Keys.Back))
                        input = input.Substring(0, input.Length - 1);
                    else if (keys[0].ToString().Length <= 2)
                        input += keys[0].ToString();
                }

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
                            success = PongConnection.StartListening(true, input);
                            break;

                        case 1:
                            success = PongConnection.StartListening(false, input);
                            break;

                        default:
                            success = false;
                            break;
                    }

                    if (success)
                    {
                        info = "";

                        if (selectedText == 0)
                            targetState = State_Playing.Singleton;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the Menu State. 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.DrawString(Pong.font1, "MENU", new Vector2(200, 5), Color.White);

            if(serverText == Color.Red)
                spriteBatch.DrawString(Pong.font1, "Connect as server, Set Password: \"" + input + "\"", new Vector2(200, 45), serverText);
            else
                spriteBatch.DrawString(Pong.font1, "Connect as server", new Vector2(200, 45), serverText);

            if(clientText == Color.Red)
                spriteBatch.DrawString(Pong.font1, "Connect as client, Password: \"" + input + "\"", new Vector2(200, 75), clientText);
            else
                spriteBatch.DrawString(Pong.font1, "Connect as client", new Vector2(200, 75), clientText);

            spriteBatch.DrawString(Pong.font1, info, new Vector2(200, 150), Color.White);
        }
    }
}
