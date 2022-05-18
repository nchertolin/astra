using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Alien
    {
        public Vector2 pos;
        public Vector2 dir;
        public static Texture2D expTexture;
        public Color color = Color.White;
        public int maxHealth;
        public int health;
        public int expFrameCount = 0;
        public int elapsed = 0;
        public readonly static int playZoneX1 = GameLogic.playZoneX1;
        public readonly static int playZoneX2 = GameLogic.playZoneX2;


        public Vector2 GetAlienPos => pos;
        public bool isDie => health == 0;

        public Alien() { }

        public Alien(Vector2 position, Vector2 direction)
        {
            pos = position;
            dir = direction;
            health = maxHealth;
        }

        public virtual void Shot(Vector2 ShipPos)  => 
            GameLogic.alienShots.Add(new AlienShot(GetAlienPos, ShipPos));

        public virtual void TakeDamage()
        {
            health--;
            color = Color.Red;
        }

        public virtual void Respawn()
        {
            expFrameCount = 0;
            health = maxHealth;
            RandomSet();
        }

        public virtual void RandomSet() { }

        public virtual void Update(GameTime gameTime, StarShip starShip, int shotDelay)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            pos += dir;
            if (pos.Y > GameLogic.height || expFrameCount > 30) Respawn();
            else if (health <= 0) expFrameCount++;

            if (!starShip.IsGod && !starShip.IsGameOver && pos.Y >= 0 && pos.Y <= GameLogic.height && elapsed >= shotDelay)
            {
                Shot(starShip.GetPosForShot);
                elapsed = 0;
            }
            color = Color.White;
        }

        public virtual void Draw(int frame) { }
    }

    public class SmallAlien : Alien
    {
        public static Texture2D texture;

        public SmallAlien(Vector2 position, Vector2 direction)
        {
            pos = position;
            dir = direction;
            maxHealth = 1;
            health = maxHealth;
            expFrameCount = 0;
        }

        public override void RandomSet() =>
            pos = new Vector2(GameLogic.GetRnd(playZoneX1, playZoneX2),GameLogic.GetRnd(-300, -100));

        public override void Draw(int frame)
        {
            if (health > 0)
                GameLogic.SpriteBatch.Draw(texture, pos,
                    new Rectangle(48 * (frame % 2), 0, 48, 48), color);
            else
                GameLogic.SpriteBatch.Draw(expTexture, pos,
                    new Rectangle(48 * frame, 0, 48, 48), Color.White);
        }
    }

    public class MedAlien : Alien
    {
        public static Texture2D texture;

        public MedAlien(Vector2 position, Vector2 direction)
        {
            pos = position;
            dir = direction;
            maxHealth = 5;
            health = maxHealth;
            expFrameCount = 0;
        }

        public override void RandomSet() =>
           pos = new Vector2(GameLogic.GetRnd(playZoneX1, playZoneX2), GameLogic.GetRnd(-700, -600));

        public override void Draw(int frame)
        {
            if (health > 0)
                GameLogic.SpriteBatch.Draw(texture, pos,
                    new Rectangle(96 * (frame % 2), 0, 96, 48), color);
           else
                GameLogic.SpriteBatch.Draw(expTexture, new Vector2(pos.X + 20, pos.Y),
            new Rectangle(48 * frame, 0, 48, 48), Color.White);
        }
    }

    public class BigAlien : Alien
    {
        public static Texture2D texture;
        public BigAlien(Vector2 position, Vector2 direction)
        {
            pos = position;
            dir = direction;
            maxHealth = 10;
            health = maxHealth;
            expFrameCount = 0;
        }

        public override void RandomSet() =>
          pos = new Vector2(GameLogic.GetRnd(playZoneX1, playZoneX2), GameLogic.GetRnd(-1000, -900));

        public override void Update(GameTime gameTime, StarShip starShip, int shotDelay)
        {
            elapsed += gameTime.ElapsedGameTime.Milliseconds;
            pos += dir;
            if (pos.Y > GameLogic.height || expFrameCount > 20) Respawn();
            else if (health <= 0) expFrameCount++;

            if (!starShip.IsGod && !starShip.IsGameOver && pos.Y >= 0 && pos.Y <= GameLogic.height && elapsed >= shotDelay)
            {
                var shipPos = starShip.GetPosForShot;
                for (var i = 0; i < GameLogic.GetRnd(2, 4); i++)
                {
                    GameLogic.alienShots.Add(new AlienShot(new Vector2(GetAlienPos.X + 30, GetAlienPos.Y + 30),
                        new Vector2(shipPos.X + i * 30, shipPos.Y)));
                }
                elapsed = 0;
            }
            color = Color.White;
        }

        public override void Draw(int frame)
        {
            if (health > 0)
                GameLogic.SpriteBatch.Draw(texture, pos,
                    new Rectangle(96 * (frame % 2), 0, 96, 96), color);
           else
                GameLogic.SpriteBatch.Draw(expTexture, new Vector2(pos.X + 30, pos.Y + 30),
                    new Rectangle(48 * frame, 0, 48, 48), Color.White);
        }
    }  
  
}