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
            if (!character.isJumping && !isSpacePressed && currentKeyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space) )
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

            if (GameConstants.isDebugMode)
                Console.WriteLine(character.Position.ToString());
        }


        /// <summary>
        /// <p>calculates the mid point and endpoint of the jump</p>
        /// <p>and triggers the jumping to the Player</p>
        /// </summary>
        /// <param name="character">the Player</param>
        void Jump(Player character)
        {
            if (GameConstants.isDebugMode)
                Console.WriteLine("Jump");
            float fjumpEndPositionY = character.Position.Y + GameConstants.fJumpWidth;
            Vector2 jumpMid = new Vector2(character.Position.Y + GameConstants.fJumpWidth / 2, GameConstants.fJumpHeight);
            float m = calculateFactor(jumpMid,character.Position.Y);

            if (GameConstants.isDebugMode)
                Console.WriteLine("Jump ends at " + character.Position.X+":"+fjumpEndPositionY);
            character.SetJumpingCurve(jumpMid,m, fjumpEndPositionY);
            character.isJumping = true;

        }

        private float calculateFactor(Vector2 mid, float height)
        {
            Vector2 landing = new Vector2(height, 0);
            return (landing.Y - mid.Y) / ((landing.X - mid.X)* (landing.X - mid.X));
        }
    }
}
