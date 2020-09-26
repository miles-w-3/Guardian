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
    public class Player
    {
        //properties 

        //loads the sprite textures for each way it faces
        Texture2D middle;
        Texture2D left;
        Texture2D right;
        //position for the sprite 
        public Vector2 position;
        Vector2 size; //length and width of the sprite
        public Vector2 oldPosition;
        public BoundingBox BoundingBox; //for collision
        public int hp = 10;     //health points
        public float cp = 17; //charge points
        public int sp = 0; //shield points
        public int score = 0;

        //constructors 

        public Player(Vector2 screenSize)
        {
            position = new Vector2 ((screenSize.X/2)-100, (screenSize.Y/2)-100); //sets player position
            
        }
        //function to load picture
        public void LoadContent(ContentManager cm)
        {
            //load sprite image 
            middle = cm.Load<Texture2D>("playerMiddle");
            left = cm.Load<Texture2D>("shootLeft");
            right = cm.Load<Texture2D>("shootRight");
            //get size of the sprite
            size = new Vector2(left.Width, left.Height); //sets size vector to the size of the sprite, uses left/ right cuz they're wider
            //Create a bounding box around the sprite dimensions
            BoundingBox = new BoundingBox(new Vector3(position.X, position.Y, 0), new Vector3(position.X + size.X, position.Y + size.Y, 0));
        }

        //Abilities

        public void Move(Vector2 screenSize)
        {
            oldPosition = position; //saves the current position as an old position before it changes 
            
            KeyboardState keys = Keyboard.GetState();

            GamePadState sticks = GamePad.GetState(PlayerIndex.One);


            if (keys.IsKeyDown(Keys.D) || sticks.ThumbSticks.Left.X > 0) //right
            {
                if (position.X < screenSize.X-size.X)//current approximation of the right edge
                {
                    position = position + new Vector2(5, 0);
                }
            }
            if (keys.IsKeyDown(Keys.A) || sticks.ThumbSticks.Left.X < 0) //left
            {
                if (position.X > 0)
                {
                    position = position + new Vector2(-5, 0);
                }
            }
            if (keys.IsKeyDown(Keys.W) || sticks.ThumbSticks.Left.Y > 0) //up
            {
                if (position.Y > 0)
                {
                    position = position + new Vector2(0, -5);
                }
            }
            if (keys.IsKeyDown(Keys.S) || sticks.ThumbSticks.Left.Y < 0) //down
            {
                if (position.Y < screenSize.Y-size.Y)  //current approximation of bottom edge 
                {
                    position = position + new Vector2(0, 5);
                }
            }
            
            //move boundingbox
            BoundingBox.Min.X = position.X;
            BoundingBox.Min.Y = position.Y;
            BoundingBox.Max.X = position.X + size.X;
            BoundingBox.Max.Y = position.Y + size.Y;  
        }


        public void Draw(SpriteBatch sb)
        {
            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.D)) //right
            {
                sb.Draw(right, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
            else if (keys.IsKeyDown(Keys.A)) //left
            {
                sb.Draw(left, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
            else //if left and right arent being pressed 
            {
                sb.Draw(middle, position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }
        }
    }
}
