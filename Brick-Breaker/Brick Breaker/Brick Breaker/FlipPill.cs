using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Class inherits behavior from the pill class.
    /// </summary>
    class FlipPill : Pill
    {
        /// <summary>
        /// Constructor for the flip pill. Class inherits from pill.
        /// </summary>
        /// <param name="game"> The current game</param>
        /// <param name="pillImage">The pill image</param>
        /// <param name="position">The pill's position</param>
        public FlipPill(Game game, Texture2D pillImage, Vector2 position)
            : base(game,pillImage,position)
        {
            //Do nothing
        }

        /// <summary>
        /// Initiliazes the current name of the pill
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            name = "Flip";
        } // End Initialize()

    }
}
