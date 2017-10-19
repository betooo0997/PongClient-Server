using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pong : Microsoft.Xna.Framework.Game
    {
        public static bool frameUpdate = false;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static KeyboardState prevKeyState { get; private set; }
        public static KeyboardState currKeyState { get; private set; }

        State_Menu state_menu;
        State_Playing state_playing;
        State_GameOver state_gameOver;

        public static GameState targetState;
        GameState currentState;

        public static SpriteFont font1;

        static Player[] players;
        static Ball ball;

        public static Pong Singleton;

        PongConnection server;

        public Pong()
        {
            server = new PongConnection(11000);
            server.Start();

            Singleton = this;

            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            state_menu = new State_Menu();
            state_playing = new State_Playing();
            state_gameOver = new State_GameOver();

            targetState = state_menu;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        public static void InitializeGame()
        {
            players = new Player[2];
            players[0] = new Player(10);
            players[1] = new Player(Singleton.GraphicsDevice.Viewport.Width - 25);
            ball = new Ball(Singleton.GraphicsDevice);
            ConnectionHandler.SendBallDataToAllClients();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Entity.SetSpriteBatch(spriteBatch, Content);
            GameState.SetSpriteBatch(spriteBatch);

            font1 = Content.Load<SpriteFont>("Font1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            prevKeyState = currKeyState;
            currKeyState = Keyboard.GetState();

            if (PongConnection.PlayerID != -1 && PongConnection.running)
            {
                if(currKeyState != prevKeyState)
                {
                    Keys[] pressedKeys = currKeyState.GetPressedKeys();

                    if (pressedKeys.Length > 0)
                        PongConnection.SyncPlayerPositions(pressedKeys[0].ToString());
                }
            }

            if (currKeyState.IsKeyDown(Keys.Escape))
            {
                if (currKeyState != prevKeyState)
                {
                    if (!PongConnection.running)
                        Exit();
                    else
                        PongConnection.CloseConnection();
                }
            }

            currentState = targetState;
            currentState.Update(gameTime);

            frameUpdate = true;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            currentState.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
