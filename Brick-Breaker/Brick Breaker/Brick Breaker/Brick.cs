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
    public class Brick : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Game game;
        protected SpriteBatch spriteBatch;
        protected Texture2D brickImage;
        protected Texture2D[] crackImage;
        protected Vector2 position;
        protected int crack;
        protected int resistance = 1;
        protected bool isBroken;
        protected int points;
        


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="brickImage">The reference to the brick Texture2D</param>
        /// <param name="position">The reference to the position Vector2</param>
        /// <param name="resistance">The resistance value</param>
        public Brick(Game game, Texture2D brickImage, Vector2 position, int resistance)
            : base(game)
        {
            this.game = game;
            this.brickImage = brickImage;
            this.position = position;
            this.resistance = resistance;

            // Initialize in constructor (rather than iterating through each
            // value in a multidimensional array).
            Initialize();
        } // End Brick()


        /// <summary>
        /// Public get only property (broken status)
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
        } // End IsBroken


        /// <summary>
        /// Public get only property (points)
        /// </summary>
        public int Points
        {
            get { return points; }

        } // End Points


        /// <summary>
        /// Public get only property (collision box)
        /// </summary>
        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                   (int)position.X,
                   (int)position.Y,
                   brickImage.Width,
                   brickImage.Height);
            }
        } // End CollisionBox


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            points = 10;
            isBroken = false;

            base.Initialize();
        } // End Initialize()


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        } // End Update()


        /// <summary>
        /// Draws the sprites
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!isBroken)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(brickImage, this.position, Color.White);
                spriteBatch.Draw(crackImage[crack], this.position, Color.White);
                spriteBatch.End();
            }
            else

                base.Draw(gameTime);
        } // End Draw()


        /// <summary>
        /// This loads the crack sprite and array, initializes the preset crack values.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Instantiate array of crackImages
            crackImage = new Texture2D[5];

            // Load array full with crackImages
            for (int i = 0; i < crackImage.GetLength(0); i++)
                crackImage[i] = Game.Content.Load<Texture2D>("crack" + i);

            // Initialize crack value
            setCrack();

            base.LoadContent();
        } // End LoadContent()


        /// <summary>
        /// This method alters the resistance and changes the crack value. 
        /// It also sets the brick to broken once the resistance is below 1.
        /// </summary>
        public virtual void TakeHit()
        {
            resistance -= 1;

            setCrack();

            if (resistance < 1)
                isBroken = true;
        } // End TakeHit()


        /// <summary>
        /// This method sets the value of the crack counter.
        /// </summary>
        private void setCrack()
        {
            switch (resistance)
            {
                case 1:
                    crack = 4;
                    break;
                case 2:
                    crack = 3;
                    break;
                case 3:
                    crack = 2;
                    break;
                case 4:
                    crack = 1;
                    break;
                case 5:
                    crack = 0;
                    break;
                default:
                    crack = 0;
                    break;
            }
        } // End setCrack()

    } // End Brick class
}
