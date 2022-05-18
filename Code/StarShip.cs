using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AstraLostInSpace
{
    public class StarShip
    {
        enum TextureType
        {
            Stay,
            Right,
            Left,
            Move
        }
        Vector2 pos;
        Color color = Color.White;

        public int speed = 4;
        public static Texture2D texture;
        public static SoundEffect hitSound;
        int health = 3;
        public bool IsGameOver => health == 0;
        public bool IsGod;
        bool isDamaged;
        float elapsed, elapsedToGod;
        public int expFrameCount = 0;
        readonly float godTimer = 2000;

        TextureType textureType;

        public Vector2 GetPosForShot => pos;

        public void TakeDamage()
        {
            hitSound.Play();
            health--;
            color = Color.Black;
            if (!IsGameOver) pos = new Vector2(GameLogic.width / 2 - 15, GameLogic.height * 22 / 27);
            isDamaged = true;
        }

        public void BecomeGod()
        {

            IsGod = true;
            color = Color.FromNonPremultiplied(255, 255, 255, 50);
        }

        public void Respawn()
        {
            IsGod = false;
            color = Color.White;
        }

        public StarShip(Vector2 position)
        {
            pos = position;
            SoundEffect.MasterVolume = 0.03f;
        }

        public void Up()
        {
            if (pos.Y > 0 && !IsGameOver)
            {
                textureType = TextureType.Move;
                pos.Y -= speed;
            }
        }

        public void Down()
        {
            if (pos.Y < GameLogic.height - 50 && !IsGameOver)
            {
                textureType = TextureType.Move;
                pos.Y += speed;
            }
        }

        public void Left()
        {
            if (pos.X > GameLogic.playZoneX1 && !IsGameOver)
            {
                textureType = TextureType.Left;
                pos.X -= speed;
            }
        }
        public void Right()
        {
            if (pos.X < GameLogic.playZoneX2 && !IsGameOver)
            {
                textureType = TextureType.Right;
                pos.X += speed;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (expFrameCount > 40) AstraLostInSpace.stat = Stat.Final;
            if (health > 0)
            {
                if (isDamaged)
                {
                    elapsedToGod += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedToGod >= 400)
                    {
                        isDamaged = false;
                        BecomeGod();
                        elapsedToGod = 0;
                    }
                }
                if (IsGod)
                {
                    elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsed >= godTimer)
                    {
                        elapsed = 0;
                        Respawn();
                    }
                }
            }
            else expFrameCount++;
        }

        public void Draw(int frame)
        {
            if (health > 0)
            {
                if (textureType == TextureType.Stay)
                    GameLogic.SpriteBatch.Draw(texture, pos, new Rectangle(95, 72 * (frame % 2), 52, 72), color);
                else if (textureType == TextureType.Right)
                    GameLogic.SpriteBatch.Draw(texture, pos, new Rectangle(197, 72 * (frame % 2), 40, 72), color);
                else if (textureType == TextureType.Left)
                    GameLogic.SpriteBatch.Draw(texture, pos, new Rectangle(5, 72 * (frame % 2), 40, 72), color);
                else
                {
                    GameLogic.SpriteBatch.Draw(texture, pos, new Rectangle(51, 72 * (frame % 2), 46, 72), color);
                }
                textureType = TextureType.Stay;
            }
           else GameLogic.SpriteBatch.Draw(SmallAlien.expTexture, pos, new Rectangle(48 * frame, 0, 48, 48), Color.White);
        }

    }

    public class Healths
    {
        Vector2 pos;
        public static Texture2D texture;

        public Healths(Vector2 position) => pos = position;

        public void Draw() => GameLogic.SpriteBatch.Draw(texture, pos, new Rectangle(32, 0, 17, 24), Color.White);

        public void Update() { }
    }  
}
