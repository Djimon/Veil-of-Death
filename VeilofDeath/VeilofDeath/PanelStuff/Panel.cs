using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath.PanelStuff
{
    class Panel
    {
        /*
         Base-PanelStuff bekommt bestimmte Position. alle anderen Elemente kreigen eine Position
         relativ um basePanel

         */
        
        // BASE PANEL
        public Vector2 Position;
        private float X, Y;
        private Texture2D TX;

        //CHILD PANNELS
        private List<PanelElement> children;

        /// <summary>
        /// Is a 2D structure which can hold many Panel Elements
        /// </summary>
        /// <param name="tex">Texture of Panel</param>
        /// <param name="pos">Location on Screen</param>
        public Panel(Texture2D tex, Vector2 pos)
        {
            Position = pos;
            TX = tex;
            X = TX.Width;
            Y = TX.Height;
            children = new List<PanelElement>();
        }

        /// <summary>
        /// Adds a PanelStuff Element at the given relative position
        /// </summary>
        /// <param name="p">PanelStuff Element to add</param>
        /// <param name="relPos">relative position in float (X,Y)-Vector2 
        /// where (0,0) means top left and (1,1) means bottom right</param>
        /// <returns></returns>
        public int Add(PanelElement p, Vector2 relPos)
        {
            PanelElement PE = p;
            PE.Position = new Vector2(relPos.X * X + Position.X, relPos.Y * Y + Position.Y);
            PE.Parent = this;
            children.Add(PE);

            if (GameConstants.isDebugMode)
                Console.WriteLine("added "+PE.ToString() +" "+ PE.Position);

            return children.Count;
        }

        public void Update(GameTime time)
        {

            //foreach (PanelElement p in children)
            //{
            //    if (p.isAnimated)
            //        p.Update(time);
            //}

            // global panel update rutine

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TX, Position, Color.White);

            foreach (PanelElement p in children)
            {
                if (p.isActive)
                    p.Draw(spriteBatch);
            }
        }




    }
}
