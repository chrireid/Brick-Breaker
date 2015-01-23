using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Brick_Breaker
{
    class HighScore
    {
        // Class Variables
        private int currentHighScore;
        private int newHighScore;


        /// <summary>
        /// Constructor for Highscore class.
        /// </summary>
        /// <param name="newHighScore">New Highscore</param>
        public HighScore(int newHighScore)
        {
            this.newHighScore = newHighScore;

            string scorePath = "score.txt";

            // File IO Validation
            if (!File.Exists(scorePath))
            {
                File.Create(scorePath);
                using (StreamWriter sw = File.AppendText(scorePath))
                    sw.WriteLine("0");
            }

            currentHighScore = ReadScoreFromFile();
            CheckHighScore();
        } // End Constructor()


        /// <summary>
        /// Public property for highScore class
        /// </summary>
        public int CurrentHighScore
        {
            get { return currentHighScore; }
        } // End CurrentHighScore
      
       
        /// <summary>
        /// Checks if the new highscore is higher than the file high score. 
        /// </summary>
        public void CheckHighScore()
        {
            if (newHighScore > currentHighScore)
            { 
                WriteScoreToFile(newHighScore);
                currentHighScore = ReadScoreFromFile();
            }
        } // End checkHighScore()

        
        /// <summary>
        /// Write the new high score to the file.
        /// </summary>
        /// <param name="newHighScore">New high score</param>
        public static void WriteScoreToFile(int newHighScore)
        {
            using (StreamWriter file = new StreamWriter(new FileStream("score.txt", FileMode.Open, FileAccess.Write)))
                file.WriteLine(newHighScore);
        } // End WriteScoreToFile()


        /// <summary>
        /// Reads the current file highscore and puts the value in the variable.
        /// </summary>
        /// <param name="currentHighScore">Current High Score</param>
        public static int ReadScoreFromFile()
        {
            int cHighScore;

            using (System.IO.StreamReader file = new StreamReader(new FileStream("score.txt", FileMode.Open, FileAccess.Read)))
            {
                if (file.Peek() > -1) // If there is a line in the file
                {
                    try
                    {
                        cHighScore = int.Parse(file.ReadLine());
                    }
                    catch (FormatException) // If the user modified the file and broke something
                    {
                        cHighScore = 0;
                    }
                }
                else // file is empty
                    cHighScore = 0;
            }
            return cHighScore;
        } // End ReadScoreFromFile()
    }
}
