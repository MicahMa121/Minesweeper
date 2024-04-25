using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Minesweeper
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int width, height,numMines,flags;

        Texture2D rectTexture,flagTexture,mineTexture,explodeTexture,heartTexture,coinTexture,restartTexture,returnTexture;

        MouseState mouseState, prevMouseState;

        Board game;

        List<Button> buttons = new List<Button>();

        SpriteFont spriteFont;

        SoundEffect bubSound, explosionSound;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 800;//GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 600;// GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;


            base.Initialize();

            Rectangle rectEasy = new Rectangle(width / 4 - width / 20, height / 10 - height / 40, width / 10, height / 20);
            Button btnEasy = new Button(rectTexture,spriteFont,rectEasy,"Easy",Color.Gray,true);
            buttons.Add(btnEasy);//0

            Rectangle rectRestart = new Rectangle(width / 2 - height / 40, height - height /10, height / 20, height / 20);
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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            if (buttons[0].Contain(mouseState))
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    buttons[0].Visible = false;
                    buttons[2].Visible = false;
                    buttons[3].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 2 - width / 4, height / 2 - width / 4, width / 2, width / 2);
                    game = new Board(rectTexture, mineTexture,flagTexture, 9, 9, 10, boardRect, spriteFont);
                    game.bub = bubSound;
                    game.explosion = explosionSound;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
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

            if (buttons[1].Contain(mouseState)&& mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                game.NewBoard();
            }

            if (buttons[2].Contain(mouseState))
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    buttons[2].Visible = false;
                    buttons[0].Visible = false;
                    buttons[3].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 4 , height / 2 - width / 4, width / 2+2, width / 2+2);
                    game = new Board(rectTexture, mineTexture, flagTexture, 16, 16,40, boardRect, spriteFont);
                    game.bub = bubSound;
                    game.explosion = explosionSound;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
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

            if (buttons[3].Contain(mouseState))
            {
                if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                {
                    buttons[2].Visible = false;
                    buttons[0].Visible = false;
                    buttons[3].Visible = false;
                    Rectangle boardRect = new Rectangle(width / 2- (width / 2 + 2) / 16 * 15, height / 2 - width / 4, (width / 2 + 2)/16*30, width / 2 + 2);
                    game = new Board(rectTexture, mineTexture, flagTexture,30, 16, 99, boardRect, spriteFont);
                    game.bub = bubSound;
                    game.explosion = explosionSound;
                    game.NewBoard();
                    buttons[1].Visible = true;
                    buttons[4].Visible = true;
                    buttons[5].Visible = true;
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

            if (buttons[4].Contain(mouseState) && mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
            {
                game = null;
                buttons[0].Visible = true;
                buttons[2].Visible = true;
                buttons[3].Visible = true;
                buttons[1].Visible = false;
                buttons[4].Visible = false;
                buttons[5].Visible = false;
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
                }
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

            foreach(Button button in buttons)
            {
                if (button.Visible)
                {
                     button.Draw(_spriteBatch);
                }
            }

            if (game != null)
            {
                game.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}