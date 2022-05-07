

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Shot
    {
        Vector2 _pos;
        Vector2 _dir;
        const int Speed = 7;
        public static Texture2D texture2D;

        public Vector2 GetShotPos => _pos;

        public bool IsHidden => _pos.Y <= 0;

        public Shot(Vector2 position)
        {
            _pos = position;
            _dir = new Vector2(0, -Speed);
        }

        public void Update()
        {
            if (_pos.Y > 0) _pos += _dir;
        }

        public void Draw(int frame)
        {
            GameLogic.SpriteBatch.Draw(texture2D, _pos, new Rectangle(48 * (frame % 2), 48, 48, 48), Color.White);
        }
    }
}
