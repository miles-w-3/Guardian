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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


//TODO: fully add in the sprite for bullet in the Game1 class, need to call move
//working on vector for bullet
namespace Guardian
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D target;
        int gameLevel; //Shows level in game 
        TimeSpan rTime;
        int k; //bullet speed chance 

        //Calling classes
        Player p1;
        Target t;
        Boss b;
        //items
        List<Item> Items;

        List<Bullet> bullets; //arraylist for player bullets 
        List<Ebullet> ebullets;    //arraylist for enemy bullets 

        List<Type1> enemies1;   //enemies class 1 
        List<Type2> enemies2;   //enemies class 2 
        List<Enemy> enemies;    //enemies "parent" class
        //for loading things
        Texture2D background;
        Texture2D end;
        Texture2D win;
        Texture2D healthBar;
        Texture2D shieldBar;
        Texture2D chargeBar;
        Texture2D reloadBar;
        Texture2D reloadBarF;
        Texture2D howK;
        Texture2D howC;
        Texture2D BBar; //boss health bar 

        SpriteFont write;
        SpriteFont big;
        SpriteFont myFont;

        KeyboardState OldKeyState; //stores old state of keyboard to prevent double shooting 
        GamePadState OldPadState;
        int screenX;
        int screenY;
        Boolean controller; //true if controller is connected

        //gameMode enumerator
        public enum modeType { Help, Loss, Win, Menu, OneLevel, TwoLevel, OneBoss, TwoBoss, Pause };
        modeType gameMode; //stores the state of the game
        modeType prevMode;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            MediaPlayer.IsRepeating = true;
            rTime = rTime.Add(TimeSpan.FromSeconds(5)); //starts the reload counter at 5 so you can reload at the start
            Content.RootDirectory = "Content";
            //"births" classes
            t = new Target();
            enemies = new List<Enemy>();    //is this ever used? 
            enemies1 = new List<Type1>();
            enemies2 = new List<Type2>();
            bullets = new List<Bullet>();
            ebullets = new List<Ebullet>();
            Items = new List<Item>();

            OldKeyState = new KeyboardState(); //saves old keyboard state 

            //declare gamemode variables
            gameMode = new modeType();
            prevMode = new modeType();
            gameMode = modeType.Menu;
            //screen size 
            screenX = 3200;
            screenY = 1800;
            p1 = new Player(new Vector2(screenX, screenY));




        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            target = new RenderTarget2D(GraphicsDevice, screenX, screenY);   //resolution 3200 by 1800
            Bullet.LoadContent(Content);        //loads bullet image so that each bullet can use the same static image- saves memory

            //Enemies and Enemy bullets 
            T1Bullet.LoadContent(Content);
            T2Bullet.LoadContent(Content);
            BBullet.LoadContent(Content);
            Type1.LoadContent(Content);
            Type2.LoadContent(Content);


            Health.LoadContent(Content);
            Shield.LoadContent(Content);

            // TODO: use this.Content to load your game content here
            //normal background image 
            background = Content.Load<Texture2D>("Background");
            //status bars
            healthBar = Content.Load<Texture2D>("healthBar");
            shieldBar = Content.Load<Texture2D>("shieldBar");
            chargeBar = Content.Load<Texture2D>("chargeBar");
            reloadBar = Content.Load<Texture2D>("reloadBar");
            reloadBarF = Content.Load<Texture2D>("reloadBarF");
            BBar = Content.Load<Texture2D>("BBar");
            //howto images 
            howK = Content.Load<Texture2D>("howtoK");
            howC = Content.Load<Texture2D>("howtoC");


            //lose image 
            end = Content.Load<Texture2D>("gg");
            //win image 
            win = Content.Load<Texture2D>("win");
            p1.LoadContent(Content);
            t.LoadContent(Content);

            //loading text 
            write = Content.Load<SpriteFont>("write");
            big = Content.Load<SpriteFont>("bigWrite");
            myFont = Content.Load<SpriteFont>("myFont");

            //spawns an enemy at each gate
            Type1 t1; Type1 t2; Type1 t3; Type1 t4;
            t1 = new Type1((screenX / 2) - 150, 0);   //top gate
            t2 = new Type1(0, 600);   //left gate
            t3 = new Type1(screenX - 335, 600);   //right gate
            t4 = new Type1((screenX / 2) - 150, screenY - 460);   //bottom gate
            enemies1.Add(t1); enemies1.Add(t2); enemies1.Add(t3); enemies1.Add(t4);

            Sound.LoadContent(Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).IsConnected)//if you're using an xbox controller instead of keyboard
            {
                controller = true;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                this.Exit();
            }

            //check level for indicator 
            if (gameMode == modeType.OneLevel)
            {
                gameLevel = 1;
            }
            else if (gameMode == modeType.TwoLevel)
            {
                gameLevel = 2;
            }
            else if (gameMode == modeType.OneBoss)
            {
                gameLevel = 3;
            }

            if (gameMode != modeType.Pause)
            {

                if (gameMode == modeType.OneBoss || gameMode == modeType.TwoBoss || gameMode == modeType.OneLevel || gameMode == modeType.TwoLevel) //if in game
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.P) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftStick)) //Pauses game
                    {
                        prevMode = gameMode;
                        gameMode = modeType.Pause;
                    }
                }
                //move to wave 2 
                if (gameMode == modeType.OneLevel && enemies1.Count <= 0)  //if you kill all wave 1 enemies
                {
                    //creates a new shield, adds it to list of items
                    Shield iS;
                    iS = new Shield(new Vector2((screenX / 2), (screenY / 2)));
                    Items.Add(iS);

                    gameMode = modeType.TwoLevel;  //move to wave 2 

                    //spawn wave 2 enemies 

                    //enemy class 1:
                    Type1 t1; Type1 t2; Type1 t3; Type1 t4;
                    t1 = new Type1((screenX / 2) - 150, 0);   //top gate
                    t2 = new Type1(0, 600);   //left gate
                    t3 = new Type1(screenX - 335, 600);   //right gate
                    t4 = new Type1((screenX / 2) - 150, screenY - 460);   //bottom gate
                    enemies1.Add(t1); enemies1.Add(t2); enemies1.Add(t3); enemies1.Add(t4);

                    //enemy class 2:
                    Type2 t5; Type2 t6; Type2 t7; Type2 t8;
                    t5 = new Type2(screenX - 335, 50);   //top right
                    t6 = new Type2(50, 50);   //top left
                    t7 = new Type2(screenX - 335, screenY - 470);   //bottom right
                    t8 = new Type2(50, screenY - 470);   //bottom left
                    enemies2.Add(t5); enemies2.Add(t6); enemies2.Add(t7); enemies2.Add(t8);
                }

                //Part 1 Boss fight
                if (gameMode == modeType.TwoLevel && enemies1.Count == 0 && enemies2.Count == 0)    //if you kill all wave 2 enemies 
                {
                    b = new Boss(new Vector2((screenX / 2) - 200, (screenY / 2) - 200));
                    b.LoadContent(Content);
                    b.hp = 10;

                    //creates a new shield, adds it to list of items
                    Shield iS;
                    iS = new Shield(new Vector2((screenX / 2), 2 * (screenY / 3) + 50));
                    Items.Add(iS);

                    gameMode = modeType.OneBoss; //sets game mode to boss level
                }

                //part 2 boss fight
                if (gameMode == modeType.OneBoss && b.hp < 5)
                {
                    b.stage = 2; //moves boss to new abilities

                    //add in enemy class 2 fighters
                    Type2 t5; Type2 t6; Type2 t7; Type2 t8;
                    t5 = new Type2(screenX - 335, 50);   //top right
                    t6 = new Type2(50, 50);   //top left
                    t7 = new Type2(screenX - 335, screenY - 470);   //bottom right
                    t8 = new Type2(50, screenY - 470);   //bottom left
                    enemies2.Add(t5); enemies2.Add(t6); enemies2.Add(t7); enemies2.Add(t8);

                    b.hp = 10;

                    gameMode = modeType.TwoBoss;
                }

                if (gameMode == modeType.TwoBoss && b.hp <= 0) //if you kill the boss
                {
                    gameMode = modeType.Win; //move to win screen

                }


                //reset game
                if (gameMode == modeType.Win || gameMode == modeType.Loss)
                {
                    //resets game back to wave 1
                    enemies1.Clear();    //respawn all wave 1 enemies
                    enemies2.Clear();
                    Items.Clear();
                    Type1 t1; Type1 t2; Type1 t3; Type1 t4;
                    t1 = new Type1(900, 0);   //top gate
                    t2 = new Type1(0, 600);   //left gate
                    t3 = new Type1(1900, 600);   //right gate
                    t4 = new Type1(900, 1000);   //bottom gate
                    enemies1.Add(t1); enemies1.Add(t2); enemies1.Add(t3); enemies1.Add(t4);
                    p1 = new Player(new Vector2(screenX, screenY));  //override old player 
                    p1.LoadContent(Content);
                    //clear the bullet classes too 
                    bullets.Clear(); ebullets.Clear();

                    //to restart at level 1
                    if (Keyboard.GetState().IsKeyDown(Keys.V) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                    {

                        gameMode = modeType.OneLevel; //if v is pressed when the game is over, restart at level 1 
                        MediaPlayer.Play(Sound.background); //background music 

                    }
                    //to go back to menu
                    if (Keyboard.GetState().IsKeyDown(Keys.M) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))
                    {
                        gameMode = modeType.Menu; //if M is pressed, go back to main menu 
                    }
                }
                //start the game 
                if (gameMode == modeType.Menu)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.E) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.X)) //starts the game 
                    {
                        gameMode = modeType.OneLevel;
                        MediaPlayer.Play(Sound.background); //background music 
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.H) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y))//go to help screen
                    {
                        gameMode = modeType.Help;
                    }

                }

                if (gameMode == modeType.Help)//if in help screen
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.M) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightStick)) //if these buttons are pressed return to menu screen 
                    {
                        gameMode = modeType.Menu;
                    }
                }

                if (gameMode == modeType.OneLevel || gameMode == modeType.TwoLevel || gameMode == modeType.OneBoss || gameMode == modeType.TwoBoss) //if in game, not end game or menu screen
                {
                    rTime = rTime.Add(gameTime.ElapsedGameTime);

                    p1.Move(new Vector2(screenX, screenY));
                    t.Move(new Vector2(screenX, screenY));

                    //player death 
                    if (p1.hp <= 0)
                    {
                        gameMode = modeType.Loss;  //goto lose game screen 
                    }


                    //moves enemy1
                    foreach (Type1 l in enemies1)
                    {
                        l.Move(p1.position, p1.oldPosition);   //passing player position and previous player position
                        l.MovebBox();   //moves the bounding box. For now, i think it has to be in a separate function because i dont want to override move
                    }

                    //moves enemy2
                    foreach (Type2 f in enemies2)
                    {
                        f.Move(p1.position, p1.oldPosition);   //passing player position and previous player position
                        f.MovebBox();   //moves the bounding box. For now, i think it has to be in a separate function because i dont want to override move
                    }


                    //for bullets:

                    //moves bullets
                    foreach (Bullet l in bullets)
                    {
                        l.Move();
                    }
                    //moves enemy bullets 
                    foreach (Ebullet j in ebullets)
                    {
                        j.Move();
                    }

                    //player shooting bullets 
                    KeyboardState NewKeyState = Keyboard.GetState();
                    GamePadState NewPadState = GamePad.GetState(PlayerIndex.One);
                    //if space was up in the previous frame and is down now, then shoot
                    if (NewKeyState.IsKeyDown(Keys.Space) && OldKeyState.IsKeyUp(Keys.Space) || NewPadState.IsButtonDown(Buttons.RightShoulder) && OldPadState.IsButtonUp(Buttons.RightShoulder))
                    {
                        if (p1.cp >= 1) //if the player has enough charge points
                        {
                            //Creates bullet and adds it to the list of bullets 
                            Bullet b1;
                            b1 = new Bullet(p1.position, t.position);   //spawns bullets at player position with goal at target position
                            bullets.Add(b1);        //adds this bullet to the arraylist

                            p1.cp = p1.cp - 1; //lowers players gun charge

                            Sound.pShoot.Play();
                        }
                        else//if there is no gun charge 
                        {
                            Sound.noCharge.Play();
                        }
                    }
                    //reloads charge 
                    if (rTime >= TimeSpan.FromSeconds(5))
                    {
                        //Stops rTime from going over 5 so the bar looks better 
                        if (rTime.Seconds > 5)
                        {
                            rTime = TimeSpan.FromSeconds(5);
                        }
                        //reloading charge
                        if (NewKeyState.IsKeyDown(Keys.R) && OldKeyState.IsKeyUp(Keys.R) || NewPadState.IsButtonDown(Buttons.LeftShoulder) && OldPadState.IsButtonDown(Buttons.LeftShoulder))
                        {
                            if (p1.cp < 8)
                            {
                                p1.cp = p1.cp + 10;
                                Sound.Charge.Play();
                            }
                            rTime = TimeSpan.Zero;

                        }
                    }
                    OldKeyState = NewKeyState;  //updates oldkeystate to newkeystate
                    OldPadState = NewPadState;


                    //for enemy1 shooting bullets
                    Random r = new Random();    //random integer for bullets 
                    if (enemies1.Count > 0)
                    { //makes sure there is something in the list first 
                        if (r.Next(0, 200) < 2) //shoots bullets at random intervals 
                        {
                            T1Bullet s;
                            int i = r.Next(0, enemies1.Count());    //pick a random enemy to shoot
                            s = new T1Bullet(enemies1[i].position, p1.position);  //creates new bullet, passes the chosen enemy's position and player position
                            ebullets.Add(s);
                            Sound.t1Shoot.Play();
                        }
                    }

                    //for enemy2 shooting bullets 
                    if (enemies2.Count > 0) //makes sure there is something in the list first 
                    {
                        if (r.Next(0, 200) < 2) //shoots bullets at random intervals 
                        {
                            T2Bullet s;
                            int i = r.Next(0, enemies2.Count());    //pick a random enemy to shoot
                            s = new T2Bullet(enemies2[i].position, p1.position);  //creates new bullet, passes the chosen enemy's position and player position
                            ebullets.Add(s);
                            Sound.t2Shoot.Play();
                        }
                    }


                    //COLLISION STUFF
                    //Enemy Bullet collision with player 
                    for (int i = ebullets.Count() - 1; i >= 0; i--)
                    {
                        if (p1.BoundingBox.Intersects(ebullets[i].BoundingSphere))
                        {


                            if (p1.sp > 0)
                            {
                                p1.sp = p1.sp - 1;  //lowers player shield if they have 
                            }
                            else //if the player doesn't have a shield 
                            {
                                ebullets[i].Action(p1);
                            }

                            ebullets.RemoveAt(i);   //removes bullet
                            Sound.pHit.Play();
                        }
                    }

                    //Allied bullet with enemy player1
                    for (int j = enemies1.Count - 1; j >= 0; j--)
                    {
                        for (int i = bullets.Count - 1; i >= 0; i--)
                        {
                            if (j <= enemies1.Count - 1)
                            {
                                if (enemies1[j].BoundingBox.Intersects(bullets[i].BoundingSphere))
                                {
                                    //chance for enemies to drop health on death 
                                    Random l = new Random();
                                    if (l.Next(0, 5) == 0)
                                    {
                                        Health iH;
                                        iH = new Health(new Vector2(enemies1[j].position.X + 50, enemies1[j].position.Y + 50));  //if the random number is 0, leave a health pack at the enemy's positon
                                        Items.Add(iH);
                                    }
                                    bullets.RemoveAt(i);    //removes bullet
                                    enemies1.RemoveAt(j);    //kills (removes) enemy    
                                    Sound.ehit.Play();  //death sound 
                                }
                            }
                        }
                    }

                    //Allied bullet with enemy player2
                    for (int j = enemies2.Count - 1; j >= 0; j--)
                    {
                        for (int i = bullets.Count - 1; i >= 0; i--)
                        {
                            if (j <= enemies2.Count - 1)
                            {
                                if (enemies2[j].BoundingBox.Intersects(bullets[i].BoundingSphere))
                                {
                                    bullets.RemoveAt(i);    //removes bullet
                                    enemies2.RemoveAt(j);    //kills (removes) enemy
                                    Sound.ehit.Play();  //death sound 
                                }
                            }
                        }
                    }
                    //item collisions 
                    for (int i = Items.Count - 1; i >= 0; i--)
                    {
                        if (p1.BoundingBox.Intersects(Items[i].BoundingBox))
                        {
                            Items[i].Action(p1);
                            Items.RemoveAt(i);
                        }
                    }



                    // removes bullets if they go offscreen             
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        //collision detection
                        if (bullets[i].position.X >= screenX || bullets[i].position.X <= 0 || bullets[i].position.Y >= screenY || bullets[i].position.X <= 0)
                        {
                            bullets.RemoveAt(i);
                        }
                    }

                    //removes enemy bullets if they go offscreen             
                    for (int i = ebullets.Count - 1; i >= 0; i--)
                    {
                        //collision detection
                        if (ebullets[i].position.X >= screenX || ebullets[i].position.X <= 0 || ebullets[i].position.Y >= screenY || ebullets[i].position.X <= 0)
                        {
                            ebullets.RemoveAt(i);
                        }
                    }

                }

                //if in boss battle
                if (gameMode == modeType.OneBoss || gameMode == modeType.TwoBoss)
                {
                    //boss shooting  
                    Random rr = new Random();
                    //shoots more often during stage 2
                    if (b.stage == 1)
                    {
                        k = 100;
                    }
                    if (b.stage == 2)
                    {
                        k = 50;
                    }
                    if (rr.Next(0, k) < 2)
                    {
                        //Creates bullet and adds it to the list of bullets 
                        Ebullet c;
                        c = new BBullet(b.position, p1.position);   //spawns bullets at player position with goal at target position
                        ebullets.Add(c);        //adds this bullet to the arraylist

                    }

                    //player bullets with boss
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        if (bullets[i].BoundingSphere.Intersects(b.BoundingBox))
                        {
                            b.hp = b.hp - 1; //lower boss hp
                            bullets.RemoveAt(i);
                            Sound.ehit.Play();  //death sound 
                        }
                    }

                    // player  with boss
                    if (p1.BoundingBox.Intersects(b.BoundingBox))
                    {
                        p1.hp = p1.hp - p1.hp; //you die if you touch the boss
                    }
                }


            }
            else  //if in pause 
            {
                if (Keyboard.GetState().IsKeyDown(Keys.T) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightStick))
                {
                    gameMode = prevMode;
                }
            }
            base.Update(gameTime);

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //SpriteBatch targetBatch = new SpriteBatch(GraphicsDevice);

            GraphicsDevice.SetRenderTarget(target);
            // TODO: Add your drawing code here
            GraphicsDevice.Clear(Color.Maroon);

            if (gameMode == modeType.Pause)
            {
                GraphicsDevice.Clear(Color.Maroon);
                spriteBatch.Begin();
                if (controller)
                {
                    spriteBatch.DrawString(write, "press Right Stick to resume", new Vector2(200, 100), Color.White);
                    spriteBatch.Draw(howC, new Vector2(450, 300), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

                }
                else
                {
                    spriteBatch.DrawString(write, "press T to resume", new Vector2(200, 100), Color.White);
                    spriteBatch.Draw(howK, new Vector2(450, 300), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

                }
                spriteBatch.End();
            }

            if (gameMode == modeType.Menu) //menu 
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(big, "MAIN MENU", new Vector2((GraphicsDevice.Viewport.Width / 3) + 100, 300), Color.Black);
                if (controller) //using controller 
                {
                    spriteBatch.DrawString(write, "press X to play", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 500), Color.White);
                    spriteBatch.DrawString(write, "press Y for help", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 575), Color.White);
                    spriteBatch.DrawString(write, "press B to quit", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 650), Color.White);
                    spriteBatch.DrawString(write, "press Left Stick to pause", new Vector2((GraphicsDevice.Viewport.Width / 3) - 160, 725), Color.White);
                }
                else //using keyboard
                {
                    spriteBatch.DrawString(write, "press E to play", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 500), Color.White);
                    spriteBatch.DrawString(write, "press H for help", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 575), Color.White);
                    spriteBatch.DrawString(write, "press Q to quit", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 650), Color.White);
                    spriteBatch.DrawString(write, "press P to pause", new Vector2((GraphicsDevice.Viewport.Width / 3) + 65, 725), Color.White);
                }
                spriteBatch.End();


            }

            //Help Screen 
            if (gameMode == modeType.Help)
            {
                spriteBatch.Begin();
                if (controller)
                {
                    spriteBatch.DrawString(write, "press Right Stick to return to main menu", new Vector2(200, 100), Color.White);
                    spriteBatch.Draw(howC, new Vector2(450, 300), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

                }
                else
                {
                    spriteBatch.DrawString(write, "press M to return to main menu ", new Vector2(200, 100), Color.White);
                    spriteBatch.Draw(howK, new Vector2(450, 300), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

                }
                spriteBatch.End();
            }
            //draws the game over screen
            if (gameMode == modeType.Loss) //lose
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin();
                spriteBatch.Draw(end, new Vector2(200, 0), null, Color.White, 0f, Vector2.Zero, 1.3f, SpriteEffects.None, 0f);
                if (controller)
                {
                    spriteBatch.DrawString(write, "A to restart, Y for main menu", new Vector2(700, 1000), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(write, "V to restart, M for main menu", new Vector2(700, 1000), Color.White);

                }
                spriteBatch.End();
            }


            //game won screen
            if (gameMode == modeType.Win)    //win 
            {
                GraphicsDevice.Clear(new Color(0, 114, 97));
                spriteBatch.Begin();
                spriteBatch.Draw(win, new Vector2(screenX / 5, screenY / 4), null, Color.White, 0f, Vector2.Zero, 1.3f, SpriteEffects.None, 0f);
                if (controller) //controller 
                {
                    spriteBatch.DrawString(write, "A for Restart, Y for main menu", new Vector2(700, 1000), Color.White);
                }
                else //keyboard
                {
                    spriteBatch.DrawString(write, "V to Restart, M for main menu", new Vector2(700, 1000), Color.White);
                }
                spriteBatch.End();
            }

            //level 1, level 2, and the Boss Level
            if (gameMode == modeType.OneLevel || gameMode == modeType.TwoLevel || gameMode == modeType.TwoBoss || gameMode == modeType.OneBoss)
            {
                spriteBatch.Begin();
                //draws the background
                float scalex = (float)screenX / (float)background.Width;
                float scaley = (float)screenY / (float)background.Height;
                spriteBatch.Draw(background, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(scalex, scaley), SpriteEffects.None, 0f);
                // original before darby 
                //spriteBatch.Draw(background, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 0.56f, SpriteEffects.None, 0f);

                //level indicator 
                spriteBatch.DrawString(write, "Level " + gameLevel, new Vector2(screenX - 350, screenY - 100), Color.White);

                //drawing health bar- scaled with a vector that adds the player's hp to the current x coordinate to make the bar bigger.

                if (p1.hp > 0)
                {
                    //p1.hp is multiplied by 3 so that scaling is more noticeable 

                    spriteBatch.Draw(healthBar, new Vector2(10, screenY - 70), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 15 * p1.hp, 1), SpriteEffects.None, 0f);
                }
                //drawing shield bar
                if (p1.sp > 0)
                {
                    //p1.hp is multiplied by 3 so that scaling is more noticeable 

                    spriteBatch.Draw(shieldBar, new Vector2(10, screenY - 100), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 10 * p1.sp, 1), SpriteEffects.None, 0f);
                }
                //drawing charge bar
                if (p1.cp > 0)
                {
                    spriteBatch.Draw(chargeBar, new Vector2(10, screenY - 170), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 5 * p1.cp, 1), SpriteEffects.None, 0f);
                }
                //drawing reload bar 
                if (rTime.Seconds > 0 && rTime.Seconds <= 5)
                {
                    spriteBatch.Draw(reloadBar, new Vector2(10, screenY - 175), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 10 * rTime.Seconds, 1), SpriteEffects.None, 0f);
                }
                //draws full bar- different color so you know you can press 
                if (rTime.Seconds == 5 && p1.cp < 8)
                {
                    spriteBatch.Draw(reloadBarF, new Vector2(10, screenY - 175), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 10 * rTime.Seconds, 1), SpriteEffects.None, 0f);
                }


                if (enemies1.Count() > 0)
                {
                    //draws enemy class 1
                    foreach (Type1 i in enemies1)
                    {
                        i.Draw(spriteBatch);
                    }
                }
                if (enemies2.Count() > 0)
                {
                    //draws enemy class 2 
                    foreach (Type2 t in enemies2)
                    {
                        t.Draw(spriteBatch);
                    }
                }
                //draws bullets 
                foreach (Bullet j in bullets)
                {
                    j.Draw(spriteBatch);
                }
                //draws enemy bullets
                foreach (Ebullet k in ebullets)
                {
                    k.Draw(spriteBatch);
                }
                //draws items
                foreach (Item k in Items)
                {
                    k.Draw(spriteBatch);
                }
                //draw these last so that they overlap everything
                p1.Draw(spriteBatch);   //player
                t.Draw(spriteBatch);    //crosshair       

                spriteBatch.End();
            }

            //if during boss battle 
            if (gameMode == modeType.OneBoss || gameMode == modeType.TwoBoss)
            {
                spriteBatch.Begin();
                b.Draw(spriteBatch); //final boss
                                     //boss health bar 
                if (b.hp > 0)
                {
                    spriteBatch.Draw(BBar, new Vector2(b.position.X, b.position.Y - 10), null, Color.White, 0f, Vector2.Zero, new Vector2(10 + 15 * b.hp, 1), SpriteEffects.None, 0f);
                }
                t.Draw(spriteBatch);    //crosshair -- need to redraw so it goes above boss
                spriteBatch.End();
            }

            //set rendering back to the back buffer
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Maroon);
            //render target to back buffer
            spriteBatch.Begin();
            //try to use th other one in notes instead of displaymode
            spriteBatch.Draw(target, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            //GraphicsDevice.DisplayMode
            base.Draw(gameTime);

        }
    }
}
