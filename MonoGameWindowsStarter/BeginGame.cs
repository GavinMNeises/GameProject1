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
    public class BeginGame
    {
        Game1 game;

        Texture2D texture;

        //Stores the size and location of the message box
        Rectangle messagePosition;

        public BeginGame(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("BeginMessage");
            messagePosition.Width = 400;
            messagePosition.Height = 100;
            messagePosition.X = (game.GraphicsDevice.Viewport.Width/2) - (messagePosition.Width/2);
            messagePosition.Y = (game.GraphicsDevice.Viewport.Height/2) - (messagePosition.Height/2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, messagePosition, Color.White);
        }
    }
}
