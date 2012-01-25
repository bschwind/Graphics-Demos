using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GraphicsToolkit;

namespace GraphicsDemos.Demos
{
    public class TriangleDemo : Demo
    {
        Matrix view, projection;
        BasicEffect effect;
        VertexPositionColor[] verts = new VertexPositionColor[3];

        public override void LoadContent(ContentManager c, GraphicsDevice g)
        {
            base.LoadContent(c, g);

            verts[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue);
            verts[1] = new VertexPositionColor(new Vector3(0, 1, 0), Color.Red);
            verts[2] = new VertexPositionColor(new Vector3(1, 0, 0), Color.Blue);

            view = Matrix.CreateLookAt(new Vector3(0, 1, 5), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), (float)Config.ScreenWidth / Config.ScreenHeight, 0.1f, 1000f);

            effect = new BasicEffect(g);
            effect.VertexColorEnabled = true;
        }

        public override void Update(GameTime g)
        {

        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            effect.View = view;
            effect.Projection = projection;

            effect.CurrentTechnique.Passes[0].Apply();
            Device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, verts, 0, 1);
        }
    }
}
