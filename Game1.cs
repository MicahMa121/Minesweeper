﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Minesweeper
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int width, height;

        Texture2D rectTexture,flagTexture,mineTexture,explodeTexture,heartTexture,coinTexture,restartTexture;

        MouseState mouseState, prevMouseState;

        Board game;

        List<Button> buttons = new List<Button>();

        SpriteFont spriteFont;
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

            Rectangle rectEasy = new Rectangle(width/2 - width/20, height/10-height/40, width/10, height/20);
            Button btnEasy = new Button(rectTexture,spriteFont,rectEasy,"Easy",Color.Gray,true);
            buttons.Add(btnEasy);//0

            Rectangle rectRestart = new Rectangle(width / 2 - width / 3, height / 2 - width / 4, height / 20, height / 20);
            Button btnRestart = new Button(restartTexture, spriteFont, rectRestart, "", Color.White, false);
            buttons.Add(btnRestart);//1


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
                    Rectangle boardRect = new Rectangle(width / 2 - width / 4, height / 2 - width / 4, width / 2, width / 2);
                    game = new Board(rectTexture, mineTexture, 9, 9, 10, boardRect, spriteFont);
                    game.NewBoard();
                    buttons[1].Visible = true;
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

            if (game != null)
                game.Update(mouseState, prevMouseState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

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