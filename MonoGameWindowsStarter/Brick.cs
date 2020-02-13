using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// BrickState allows the brick to know if it is active, breaking, or if it is already broken
    /// Also allows the brick to draw its individual frames
    /// </summary>
    public enum BrickState
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

    public class Brick
    {
        Texture2D texture;

        SoundEffect brickBreak;

        BoundingRectangle bounds;

        public BoundingRectangle Bounds
        {
            get
            {
                return bounds;
            }
        }

        public BrickState state;

        public Brick()
        {
        }

        /// <summary>
        /// Initializes the brick
        /// </summary>
        /// <param name="content">Allows the brick to retreive its texture</param>
        /// <param name="locationX">Offputs the brick's x position by its location in the array so that they are evenly spread apart</param>
        public void LoadContent(ContentManager content, int locationX)
        {
            texture = content.Load<Texture2D>("Brick");

            brickBreak = content.Load<SoundEffect>("BrickBreak");

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
                //Offput the frame by the state of the brick
                Rectangle frame = new Rectangle(((int)state)*100, 0, 100, 100);
                spriteBatch.Draw(texture, bounds, frame, Color.White);

                //If the brick is breaking transition to the next frame state
                if(state != BrickState.Active)
                {
                    state++;
                }
            }
        }

        /// <summary>
        /// Helper method that allows the ball to change the brick state to broken when hit.
        /// </summary>
        public void Break()
        {
            state = BrickState.Breaking1;

            brickBreak.Play();
        }
    }
}
