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
    public enum GameStates
    {
        Menu,
        Playing,
        GameOver
    }

    public abstract class GameState
    {
        protected static SpriteBatch spriteBatch;
        protected GameState targetState;

        protected void Start()
        {
            targetState = this;
        }

        public static void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            GameState.spriteBatch = spriteBatch;
        }

        public virtual void Update(GameTime gameTime)
        {
            Pong.targetState = targetState;
        }

        public abstract void Draw(GameTime gameTime);
    }
}
