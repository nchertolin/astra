using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class GameLogic
    {
        public static int width, height, playZoneX1, playZoneX2;
        public static Random rnd = new Random();
        public static SpriteBatch SpriteBatch { get; set; }

        static Star[] stars;
        static SmallAlien[] aliensSmall;
        static MedAlien[] aliensMed;
        static BigAlien[] aliensBig;

        public static StarShip starShip { get; set; }
        static List<Shot> shots;
        public static List<AlienShot> alienShots;
        static List<Healths> healths;

        static float elapsed;
        static float elapsedForGuide;
        static float delay;
        static int frame;
        static bool isPowerUp;

        static readonly SpriteFont font = SplashScreen.medFont;
        static readonly SpriteFont guideFont = SplashScreen.smallFont;
        public static int Score { get; private set; }

        public static SoundEffect Laser { get; set; }
        public static SoundEffect Hit { get; set; }
        public static SoundEffect Explosion { get; set; }

        public static int GetRnd(int min, int max) => rnd.Next(min, max);

        public static void StarShipShot()
        {
            if (Score < 2 || Score % 20 >= 0 && Score % 20 < 10)
            {
                isPowerUp = false;
                shots.Add(new Shot(starShip.GetPosForShot));
            }
            else
            {
                isPowerUp = true;
                for (var i = -1; i < 2; i++)
                {
                    shots.Add(new Shot(new Vector2(starShip.GetPosForShot.X + 20 * i, starShip.GetPosForShot.Y)));
                }
            }
            Laser.Play();
        }

        public static void Initialize(SpriteBatch spriteBatch, int Width, int Height)
        {
            width = Width; height = Height; playZoneX1 = width * 7 / 24; playZoneX2 = width * 17 / 24 + 1;
            SpriteBatch = spriteBatch;
            stars = new Star[90];
            aliensSmall = new SmallAlien[2]; aliensMed = new MedAlien[1]; aliensBig = new BigAlien[1];
            Score = 0; frame = 0; frame = 0;
            delay = 300;

            SoundEffect.MasterVolume = 0.03f;

            for (var i = 0; i < stars.Length - 1; i += 2)
            {
                stars[i] = new Star(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(0, height)), new Vector2(0, 1f), Color.White);
                stars[i + 1] = new Star(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(0, height)), new Vector2(0, 0.8f), Color.FromNonPremultiplied(60, 60, 60, 255));
            }

            for (var i = 0; i < aliensSmall.Length; i++)
                aliensSmall[i] = new SmallAlien(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(-height * 5 / 18, -height * 5 / 54)), new Vector2(0, 0.9f));

            for (var i = 0; i < aliensMed.Length; i++)
            {
                aliensMed[i] = new MedAlien(new Vector2(GetRnd(playZoneX1 + 20, playZoneX2 - 20), GetRnd(-height * 5 / 9, -height * 35 / 108)), new Vector2(0, 0.7f));
                aliensBig[i] = new BigAlien(new Vector2(GetRnd(playZoneX1 + 30, playZoneX2 - 30), GetRnd(-height * 20 / 27, -height * 65 / 108)), new Vector2(0, 0.5f));
            }

            starShip = new StarShip(new Vector2(Width / 2 - 20, Height * 22 / 27)); healths = new List<Healths>();
            shots = new List<Shot>();

            alienShots = new List<AlienShot>();

            for (var i = 0; i < 3; i++)
                healths.Add(new Healths(new Vector2(playZoneX1 + 23 * i, height * 67 / 72)));
        }

        #region Анимация
        public static void Animate(GameTime gameTime)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsed >= delay - 100)
            {
                if (frame == 4)
                    frame = 0;
                else frame++;
                elapsed = 0;
            }
        }
        #endregion

        public static void Draw()
        {
            foreach (var star in stars) star.Draw();
            starShip.Draw(frame);
            foreach (var shot in shots) shot.Draw(frame);

            if (elapsedForGuide < 6000)
            {
                if (elapsedForGuide < 1500)
                    SpriteBatch.DrawString(guideFont, "\'wasd\' to move", new Vector2(width * 43 / 96, height / 2), Color.White);
                else if (elapsedForGuide < 3000)
                    SpriteBatch.DrawString(guideFont, "\'enter\' to fire", new Vector2(width * 43 / 96, height / 2), Color.White);
                else if (elapsedForGuide < 4500)
                    SpriteBatch.DrawString(guideFont, "\'p\' to pause", new Vector2(width * 89 / 192, height / 2), Color.White);
                else
                    SpriteBatch.DrawString(font, "enjoy!", new Vector2(width * 91 / 192, height / 2), Color.White);
            }
            else
            {
                if (isPowerUp) SpriteBatch.DrawString(guideFont, "x3", new Vector2(playZoneX1, height * 97 / 108), Color.White);

                SpriteBatch.DrawString(font, Score.ToString(), new Vector2(playZoneX1, height * 26 / 27), Color.White);
                foreach (var health in healths) health.Draw();
                foreach (var alien in aliensSmall) alien.Draw(frame);
                foreach (var alien in aliensMed) alien.Draw(frame);
                foreach (var alien in aliensBig) alien.Draw(frame);
                foreach (var shot in alienShots) shot.Draw(frame);
            }
        }

        #region Попадание
        static bool IsHit(Vector2 shotPos, Vector2 alienPos, int eps)
            => shotPos.X >= alienPos.X - eps && shotPos.X <= alienPos.X + eps
                && shotPos.Y >= alienPos.Y - eps && shotPos.Y <= alienPos.Y + eps;
        # endregion

        #region Увеличить счет
        static void AddScore(int magnifier)
        {
            global::AstraLostInSpace.Score.IncreaseScore(magnifier);
            Score = global::AstraLostInSpace.Score.GetCurrentScore;
        }
        # endregion

        static void HitAlien(Alien alien, int i, int magnifier)
        {
            alien.TakeDamage();
            shots.RemoveAt(i);
            if (alien.isDie)
            {
                AddScore(magnifier);
                Explosion.Play();
            }
            else Hit.Play();
        }

        public static void Update(GameTime gameTime)
        {
            if (starShip.IsGameOver) Final.score = Score;
            if (elapsedForGuide < 6000) elapsedForGuide += gameTime.ElapsedGameTime.Milliseconds;

            Animate(gameTime);
            foreach (var star in stars) star.Update();
            starShip.Update(gameTime);

            if (elapsedForGuide >= 6000)
            {
                Parallel.Invoke(
                     () => { foreach (var alien in aliensSmall) alien.Update(gameTime, starShip, 1200); },
                     () => { foreach (var alien in aliensMed) alien.Update(gameTime, starShip, 1000); },
                     () => { foreach (var alien in aliensBig) alien.Update(gameTime, starShip, 800); }
                     );

                #region Выстрелы пришельцев
                for (var i = 0; i < alienShots.Count; i++)
                {
                    var alienShot = alienShots[i];
                    alienShot.Update();
                    if (!starShip.IsGod && !starShip.IsGameOver && IsHit(alienShot.GetShotPos, starShip.GetPosForShot, 20))
                    {
                        starShip.TakeDamage();
                        healths.RemoveAt(healths.Count - 1);
                        alienShots.RemoveAt(i);
                        i--;
                    }

                    if (alienShot.IsHidden)
                    {
                        alienShots.RemoveAt(i);
                        i--;
                    }
                }
                #endregion
            }

            #region Выстрелы игрока
            for (var i = 0; i < shots.Count; i++)
            {
                var shot = shots[i];
                shot.Update();

                Parallel.Invoke(
                    () =>
                    {   foreach (var alien in aliensSmall)
                        {
                            if (IsHit(shot.GetShotPos, alien.GetAlienPos, 20))
                            {
                                HitAlien(alien, i, 1);
                                i--;
                            }}},

                    () =>
                    {   foreach (var alien in aliensMed)
                        {
                            if (IsHit(shot.GetShotPos,
                                new Vector2(alien.GetAlienPos.X + 30, alien.GetAlienPos.Y), 40))
                            {
                                HitAlien(alien, i, 5);
                                i--;
                            }}},

                    () =>
                    {   foreach (var alien in aliensBig)
                        {
                            if (IsHit(shot.GetShotPos,
                                new Vector2(alien.GetAlienPos.X + 30, alien.GetAlienPos.Y), 40))
                            {
                                HitAlien(alien, i, 10);
                                i--;
                            }}});

                if (shot.IsHidden)
                {
                    shots.RemoveAt(i);
                    i--;
                }
            }
            #endregion
        }
    }
}
