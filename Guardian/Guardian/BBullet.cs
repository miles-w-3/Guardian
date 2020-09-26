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
    class BBullet : Ebullet
    {
        public BBullet(Vector2 start, Vector2 end) : base(start, end)    //runs base constructor 
        {
            course = Vector2.Normalize(course) * 3f; //multiply by speed  
            shoot = shoot3;
        }


        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
            shoot3 = cm.Load<Texture2D>("Bshoot");


        }

        public override void Action(Player p)
        {
            p.hp = p.hp - 4;
        }
    }
}
