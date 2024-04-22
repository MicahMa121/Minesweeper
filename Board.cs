using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection;

namespace Minesweeper
{
    internal class Board
    {
        private int _width, _height, _numMines;
        private Texture2D _rectTexture;
        private SpriteFont _font;
        private MouseState _mouseState, _prevMouseState;
        private List<Rectangle> _rects;
        public Board(Texture2D texture, int width, int height, int numMines, SpriteFont font)
        {
            _width = width;
            _height = height;
            
            _numMines = numMines;
            _rectTexture = texture;

            _font = font;
        }
        private void NewBoard()
        {

        }
        public void Update(MouseState mousestate, MouseState prevmousestate)
        {
            _mouseState = mousestate;
            _prevMouseState = prevmousestate;

        }
    }
}
