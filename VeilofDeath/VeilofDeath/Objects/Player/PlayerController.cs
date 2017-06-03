using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        bool isUpPressed = false;
        bool isDownPressed = false;
        bool isRightPressed = false;
        bool isLeftPressed = false;
       // private Player character;

        public PlayerController()
        {
            //TODO: Tastatur oder GamePad
        }

        public void Update (KeyboardState oldKeyboardState, Player character)
        {
            currentKeyboardState = Keyboard.GetState();


            if (currentKeyboardState.IsKeyDown(Keys.Right) && !oldKeyboardState.IsKeyDown(Keys.Right) && !isRightPressed)
            {
                character.Position.X += 1 * GameConstants.iBlockSize;
                isRightPressed = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left) && !isLeftPressed)
            {
                character.Position.X -= 1 * GameConstants.iBlockSize;
                isLeftPressed = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up) && !isUpPressed)
            {
                character.Position.Y += 1 * GameConstants.iBlockSize;
                isUpPressed = true;
            }
            if (currentKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down) && !isDownPressed)
            {
                character.Position.Y -= 1 * GameConstants.iBlockSize;
                isDownPressed = true;
            }

            if (isRightPressed && currentKeyboardState.IsKeyUp(Keys.Right))
                isRightPressed = false;
            if (isLeftPressed && currentKeyboardState.IsKeyUp(Keys.Left))
                isLeftPressed = false;
            if (isUpPressed && currentKeyboardState.IsKeyUp(Keys.Up))
                isUpPressed = false;
            if (isDownPressed && currentKeyboardState.IsKeyUp(Keys.Down))
                isDownPressed = false;
       

            Console.WriteLine(character.Position.ToString());
        }

    }
}
