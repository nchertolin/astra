using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Final
    {
        public static Texture2D Background = SplashScreen.Background;
        public static int timeCounter = 0;
        public static Color color;
        public static SpriteFont bigFont = SplashScreen.bigFont;
        public static SpriteFont medFont = SplashScreen.medFont;
        public static SpriteFont smallFont = SplashScreen.smallFont;
        public static int score;
        readonly static int highScore = Score.GetHighScore;
        static int newHighScore;
        static int newScore;

        public static void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(bigFont, "game over", new Vector2(width * 35 / 96, height * 11 / 36), Color.White);
            if (score > highScore)
            {
                spriteBatch.DrawString(medFont, $"new record! congratulations!", new Vector2(width * 21 / 64, height * 5 / 12), Color.White);
                spriteBatch.DrawString(medFont, $"high score: {newHighScore}", new Vector2(width * 53 / 128, height * 55 / 108), Color.White);
            }
            else
            {
                spriteBatch.DrawString(medFont, $"your score: {newScore}", new Vector2(width * 53 / 128, height * 5 / 12), Color.White);
                spriteBatch.DrawString(medFont, $"high score: {highScore}", new Vector2(width * 79 / 192, height * 55 / 108), Color.White);
            }
            spriteBatch.DrawString(smallFont, "press \'space\' to restart", new Vector2(width * 13 / 32, height * 65 / 108), color);
            spriteBatch.DrawString(smallFont, "press \'esc\' to exit", new Vector2(width * 5 / 12, height * 25 / 36), color);
        }

        public static void Update()
        {
            if (score > highScore) Score.WriteHighScore(score);

            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 150);
            timeCounter++;
            if (timeCounter == 1000) timeCounter = 0;

            if (newHighScore < score) newHighScore++;
        
            if (newScore < score) newScore++;
        }
    }
}
