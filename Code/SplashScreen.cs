using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public static class SplashScreen
    {
        public static Texture2D Background { get; set; }
        public static int timeCounter = 0;
        public static Color color;
        public static SpriteFont bigFont;
        public static SpriteFont medFont;
        public static SpriteFont smallFont;

        public static void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            spriteBatch.Draw(Background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(bigFont, "astra", new Vector2(width * 263 / 640, height * 391 / 1080), Color.White);
            spriteBatch.DrawString(medFont, "lost in space", new Vector2(width * 33 / 80, height * 4 / 9), Color.White);
            spriteBatch.DrawString(smallFont, "press \'space\' to play", new Vector2(width * 33 / 80, height * 8 / 15), color);
            spriteBatch.DrawString(smallFont, "press \'esc\' to exit", new Vector2(width * 27 / 64, height * 73 / 120), color);
        }

        public static void Update()
        {
            color = Color.FromNonPremultiplied(255, 255, 255, timeCounter % 150);
            timeCounter++;
            if (timeCounter == 1000) timeCounter = 0;

        }
    }
}
