using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace Brick_Breaker
{
    class LevelFactory
    {
        // List of 2D arrays (Bricks)
        private List<String[,]> levels = new List<String[,]>();

        private Game game;
        private int screenWidth, screenHeight, currentLevel;
        private Vector2 position;
        private PillFactory pillFactory;

        Texture2D brickImage, obstacleBrickImage, powerUpBrickImageLife, powerUpBrickImageFlip;


        /// <summary>
        /// Constructor for the level factory
        /// </summary>
        /// <param name="screenWidth">current game width</param>
        /// <param name="screenHeight">current game height</param>
        /// <param name="game">current game</param>
        /// <param name="pillFactory">current pillFactory</param>
        public LevelFactory(int screenWidth, int screenHeight, Game game, PillFactory pillFactory)
        {
            // Loading the Texture Images
            brickImage = game.Content.Load<Texture2D>("brick");
            obstacleBrickImage = game.Content.Load<Texture2D>("obstaclebrick");
            powerUpBrickImageLife = game.Content.Load<Texture2D>("powerupbrick1");
            powerUpBrickImageFlip = game.Content.Load<Texture2D>("powerupbrick4");

            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.position.X = (screenWidth - (5 * brickImage.Width)) / 2;
            this.position.Y = (brickImage.Height * 4);
            this.game = game;
            this.pillFactory = pillFactory;
            this.currentLevel = 0;

            readLevels();

        } // End Constructor


        /// <summary>
        /// Reads the level file.
        /// </summary>
        private void readLevels()
        {
            string fileName = "levels.csv";

            // Try block to open file and stuff
            try
            {
                using (StreamReader str = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read)))
                {
                    // Count the number of lines
                    var lineCount = File.ReadLines(fileName).Count();

                    // Validation
                    if (lineCount < 7 || lineCount % 8 != 0)
                        throw new NotSupportedException("File does not contain appropriate number of rows.");

                    char[] delim = new char[] { ',' };

                    while (str.Peek() > -1)
                    {
                        // Initialize the level string array:
                        string[,] level = new string[8, 5];

                        for (int i = 0; i < level.GetLength(0); i++)
                        {
                            // Capture current line of the file into a string:
                            string currentLine = str.ReadLine();

                            // Split and store the string into the columns array:
                            string[] columns = currentLine.Split(delim);

                            // Validation
                            if (columns.Length != 5)
                                throw new NotSupportedException("File does not contain appropriate number of columns (commas).");

                            for (int j = 0; j < level.GetLength(1); j++)
                            {
                                // Clean up the string:
                                columns[j] = columns[j].ToUpper().Trim();

                                // Store the columns array into the level array:
                                level[i, j] = columns[j];
                            }

                        } // End for loops

                        levels.Add(level);
                    }
                } // End Using
            }
            catch (IOException)
            {
                throw new IOException("There is a problem with the 'levels.csv' file processing.");
            }
            

        } // End readLevels()



        /// <summary>
        /// Builds the level
        /// </summary>
        /// <returns>Returns the brick array</returns>
        private Brick[,] levelBuilder()
        {
            // Builds the brick wall 2D array:

            Brick[,] brickLevel = new Brick[8, 5];
            String[,] stringLevel = this.levels.ElementAt<string[,]>(currentLevel);

            for (int i = 0; i < brickLevel.GetLength(0); i++)
            {
                for (int j = 0; j < brickLevel.GetLength(1); j++)
                {
                    // Build the brick and store it
                    brickLevel[i, j] = brickBuilder(stringLevel[i, j], i, j);
                }
            }

            return brickLevel;
        } // End levelBuilder()

        /// <summary>
        /// Builds the bricks in the desired level
        /// </summary>
        /// <param name="blueprint">COntains the position of the bricks, pills and resistance</param>
        /// <param name="row">The number of rows. (lines in the file)</param>
        /// <param name="col">The number of columns ( commas in the file)</param>
        /// <returns>Returns the brick</returns>
        private Brick brickBuilder(string blueprint, int row, int col)
        {
            Brick brick = null;
            int resistance;
            int whitespace = blueprint.IndexOf(' ');
            string pillStr = "";

            if (blueprint != "")
            {
                try
                {
                    if (whitespace < 0) // non-pill brick
                    {
                        resistance = Int32.Parse(blueprint);

                        if (resistance > 0)
                            brick = new Brick(this.game, brickImage, brickPosition(row, col), resistance);
                        else if (resistance == -1)
                            brick = new ObstacleBrick(this.game, obstacleBrickImage, brickPosition(row, col));
                        else
                            throw new ArgumentException("Invalid Resistance!");
                    }
                    else // pill brick
                    {
                        resistance = Int32.Parse(blueprint.Substring(0, whitespace));
                        pillStr = blueprint.Substring(whitespace + 1);

                        if (pillStr == "L") // life brick
                            brick = new PowerUpBrick(this.game, powerUpBrickImageLife, brickPosition(row, col), resistance, pillFactory, PillType.L);
                        else if (pillStr == "F") // flip brick
                            brick = new PowerUpBrick(this.game, powerUpBrickImageFlip, brickPosition(row, col), resistance, pillFactory, PillType.F);
                        else
                            // throw exception (not F or L)
                            throw new ArgumentException("Wrong type of enum! It should be F or L!");
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException("Invalid resistance entered; value should be a number!");
                }
            }

            return brick;

        } // End brickBuilder()


        /// <summary>
        /// Method that calculates the position of the bricks on the wall.
        /// </summary>
        /// <param name="row">The row of the wall</param>
        /// <param name="col">The column of the wall</param>
        /// <returns>The brick position</returns>
        private Vector2 brickPosition(int row, int col)
        {
            return new Vector2(position.X + (brickImage.Width * col), position.Y + (brickImage.Height * row));
        } // End brickPosition()


        /// <summary>
        /// Constructs the next level in an array of bricks.
        /// </summary>
        /// <returns>The array of bricks for next level</returns>
        public Brick[,] GetNextLevel()
        {
            Brick[,] nextLevel = levelBuilder();

            // Check to ensure that the next level (int) is within the level bounds
            if (currentLevel < levels.Count - 1)
                currentLevel++;
            else
                currentLevel = 0;

            // Return the next level of bricks
            return nextLevel;
        } // End GetNextLevel()

    } // End LevelFactory

}