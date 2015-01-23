using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Brick_Breaker
{

    //Creating the delegate for the event
    public delegate void BrickHit(Brick brick);
    //Creating the delegate for the lose life event
    public delegate void LoseLife();




    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    /// 
    public class Ball : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private int screenWidth, screenHeight;
        private Game game;
        private SpriteBatch spriteBatch;
        private Texture2D ballImage;
        private Vector2 position, velocity, startVelocity;
        private Paddle paddle;
        private readonly float maxVelocity = 5.0F;
        private Wall wall;

        private bool onPaddle;

     

        //BrickHit event
        public event BrickHit BrickExploded;

        //LoseLife event
        public event LoseLife GroundHit;
        

        /// <summary>
        /// Ball constructor
        /// </summary>
        /// <param name="width">The screen width</param>
        /// <param name="height">The screen height</param>
        /// <param name="velocity">The ball velocity</param>
        /// <param name="game">The reference to the game</param>
        /// <param name="paddle">The reference to the paddle</param>
        public Ball(int screenWidth, int screenHeight, Vector2 velocity, Game game, Paddle paddle, Wall wall)
            : base(game)
        {
            this.game = game;
            this.paddle = paddle;
            this.velocity = velocity;
            this.startVelocity = velocity;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.wall = wall;
        }


        public bool OnPaddle
        {
            get { return onPaddle; }
            set { onPaddle = value; }
        }

        /// <summary>
        /// Public Get Property (Collision Box)
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    ballImage.Width,
                    ballImage.Height);
            }
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

        }


        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(ballImage, this.position, Color.White);



            spriteBatch.End();

            base.Draw(gameTime);
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            checkInput();

            if (this.onPaddle)  // something
                resetPosition();
            else    // Move the ball
                move();
            

            base.Update(gameTime);
        }


        /// <summary>
        /// Loads the sprite and sets initial position
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ballImage = game.Content.Load<Texture2D>("ball");

            resetPosition();
            PlaceBall();

            //Loading the event with the method
            //this.BrickExploded += updateScore;

            this.GroundHit += resetBall;
            this.GroundHit += removeLife;
            wall.LevelComplete += resetBall;

            base.LoadContent();
        }


        /// <summary>
        /// Moves the ball
        /// </summary>
        private void move()
        {
            position += velocity;
            checkCollision();
        }


        /// <summary>
        /// Checks for ball collision
        /// </summary>
        private void checkCollision()
        {
            checkHitPaddle();
            checkHitBrick();

            checkHitLeft();
            checkHitRight();
            checkHitTop();
            checkHitBottom();
        }


        /// <summary>
        /// Checks collision between the ball and the paddle
        /// </summary>
        private void checkHitPaddle()
        {
            // Check if the ball CollisionBox intersets the paddle CollisionBox
            if (this.CollisionBox.Intersects(paddle.CollisionBox))
            {
                // If the Y velocity is 0, the game is over
                if (velocity.Y != 0)
                {
                    // Midpoints
                    float midpointBall = this.CollisionBox.Center.X;
                    float midpointPaddle = paddle.CollisionBox.Center.X;

                    int overlapX, overlapY;

                    // The number of pixels that the ball intersected the paddle vertically
                    overlapY = this.CollisionBox.Bottom - paddle.CollisionBox.Top;
                    overlapX = (int)Math.Round(overlapY / velocity.Y);

                    // Bring the ball tangent to the paddle back in the opposite vector
                    position.Y -= overlapY;
                    position.X -= overlapX;

                    position -= velocity;

                    // Modify the ball velocity appropriately
                    velocity.X = (midpointBall - midpointPaddle) / 6;
                    velocity.Y = -velocity.Y;

                    // Increase speed of ball:
                    velocity.X *= 1.1F;
                    velocity.Y *= 1.1F;

                    //check for maximum velocity
                    checkMaxVelocity();
                }
            }
        }


        /// <summary>
        /// Checks collision between the ball and the left boundary
        /// </summary>
        private void checkHitLeft()
        {
            // The ball reached or surpassed the left
            if (this.CollisionBox.Left <= 0)
            {
                velocity.X = -velocity.X;

                // The ball surpassed the boundary
                if (this.CollisionBox.Left < 0)
                    position.X = -this.CollisionBox.Left;
            }
        }


        /// <summary>
        /// Checks collision between the ball and the right boundary
        /// </summary>
        private void checkHitRight()
        {
            // The ball reached or surpassed the right
            if (this.CollisionBox.Right >= screenWidth)
            {
                velocity.X = -velocity.X;

                // The ball surpassed the boundary
                if (this.CollisionBox.Right > screenWidth)
                    position.X -= (this.CollisionBox.Right - screenWidth);
            }
        }


        /// <summary>
        /// Checks collision between the ball and the top boundary
        /// </summary>
        private void checkHitTop()
        {
            // The ball reached or surpassed the top
            if (this.CollisionBox.Top <= 0)
            {
                velocity.Y = -velocity.Y;

                // The ball surpassed the boundary
                if (this.CollisionBox.Top < 0)
                    position.Y = -this.CollisionBox.Top;
            }
        }


        /// <summary>
        /// Checks collision between the ball and the bottom boundary
        /// </summary>
        private void checkHitBottom()
        {
            // The ball reached or surpassed the bottom
            if (this.CollisionBox.Bottom >= screenHeight)
            {
                // The ball surpassed the boundary
                if (this.CollisionBox.Bottom > screenHeight)
                    position.Y = screenHeight - ballImage.Height;

               // velocity = new Vector2(0, 0);  // Game over
                OnGroundHit();
            }
        }


        /// <summary>
        /// Checks the current velocity and ensures it remains below maximum value
        /// </summary>
        /// <param name="velocity">Current velocity</param>
        private void checkMaxVelocity()
        {
            // Right velocity maximum
            if (velocity.X > maxVelocity)
                velocity.X = maxVelocity;
            // Left velocity maximum
            if (velocity.X < -maxVelocity)
                velocity.X = -maxVelocity;
            // Down velocity maximum
            if (velocity.Y > maxVelocity)
                velocity.Y = maxVelocity;
            // Up velocity maximum
            if (velocity.Y < -maxVelocity)
                velocity.Y = -maxVelocity;

        }


        /// <summary>
        /// Checks if the ball hits the brick
        /// </summary>
        private void checkHitBrick()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (wall[i, j] != null)
                    {   //Checks if the brick is broken and if the ball and brick intersect
                        if (!wall[i, j].IsBroken && this.CollisionBox.Intersects(wall[i, j].CollisionBox))
                        {
                            //Storing the intersecting rectangle of the ball and the brick in a variable
                            Rectangle intersect = Rectangle.Intersect(this.CollisionBox, wall[i, j].CollisionBox);

                            //Storing the brick's collision box in a variable
                            Rectangle brick = wall[i, j].CollisionBox;

                            //Creating boolean values to figure out the bouncing of the ball
                            bool left = false, right = false, top = false, bottom = false;

                            // Check Y velocity: it will never be == 0
                            if (velocity.Y > 0) // if the ball is going down...
                            {
                                if (velocity.X > 0) // if the ball is going right...
                                {
                                    //sides to check: top, left
                                    left = checkHitBrickLeft(intersect, brick);
                                    top = checkHitBrickTop(intersect, brick);
                                }
                                else if (velocity.X < 0) // if the ball is going left...
                                {
                                    //sides to check: top, right
                                    right = checkHitBrickRight(intersect, brick);
                                    top = checkHitBrickTop(intersect, brick);
                                }
                                else // else the ball is not moving left or right
                                {
                                    top = checkHitBrickTop(intersect, brick);
                                }
                            }
                            else if (velocity.Y < 0) // if the ball is going up...
                            {
                                if (velocity.X > 0) // if the ball is going right...
                                {
                                    //sides to check: bottom, left
                                    left = checkHitBrickLeft(intersect, brick);
                                    bottom = checkHitBrickBottom(intersect, brick);
                                }
                                else if (velocity.X < 0) // else if the ball is going left...
                                {
                                    //sides to check: bottom, right
                                    right = checkHitBrickRight(intersect, brick);
                                    bottom = checkHitBrickBottom(intersect, brick);
                                }
                                else // else the ball is not moving left or right
                                {
                                    bottom = checkHitBrickBottom(intersect, brick);
                                }
                            }

                            //Conditional statements to figure out where/how the ball will bounced
                            if (left)
                                position.X -= intersect.Width;
                            if (right)
                                position.X += intersect.Width;
                            if (top)
                                position.Y -= intersect.Height;
                            if (bottom)
                                position.Y += intersect.Height;



                            //Calling the brick's takehit method
                            wall[i, j].TakeHit();

                            //Rasing the event
                            OnBrickExploded(wall[i, j]);

                        } // end: if
                    } // end: if
                } // end: for
            } // end: for
        }


        /// <summary>
        /// Check if the ball hits the left side of a brick.
        /// </summary>
        /// <param name="intersect">Rectangle of the intersection of the ball and the brick</param>
        /// <param name="brick">Rectangle of the brick</param>
        /// <returns>True if hits left</returns>
        private bool checkHitBrickLeft(Rectangle intersect, Rectangle brick)
        {

            if (intersect.Left == brick.Left) // it hit the left side ***FIX THIS
            {
                velocity.X = -velocity.X;
                return true;
            }
            return false;

        }


        /// <summary>
        /// Check if the ball hits the right side of a brick.
        /// </summary>
        /// <param name="intersect">Rectangle of the intersection of the ball and the brick</param>
        /// <param name="brick">Rectangle of the brick</param>
        /// <returns>True if hits right</returns>
        private bool checkHitBrickRight(Rectangle intersect, Rectangle brick)
        {
            if (intersect.Right == brick.Right) // it hit the right side ***FIX THIS
            {
                velocity.X = -velocity.X;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Check if the ball hits the top side of a brick.
        /// </summary>
        /// <param name="intersect">Rectangle of the intersection of the ball and the brick</param>
        /// <param name="brick">Rectangle of the brick</param>
        /// <returns>True if hits top</returns>
        private bool checkHitBrickTop(Rectangle intersect, Rectangle brick)
        {
            if (intersect.Top == brick.Top)
            {
                velocity.Y = -velocity.Y;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Check if the ball hits the bottom side of a brick.
        /// </summary>
        /// <param name="intersect">Rectangle of the intersection of the ball and the brick</param>
        /// <param name="brick">Rectangle of the brick</param>
        /// <returns>True if hits bottom</returns>
        private bool checkHitBrickBottom(Rectangle intersect, Rectangle brick)
        {
            if (intersect.Bottom == brick.Bottom)
            {
                velocity.Y = -velocity.Y;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Event handler of the BrickExploded event.
        /// </summary>
        /// <param name="brick"> Current Brick </param>
        protected void OnBrickExploded(Brick brick)
        {
            if (BrickExploded != null)
                BrickExploded(brick);
        }

        protected void OnGroundHit()
        {
            if(GroundHit != null)
                GroundHit();

        }

        void removeLife()
        {
            if (paddle.Life > 0)
                this.paddle.Life -= 1;
        }
        
     
       
        void resetBall()
        {
            if (paddle.Life > 0)
            {
               
                
                resetPosition();
                
            }
            else
                velocity = new Vector2(0, 0);
        }

        private void resetPosition()
        {
            // Set the onPaddle boolean to true (place the ball on the paddle)
            onPaddle = true;

            // X and Y positions
            float x = paddle.CollisionBox.Left + (paddle.CollisionBox.Width - this.CollisionBox.Width) / 2;
            float y = paddle.CollisionBox.Top - this.CollisionBox.Height;

            position = new Vector2(x, y);

        }


        public void PlaceBall()
        {
            if (onPaddle)
                velocity = new Vector2(0, 0);
            else
                velocity = startVelocity;
            

        }


        private void checkInput()
        {


            KeyboardState newState = Keyboard.GetState();
            
             
            // Both keys are pressed down
            if (newState.IsKeyDown(Keys.Space))
            {
                if (onPaddle)
                {
                    
                    onPaddle = false;
                    PlaceBall();
                }
            }
        }

    } // End of Ball class
}