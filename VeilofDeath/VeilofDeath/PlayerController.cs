using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    class PlayerController
    {

        private KeyboardState currentKeyboardState;
       // private Player character;

        public PlayerController()
        {

        }

        //public PlayerController(Player character)
        //{
        //    this.character = character;
        //}

        public void Update (KeyboardState oldKeyboardState, Player character)
        {
            currentKeyboardState = Keyboard.GetState();

            //if (currentKeyboardState.IsKeyDown(Keys.Escape))
            //Exit();

            if (currentKeyboardState.IsKeyDown(Keys.Right) && !oldKeyboardState.IsKeyDown(Keys.Right))
                character.Position.X += 10;

            if (currentKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left))
                character.Position.X -= 10;

            if (currentKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up))
                character.Position.Y+= 10;

            if (currentKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down))
                character.Position.Y -= 10;

            Console.WriteLine(character.Position.ToString());
        }
    }
}
