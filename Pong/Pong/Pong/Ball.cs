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
    public class Ball : Entity
    {
        public static Vector2 Position;
        public static Vector2 directionVector;
        Vector2 size;

        GraphicsDevice graphicsDevice;

        Vector2 windowSize;

        public static float ballSyncIntervall = 1.5f;
        public static float timeSinceBallSync = 0;

        int startTime = 3;
        double ellapsedTime = 0;


        public Ball(GraphicsDevice graphicsDevice)
        {
            AddToArray(this);

            this.graphicsDevice = graphicsDevice;

            Position = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            LoadContent();

            if (PongConnection.PlayerID == -1)
            {
                Random random = new Random();
                directionVector = Vector2.Normalize(new Vector2(150, (float)random.NextDouble() - 0.5f)) * 100;
            }
            else
                directionVector = new Vector2();

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
            ConnectionHandler.SendScoreToAllClients();
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
            if (PongConnection.running)
            {
                if (ellapsedTime < startTime)
                    ellapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
                else
                    Position += directionVector * (float)gameTime.ElapsedGameTime.TotalSeconds;

                CheckBorderCollision(new Vector2(windowSize.X, windowSize.Y / 2), new Vector2(2, windowSize.Y), 1);
                CheckBorderCollision(new Vector2(0, windowSize.Y / 2), new Vector2(2, windowSize.Y), 2);
                CheckBorderCollision(new Vector2(windowSize.X / 2, windowSize.Y), new Vector2(windowSize.X, 2));
                CheckBorderCollision(new Vector2(windowSize.X / 2, 0), new Vector2(windowSize.X, 2));

                foreach (Player player in Player.players)
                    CheckPlayerCollision(player);

                if (PongConnection.PlayerID == -1)
                {
                    timeSinceBallSync += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeSinceBallSync > ballSyncIntervall)
                        ConnectionHandler.SendBallDataToAllClients();
                }
            }
        }

        void CheckBorderCollision(Vector2 objectPosition, Vector2 objectSize, int? PlayerID = null)
        {
            if (Position.X + size.X > objectPosition.X && Position.X < objectPosition.X + objectSize.X ||
                objectPosition.X + objectSize.X > Position.X && objectPosition.X < Position.X + size.X)
            {
                Collision(Axis.X);

                if (PongConnection.PlayerID == -1 && PlayerID != null)
                    MakeGoal(Player.players[(int)PlayerID - 1]);
            }

            if (Position.Y + size.Y > objectPosition.Y && Position.Y < objectPosition.Y + objectSize.Y ||
                objectPosition.Y + objectSize.Y > Position.Y && objectPosition.Y < Position.Y + size.Y)
            {
                Collision(Axis.Y);
            }
        }

        void CheckPlayerCollision(Player player)
        {
            if (Position.X + size.X > player.Position.X && Position.X < player.Position.X + player.Size.X ||
                player.Position.X + player.Size.X > Position.X && player.Position.X < Position.X + size.X)
            {
                if (Position.Y + size.Y > player.Position.Y && Position.Y < player.Position.Y + player.Size.Y ||
                    player.Position.Y + player.Size.Y > Position.Y && player.Position.Y < Position.Y + size.Y)
                {
                    Collision(Axis.X);

                    int x = 1;

                    if (directionVector.X < 0)
                        x = -1;

                    directionVector = player.UpdateDirectionVector(Position, x);

                    if (PongConnection.PlayerID == -1)
                        ConnectionHandler.SendBallDataToAllClients();
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