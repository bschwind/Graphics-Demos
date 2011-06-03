using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsDemos.Cameras;

namespace GraphicsDemos.Demos
{
    public class VectorsDemo : Demo
    {
        private UprightFirstPersonCamera cam;
        private PrimitiveBatch primBatch;

        public override void LoadContent(ContentManager c, GraphicsDevice g)
        {
            base.LoadContent(c, g);

            cam = new UprightFirstPersonCamera(0.5f, 5f);
            primBatch = new PrimitiveBatch(g);
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
            primBatch.End();
        }
    }
}
