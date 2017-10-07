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
    public abstract class Entity
    {
        protected static ContentManager content;
        public static Entity[] NonPlayerEntities;

        protected static SpriteBatch spriteBatch;
        protected Texture2D texture;

        public static void SetSpriteBatch(SpriteBatch spriteBatch, ContentManager content)
        {
            NonPlayerEntities = new Entity[0];
            Entity.spriteBatch = spriteBatch;
            Entity.content = content;
        }

        /// <summary>
        /// Adds a new Entity to entities.
        /// </summary>
        /// <param name="newEntity">The Entity to add.</param>
        public static void AddToArray(Entity newEntity)
        {
            if (NonPlayerEntities == null)
                NonPlayerEntities = new Entity[0];

            Entity[] temp = NonPlayerEntities;

            NonPlayerEntities = new Entity[temp.Length + 1];

            for(int x = 0; x < NonPlayerEntities.Length; x++)
            {
                if(x < NonPlayerEntities.Length - 1)
                    NonPlayerEntities[x] = temp[x];
                else
                    NonPlayerEntities[x] = newEntity;
            }
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
