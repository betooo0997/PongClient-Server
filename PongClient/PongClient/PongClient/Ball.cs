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
    public class Ball : Entity
    {
        public static Vector2 directionVector;
        public static Vector2 Position;
        static Vector2 size;
        Vector2 windowSize;


        public Ball(GraphicsDevice graphicsDevice)
        {
            AddToArray(this);

            Position = new Vector2();
            directionVector = new Vector2();
            LoadContent();
            size = new Vector2(10, 10);

            windowSize = new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
        }

        void LoadContent()
        {
            texture = content.Load<Texture2D>("Pixel");
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

        void CheckBorderCollision(Vector2 objectPosition, Vector2 objectSize, int? PlayerID = null)
        {
            if (Position.X + size.X > objectPosition.X && Position.X < objectPosition.X + objectSize.X ||
                objectPosition.X + objectSize.X > Position.X && objectPosition.X < Position.X + size.X)
            {
                Collision(Axis.X);
            }

            if (Position.Y + size.Y > objectPosition.Y && Position.Y < objectPosition.Y + objectSize.Y ||
                objectPosition.Y + objectSize.Y > Position.Y && objectPosition.Y < Position.Y + size.Y)
            {
                Collision(Axis.Y);
            }
        }


        public override void Update(GameTime gameTime)
        {
            Position += directionVector;

            CheckBorderCollision(new Vector2(windowSize.X, windowSize.Y / 2), new Vector2(2, windowSize.Y), 1);
            CheckBorderCollision(new Vector2(0, windowSize.Y / 2), new Vector2(2, windowSize.Y), 2);
            CheckBorderCollision(new Vector2(windowSize.X / 2, windowSize.Y), new Vector2(windowSize.X, 2));
            CheckBorderCollision(new Vector2(windowSize.X / 2, 0), new Vector2(windowSize.X, 2));
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(texture, new Rectangle((int)Position.X, (int)Position.Y, 10, 10), Color.White);
        }
    }

    enum Axis
    {
        X,
        Y
    }
}
