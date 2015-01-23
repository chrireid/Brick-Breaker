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
    public class ObstacleBrick : Brick
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="brickImage">The reference the the brick Texture2D</param>
        /// <param name="position">The reference to the position Vector2</param>
        public ObstacleBrick(Game game, Texture2D brickImage, Vector2 position)
            : base(game, brickImage, position, 0)
        {
            // Do nothing
        } // End Constructor


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            points = 0;
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
            base.Draw(gameTime);
        } // End Draw()


        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        } // End LoadContent()


        /// <summary>
        /// Overriden method to do nothing when called
        /// </summary>
        public override void TakeHit()
        {
            // Do nothing
        } // End TakeHit()

    }
}
