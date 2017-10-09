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
    public class Player : Entity
    {
        public static Player[] players;
        public int Score;

        public Vector2 Position;

        public Vector2 Size { get; private set; }

        public int ID { get; private set; }

        public Player(float PositionX)
        {
            AddToArray();

            Score = 0;
            Position = new Vector2(PositionX, 50);
            Size = new Vector2(15, 50);

            LoadContent();
        }

        void LoadContent()
        {
            texture = content.Load<Texture2D>("Pixel");
        }

        public static string GetPositions()
        {
            string result = (int)players[0].Position.Y + " " + (int)players[1].Position.Y;
            return result;
        }

        public void Move(float direction)
        {
            Position += new Vector2(0, direction * 25);
        }

        public void IncreasePoints()
        {
            Score++;
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

        public Vector2 UpdateDirectionVector(Vector2 ballPosition, int x)
        {
            float yDifference = ballPosition.Y + 5 - (Position.Y + Size.Y / 2);
            int a = 1;

            if (yDifference < 0)
                a = -1;

            yDifference = (float)(Math.Pow(yDifference * a, 1.5f) * a);

            Console.WriteLine("Player Pos: " + Position.Y + "\nBall Pos: " + ballPosition.Y + "\nYDifference: " + yDifference);

            return new Vector2(125 * x, yDifference);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);


            switch (ID)
            {
                case 0:
                    spriteBatch.DrawString(Pong.font1, "Player " + 1 + ":" + Score, new Vector2(Position.X, 15), Color.White);
                    break;

                case 1:
                    spriteBatch.DrawString(Pong.font1, "Player " + 2 + ":" + Score, new Vector2(Position.X - 100, 15), Color.White);
                    break;
            }
        }
    }
}
