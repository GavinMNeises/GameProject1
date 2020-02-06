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
    class EndGame
    {
        Game1 game;

        Texture2D texture;

        //Stores the location and size of the message box
        Rectangle messagePosition;

        public EndGame(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("GameOverMessage");
            messagePosition.Width = 200;
            messagePosition.Height = 100;
            messagePosition.X = (game.GraphicsDevice.Viewport.Width / 2) - (messagePosition.Width / 2);
            messagePosition.Y = 100;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, messagePosition, Color.White);
        }
    }
}
