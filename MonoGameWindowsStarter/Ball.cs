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
    public class Ball
    {
        Game1 game;

        Texture2D texture;

        public BoundingCircle bounds;

        Vector2 ballVelocity;

        //Multiplier used to increase ball speed as the game is played
        public double velocityMultiplier;

        public Vector2 Velocity
        {
            get
            {
                return ballVelocity;
            }

            set
            {
                ballVelocity = value;
            }
        }

        public Ball(Game1 game)
        {
            this.game = game;
            ballVelocity = new Vector2(0, 1);
            ballVelocity.Normalize();
            velocityMultiplier = .25;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Ball");
            bounds.Radius = 37;
            bounds.X = game.GraphicsDevice.Viewport.Width/2;
            bounds.Y = game.GraphicsDevice.Viewport.Height - 200;
        }

        /// <summary>
        /// Updates the position of the ball, checks for collisions, and updates other components based on the ball's collisions
        /// </summary>
        /// <param name="gameTime">Allows ball to move with a constant speed</param>
        /// <param name="paddle">Used to check collisions with the paddle</param>
        /// <param name="bricks">Used to check collisions with the bricks</param>
        public void Update(GameTime gameTime, BoundingRectangle paddle, Brick[] bricks)
        {
            //Update location based on velocity, time, and the current multiplier
            bounds.X += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * ballVelocity.X * velocityMultiplier);
            bounds.Y += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * ballVelocity.Y * velocityMultiplier);
        
            //Top of screen detection
            if(bounds.Y-bounds.Radius < 0)
            {
                ballVelocity.Y *= -1;
                float delta = 0 - (bounds.Y - bounds.Radius);
                bounds.Y += 2 * delta;
            }

            //Bottom of screen detection and game over
            if(bounds.Y+bounds.Radius > game.GraphicsDevice.Viewport.Height)
            {
                game.EndGame();
            }

            //Left side of screen detection 
            if(bounds.X-bounds.Radius < 0)
            {
                ballVelocity.X *= -1;
                float delta = 0 - (bounds.X - bounds.Radius);
                bounds.X += 2 * delta;
            }

            //Right side of screen detection 
            if(bounds.X+bounds.Radius > game.GraphicsDevice.Viewport.Width)
            {
                ballVelocity.X *= -1;
                float delta = game.GraphicsDevice.Viewport.Width - (bounds.X + bounds.Radius);
                bounds.X += 2 * delta;
            }

            //Paddle collision detection
            if(bounds.CollidesWith(paddle))
            {
                //Reflect Y velocity
                ballVelocity.Y *= -1;
                float delta = paddle.Y - (bounds.Y + bounds.Radius);
                bounds.Y += 2 * delta;

                //Change X velocity based on where the ball hit the paddle
                double xRebound = ((paddle.X + (paddle.Width / 2)) - bounds.X) / (paddle.Width);
                xRebound *= 90;
                xRebound = Math.PI * xRebound / 180;
                ballVelocity.X = (float)Math.Sin(xRebound) * -1;
            }

            //Check for brick collisions
            for (int i = 0; i < bricks.Length; i++) {
                if(bricks[i].state == BrickState.Active && bounds.CollidesWith(bricks[i].Bounds))
                {
                    //If collided break brick
                    bricks[i].Break();

                    //Increase multiplier
                    velocityMultiplier += .04;
                    
                    //Check if the X velocity needs to be inverted
                    if(bounds.CollidesWithLeftRight(bricks[i].Bounds))
                    {
                        ballVelocity.X *= -1;
                    }

                    //Check if the Y velocity needs to be inverted
                    if (bounds.CollidesWithTopBottom(bricks[i].Bounds))
                    {
                        ballVelocity.Y *= -1;
                    }

                    //Break to reduce multiple brick breaks per one bounce
                    break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
