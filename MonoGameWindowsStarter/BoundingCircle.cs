using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingCircle
    {
        public float X;
        public float Y;
        public float Radius;

        //Syntactic sugar for a get method
        //public Vector2 Center => new Vector2(X, Y);

        public Vector2 Center
        {
            get => new Vector2(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return (Math.Pow(this.Radius + other.Radius, 2) <= Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            float nearestX = Clamp(this.X, other.X, other.X + other.Width);
            float nearestY = Clamp(this.Y, other.Y, other.Y + other.Height);
            return (Math.Pow(this.Radius, 2) >= (Math.Pow(this.X - nearestX, 2) + Math.Pow(this.Y - nearestY, 2)));
        }

        /// <summary>
        /// Checks to see if the ball collided with the top/bottom or left/right sides of the rectangle
        /// </summary>
        /// <param name="other">The rectangle to detect collisions with</param>
        /// <returns>If the ball collided with the top/bottom</returns>
        public bool CollidesWithTopBottom(BoundingRectangle other)
        {
            float xDiff = X - (other.X + (other.Width / 2));
            float yDiff = Y - (other.Y + (other.Height / 2));
            return Math.Pow(xDiff, 2) < Math.Pow(yDiff, 2);
        }

        public float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        public static implicit operator Rectangle(BoundingCircle bs)
        {
            return new Rectangle((int)(bs.X-bs.Radius), (int)(bs.Y-bs.Radius), (int)(bs.Radius*2), (int)(bs.Radius*2));
        }
    }
}
