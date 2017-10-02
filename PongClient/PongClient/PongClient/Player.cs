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

namespace PongClient
{
    public class Player : Entity
    {
        public static Player[] players;
        public int Points { get; private set; }
        bool contentLoaded = false;

        public Vector2 Position;

        public int ID { get; private set; }

        public Player(float PositionX)
        {
            AddToArray();

            Points = 0;
            Position = new Vector2(PositionX, 50);

            LoadContent();
        }

        void LoadContent()
        {
            texture = content.Load<Texture2D>("Pixel");
            contentLoaded = true;
        }

        void AddToArray()
        {
            if (players == null)
                players = new Player[0];

            Player[] temp = players;

            players = new Player[temp.Length + 1];

            for (int x = 0; x < players.Length; x++)
            {
                if (x < players.Length - 1)
                    players[x] = temp[x];
                else
                {
                    players[x] = this;
                    ID = x;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 15, 40), Color.White);
        }
    }
}
