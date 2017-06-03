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
        
        public Matrix X_World
        {
            get
            {
                return x_world;
            }
            set
            {
                x_world = value;
            }
        }

        public Matrix X_View
        {
            get
            {
                return x_view;
            }
            set
            {
                x_view = value;
            }
        }
        public Matrix X_Projection
        {
            get
            {
                return x_projection;
            }
            set
            {
                x_projection = value;
            }
        }

        Player target;
        bool isTargetSet = false;

        //Matrices
        Matrix x_world;
        Matrix x_view;
        Matrix x_projection;

        //Vectors
        Vector3 camPos;
        Vector3 lastPos;
        Vector3 camTarget;
        Vector3 offset = new Vector3(-2f, 0f, GameConstants.fCameraHeight);
        public Vector3 UP = Vector3.UnitZ;
        //Vector3 camUp;

        private static NewCamera instance;

        /// <summary>
        /// returns the only instance of the camere (singlton)
        /// </summary>
        public static NewCamera Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NewCamera();
                }
                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NewCamera()
        {
            InitializeCameraVectors();
            InitializeCameraMatrices();
        }

        void InitializeCameraMatrices()
        {
            x_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            x_view = Matrix.CreateLookAt(camPos, camTarget, UP);
            x_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 
                                                            GameConstants.fAspectRatio, 
                                                            GameConstants.fNearClipPlane, 
                                                            GameConstants.fFarClipPlane); 
        }

        /// <summary>
        /// Updates the camera when a target is attached
        /// </summary>
        /// <param name="time">global gametime</param>
        public void Update(GameTime time)
        {
            if (!isTargetSet) return;

            //TODO: fix (Die Kamera soll im prinzip nur mit der Y-Achse des Players bewegt werden
            camTarget = target.Position ; 
            camPos = lastPos + (camTarget - lastPos) * 0.013f * (float)time.TotalGameTime.TotalSeconds; //TODO: get rid of Magic numbers
            lastPos = camPos;

            x_view = Matrix.CreateLookAt(camPos, 
                                        camPos /*- new Vector3(0f,0f, GameConstants.fCameraHeight)*/,
                                        UP );
        }

        /// <summary>
        /// attaches an target to the camera
        /// </summary>
        /// <param name="actor">the new targer</param>
        public void SetTarget(Player actor)
        {
            target = actor;
            isTargetSet = true;
        }

        /// <summary>
        /// deletes the target from the camera
        /// </summary>
        public void UnsetTarget()
        {
            target = null;
            isTargetSet = false;
        }


        void InitializeCameraVectors()
        {
            ResetCamera();
        }

        /// <summary>
        /// resets the camera to the start position
        /// </summary>
        public void ResetCamera()
        {
            camPos = new Vector3(0, -20, 20); // *3 <= *6 Lanes and :2 (center) 
            lastPos = camPos;
            ResetRotation();
        }

        public void ResetRotation()
        {
            camTarget = new Vector3(0, 0, 0);
        }
    }
}
