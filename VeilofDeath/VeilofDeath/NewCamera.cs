using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
     public class NewCamera
    {
        public static Vector2 WINDOWSIZE = new Vector2(1200, 700);

        public Matrix World { get => world; set => world = value; }
        public Matrix View { get => view; set => view = value; }
        public Matrix Projection { get => projection; set => projection = value; }

        public Vector3 UP = Vector3.UnitZ;

        //Matrices
        Matrix world;
        Matrix view;
        Matrix projection;

        //Vectors
        Vector3 camPos;
        Vector3 camTarget;
        //Vector3 camUp;

        private static NewCamera instance;

        public static NewCamera Instance
        {
            get
            {
                if (instance == null)
                    instance = new NewCamera();
                return instance;
            }

            set
            {
                instance = value;
            }

        }

        NewCamera()
        {
            InitializeCameraVectors();
            InitializeCameraMatrices();
        }

        void InitializeCameraMatrices()
        {
            world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            view = Matrix.CreateLookAt(camPos, camTarget, UP);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), WINDOWSIZE.X / WINDOWSIZE.Y, 0.1f, 150f);
        }

        void InitializeCameraVectors()
        {
            ResetCamera();
        }


        public void ResetCamera()
        {
            camPos = new Vector3(0, 40, 20);
            ResetRotation();
        }

        public void ResetRotation()
        {
            camTarget = new Vector3(0, 0, 0);
        }
    }
}
