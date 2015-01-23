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
    public class ScoreSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {


      
        private SpriteFont scoreAndLifesFont;
        private SpriteBatch spriteBatch;
        private Game1 game;
        private int screenWidth;
        private int screenHeight;
        private Ball ball;
        private Paddle paddle;
        private PillFactory pillFactory;

        private int gameScore;

        private int gameLifes;

        private Vector2 centerFirstLine;
        private Vector2 centerSecondLine;
        private Vector2 centerThirdLine;

        private HighScore highScore;



        //top right
        Vector2 livePos;
        //top
        Vector2 scorePos;

        /// <summary>
        /// Constructor for the ScoreSprite.
        /// </summary>
        /// <param name="game">The current game</param>
        /// <param name="screenWidth">The game's screen width</param>
        /// <param name="screenHeight">The game's screen height</param>
        /// <param name="paddle">The current paddle.</param>
        /// <param name="ball">The current ball.</param>
        /// <param name="pillFactory"> The current PillFactory.</param>
        public ScoreSprite(Game1 game,int screenWidth, int screenHeight,Paddle paddle, Ball ball,PillFactory pillFactory)
            : base(game)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.game = game;
            this.paddle = paddle;
            this.ball = ball;
            this.pillFactory = pillFactory;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            //Subscribers to the events.
            ball.BrickExploded += updateScore;
            pillFactory.pillCaught += checkIfLifePill;
            ball.GroundHit += ballTouchesGround;

            gameScore = 0;
            gameLifes = 3;

            highScore = new HighScore(0);

            base.Initialize();
        }

        /// <summary>
        /// Subscribes to the brickHit ball event to determine the current score
        /// </summary>
        /// <param name="brick"> The brick that has been hit..</param>
        void updateScore(Brick brick)
        {
            gameScore += brick.Points;

        }


        /// <summary>
        /// Subscribes to the loseLife ball event to check how many lives are left.
        /// </summary>
        void ballTouchesGround()
        {
            gameLifes -= 1;
        }

        /// <summary>
        /// Subscribes to the pillCaught event to determine if a life is added or not.
        /// </summary>
        /// <param name="pill">The pill caught by the paddle.</param>
        void checkIfLifePill(Pill pill)
        {
            gameScore += pill.Score;
            if (pill is LifePill)
            {gameLifes += 1;}
        }

        public override void Draw(GameTime gameTime)
        {

            string scoreString = ("Score: " + gameScore);
            string livesString = ("Lives: " + gameLifes);

            Vector2 FontOriginSc = scoreAndLifesFont.MeasureString(scoreString) / 2;
            Vector2 FontOriginLi = scoreAndLifesFont.MeasureString(livesString) / 2;

            spriteBatch.Begin();
            spriteBatch.DrawString(scoreAndLifesFont, livesString, livePos, Color.White, 0, FontOriginLi, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(scoreAndLifesFont, scoreString, scorePos, Color.White, 0, FontOriginSc, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.End();



            base.Draw(gameTime);
        }

      

        protected override void LoadContent()
        {

            livePos = new Vector2(screenWidth - 100, 16);
            scorePos = new Vector2((screenWidth / 2) - 30, 16);



            spriteBatch = new SpriteBatch(GraphicsDevice);

            //playerLives = game.Content.Load<SpriteFont>("playerLives");
            scoreAndLifesFont = game.Content.Load<SpriteFont>("scoreAndLifesFont");



            centerFirstLine = new Vector2((screenWidth / 2) - (screenWidth/4), screenHeight / 2 - scoreAndLifesFont.LineSpacing - 50);
            centerSecondLine = new Vector2((screenWidth / 2) - (screenWidth / 4), screenHeight / 2 - 25);
            centerThirdLine = new Vector2((screenWidth / 2) - (screenWidth / 4), screenHeight / 2 + scoreAndLifesFont.LineSpacing - 25);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            checkIfGameOver();
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if the game is over and displays the score.
        /// </summary>
        void checkIfGameOver()
        {
            if(gameLifes == 0)
            {
                
                highScore = new HighScore(gameScore);

                String finalScore = ("Final Score : " + gameScore);
                String finalHighScore = ("Current High Score : " + highScore.CurrentHighScore);

                this.game.RemoveWallAndBall();
            spriteBatch.Begin();
            spriteBatch.DrawString(scoreAndLifesFont, "GAME OVER!", centerFirstLine, Color.White);
             spriteBatch.DrawString(scoreAndLifesFont, finalScore, centerSecondLine, Color.White);
             spriteBatch.DrawString(scoreAndLifesFont, finalHighScore, centerThirdLine, Color.White);
            spriteBatch.End();


            }
        }



    }
}
