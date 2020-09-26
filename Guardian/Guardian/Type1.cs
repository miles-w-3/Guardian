using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Guardian
{
   public  class Type1 : Enemy
    {
        //properties 
         public static Texture2D enemyTexture; 
         public BoundingBox BoundingBox; //for collision


       
        public Type1(float px, float py)
        {
            //constructors 
            position = new Vector2(px, py);
            //Create a bounding box around the sprite dimensions
            BoundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3((position.X+enemyTexture.Width), (position.Y+enemyTexture.Height), 0)); 
        }
        public static void LoadContent(ContentManager cm)
        {
            //load sprite image 
              enemyTexture = cm.Load<Texture2D>("enemy1");    //loads enemy1 texture from pipeline
        }

        public void MovebBox()
        {

            //move boundingbox
            BoundingBox.Min.X = position.X;
            BoundingBox.Min.Y = position.Y;
            BoundingBox.Max.X = position.X + enemyTexture.Width;
            BoundingBox.Max.Y = position.Y + enemyTexture.Height;

        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(enemyTexture, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
    }
}
