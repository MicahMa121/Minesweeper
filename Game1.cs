using Microsoft.Xna.Framework;
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
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            width = _graphics.PreferredBackBufferWidth;
            height = _graphics.PreferredBackBufferHeight;


            base.Initialize();
            Rectangle rectEasy = new Rectangle(width/2 - width/20, height/10-height/40, width/10, height/20);
            Button btnEasy = new Button(rectTexture,spriteFont,rectEasy,"Easy",Color.Gray,true);
            buttons.Add(btnEasy);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            foreach (Button button in buttons)
            {
                if (button.Contain(mouseState))
                {
                    if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
                    {
                        button.Visible = false;

                    }
                    else
                    {
                        button.Color = Color.DarkGray;
                    }
                }
                else
                {
                    button.Color = Color.Gray;
                }
            }

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

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}