﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
    internal class Box
    {
        private Texture2D _texture;
        private Rectangle _position;
        private int _value;
        private Color _color,_txtColor;
        private SpriteFont _font;
        private Vector2 _center;
        private Texture2D _bombTexture;

        public Box(Texture2D texture, Texture2D bombtexture, Rectangle position, int value, Color color,  SpriteFont font)
        {
            _texture = texture;
            _bombTexture = bombtexture;
            _position = position;
            _value = value;
            _color = color;
            _font = font;
            _center = new Vector2(_position.X + _position.Width / 2, _position.Y + _position.Height / 2);
            _txtColor = Color.Black;
        }

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _color);
            if (_value == 9 )
            {
                spriteBatch.Draw(_bombTexture, _position, _color);
            }
            else 
            {
                string txt = _value.ToString();
                Vector2 txtPosition = new Vector2(_center.X - _font.MeasureString(txt).X / 2, _center.Y - _font.MeasureString(txt).Y / 2);
                if (Value != 0)
                    spriteBatch.DrawString(_font, txt, txtPosition, _txtColor);
            }
        }
    }
}