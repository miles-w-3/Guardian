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
    class Target
    {
        //properties 

        //loads the sprite texture
        Texture2D crosshair;
        //position for the sprite 
        public Vector2 position;
        //constructors 

        public Target()
        {
            position = new Vector2(400, 400); 

        }
        //function to load picture
        public void LoadContent(ContentManager cm)
        {
            //load sprite image 
            crosshair = cm.Load<Texture2D>("crosshair");
        }

        //Abilities
        public void Move(Vector2 screenSize)  //pass this the width and height of viewport
        {
            KeyboardState keys = Keyboard.GetState();
            GamePadState sticks = GamePad.GetState(PlayerIndex.One);

            if (keys.IsKeyDown(Keys.L)|| sticks.ThumbSticks.Right.X > 0) //right
            {
                if (position.X <= screenSize.X-53.5)
                {
                    position = position + new Vector2(15, 0);
                }
            }
            if (keys.IsKeyDown(Keys.J) || sticks.ThumbSticks.Right.X < 0) //left
            {
                if (position.X >= 0)
                {
                    position = position + new Vector2(-15, 0);
                }
            }
            if (keys.IsKeyDown(Keys.I) || sticks.ThumbSticks.Right.Y > 0) //up
            {
                if (position.Y >= 0)
                {
                    position = position + new Vector2(0, -15);
                }
            }
            if (keys.IsKeyDown(Keys.K) || sticks.ThumbSticks.Right.Y < 0) //down
            {
                if (position.Y <=screenSize.Y-53.5) 
                {
                    position = position + new Vector2(0, 15);
                }
            }
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(crosshair, position, null, Color.White, 0f, Vector2.Zero, .05f, SpriteEffects.None, 0f);
        }

    }
}
