using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public class Brick : IBoundable
    {
        static Texture2D Texture;

        static SoundEffect brickBreak;

        BoundingRectangle bounds;

        /// <summary>
        /// Allows access to Bounds to satisfy IBoundable
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public BrickState state;

        /// <summary>
        /// Used to create a new brick
        /// </summary>
        /// <param name="texture">The texture used for bricks</param>
        /// <param name="soundEffect">The sound effect used for bricks</param>
        /// <param name="locationX">The x offset of the brick</param>
        /// <param name="locationY">The y offset of the brick</param>
        public Brick(Texture2D texture, SoundEffect soundEffect, int locationX, int locationY)
        {
            if (texture == null || brickBreak == null)
            {
                Texture = texture;
                brickBreak = soundEffect;
            }

            bounds.Width = 100;
            bounds.Height = 100;
            bounds.X = bounds.Width * locationX + 21;
            bounds.Y = 100 * locationY;

            state = BrickState.Active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //If the brick is broken do not draw
            if(state != BrickState.Broken)
            {
                //Offput the frame by the state of the brick
                Rectangle frame = new Rectangle(((int)state)*100, 0, 100, 100);
                spriteBatch.Draw(Texture, bounds, frame, Color.White);

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
