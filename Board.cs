using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

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
        private Texture2D _flagTexture;
        public int Flags;
        public SoundEffectInstance bub, explosion;
        public Board(Texture2D texture,Texture2D bombtexture,Texture2D flagtexture, int width, int height, int numMines, Rectangle board,SpriteFont font)
        {
            _width = width;
            _height = height;
            _boardRect = board;
            _center = new Vector2(board.X + board.Width / 2, board.Y + board.Height / 2);
            _numMines = numMines;
            _rectTexture = texture;
            _bombTexture = bombtexture;
            _flagTexture = flagtexture;
            _font = font;
            _board = new Box[width, height];
            _rects = new Rectangle[width, height];


        }
        public string GameState
        {
            get { return _gameState; }
            set { _gameState = value; }
        }


        public void NewBoard()
        {
            Flags = 0;
            _gameState = "ready";
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    int width = _boardRect.Width / _width;
                    Rectangle rect = new Rectangle(_boardRect.X + width * i + 4, _boardRect.Y + width * j + 4, width - 1, width - 1);
                    _rects[i, j] = rect;
                    int value = 0;
                    Box box = new Box(_rectTexture, _bombTexture, _flagTexture, rect, value, Color.LightGray, _font, false);
                    _board[i, j] = box;
                }
            }
        }
        private void GameStart(int x, int y)
        {
            List<int> mines = new List<int> ();
            List<int> safezones = new List<int>();
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    int safezone = (x-1+k) * _height + (y-1+l);
                    safezones.Add(safezone);
                }
            }
            while (mines.Count < _numMines)
            {
                int mine = gen.Next(_width * _height);
                bool safe = true;
                foreach (int safezone in safezones)
                {
                    if (mine == safezone)
                    {
                        safe = false;
                    }
                }
                foreach (int used in mines)
                {
                    if (mine == used)
                    {
                        safe = false;
                    }
                }
                if (safe)
                {
                    mines.Add(mine);
                }
            }
            int count = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0;  j < _height; j++)
                {
                    int width = _boardRect.Width / _width;
                    Rectangle rect = new Rectangle(_boardRect.X + width * i + 4, _boardRect.Y + width * j + 4, width - 1, width - 1);
                    _rects[i,j] = rect;
                    int value;
                    if (mines.Contains(count))
                    {
                        value = 9;
                    }
                    else
                    {
                        value = 0;
                    }
                    Box box = new Box(_rectTexture, _bombTexture,_flagTexture, rect, value, Color.LightGray, _font, false) ;
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
            if (_gameState == "won")
            {
                Vector2 txtPosition = new Vector2(_center.X - _font.MeasureString("You Won!").X / 2, _center.Y - _font.MeasureString("You Won!").Y / 2);
                spriteBatch.DrawString(_font, "You Won!", txtPosition, Color.Green);
                for (int i = 0; i < _width; i++)
                {
                    for (int j = 0; j < _height; j++)
                    {
                        _board[i, j].Reveal = true ;
                    }
                }
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

            int winScore = 0;
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (_board[i,j].Value != 9 && _board[i,j].Reveal)
                    {
                        winScore++;
                    }
                }
            }
            if(winScore == (_width*_height - _numMines))
            {
                _gameState = "won";
                string[] arrLine = File.ReadAllLines(@"data.txt");
                if (_width == 9)
                {
                    arrLine[0] = Convert.ToString(1 + Convert.ToInt16(arrLine[0]));
                }
                else if (_width == 16)
                {
                    arrLine[1] = Convert.ToString(1 + Convert.ToInt16(arrLine[0]));
                }
                else if (_width == 30)
                {
                    arrLine[2] = Convert.ToString(1 + Convert.ToInt16(arrLine[0]));
                }
                return;
            }

            for (int i =0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (!_board[i,j].Flagged&&_rects[i, j].Contains(_mouseState.Position) && _mouseState.LeftButton == ButtonState.Released && _prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        explosion.Stop();
                        if (_gameState == "ready")
                        {
                            _gameState = "play";
                            GameStart(i,j);
                        }
                        if (_board[i,j].Value == 9) 
                        {
                            _gameState = "lost";
                            explosion.Play();
                            _board[i, j].Reveal = true;
                            return;
                        }
                        else if (_board[i,j].Value == 0)
                        {
                            for (int k = 0; k < _width; k++)
                            {
                                for (int l = 0; l < _height; l++)
                                {
                                    _board[k, l].Checked = false ;
                                }
                            }
                            CheckSurrounding(i,j);
                        }
                        else if (_board[i,j].Reveal && _board[i, j].Value != 0)
                        {
                            bool canquickcheck = true;
                            for (int k = 0; k < 3; k++)
                            {
                                for (int l = 0; l < 3; l++)
                                {
                                    if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                                    {
                                        if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                                        {
                                            if ((_board[i - 1 + k, j - 1 + l].Value == 9 && _board[i - 1 + k, j - 1 + l].Flagged == false)||(_board[i - 1 + k, j - 1 + l].Flagged&& _board[i - 1 + k, j - 1 + l].Value != 9))
                                            {
                                                canquickcheck = false;
                                            }
                                        }
                                    }
                                }
                            }
                            if (canquickcheck)
                            {
                                QuickCheck(i,j);
                            }
                        }
                        _board[i,j].Reveal = true;
                        bub.Play();
                        }
                        if (_gameState == "play"&&_rects[i, j].Contains(_mouseState.Position) && _mouseState.RightButton == ButtonState.Released && _prevMouseState.RightButton == ButtonState.Pressed)
                        {
                            if (_board[i,j].Flagged)
                            {
                                _board[i, j].Flagged = false;
                                Flags--;
                            }
                            else if (!_board[i, j].Flagged && !_board[i, j].Reveal)
                            {
                                _board[i, j].Flagged = true;
                                Flags++;
                            }
                    }
                }
            }
        }
        private void CheckSurrounding(int i, int j)
        {
            _board[i, j].Checked = true;
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                    {
                        if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                        {
                            if (!(k == 1 && l == 1) && _board[i - 1 + k, j - 1 + l].Value == 0 && _board[i - 1 + k, j - 1 + l].Checked == false)
                            {
                                _board[i - 1 + k, j - 1 + l].Reveal = true;
                                CheckSurrounding(i - 1 + k, j - 1 + l);
                            }
                            else
                            {
                                _board[i - 1 + k, j - 1 + l].Reveal = true;
                            }
                        }
                    }
                }
            }
        }
        private void QuickCheck(int i, int j)
        {
            _board[i, j].Checked = true;
            for (int k = 0; k < 3; k++)
            {
                for (int l = 0; l < 3; l++)
                {
                    if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                    {
                        if (i - 1 + k >= 0 && i - 1 + k < _width && j - 1 + l >= 0 && j - 1 + l < _height)
                        {
                            if (!(k == 1 && l == 1) && _board[i - 1 + k, j - 1 + l].Value == 0 && _board[i - 1 + k, j - 1 + l].Checked == false)
                            {
                                _board[i - 1 + k, j - 1 + l].Reveal = true;
                                QuickCheck(i - 1 + k, j - 1 + l);
                            }
                            else if (!(k == 1 && l == 1) && _board[i - 1 + k, j - 1 + l].Value != 9 && _board[i - 1 + k, j - 1 + l].Checked == false)
                            {
                                _board[i - 1 + k, j - 1 + l].Reveal = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
