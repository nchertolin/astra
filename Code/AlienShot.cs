using System;
using Microsoft.Xna.Framework;

namespace AstraLostInSpace
{
    public class AlienShot
    {
        Vector2 pos;
        Vector2 dir;
        const int Speed = 6;

        public Vector2 GetShotPos => pos;

        public bool IsHidden => pos.X < GameLogic.width * 7 / 24 || pos.X > GameLogic.width * 17 / 24 || pos.Y > GameLogic.height || pos.Y < 0;

        public AlienShot(Vector2 position, Vector2 ShipPosition)
        {
            pos = position; 

            if (pos.Y >= 0 && pos.Y <= GameLogic.height)
            {
                var dx = ShipPosition.X - pos.X; var dy = ShipPosition.Y - pos.Y;
                var d = Math.Sqrt(dx * dx + dy * dy);
                dir = new Vector2((float)(dx / d * Speed), (float)(dy / d * Speed));
            }
        }

        public void Update() => pos += dir;

        public void Draw(int frame)
        {
            GameLogic.SpriteBatch.Draw(Shot.texture2D, pos, new Rectangle(48 * (frame % 2), 0, 48, 48), Color.White);
        }
    }
}
