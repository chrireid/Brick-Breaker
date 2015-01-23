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
    public class Pill : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D pillImage;
        protected Game game;
        protected Vector2 position;
        protected String name;
        protected SpriteFont pillName;
        protected int counter, score;
        protected float speed;

        /// <summary>
        /// Pill constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="pillImage">The reference to the pill image</param>
        /// <param name="position">The reference to the position of the pill</param>
        public Pill(Game game, Texture2D pillImage, Vector2 position)
            : base(game)
        {
            this.game = game;
            this.pillImage = pillImage;
            this.position = position;
        } // End Constructor


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
                    pillImage.Width,
                    pillImage.Height);
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            name = "base";
            counter = 0;
            speed = 1.5F;
            score = 50;

            base.Initialize();
        } // End Initialize()

        /// <summary>
        /// Public Get property for score
        /// </summary>
        public int Score
        {
            get { return score; }
        }

        /// <summary>
        /// Public get, protected set property for the pill.
        /// </summary>
        public String Name
        {
            get { return name; }
            protected set { name = value; }
        }


        /// <summary>
        /// Loads the sprites.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            pillName = game.Content.Load<SpriteFont>("pillName");

            base.LoadContent();
        } // End LoadContent()


        /// <summary>
        /// Draws the sprites.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 FontOrigin = pillName.MeasureString(name) / 2;

            spriteBatch.Begin();
            spriteBatch.Draw(pillImage, this.position, Color.White);
            if (counter <= 15)
                spriteBatch.DrawString(pillName, name, position, Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();

            base.Draw(gameTime);
        } // End Draw()


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            this.position.Y += speed;
            counter += 1;

            base.Update(gameTime);
        } // End Update()

    } // End Pill class
}
