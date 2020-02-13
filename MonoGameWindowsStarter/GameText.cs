using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    public class GameText
    {
        SpriteFont spriteFont;

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("defaultFont");
        }

        public void Draw(SpriteBatch spriteBatch, String message, Vector2 location)
        {
            location -= spriteFont.MeasureString(message)/2;
            spriteBatch.DrawString(spriteFont, message, location, Color.Red);
        }

        public void DrawScore(SpriteBatch spriteBatch, String message, Vector2 location)
        {
            location.X -= spriteFont.MeasureString(message).X;
            spriteBatch.DrawString(spriteFont, message, location, Color.Gold);
        }
    }
}
