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
    public class PowerUpBrick : Brick
    {
        protected PillFactory pillFactory;
        protected bool hasFirstHit;
        protected PillType typeOfPill;
        
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="brickImage">The reference to the brick Texture2D</param>
        /// <param name="position">The reference to the position Vector2</param>
        /// <param name="resistance">The resistance value</param>
        /// <param name="pillFactory">The reference to the PillFactory</param>
        public PowerUpBrick(Game game, Texture2D brickImage, Vector2 position, int resistance, PillFactory pillFactory, PillType typeOfPill)
            : base (game, brickImage, position, resistance)
        {
            this.pillFactory = pillFactory;
            this.typeOfPill = typeOfPill;
           
        } // End Constructor


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            hasFirstHit = false;

            base.Initialize();
        } // End Initialize()


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        } // End of Update()


        /// <summary>
        /// Draws the sprites
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        } // End of Draw()


        /// <summary>
        /// Loads the content
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        } // End of LoadContent()


        /// <summary>
        /// Overriden TakeHit method, drops a pill on first contact
        /// </summary>
        public override void TakeHit()
        {
            base.TakeHit();

            if (!hasFirstHit)
            {
                hasFirstHit = true;
                pillFactory.dropPill(this.CollisionBox, typeOfPill);
            }
        } // End of TakeHit()

    } // End PowerUpBrick class
}

