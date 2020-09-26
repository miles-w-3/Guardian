using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// needed for user input
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Guardian
{
    class Item
    {
        public Vector2 position;
        public Texture2D generic;
        public static Texture2D iSheild;
        public static Texture2D iHealth;
        public BoundingBox BoundingBox;

        public Item()
        {
        }

        public virtual void Action(Player p) 
        {

        }


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(generic, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
