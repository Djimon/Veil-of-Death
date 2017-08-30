using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeilofDeath.Objects.PlayerStuff;

namespace VeilofDeath.Core
{
     public class Camera
    {

        #region different Matrices

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

        #endregion

        #region member, variables

        Player target;
        bool isTargetSet = false;

        //Matrices
        Matrix x_world;
        Matrix x_view;
        Matrix x_projection;

        //Vectors
        public Vector3 camPos { get; private set; }
        Vector3 lastPos;
        Vector3 camTarget;
        Vector3 offset = new Vector3(-2f, 0f, GameConstants.fCameraHeight);
        public Vector3 UP = Vector3.UnitZ;
        //Vector3 camUp;

        #endregion

        #region Constructor, Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        public Camera()
        {
            InitializeCameraVectors();
            InitializeCameraMatrices();
        }

        /// <summary>
        /// initializes the matrices for the camera
        /// </summary>
        void InitializeCameraMatrices()
        {
            x_world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            x_view = Matrix.CreateLookAt(camPos, new Vector3(GameConstants.fLaneCenter, 0, 0), UP);
            x_projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(GameConstants.CameraAngle),
                GameConstants.fAspectRatio,
                GameConstants.fNearClipPlane,
                GameConstants.fFarClipPlane);

        }

        /// <summary>
        /// initializes the camera for usage
        /// </summary>
        void InitializeCameraVectors()
        {
            ResetCamera();
        }

        #endregion

        #region reset functions

        /// <summary>
        /// resets the camera to the start position
        /// </summary>
        public void ResetCamera()
        {
            if (GameConstants.isDebugMode)
                Console.WriteLine("ResetCam");
            camPos = new Vector3(GameConstants.fLaneCenter, GameConstants.CameraDistance, GameConstants.fCameraHeight); // x:= bloksize *3 <= *6 Lanes and :2 (center) 
            lastPos = camPos;
            ResetRotation();
        }

        /// <summary>
        /// hard reset of the rotation to the vector (0, 0, 0)
        /// </summary>
        public void ResetRotation()
        {
            camTarget = new Vector3(0, 0, 0);
        }

        #endregion

        /// <summary>
        /// Updates the camera when a target is attached
        /// </summary>
        /// <param name="time">global gametime</param>
        public void Update(GameTime time)
        {
            if (!isTargetSet) return;

            camTarget = target.Position ; 
            camPos += new Vector3(0, target.Velocity.Y ,0);

            Vector3 targetVector = new Vector3(GameConstants.fLaneCenter, target.Position.Y, 0f);
            x_view = Matrix.CreateLookAt(camPos,targetVector,UP );

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
    }
}
