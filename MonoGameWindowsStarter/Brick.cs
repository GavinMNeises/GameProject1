using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// BrickState allows the brick to know if it is active or if it already broken
    /// </summary>
    public enum BrickState
    {
        Active,
        Broken
    }

    public class Brick
    {
        Game1 game;

        Texture2D texture;

        BoundingRectangle bounds;

        public BoundingRectangle Bounds
        {
            get
            {
                return bounds;
            }
        }

        public BrickState state;

        public Brick(Game1 game)
        {
            this.game = game;
        }

        /// <summary>
        /// Initializes the brick
        /// </summary>
        /// <param name="content">Allows the brick to retreive its texture</param>
        /// <param name="locationX">Offputs the brick's x position by its location in the array so that they are evenly spread apart</param>
        public void LoadContent(ContentManager content, int locationX)
        {
            texture = content.Load<Texture2D>("Brick");
            bounds.Width = 100;
            bounds.Height = 100;
            bounds.X = bounds.Width * locationX + 21;
            bounds.Y = 0;

            state = BrickState.Active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //If the brick is broken do not draw
            if(state != BrickState.Broken)
            {
                spriteBatch.Draw(texture, bounds, Color.White);
            }
        }

        /// <summary>
        /// Helper method that allows the ball to change the brick state to broken when hit.
        /// </summary>
        public void Break()
        {
            state = BrickState.Broken;
        }
    }
}
