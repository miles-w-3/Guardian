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
    //slower, bigger bullet 
    class T2Bullet : Ebullet
    {
      
        public T2Bullet(Vector2 start, Vector2 end) : base(start, end)
        {
            course = Vector2.Normalize(course) * 3; //multiply by speed
            shoot = shoot2;
        }


        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
            shoot2 = cm.Load<Texture2D>("Bullet2");

        }
         
        public override void Action (Player p)
        {
            p.hp = p.hp - 2;
        }

        
    }
}
