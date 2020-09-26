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
    public class T1Bullet : Ebullet
    {
       

        public T1Bullet(Vector2 start, Vector2 end):base(start, end)    //runs base constructor 
        {
            course = Vector2.Normalize(course) * 40; //multiply by speed    //NEED TO CHANGE FOR T1 AND T2
            shoot = shoot1;
        }


        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
            shoot1 = cm.Load<Texture2D>("Bullet1");
           
            
        }
        public override void Action(Player p)
        {
            p.hp = p.hp - 1;
        }

    }
}
