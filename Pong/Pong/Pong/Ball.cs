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

namespace PongServer
{
    public class Ball : Entity
    {
        public static Vector2 directionVector { get; protected set; }
        Vector2 size;

        GraphicsDevice graphicsDevice;

        public static bool paused = false;

        Vector2 windowSize;

        public static Vector2 Position { get; protected set; }


        public Ball(GraphicsDevice graphicsDevice)
        {
            AddToArray(this);

            this.graphicsDevice = graphicsDevice;

            Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            LoadContent();

            Random random = new Random();
            directionVector = Vector2.Normalize(new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f)) * 2;
            size = new Vector2(10, 10);
            windowSize = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        void LoadContent()
        {
            texture = content.Load<Texture2D>("Pixel");
        }

        void MakeGoal(Player Winner)
        {
            Winner.IncreasePoints();
        }

        void Collision(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    directionVector = new Vector2(-directionVector.X, directionVector.Y);
                    break;

                case Axis.Y:
                    directionVector = new Vector2(directionVector.X, -directionVector.Y);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                Position += directionVector;

                CheckBorderCollision(new Vector2(windowSize.X, windowSize.Y / 2), new Vector2(2, windowSize.Y), 1);
                CheckBorderCollision(new Vector2(0, windowSize.Y / 2), new Vector2(2, windowSize.Y), 2);
                CheckBorderCollision(new Vector2(windowSize.X / 2, windowSize.Y), new Vector2(windowSize.X, 2));
                CheckBorderCollision(new Vector2(windowSize.X / 2, 0), new Vector2(windowSize.X, 2));

                foreach (Player player in Player.players)
                    CheckPlayerCollision(player.Position, player.Size);
            }
        }

        void CheckBorderCollision(Vector2 objectPosition, Vector2 objectSize, int? PlayerID = null)
        {
            if (Position.X + size.X > objectPosition.X && Position.X < objectPosition.X + objectSize.X ||
                objectPosition.X + objectSize.X > Position.X && objectPosition.X < Position.X + size.X)
            {
                Collision(Axis.X);

                if(PlayerID != null)
                    MakeGoal(Player.players[(int)PlayerID - 1]);
            }

            if (Position.Y + size.Y > objectPosition.Y && Position.Y < objectPosition.Y + objectSize.Y ||
                objectPosition.Y + objectSize.Y > Position.Y && objectPosition.Y < Position.Y + size.Y)
            {
                Collision(Axis.Y);
            }
        }

        void CheckPlayerCollision(Vector2 objectPosition, Vector2 objectSize)
        {
            if (Position.X + size.X > objectPosition.X && Position.X < objectPosition.X + objectSize.X ||
                objectPosition.X + objectSize.X > Position.X && objectPosition.X < Position.X + size.X)
            {
                if (Position.Y + size.Y > objectPosition.Y && Position.Y < objectPosition.Y + objectSize.Y ||
                    objectPosition.Y + objectSize.Y > Position.Y && objectPosition.Y < Position.Y + size.Y)
                {
                    Collision(Axis.X);
                    HandleClient.SendBallDataToAllClients();

                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y), Color.White);
        }
    }

    enum Axis
    {
        X,
        Y
    }
}
