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
    //Delegate for the pillHitPaddle event
    public delegate void pillHit(Pill pill);


    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class PillFactory : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private List<Pill> pillList = new List<Pill>();
        private SpriteBatch spriteBatch;
        private Texture2D pillImage;
        private Vector2 pillPosition;
        private Paddle paddle;
        public event pillHit pillCaught;
       


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="paddle">The reference to the paddle</param>
        public PillFactory(Game game, Paddle paddle)
            : base(game)
        {
            this.game = game;
            this.paddle = paddle;
            paddle.SetPillCaughtHandler(this);
        } // End Constructor


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        } // End Initialize()


        /// <summary>
        /// Draws the sprites
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        } // End Draw()

       

        /// <summary>
        /// Loads the game content
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pillImage = game.Content.Load<Texture2D>("pill");

            pillCaught += pillUpgrade;

            base.LoadContent();
        } // End LoadContent()


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (Pill i in pillList)
            {
                if (checkPill(i))
                {
                    pillList.Remove(i);
                    break;
                }
                else
                {
                    i.Update(gameTime);
                    i.Draw(gameTime);
                }
            }
            base.Update(gameTime);
        } // End Update()


        /// <summary>
        /// Checks if the pill intersects with the paddle and is in the game
        /// </summary>
        /// <param name="pill">The reference to the pill</param>
        /// <returns>True if intersects</returns>
        Boolean checkPill(Pill pill)
        {
            if (pill.CollisionBox.Intersects(paddle.CollisionBox))
            {
                OnPillCaught(pill);
                return true;
            }
            if (!pillIsInWidth(pill) || !pillIsInHeight(pill))
                return true;
            else return false;
        } // End checkPill()


        /// <summary>
        /// Checks if the pill is in the game width
        /// </summary>
        /// <param name="pill">The reference to the pill</param>
        /// <returns>True if within game width</returns>
        Boolean pillIsInWidth(Pill pill)
        {
            if (pill.CollisionBox.Left < 0 || pill.CollisionBox.Right > paddle.ScreenWidth)
                return false;
            else return true;
        } // End pillIsInWidth()


        /// <summary>
        /// Checks if the pill is in the game height.
        /// </summary>
        /// <param name="pill">The reference to the pill</param>
        /// <returns>True if within game height</returns>
        Boolean pillIsInHeight(Pill pill)
        {
            if (pill.CollisionBox.Top < 0 || pill.CollisionBox.Bottom > paddle.ScreenHeight)
                return false;
            else return true;
        } // End pillIsInHeight()


        /// <summary>
        /// Drops the pill from the brick
        /// </summary>
        /// <param name="collisionBox">Collision box of the brick</param>
        public void dropPill(Rectangle collisionBox, PillType typeOfPill)
        {
            pillPosition.Y = collisionBox.Bottom;
            pillPosition.X = collisionBox.Left + ((collisionBox.Width - pillImage.Width) / 2);

                 if (typeOfPill == PillType.F)
            pillList.Add(new FlipPill(game, pillImage, pillPosition));
            else if (typeOfPill == PillType.L)
                pillList.Add(new LifePill(game, pillImage, pillPosition));
            pillList.Last().Initialize();
        } // End dropPill()


        /// <summary>
        /// Event handler for pillHitPaddle.
        /// </summary>
        /// <param name="pill">The pill that collides with the paddle</param>
        protected void OnPillCaught(Pill pill)
        {
            if (pillCaught != null)
                pillCaught(pill);
        } // End OnPillCaught()


        /// <summary>
        /// Handles giving the player an extra life when catching a LifePill.
        /// </summary>
        /// <param name="pill">The LifePill caught</param>
        protected void pillUpgrade(Pill pill)
        {
            if (pill is LifePill)
                addLife();
        } // End pillUpgrade()


        /// <summary>
        /// Adds a life to the player
        /// </summary>
        protected void addLife()
        {
            this.paddle.Life += 1;
        } // End addLife()

    } // End PillFactory class
}
