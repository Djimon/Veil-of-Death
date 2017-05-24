using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeilofDeath
{
    class Level
    {

        public Level()
        {
        }

        VertexPositionTexture[] x_groundPlane;
        BasicEffect effect;
        GraphicsDevice device;
        GraphicsDeviceManager graphicsManager;

        public void Initialize(GraphicsDevice device)
        {
            x_groundPlane = new VertexPositionTexture[18];

            x_groundPlane[0].Position = new Vector3( -10, -10, 0);
            x_groundPlane[1].Position = new Vector3( -10, 10, 0);
            x_groundPlane[2].Position = new Vector3( 10, -10, 0);
            x_groundPlane[3].Position = x_groundPlane [1].Position;
            x_groundPlane[4].Position = new Vector3( 10, 10, 0);
            x_groundPlane[5].Position = x_groundPlane[2].Position;
            x_groundPlane[6].Position = x_groundPlane[0].Position;
            x_groundPlane[7].Position = new Vector3(-10, -10, 5);
            x_groundPlane[8].Position = x_groundPlane[1].Position;
            x_groundPlane[9].Position = x_groundPlane[1].Position;
            x_groundPlane[10].Position = x_groundPlane[7].Position;
            x_groundPlane[11].Position = new Vector3(-10, 10, 5);
            x_groundPlane[12].Position = x_groundPlane[2].Position;
            x_groundPlane[13].Position = x_groundPlane[4].Position;
            x_groundPlane[14].Position = new Vector3(10, -10, 5);
            x_groundPlane[15].Position = x_groundPlane[4].Position;
            x_groundPlane[16].Position = new Vector3(10, 10, 5);
            x_groundPlane[17].Position = x_groundPlane[14].Position;


            effect = new BasicEffect(device);
        }

        public void DrawGround(GraphicsDevice device)
        {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(0, 40, 20);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);
             

            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, GameConstants.fAspectRatio, nearClipPlane, farClipPlane);


            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    x_groundPlane,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    6);
            }
        }

    }
}
