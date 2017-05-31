using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    public class Camera
    {
        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        public Vector3 position = new Vector3(15, 10, 10);
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime,Player P)
        {
            Vector3 campos = new Vector3(0, 0.1f, 0.6f);
            Vector3 camup = new Vector3(0, 1, 0);
            

            ViewMatrix = Matrix.CreateLookAt(campos, P.Position, camup);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, 
                                                                    GameConstants.fAspectRatio,
                                                                    GameConstants.NearClipPlane, 
                                                                    GameConstants.FarClipPlane );
        }

        /// <summary>
        /// Sets the Viewmatrix in dependence of the player
        /// </summary>
        /// <param name="P">Player</param>
        /// <returns>ViewMatrix</returns>
        public Matrix SetView(Player P)
        {
            var lookAtVector = Vector3.Zero;
            var upVector = Vector3.UnitZ;

            ViewMatrix = Matrix.CreateLookAt(position, P.Position, upVector);
            return ViewMatrix;
        }

        /// <summary>
        /// sets upthe camera projection matrix
        /// </summary>
        /// <returns>ProjectionMatrix</returns>
        public Matrix SetProjectionsMatrix()
        {
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = GameConstants.NearClipPlane;
            float farClipPlane = GameConstants.FarClipPlane;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, GameConstants.fAspectRatio, nearClipPlane, farClipPlane);
            return ProjectionMatrix;
        }     

    }
}
