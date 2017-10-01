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
        Vector2 Speed;

        public Ball(Vector2 StartPosition)
        {
            AddToArray(this);

            Position = StartPosition;
            LoadContent();
        }

        void LoadContent()
        {
            texture = content.Load<Texture2D>("Pixel");
        }

        public void setPosition(Vector2 Position)
        {
            this.Position = Position;
        }

        public override void Update(GameTime gameTime)
        {
            Position += Speed;
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
