using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GraphicsDemos.Cameras;
using GraphicsDemos.Graphics;

namespace GraphicsDemos.Demos
{
    public delegate float SurfaceFunction(float x, float y);

    public class SurfaceDemo : Demo
    {
        private FirstPersonCamera cam;
        private PrimitiveBatch primBatch;
        private float totalSeconds;

        public override void LoadContent(ContentManager c, GraphicsDevice g)
        {
            base.LoadContent(c, g);

            cam = new FirstPersonCamera(0.5f, 5f);
            cam.Pos = new Vector3(5, 1, 10);
            primBatch = new PrimitiveBatch(g);
            primBatch.SmoothShadingEnabled = true;
        }

        public override void Update(GameTime g)
        {
            cam.Update(g);
            totalSeconds = (float)g.TotalGameTime.TotalSeconds;
        }

        private float surfaceFunction(float x, float y)
        {
            //return (float)(Math.Cos(x+totalSeconds * 6) + Math.Cos(y+ totalSeconds * 6));
            return (float)Math.Cos(new Vector2(x, y).Length()  * (totalSeconds)) * 0.5f;
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            primBatch.Begin(PrimitiveType.TriangleList, cam);
            fillSurface(new Vector2(-2, -2), new Vector2(2, 2), 100, surfaceFunction);
            primBatch.End();
     
            /*primBatch.Begin(PrimitiveType.LineList, cam);
            primBatch.DrawXZGrid(10, 10, Color.Red);
            primBatch.End();*/
        }

        private void fillSurface(Vector2 startBounds, Vector2 endBounds, int iterationsPerDim, SurfaceFunction func)
        {
            float deltaX = Math.Abs(endBounds.X - startBounds.X) / iterationsPerDim;
            float deltaZ = Math.Abs(endBounds.Y - startBounds.Y) / iterationsPerDim;

            float xVal = 0;
            float zVal = 0;

            Vector3 v1, v2, v3, v4;

            for (int x = 0; x < iterationsPerDim; x++)
            {
                for (int z = 0; z < iterationsPerDim; z++)
                {
                    xVal = startBounds.X + (deltaX * x);
                    zVal = startBounds.Y + (deltaZ * z);
                    v1 = new Vector3(xVal, func(xVal, zVal), zVal);

                    xVal = startBounds.X + (deltaX * (x+1));
                    v2 = new Vector3(xVal, func(xVal, zVal), zVal);

                    zVal = startBounds.Y + (deltaZ * (z+1));
                    v3 = new Vector3(xVal, func(xVal, zVal), zVal);

                    xVal = startBounds.X + (deltaX * x);
                    v4 = new Vector3(xVal, func(xVal, zVal), zVal);

                    primBatch.FillQuad(v1, v2, v3, v4, Color.Red);
                }
            }
        }
    }
}
