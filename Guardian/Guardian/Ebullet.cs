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
    public class Ebullet
    {
        protected Vector2 course;
        public Vector2 position;
        public static Texture2D shoot1; //image for bullet 1
        public static Texture2D shoot2;    //image for bullet 2
        public static Texture2D shoot3;     //image for boss bullet
        public Texture2D shoot; 
        public BoundingSphere BoundingSphere;

        public Ebullet()
        {
        }

        public Ebullet(Vector2 start, Vector2 end)
        {
            //constructors 

            position = new Vector2(start.X, start.Y);   //eventually add a little bit to x and y so that bullets dont spawn out of top right corner. If the gun becomes a seperate thing, make it spawn from the gun
            //BoundingSphere 
            BoundingSphere = new BoundingSphere(new Vector3(position.X + (shoot1.Width / 2), position.Y + (shoot1.Height / 2), 0), shoot1.Width);  //passing the sphere its center and the radius that the sphere will be 

            //creates a new vector from player to crosshair point
            course = end - start;
           // course = Vector2.Normalize(course) * 50; //multiply by speed -done in T1 and T2, not needed here
        }

        //abilities 
        public void Move()
        {
                //adds course to position vector to make it move 
                position = position + course;
                //Moves the sphere with the sprite
                BoundingSphere.Center = new Vector3(position.X + (shoot.Width / 2), position.Y + (shoot.Height / 2), 0);
        }

        public virtual void Action (Player p)
        {
            //this will be done in each class
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(shoot, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
