using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
        private Rectangle _boardRect;
        private Random gen = new Random();
        private Texture2D _bombTexture;
        private Box[,] _board;

        public Board(Texture2D texture,Texture2D bombtexture, int width, int height, int numMines, Rectangle board,SpriteFont font)
        {
            _width = width;
            _height = height;
            _boardRect = board;
            _numMines = numMines;
            _rectTexture = texture;
            _bombTexture = bombtexture;
            _font = font;
        }
        private void NewBoard()
        {
            _board = new Box[_width, _height];
            List<int[,]> bombs = new List<int[,]> ();
            for (int i = 0; i < _numMines; i++)
            {
                int[,] bomb = new int[gen.Next(0,_width), gen.Next(0,_height)];
                bombs.Add(bomb);
            }
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0;  j < _height; j++)
                {
                    int width = _boardRect.Width / _width;
                    int height = _boardRect.Height / _height;
                    Rectangle rect = new Rectangle(_boardRect.X + width * i, _boardRect.Y + height * j, width, height);
                    _rects.Add(rect);
                    int value;
                    if (bombs.Contains(new int[i, j]))
                    {
                        value = 9;
                    }
                    else
                    {
                        value = 0;
                    }
                    Box box = new Box(_rectTexture,_bombTexture,rect,value, Color.LightGray,_font);
                    _board[i,j] = box;
                }
            }
            UpdateValues();
        }
        private void UpdateValues()
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    int value = 0;
                    for (int k = 0; k < 3 ; k++)
                    {
                        for (int l = 0; l < 3 ; l++)
                        {
                            if (_board[i - 1 + k, j - 1 + l] != null && !(k == 1 && l==1)&& _board[i - 1+k, j - 1+l].Value == 9)
                            {
                                value++;
                            }
                        }
                    }
                    _board[i,j].Value = value;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _board[i,j].Draw(spriteBatch);
                }
            }
        }
        public void Update(MouseState mousestate, MouseState prevmousestate)
        {
            _mouseState = mousestate;
            _prevMouseState = prevmousestate;

        }
    }
}
