using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Minesweeper
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int width, height,numMines,flags;

        Texture2D rectTexture, flagTexture, mineTexture, explodeTexture, heartTexture, coinTexture, restartTexture, returnTexture, exitTexture, smileTexture;

        MouseState mouseState, prevMouseState;

        Board game;

        List<Button> buttons = new List<Button>();

        SpriteFont spriteFont;

        SoundEffect bubSound, explosionSound;

        SoundEffectInstance bubInstance, explosionInstance;

        float time,gametime;

        string mode,gamestate;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight =  GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;


            base.Initialize();

            Rectangle rectEasy = new Rectangle(width / 4 - width / 20, height / 10 - height / 40, width / 10, height / 20);
            Button btnEasy = new Button(rectTexture,spriteFont,rectEasy,"Easy",Color.Gray,true);
            buttons.Add(btnEasy);//0

            Rectangle rectRestart = new Rectangle(height / 20, height * 9/10 + height/40, height / 20, height / 20);
            Button btnRestart = new Button(restartTexture, spriteFont, rectRestart, "", Color.White, false);
            buttons.Add(btnRestart);//1

            Rectangle rectMed = new Rectangle(width/2 - width/20, height/10-height/40, width/10, height/20);
            Button btnMed = new Button(rectTexture, spriteFont, rectMed, "Medium", Color.Gray, true);
            buttons.Add(btnMed);//2

            Rectangle rectHard = new Rectangle(width *3/ 4 - width / 20, height / 10 - height / 40, width / 10, height / 20);
            Button btnHard = new Button(rectTexture, spriteFont, rectHard, "Hard", Color.Gray, true);
            buttons.Add(btnHard);//3

            Rectangle rectReturn = new Rectangle( height / 20, height / 10 , height / 20, height / 20);
            Button btnReturn = new Button(returnTexture, spriteFont, rectReturn, "", Color.White, false);
            buttons.Add(btnReturn);//4

            Rectangle rectStat = new Rectangle(width / 2 - width / 20, height / 10 - height / 40, width / 10, height / 20);
            Button btnStat = new Button(rectTexture, spriteFont, rectStat, "", Color.Gray, false);
            buttons.Add(btnStat);//5

            Rectangle rectExit = new Rectangle(width - height / 10, height / 10, height / 20, height / 20);
            Button btnExit = new Button(exitTexture, spriteFont, rectExit, "", Color.White, true);
            buttons.Add(btnExit);//6

            Rectangle rectTimer = new Rectangle(width / 2 - width / 20, height *3/4 + height/10, width / 10, height / 20);
            Button btnTimer = new Button(rectTexture, spriteFont, rectTimer, "", Color.Gray, false);
            buttons.Add(btnTimer);//7

            Rectangle rectSmile = new Rectangle(width / 2 - width / 4, height / 2 - width/4, width / 2, width / 2);
            Button btnSmile = new Button(smileTexture, spriteFont, rectSmile, "", Color.White, true);
            buttons.Add(btnSmile);//8

            Rectangle rectHistory = new Rectangle(width / 2 - width /20, height - height / 10, width / 10, height / 20);
            Button btnHistory = new Button(rectTexture, spriteFont, rectHistory, "Stat", Color.Gray, true);
            buttons.Add(btnHistory);//9

            Rectangle rectData = new Rectangle(width / 2 - height / 4, height / 2 - width / 6, height / 2, width / 3);
            Button btnData = new Button(rectTexture, spriteFont, rectData, "Win a Game To Check Your Result!", Color.LightGray, false);
            buttons.Add(btnData);//10

            bubInstance = bubSound.CreateInstance();
            explosionInstance = explosionSound.CreateInstance();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            flagTexture = Content.Load<Texture2D>("flag");
            rectTexture = Content.Load<Texture2D>("rectangle");
            mineTexture = Content.Load<Texture2D>("mine");
            explodeTexture = Content.Load<Texture2D>("exploded");
            spriteFont = Content.Load<SpriteFont>("SpriteFont");
            restartTexture = Content.Load<Texture2D>("restart");
            returnTexture = Content.Load<Texture2D>("return");
            bubSound = Content.Load<SoundEffect>("bubpop");
            explosionSound = Content.Load<SoundEffect>("explosion");
            exitTexture = Content.Load<Texture2D>("exit");
            smileTexture = Content.Load<Texture2D>("smile");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            
            time = (float)gameTime.TotalGameTime.TotalSeconds;

            if (buttons[0].Contain(mouseState) && buttons[0].Visible)
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    gametime = (float)gameTime.TotalGameTime.TotalSeconds;
                    mode = "easy";
                    buttons[0].Visible = false;
                    buttons[2].Visible = false;
                    buttons[3].Visible = false;
                    buttons[8].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 2 - width / 4, height / 2 - width / 4, width / 2+2, width / 2+2);
                    game = new Board(rectTexture, mineTexture,flagTexture, 9, 9, 10, boardRect, spriteFont);
                    game.bub = bubInstance;
                    game.explosion = explosionInstance;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
                    buttons[6].Visible = true;
                    buttons[7].Visible = true;
                    buttons[9].Visible = false;
                    buttons[10].Visible = false;
                    numMines = 10;
                }
                else
                {
                    buttons[0].Color = Color.DarkGray;
                }
            }
            else
            {
                buttons[0].Color = Color.Gray;
            }

            if (buttons[1].Contain(mouseState) && buttons[1].Visible && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                gametime = (float)gameTime.TotalGameTime.TotalSeconds;
                game.NewBoard();
            }

            if (buttons[2].Contain(mouseState) && buttons[2].Visible)
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    mode = "med";
                    gametime = (float)gameTime.TotalGameTime.TotalSeconds;
                    buttons[2].Visible = false;
                    buttons[0].Visible = false;
                    buttons[3].Visible = false;
                    buttons[8].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 4 , height / 2 - width / 4, width / 2+2, width / 2+2);
                    game = new Board(rectTexture, mineTexture, flagTexture, 16, 16,40, boardRect, spriteFont);
                    game.bub = bubInstance;
                    game.explosion = explosionInstance;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
                    buttons[6].Visible = true;
                    buttons[7].Visible = true;
                    buttons[9].Visible = false;
                    buttons[10].Visible = false;
                    numMines = 40;
                }
                else
                {
                    buttons[2].Color = Color.DarkGray;
                }
            }
            else
            {
                buttons[2].Color = Color.Gray;
            }

            if (buttons[3].Contain(mouseState) && buttons[3].Visible)
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    mode = "hard";
                    gametime = (float)gameTime.TotalGameTime.TotalSeconds;
                    buttons[2].Visible = false;
                    buttons[0].Visible = false;
                    buttons[3].Visible = false;
                    buttons[8].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 2- (width / 2 + 2) / 16 * 15, height / 2 - width / 4, (width / 2 + 2)/16*30, width / 2 + 2);
                    game = new Board(rectTexture, mineTexture, flagTexture,30, 16, 99, boardRect, spriteFont);
                    game.bub = bubInstance;
                    game.explosion = explosionInstance;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
                    buttons[6].Visible = true;
                    buttons[7].Visible = true;
                    buttons[9].Visible = false;
                    buttons[10].Visible = false;
                    numMines = 99;
                }
                else
                {
                    buttons[3].Color = Color.DarkGray;
                }
            }
            else
            {
                buttons[3].Color = Color.Gray;
            }

            if (buttons[4].Contain(mouseState) && buttons[4].Visible && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                game = null;
                buttons[0].Visible = true;
                buttons[2].Visible = true;
                buttons[3].Visible = true;
                buttons[8].Visible = true;
                buttons[1].Visible = false;
                buttons[4].Visible = false;
                buttons[5].Visible = false;
                buttons[7].Visible = false;
                buttons[9].Visible = true ;

            }

            if (buttons[6].Contain(mouseState) && buttons[6].Visible && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                Exit();
            }

            if (buttons[9].Contain(mouseState) && buttons[9].Visible && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                if (buttons[10].Visible)
                {
                    buttons[10].Visible = false;
                }
                else
                {
                    buttons[10].Visible = true;
                }
            }


            if (game != null)
            {
                game.Update(mouseState, prevMouseState);
                if (game.GameState == "won"||game.GameState == "lost")
                {
                    buttons[0].Visible =true;
                    buttons[2].Visible =true;
                    buttons[3].Visible = true;
                    buttons[1].Visible = false;
                    buttons[4].Visible = false;
                    buttons[5].Visible = false;
                    buttons[9].Visible = true;
                }
                else
                {
                    buttons[7].Text = (time - gametime).ToString("0.00");
                }
                if (gamestate != "won"&& game.GameState == "won")
                {
                    if (File.Exists(@"data.txt"))
                    {
                        string[] arrLine = File.ReadAllLines(@"data.txt");
                        if (mode == "easy")
                        {
                            if (Convert.ToDouble(arrLine[3]) > Convert.ToDouble(buttons[7].Text))
                                arrLine[3] = buttons[7].Text;
                            arrLine[0] = Convert.ToString(Convert.ToInt16(arrLine[0]) + 1);
                        }
                        else if (mode == "med")
                        {
                            if (Convert.ToDouble(arrLine[4]) > Convert.ToDouble(buttons[7].Text))
                                arrLine[4] = buttons[7].Text;
                            arrLine[1] = Convert.ToString(Convert.ToInt16(arrLine[1]) + 1);
                        }
                        else if (mode == "hard")
                        {
                            if (Convert.ToDouble(arrLine[5]) > Convert.ToDouble(buttons[7].Text))
                                arrLine[5] = buttons[7].Text;
                            arrLine[2] = Convert.ToString(Convert.ToInt16(arrLine[2]) + 1);
                        }
                        File.WriteAllLines(@"data.txt", arrLine);
                        buttons[10].Text = "Easy Mode Completed: " + arrLine[0] + "\nMedium Mode Completed: " + arrLine[1] + "\nHard Mode Completed: " + arrLine[2] + "\nBest Record(Easy): " + arrLine[3] + "\nBest Record(Medium): " + arrLine[4] + "\nBest Record(Hard): " + arrLine[5];
                    }
                    else
                    {
                        StreamWriter writer = File.CreateText(@"data.txt");
                        for (int i = 0; i < 6; i++)
                        {
                            writer.WriteLine("0");
                        }
                        writer.Close();
                    }
                }
                gamestate = game.GameState;
                flags = game.Flags;
                buttons[5].Text = flags + "/" + numMines;

            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGoldenrodYellow);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (game != null)
            {
                game.Draw(_spriteBatch);
            }

            foreach(Button button in buttons)
            {
                if (button.Visible)
                {
                     button.Draw(_spriteBatch);
                }
            }



            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}