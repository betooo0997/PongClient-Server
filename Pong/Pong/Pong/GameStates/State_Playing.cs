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
    public class State_Playing : GameState
    {
        public static State_Playing Singleton { get; private set; }

        public State_Playing()
        {
            Start();
            Singleton = this;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Entity entity in Entity.NonPlayerEntities)
                entity.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (Entity entity in Entity.NonPlayerEntities)
                entity.Draw(gameTime);

            foreach (Player player in Player.players)
                player.Draw(gameTime);

            spriteBatch.DrawString(Pong.font1, "STATE: PLAYING", new Vector2(500, 500), Color.White);
        }
    }
}
