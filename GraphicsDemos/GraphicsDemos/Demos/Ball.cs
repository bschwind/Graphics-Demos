using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsDemos.Demos
{
    public class Ball
    {
        public Vector3 Pos;
        public Vector3 Vel;
        public float Radius;

        public Ball(Vector3 pos, Vector3 vel, float radius)
        {
            Pos = pos;
            Vel = vel;
            Radius = radius;
        }
    }
}
