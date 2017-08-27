using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.PanelStuff
{
    class PanelElement
    {
        public Panel Parent;
        public Vector2 Position;
        private Texture2D tx;
        private SpriteFont Font;
        private Color textColor;
        private string text;
        private float sizefactor;
        public bool isActive;

        

        private List<Texture2D> additionalTex;

        //public bool isAnimated { get; private set; }

        /// <summary>
        /// Constructor of a panel element
        /// </summary>
        /// <param name="tex">Texture</param>
        /// <param name="on">if it is active by default</param>
        public PanelElement(Texture2D tex, bool on)
        {
            tx = tex;
            isActive = on;
            additionalTex = new List<Texture2D>();
        }

        /// <summary>
        /// Add a Text (string) to the Element at the same Position
        /// </summary>
        /// <param name="font">Font for the Text</param>
        /// <param name="t">Text that should be displayed</param>
        /// <param name="texColor">Color of the Text</param>
        public void AddText(SpriteFont font, string t,Color texColor, float scale = 1.7f)
        {
            Font = font;
            textColor = texColor;
            text = t;
            sizefactor = scale;
        }

        /// <summary>
        /// Adds another Texture, which is drawn over the other ones
        /// </summary>
        /// <param name="tex"></param>
        public void AddTexture(Texture2D tex)
        {
            additionalTex.Add(tex);
        }

        public void Update(GameTime time)
        {
            // Make Updates
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tx, Position, Color.White);

            if (additionalTex.Count > 0)
                foreach (Texture2D at in additionalTex)
                {
                    spriteBatch.Draw(at, Position, Color.White);
                }

            if (Font != null)
                spriteBatch.DrawString(Font, text, Position, textColor, 0, new Vector2(1f,1f), 1.7f, SpriteEffects.None, 1);     
        }

    }
}
