using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Brick_Breaker
{

    //Creating 
    public delegate void CompleteLevel();

    //Creating the enum pillType
    public enum PillType { L, F };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Wall : Microsoft.Xna.Framework.DrawableGameComponent
    {

        private Brick[,] brickWall = new Brick[8, 5];
        private SpriteBatch spriteBatch;
        private Game game;
        private Vector2 position;
        private PillFactory pillFactory;
        private Paddle paddle;
        private int updateCount;
        private bool endGame;
        private int levelNum = 2;
       
        public event CompleteLevel LevelComplete;
        LevelFactory levelFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The reference to the game</param>
        /// <param name="position">The reference to the position Vector2</param>
        /// <param name="pillFactory">The reference to the PillFactory</param>
        public Wall(Game game, Vector2 position, PillFactory pillFactory, Paddle paddle)
            : base(game)
        {
            this.game = game;
            this.pillFactory = pillFactory;
            this.position = position;
            this.paddle = paddle;
        } // End Constructor


        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="i">Rank 0 of array</param>
        /// <param name="j">Rank 1 of array</param>
        /// <returns></returns>
        public Brick this[int i, int j]
        {
            get { return brickWall[i, j]; }

        } // End Indexer


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            LevelComplete += passLevel;

            LevelComplete += paddle.SetTrueControls;

            updateCount = 0;
            endGame = false;


        } // End Initialize()


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < brickWall.GetLength(0); i++)
                for (int j = 0; j < brickWall.GetLength(1); j++)
                    if (brickWall[i, j] != null)
                    {
                        brickWall[i, j].Update(gameTime);
                        brickWall[i, j].Draw(gameTime);
                    }

            base.Update(gameTime);

            if (endGame)
                passLevel();

            checkBricks();

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
        /// Loads the sprites and places bricks into the correct positions
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            levelFactory = new LevelFactory((int)position.X, (int)position.Y, this.game, this.pillFactory);

            brickWall = levelFactory.GetNextLevel();
        
            base.LoadContent();
        } // End LoadContent()


        /// <summary>
        /// Event handler of the LevelComplete event.
        /// </summary>
        private void OnLevelComplete()
        {
            if (LevelComplete != null)
                LevelComplete();
        }


        /// <summary>
        /// Checks to see if a brick was hit.
        /// </summary>
        private void checkBricks()
        {
            //initialize count
            int count = 0;

            for (int i = 0; i < brickWall.GetLength(0); i++)
                for (int j = 0; j < brickWall.GetLength(1); j++)
                    if (brickWall[i,j] == null || brickWall[i,j] is ObstacleBrick || brickWall[i,j].IsBroken)
                        count += 1;

            //raises OnLevelcompleteEvent()
            if(count == brickWall.Length && !endGame)
                OnLevelComplete();

        }


        /// <summary>
        /// Draws text indicating the next level number.
        /// </summary>
        private void DrawLevelCompleteString()
        {
            SpriteFont spriteFont = game.Content.Load<SpriteFont>("NextLevelFont");

            //create String containing number of next level
            String spriteLevelString = "LEVEL " + levelNum;

            Vector2 centerPosition = new Vector2(position.X/2 -125, position.Y/2 - spriteFont.LineSpacing);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, spriteLevelString, centerPosition, Color.White);
            spriteBatch.End();
        }


        /// <summary>
        /// Triggers Level Number animation and loads new level.
        /// </summary>
        private void passLevel()
        {
            endGame = true;

            if (updateCount <= 30) {
                brickWall = new Brick[0, 0];
                DrawLevelCompleteString();
            }
            else
            {
                levelNum++;
                brickWall = levelFactory.GetNextLevel();
                endGame = false;
                updateCount = 0;
            }

            updateCount++;
        }
    }
}
