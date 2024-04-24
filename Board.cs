using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;

namespace Minesweeper
{
    internal class Board
    {
        private int _width, _height, _numMines;
        private Texture2D _rectTexture;
        private SpriteFont _font;
        private MouseState _mouseState, _prevMouseState;
        private Rectangle[,] _rects;
        private Rectangle _boardRect;
        private Random gen = new Random();
        private Texture2D _bombTexture;
        private Box[,] _board;
        private string _gameState;
        private Vector2 _center;

        public Board(Texture2D texture,Texture2D bombtexture, int width, int height, int numMines, Rectangle board,SpriteFont font)
        {
            _width = width;
            _height = height;
            _boardRect = board;
            _center = new Vector2(board.X + board.Width / 2, board.Y + board.Height / 2);
            _numMines = numMines;
            _rectTexture = texture;
            _bombTexture = bombtexture;
            _font = font;
            _board = new Box[width, height];
            _rects = new Rectangle[width, height];

        }
        public void NewBoard()
        {
            _gameState = "ready";
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    int width = _boardRect.Width / _width;
                    int height = _boardRect.Height / _height;
                    Rectangle rect = new Rectangle(_boardRect.X + width * i + 2, _boardRect.Y + height * j + 2, width - 1, height - 1);
                    _rects[i, j] = rect;
                    int value = 0;
                    Box box = new Box(_rectTexture, _bombTexture, rect, value, Color.LightGray, _font, false);
                    _board[i, j] = box;
                }
            }
        }
        private void GameStart(int x, int y)
        {
            List<int> bombs = new List<int> ();
            List<int> safezones = new List<int>();
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    int safezone = (x-1+k) * _width + (y-1+l);
                    safezones.Add(safezone);
                }
            }
            for (int i = 0; i < _numMines; i++)
            {
                int bomb = gen.Next(_width * _height);
                bool safe = true;
                foreach (int safezone in safezones)
                {
                    if (bomb == safezone)
                    {
                        safe = false;
                    }
                }
                if (!safe)
                {
                    i--;
                }
                else
                {
                    bombs.Add(bomb);
                }

            }
            int count = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0;  j < _height; j++)
                {
                    int width = _boardRect.Width / _width;
                    int height = _boardRect.Height / _height;
                    Rectangle rect = new Rectangle(_boardRect.X + width * i + 2, _boardRect.Y + height * j + 2, width - 1, height - 1);
                    _rects[i,j] = rect;
                    int value;
                    if (bombs.Contains(count))
                    {
                        value = 9;
                    }
                    else
                    {
                        value = 0;
                    }
                    Box box = new Box(_rectTexture, _bombTexture, rect, value, Color.LightGray, _font, false) ;
                    _board[i,j] = box;
                    count++;
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
                            if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                            {
                                if( !(k == 1 && l==1)&& _board[i - 1+k, j - 1+l].Value == 9)
                                {
                                    value++;
                                }
                            }

                        }
                    }
                    if (_board[i, j].Value != 9)
                        _board[i,j].Value = value;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_rectTexture, _boardRect, Color.DimGray);
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _board[i,j].Draw(spriteBatch);
                }
            }
            if (_gameState == "lost")
            {
                Vector2 txtPosition = new Vector2(_center.X - _font.MeasureString("BOOM!").X / 2, _center.Y - _font.MeasureString("BOOM!").Y / 2);
                spriteBatch.DrawString(_font, "BOOM!", txtPosition, Color.Red);
            }
        }
        public void Update(MouseState mousestate, MouseState prevmousestate)
        {
            _mouseState = mousestate;
            _prevMouseState = prevmousestate;

            if (_gameState == null)
            {
                return;
            }

            if (_gameState == "lost"||_gameState == "won")
            {
                return;
            }


            for (int i =0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (_rects[i, j].Contains(_mouseState.Position) && _mouseState.LeftButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (_gameState == "ready")
                        {
                            _gameState = "play";
                            GameStart(i,j);
                            return;
                        }
                        if (_board[i,j].Value == 9) 
                        {
                            _gameState = "lost";
                        }
                        if (_board[i,j].Value == 0)
                        {
                            
                        }
                        _board[i,j].Reveal = true;
                    }
                }
            }
        }
    }
}
