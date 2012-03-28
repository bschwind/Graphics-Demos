using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GraphicsToolkit.Graphics;
using GraphicsToolkit.Graphics.SceneGraph;

namespace GraphicsDemos.Demos
{
    public class DeferredDemo : Demo
    {
        private PrimitiveBatch primBatch;
        private FirstPersonCamera cam;
        
        DeferredRenderer renderer;
        List<Mesh> meshes = new List<Mesh>();

        public override void LoadContent(ContentManager content, GraphicsDevice g)
        {
            base.LoadContent(content, g);

            primBatch = new PrimitiveBatch(Device);
            cam = new FirstPersonCamera(0.5f, 10);
            cam.Pos = new Vector3(3, 3, 13);

            renderer = new DeferredRenderer(g, content);

            MeshBuilder mb = new MeshBuilder(g);
            Mesh box = mb.CreateBox(1, 1, 1);
            mb.Begin();
            mb.AddQuad(new Vector3(-100, 0, -100), new Vector3(100, 0, -100), new Vector3(100, 0, 100), new Vector3(-100, 0, 100), false, Vector2.Zero, Vector2.One);
            Mesh floor = mb.End();
            box.Texture = content.Load<Texture2D>("Cube");
            floor.Texture = content.Load<Texture2D>("Cube");
            meshes.Add(box);
            meshes.Add(floor);
        }
        public override void Update(GameTime g)
        {
            cam.Update(g);

            float dt = (float)g.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime g)
        {
            Device.Clear(Color.Black);

            /*primBatch.Begin(PrimitiveType.LineList, cam);

            primBatch.DrawXZGrid(10, 10, Color.Blue);
            primBatch.DrawXYGrid(10, 10, Color.Red);
            primBatch.DrawYZGrid(10, 10, Color.Green);

            primBatch.End();*/

            renderer.Render(meshes, cam);
        }
    }
}
