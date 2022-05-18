using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AstraLostInSpace
{
    public class Star
    {
        Vector2 _pos;
        Vector2 _dir;
        Color color;
        readonly static int playZoneX1 = GameLogic.playZoneX1;
        readonly static int playZoneX2 = GameLogic.playZoneX2;
        public static Texture2D texture;

        public Star(Vector2 position, Vector2 direction, Color color)
        {
            _pos = position;
            _dir = direction;
            this.color = color;
        }

        public void Update()
        {
            _pos += _dir;
            if (_pos.Y > GameLogic.height) RandomSet();
        }

        public void RandomSet() =>
            _pos = new Vector2(GameLogic.GetRnd(playZoneX1, playZoneX2), GameLogic.GetRnd(-800, 0));

        public void Draw() => GameLogic.SpriteBatch.Draw(texture, _pos, color);
    }
}
