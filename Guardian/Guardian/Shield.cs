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
    class Shield : Item
    {
        public Shield(Vector2 location)
        {
            generic = iSheild;
            position = location;
            BoundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + generic.Width, position.Y + generic.Height, 0));
            
        }

        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
            iSheild = cm.Load<Texture2D>("iShield");
        }
        public override void Action(Player p)
        {
            p.sp = p.sp + 5;
        }
    }
}
