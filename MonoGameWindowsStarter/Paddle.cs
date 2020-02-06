using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    public class Paddle
    {
        Game1 game;

        Texture2D texture;

        public BoundingRectangle bounds;

        /// <summary>
        /// Creates a paddle
        /// </summary>
        /// <param name="game">Reference to the game the paddle belongs to</param>
        public Paddle(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Paddle");
            bounds.Width = 200;
            bounds.Height = 75;
            bounds.X = game.GraphicsDevice.Viewport.Width/2 - bounds.Width/2;
            bounds.Y = game.GraphicsDevice.Viewport.Height - bounds.Height;
        }

        public void Update(GameTime gameTime)
        {
            var newKeyboardState = Keyboard.GetState();

            //If the left  or A key is down move left
            if (newKeyboardState.IsKeyDown(Keys.Left) || newKeyboardState.IsKeyDown(Keys.A))
            {
                bounds.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //If the right or D key is down move left
            if (newKeyboardState.IsKeyDown(Keys.Right) || newKeyboardState.IsKeyDown(Keys.D))
            {
                bounds.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            //If the paddle hits the left wall put it back in the window
            if (bounds.X < 0)
            {
                bounds.X = 0;
            }

            //If the paddle hits the right wall put it back in the window
            if (bounds.X > game.GraphicsDevice.Viewport.Width - bounds.Width)
            {
                bounds.X = game.GraphicsDevice.Viewport.Width - bounds.Width;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
