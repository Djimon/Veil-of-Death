using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VeilofDeath.Core.GameStates
{
    class MainMenu : IGameState
    {
        SpriteBatch spriteBatch;
        private Texture2D background;

        public EState newState { get; set; }

        public bool canLeave { get; set; }



        public MainMenu()
        {
            spriteBatch = GameConstants.SpriteBatch;
            Initialize();
            LoadContent();
        }


        public void Initialize()
        {
            newState = EState.none;
        }

        public void LoadContent()
        {
            background = GameConstants.Content.Load<Texture2D>("menuBG");
            //Load other Textures, like Buttons
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime time)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                newState = EState.Ingame;


        }

        public void Draw(GameTime time)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, (int)GameConstants.WINDOWSIZE.X, (int)GameConstants.WINDOWSIZE.Y), Color.White);
            //Draw more 2D stuff here

            spriteBatch.End();
        }
    }
}
