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
    class Boss
    {
        //properties 
        public Texture2D boss1S;
        public Texture2D boss2S;
        public BoundingBox BoundingBox; //for collision
        public int stage;
        public int hp;
        public Vector2 position;

        //constructors
        public Boss(Vector2 spawn)
        {
            //constructors 
            stage = 1;
            position = new Vector2(spawn.X, spawn.Y);
            //Create a bounding box around the sprite dimensions
        }
        public void LoadContent(ContentManager cm)
        {
            //load sprite image 
            boss1S = cm.Load<Texture2D>("boss1S");    //load texture from pipeline
            boss2S = cm.Load<Texture2D>("boss2S");    //load texture from pipeline
            //Create a bounding box around the sprite dimensions
            BoundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3((position.X + boss1S.Width), (position.Y + boss1S.Height), 0));
    }

        public void MovebBox()
        {
            //move boundingbox
            BoundingBox.Min.X = position.X;
            BoundingBox.Min.Y = position.Y;
            BoundingBox.Max.X = position.X + boss2S.Width;
            BoundingBox.Max.Y = position.Y + boss2S.Height;
        }
        public void Draw(SpriteBatch sb)
        {
            if (stage == 1)
            {
                sb.Draw(boss1S, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
            if (stage ==2) //stage 2
            {
                sb.Draw(boss2S, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}
