using System;
using System.Collections.Generic;
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

        static SpriteFont font = SplashScreen.medFont;
        static SpriteFont guideFont = SplashScreen.smallFont;
        public static int score { get; private set; }

        public static SoundEffect Laser { get; set; }
        public static SoundEffect Hit { get; set; }
        public static SoundEffect Explosion { get; set; }

        public static int GetRnd(int min, int max) => rnd.Next(min, max);

        public static void StarShipShot()
        {
            shots.Add(new Shot(starShip.GetPosForShot));
            Laser.Play();
        }

        public static void Initialize(SpriteBatch spriteBatch, int Width, int Height)
        {
            width = Width; height = Height; playZoneX1 = width * 7 / 24; playZoneX2 = width * 17 / 24 + 1;
            SpriteBatch = spriteBatch;
            stars = new Star[90];
            aliensSmall = new SmallAlien[2]; aliensMed = new MedAlien[1]; aliensBig = new BigAlien[1];
            score = 0; frame = 0; frame = 0;
            delay = 300;

            SoundEffect.MasterVolume = 0.03f;

            for (var i = 0; i < stars.Length - 1; i += 2)
            {
                stars[i] = new Star(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(0, height)), new Vector2(0, 1.1f), Color.White);
                stars[i + 1] = new Star(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(0, height)), new Vector2(0, 0.8f), Color.FromNonPremultiplied(60, 60, 60, 255));
            }

            for (var i = 0; i < aliensSmall.Length; i++)
                aliensSmall[i] = new SmallAlien(new Vector2(GetRnd(playZoneX1, playZoneX2), GetRnd(-height * 5 / 18, -height * 5 / 54)), new Vector2(0, 1));

            for (var i = 0; i < aliensMed.Length; i++)
            {
                aliensMed[i] = new MedAlien(new Vector2(GetRnd(playZoneX1 + 20, playZoneX2 - 20), GetRnd(-height * 5 / 9, -height * 35 / 108)), new Vector2(0, 0.7f));
                aliensBig[i] = new BigAlien(new Vector2(GetRnd(playZoneX1 + 30, playZoneX2 - 30), GetRnd(-height * 20 / 27, -height * 65 / 108)), new Vector2(0, 0.5f));
            }

            starShip = new StarShip(new Vector2(Width / 2, Height - Height * 5 / 27)); healths = new List<Healths>();
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
                    SpriteBatch.DrawString(guideFont, "\'wasd\' to move", new Vector2(width * 85 / 192, height / 2), Color.White);
                else if (elapsedForGuide < 3000)
                    SpriteBatch.DrawString(guideFont, "\'enter\' to fire", new Vector2(width * 85 / 192, height / 2), Color.White);
                else if (elapsedForGuide < 4500)
                    SpriteBatch.DrawString(guideFont, "\'p\' to pause", new Vector2(width * 11 / 24, height / 2), Color.White);
                else
                    SpriteBatch.DrawString(font, "enjoy!", new Vector2(width * 15 / 32, height / 2), Color.White);
            }
            else
            {
                SpriteBatch.DrawString(font, score.ToString(), new Vector2(playZoneX1, height * 26 / 27), Color.White);
                foreach (var health in healths) health.Draw();
                foreach (var alien in aliensSmall) alien.Draw(frame);
                foreach (var alien in aliensMed) alien.Draw(frame);
                foreach (var alien in aliensBig) alien.Draw(frame);
                foreach (var shot in alienShots) shot.Draw(frame);
            }
        }

        #region Попадание
        public static bool IsHit(Vector2 shotPos, Vector2 alienPos, int eps)
            => shotPos.X >= alienPos.X - eps && shotPos.X <= alienPos.X + eps
                && shotPos.Y >= alienPos.Y - eps && shotPos.Y <= alienPos.Y + eps;
        # endregion

        #region Увеличить счет
        public static void AddScore(int magnifier)
        {
            Score.IncreaseScore(magnifier);
            score = Score.GetCurrentScore;
        }
        # endregion

        public static void Update(GameTime gameTime)
        {
            if (starShip.IsGameOver) Final.score = score;
            if (elapsedForGuide < 6000) elapsedForGuide += gameTime.ElapsedGameTime.Milliseconds;
            Animate(gameTime);

            foreach (var star in stars) star.Update();
            starShip.Update(gameTime);

            if (elapsedForGuide >= 6000)
            {
                foreach (var alien in aliensSmall) alien.Update(gameTime, starShip, 1200);
                foreach (var alien in aliensMed) alien.Update(gameTime, starShip, 1000);
                foreach (var alien in aliensBig) alien.Update(gameTime, starShip, 800);
                for (var i = 0; i < alienShots.Count; i++)
                {
                    //var alienShot = alienShots[i];
                    alienShots[i].Update();
                    if (!starShip.IsGod && !starShip.IsGameOver && IsHit(alienShots[i].GetShotPos, starShip.GetPosForShot, 20))
                    {
                        starShip.TakeDamage();
                        healths.RemoveAt(healths.Count - 1);
                        alienShots.RemoveAt(i);
                        i--;
                    }

                    if (alienShots[i].IsHidden)
                    {
                        alienShots.RemoveAt(i);
                        i--;
                    }
                }
            }

            #region Выстрелы игрока
            for (var i = 0; i < shots.Count; i++)
            {
                var shot = shots[i];
                shot.Update();

                foreach (var alien in aliensSmall)
                {
                    if (IsHit(shot.GetShotPos, alien.GetAlienPos, 20))
                    {
                        alien.TakeDamage();
                        shots.RemoveAt(i);
                        i--;
                        if (alien.isDie)
                        {
                            AddScore(1);
                            Explosion.Play();
                        }
                        else Hit.Play();
                    }
                }

                foreach (var alien in aliensMed)
                {
                    if (IsHit(shot.GetShotPos,
                        new Vector2(alien.GetAlienPos.X + 30, alien.GetAlienPos.Y), 40))
                    {
                        alien.TakeDamage();
                        shots.RemoveAt(i);
                        i--;
                        if (alien.isDie)
                        {
                            AddScore(5);
                            Explosion.Play();
                        }
                        else Hit.Play();
                    }
                }

                foreach (var alien in aliensBig)
                {
                    if (IsHit(shot.GetShotPos,
                        new Vector2(alien.GetAlienPos.X + 30, alien.GetAlienPos.Y), 40))
                    {
                        alien.TakeDamage();
                        shots.RemoveAt(i);
                        i--;
                        if (alien.isDie)
                        {
                            AddScore(10);
                            Explosion.Play();
                        }
                        else Hit.Play();
                    }
                }

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
