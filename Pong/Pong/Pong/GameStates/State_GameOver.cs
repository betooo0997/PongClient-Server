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
    public class State_GameOver : GameState
    {
        public static State_GameOver Singleton { get; private set; }

        public State_GameOver()
        {
            Start();
            Singleton = this;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.DrawString(Pong.font1, "STATE: GAMEOVER", new Vector2(500, 500), Color.White);
        }
    }
}
