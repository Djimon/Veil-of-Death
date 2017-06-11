using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeilofDeath
{
     public class Camera
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
        float CameraMidPoint;
        //Vector3 camUp;


        /// <summary>
        /// Constructor
        /// </summary>
        public Camera()
        {
            InitializeCameraVectors();
            InitializeCameraMatrices();
            CameraMidPoint = GameConstants.fLaneCenter - (GameConstants.iBlockSize / 2);
        }

        void InitializeCameraMatrices()
        {
            x_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            x_view = Matrix.CreateLookAt(camPos,new Vector3(GameConstants.fLaneCenter,0,0), UP);
            x_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(50), 
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
            camPos += new Vector3(0, target.Velocity.Y ,0); //TODO: get rid of Magic numbers

            Vector3 targetVector = new Vector3(GameConstants.fLaneCenter, target.Position.Y, 0f);
            x_view = Matrix.CreateLookAt(camPos, 
                                        targetVector /*- new Vector3(0f,0f, GameConstants.fCameraHeight)*/,
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
            Console.WriteLine("ResetCam");
            camPos = new Vector3(GameConstants.fLaneCenter, -20, 10); // x:= bloksize *3 <= *6 Lanes and :2 (center) 
            lastPos = camPos;
            ResetRotation();
        }

        public void ResetRotation()
        {
            camTarget = new Vector3(0, 0, 0);
        }
    }
}
