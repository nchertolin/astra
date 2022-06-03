using System.IO;
using System.Text;

namespace AstraLostInSpace
{
    public static class Score
    {
        static int currentScore, highScore;

        public static int GetCurrentScore => currentScore;
        public static int GetHighScore => ReadHighScore();

        public static void IncreaseScore(int magnifier)
        {
            currentScore += magnifier;
            highScore = currentScore > highScore ? currentScore : highScore;
        }

        public static void WriteHighScore(int highScore)
        {
            var sw = new StreamWriter("HighScore.txt", false, Encoding.ASCII);
            sw.WriteLine(highScore);
            sw.Close();
        }


        public static int ReadHighScore()
        {
            try
            {
                var sr = new StreamReader("HighScore.txt");
                var highScore = int.Parse(sr.ReadLine());
                sr.Close();
                return highScore;
            }
            catch (System.Exception)
            {
                using (FileStream fs = File.Create("HighScore.txt"))
                {
                    fs.Write(new UTF8Encoding(true).GetBytes("0", 0, 1));
                }
                return 0;
            }
        }
    }
}
