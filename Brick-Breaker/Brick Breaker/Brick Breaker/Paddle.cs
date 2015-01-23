using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Brick_Breaker
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Paddle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private int screenWidth, screenHeight;
        private readonly float speed;
        private Vector2 position, startPosition;
        private Game game;
        private int life;

        private KeyboardState oldState;
        private SpriteBatch spriteBatch;
        private Texture2D paddleImage;

        private Boolean switchControls = false;
        

        /// <summary>
        /// Paddle constructor
        /// </summary>
        /// <param name="width">The screen width</param>
        /// <param name="height">The screen height</param>
        /// <param name="speed">The paddle speed</param>
        /// <param name="game">The reference to the game</param>
        public Paddle(int width, int height, float speed, Game game)
            : base(game)
        {
            this.game = game;

            screenWidth = width;
            screenHeight = height;
            this.speed = speed;
        } // End Constructor


        /// <summary>
        /// Public Get property (Collision Box)
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)position.X,
                    (int)position.Y,
                    paddleImage.Width,
                    paddleImage.Height);
            }
        }


        /// <summary>
        /// Public Get property for screenWidth
        /// </summary>
        public int ScreenWidth
        {
            get { return screenWidth; }
        }


        /// <summary>
        /// Public Get property for screenHeight
        /// </summary>
        public int ScreenHeight
        {
            get { return screenHeight; }
        }


        /// <summary>
        /// Public Get/Set property for life
        /// </summary>
        public int Life
        {
            get { return life; }
            set { life = value; }
        }
 

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            oldState = Keyboard.GetState();

            life = 3;

            base.Initialize();
        } // End Initialize()

        /// <summary>
        /// Draws the sprite
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(paddleImage, this.position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        } // End Draw()

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            checkInput(gameTime);

            base.Update(gameTime);
        } // End Update()

        /// <summary>
        /// Loads the sprite and sets initial position
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            paddleImage = game.Content.Load<Texture2D>("paddle");

            // X and Y positions
            float x = (screenWidth - paddleImage.Width) / 2;
            float y = (screenHeight - paddleImage.Height);

            position = startPosition = new Vector2(x, y);

            base.LoadContent();
        } // End LoadContent()

        /// <summary>
        /// Checks for user input
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void checkInput(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (!switchControls)
            {
                // Both keys are pressed down
                if (newState.IsKeyDown(Keys.Left) && newState.IsKeyDown(Keys.Right))
                {
                    // do nothing
                }
                else if (newState.IsKeyDown(Keys.Right))
                    moveRight(gameTime);

                else if (newState.IsKeyDown(Keys.Left))
                    moveLeft(gameTime);
            }
            else
            {
                if (newState.IsKeyDown(Keys.Left) && newState.IsKeyDown(Keys.Right))
                {
                    // do nothing
                }
                else if (newState.IsKeyDown(Keys.Left))
                    moveRight(gameTime);

                else if (newState.IsKeyDown(Keys.Right))
                    moveLeft(gameTime);
            }
        } // End checkInput()

        /// <summary>
        /// Moves the paddle left
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void moveLeft(GameTime gameTime)
        {
            position.X -= speed;
            checkBoundaries();
        } // End moveLeft()

        /// <summary>
        /// Moves the paddle right
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void moveRight(GameTime gameTime)
        {
            position.X += speed;
            checkBoundaries();
        } // End moveRight()

        /// <summary>
        /// Checks the boundaries of the game
        /// </summary>
        private void checkBoundaries()
        {
            if (position.X < 0)
                position.X = 0;
            else if (position.X + paddleImage.Width > screenWidth)
                position.X = screenWidth - paddleImage.Width;
        } // End checkBoundaries()

        /// <summary>
        /// Grants access to the event (called by pillFactory).
        /// </summary>
        /// <param name="pf">The PillFactory of the caught pill</param>
        public void SetPillCaughtHandler(PillFactory pf)
        {
            pf.pillCaught += checkPill;
        } // End SetPillCaughtHandler()

        /// <summary>
        /// Resets the keyboard controls (Key.Left == moveLeft)
        /// </summary>
        public void SetTrueControls()
        {
            switchControls = false;
        } // End SetTrueControls()

        /// <summary>
        /// Checks the pill to switch controls or not.
        /// </summary>
        /// <param name="pill">The pill caught</param>
        private void checkPill(Pill pill)
        {
            if (pill is FlipPill)
            {
                if (switchControls)
                    switchControls = false;
                else 
                    switchControls = true;
            }
            if (pill is LifePill)
                switchControls = false;
        } // End checkPill()

    } // End of Paddle class
}