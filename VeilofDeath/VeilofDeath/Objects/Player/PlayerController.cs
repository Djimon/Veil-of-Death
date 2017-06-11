using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VeilofDeath
{
    class PlayerController
    {

        private KeyboardState currentKeyboardState;
        bool isSpacePressed = false;
        bool isDownPressed = false;
        bool isRightPressed = false;
        bool isLeftPressed = false;

        bool isLanded =false ;
        Player character;
       // private Player character;

        public PlayerController(Player player)
        {
            character = player;
        }

        public void Update (KeyboardState oldKeyboardState)
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
            if (isLanded && !isSpacePressed && currentKeyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space) )
            {
                //character.Position.Z += 1 * GameConstants.iBlockSize;
                Jump(character);
                isSpacePressed = true;
            }
            //if (currentKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down) && !isDownPressed)
            //{
            //    character.Position.Y -= 1 * GameConstants.iBlockSize;
            //    isDownPressed = true;
            //}

            if (isRightPressed && currentKeyboardState.IsKeyUp(Keys.Right))
                isRightPressed = false;
            if (isLeftPressed && currentKeyboardState.IsKeyUp(Keys.Left))
                isLeftPressed = false;
            if (isSpacePressed && currentKeyboardState.IsKeyUp(Keys.Space))
                isSpacePressed = false;
            //if (isDownPressed && currentKeyboardState.IsKeyUp(Keys.Down))
            //    isDownPressed = false;


            Console.WriteLine(character.Position.ToString());
        }

        void Jump(Player character)
        {
            Console.WriteLine("Jump");
        }

 

    }
}
