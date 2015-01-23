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
    /// Class inherits behaviour from the Pill.
    /// </summary>
    class LifePill : Pill
    {
        /// <summary>
        /// Constructor for the life pill. Class inherits from pill
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="pillImage">The pill image</param>
        /// <param name="position">The pill's position</param>
        public LifePill(Game game, Texture2D pillImage, Vector2 position)
            : base(game, pillImage, position)
        {
            //Do nothing
        }

        /// <summary>
        /// Initializes the name of the pill
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            name = "Life";
        } // End Initialize()

    }
}
