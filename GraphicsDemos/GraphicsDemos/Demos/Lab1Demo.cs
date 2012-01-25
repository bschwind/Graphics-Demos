using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;

namespace GraphicsDemos.Demos
{
    public class Lab1Demo : Demo
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
            cam.Pos = new Vector3(0, 2, 10);
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
            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            primBatch.End();

            //Draw a hollow cube with different colors
            primBatch.Begin(PrimitiveType.TriangleList, cam);
            primBatch.FillQuad(new Vector3(-1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1), true, Color.Red);
            primBatch.FillQuad(new Vector3(-1, 1, -1), new Vector3(-1, 1, 1), new Vector3(-1, -1, 1), new Vector3(-1, -1, -1), true, Color.Blue);
            primBatch.FillQuad(new Vector3(1, 1, -1), new Vector3(-1, 1, -1), new Vector3(-1, -1, -1), new Vector3(1, -1, -1), true, Color.Green);
            primBatch.FillQuad(new Vector3(1, 1, 1), new Vector3(1, 1, -1), new Vector3(1, -1, -1), new Vector3(1, -1, 1), true, Color.Yellow);
            primBatch.End();
        }
    }
}
