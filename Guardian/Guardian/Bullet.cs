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
    public class Bullet
    {
        //properties 
        Vector2 course;
       public Vector2 position;
        public BoundingSphere BoundingSphere;

        //Declaring bullets 
        static Texture2D bullet1;
        // Texture2D bullet2;  //for later 

            
        //constructors 
         public Bullet(Vector2 start, Vector2 end) {  //function will be passed a start and end location for the bullet
            //gets the mouse location 

            position = new Vector2(start.X, start.Y);   //eventually add a little bit to x and y so that bullets dont spawn out of top right corner. If the gun becomes a seperate thing, make it spawn from the gun

            //creates a new vector from player to crosshair point
             course = end - start;  

            course = Vector2.Normalize(course)*10; //multiply by speed

            //BoundingSphere 
            BoundingSphere = new BoundingSphere(new Vector3(position.X + (bullet1.Width / 2), position.Y + (bullet1.Height / 2), 0), bullet1.Width/2);  //passing the sphere its center and the radius that the sphere will be 
        }

        //abilities 
        public void Move()
        {

            //adds course to position vector to make it move 
            position = position + course;
            //Moves the sphere with the sprite
            BoundingSphere.Center = new Vector3(position.X + (bullet1.Width / 2), position.Y + (bullet1.Height / 2), 0);

        }

        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
            bullet1= cm.Load<Texture2D>("Bullet1");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(bullet1, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
        
    }

}
