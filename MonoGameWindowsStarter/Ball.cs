using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// BallState allows the ball to know if it is active, breaking or broken
    /// Allows the ball to be drawn using its different frames
    /// </summary>
    public enum BallState
    {
        Active,
        Breaking1,
        Breaking2,
        Breaking3,
        Breaking4,
        Breaking5,
        Breaking6,
        Breaking7,
        Breaking8,
        Broken
    }

    public class Ball
    {
        Game1 game;

        Texture2D texture;

        public BoundingCircle bounds;

        Vector2 ballVelocity;

        //Multiplier used to increase ball speed as the game is played
        public double velocityMultiplier;

        SoundEffect ballBounce;
        SoundEffect ballBreak;

        public BallState ballState;

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
            ballState = BallState.Active;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Ball");

            ballBounce = content.Load<SoundEffect>("Bounce");
            ballBreak = content.Load<SoundEffect>("BallBreak");

            bounds.Radius = 36;
            bounds.X = game.GraphicsDevice.Viewport.Width/2;
            bounds.Y = game.GraphicsDevice.Viewport.Height - 200;
        }

        /// <summary>
        /// Updates the position of the ball, checks for collisions, and updates other components based on the ball's collisions
        /// </summary>
        /// <param name="gameTime">Allows ball to move with a constant speed</param>
        /// <param name="paddle">Used to check collisions with the paddle</param>
        /// <param name="bricks">Used to check collisions with the bricks</param>
        public void Update(GameTime gameTime, BoundingRectangle paddle, IEnumerable<IBoundable> bricks)
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

                ballBounce.Play();
            }

            //Bottom of screen detection and game over
            if(bounds.Y+bounds.Radius > game.GraphicsDevice.Viewport.Height)
            {
                game.EndGame();
                ballState = BallState.Breaking1;
                ballBreak.Play();
            }

            //Left side of screen detection 
            if(bounds.X-bounds.Radius < 0)
            {
                ballVelocity.X *= -1;
                float delta = 0 - (bounds.X - bounds.Radius);
                bounds.X += 2 * delta;

                ballBounce.Play();
            }

            //Right side of screen detection 
            if(bounds.X+bounds.Radius > game.GraphicsDevice.Viewport.Width)
            {
                ballVelocity.X *= -1;
                float delta = game.GraphicsDevice.Viewport.Width - (bounds.X + bounds.Radius);
                bounds.X += 2 * delta;

                ballBounce.Play();
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

                ballBounce.Play();
            }

            //Check for brick collisions
            foreach (Brick brick in bricks)
            {
                //If the brick is active and the ball collides with it
                if(brick.state == BrickState.Active && bounds.CollidesWith(brick.Bounds))
                {
                    //If collided break brick
                    brick.Break();

                    //Increase game score
                    game.score++;

                    //Increase multiplier
                    velocityMultiplier += .02;

                    //Check if the Y velocity needs to be inverted
                    if (bounds.CollidesWithTopBottom(brick.Bounds))
                    {
                        ballVelocity.Y *= -1;
                        //Check if ball hit the top of the brick
                        if (bounds.Y > brick.Bounds.Y)
                        {
                            float delta = (brick.Bounds.Y + brick.Bounds.Height) - (bounds.Y - bounds.Radius);
                            bounds.Y += 2 * delta;
                        }
                        //Else the ball hit the bottom of the brick
                        else
                        {
                            float delta = brick.Bounds.Y - (bounds.Y + bounds.Radius);
                            bounds.Y -= 2 * delta;
                        }
                    }
                    //Else the X velocity needs to be inverted
                    else
                    {
                        ballVelocity.X *= -1;
                        //Check if the ball hit the right side of the brick
                        if(bounds.X > brick.Bounds.X)
                        {
                            float delta = (brick.Bounds.X + brick.Bounds.Width) - (bounds.X - bounds.Radius);
                            bounds.X += 2 * delta;
                        }
                        //Else the ball hit the left side of the brick
                        else
                        {
                            float delta = brick.Bounds.X - (bounds.X + bounds.Radius);
                            bounds.X -= 2 * delta;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw ball if it is not broken
            if (ballState != BallState.Broken)
            {
                //Offput the frame by the ballState
                Rectangle frame = new Rectangle(((int)ballState) * 100, 0, 100, 100);
                spriteBatch.Draw(texture, bounds, frame, Color.White);
                
                //If the ball is breaking transition to the next frame
                if(ballState != BallState.Active)
                {
                    ballState++;
                }
            }
        }
    }
}
