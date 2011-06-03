using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GraphicsDemos.Cameras
{
    //Implements a first person camera that can look up and down, left and right,
    //and move in the direction it is facing
    public class UprightFirstPersonCamera : Camera
    {
        private int lastMouseX, lastMouseY;
        private float xRot, yRot;
        private Matrix rotation;
        private Vector3 left;
        private Vector3 target;
        private float turnSpeed;
        private float moveSpeed;

        public UprightFirstPersonCamera(float turnSpeed, float moveSpeed)
            : base()
        {
            if (turnSpeed < 0.1f || turnSpeed > 1.0f)
            {
                throw new ArgumentOutOfRangeException("turnSpeed", "Turn speed must be between 0.1 and 1.0");
            }

            if (moveSpeed < 0f)
            {
                throw new ArgumentOutOfRangeException("moveSpeed", "Move speed must be greater than 0");
            }

            this.turnSpeed = turnSpeed;
            this.moveSpeed = moveSpeed;

            //Set the last mouse pos equal to the center of the screen
            //and center our current mouse position
            lastMouseX = Config.ScreenWidth / 2;
            lastMouseY = Config.ScreenHeight / 2;
            Mouse.SetPosition(lastMouseX, lastMouseY);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime g)
        {
            base.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;
            MouseState mState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            Mouse.SetPosition(Config.ScreenWidth / 2, Config.ScreenHeight / 2);

            //Update orienation according to mouse movement
            int currentX = mState.X;
            int currentY = mState.Y;
            int dx = currentX - lastMouseX;
            int dy = currentY - lastMouseY;

            xRot -= dy * turnSpeed * dt;
            xRot = MathHelper.Clamp(xRot, -MathHelper.PiOver2, MathHelper.PiOver2);
            yRot -= dx * turnSpeed * dt;
            yRot = Math.Sign(yRot) * Math.Abs(yRot % MathHelper.TwoPi);
            rotation = Matrix.CreateRotationX(xRot) * Matrix.CreateRotationY(yRot);
            Vector3.TransformNormal(ref startDir, ref rotation, out dir);
            Vector3.TransformNormal(ref startUp, ref rotation, out up);
            Vector3.Cross(ref up, ref dir, out left);

            //Update position according to WASD input
            float forwardMovement = 0f;
            float sideMovement = 0f;

            if (keyState.IsKeyDown(Keys.W))
            {
                forwardMovement = dt;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                forwardMovement = -dt;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                sideMovement = dt;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                sideMovement = -dt;
            }

            pos += dir * forwardMovement * moveSpeed + left * sideMovement * moveSpeed;
            target = pos + dir;

            //Update view with our new info
            Matrix.CreateLookAt(ref pos, ref target, ref up, out view);
        }
    }
}
