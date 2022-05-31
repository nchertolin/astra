using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Meteor
    {
        private Vector2 _pos;
        private Vector2 _dir;
        private int _speed = 7;
        private int delay = 2000;
        private int elapsed = 0;
        public static Texture2D texture;

        public Vector2 GetPosition => _pos;

        public Meteor(Vector2 position, Vector2 shipPosition)
        {
            _pos = position;
            _dir = CalculateDirection(shipPosition);
        }

        public void Update(Vector2 shipPosition, GameTime gameTime)
        {
            if (_pos.Y >= GameLogic.height || _pos.X < GameLogic.playZoneX1
                || _pos.X > GameLogic.playZoneX2 || _pos.Y <= 0)
            {
                elapsed += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsed >= delay)
                {
                    Respawn(shipPosition);
                }
            }
            else _pos += _dir;
        }

        public void Draw()
        {
            if (elapsed == 0)
            GameLogic.SpriteBatch.Draw(texture, _pos, new Rectangle(9, 0, 32, 32), Color.White);
        }

        private Vector2 CalculateDirection(Vector2 shipPosition)
        {
            var dx = shipPosition.X - _pos.X; var dy = shipPosition.Y - _pos.Y;
            var d = System.Math.Sqrt(dx * dx + dy * dy);
            return new Vector2((float)(dx / d * _speed), (float)(dy / d * _speed));
        }

        private Vector2 RandomSet()
        => GameLogic.GetRnd(1, 3) % 2 == 0
                ? new Vector2(GameLogic.playZoneX1, GameLogic.GetRnd(0, GameLogic.height / 2))
                : new Vector2(GameLogic.playZoneX2, GameLogic.GetRnd(0, GameLogic.height / 2));

        private void Respawn(Vector2 shipPosition)
        {
            elapsed = 0;
            _pos = RandomSet();
            _dir = CalculateDirection(shipPosition);
        }

        public void SetElapsed(int value) => elapsed = value;
    }
}
