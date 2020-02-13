using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// GameState allows the game to have a begin, playing, and end state
    /// </summary>
    enum GameState
    {
        Begin,
        Playing,
        End
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //User controlled paddle
        Paddle paddle;

        //Bouncing ball
        Ball ball;

        //Bricks for the user to break
        Brick[] bricks;

        //Renders text onto the screen using SpriteFonts
        GameText textRenderer;

        //Score keeps track of the player score
        public int score;
        
        //State of the current game
        GameState gameState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            paddle = new Paddle(this);
            ball = new Ball(this);
            bricks = new Brick[10];
            for(int i = 0; i < bricks.Length; i++)
            {
                bricks[i] = new Brick();
            }
            textRenderer = new GameText();

            score = 0;

            gameState = GameState.Begin;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1042;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            paddle.LoadContent(Content);
            ball.LoadContent(Content);
            for(int i = 0; i < bricks.Length; i++)
            {
                bricks[i].LoadContent(Content, i);
            }
            textRenderer.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Start or restart the game
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                //If game over reset all of the variables to a new game state
                if(gameState == GameState.End)
                {
                    ball.Velocity = new Vector2(0, 1);
                    ball.velocityMultiplier = .25;
                    ball.bounds.X = this.GraphicsDevice.Viewport.Width / 2;
                    ball.bounds.Y = this.GraphicsDevice.Viewport.Height - 200;
                    ball.ballState = BallState.Active;
                    
                    paddle.bounds.X = this.GraphicsDevice.Viewport.Width / 2 - paddle.bounds.Width / 2;
                    paddle.bounds.Y = this.GraphicsDevice.Viewport.Height - paddle.bounds.Height;

                    for(int i = 0; i < bricks.Length; i++)
                    {
                        bricks[i].state = BrickState.Active;
                    }

                    score = 0;
                }

                //Start game
                gameState = GameState.Playing;
            }

            //If the game is active update the ball and paddle
            if (gameState == GameState.Playing)
            {
                paddle.Update(gameTime);
                ball.Update(gameTime, paddle.bounds, bricks);
            }

            //Check if all of the bricks are broken so that they can be reset
            bool allBrokenBricks = false;
            for(int i = 0; i < bricks.Length; i++)
            {
                if(bricks[i].state == BrickState.Active)
                {
                    allBrokenBricks = false;
                    break;
                }
                else
                {
                    allBrokenBricks = true;
                }
            }

            //Reset bricks if all are broken
            if (allBrokenBricks)
            {
                for (int i = 0; i < bricks.Length; i++)
                {
                    bricks[i].state = BrickState.Active;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //Draw paddle, ball, and bricks
            spriteBatch.Begin();
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            for(int i = 0; i < bricks.Length; i++)
            {
                bricks[i].Draw(spriteBatch);
            }

            //Draw current score
            Vector2 location = new Vector2(GraphicsDevice.Viewport.Width, 0);
            textRenderer.DrawScore(spriteBatch, "Score: " + score.ToString(), location);

            //If start of the game draw the begin game message
            if (gameState == GameState.Begin)
            {
                location = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                textRenderer.Draw(spriteBatch, "Press Enter To Begin", location);
            }

            //If game is over draw the end game and begin game message
            if(gameState == GameState.End)
            {
                location = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                textRenderer.Draw(spriteBatch, "Press Enter To Begin", location);

                location.Y = GraphicsDevice.Viewport.Height / 4;
                textRenderer.Draw(spriteBatch, "Game Over!", location);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        //This method allows ball to change the game state to end if it hits the bottom of the screen
        public void EndGame()
        {
            gameState = GameState.End;
        }
    }
}
