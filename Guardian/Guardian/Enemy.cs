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
    public class Enemy
    {
        //generic texture and position values
        public Vector2 position;
        public Enemy()
        {
            position = Vector2.Zero; //sets player position position to (0,0)
             
        }
        public void Move(Vector2 goal, Vector2 pgoal)   //gets passed goal and pgoal, the vector's current and previous position
        {
          
        //makes enemy follow you if you move too far 
            //finds the difference between the predicted mouse location and the current mouse location 
            float px = goal.X - pgoal.X;
            float py = goal.Y- pgoal.Y;
            //creates a vector for the predictred location of the mouse, px and py are multiplied to make their difference significant- basically a projection even further in the future than just px or py
            Vector2 predict;
            predict = new Vector2(goal.X + px * 20, goal.Y + py * 20);
            //create a vector for the new heading of the evader 
            Vector2 newV = Vector2.Subtract(predict, position);
            newV = Vector2.Normalize(newV) * 5; //multiply by speed 
            if (Vector2.Distance(goal, position) > 1000) //if the player moves too far away, follow it
            {
                position = position + newV;
            }

            //gives them some slight up and down movement -they have a random chance to move up and down
            Random r = new Random();    //random 
            if (r.Next(0, 50) == 0)
            {
                position = position + new Vector2(0, 5);
            }
            else if (r.Next(0, 50) == 1)
            {
                position = position - new Vector2(0, 5);
            }

        }
    }
}
