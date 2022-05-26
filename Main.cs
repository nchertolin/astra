using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace AstraLostInSpace
{
    public enum Stat
    {
        SplashScreen,
        Game,
        Final,
        Pause
    }

    public class AstraLostInSpace : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Stat stat = Stat.SplashScreen;
        KeyboardState keyState, oldKeyState;
        Song MainTheme;
        const int Delay = 350;
        float elapsed;
        int width, height;

        public AstraLostInSpace()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
            width = graphics.PreferredBackBufferWidth; height = graphics.PreferredBackBufferHeight;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SplashScreen.Background = Content.Load<Texture2D>("newBackground");
            SplashScreen.bigFont = Content.Load<SpriteFont>("bigFont");
            SplashScreen.medFont = Content.Load<SpriteFont>("medFont");
            SplashScreen.smallFont = Content.Load<SpriteFont>("smallFont");

            GameLogic.Initialize(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            GameLogic.Laser = Content.Load<SoundEffect>("laser");
            GameLogic.Hit = Content.Load<SoundEffect>("hit");
            GameLogic.Explosion = Content.Load<SoundEffect>("explode");

            Star.texture = Content.Load<Texture2D>("stars");
            StarShip.texture = Content.Load<Texture2D>("ship");
            StarShip.hitSound = Content.Load<SoundEffect>("jump");
            Healths.texture = Content.Load<Texture2D>("health");
            Shot.texture2D = Content.Load<Texture2D>("lasers");

            SmallAlien.texture = Content.Load<Texture2D>("enemySmall");
            MedAlien.texture = Content.Load<Texture2D>("enemyMed");
            BigAlien.texture = Content.Load<Texture2D>("enemyBig");
            Alien.expTexture = Content.Load<Texture2D>("explosion");

            MainTheme = Content.Load<Song>("main");
            MediaPlayer.Play(MainTheme);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
        }

        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            switch (stat)
            {
                case Stat.SplashScreen:
                    MediaPlayer.Pause();
                    SplashScreen.Update();
                    if (keyState.IsKeyDown(Keys.Space)) stat = Stat.Game;
                    if (keyState.IsKeyDown(Keys.Escape) && !oldKeyState.IsKeyDown(Keys.Escape)) Exit();
                        break;

                case Stat.Pause:
                    MediaPlayer.Pause();
                    Pause.Update();
                    if (keyState.IsKeyDown(Keys.Space)) stat = Stat.Game;
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        stat = Stat.SplashScreen;
                        ResetGame();
                    }
                    break;

                case Stat.Final:
                    MediaPlayer.Pause();
                    Final.Update();
                    if (keyState.IsKeyDown(Keys.Space))
                    {
                        stat = Stat.Game;
                        ResetGame();
                    }

                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        stat = Stat.SplashScreen;
                        ResetGame();
                    }
                    break;

                case Stat.Game:
                    elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    MediaPlayer.Resume();
                    GameLogic.Update(gameTime);

                    if (keyState.IsKeyDown(Keys.P)) stat = Stat.Pause;
                    if (keyState.IsKeyDown(Keys.W)) GameLogic.starShip.Up() ;
                    if (keyState.IsKeyDown(Keys.A)) GameLogic.starShip.Left();
                    if (keyState.IsKeyDown(Keys.S)) GameLogic.starShip.Down();
                    if (keyState.IsKeyDown(Keys.D)) GameLogic.starShip.Right();
                    if (keyState.IsKeyDown(Keys.Enter) && oldKeyState.IsKeyUp(Keys.Enter)
                        && elapsed >= Delay)
                    {
                        GameLogic.StarShipShot();
                        elapsed = 0;
                    }
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        stat = Stat.SplashScreen;
                        ResetGame();
                    }
                        break;
            }

            oldKeyState = keyState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (stat)
            {
                case Stat.SplashScreen:
                    SplashScreen.Draw(spriteBatch, width, height);
                    break;

                case Stat.Pause:
                    Pause.Draw(spriteBatch, width, height);
                    break;

                case Stat.Final:
                    Final.Draw(spriteBatch, width, height);
                    break;

                case Stat.Game:
                    GameLogic.Draw();
                    break;
            }

            spriteBatch.End(); 

            base.Draw(gameTime);
        }

        void ResetGame() => GameLogic.Initialize(spriteBatch, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
    }
}
