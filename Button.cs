using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Minesweeper
{
    internal class Button
    {
        private Texture2D _rectTexture;
        private SpriteFont _font;
        private Rectangle _rect;
        private string _text;
        private Color _color;
        private Vector2 _center;
        private bool _visible;
        public Button( Texture2D rectTexture, SpriteFont font, Rectangle rect, string text, Color color, bool visible)
        {
            _rect = rect;
            _font = font;
            _rectTexture = rectTexture;
            _text = text;
            _color = color;
            _center = new Vector2(_rect.X + _rect.Width / 2, _rect.Y + _rect.Height / 2);
            _visible = visible;
        }
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public bool Contain(MouseState state)
        {
            return _rect.Contains(state.Position);
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_rectTexture, _rect,_color);
            Vector2 txtPosition = new Vector2(_center.X - _font.MeasureString(_text).X / 2, _center.Y - _font.MeasureString(_text).Y / 2);
            spriteBatch.DrawString(_font, _text, txtPosition, Color.Black);
        }


    }
}
