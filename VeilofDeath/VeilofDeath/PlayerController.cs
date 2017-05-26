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
                character.Position.X += 1 * GameConstants.MovingSpeed;
                character.Velocity += new Vector3(1, 0, 0);

            if (currentKeyboardState.IsKeyDown(Keys.Left) && !oldKeyboardState.IsKeyDown(Keys.Left))
                character.Position.X -= 1 * GameConstants.MovingSpeed;
                character.Velocity += new Vector3(-1, 0, 0);

            if (currentKeyboardState.IsKeyDown(Keys.Up) && !oldKeyboardState.IsKeyDown(Keys.Up))
                character.Position.Y+= 1 * GameConstants.MovingSpeed;
            character.Velocity += new Vector3(0, 1, 0);

            if (currentKeyboardState.IsKeyDown(Keys.Down) && !oldKeyboardState.IsKeyDown(Keys.Down))
                character.Position.Y -= 1 * GameConstants.MovingSpeed;
            character.Velocity += new Vector3(0, -1, 0);

            Console.WriteLine(character.Position.ToString());
        }

        //public void Draw(Vector3 cameraPosition, float aspectRatio, Model model, Player character)
        //{
        //    foreach (var mesh in model.Meshes)
        //    {
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {
        //            effect.EnableDefaultLighting();
        //            effect.PreferPerPixelLighting = true;
        //            // We’ll be doing our calculations here...
        //            effect.World = GetWorldMatrix(character);

        //            var cameraLookAtVector = Vector3.Zero;
        //            var cameraUpVector = Vector3.UnitZ;

        //            effect.View = Matrix.CreateLookAt(
        //                cameraPosition, cameraLookAtVector, cameraUpVector);

        //            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
        //            float nearClipPlane = 1;
        //            float farClipPlane = 200;

        //            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
        //                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
        //        }

        //        mesh.Draw();
        //    }
        //}

        public Matrix GetWorldMatrix(Player character)
        {
            Matrix translation = Matrix.CreateTranslation(new Vector3(character.Position.X, character.Position.Y, character.Position.Z));
            return translation;
        }
    }
}
