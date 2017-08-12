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
        bool isOnGround = true;

        Player character;

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        /// <param name="player">Main Player</param>
        public PlayerController(Player player)
        {
            character = player;
        }

        /// <summary>
        /// updates the player movement related to the keyboard state
        /// </summary>
        /// <param name="oldKeyboardState">keyboard state of the predecessor tick</param>
        public void Update (KeyboardState oldKeyboardState, Map map)
        {

            currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Right) && !oldKeyboardState.IsKeyDown(Keys.Right) && !isRightPressed)
            {
                if (CheckFrontIsWalkable(map, "right"))
                {
                    character.Position.X += 1 * GameConstants.iBlockSize;
                    isRightPressed = true;
                }
                
            }
            if (currentKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left) && !isLeftPressed)
            {
                if (CheckFrontIsWalkable(map, "left"))
                {
                    character.Position.X -= 1 * GameConstants.iBlockSize;
                    isLeftPressed = true;
                }
            }
            if (!character.isJumping && !isSpacePressed && currentKeyboardState.IsKeyDown(Keys.Space) && !oldKeyboardState.IsKeyDown(Keys.Space) )
            {
                //character.Position.Z += 1 * GameConstants.iBlockSize;
                Jump(character);
                isSpacePressed = true;
            }

            //if (!character.isSliding && currentKeyboardState.IsKeyDown(Keys.LeftShift) && !oldKeyboardState.IsKeyDown(Keys.LeftShift) && !isDownPressed)
            //{
            //    character.isSliding = true; ;
            //    //TODO: Just Change the Animation and check if the Collision-boxes follow the Animation (= is small enough to pass under a obsticle)
            //    isDownPressed = true;
            //}

            if (isRightPressed && currentKeyboardState.IsKeyUp(Keys.Right))
                isRightPressed = false;
            if (isLeftPressed && currentKeyboardState.IsKeyUp(Keys.Left))
                isLeftPressed = false;
            if (isSpacePressed && currentKeyboardState.IsKeyUp(Keys.Space))
                isSpacePressed = false;
            //if (isDownPressed && currentKeyboardState.IsKeyUp(Keys.LeftShift))
            //    isDownPressed = false;

            if (GameConstants.isDebugMode)
                Console.WriteLine(character.Position.ToString());

            if (character.Position.Z <= 0.2f && !isOnGround)
                character.AniModel.BlendToAnimationPart("Run");

            if (character.Position.Z <= 0.2f && !character.isJumping)
                isOnGround = true;

            if (isSpacePressed)
                character.AniModel.BlendToAnimationPart("Idle");

        }


        private bool CheckFrontIsWalkable(Map map, string direction)
        {

            int GridX = (int)(character.Position.X - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize;
            int GridY = (int)(character.Position.Y - (GameConstants.iBlockSize / 2)) / GameConstants.iBlockSize;
            Vector2 GridPos = new Vector2(GridX, GridY);
            //Console.WriteLine("GridPos:" + GridPos);
            //Console.WriteLine("MapPos:" + map.map[GridX, GridY].position + " - " + map.map[GridX, GridY].isWalkable);
            try
            {
                switch (direction)
                {
                    case "left":
                        if (!map.map[GridX - 1, GridY].isWalkable)
                            return false;
                        break;
                    case "right":
                        if (!map.map[GridX + 1, GridY].isWalkable)
                            return false;
                        break;
                    default:
                        return true;
                        break;
                }
            } catch(IndexOutOfRangeException e)
            {
                Console.WriteLine("Indx Out of Range");
                GameConstants.currentGame.Exit();
            }
            
            return true;
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
            isOnGround = false;
        }

        /// <summary>
        /// calculates the factor for the S-form of the square function
        /// </summary>
        /// <param name="mid">S-point</param>
        /// <param name="posX">another point (actual Y-Position in 3D)</param>
        /// <returns></returns>
        private float calculateFactor(Vector2 mid, float posX)
        {
            Vector2 landing = new Vector2(posX, 0);
            return (landing.Y - mid.Y) / ((landing.X - mid.X)* (landing.X - mid.X));
        }
    }
}
