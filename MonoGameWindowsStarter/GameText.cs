﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// GameText allows Game1 to display any message on the screen using sprite fonts
    /// </summary>
    public class GameText
    {
        SpriteFont spriteFont;

        public void LoadContent(ContentManager content)
        {
            spriteFont = content.Load<SpriteFont>("defaultFont");
        }

        /// <summary>
        /// Draw method for the start and end game messages
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="message">Message to display</param>
        /// <param name="location">Where to display the message relative to the screen</param>
        public void Draw(SpriteBatch spriteBatch, String message, Vector2 location)
        {
            location -= spriteFont.MeasureString(message)/2;
            spriteBatch.DrawString(spriteFont, message, location, Color.Red);
        }

        /// <summary>
        /// Draw method for the score
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="message">Score to display</param>
        /// <param name="location">Where to display the score relative to the screen</param>
        public void DrawScore(SpriteBatch spriteBatch, String message, Vector2 location)
        {
            location.X -= spriteFont.MeasureString(message).X;
            spriteBatch.DrawString(spriteFont, message, location, Color.Gold);
        }
    }
}
