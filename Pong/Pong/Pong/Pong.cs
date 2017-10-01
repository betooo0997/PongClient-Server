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

namespace PongServer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Pong : Microsoft.Xna.Framework.Game
    {
        public static bool frameUpdate = false;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        State_Menu state_menu;
        State_Playing state_playing;
        State_GameOver state_gameOver;

        public static GameState targetState;
        GameState currentState;

        public static SpriteFont font1;

        Player[] players;
        Ball ball;

        SocketServer server;

        public Pong()
        {
            server = new SocketServer(11000);

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
            server.Start();
            base.Initialize();
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

            players = new Player[2];
            players[0] = new Player(10);
            players[1] = new Player(GraphicsDevice.Viewport.Width - 25);
            ball = new Ball(GraphicsDevice);
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
            KeyboardState keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Keys.Escape))
                Exit();

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
