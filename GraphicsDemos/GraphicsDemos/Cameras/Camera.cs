using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GraphicsDemos.Graphics;

namespace GraphicsDemos.Cameras
{
    // Defines a common class for special cameras to derive from
    // Default viewing position is at 0,0,0, facing down the negative Z axis
    public class Camera : IGraphicsObject
    {
        protected Matrix world, view, projection;
        protected Vector3 pos, dir, up;
        protected static Vector3 startUp = Vector3.Up;
        protected static Vector3 startDir = new Vector3(0, 0, -1);
        private float nearPlane, farPlane;

        public Matrix World
        {
            get
            {
                return world;
            }
        }

        public Matrix View
        {
            get
            {
                return view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        public Vector3 Pos
        {
            get
            {
                return pos;
            }
        }

        public Vector3 ViewDir
        {
            get
            {
                return dir;
            }
        }

        public Vector3 Up
        {
            get
            {
                return up;
            }
        }

        public float NearPlane
        {
            get
            {
                return nearPlane;
            }
            set
            {
                nearPlane = value;
            }
        }

        public float FarPlane
        {
            get
            {
                return farPlane;
            }
            set
            {
                farPlane = value;
            }
        }

        public Camera() : this(Vector3.Zero, 45f)
        {

        }

        public Camera(Vector3 pos, float fieldOfView)
        {
            this.pos = pos;
            this.dir = startDir;
            this.up = startUp;
            this.nearPlane = 0.01f;
            this.farPlane = 1000f;

            float aspectRatio = (float)Config.ScreenWidth / Config.ScreenHeight;

            world = Matrix.CreateTranslation(pos);
            view = Matrix.CreateLookAt(pos, pos+dir, up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fieldOfView), aspectRatio, nearPlane, farPlane);
        }

        public virtual void Update(GameTime g)
        {
            //Subclasses are responsible for handling user control
            //and updating their positions accordingly
        }

        public void Draw(GameTime g)
        {
            //Required for the IGraphicsObject interface. Don't actually draw anything
        }
    }
}
