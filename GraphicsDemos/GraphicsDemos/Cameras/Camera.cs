using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GraphicsDemos.Graphics;

namespace GraphicsDemos.Cameras
{
    public class Camera : IGraphicsObject
    {
        protected Matrix world, view, projection;
        protected Vector3 pos, target;
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

        public Vector3 Target
        {
            get
            {
                return pos;
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

        public Camera() : this(Vector3.Zero, new Vector3(0,0,-1f), 45)
        {

        }

        public Camera(Vector3 pos, Vector3 target, float fieldOfView)
        {
            this.pos = pos;
            this.target = target;
            this.nearPlane = 0.1f;
            this.farPlane = 1000f;

            float aspectRatio = (float)Config.ScreenWidth / Config.ScreenHeight;

            world = Matrix.CreateTranslation(pos);
            view = Matrix.CreateLookAt(pos, target, Vector3.Up);
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
