using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public struct BoundingRectangle
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public BoundingRectangle(float x, float y, float width, float height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool CollidesWith(BoundingRectangle b)
        {
            return !(this.X > b.X + b.Width
                || this.X + this.Width < b.X
                || this.Y > b.Y + b.Width
                || this.Y + this.Width < b.Y);
        }

        public static implicit operator Rectangle(BoundingRectangle br)
        {
            return new Rectangle((int)br.X, (int)br.Y, (int)br.Width, (int)br.Height);
        }
    }
}
