using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Pause
    {
        public static Texture2D Background { get; set; } = SplashScreen.Background;
        public static SpriteFont Font { get; set; } = SplashScreen.bigFont;
        public static SpriteFont FontButtons { get; set; } = SplashScreen.smallFont;
        static Color color;
        static int timeCounter = 0;

        public static void Draw(SpriteBatch _spriteBatch, int width, int height)
        {
            _spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            _spriteBatch.DrawString(Font, "paused", new Vector2(width * 77 / 192, height * 83 / 216), Color.White);
            _spriteBatch.DrawString(FontButtons, "press \'space\' to resume", new Vector2(width * 197 / 480, height * 47 / 90), color);
            _spriteBatch.DrawString(FontButtons, "press \'esc\' to back menu", new Vector2(width * 261 / 640, height * 109 / 180), color);
        }

        public static void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 150);
            timeCounter++;
            if (timeCounter == 1000) timeCounter = 0;
        }
    }
}
