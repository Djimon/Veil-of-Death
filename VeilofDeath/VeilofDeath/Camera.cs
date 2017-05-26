using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
    class Camera
    {
        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        Vector3 position = new Vector3(15, 10, 10);
        public Matrix ProjectionMatrix;
        public Matrix ViewMatrix;

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Update(GameTime gameTime,Player P)
        {
            position = Vector3.Transform(position, Matrix.CreateFromQuaternion(P.Rotation));
            //position += P.Position;
            SetView(P);
        }
        
        public Matrix SetView(Player P)
        {
            var lookAtVector = Vector3.Zero;
            var upVector = Vector3.UnitZ;
            upVector = Vector3.Transform(upVector, Matrix.CreateFromQuaternion(P.Rotation));

            ViewMatrix = Matrix.CreateLookAt(position, P.Position, upVector);
            return ViewMatrix;
        }        
        
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
