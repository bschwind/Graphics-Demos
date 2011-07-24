using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsDemos.Cameras;
using GraphicsDemos.Graphics;

namespace GraphicsDemos.Demos
{
    public class VectorsDemo : Demo
    {
        private FirstPersonCamera cam;
        private PrimitiveBatch primBatch;
        //Vectors for our triangle
        private Vector3 a, b, c, n, triangleCenter;

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            //Set up our camera and primitive renderer
            cam = new FirstPersonCamera(0.5f, 5f);
            cam.Pos = new Vector3(5, 1, 10);
            primBatch = new PrimitiveBatch(g);


            //Set up our triangle
            a = new Vector3(4, 1, 5);
            b = new Vector3(6, 1.5f, 3);
            c = new Vector3(6, 1, 5);

            triangleCenter = (a + b + c) / 3f;

            n = Vector3.Normalize(Vector3.Cross(c - a, b - a));
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);
            primBatch.Begin(PrimitiveType.LineList, cam);
            primBatch.DrawXZGrid(10, 10, Color.LightBlue);

            //Draw the triangle and its normal
            primBatch.DrawTriangle(a, b, c, Color.Blue);
            primBatch.DrawLine(triangleCenter, triangleCenter + n, Color.Orange);

            //Draw our coordinate axes for fun
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(1, 1, 0), Color.Red);
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 2, 0), Color.Green);
            primBatch.DrawLine(new Vector3(0, 1, 0), new Vector3(0, 1, 1), Color.Blue);

            primBatch.End();
        }
    }
}
